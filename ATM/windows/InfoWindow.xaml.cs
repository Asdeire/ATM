using System;
using System.Collections;
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

namespace ATM.windows
{
    /// <summary>
    /// Логика взаимодействия для InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public string[] types { get; set; }
        public string type;
        public string cardNum;
        public string Connection = Singleton.Instance.connection;

        public InfoWindow()
        {
            InitializeComponent();
            types = new string[] { "Усі", "Переказ", "Поповнення", "Зняття готівки" };
            ChooseTypeBox.ItemsSource = types;

            using (OleDbConnection connection = new OleDbConnection(Connection))
            {
                connection.Open();
                string query = "SELECT Number FROM Card";
                OleDbCommand oleDbCommand = new OleDbCommand(query, connection);
                OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader();
                while (oleDbDataReader.Read())
                {
                    string cardNumber = oleDbDataReader.GetString(0); 
                    ChooseNumTypeBox.Items.Add(cardNumber);
                }
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
            this.Close();
        }

        private void ChooseTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox typeBox = sender as ComboBox;
            type = typeBox.SelectedItem.ToString();

            string query;


            OleDbConnection connection = new OleDbConnection(Connection);

            if (type == "Усі")
            {
                query = "SELECT Number, Type, Amount, Data FROM History WHERE Number = '" + cardNum + "'";

                DataTable dt = new DataTable();

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                adapter.Fill(dt);

                data.ItemsSource = dt.DefaultView;
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

        private void ChooseNumTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox numTypeBox = sender as ComboBox;
            cardNum = numTypeBox.SelectedItem.ToString();

            OleDbConnection connection = new OleDbConnection(Connection);
                string query = "SELECT Number, Type, Amount, Data FROM History WHERE Number = '" + cardNum + "'";

                DataTable dt = new DataTable();

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                adapter.Fill(dt);

                data.ItemsSource = dt.DefaultView;

            ChooseTypeBox.Text = "Усі";
        }
    }
}
