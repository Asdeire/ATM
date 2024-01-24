using ATM.messages;
using System;
using System.Collections.Generic;
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

namespace ATM.windows
{
    /// <summary>
    /// Логика взаимодействия для SumWindow.xaml
    /// </summary>
    public partial class SumWindow : Window
    {
        public string secNum;

        public SumWindow(string secondCardNum)
        {
            InitializeComponent();
            secNum = secondCardNum;
        }

        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            AmountTxtBox.Text += button.Content.ToString();
        }

        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AmountTxtBox.Text.Length > 0)
            {
                AmountTxtBox.Text = AmountTxtBox.Text.Substring(0, AmountTxtBox.Text.Length - 1);
            }
        }

        private void hunBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            AmountTxtBox.Text = "";
            AmountTxtBox.Text += button.Content.ToString();
        }

        private void СontinuenBtn_Click(object sender, RoutedEventArgs e)
        {
            string cardNum = Singleton.Instance.cardNumber;
            int firstBalance = int.Parse(Singleton.Instance.Balance);
            string secBalance;
            string secondCardNum = secNum;

            string type = "Переказ";
            string query = "SELECT Balance FROM Card WHERE Number = @number";
            string sql = "SELECT * FROM Card WHERE Number = @number";
            string insert = "INSERT INTO History VALUES (@number, Date$() + ' ' + Time$(), @type, @amount)";
            string update = "UPDATE CARD SET Balance = @newBalance WHERE Number = @cardNum";
            string Connection = Singleton.Instance.connection;

            OleDbConnection connection = new OleDbConnection(Connection);
            connection.Open();

            OleDbCommand command = new OleDbCommand(sql, connection);
            command.Parameters.AddWithValue("@number", secondCardNum);

            OleDbDataReader reader = command.ExecuteReader();


            if (AmountTxtBox.Text.Length > 0)
            {
                OleDbCommand comm = new OleDbCommand(query, connection);
                comm.Parameters.AddWithValue("@number", secondCardNum);
                int amount = int.Parse(AmountTxtBox.Text);
                OleDbDataReader read = comm.ExecuteReader();

                if (read.Read())
                {
                    secBalance = read.GetString(0);
                    if (firstBalance > amount)
                    {
                        int newMinBalance = firstBalance - amount;
                        int newPlusBalance = int.Parse(secBalance) + amount;

                        OleDbCommand updateCommand = new OleDbCommand(update, connection);
                        updateCommand.Parameters.AddWithValue("@new_balance", newMinBalance);
                        updateCommand.Parameters.AddWithValue("@cardNum", cardNum);
                        updateCommand.ExecuteNonQuery();

                        OleDbCommand updateCom = new OleDbCommand(update, connection);
                        updateCom.Parameters.AddWithValue("@new_balance", newPlusBalance);
                        updateCom.Parameters.AddWithValue("@cardNum", secondCardNum);
                        updateCom.ExecuteNonQuery();

                        OleDbCommand insertCommand = new OleDbCommand(insert, connection);
                        insertCommand.Parameters.AddWithValue("@number", cardNum);
                        insertCommand.Parameters.AddWithValue("@type", type);
                        insertCommand.Parameters.AddWithValue("@amount", -amount);
                        insertCommand.ExecuteNonQuery();

                        OleDbCommand insertCom = new OleDbCommand(insert, connection);
                        insertCom.Parameters.AddWithValue("@number", secondCardNum);
                        insertCom.Parameters.AddWithValue("@type", type);
                        insertCom.Parameters.AddWithValue("@amount", amount);
                        insertCom.ExecuteNonQuery();

                        Succes succes = new Succes("Успішна операція");
                        succes.Show();
                        AmountTxtBox.Text = "";
                    }
                    else
                    {
                        Alert alert = new Alert("Недостатньо коштів");
                        alert.Show();
                    }

                }
                else
                {
                    Alert alert = new Alert("Помилка даних");
                    alert.Show();
                    AmountTxtBox.Text = "";
                }
            }
        }        

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            SendWindow sendWindow = new SendWindow();
            sendWindow.Show();
            this.Close();
        }
    }
}

