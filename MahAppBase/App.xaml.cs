using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MahAppBase
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        public App() 
        {
            ucLogin loginWindow = new ucLogin();
            loginWindow.Show();
        }
        
    }
}
