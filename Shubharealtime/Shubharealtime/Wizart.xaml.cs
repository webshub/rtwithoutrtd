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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using System.Security.Principal;
using System.Diagnostics;
using log4net;
using log4net.Config;

namespace Shubharealtime
{
    /// <summary>
    /// Interaction logic for Wizart.xaml
    /// </summary>
    public partial class Wizart : Window
    {
        public Wizart()
        {
            InitializeComponent();
        }
       int nextcount = 0;

        //run application as admin 
       private bool IsRunAsAdministrator()
       {
           var wi = WindowsIdentity.GetCurrent();
           var wp = new WindowsPrincipal(wi);

           return wp.IsInRole(WindowsBuiltInRole.Administrator);
       }
       //close application 
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
           
        }

      
        //checking os is 32 bit or 64 bit 
        static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



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
                    System.Windows.MessageBox.Show("Unable to Run program as Administrator,please ensure that you have admin privileges'", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                }

                // Shut down the current process
                Application.Current.Shutdown();
            }
            else
            {



                try
                {
                    notagree.IsChecked = true;


                    if (!Directory.Exists("C:\\myshubhalabha\\amirealtime"))
                    {
                        Directory.CreateDirectory("C:\\myshubhalabha\\amirealtime");
                    }
                    if (!Directory.Exists("C:\\myshubhalabha\\amibroker format file"))
                    {
                        Directory.CreateDirectory("C:\\myshubhalabha\\amibroker format file");
                    }


                    string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                    string processtostart = "";
                    string programfilepath = ProgramFilesx86();


                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Notice.txt";

                    RegistryKey regKey = Registry.CurrentUser;
                    regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                    var Amiexepath = regKey.GetValue("Amiexepath");

                   
                    //copy files into folder 
                    File.Copy(processtostart, "C:\\myshubhalabha\\Notice.txt", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhaxls.format";

                    
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhaxls.format", true);
                    File.Copy(processtostart, Amiexepath + "\\Formats\\shubhaxls.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhasharekhan.format";


                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhasharekhan.format", true);
                    File.Copy(processtostart, Amiexepath + "\\Formats\\Shubhasharekhan.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhanest-now.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhanest-now.format", true);
                    File.Copy(processtostart, Amiexepath + "\\Formats\\shubhanest-now.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaRt.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\ShubhaRt.format", true);
                    File.Copy(processtostart, Amiexepath + "\\Formats\\ShubhaRt.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhabackfill.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhabackfill.format", true);
                    File.Copy(processtostart, Amiexepath + "\\Formats\\Shubhabackfill.format", true);
                    processtostart = filepath.Substring(0, filepath.Length - 18) + "RT.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\RT.format", true);
                    File.Copy(processtostart, Amiexepath + "\\Formats\\RT.format", true);
                }
                catch(Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Window1));
                    log.Debug(ex.Message );
                }
                try
                {

                    //Shubhalabha123.Regidtartion r = new Shubhalabha123.Regidtartion();
                    //stackcontainer.Children.Add(r);

                    shubhalabharts.Welcomewizrd n = new shubhalabharts.Welcomewizrd();
                    stackcontainer.Children.Add(n);
                }
                catch(Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Window1));
                    log.Debug(ex.Message);
                }

            }
        }

        private void nextButtonforterminal_Click(object sender, RoutedEventArgs e)
        {

            //on click of next button checking required things are done or not 
            try
            {
                Rsult_lbl.Content = "";
                if (nextcount == 0)
                {
                    agree.Visibility = Visibility.Visible;
                    notagree.Visibility = Visibility.Visible;


                    try
                    {
                        stackcontainer.Children.RemoveAt(0);
                    }
                    catch(Exception ex)
                    {
                        log4net.Config.XmlConfigurator.Configure();
                        ILog log = LogManager.GetLogger(typeof(Window1));
                        log.Debug(ex.Message);
                    }
                    Shubhalabha123.GNUGPL c = new Shubhalabha123.GNUGPL();
                    stackcontainer.Children.Add(c);
                    nextcount++;
                    backButton.IsEnabled = true;


                    return;
                   
                }
                if (nextcount == 1)
                {

                    if(agree.IsChecked==false )
                    {
                        Rsult_lbl.Foreground = Brushes.Red;
                        Rsult_lbl.Content = "Please accept license agreement else Click  Cancel to exit. ";

                        return ;

                    }

                    agree.Visibility = Visibility.Hidden;
                    notagree.Visibility = Visibility.Hidden;
                        

                    try
                    {
                        stackcontainer.Children.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                        log4net.Config.XmlConfigurator.Configure();
                        ILog log = LogManager.GetLogger(typeof(Window1));
                        log.Debug(ex.Message);
                    }
                    Introduction n = new Introduction();
                    stackcontainer.Children.Add(n);
                    nextcount++;
                    backButton.IsEnabled = true;


                    return;
                }

                if (nextcount == 2)
                {
                    try
                    {
                        stackcontainer.Children.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                        log4net.Config.XmlConfigurator.Configure();
                        ILog log = LogManager.GetLogger(typeof(Window1));
                        log.Debug(ex.Message);
                    }
                    terminal t = new terminal();
                    stackcontainer.Children.Add(t);
                    nextcount++;
                    backButton.IsEnabled = true;


                    return;


                }
                if (nextcount == 3)
                {

                    RegistryKey regKey = Registry.CurrentUser;
                    regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                    var terminalname = regKey.GetValue("terminal");
                    string terminal = "";
                    try
                    {
                        terminal = terminalname.ToString();
                    }
                    catch
                    {
                        Rsult_lbl.Content = "Error :Please select atleast one terminal ";

                    }
                    //nest cheking
                    if (terminal == "NEST")
                    {
                        wizartchecking d = new wizartchecking();
                        string result = d.checknestfiled();
                        if (result == "Done")
                        {
                            var backfill1 = regKey.GetValue("backfill");
                            string backfill = "";
                            if (backfill1 != null)
                            {
                                backfill = backfill1.ToString();

                            }
                            if (backfill == "yes")
                            {
                                System.Windows.MessageBox.Show("Please DO NOT USE YOUR COMPUTER for few minutes .\n As application is checking for PLUS configuration", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                               
                                int resultforbackfill = d.checkbackfill();
                                if (resultforbackfill == 1)
                                {

                                    try
                                    {
                                        stackcontainer.Children.RemoveAt(0);
                                    }
                                    catch (Exception ex)
                                    {
                                        log4net.Config.XmlConfigurator.Configure();
                                        ILog log = LogManager.GetLogger(typeof(Window1));
                                        log.Debug(ex.Message);
                                    }

                                    Rsult_lbl.Foreground = Brushes.Green;
                                    Rsult_lbl.Content = "NEST Plus is configured successfully for backfill ";



                                    Chartingapllication c = new Chartingapllication();
                                    stackcontainer.Children.Add(c);
                                    nextcount++;

                                    return;
                                }
                                else
                                {
                                    Rsult_lbl.Foreground = Brushes.Red ;
                                   
                                    Rsult_lbl.Content = "Backfill cannot be done ";
                                    return;
                                }
                            }
                            else
                            {
                                try
                                {
                                    stackcontainer.Children.RemoveAt(0);
                                }
                                catch (Exception ex)
                                {
                                    log4net.Config.XmlConfigurator.Configure();
                                    ILog log = LogManager.GetLogger(typeof(Window1));
                                    log.Debug(ex.Message);
                                }




                                Rsult_lbl.Foreground = Brushes.Green ;
                                Rsult_lbl.Content = "Terminal selection done successfully  ";

                                Chartingapllication c = new Chartingapllication();
                                stackcontainer.Children.Add(c);
                                nextcount++;

                                return;
                            }

                        }
                        else
                        {
                            Rsult_lbl.Foreground = Brushes.Red;

                            Rsult_lbl.Content = result;
                            return;
                        }



                    }


                    //now checking
                    else   if (terminal == "NOW")
                    {
                        wizartchecking d = new wizartchecking();
                        string result = d.checknowfiled();
                        if (result == "Done")
                        {
                            var backfill1 = regKey.GetValue("backfill");
                            string backfill = "";
                            if (backfill1!=null )
                            {
                                backfill = backfill1.ToString();

                            }
                            if (backfill == "yes")
                            {
                               
                                System.Windows.MessageBox.Show("Please DO NOT USE YOUR COMPUTER for few minutes .\n As application is checking for backfill option.", "Warning Message", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                                int resultforbackfill = d.checkbackfill();
                                if (resultforbackfill == 1)
                                {

                                    

                                    try
                                    {
                                        stackcontainer.Children.RemoveAt(0);
                                    }
                                    catch (Exception ex)
                                    {
                                        log4net.Config.XmlConfigurator.Configure();
                                        ILog log = LogManager.GetLogger(typeof(Window1));
                                        log.Debug(ex.Message);
                                    }





                                    Rsult_lbl.Foreground = Brushes.Green;
                                    Rsult_lbl.Content = "Terminal selection done successfully  ";
                                    Chartingapllication c = new Chartingapllication();
                                    stackcontainer.Children.Add(c);
                                    nextcount++;

                                    return;
                                }
                                else
                                {
                                    Rsult_lbl.Foreground = Brushes.Red;

                                    Rsult_lbl.Content = "Backfill cannot be done ";
                                    return;
                                }
                            }
                            else
                            {
                                try
                                {
                                    stackcontainer.Children.RemoveAt(0);
                                }
                                catch (Exception ex)
                                {
                                    log4net.Config.XmlConfigurator.Configure();
                                    ILog log = LogManager.GetLogger(typeof(Window1));
                                    log.Debug(ex.Message);
                                }






                                Chartingapllication c = new Chartingapllication();
                                stackcontainer.Children.Add(c);
                                nextcount++;

                                return;
                            }

                        }
                        else
                        {
                            Rsult_lbl.Foreground = Brushes.Red;

                            Rsult_lbl.Content = result;
                            return;
                        }


                    }

                    else  if (terminal == "Odin")
                    {
                        try
                        {
                            stackcontainer.Children.RemoveAt(0);
                        }
                        catch (Exception ex)
                        {
                            log4net.Config.XmlConfigurator.Configure();
                            ILog log = LogManager.GetLogger(typeof(Window1));
                            log.Debug(ex.Message);
                        }
                        Rsult_lbl.Foreground = Brushes.Green;
                        Rsult_lbl.Content = "Terminal selection done successfully  ";
                        Chartingapllication c = new Chartingapllication();
                        stackcontainer.Children.Add(c);
                        nextcount++;

                        return;
                    }

                    else if (terminal == "Tradetiger")
                    {
                        try
                        {
                            stackcontainer.Children.RemoveAt(0);
                        }
                        catch (Exception ex)
                        {
                            log4net.Config.XmlConfigurator.Configure();
                            ILog log = LogManager.GetLogger(typeof(Window1));
                            log.Debug(ex.Message);
                        }
                        Rsult_lbl.Foreground = Brushes.Green;
                        Rsult_lbl.Content = "Terminal selection done successfully  ";
                        Chartingapllication c = new Chartingapllication();
                        stackcontainer.Children.Add(c);
                        nextcount++;

                        return;
                    }

                    else
                    {
                        Rsult_lbl.Foreground = Brushes.Red;

                        Rsult_lbl.Content = "Error :Please select atleast one terminal ";
                    }

                    return;


                }

                if (nextcount == 4)
                {
                    ///for checking amibroker database path

                    RegistryKey regKey = Registry.CurrentUser;
                    regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                    var terminalname = regKey.GetValue("terminal");
                    var Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath");
                    var Chartingapplication = regKey.GetValue("Chartingapplication");
                    if (Chartingapplication == null || Chartingapplication == "")
                    {
                        Rsult_lbl.Foreground = Brushes.Red;

                        Rsult_lbl.Content = "Please select charting application ";
                        return;
                    }
                    

////////////////////////////////////////checking format folder of amibroker 
                    if (Chartingapplication.ToString().Contains("Amibroker"))
                    {
                        try
                        {
                            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                            string processtostart = "";
                            string programfilepath = ProgramFilesx86();


                            processtostart = filepath.Substring(0, filepath.Length - 18) + "Notice.txt";

                           
                            var Amiexepath = regKey.GetValue("Amiexepath");


                         


                            File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhaxls.format", true);
                            File.Copy(processtostart, Amiexepath + "\\Formats\\shubhaxls.format", true);

                            processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhasharekhan.format";


                            File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhasharekhan.format", true);
                            File.Copy(processtostart, Amiexepath + "\\Formats\\Shubhasharekhan.format", true);

                            processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhanest-now.format";
                            File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhanest-now.format", true);
                            File.Copy(processtostart, Amiexepath + "\\Formats\\shubhanest-now.format", true);

                            processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaRt.format";
                            File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\ShubhaRt.format", true);
                            File.Copy(processtostart, Amiexepath + "\\Formats\\ShubhaRt.format", true);

                            processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhabackfill.format";
                            File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhabackfill.format", true);
                            File.Copy(processtostart, Amiexepath + "\\Formats\\Shubhabackfill.format", true);
                            processtostart = filepath.Substring(0, filepath.Length - 18) + "RT.format";
                            File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\RT.format", true);
                            File.Copy(processtostart, Amiexepath + "\\Formats\\RT.format", true);
                        }
                        catch
                        {
                            Rsult_lbl.Foreground = Brushes.Red;

                            Rsult_lbl.Content = "Please set right Amibroker Exe path ";
                            return;
                        }
                    }
////////////////////////////////////////////////////////////////////////

                    try
                    {
                        stackcontainer.Children.RemoveAt(0);
                    }
                    catch (Exception ex)
                    {
                        log4net.Config.XmlConfigurator.Configure();
                        ILog log = LogManager.GetLogger(typeof(Window1));
                        log.Debug(ex.Message);
                    }

                    Result r = new Result();
                    stackcontainer.Children.Add(r);

                    finish.IsEnabled = true;
                    cancelButton.IsEnabled = false;
                    nextButtonforterminal.IsEnabled = false;
                    nextcount++;
                }

            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                ILog log = LogManager.GetLogger(typeof(Window1));
                log.Debug(ex.Message);
            }
        }

        private void nextbuttonforchart_Click(object sender, RoutedEventArgs e)
        {


        }
        //show previouse window 

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            cancelButton.IsEnabled = true ;
            
            Rsult_lbl.Content = "";
            
            if (nextcount == 5)
            {
               
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Window1));
                    log.Debug(ex.Message);
                }
                Chartingapllication c = new Chartingapllication();
                stackcontainer.Children.Add(c);
                nextcount--;
                nextButtonforterminal.IsEnabled = true;
                return;
            }
            if (nextcount == 4)
            {
                finish.IsEnabled = false ;
                nextButtonforterminal.IsEnabled = true;
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Window1));
                    log.Debug(ex.Message);
                }
                terminal t = new terminal();
                stackcontainer.Children.Add(t);
                nextcount--;
                return;
            }
            if (nextcount == 3)
            {
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Window1));
                    log.Debug(ex.Message);
                }

                Introduction c = new Introduction();
                stackcontainer.Children.Add(c);
                nextcount--;
                return;
            }


            if (nextcount == 2)
            {
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Window1));
                    log.Debug(ex.Message);
                }
                agree.Visibility = Visibility.Visible;
                notagree.Visibility = Visibility.Visible;
                Shubhalabha123.GNUGPL  c = new  Shubhalabha123.GNUGPL ();
                stackcontainer.Children.Add(c);
                nextcount--;
                return;
            }
            if (nextcount == 1)
            {
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch (Exception ex)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Window1));
                    log.Debug(ex.Message);
                }
                agree.Visibility = Visibility.Hidden;
                notagree.Visibility = Visibility.Hidden;
                shubhalabharts.Welcomewizrd n = new shubhalabharts.Welcomewizrd();
                stackcontainer.Children.Add(n);
              
                nextcount--;
                return;
              

            }

        }

        //finish the wizard 
        private void finish_Click(object sender, RoutedEventArgs e)
        {
            //save registry entry 
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("Wizart", "done");
            regKey.SetValue("noofsymbol", "15");
          

          
            try
            {
                var registerdate = regKey.GetValue("sd");
                var paidornot = regKey.GetValue("sp");

                var chktmp = regKey.GetValue("ApplicationId");

                //if user delete register entry then show login window agian 
                if (chktmp != null)
                {
                    if (chktmp.ToString() == "1" && registerdate != null && paidornot != null)
                    {

                        this.Hide();
                        Shubharealtime.Window1 w = new Window1();
                        w.ShowDialog();
                    }
                    else
                    {

                        this.Hide();
                        Shubharealtime.MainWindow w = new MainWindow();
                        w.ShowDialog();
                    }
                }
                else
                {
                    this.Hide();
                    Shubharealtime.MainWindow w = new MainWindow();
                    w.ShowDialog();
                }




            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                ILog log = LogManager.GetLogger(typeof(Window1));
                log.Debug(ex.Message);
            }

            this.Close();
            //string path = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            //string pathtostartprocess = path.Substring(0, path.Length - 18);
            //System.Diagnostics.Process.Start(pathtostartprocess + "Shubharealtime.exe");

        }
        //close all process 
        public void closeallprocess()
        {
            Process[] workers = Process.GetProcessesByName("Shubharealtime.vshost");
            Process[] workers1 = Process.GetProcessesByName("Shubharealtime");
            Process[] workers2 = Process.GetProcessesByName("Endrt.vshost");
            Process[] workers3 = Process.GetProcessesByName("Endrt");
            Process[] workers4 = Process.GetProcessesByName("Broker");
            Process[] workers5 = Process.GetProcessesByName("Shubharealtime123");



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
        //close this window 
        private void Window_Closed(object sender, EventArgs e)
        {
            closeallprocess();
        }
    }
}
