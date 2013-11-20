using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
namespace Endrt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
     
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


           
            
        }

        private void EndRT_Click(object sender, RoutedEventArgs e)
        {
           
            
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            

            var path = regKey.GetValue("Applicationpath");
            string pathtostartprocess = path.ToString().Substring(0, path.ToString().Length -4);
            Process[] workers = Process.GetProcessesByName("shubhalabhartx.vshost");
            Process[] workers1 = Process.GetProcessesByName("shubhalabhartx");

            foreach (Process worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
            foreach (Process worker in workers1)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
            try
            {

                System.Diagnostics.Process.Start(path.ToString());
            }
            catch
            {
            }
            Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");


            var path = regKey.GetValue("Applicationpath");
            string pathtostartprocess = path.ToString().Substring(0, path.ToString().Length - 4);
            Process[] workers = Process.GetProcessesByName("shubhalabhartx.vshost");
            Process[] workers1 = Process.GetProcessesByName("shubhalabhartx");

            foreach (Process worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
            foreach (Process worker in workers1)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
        }
    }
}
