using System;
using System.Management;
using System.Configuration;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading.Tasks ;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Collections;
using System.IO.Compression;
using System.Diagnostics;

using System.IO.Packaging;
using System.Text.RegularExpressions;
using System.Data.OleDb;

using Microsoft.Win32;
using System.Security.Principal;
namespace ExcelLogin
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
        //Run program as Administartor 
        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");

            var registerdate = regKey.GetValue("sd");
            var paidornot = regKey.GetValue("sp");

            ///////////////////////////////
            //expiration

            try
            {



                regKey.SetValue("Wizart", "done");




                string[] datefromreg = registerdate.ToString().Split('-');



                DateTime reg = new DateTime(Convert.ToInt32(datefromreg[2]), Convert.ToInt32(datefromreg[1]), Convert.ToInt32(datefromreg[0]));




                reg = reg.AddDays(9);
                //its checking trail period expired or not 
                if (reg < DateTime.Today.Date)
                {
                    // Uri a = new System.Uri("http://besttester.com/lic/lic.txt");
                    Uri a = new System.Uri("http://shubhalabha.in/lic.txt");


                    // webBrowser1.Source = a;
                    string credentials = "liccheck:lic123!@#";
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(a);
                    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));
                    request.PreAuthenticate = true;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    ////////////////////////////////////////////

                    //checking file which is store  on server 
                    string[] serverdata = reader.ReadToEnd().Split(',');
                    string[] serverdata1 = null;
                    int flagforuserpresentonserver = 0;
                    for (int i = 0; i < serverdata.Count(); i++)
                    {
                        serverdata1 = serverdata[i].Split(' ');
                        string[] datefromserver = serverdata1[1].ToString().Split('-');
                        // DateTime dateonserver=Convert.ToDateTime(serverdata1[1]);
                        DateTime dateonserver = new DateTime(Convert.ToInt32(datefromserver[2]), Convert.ToInt32(datefromserver[1]), Convert.ToInt32(datefromserver[0]));

                        ManagementObject dsk1 = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                        dsk1.Get();
                        string id1 = dsk1["VolumeSerialNumber"].ToString();
                        if (id1 == serverdata1[0])
                        {
                            flagforuserpresentonserver = 1;
                            if (dateonserver < DateTime.Today.Date)
                            {
                                System.Windows.MessageBox.Show("Your Trial version is expired, please contact sales@shubhalabha.in'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                                RegistryKey regKey1 = Registry.CurrentUser;
                                regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
                                regKey1.SetValue("valid", "notworking");

                            }
                        }
                    }

                    if (flagforuserpresentonserver == 0)
                    {
                        System.Windows.MessageBox.Show("Your Trial version is expired, please contact sales@shubhalabha.in'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        RegistryKey regKey1 = Registry.CurrentUser;
                        regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
                        regKey1.SetValue("valid", "notworking");
                        Environment.Exit(0);
                        return;
                        // closeallprocess();
                    }
                    else
                    {
                        RegistryKey regKey1 = Registry.CurrentUser;
                        regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
                        regKey1.SetValue("valid", "working");
                        Environment.Exit(0);
                    }
                    ///////////////////////////////////////////


                }
                else
                {
                    System.Windows.MessageBox.Show("Your trial period will expire on " + reg.ToShortDateString(), "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    RegistryKey regKey1 = Registry.CurrentUser;
                    regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
                    regKey1.SetValue("valid", "working");
                    Environment.Exit(0);
                }


            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(ex.Message, "Error Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            }



















            //RegistryKey regKey = Registry.CurrentUser;
            //regKey = regKey.CreateSubKey(@"Windows-temp\");

            //try
            //{
            //    var registerdate = regKey.GetValue("sd");
            //    var paidornot = regKey.GetValue("sp");

            //    DateTime reg = Convert.ToDateTime(registerdate);
            //    reg = reg.AddDays(3);

            //    if (paidornot.ToString() == "Key for xp")
            //    {
            //        if (reg < DateTime.Today.Date)
            //        {
            //            //System.Windows.MessageBox.Show("Trial version expired please contact to sales@shubhalabha.in ");
            //            RegistryKey regKey1 = Registry.CurrentUser;
            //            regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
            //            regKey1.SetValue("valid", "notworking");
            //            this.Close();
            //            Environment.Exit(0);
            //            return;
            //        }
            //        else
            //        {
            //            RegistryKey regKey1 = Registry.CurrentUser;
            //            regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
            //            regKey1.SetValue("valid","working");
            //            this.Close();
            //            Environment.Exit(0);
            //        }

            //    }
            //    else
            //    {
            //        if (paidornot.ToString() == "1001")
            //        {
            //            RegistryKey regKey1 = Registry.CurrentUser;
            //            regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
            //            regKey1.SetValue("valid", "working");
            //            this.Close();
            //            Environment.Exit(0);
            //        }
            //        else
            //        {
            //            //System.Windows.MessageBox.Show("Trial version expired please contact to sales@shubhalabha.in ");
            //            RegistryKey regKey1 = Registry.CurrentUser;
            //            regKey1 = regKey1.CreateSubKey(@"Software\VB and VBA Program Settings\windows\Startup");
            //            regKey1.SetValue("valid", "notworking");
            //            this.Close();
            //            Environment.Exit(0);

            //        }

            //    }
            //}
            //catch (Exception ex)
            //{

            //}

        }
    }
}
