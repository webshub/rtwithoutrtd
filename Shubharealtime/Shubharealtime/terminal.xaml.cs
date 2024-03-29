﻿//////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////

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

namespace Shubharealtime
{
    /// <summary>
    /// Interaction logic for terminal.xaml
    /// </summary>
    /// 
   
    public partial class terminal : UserControl
    {
         Wizart w = new Wizart();
        public terminal()
        {
            InitializeComponent();
        }
         
        private void now_Checked(object sender, RoutedEventArgs e)
        {
            w.Rsult_lbl.Content = "";
            plusbackfill.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;

            try
            {
                stackPanel2.Children.RemoveAt(0);
            }
            catch
            {
            }
            Nowterminal n = new Nowterminal();
            stackPanel2.Children.Add(n);
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("terminal", "NOW");
        }

        private void tradetiger_Checked(object sender, RoutedEventArgs e)
        {
            w.Rsult_lbl.Content = "";

            try
            {
                stackPanel2.Children.RemoveAt(0);
            }
            catch
            {
            }
            Tradetiger t = new Tradetiger();
            stackPanel2.Children.Add(t);
        }

        private void odin_Checked(object sender, RoutedEventArgs e)
        {
            w.Rsult_lbl.Content = "";

            try
            {
                stackPanel2.Children.RemoveAt(0);
            }
            catch
            {
            }
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            ODIN o = new ODIN();
            stackPanel2.Children.Add(o);
            regKey.SetValue("terminal", "NOW");

        }

        private void nest_Checked(object sender, RoutedEventArgs e)
        {
            w.Rsult_lbl.Content = "";
            plusbackfill.Visibility = Visibility.Visible;
            label2.Visibility = Visibility.Visible;

            try
            {
                stackPanel2.Children.RemoveAt(0);
            }
            catch
            {
            }
            Nest n = new Nest();
            stackPanel2.Children.Add(n);

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("terminal", "NEST");


        }

        private void tradetiger_Checked_1(object sender, RoutedEventArgs e)
        {
            w.Rsult_lbl.Content = "";
            plusbackfill.IsChecked = false;
            plusbackfill.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;
            try
            {
                stackPanel2.Children.RemoveAt(0);
            }
            catch
            {
            }
            Tradetiger t = new Tradetiger();
            stackPanel2.Children.Add(t );
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("terminal", "Tradetiger");
            regKey.SetValue("backfill", "no");



        }

        private void odin_Checked_1(object sender, RoutedEventArgs e)
        {
            w.Rsult_lbl.Content = "";
            plusbackfill.IsChecked = false;
            plusbackfill.Visibility = Visibility.Hidden;
            label2.Visibility = Visibility.Hidden;


            try
            {
                stackPanel2.Children.RemoveAt(0);
            }
            catch
            {
            }
            ODIN o = new ODIN();
            stackPanel2.Children.Add(o);
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("terminal", "Odin");
            regKey.SetValue("backfill", "no");


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            w.Rsult_lbl.Content = "";
            try
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                var terminalname = regKey.GetValue("terminal");
                string terminal = terminalname.ToString();
                var backfill1 = regKey.GetValue("backfill");
                string backfill = backfill1.ToString();
                if (backfill == "yes")
                {
                    plusbackfill.IsChecked = true;
                }

                if (terminal == "NEST")
                {
                    nest.IsChecked = true;
                }
                else if (terminal == "NOW")
                {
                    now.IsChecked = true;
                }
                else if (terminal == "Odin")
                {
                    odin.IsChecked = true;
                }
                else if (terminal == "Tradetiger")
                {
                    tradetiger.IsChecked = true;
                }
                else
                {
                    nest.IsChecked = true;

                }
            }
            catch
            {
            }
        }

        private void plusbackfill_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("backfill", "yes");
           
           

        }

        private void plusbackfill_Click(object sender, RoutedEventArgs e)
        {
            if(plusbackfill.IsChecked==false )
            {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("backfill", "no");
            }
        }

        private void stackPanel2_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
