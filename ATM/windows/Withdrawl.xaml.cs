using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
using ATM.messages;


namespace ATM
{
    /// <summary>
    /// Логика взаимодействия для Withdrawl.xaml
    /// </summary>
    public partial class Withdrawl : Window
    {
        public Withdrawl()
        {
            InitializeComponent();
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow  mainWindow  = new MainWindow();
            mainWindow.Show();  
            this.Close();
        }

        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            amountBox.Text += button.Content.ToString();
        }


        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (amountBox.Text.Length > 0)
            {
                amountBox.Text = amountBox.Text.Substring(0, amountBox.Text.Length - 1);
            }
        }

        private void HunBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            amountBox.Text = button.Content.ToString();
        }

        private void СontinuenBtn_Click(object sender, RoutedEventArgs e)
        {
            int amount = int.Parse(amountBox.Text);
            int balance = int.Parse(Singleton.Instance.Balance);
            string cardNum = Singleton.Instance.cardNumber;

            string Connection = Singleton.Instance.connection;
            string update = "UPDATE CARD SET Balance = @newBalance WHERE Number = @cardNum";
            string insert = "INSERT INTO History VALUES (@number, Date$() + ' ' + Time$(), @type, @amount)";

            string type = "Зняття готівки ";

            using (OleDbConnection connection = new OleDbConnection(Connection))
            {
                connection.Open();

                if (amount % 50 == 0)
                {
                    if (balance > amount)
                    {
                        int newBalance = balance - amount;
                        newBalance.ToString();

                        
                        OleDbCommand insertCommand = new OleDbCommand(insert, connection);
                        insertCommand.Parameters.AddWithValue("@number", cardNum);
                        insertCommand.Parameters.AddWithValue("@type", type);
                        insertCommand.Parameters.AddWithValue("@amount", -amount);
                        insertCommand.ExecuteNonQuery();

                        OleDbCommand updateCommand = new OleDbCommand(update, connection);
                        updateCommand.Parameters.AddWithValue("@new_balance", newBalance);
                        updateCommand.Parameters.AddWithValue("@cardNum", cardNum);
                        updateCommand.ExecuteNonQuery();
                       
                        Succes succes = new Succes("Успішно");
                        succes.Show();
                        amountBox.Text = "";
                    }
                    else
                    {
                        Alert alert = new Alert("Недостатньо коштів");
                        alert.Show();
                    }
                }
                else
                {
                    Alert alert = new Alert("Сума не кратна 50");
                    alert.Show();
                }             
            }
        }
    }
}
