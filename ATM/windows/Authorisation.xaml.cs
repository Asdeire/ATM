using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Net.NetworkInformation;
using ATM.messages;
using ATM.windows;
using System.IO;

namespace ATM
{
    /// <summary>
    /// Логика взаимодействия для Authorisation.xaml
    /// </summary>
    public partial class Authorisation : Window
    {

        public Authorisation()
        {
            InitializeComponent();
            string directory = Directory.GetCurrentDirectory();
            Singleton.Instance.connection = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + directory + @"\cards.accdb;Persist Security Info=True";           
        }


        private void ConBtn_Click(object sender, RoutedEventArgs e) 
        {            
            string Connection = Singleton.Instance.connection;

            try
            {
                string cardNum = CardNumberTxtBox.Text;
                

                using (OleDbConnection connection = new OleDbConnection(Connection)) // перевіряє правильність пін-коду
                {
                    connection.Open();

                    string query = "SELECT * FROM Card WHERE Number= @number";
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.AddWithValue("@number", cardNum);

                    OleDbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) 
                    {

                        PincodeWindow pincodeWindow = new PincodeWindow(cardNum);
                        pincodeWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        CardNumberTxtBox.Text = "";
                        Alert alert = new Alert("Такої карти не існує");
                        alert.Show();
                    }
                }
            }
            catch 
            { 
                Alert alert = new Alert("Помилка даних");
                alert.Show();
            }
            
        }
    
        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender; // зчитує текст із клавіші
            CardNumberTxtBox.Text += button.Content.ToString();

        }               

        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CardNumberTxtBox.Text.Length > 0)
            {
                CardNumberTxtBox.Text = CardNumberTxtBox.Text.Substring(0, CardNumberTxtBox.Text.Length - 1);
            }
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminAuthorisation adminAuthorisation = new AdminAuthorisation();
            adminAuthorisation.Show();
            this.Close();
        }

       
    }        
}
