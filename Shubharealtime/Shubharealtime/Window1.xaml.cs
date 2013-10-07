﻿//////////////////////////////////////////////////
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

        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
        private void SendMail(string p_sEmailTo, string subject, string messageBody, bool isHtml)
        {
            var fromAddress = new MailAddress("shanteshpaigude1988@gmail.com", "From Name");
            var toAddress = new MailAddress("shanteshpaigude1988@gmail.com", "To Name");
            subject = "Your Password";
            string body = "This is your password:" + subject + "\n ";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = false ,
                DeliveryMethod = SmtpDeliveryMethod.Network,
               UseDefaultCredentials = true  ,
                Credentials = new NetworkCredential(fromAddress.Address, "lionking@143")
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }


        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ///////////////////////////////
            //expiration

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-temp\");

            try
            {
                var registerdate = regKey.GetValue("sd");
                var paidornot = regKey.GetValue("sp");

                DateTime reg = Convert.ToDateTime(registerdate);
                reg = reg.AddDays(3);


                if (reg < DateTime.Today.Date)
                {
                    Uri a = new System.Uri("http://besttester.com/lic/lic.html");

                    // webBrowser1.Source = a;
                    string credentials = "liccheck:lic123!@#";
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(a);
                    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));
                    request.PreAuthenticate = true;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    // System.Windows.MessageBox.Show(reader.ReadToEnd());

                    //System.Windows.MessageBox.Show(reader.ReadToEnd());

                    string a1 = reader.ReadToEnd();
                  //  System.Windows.MessageBox.Show(a1);
                    //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                    //ManagementObjectCollection moc = mos.Get();
                    string motherBoard = "";
                    //foreach (ManagementObject mo in moc)
                    //{
                    //  motherBoard = (string)mo["SerialNumber"];
                    // //  motherBoard = (string)mo["VolumeSerialNumber"];

                        
                    //}
                    System.Windows.MessageBox.Show ("Cheking Trail ");
                    ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                    dsk.Get();
                    string id = dsk["VolumeSerialNumber"].ToString();
                    motherBoard = id;
                    System.Windows.MessageBox.Show("Cheking done ");

                    if (a1.Contains(motherBoard))
                    {
                        

                        //if motherboard id is not present then it is paid user continue 
                    }
                    else
                    {
                        //if motherboard id is not present on server then close all process 
                       System.Windows.Forms.   MessageBox.Show("Trial version expired please contact to sales@shubhalabha.in ");
                       closeallprocess();
                    }

                }
                else
                {

                }


                //if (paidornot.ToString() == "Key for xp")
                //{
                //    if (reg < DateTime.Today.Date)
                //    {
                //     System.Windows.Forms.   MessageBox.Show("Trial version expired please contact to sales@shubhalabha.in ");
                //        this.Close();
                //        Environment.Exit(0);
                //        return;
                //    }
                //    else
                //    {

                //    }

                //}
                //else
                //{
                //    if (paidornot.ToString() == "1001")
                //    {
                //    }
                //    else
                //    {
                //        System.Windows.Forms.MessageBox.Show("Trial version expired please contact to sales@shubhalabha.in ");
                //        this.Close();
                //        return;
                //    }

                //}
            }
            catch(Exception ex) 
            {
              //  System.Windows.MessageBox.Show(ex.Message );
               System.Windows.MessageBox.Show("Please check internet connection we cant check registraion as your trail period expired ");
               // closeallprocess();
            }





            /////////////////////////////

            string targetpath = ConfigurationManager.AppSettings["targetpathforcombo"];
            string amipath = ConfigurationManager.AppSettings["amipath"];
            string terminalname=ConfigurationManager.AppSettings["terminalname"];
            string chartingapp = ConfigurationManager.AppSettings["chartingapp"];
            string timetosave = ConfigurationManager.AppSettings["timetoRT"];

            string googleday = ConfigurationManager.AppSettings["Daysforgoogle"];

            string google_time = ConfigurationManager.AppSettings["google_time_frame"];
            string nestback = ConfigurationManager.AppSettings["nestbackfill"];
            string tradetiger1 = ConfigurationManager.AppSettings["tradetiger"];

            string googleback = ConfigurationManager.AppSettings["googlebackfill"];
            string withoutback = ConfigurationManager.AppSettings["withoutbackfill"];

          
            ///////////////////////

          
            ////////////////////


           
            try
            {
                //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                //ManagementObjectCollection moc = mos.Get();
                //string motherBoard = "";
                //ManagementObjectCollection mbsList = null;
                //foreach (ManagementObject mo in moc)
                //{
                //    motherBoard = (string)mo["SerialNumber"];
                //}

                //ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
                //mbsList = mbs.Get();
                //string id = "";
                //foreach (ManagementObject mo in mbsList)
                //{
                //    id = mo["ProcessorID"].ToString();
                //}

                ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                dsk.Get();
                string id = dsk["VolumeSerialNumber"].ToString();
                moterboard.Text = id ;

                withoutbackfill.IsChecked = true;
                if (nestback == "True")
                {
                    nestbackfill.IsChecked = true;
                }
                if (tradetiger1 == "True")
                {
                    tradetiger.IsChecked = true;
                }
                if (googleback == "True")
                {
                    googlebackfill.IsChecked = true;
                }
                if (withoutback == "True")
                {
                    withoutbackfill.IsChecked = true;
                }





                var terminalfromwizart = regKey.GetValue("terminal");
                var Chartingappfromwizart = regKey.GetValue("chart");
                if (Chartingappfromwizart == "Amibroker")
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
                if (amipath != null)
                {
                    db_path.Text = amipath;

                }
                else
                {
                    db_path.Text = "C:\\myshubhalabha\\amirealtime";
                }
               
                if (!Directory.Exists(targetpath + "\\sharekhan"))
                {
                    Directory.CreateDirectory(targetpath + "\\sharekhan");
                }

                if (!Directory.Exists(targetpath + "\\odin"))
                {
                    Directory.CreateDirectory(targetpath + "\\odin");
                }
                if (!Directory.Exists(targetpath + "\\nest-now"))
                {
                    Directory.CreateDirectory(targetpath + "\\nest-now");
                }


            }
            catch
            {
            }
           

           
            
            try
            {

                string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                string processtostart = filepath.Substring(0, filepath.Length - 18) + "sharekhantoami.xlsm";

                File.Copy(processtostart, targetpath + "\\sharekhantoami.xlsm", true);
                if(!Directory.Exists("C:\\myshubhalabha\\amirealtime"))
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
                 File.Copy(processtostart, "C:\\shubhaodin.xlsm", true);

                 processtostart = filepath.Substring(0, filepath.Length - 18) + "ExcelLogin.exe";

                 File.Copy(processtostart, targetpath + "\\ExcelLogin.exe", true);
                 File.Copy(processtostart, "C:\\ExcelLogin.exe", true);
                 File.Copy(processtostart, "C:\\myshubhalabha\\ExcelLogin.exe", true);

                 processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhaxls.format";


                 File.Copy(processtostart,  "C:\\myshubhalabha\\amibroker format file\\shubhaxls.format", true);
                 File.Copy(processtostart,programfilepath+"\\AmiBroker\\Formats\\shubhaxls.format", true);

                 processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhasharekhan.format";


                 File.Copy(processtostart,  "C:\\myshubhalabha\\amibroker format file\\Shubhasharekhan.format", true);
                 File.Copy(processtostart, programfilepath+"\\AmiBroker\\Formats\\Shubhasharekhan.format", true);

                 processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhanest-now.format";
                 File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhanest-now.format", true);
                 File.Copy(processtostart, programfilepath+"\\AmiBroker\\Formats\\shubhanest-now.format", true);

                 processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaRt.format";
                 File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\ShubhaRt.format", true);
                 File.Copy(processtostart, programfilepath+"\\AmiBroker\\Formats\\ShubhaRt.format", true);

                 processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhabackfill.format";
                 File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhabackfill.format", true);
                 File.Copy(processtostart, programfilepath+"\\AmiBroker\\Formats\\Shubhabackfill.format", true);

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


                
                
            }
            catch
            {
            }
            for (int i = 0; i < 12; i++)
            {
                GHRS.Items.Add(i);
            }

            for (int i = 0; i < 60; i++)
            {
                GMIN.Items.Add(i);
            }
           

            //if (targetpath != null)
            //{
            //    txtTargetFolder.Text = targetpath;
            //}
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create("http://www.Google.co.in");
                System.Net.WebResponse myResponse = myRequest.GetResponse();


                Uri a = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=18&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                Uri a1 = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=17&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                Uri a2 = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=17&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                Uri a3 = new System.Uri("http://shubhalabha.in/eng/ads/www/delivery/afr.php?zoneid=17&amp;target=_blank&amp;cb=INSERT_RANDOM_NUMBER_HERE");
                Uri piwikvisit = new System.Uri("http://list.shubhalabha.in/withoutrtd.html");
                
                wad1.Source = a1;

                wad2.Source = a2;
                wad3.Source = a3;
                piwik.Source  = piwikvisit;
                //  wad4.Source = a4;


            }
            catch
            {


                wad1.Visibility = Visibility.Hidden;
                wad2.Visibility = Visibility.Hidden;
                wad3.Visibility = Visibility.Hidden;

            }
            
            
            
            RTD_server_name.Items.Add("NEST");
            RTD_server_name.Items.Add("NOW");

          
            Format_cb.Items.Add("Amibroker");
            Format_cb.Items.Add("Metastock");
            Format_cb.Items.Add("Fchart");

            Format_cb.SelectedIndex = 0;

            for (int i = 1; i < 60; i++)
            {
                timetoRT.Items.Add(i);
            }

            try
            {

                string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                string processtostart = filepath.Substring(0, filepath.Length - 18) + "asc2ms.exe";

                File.Copy(processtostart,"C:\\asc2ms.exe", true);
                File.Copy(processtostart, "C:\\Windows\\System32\\asc2ms.exe", true);


                processtostart = filepath.Substring(0, filepath.Length - 18) + "pthread.dll";

                File.Copy(processtostart,"C:\\pthread.dll", true);
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
                    Format_cb.SelectedItem  = chartingapp;
                }
                if (timetosave == null)
                {
                    timetoRT.SelectedIndex = 0;

                }
                else
                {
                    timetoRT.SelectedIndex = Convert.ToInt32(timetosave) - 1;
                }
               
            }
            catch
            {
            }


        }



        void wb_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string script = "document.body.style.overflow ='hidden'";
            System.Windows.Controls.WebBrowser wb = (System.Windows.Controls.WebBrowser)sender;
            wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }

       
      

        
        private void StartRT_Click(object sender, RoutedEventArgs e)
        {
            savedata();
             
            //if trade tiger backfill 
            if(tradetiger.IsChecked==true )
            {
                Type ExcelType;
                object ExcelInst;
                object[] args = new object[3];

                string[] sharekhanfilePaths = Directory.GetFiles( "C:\\myshubhalabha\\sharekhan", "*.csv", SearchOption.TopDirectoryOnly);
                ExcelType = Type.GetTypeFromProgID("Broker.Application");
                ExcelInst = Activator.CreateInstance(ExcelType);
                ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                          ExcelInst, new object[1] { true });
                ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                     ExcelInst, new string[1] { db_path.Text });

                for (int i = 0; i < sharekhanfilePaths.Count(); i++)
                {
                    args[0] = Convert.ToInt16(0);
                    args[1] = sharekhanfilePaths[i];
                    args[2] = "Shubhasharekhan.format";


                    ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                              ExcelInst, args);
                }

                System.Windows.MessageBox.Show("Backfill For Trade Tiger Completed .... ");
                return;

            }


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
                    System.Windows.MessageBox.Show("Please start Nest as Run as Administrator and again start Realtime combo");
                    s1.closeallprocess();

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
                    System.Windows.MessageBox.Show("Please start Nest as 'Run as Administrator' and again start Realtime combo");
                    s1.closeallprocess();

                    return;
                }
            }
            
            if (txtTargetFolder.Text == "")
            {
                System.Windows.MessageBox.Show("Set Target Path.");
                txtTargetFolder.Focus();
                return;

            }



            System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "Application is pulling backfill data and processing files,please wait for some time.");
      

  
            
            CommandManager.InvalidateRequerySuggested();

            try
            {


                //   type = Type.GetTypeFromProgID("nest.scriprtd");
               


                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


                config.AppSettings.Settings.Remove("targetpathforcombo");

                config.AppSettings.Settings.Add("targetpathforcombo", txtTargetFolder.Text );
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");

                config.AppSettings.Settings.Remove("format");

                config.AppSettings.Settings.Add("format", Format_cb.SelectedItem.ToString() );
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
               
                config.AppSettings.Settings.Remove("terminal");

                config.AppSettings.Settings.Add("terminal",RTD_server_name.SelectedItem.ToString() );
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
            /////////////////////////////
            //servaer checking now/nest

            
            /////////////////////////////
            //Advancesetting.IsEnabled = false;
            //contactus.IsEnabled = false;
            //help.IsEnabled = false;
            //Format_cb.IsEnabled = false;
            //RTD_server_name.IsEnabled = false;
            //timetoRT.IsEnabled = false;
            //StartRT.IsEnabled = false;



            
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            string pathtostartprocess = path.Substring(0, path.Length - 18);
            System.Diagnostics.Process.Start(pathtostartprocess + "Endrt.exe");

            Shubharealtime.datadownload s = new datadownload();
            if (RTD_server_name.SelectedItem == "NEST")
            {
                s.checknestfiled();
            }
            if (RTD_server_name.SelectedItem == "NOW")
            {
                s.checknowfiled();
            }
            Task.Factory.StartNew(s.startdownload);
            this.Hide();
           

          
          
            CommandManager.InvalidateRequerySuggested();
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



        public void checknestfiled()
        {
            Process[] processes = Process.GetProcessesByName("NestTrader");
            IntPtr abcd = new IntPtr();
            IntPtr abcd1 = new IntPtr();
            IntPtr abcd2 = new IntPtr();
            IntPtr windowHandle = new IntPtr();
            List<Thread> processtostartback = new List<Thread>();

            SystemAccessibleObject sao;
            foreach (Process p in processes)
            {
                windowHandle = p.MainWindowHandle;

                //System.Windows.Forms.MessageBox.Show(p.HandleCount.ToString());

                abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");


                // do something with windowHandle
            }


            SystemWindow a = new SystemWindow(abcd1);
            try
            {
                string marketwatch = abcd1.ToString();
                if (marketwatch == "0")
                {
                    System.Windows.MessageBox.Show("Nest is not running or Market Watch not present check out and run real time combo again \n     thank you  ");

                }
                sao = SystemAccessibleObject.FromWindow(a, AccessibleObjectID.OBJID_WINDOW);


}
            catch
            {
                System.Windows.MessageBox.Show("Market Watch not found ");
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


            if (!marketwathrequiredfield.Contains("LTT"))
            {
                flag = 1;
                System.Windows.MessageBox.Show("LTT Not Present into market watch add LTT ");

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
                System.Windows.MessageBox.Show("Some required fileds are missing in market watch please add that fileds and try shubha real time combo again \n Thank you  ");
                closeallprocess();
            }
        }



        public void closeallprocess()
        {
            Process[] workers = Process.GetProcessesByName("Shubharealtime.vshost");
            Process[] workers1 = Process.GetProcessesByName("Shubharealtime");
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


        public void savedata()
        {
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

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("amipath");

            config.AppSettings.Settings.Add("amipath", db_path.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("targetpathforcombo");

            config.AppSettings.Settings.Add("targetpathforcombo", txtTargetFolder.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
            config.AppSettings.Settings.Remove("amipath");

            config.AppSettings.Settings.Add("amipath", db_path.Text);
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            config.AppSettings.Settings.Remove("terminalname");

            config.AppSettings.Settings.Add("terminalname", RTD_server_name.SelectedItem.ToString());
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            savedata();


            Environment.Exit(0);
        }

        private void btnTarget_Click(object sender, RoutedEventArgs e)
        {
            var Open_Folder = new System.Windows.Forms.FolderBrowserDialog();
            if (Open_Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Target_Folder_Path = Open_Folder.SelectedPath;


                db_path.Text = Target_Folder_Path;

                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("amipath");

                config.AppSettings.Settings.Add("amipath", db_path.Text);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("targetpathforcombo");

                config.AppSettings.Settings.Add("targetpathforcombo", txtTargetFolder.Text);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
                config.AppSettings.Settings.Remove("amipath");

                config.AppSettings.Settings.Add("amipath", db_path.Text);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private void Import_symbol_Click(object sender, RoutedEventArgs e)
        {
            //dataGrid3.Items.Add(new DataItem { Column0 = "abcd", Column1 = "dasdasdas" });
            //dataGrid3.Items.Add(new DataItem { Column0 = "abcasdsasdd", Column1 = "das1111111111111dasdas" });
            string terminalname = ConfigurationManager.AppSettings["terminalname"];
            Process[] processes = null;

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
                    System.Windows.MessageBox.Show(" Please start Nest as Run as Administrator and again start Realtime combo");
                    closeallprocess();
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
                    System.Windows.MessageBox.Show(" Please start Nest as Run as Administrator and again start Realtime combo");
                    closeallprocess();
                    return;
                }
            }         IntPtr abcd = new IntPtr();
            IntPtr abcd1 = new IntPtr();
            IntPtr abcd2 = new IntPtr();
            IntPtr windowHandle = new IntPtr();



            List<Thread> processtostartback = new List<Thread>();
            SystemAccessibleObject sao,f;

            foreach (Process p in processes)
            {
                windowHandle = p.MainWindowHandle;

                //System.Windows.Forms.MessageBox.Show(p.HandleCount.ToString());

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
                    System.Windows.MessageBox.Show("Nest is not running or Market Watch not present check out and run real time combo again \n     thank you  ");
                    closeallprocess();

                }
                sao = SystemAccessibleObject.FromWindow(a, AccessibleObjectID.OBJID_WINDOW);
            }
            catch
            {
                System.Windows.MessageBox.Show("Market Watch not found ");
                return;
            }
           


            f = sao.Children[3];

            for (int i = 0; i < f.Children.Count() - 1; i++)
            {
                listView1.Items.Add(new ListViewData(f.Children[i].Name, "NOTBACKFILL", f.Children[i].Name));

            }
            
            
            
          

        }

        private void dataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        

        private void dataGrid3_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        
        }

        private void dataGrid3_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewData lvc = (ListViewData)listView1.SelectedItem;
            if (lvc != null)
            {
                stopRefreshControls = true;
                textBox1.Text = lvc.Col1;
                textBox2.Text = lvc.Col2;
                textBox3.Text = lvc.Col3;
                stopRefreshControls = false;
            }
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListView(textBox1.Text, textBox2.Text,textBox3.Text );
        }
        private void setDataChanged(bool value)
        {
            dataChanged = value;
        }
        private void RefreshListView(string value1, string value2, string value3)
        {
            ListViewData lvc = (ListViewData)listView1.SelectedItem; //new ListViewClass(value1, value2);
            if (lvc != null && !stopRefreshControls)
            {

                lvc.Col1 = value1;
                lvc.Col2 = value2;
                lvc.Col3 = value3;

                listView1.Items.Refresh();
            }
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListView(textBox1.Text, textBox2.Text, textBox3.Text);

        }

        private void sav_symbolfile_Click(object sender, RoutedEventArgs e)
        {

           
                MyData md = new MyData();
                md.Save(listView1.Items);
                setDataChanged(false);
            
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

        private void nestbackfill_Checked(object sender, RoutedEventArgs e)
        {
            saveradiobuttn();
        }

        private void googlebackfill_Checked(object sender, RoutedEventArgs e)
        {
            saveradiobuttn();
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

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("shanteshpaigude1988@gmail.com");
            mail.To.Add("shanteshpaigude1988@gmail.com");
            mail.Subject = "demo";
            mail.Body = "Report";
            //Attachment attachment = new Attachment(filename);
            //mail.Attachments.Add(attachment);

            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential("shanteshpaigude1988@gmail.com", "");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
           // SendMail("", "", "", true);
        }

        private void tradetiger_Checked(object sender, RoutedEventArgs e)
        {
            saveradiobuttn();
        }

        
       

       


      

       
       

       
       

       
       
       

        
    }
}