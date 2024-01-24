using ATM.messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection.PortableExecutable;
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
using BCrypt.Net;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ATM
{
    /// <summary>
    /// Логика взаимодействия для PincodeWindow.xaml
    /// </summary>
    public partial class PincodeWindow : Window
    {
        
        public PincodeWindow(string num)
        {           
            InitializeComponent();
            PinPswrdBox.IsEnabled = false;
            CardNumber2TxtBox.IsEnabled = false;
            CardNumber2TxtBox.Text += num;
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            string Connection = Singleton.Instance.connection;
            string query = "SELECT Pincode FROM Card WHERE Number = @number";
            string number = CardNumber2TxtBox.Text;
            string pincode = PinPswrdBox.Password;
            string pinFromDatabase = null;

            using (OleDbConnection connection = new OleDbConnection(Connection))
            {
                connection.Open();
                
                
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@number", number);

                OleDbDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    pinFromDatabase = reader["Pincode"].ToString();
                }


                bool isMatch = BCryptNet.Verify(pincode, pinFromDatabase);

                if (isMatch)
                {
                    Singleton.Instance.cardNumber = number;

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Succes succes = new Succes("Авторизація успішна");
                    succes.Show();
                    this.Close();
                }
                else
                {
                    Alert alert = new Alert("Пінкод невірний");
                    alert.Show();
                    PinPswrdBox.Password = "";
                }
            }            
        }
        


               
           
        

        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button= (Button)sender;
            PinPswrdBox.Password += button.Content;

        }

        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PinPswrdBox.Password.Length>0)
            {
                PinPswrdBox.Password = PinPswrdBox.Password.Substring(0, PinPswrdBox.Password.Length - 1);
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Authorisation authorisation = new Authorisation();
            authorisation.Show();
            this.Close();
        }
    }
}
