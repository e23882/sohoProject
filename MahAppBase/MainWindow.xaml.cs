using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using MahAppBase.ViewModel;
using MahApps.Metro.Controls;

namespace MahAppBase
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow(string book, string lesson, string studentID)
        {
            InitializeComponent();
            MainComponent mainViewModel = new MainComponent(book, lesson, studentID);
            
            gdMain.DataContext = mainViewModel;
            mainViewModel.GetBookText();
            mainViewModel.GetBookParagraphCount();
        }

        public string Book { get; internal set; }
        public string Lesson { get; internal set; }
        public string StudentID { get; internal set; }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            // 在默认浏览器中打开超链接的目标 URL
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}