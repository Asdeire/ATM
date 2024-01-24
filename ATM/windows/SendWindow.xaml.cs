using ATM.messages;
using ATM.windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics.Eventing.Reader;
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

namespace ATM
{
    /// <summary>
    /// Логика взаимодействия для SendWindow.xaml
    /// </summary>
    public partial class SendWindow : Window
    {
        public SendWindow()
        {
            InitializeComponent();           
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void NumberBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            CardNumberTxtBox.Text += button.Content.ToString();
        }

        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CardNumberTxtBox.Text.Length > 0)
            {
                CardNumberTxtBox.Text = CardNumberTxtBox.Text.Substring(0, CardNumberTxtBox.Text.Length - 1);
            }

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            string secondCardNum = CardNumberTxtBox.Text;

            string sql = "SELECT * FROM Card WHERE Number= @number";
            string Connection = Singleton.Instance.connection;

            OleDbConnection connection = new OleDbConnection(Connection);
            connection.Open();

            OleDbCommand command = new OleDbCommand(sql, connection);
            command.Parameters.AddWithValue("@number", secondCardNum);

            OleDbDataReader reader = command.ExecuteReader();

            if(secondCardNum != Singleton.Instance.cardNumber)
            {
                if (reader.HasRows)
                {
                    SumWindow sumWindow = new SumWindow(secondCardNum);
                    sumWindow.Show();
                    this.Close();
                }
                else
                {
                    Alert alert = new Alert("Такої карти не існує");
                    alert.Show();
                    CardNumberTxtBox.Text = "";

                }
                
            }
            else
            {
                Alert alert = new Alert("Введіть іншу карту");
                alert.Show();
                CardNumberTxtBox.Text = "";
            }
        }
    }
}
