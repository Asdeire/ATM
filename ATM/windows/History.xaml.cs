using ATM.messages;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace ATM
{
    /// <summary>
    /// Логика взаимодействия для History.xaml
    /// </summary>
    public partial class History : Window
    {
        public string[] types { get; set; }
        public string type;

        public History()
        {
            InitializeComponent();
            types = new string[] {"Усі", "Переказ", "Поповнення", "Зняття готівки"};
            DataContext = this; // заповнює combobox значеннями
          
                     
        }

        public string Connection = Singleton.Instance.connection;


        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ChooseTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox typeBox = sender as ComboBox;
            type = typeBox.SelectedItem.ToString();

            string cardNum = Singleton.Instance.cardNumber;
            string query;


            OleDbConnection connection = new OleDbConnection(Connection); 

            if (type == "Усі")
            {
                query = "SELECT Number, Type, Amount, Data FROM History WHERE Number = '" + cardNum + "'";

                DataTable dt = new DataTable();

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                adapter.Fill(dt);

                data.ItemsSource = dt.DefaultView; // заповнює таблицю значеннями з бд
            }
            else
            {
                query = "SELECT Number, Type, Amount, Data FROM History WHERE Number = '" + cardNum + "' AND Type = '" + type + "'";

                DataTable dt = new DataTable();

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                adapter.Fill(dt);

                data.ItemsSource = dt.DefaultView;
            }
        }
    }
}
