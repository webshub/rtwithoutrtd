//////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////


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
using Microsoft.Office.Interop.Excel;
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
using ManagedWinapi.Windows;
using ManagedWinapi.Accessibility;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Collections;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows.Forms;

using System.IO.Packaging;
using System.Text.RegularExpressions;
using System.Data.OleDb;

using Microsoft.Win32;
using System.Security.Principal;
using log4net;
using log4net.Config;
namespace Shubharealtime
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 :System.Windows . Window
    {
        private bool stopRefreshControls = false;
        private bool dataChanged = false;
        Configuration config;
        WebClient Client = new WebClient();
        List<string> marketsymbol = new List<string>();
        List<string> Exchangename = new List<string>();
        Type type;
        List<string> yahoortname = new List<String>();
        List<string> yahoortdata = new List<String>();
        List<string> symbolname = new List<String>();
        List<string> exchagename = new List<string>();
        IRtdServer m_server;

        object[] args = new object[3];

        System.Windows.Threading.DispatcherTimer DispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
       
        List<int> marketsymboltoremove = new List<int>();
        public Window1()
        {
            
            InitializeComponent();
        }
        

        protected void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        //find machine  32 bit or 64 bit 
        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
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
           

            nestbackfill.IsChecked = false;
            googlebackfill.IsChecked = false;
            string amiexeoath1 = "";
            if (!IsRunAsAdministrator())
            {
                // It is not possible to launch a ClickOnce app as administrator directly, so instead we launch the
                // app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                // The following properties run the new process as administrator
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                // Start the new process
                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception)
                {
                    // The user did not allow the application to run as administrator
                    System.Windows.MessageBox.Show("Unable to Run program as Administrator,please ensure that you have admin privileges'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning );

                }

                // Shut down the current process
                Environment.Exit(0);
                System.Windows.Application.Current.Shutdown();

            }
            else
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



                    DateTime reg=new DateTime(Convert.ToInt32(datefromreg[2]),Convert.ToInt32(datefromreg[1]),Convert.ToInt32(datefromreg[0]));
                  


                  
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
                        string [] serverdata = reader.ReadToEnd().Split(',');
                        string [] serverdata1=null;
                        int flagforuserpresentonserver = 0;
                        string id1 = "";
                        for (int i = 0; i < serverdata.Count(); i++)
                         {
                             serverdata1 = serverdata[i].Split(' ');
                             string[] datefromserver = serverdata1[1].ToString().Split('-');
                           // DateTime dateonserver=Convert.ToDateTime(serverdata1[1]);
                             DateTime dateonserver = new DateTime(Convert.ToInt32(datefromserver[2]), Convert.ToInt32(datefromserver[1]), Convert.ToInt32(datefromserver[0]));

                            ManagementObject dsk1 = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                            dsk1.Get();
                             id1 = dsk1["VolumeSerialNumber"].ToString();
                            if (id1 == serverdata1[0] )
                            {
                                flagforuserpresentonserver = 1;
                            if (dateonserver<DateTime.Today.Date )
                            {
                                System.Windows.MessageBox.Show("Your Trial version is expired, please contact sales@shubhalabha.in \n Your registration ID is -" + id1 + "'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                                closeallprocess();

                            }
                            }
                         }

                        if (flagforuserpresentonserver==0)
                        {
                            System.Windows.MessageBox.Show("Your Trial version is expired, please contact sales@shubhalabha.in \n Your registration ID is -" + id1 + "", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                            closeallprocess();


                            return;
                        }
                        ///////////////////////////////////////////


                    }
                    else
                    {
                       
                        System.Windows.MessageBox.Show("Your trial period will expire on " + reg.ToShortDateString(), "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                    }


                }
                catch(Exception ex)
                {
                   
                    System.Windows.MessageBox.Show(ex.Message, "Error Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error );

                }



                ////show google symbol file in listview

            try
            {
                using (var reader = new StreamReader("C:\\myshubhalabha\\GoogleSymbolname.csv"))
                {
                    string line = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] name = line.Split(',');

                        listView1.Items.Add(new ListViewData(name[0], name[1], name[2]));


                       
                    }
                }
                using (var reader = new StreamReader("C:\\myshubhalabha\\Symbolname.csv"))
                {
                    string line = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] name = line.Split(',');

                        listView2.Items.Add(new ListViewData(name[0], name[1], name[2]));



                    }
                }
            }
            catch
            {
            }



            for (int i = 1; i < 11;i++ )
            {
                googledays.Items.Add(i );
            }
            googletime.Items.Add(1);
            googletime.Items.Add(5);

            googletime.SelectedIndex = 0;
            googledays.SelectedIndex = 5;

                /////take all save data 
                string targetpath = ConfigurationManager.AppSettings["targetpathforcombo"];
                string amipath = ConfigurationManager.AppSettings["amipath"];
                string terminalname = ConfigurationManager.AppSettings["terminalname"];
                string chartingapp = ConfigurationManager.AppSettings["chartingapp"];
                string chartingappbackfill = ConfigurationManager.AppSettings["chartingappbackfill"];

                string timetosave = ConfigurationManager.AppSettings["timetoRT"];

                string googleday = ConfigurationManager.AppSettings["googleday"];

                string google_time = ConfigurationManager.AppSettings["googlemin"];
                string nestback = ConfigurationManager.AppSettings["nestbackfill"];
                string tradetiger1 = ConfigurationManager.AppSettings["tradetiger"];

                string googleback = ConfigurationManager.AppSettings["googlebackfill"];
                string withoutback = ConfigurationManager.AppSettings["withoutbackfill"];
                withoutbackfill.IsChecked = true;
                googletime.SelectedIndex = Convert.ToInt32(google_time);
                googledays.SelectedIndex = Convert.ToInt32(googleday);

                //if (nestback == "True")
                //{

                //    nestbackfill.IsChecked = true;
                //    listView1.Visibility = Visibility.Hidden;
                //    listView2.Visibility = Visibility.Visible;
                //}
                //if (tradetiger1 == "True")
                //{
                //    tradetiger.IsChecked = true;
                //}
                //if (googleback == "True")
                //{
                //    googlebackfill.IsChecked = true;

                //    listView1.Visibility = Visibility.Visible;
                //    listView2.Visibility = Visibility.Hidden;
                //}
                //if (withoutback == "True")
                //{
                //    withoutbackfill.IsChecked = true;
                //}

                try
                {
                    //taking registry values 
                    var terminalname1 = regKey.GetValue("terminal");
                    var Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath");
                    var Metastockdatapath = regKey.GetValue("Metastockdatapath");
                    var Chartingapplication = regKey.GetValue("Chartingapplication");
                      
                    
                    var Amiexepath = regKey.GetValue("Amiexepath");
                    amiexeoath1 = Amiexepath.ToString();
                    var backfill1 = regKey.GetValue("backfill");
                    string backfill = "Not present";
                    if (backfill1 != null)
                    {
                        backfill = backfill1.ToString();

                    }

                    amipath = Amibrokerdatapath.ToString();
                    terminalname = terminalname1.ToString();
                    if (backfill != "yes")
                    {
                        nestbackfill.IsChecked = false;
                        nestbackfill.IsEnabled = false;
                        nestbackfill.Visibility = Visibility.Hidden;

                        // RTmapsymbol.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        nestbackfill.Visibility = Visibility.Visible;

                        RTmapsymbol.Visibility = Visibility.Hidden;
                    }
                    if (terminalname=="NEST")
                    {
                        textBlock4.Visibility = Visibility.Hidden;

                        RTD_server_name.SelectedIndex = 0;
                    }
                    else if (terminalname == "NOW")
                    {
                        textBlock4.Visibility = Visibility.Hidden;

                        RTD_server_name.SelectedIndex = 1;

                    }
                    else
                    {
                        Startrealtimeonly.Visibility = Visibility.Hidden;
                        Format_cb.Visibility = Visibility.Hidden;
                        RTD_server_name.Visibility = Visibility.Hidden;
                        timetoRT.Visibility = Visibility.Hidden;
                        label5.Visibility = Visibility.Hidden;
                        label8.Visibility = Visibility.Hidden;
                        label9.Visibility = Visibility.Hidden;

                        clickhere.Visibility = Visibility.Visible;
                        textBlock1.Visibility = Visibility.Visible;

                        textBlock3.Visibility = Visibility.Visible;
                        RTmapsymbol.Visibility = Visibility.Hidden;
                        nestbackfill.Visibility = Visibility.Hidden;
                        textBlock4.Visibility = Visibility.Visible;
                        nestbackfill.Visibility = Visibility.Hidden;
                        listView2.Visibility = Visibility.Hidden;
                        tradetigerinfo.Visibility = Visibility.Hidden;
                    }
                   


                }
                catch
                {
                }
               try
                {
                   
                   //Get motherboard id
                    ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                    dsk.Get();
                    string id = dsk["VolumeSerialNumber"].ToString();
                    moterboard.Text = id;
                    regiID.Content = id;
                    var terminalfromwizart1 = regKey.GetValue("terminal");
                    var Chartingappfromwizart1 = regKey.GetValue("chart");
                    string Chartingappfromwizart = "";
                    string terminalfromwizart = "";
                    try
                    {
                        Chartingappfromwizart = Chartingappfromwizart1.ToString();
                        terminalfromwizart = terminalfromwizart1.ToString();
                    }
                    catch
                    {

                    }


                    if (terminalfromwizart == "NEST")
                    {
                        RTD_server_name.SelectedIndex = 0;
                    }
                    if (terminalfromwizart == "NOW")
                    {
                        RTD_server_name.SelectedIndex = 1;
                    }
                   

                    //if (!Directory.Exists(targetpath + "\\sharekhan"))
                    //{
                    //    Directory.CreateDirectory(targetpath + "\\sharekhan");
                    //}

                    //if (!Directory.Exists(targetpath + "\\odin"))
                    //{
                    //    Directory.CreateDirectory(targetpath + "\\odin");
                    //}
                    //if (!Directory.Exists(targetpath + "\\nest-now"))
                    //{
                    //    Directory.CreateDirectory(targetpath + "\\nest-now");
                    //}


                }
                catch
                {
                }


              

               var versionno = regKey.GetValue("version");


               if (versionno == null || versionno.ToString() != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
                {
                //copy files into folder 
                try
                {
                    string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                    string processtostart = filepath.Substring(0, filepath.Length - 18) + "sharekhantoami.xlsm";

                   
                    File.Copy(processtostart, targetpath + "\\sharekhantoami.xlsm", true);
                    if (!Directory.Exists("C:\\myshubhalabha\\amirealtime"))
                    {
                        Directory.CreateDirectory("C:\\myshubhalabha\\amirealtime");
                    }
                    if (!Directory.Exists("C:\\myshubhalabha\\amibroker format file"))
                    {
                        Directory.CreateDirectory("C:\\myshubhalabha\\amibroker format file");
                    }

                    string programfilepath = ProgramFilesx86();

                    File.Copy(processtostart, "C:\\myshubhalabha\\sharekhantoami.xlsm", true);

                   



                    processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhaodin.xlsm";


                    File.Copy(processtostart, targetpath + "\\shubhaodin.xlsm", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "ExcelLogin.exe";

                    File.Copy(processtostart, targetpath + "\\ExcelLogin.exe", true);
                    File.Copy(processtostart, "C:\\myshubhalabha\\ExcelLogin.exe", true);



                    processtostart = filepath.Substring(0, filepath.Length - 18) + "MetaStockRefresher V 2.0.9 setup.exe";

                    File.Copy(processtostart, targetpath + "\\MetaStockRefresher V 2.0.9 setup.exe", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaNest-Now.xlsm";

                    File.Copy(processtostart, targetpath + "\\ShubhaNest-Now.xlsm", true);

                  



                    processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhaxls.format";


                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhaxls.format", true);
                    File.Copy(processtostart, amiexeoath1 + "\\Formats\\shubhaxls.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhasharekhan.format";


                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhasharekhan.format", true);
                    File.Copy(processtostart, amiexeoath1 + "\\Formats\\Shubhasharekhan.format", true);



                    processtostart = filepath.Substring(0, filepath.Length - 18) + "nestbackfill.format";

                    File.Copy(processtostart, amiexeoath1 + "\\Formats\\nestbackfill.format", true);



                    processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhanest-now.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhanest-now.format", true);
                    File.Copy(processtostart, amiexeoath1 + "\\Formats\\shubhanest-now.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaRt.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\ShubhaRt.format", true);
                    File.Copy(processtostart, programfilepath + "\\AmiBroker\\Formats\\ShubhaRt.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhabackfill.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhabackfill.format", true);
                    File.Copy(processtostart, amiexeoath1 + "\\Formats\\Shubhabackfill.format", true);
                   
                    processtostart = filepath.Substring(0, filepath.Length - 18) + "RT.format";


                    File.Copy(processtostart,"C:\\myshubhalabha\\amibroker format file\\\\RT.format", true);
                    File.Copy(processtostart, amiexeoath1 + "\\Formats\\RT.format", true);


                    //samples 

                    if (!Directory.Exists("C:\\myshubhalabha\\samples"))
                    {
                        Directory.CreateDirectory("C:\\myshubhalabha\\samples");
                    }
                    processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaOdin.txt";
                    File.Copy(processtostart, "C:\\myshubhalabha\\samples\\ShubhaOdin.txt", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Googlebackfill.csv";
                    File.Copy(processtostart, "C:\\myshubhalabha\\samples\\Googlebackfill.csv", true);


                    processtostart = filepath.Substring(0, filepath.Length - 18) + "realtimefchart.csv";
                    File.Copy(processtostart, "C:\\myshubhalabha\\samples\\realtimefchart.csv", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "realtimemetastock.csv";
                    File.Copy(processtostart, "C:\\myshubhalabha\\samples\\realtimemetastock.csv", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "AmibrokerRTdata.txt";
                    File.Copy(processtostart, "C:\\myshubhalabha\\samples\\AmibrokerRTdata.txt", true);


                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhasharekhan.txt";
                    File.Copy(processtostart, "C:\\myshubhalabha\\samples\\Shubhasharekhan.txt", true);
                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Notice.txt";
                    File.Copy(processtostart, "C:\\myshubhalabha\\samples\\Notice.txt", true);

                    regKey.SetValue("version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

                    savedata();
                    Environment.Exit(0);


                }
                catch
                {
                }


            }

                //copy xls file every time in sample folder 
               try
               {
                   string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                   string processtostart = filepath.Substring(0, filepath.Length - 18) + "sharekhantoami.xlsm";
                   File.Copy(processtostart, "C:\\myshubhalabha\\samples\\sharekhantoami.xlsm", true);



                   processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhaodin.xlsm";


                   File.Copy(processtostart, "C:\\myshubhalabha\\samples\\\\shubhaodin.xlsm", true);

                   processtostart = filepath.Substring(0, filepath.Length - 18) + "ExcelLogin.exe";

                   File.Copy(processtostart, "C:\\myshubhalabha\\samples\\\\ExcelLogin.exe", true);
                   File.Copy(processtostart, "C:\\myshubhalabha\\ExcelLogin.exe", true);



                   processtostart = filepath.Substring(0, filepath.Length - 18) + "MetaStockRefresher V 2.0.9 setup.exe";

                   File.Copy(processtostart, "C:\\myshubhalabha\\samples\\\\MetaStockRefresher V 2.0.9 setup.exe", true);

                   processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaNest-Now.xlsm";

                   File.Copy(processtostart, "C:\\myshubhalabha\\samples\\\\ShubhaNest-Now.xlsm", true);
               }

               catch
               {

               }
                //load banner form server 
                try
                {
                    System.Net.WebRequest myRequest = System.Net.WebRequest.Create("http://www.Google.co.in");
                    System.Net.WebResponse myResponse = myRequest.GetResponse();


                    Uri a = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=18&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                    Uri a1 = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=17&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                    Uri a2 = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=17&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                    Uri a3 = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=17&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                    Uri tradetigerbanner1 = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=25&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                    Uri tipofday = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=26&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                    Uri homepage = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=24&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                   
                    Uri piwikvisit = new System.Uri("http://list.shubhalabha.in/withoutrtd.html");

                    wad1.Source = a1;

                    wad2.Source = a2;
                    wad3.Source = a3;

                    wad4.Source = a1;

                    wad5.Source = a2;
                    wad6.Source = a3;

                    wad7.Source = a1;

                    wad8.Source = a2;
                    wad9.Source = a3;
                    wad10.Source = a1;

                    wad11.Source = a2;
                    wad12.Source = a3;

                    homepageadd.Source = homepage;
                    tipoftheday.Source = tipofday;

                    piwik.Source = piwikvisit;
                    //  wad4.Source = a4;


                }
                catch
                {


                    wad1.Visibility = Visibility.Hidden;
                    wad2.Visibility = Visibility.Hidden;
                    wad3.Visibility = Visibility.Hidden;
                    wad4.Visibility = Visibility.Hidden;
                    wad5.Visibility = Visibility.Hidden;
                    wad6.Visibility = Visibility.Hidden;
                    wad7.Visibility = Visibility.Hidden;
                    wad8.Visibility = Visibility.Hidden;
                    wad9.Visibility = Visibility.Hidden;

                }

             //   System.IO.StreamReader objReader = new StreamReader("C:\\myshubhalabha\\Notice.txt");



                RTD_server_name.Items.Add("NEST");
                RTD_server_name.Items.Add("NOW");


                Format_cb.Items.Add("Amibroker");
                Format_cb.Items.Add("Metastock");
                Format_cb.Items.Add("Fchart");

                Format_cb.SelectedIndex = 0;
                chartonbackfill.Items.Add("Amibroker");
                chartonbackfill.Items.Add("Metastock");
                chartonbackfill.Items.Add("Fchart");




                chartonbackfill.SelectedIndex = 0;
                if (chartingapp == "Amibroker")
                {
                    Format_cb.SelectedIndex = 0;

                }
                else if (chartingapp == "Metastock")
                {
                    Format_cb.SelectedIndex = 1;

                }
                else if (chartingapp == "Fchart")
                {
                    Format_cb.SelectedIndex = 2;

                }

                if (chartingappbackfill  == "Amibroker")
                {
                    chartonbackfill.SelectedIndex = 0;

                }
                else if (chartingappbackfill  == "Metastock")
                {
                    chartonbackfill.SelectedIndex = 1;

                }
                else if (chartingappbackfill  == "Fchart")
                {
                    chartonbackfill.SelectedIndex = 2;

                }


                int i1 = 1;
                timetoRT.Items.Add(i1 );
                i1 = 3;
                timetoRT.Items.Add(i1);
                i1 = 5;
                timetoRT.Items.Add(i1);

                i1 = 10;
                timetoRT.Items.Add(i1);

                i1 = 20;
                timetoRT.Items.Add(i1);
                i1 = 30;
                timetoRT.Items.Add(i1);
                var timesec = regKey.GetValue("timesec");
                if (timesec == null || timesec == "")
                {
                    timetoRT.SelectedIndex = 2;
                }
                else
                {
                    timetoRT.SelectedIndex = Convert.ToInt32(timesec);
                }
                //copy files into system32 folder
                try
                {

                    string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                    string processtostart = filepath.Substring(0, filepath.Length - 18) + "asc2ms.exe";

                    File.Copy(processtostart, "C:\\asc2ms.exe", true);
                    File.Copy(processtostart, "C:\\Windows\\System32\\asc2ms.exe", true);


                    processtostart = filepath.Substring(0, filepath.Length - 18) + "pthread.dll";

                    File.Copy(processtostart, "C:\\pthread.dll", true);
                    File.Copy(processtostart, "C:\\Windows\\System32\\pthread.dll", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "pthreadGC2.dll";

                    File.Copy(processtostart, "C:\\Windows\\System32\\pthreadGC2.dll", true);



                }
                catch
                {
                }

                try
                {
                    if (chartingapp == null)
                    {
                        Format_cb.SelectedIndex = 0;

                    }
                    else
                    {
                        Format_cb.SelectedItem = chartingapp;
                    }
                   

                }
                catch
                {
                }


            }


        }
        //Removing scroll bar from webbrowser
        void wb_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            System.Windows.Controls.WebBrowser wb = (System.Windows.Controls.WebBrowser)sender;
            wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }
        //starting real time data feed 
        private void StartRT_Click(object sender, RoutedEventArgs e)
        {
          
        }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        public static IntPtr FindChildWindow(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszTitle)
        {
            // Try to find a match.
            IntPtr hwnd = FindWindowEx(hwndParent, IntPtr.Zero, lpszClass, lpszTitle);
            if (hwnd == IntPtr.Zero)
            {
                // Search inside the children.
                IntPtr hwndChild = FindWindowEx(hwndParent, IntPtr.Zero, null, null);
                while (hwndChild != IntPtr.Zero && hwnd == IntPtr.Zero)
                {
                    hwnd = FindChildWindow(hwndChild, IntPtr.Zero, lpszClass, lpszTitle);
                    if (hwnd == IntPtr.Zero)
                    {
                        // If we didn't find it yet, check the next child.
                        hwndChild = FindWindowEx(hwndParent, hwndChild, null, null);
                    }
                }
            }
            return hwnd;
        }


        //Checking all require fields in nest terminal 
        public void checknestfiled()
        {
            Process[] processes = Process.GetProcessesByName("NestTrader");
            IntPtr abcd = new IntPtr();
            IntPtr abcd1 = new IntPtr();
            IntPtr windowHandle = new IntPtr();
            List<Thread> processtostartback = new List<Thread>();

            SystemAccessibleObject sao;
            //taking nest process and finds window handle for that 
            foreach (Process p in processes)
            {
                windowHandle = p.MainWindowHandle;

                //System.Windows.Forms.MessageBox.Show(p.HandleCount.ToString());

                abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");


               
            }


            SystemWindow a = new SystemWindow(abcd1);
            try
            {
                //checking marketwatch present or not 
                string marketwatch = abcd1.ToString();
                if (marketwatch == "0")
                {
                   
                    System.Windows.MessageBox.Show("Please check either your NEST is not running or Market Watch is not present", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


                }
                sao = SystemAccessibleObject.FromWindow(a, AccessibleObjectID.OBJID_WINDOW);


}
            catch
            {
                
                System.Windows.MessageBox.Show("Market Watch not found in your trading terminal", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                return;
            }

            SystemAccessibleObject finalobject;
            SystemAccessibleObject f = sao.Children[3];
            finalobject = f.Children[0];
            string s1 = finalobject.Description;
                      int flag = 0;
            string[] checkterminalcol = s1.Split(',');
            string marketwathrequiredfield = "";
           
            for (int i = 0; i < checkterminalcol.Count(); i++)
            {
                marketwathrequiredfield = marketwathrequiredfield + checkterminalcol[i].ToString();
            }

            //checking all required fileds of nest terminal 
            if (!marketwathrequiredfield.Contains("LTT"))
            {
                flag = 1;

            }
            if (!marketwathrequiredfield.Contains("LTP"))
            {
                flag = 1;

            }
            if (!marketwathrequiredfield.Contains("Volume Traded Today"))
            {
                flag = 1;

            }
            if (!marketwathrequiredfield.Contains("Open Interest"))
            {
                flag = 1;

            }
            if (!checkterminalcol[0].Contains("LTT"))
            {
                flag = 1;

            }
            if (flag == 1)
            {
                System.Windows.MessageBox.Show("One or more columns are missing in the order as below!\n Trading symbol , LTT , LUT , LTP , Volume Traded Today ,Open Interest. ", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
               
                closeallprocess();
            }
        }


        //Close all running process of the application 
        public void closeallprocess()
        {

            Process[] workers = Process.GetProcessesByName("shubhalabhartx.vshost");
            Process[] workers1 = Process.GetProcessesByName("shubhalabhartx");
            Process[] workers2 = Process.GetProcessesByName("Endrt.vshost");
            Process[] workers3 = Process.GetProcessesByName("Endrt");
            Process[] workers4 = Process.GetProcessesByName("Broker");
           



            foreach (Process worker in workers4)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
            foreach (Process worker in workers3)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
            foreach (Process worker in workers2)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
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
            Environment.Exit(0);
        }

        //Save data 
        public void savedata()
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("timesec", timetoRT.SelectedIndex.ToString());
            regKey.SetValue("googleday", googledays.SelectedItem.ToString());
            regKey.SetValue("googletime", googletime.SelectedItem.ToString());


            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("terminalname");
            config.AppSettings.Settings.Add("terminalname", RTD_server_name.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

          


            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("chartingapp");

            config.AppSettings.Settings.Add("chartingapp", Format_cb .SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("chartingappbackfill");

            config.AppSettings.Settings.Add("chartingappbackfill", chartonbackfill.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("nestbackfill");

            config.AppSettings.Settings.Add("nestbackfill", nestbackfill.IsChecked.ToString());
            config.Save(ConfigurationSaveMode.Full);

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("googlebackfill");

            config.AppSettings.Settings.Add("googlebackfill", googlebackfill.IsChecked.ToString());
            config.Save(ConfigurationSaveMode.Full);

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("withoutbackfill");

            config.AppSettings.Settings.Add("withoutbackfill", withoutbackfill.IsChecked.ToString());
            config.Save(ConfigurationSaveMode.Full);

           
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("targetpathforcombo");

            config.AppSettings.Settings.Add("targetpathforcombo", txtTargetFolder.Text);
            config.Save(ConfigurationSaveMode.Full);
           
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("terminalname");

            config.AppSettings.Settings.Add("terminalname", RTD_server_name.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");


            config.AppSettings.Settings.Remove("googlemin");

            config.AppSettings.Settings.Add("googlemin", googletime.SelectedIndex.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("googleday");

            config.AppSettings.Settings.Add("googleday", googledays.SelectedIndex.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

           

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(Window1  ));
            log.Debug("Application Close  ");
            savedata();
            
           // closeallprocess();
            Environment.Exit(0);
        }
        //open dialog box and save path
        private void btnTarget_Click(object sender, RoutedEventArgs e)
        {
            var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;



               
                ConfigurationManager.RefreshSection("appSettings");
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("targetpathforcombo");

                config.AppSettings.Settings.Add("targetpathforcombo", txtTargetFolder.Text);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
                
            }
        }

        //Import symbol from NEST or NOW terminal
        private void importsymbol()
        {


            listView2.Items.Clear();
            
        
            string terminalname = ConfigurationManager.AppSettings["terminalname"];
            Process[] processes = null;


            if (nestbackfill.IsChecked == true || RTmapsymbol.IsChecked==true )
            {
                int countfortotalsym=0;
                string[] name = null;
                //Show already saved symbol list 
                




               //checking nest running as admin or not 
                if (terminalname == "NEST")
                {
                    try
                    {
                        type = Type.GetTypeFromProgID("nest.scriprtd");

                        m_server = (IRtdServer)Activator.CreateInstance(type);
                        processes = Process.GetProcessesByName("NestTrader");


                    }
                    catch
                    {
                        System.Windows.MessageBox.Show(" Please start Nest as 'Run as Administrator'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                        nestbackfill.IsChecked = false;
                        RTmapsymbol.IsChecked = false; 
                        return;
                    }
                }
                if (terminalname == "NOW")
                {
                    try
                    {
                        type = Type.GetTypeFromProgID("now.scriprtd");

                        m_server = (IRtdServer)Activator.CreateInstance(type);
                        processes = Process.GetProcessesByName("NOW");

                    }
                    catch
                    {
                        System.Windows.MessageBox.Show(" Please start Nest as 'Run as Administrator'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                        nestbackfill.IsChecked = false;
                        
                        return;
                    }
                } IntPtr abcd = new IntPtr();
                IntPtr abcd1 = new IntPtr();
                IntPtr windowHandle = new IntPtr();



                List<Thread> processtostartback = new List<Thread>();
                SystemAccessibleObject sao, f;

                foreach (Process p in processes)
                {
                    windowHandle = p.MainWindowHandle;


                    abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                    abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");


                    // do something with windowHandle
                }

                SystemWindow a = new SystemWindow(abcd1);


                // sao = SystemAccessibleObject.FromPoint(4, 200);
                try
                {
                    string marketwatch = abcd1.ToString();
                    if (marketwatch == "0")
                    {
                        System.Windows.MessageBox.Show("Either your NEST is not running or Market Watch is not found ", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                        return;
                        //closeallprocess();

                    }
                    sao = SystemAccessibleObject.FromWindow(a, AccessibleObjectID.OBJID_WINDOW);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Market Watch is not found in your trading terminal", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                    return;
                }



                f = sao.Children[3];
                int flagforsymbolpresentornot = 0;
                //add symbols into listview 
                try
                {
                    for (int i = 0; i < f.Children.Count() - 1; i++)
                    {
                        flagforsymbolpresentornot = 0;
                        try
                        {

                            using (var reader = new StreamReader("C:\\myshubhalabha\\Symbolname.csv"))
                            {
                                string line = null;

                                while ((line = reader.ReadLine()) != null)
                                {
                                    name = line.Split(',');

                                    if (f.Children[i].Name == name[0])
                                   {
                                       listView2.Items.Add(new ListViewData(name[0], name[1], name[2]));
                                       flagforsymbolpresentornot = 1;
                                       break;
                                   }
                                    


                                }
                            }
                        }
                        catch
                        {
                        }

                                if (flagforsymbolpresentornot == 0)
                                {
                                    listView2.Items.Add(new ListViewData(f.Children[i].Name, ":Enter google symbol", f.Children[i].Name));
                                }
                       

                       
                    }


                }
                catch
                {
                }
            }
            else if (googlebackfill.IsChecked == true)
            {
                //add symbols into listview 
                int countfortotalsym = 0;
                string[] name = null;
                //Show already saved symbol list 
                try
                {
                    if (File.Exists("C:\\myshubhalabha\\GoogleSymbolname.csv"))
                    {
                    using (var reader = new StreamReader("C:\\myshubhalabha\\GoogleSymbolname.csv"))
                    {
                        string line = null;

                        while ((line = reader.ReadLine()) != null)
                        {
                            name = line.Split(',');

                            listView2.Items.Add(new ListViewData(name[0], name[1], name[2]));
                            countfortotalsym++;


                        }
                    }
                    }
                    else
                    {

                for (int i = 0; i < 50; i++)
                {
                    listView1.Items.Add(new ListViewData("NOTNEEDED", ":Enter google symbol", "NO"));

                }

                    }
                }
                catch
                {
                }


            }
            else
            {
                System.Windows.MessageBox.Show("Please select atleast one checkbox option from backfill ", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

            }


        }

       
        //Selected listview symbol name Editing 
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //edit data in listview 
            ListViewData lvc = (ListViewData)listView1.SelectedItem;
            if (lvc != null)
            {
                stopRefreshControls = true;
                textBox1.Text = lvc.Col1;
                textBox2.Text = lvc.Col2;
                textBox3.Text = lvc.Col3;
                stopRefreshControls = false;
              
            }
            textBox3.Focus();

        }

        //Editing of symbol name
        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            //save data into listview 
            RefreshListView(textBox1.Text, textBox2.Text,textBox3.Text );
        }
        private void setDataChanged(bool value)
        {
            dataChanged = value;
        }
        private void RefreshListView(string value1, string value2, string value3)
        {
            //add editaed data into listview 
            ListViewData lvc = (ListViewData)listView1.SelectedItem; //new ListViewClass(value1, value2);
            if (lvc != null && !stopRefreshControls)
            {

                lvc.Col1 = value1;
                lvc.Col2 = value2;
                lvc.Col3 = value3;

                listView1.Items.Refresh();
            }
            lvc = (ListViewData)listView2.SelectedItem; //new ListViewClass(value1, value2);
            if (lvc != null && !stopRefreshControls)
            {

                lvc.Col1 = value1;
                lvc.Col2 = value2;
                lvc.Col3 = value3;

                listView2.Items.Refresh();
            }
        }

        //Editing of symbol name
        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListView(textBox1.Text, textBox2.Text, textBox3.Text);

        }

        //Save symbol into File 
        private void sav_symbolfile_Click(object sender, RoutedEventArgs e)
        {
            //save data into txt file 
            if (nestbackfill.IsChecked == true || RTmapsymbol.IsChecked == true)
            {
                string finameformap = "C:\\myshubhalabha\\Symbolname.csv";
                MyData md = new MyData();
                md.Save(listView2.Items, finameformap);
                setDataChanged(false);
                System.Windows.MessageBox.Show("Symbol file saved ", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

           
            }
            if (googlebackfill .IsChecked == true)
            {
                string finameformap = "C:\\myshubhalabha\\GoogleSymbolname.csv";

                MyData md = new MyData();
                md.Save(listView1.Items, finameformap);
                setDataChanged(false);
                System.Windows.MessageBox.Show("Symbol file saved ", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);


            }

        }
        public void saveradiobuttn()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("tradetiger");

            config.AppSettings.Settings.Add("tradetiger", tradetiger.IsChecked.ToString());
            config.Save(ConfigurationSaveMode.Full);


            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("nestbackfill");

            config.AppSettings.Settings.Add("nestbackfill", nestbackfill.IsChecked.ToString());
            config.Save(ConfigurationSaveMode.Full);

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("googlebackfill");

            config.AppSettings.Settings.Add("googlebackfill", googlebackfill.IsChecked.ToString());
            config.Save(ConfigurationSaveMode.Full);

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("withoutbackfill");

            config.AppSettings.Settings.Add("withoutbackfill", withoutbackfill.IsChecked.ToString());
            config.Save(ConfigurationSaveMode.Full);
        }

        private void withoutbackfill_Checked(object sender, RoutedEventArgs e)
        {
            saveradiobuttn();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("NestTrader");

            IntPtr abcd = new IntPtr();
            IntPtr abcd1 = new IntPtr();
            IntPtr abcd2 = new IntPtr();
            IntPtr windowHandle = new IntPtr();

            List<Thread> processtostartback = new List<Thread>();


            foreach (Process p in processes)
            {
                windowHandle = p.MainWindowHandle;

                //System.Windows.Forms.MessageBox.Show(p.HandleCount.ToString());

                abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770 (Dialog)", "");

                abcd1 = FindChildWindow(abcd, IntPtr.Zero, null, "DataTable : TCS-EQ");
                // SetForegroundWindow(abcd1);
                abcd2 = FindChildWindow(abcd1, IntPtr.Zero, "SysListView32", null);
                SystemWindow table = new SystemWindow(abcd2);

                SystemAccessibleObject datatable = SystemAccessibleObject.FromWindow(table, AccessibleObjectID.OBJID_WINDOW);
                datatable = datatable;

                // do something with windowHandle
            }
          

        }

        //Editing of symbol name

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tradetiger_Checked(object sender, RoutedEventArgs e)
        {
            saveradiobuttn();
        }

        //Load all current setting of the user 
        private void Current_Setting_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                var terminalname = regKey.GetValue("terminal");
                var Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath");
                var Metastockdatapath = regKey.GetValue("Metastockdatapath");
                var Chartingapplication = regKey.GetValue("Chartingapplication");
                var Fchartdatapath = regKey.GetValue("fchart");
               

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
                result_metapath.Content = Metastockdatapath.ToString()+"\\Metastock";
                fchartpath.Content = Fchartdatapath.ToString()+"\\Fchart";
                nestnowbackfill.Content = backfill;
                System.OperatingSystem osInfo2 = System.Environment.OSVersion;
                string result = string.Empty;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
                foreach (ManagementObject os in searcher.Get())
                {
                    result = os["Caption"].ToString();
                    if (IntPtr.Size == 8)
                    {
                        result = result + " -64 bit ";

                    }
                    else
                    {
                        result = result + " -32 bit ";

                    }

                    break;
                }
                osversition.Content = result.ToString();

            }
            catch
            {
            }
        }

        //start backfill data 
        private void StartRT_Click_1(object sender, RoutedEventArgs e)
        {

            Process[] workers = Process.GetProcessesByName("Broker");
            Process[] workers1 = Process.GetProcessesByName("shubhalabhartx");


            if (workers.Count() >= 1)
            {

                System.Windows.MessageBox.Show("Amibroker instance is already running please close it", "Important Note", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


                return;
            }

            if (workers1.Count() >1)
            {

                System.Windows.MessageBox.Show("One or more Shubha Real Time Combo instance is already running please close it", "Important Note", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


                return;
            }



                //save mapping symbol name file 
                try
                {
                    if (nestbackfill.IsChecked == true || RTmapsymbol.IsChecked == true)
                    {
                        string finameformap = "C:\\myshubhalabha\\Symbolname.csv";
                        MyData md = new MyData();
                        md.Save(listView2.Items, finameformap);
                        setDataChanged(false);

                    }
                    if (googlebackfill.IsChecked == true)
                    {
                        string finameformap = "C:\\myshubhalabha\\GoogleSymbolname.csv";

                        MyData md = new MyData();
                        md.Save(listView1.Items, finameformap);
                        setDataChanged(false);

                    }
                }
                catch
                {

                }


                savedata();

                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                var terminalname = regKey.GetValue("terminal");
                var Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath");
                regKey.SetValue("googleday", googledays.SelectedItem.ToString());
                regKey.SetValue("googletime", googletime.SelectedItem.ToString());


                //if trade tiger backfill 
                if (tradetiger.IsChecked == true)
                {
                    Type ExcelType;
                    object ExcelInst;
                    object[] args = new object[3];

                    string[] sharekhanfilePaths = Directory.GetFiles("C:\\myshubhalabha\\sharekhan", "*.csv", SearchOption.TopDirectoryOnly);
                    ExcelType = Type.GetTypeFromProgID("Broker.Application");
                    ExcelInst = Activator.CreateInstance(ExcelType);
                    ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                              ExcelInst, new object[1] { true });
                    ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                         ExcelInst, new string[1] { Amibrokerdatapath.ToString() });

                    for (int i = 0; i < sharekhanfilePaths.Count(); i++)
                    {
                        args[0] = Convert.ToInt16(0);
                        args[1] = sharekhanfilePaths[i];
                        args[2] = "Shubhasharekhan.format";


                        ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                                  ExcelInst, args);
                    }


                    return;

                }

                if (nestbackfill.IsChecked == true)
                {


                    try
                    {
                        foreach (var file in Directory.GetFiles("C:\\myshubhalabha\\NESTbackfill"))
                            File.Delete(file);

                    }
                    catch
                    {
                    }

                    Shubharealtime.datadownload s = new datadownload();



                    Shubharealtime.datadownload s1 = new datadownload();
                    //if (RTD_server_name.SelectedItem == "NEST")
                    //{
                    //    int result = s.checknestfiled();
                    //    if (result == 0)
                    //    {
                    //        return;
                    //    }
                    //}
                    //if (RTD_server_name.SelectedItem == "NOW")
                    //{
                    //    int result = s.checknowfiled ();
                    //    if (result == 0)
                    //    {
                    //        return;
                    //    }
                    //}

                    if (txtTargetFolder.Text == "")
                    {

                        txtTargetFolder.Focus();
                        return;

                    }




                    System.Windows.MessageBox.Show("Please DO NOT USE YOUR COMPUTER for few minutes .\n As application is pulling backfill data and processing files.", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);



                    CommandManager.InvalidateRequerySuggested();
                }
                try
                {


                    //   type = Type.GetTypeFromProgID("nest.scriprtd");



                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


                    config.AppSettings.Settings.Remove("targetpathforcombo");

                    config.AppSettings.Settings.Add("targetpathforcombo", txtTargetFolder.Text);
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                    config.AppSettings.Settings.Remove("format");

                    config.AppSettings.Settings.Add("format", Format_cb.SelectedItem.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                    config.AppSettings.Settings.Remove("terminal");

                    config.AppSettings.Settings.Add("terminal", RTD_server_name.SelectedItem.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                    config.AppSettings.Settings.Remove("interval");

                    config.AppSettings.Settings.Add("interval", timetoRT.SelectedItem.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                    //SystemAccessibleObject sao = SystemAccessibleObject.FromPoint(4, 200);
                    // LoadTree(sao);
                }
                catch
                {


                }

                string terminal = ConfigurationManager.AppSettings["terminal"];


                string path = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                string pathtostartprocess = path.Substring(0, path.Length - 18);
                // System.Diagnostics.Process.Start(pathtostartprocess + "Endrt.exe");
                Shubharealtime.datadownload s2 = new datadownload();

                //check required field from nest or now terminal 
                //if(nestbackfill.IsChecked==true )
                //{
                //if (RTD_server_name.SelectedItem == "NEST")
                //{
                //    s2.checknestfiled();
                //}
                //if (RTD_server_name.SelectedItem == "NOW")
                //{
                //    s2.checknowfiled();
                //}
                //}
                Task.Factory.StartNew(s2.startdownload);
                // this.Hide();




                CommandManager.InvalidateRequerySuggested();
            
        }
        //Srtart wazart again 
        private void changesetting_Click(object sender, RoutedEventArgs e)
        {
           
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("Wizart", "notdone");
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();

           
        }

        private void listView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //edit list view data
            ListViewData lvc = (ListViewData)listView2.SelectedItem;
            if (lvc != null)
            {
                stopRefreshControls = true;
                textBox1.Text = lvc.Col1;
                textBox2.Text = lvc.Col2;
                textBox3.Text = lvc.Col3;
                stopRefreshControls = false;

            }
            textBox3.Focus();
        }

        private void googlebackfill_Checked(object sender, RoutedEventArgs e)
        {
           
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";

                lblgooggleday.Visibility = Visibility.Visible;
                lblgoogletime.Visibility = Visibility.Visible;
                googletime.Visibility = Visibility.Visible;
                googledays.Visibility = Visibility.Visible;
                textBox1.Visibility = Visibility.Hidden;
                textBox2.Visibility = Visibility.Visible;
                listView1.Visibility = Visibility.Visible;
                listView2.Visibility = Visibility.Hidden;
                Shubhalabha123.Tradetigerinformation t = new Shubhalabha123.Tradetigerinformation();
                tradetigerinfo.Children.Add(t);
                tradetigerinfo.Visibility = Visibility.Visible;
                chartonbackfill.Visibility = Visibility.Visible;
                label1.Visibility = Visibility.Visible;
                StartRT.Visibility = Visibility.Visible;

                saveradiobuttn();
                importsymbol();
           
        }

        private void nestbackfill_Checked(object sender, RoutedEventArgs e)
        {

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                lblgooggleday.Visibility = Visibility.Hidden;
                lblgoogletime.Visibility = Visibility.Hidden;
                googletime.Visibility = Visibility.Hidden;
                googledays.Visibility = Visibility.Hidden;
                textBox1.Visibility = Visibility.Visible;
                textBox2.Visibility = Visibility.Hidden;
                listView1.Visibility = Visibility.Hidden;
                listView2.Visibility = Visibility.Visible;


                tradetigerinfo.Visibility = Visibility.Hidden;
                saveradiobuttn();
                importsymbol();
            

        }

        private void tradetiger_Checked_1(object sender, RoutedEventArgs e)
        {

            lblgooggleday.Visibility = Visibility.Hidden;
            lblgoogletime.Visibility = Visibility.Hidden;
            googletime.Visibility = Visibility.Hidden;
            googledays.Visibility = Visibility.Hidden;
            //add user control 
            Shubhalabha123.Tradetigerinformation t=new Shubhalabha123.Tradetigerinformation();
            tradetigerinfo.Children.Add(t);
            tradetigerinfo.Visibility = Visibility.Visible;
        }




        //start realtime data feed
        private void Startrealtimeonly_Click(object sender, RoutedEventArgs e)
        {
            //check if any other instance is running 
            Process[] workers = Process.GetProcessesByName("Broker");
            Process[] workers1 = Process.GetProcessesByName("shubhalabhartx");


            if(workers.Count()>=1)
            {

                System.Windows.MessageBox.Show("Amibroker instance is already running please close it", "Important Note", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


                return;
            }

            if (workers1.Count() > 1)
            {

                System.Windows.MessageBox.Show("One or more Shubha Real Time Conmbo instance is already running please close it", "Important Note", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


                return;
            }



            savedata();

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("timesec", timetoRT.SelectedIndex  .ToString());
            var ApplicationID = regKey.GetValue("sd");

            Shubharealtime.datadownload s1 = new datadownload();
            if (RTD_server_name.SelectedItem == "NEST")
            {
                try
                {
                    type = Type.GetTypeFromProgID("nest.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    m_server.ServerTerminate();
                }
                catch
                {

                    System.Windows.MessageBox.Show("Please start NEST as 'Run as Administrator'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                    return;
                }
            }
            if (RTD_server_name.SelectedItem == "NOW")
            {
                try
                {
                    type = Type.GetTypeFromProgID("now.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    m_server.ServerTerminate();
                }
                catch
                {
                    System.Windows.MessageBox.Show("Please start NOW as 'Run as Administrator'", "Important Note", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


                    return;
                }
            }

            if (txtTargetFolder.Text == "")
            {
                
                txtTargetFolder.Focus();
                return;

            }


            CommandManager.InvalidateRequerySuggested();

            try
            {


                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


                config.AppSettings.Settings.Remove("targetpathforcombo");

                config.AppSettings.Settings.Add("targetpathforcombo", txtTargetFolder.Text);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");

                config.AppSettings.Settings.Remove("format");

                config.AppSettings.Settings.Add("format", Format_cb.SelectedItem.ToString());
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");

                config.AppSettings.Settings.Remove("terminal");

                config.AppSettings.Settings.Add("terminal", RTD_server_name.SelectedItem.ToString());
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");

                config.AppSettings.Settings.Remove("interval");

                config.AppSettings.Settings.Add("interval", timetoRT.SelectedItem.ToString());
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");


                config.AppSettings.Settings.Remove("googlemin");

                config.AppSettings.Settings.Add("googlemin", googletime.SelectedIndex .ToString());
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");

                config.AppSettings.Settings.Remove("googleday");

                config.AppSettings.Settings.Add("googleday", googledays.SelectedIndex.ToString());
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");



                //SystemAccessibleObject sao = SystemAccessibleObject.FromPoint(4, 200);
                // LoadTree(sao);
            }
            catch
            {


            }

           

            Shubharealtime.datadownload s = new datadownload();
            if (RTD_server_name.SelectedItem == "NEST")
            {
              int result=  s.checknestfiled();
                if(result==0)
                {
                    return;
                }
            }
            if (RTD_server_name.SelectedItem == "NOW")
            {
                int result = s.checknowfiled();
                if (result == 0)
                {
                    return;
                }
              
            }
            string terminal = ConfigurationManager.AppSettings["terminal"];
            //it start another window and hide this window 
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            string pathtostartprocess = path.Substring(0, path.Length - 18);
            System.Diagnostics.Process.Start(pathtostartprocess + "Endrt.exe");
            Task.Factory.StartNew(s.startRealtime);
            this.Hide();
        }

        private void chartonbackfill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("chartingappforbackfill", chartonbackfill.SelectedItem.ToString());
        }

        private void RTmapsymbol_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            
            lblgooggleday.Visibility = Visibility.Hidden;
            lblgoogletime.Visibility = Visibility.Hidden;
            googletime.Visibility = Visibility.Hidden;
            googledays.Visibility = Visibility.Hidden;
            textBox1.Visibility = Visibility.Visible;
            textBox2.Visibility = Visibility.Hidden;
            listView1.Visibility = Visibility.Hidden;
            listView2.Visibility = Visibility.Visible;
            chartonbackfill.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
            StartRT.Visibility = Visibility.Hidden;
            tradetigerinfo.Visibility = Visibility.Hidden;
            saveradiobuttn();
            importsymbol();
        }

        private void googletime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            googledays.Items.Clear();
            if (googletime.SelectedIndex == 0)
            {
                for (int i = 1; i < 11;i++ )
                {
                    googledays.Items.Add(i );
                }

                googledays.SelectedIndex = 5;

            }
            else if (googletime.SelectedIndex == 1)
            {
                for (int i = 1; i < 51; i++)
                {
                    googledays.Items.Add(i);
                }
                googledays.SelectedIndex = 4;

            }

        }

        private void log_Checked(object sender, RoutedEventArgs e)
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("logcheck");

            config.AppSettings.Settings.Add("logcheck", log.IsChecked.Value.ToString());
            config.Save(ConfigurationSaveMode.Full);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (nestbackfill.IsChecked == true || RTmapsymbol.IsChecked == true)
            {
                listView2.Items.Clear();

                string finameformap = "C:\\myshubhalabha\\Symbolname.csv";
                File.Delete(finameformap);
                listView2.Items.Refresh();
                importsymbol();

                System.Windows.MessageBox.Show("Mapping symbol removed", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);


            }
            if (googlebackfill.IsChecked == true)
            {
                string finameformap = "C:\\myshubhalabha\\GoogleSymbolname.csv";
                File.Delete(finameformap);
                listView1.Items.Refresh();
                listView1.Items.Clear();
                importsymbol();
                 
                System.Windows.MessageBox.Show("Mapping symbol removed", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

              

            }
        }

      
      
    }
}
