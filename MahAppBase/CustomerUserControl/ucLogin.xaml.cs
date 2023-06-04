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

namespace MahAppBase
{
    /// <summary>
    /// ucLogin.xaml 的互動邏輯
    /// </summary>
    public partial class ucLogin : Window
    {
        public ucLogin()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow main = new MainWindow(tbBook.Text, tbLesson.Text, tbStudent.Text);
            main.Show();
            this.Close();
        }

       
        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            stWelcome.Visibility = Visibility.Collapsed;
            gdLogin.Visibility = Visibility.Visible;
        }
    }
}
