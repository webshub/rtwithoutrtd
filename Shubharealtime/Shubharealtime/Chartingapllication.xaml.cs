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

namespace Shubharealtime
{
    /// <summary>
    /// Interaction logic for Chartingapllication.xaml
    /// </summary>
    public partial class Chartingapllication : UserControl
    {
         RegistryKey regKey = Registry.CurrentUser;
                string chartingapp = "";
        public Chartingapllication()
        {
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");

            InitializeComponent();
        }
        

        private void btnTarget_Click(object sender, RoutedEventArgs e)
        {
            var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;


                db_path.Text = Target_Folder_Path;

               
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
             var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;


                Amiexepath.Text = Target_Folder_Path;

               
            }
            
        }

        private void meta_browes_Click(object sender, RoutedEventArgs e)
        {
            var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;


                meatpath_txt.Text = Target_Folder_Path;


            }
            
        }

        private void ami_chk_Checked(object sender, RoutedEventArgs e)
        {
           
               
        }

        private void meta_chk_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void meta_chk_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ami_chk.IsChecked == true)
            {
                chartingapp = chartingapp + "," + "Amibroker";
            }
            if (meta_chk.IsChecked == true)
            {
                chartingapp = chartingapp + "," + "Metastock";

            }
            if (fchart_chk.IsChecked == true)
            {
                chartingapp = chartingapp + "," + "Fchart";

            }
            if (chartingapp == "")
            {
                chartingapp = "Amibroker";
            }

            regKey.SetValue("Amibrokerdatapath", db_path.Text.ToString());
            regKey.SetValue("Metastockdatapath", meatpath_txt.Text.ToString());
            regKey.SetValue("Amiexepath", Amiexepath .Text.ToString());



            regKey.SetValue("Chartingapplication", chartingapp);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ami_chk.IsChecked = true;

        }
    }
}