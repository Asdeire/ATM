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
using System.Windows.Threading;

namespace ATM.messages
{
    /// <summary>
    /// Логика взаимодействия для Succes.xaml
    /// </summary>
    public partial class Succes : Window
    {
        private DispatcherTimer timer;

        public Succes(string message)
        {
            InitializeComponent();
            succesTxt.Text = message;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Succes_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }
    }
}
