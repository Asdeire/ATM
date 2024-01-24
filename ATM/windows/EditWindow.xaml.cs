using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bc = BCrypt.Net.BCrypt;

namespace ATM.windows
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public string cardNum = "";
        public string Connection = Singleton.Instance.connection;

        public EditWindow()
        {
            InitializeComponent();

            using (OleDbConnection connection = new OleDbConnection(Connection)) 
            {
                connection.Open();
                string query = "SELECT Number FROM Card";
                OleDbCommand oleDbCommand = new OleDbCommand(query, connection);
                OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
                while (oleDbDataReader.Read())
                {
                    string cardNumber = oleDbDataReader.GetString(0);
                    ChooseNumTypeBox.Items.Add(cardNumber); // вибір карти
                }
            }
        } 

        private void ChooseNumTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            cardNum = comboBox.SelectedItem.ToString();
            string queryBalance = "Select Balance FROM Card WHERE Number = '" + cardNum + "'";
            string queryPinCode = "Select Pincode FROM Card WHERE Number = '" + cardNum + "'";

            using (OleDbConnection connection = new OleDbConnection(Connection))
            {
                connection.Open();                

                OleDbCommand command = new OleDbCommand(queryBalance, connection);                
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read()) // записує баланс карточки
                {
                    string balance = reader.GetString(0);
                    BalanceBox.Text = balance;
                }               
            }
        }

        private void PinEditBtn_Click(object sender, RoutedEventArgs e)
        {
            PinCodeBox.IsEnabled = true;
        }

        private void SecPinEditBtn_Click(object sender, RoutedEventArgs e)
        {
            BalanceBox.IsEnabled = true;
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
            this.Close();
        }

        private void PinCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newPincode = ((TextBox)sender).Text;
            string newHashedPincode = Bc.HashPassword(newPincode);
            string update = "UPDATE CARD SET Pincode = @newPincode WHERE Number = @cardNum";

            OleDbConnection connection = new OleDbConnection(Connection);
            connection.Open();

            OleDbCommand updateCommand = new OleDbCommand(update, connection);
            updateCommand.Parameters.AddWithValue("@newPincode", newHashedPincode);
            updateCommand.Parameters.AddWithValue("@cardNum", cardNum);
           
            updateCommand.ExecuteNonQuery();
        }

        private void BalanceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newBalance = ((TextBox)sender).Text;
            string update = "UPDATE CARD SET Balance = @newBalance WHERE Number = @cardNum";

            OleDbConnection connection = new OleDbConnection(Connection);
            connection.Open();

            OleDbCommand updateCommand = new OleDbCommand(update, connection);
            updateCommand.Parameters.AddWithValue("@newBalance", newBalance);
            updateCommand.Parameters.AddWithValue("@cardNum", cardNum);

            updateCommand.ExecuteNonQuery();          
        }

        private bool IsNumericInput(string input)
        {
        return input.All(char.IsDigit); // Перевіряємо чи всі символи в рядку є цифрами
        }

        private void PinCodeBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumericInput(e.Text))
            {
                e.Handled = true; 
            }
        }

        private void BalanceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumericInput(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
