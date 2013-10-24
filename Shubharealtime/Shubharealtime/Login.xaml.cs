//////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Web;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Configuration;
using System.Net.Mail;
using System.Management;
using System.Reflection;
namespace Shubharealtime
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

        //load registartion from 
        private void Window_Loaded(object sender, RoutedEventArgs e)
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


            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create("http://www.Google.co.in");
                System.Net.WebResponse myResponse = myRequest.GetResponse();
                Uri a = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=27&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                //  Uri a = new System.Uri(" http://shubhalabha.in/products-2/");

                advertisement.Source = a;
                //  wad4.Source = a4;


            }
            catch
            {


                advertisement.Visibility = Visibility.Hidden;


            }
            
        }
       


      
        

       //set registry entry 
        public void SetRegKey()
        {

            //RegistryKey regKey = Registry.CurrentUser;
            //regKey = regKey.CreateSubKey(@"Software-RT\");
            //regKey.SetValue("ApplicationID", "1");

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("sd", DateTime.Today.Day+"-"+DateTime.Today.Month+"-"+DateTime.Today.Year);
            regKey.SetValue("sp", "Key for xp");
            regKey.SetValue("ApplicationID", "1");
            regKey.SetValue("LTT", "0");
           // regKey.SetValue("Wizart", "notdone");

            regKey.SetValue("Applicationpath", System.Reflection.Assembly.GetExecutingAssembly().Location.ToString());


            
        }

        //ignor java script error 
        void wb_LoadCompleted(object sender,System.Windows.Navigation. NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            System.Windows.Controls.WebBrowser wb = (System.Windows.Controls.WebBrowser)sender;
            wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

       

      





  public void HideScriptErrors(WebBrowser wb, bool Hide)
{

FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
if
 (fiComWebBrowser == null)
return;

object
 objComWebBrowser = fiComWebBrowser.GetValue(wb);

if
 (objComWebBrowser == null)
return;

objComWebBrowser.GetType().InvokeMember( 
"Silent",
BindingFlags.SetProperty,null,objComWebBrowser, new object[] { Hide }); 

}

 



        private void lead_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            HideScriptErrors(lead , true);

            if (lead.Source.ToString() == "http://shubhalabha.in/eng/crm/index.php?entryPoint=WebToLeadCapture")
            {
                SetRegKey();


                this.Hide();
                Shubharealtime.Window1 s = new Window1();
                s.ShowDialog();

                return;
            }
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
            
            Environment.Exit(0);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

       

       

       
    }
}
