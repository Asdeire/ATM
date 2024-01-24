using ATM.messages;
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

namespace ATM.windows
{
    /// <summary>
    /// Логика взаимодействия для AdminAuthorisation.xaml
    /// </summary>
    public partial class AdminAuthorisation : Window
    {
        public string login = "admin";
        public string password = "admin";

        public AdminAuthorisation()
        {
            InitializeComponent();               
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            string adminTxt = LoginTextBox.Text;
            string passwordTxt = PinPswrdBox.Password;            

            if(adminTxt == login && passwordTxt == password) // Перевірка на коректність введених логіну та пароля
            {
                AdminPanel adminPanel = new AdminPanel();
                adminPanel.Show();
                this.Close();
            }
            else
            {
                Alert alert = new Alert("Помилка");
                alert.Show();
            }
            LoginTextBox.Text = null;
            PinPswrdBox.Password = null;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Authorisation authorisation = new Authorisation();
            authorisation.Show();
            this.Close();
        }
    }
}
