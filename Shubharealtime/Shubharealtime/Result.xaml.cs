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
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : UserControl
    {
        public Result()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey(@"Windows-temp\");
                var terminalname = regKey.GetValue("terminal");
                var Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath");
                var Metastockdatapath = regKey.GetValue("Metastockdatapath");
                var Chartingapplication = regKey.GetValue("Chartingapplication");


                var Amiexepath = regKey.GetValue("Amiexepath");
                var backfill1 = regKey.GetValue("backfill");
                string backfill = "Not present";
                if (backfill1 != null)
                {
                    backfill = backfill1.ToString();

                }

                result_amipath.Content = Amibrokerdatapath.ToString();
                result_chart.Content = Chartingapplication.ToString();
                result_terminal.Content = terminalname.ToString();
                result_metapath.Content = Metastockdatapath.ToString();
                nestnowbackfill.Content = backfill;

            }
            catch
            {
            }
          
        }
    }
}
