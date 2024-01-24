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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace ATM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string balance;
            string num = Singleton.Instance.cardNumber;
            numTxt.Text = num;
            


            string Connection = Singleton.Instance.connection;
            string query = "SELECT Balance FROM Card WHERE Number = @number";
            

            using (OleDbConnection connection = new OleDbConnection(Connection))
            {
                connection.Open();


                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@number", num); // додає значення 

                OleDbDataReader reader = command.ExecuteReader(); ;

                if (reader.Read()) // записує баланс карточки
                {
                    balance = reader.GetString(0);
                    balanceTxt.Text = balance;
                    Singleton.Instance.Balance = balanceTxt.Text;
                }

            }
        }
       

        private void TopupBtn_Click(object sender, RoutedEventArgs e)
        {
            TopUp topUp = new TopUp();
            topUp.Show();
            this.Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Authorisation authorisation = new Authorisation();
            authorisation.Show();
            this.Close();           
        }

        private void TransferCardBtn_Click(object sender, RoutedEventArgs e)
        {
            SendWindow sendWindow = new SendWindow();
            sendWindow.Show();
            this.Close();
        }

        private void WithdrawalBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Withdrawl withdrawl = new Withdrawl();
            withdrawl.Show();
            this.Close();
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            History history = new History();
            history.Show();
            this.Close();
        }
    }
}
