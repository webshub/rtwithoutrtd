﻿using System;
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
using System.IO;
using System.Diagnostics;

namespace Shubhalabha123
{
    /// <summary>
    /// Interaction logic for EndRT.xaml
    /// </summary>
    public partial class EndRT : UserControl
    {
        public EndRT()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-temp\");


            var path = regKey.GetValue("Applicationpath");
            string pathtostartprocess = path.ToString().Substring(0, path.ToString().Length - 4);
            Process[] workers = Process.GetProcessesByName("Shubharealtime.vshost");

            foreach (Process worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
           
            try
            {

               // System.Diagnostics.Process.Start(path.ToString());
            }
            catch
            {
            }
        }
    }
}
