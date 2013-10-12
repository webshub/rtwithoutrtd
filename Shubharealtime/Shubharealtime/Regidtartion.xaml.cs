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

namespace Shubhalabha123
{
    /// <summary>
    /// Interaction logic for Regidtartion.xaml
    /// </summary>
    public partial class Regidtartion : UserControl
    {
        public Regidtartion()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            string leadurl = filepath.Substring(0, filepath.Length - 18) + "realtimereg.html";
            Uri a3 = new System.Uri("http://shubhalabha.in/real/realtimereg.html");
            try
            {

                lead.Source = a3;
            }
            catch
            {

            }
        }
    }
}
