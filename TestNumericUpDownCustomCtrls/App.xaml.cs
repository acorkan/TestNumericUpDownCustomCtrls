using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace TestNumericUpDownCustomCtrls
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            var viewModel = new MainViewModel();
            mainWindow.DataContext = viewModel;
            mainWindow.Show();
        }
    }
}
