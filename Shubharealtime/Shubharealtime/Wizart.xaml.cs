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
       private bool IsRunAsAdministrator()
       {
           var wi = WindowsIdentity.GetCurrent();
           var wp = new WindowsPrincipal(wi);

           return wp.IsInRole(WindowsBuiltInRole.Administrator);
       }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
           
        }

        private void nest_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void now_Checked(object sender, RoutedEventArgs e)
        {
            
           
        }

        private void tradetiger_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void odin_Checked(object sender, RoutedEventArgs e)
        {
           
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
                    MessageBox.Show("Sorry, this application must be run as Administrator.");
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


                    File.Copy(processtostart, "C:\\myshubhalabha\\Notice.txt", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhaxls.format";

                    
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhaxls.format", true);
                    File.Copy(processtostart, programfilepath + "\\AmiBroker\\Formats\\shubhaxls.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhasharekhan.format";


                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhasharekhan.format", true);
                    File.Copy(processtostart, programfilepath + "\\AmiBroker\\Formats\\Shubhasharekhan.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "shubhanest-now.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\shubhanest-now.format", true);
                    File.Copy(processtostart, programfilepath + "\\AmiBroker\\Formats\\shubhanest-now.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "ShubhaRt.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\ShubhaRt.format", true);
                    File.Copy(processtostart, programfilepath + "\\AmiBroker\\Formats\\ShubhaRt.format", true);

                    processtostart = filepath.Substring(0, filepath.Length - 18) + "Shubhabackfill.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\Shubhabackfill.format", true);
                    File.Copy(processtostart, programfilepath + "\\AmiBroker\\Formats\\Shubhabackfill.format", true);
                    processtostart = filepath.Substring(0, filepath.Length - 18) + "RT.format";
                    File.Copy(processtostart, "C:\\myshubhalabha\\amibroker format file\\RT.format", true);
                    File.Copy(processtostart, programfilepath + "\\AmiBroker\\Formats\\RT.format", true);
                }
                catch
                {

                }
                try
                {

                    //Shubhalabha123.Regidtartion r = new Shubhalabha123.Regidtartion();
                    //stackcontainer.Children.Add(r);

                    Shubhalabha123.GNUGPL n = new Shubhalabha123.GNUGPL();
                    stackcontainer.Children.Add(n);
                }
                catch
                {
                }

            }
        }

        private void nextButtonforterminal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rsult_lbl.Content = "";

                if (nextcount == 0)
                {

                    if(agree.IsChecked==false )
                    {
                        return ;

                    }

                    agree.Visibility = Visibility.Hidden;
                    notagree.Visibility = Visibility.Hidden;
                        

                    try
                    {
                        stackcontainer.Children.RemoveAt(0);
                    }
                    catch
                    {
                    }
                    Introduction n = new Introduction();
                    stackcontainer.Children.Add(n);
                    nextcount++;
                    backButton.IsEnabled = true;


                    return;
                }

                if (nextcount == 1)
                {
                    try
                    {
                        stackcontainer.Children.RemoveAt(0);
                    }
                    catch
                    {
                    }
                    terminal t = new terminal();
                    stackcontainer.Children.Add(t);
                    nextcount++;
                    backButton.IsEnabled = true;


                    return;


                }
                if (nextcount == 2)
                {

                    RegistryKey regKey = Registry.CurrentUser;
                    regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                    var terminalname = regKey.GetValue("terminal");
                    string terminal = terminalname.ToString();

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
                                MessageBox.Show("Please stay idle for 2 min we are checking your PLUS configaration please wait till you get responce message ");
                               
                                int resultforbackfill = d.checkbackfill();
                                if (resultforbackfill == 1)
                                {
                                    MessageBox.Show("Backfill chacking done");

                                    try
                                    {
                                        stackcontainer.Children.RemoveAt(0);
                                    }
                                    catch
                                    {
                                    }



                                    Rsult_lbl.Content = "Nest pluse is working fine  ";



                                    Chartingapllication c = new Chartingapllication();
                                    stackcontainer.Children.Add(c);
                                    nextcount++;

                                    return;
                                }
                                else
                                {
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
                                catch
                                {
                                }






                                Chartingapllication c = new Chartingapllication();
                                stackcontainer.Children.Add(c);
                                nextcount++;

                                return;
                            }

                        }
                        else
                        {
                            Rsult_lbl.Content = result;
                            return;
                        }



                    }



                    if (terminal == "NOW")
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
                                MessageBox.Show("Please stay idle for 1 min its cheking backfill option");

                                int resultforbackfill = d.checkbackfill();
                                if (resultforbackfill == 1)
                                {

                                    MessageBox.Show("Backfill chacking done");

                                    try
                                    {
                                        stackcontainer.Children.RemoveAt(0);
                                    }
                                    catch
                                    {
                                    }






                                    Chartingapllication c = new Chartingapllication();
                                    stackcontainer.Children.Add(c);
                                    nextcount++;

                                    return;
                                }
                                else
                                {
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
                                catch
                                {
                                }






                                Chartingapllication c = new Chartingapllication();
                                stackcontainer.Children.Add(c);
                                nextcount++;

                                return;
                            }

                        }
                        else
                        {
                            Rsult_lbl.Content = result;
                            return;
                        }


                    }

                    if (terminal == "Odin")
                    {
                        try
                        {
                            stackcontainer.Children.RemoveAt(0);
                        }
                        catch
                        {
                        }
                        Chartingapllication c = new Chartingapllication();
                        stackcontainer.Children.Add(c);
                        nextcount++;

                        return;
                    }

                    if (terminal == "Tradetiger")
                    {
                        try
                        {
                            stackcontainer.Children.RemoveAt(0);
                        }
                        catch
                        {
                        }
                        Chartingapllication c = new Chartingapllication();
                        stackcontainer.Children.Add(c);
                        nextcount++;

                        return;
                    }



                    return;


                }

                if (nextcount == 3)
                {
                    ///for checking amibroker database path
                    try
                    {
                        Type ExcelType;
                        object ExcelInst;
                        object[] args = new object[3];
                        RegistryKey regKey = Registry.CurrentUser;
                        regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                        var terminalname = regKey.GetValue("terminal");
                        var Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath");
                        var Chartingapplication = regKey.GetValue("Chartingapplication");
                        if (Chartingapplication.ToString().Contains("Amibroker"))
                        {

                        ExcelType = Type.GetTypeFromProgID("Broker.Application");
                        ExcelInst = Activator.CreateInstance(ExcelType);
                        ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                                  ExcelInst, new object[1] { true });

                        ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                            ExcelInst, new string[1] { Amibrokerdatapath.ToString() });



                        args[0] = Convert.ToInt16(0);
                        args[1] = "C:\\myshubhalabha\\Realtimeamibrokerdata.txt";
                        args[2] = "RT.format";
                        ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                             ExcelInst, args);
                        }
                    }catch
                    {
                        }
                    try
                    {
                        stackcontainer.Children.RemoveAt(0);
                    }
                    catch
                    {
                    }
                    Result r = new Result();
                    stackcontainer.Children.Add(r);

                    finish.IsEnabled = true;
                    nextButtonforterminal.IsEnabled = false;
                    nextcount++;
                }

            }
            catch
            {

            }

        }

        private void nextbuttonforchart_Click(object sender, RoutedEventArgs e)
        {


        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Rsult_lbl.Content = "";
            
            if (nextcount == 4)
            {
               
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch
                {
                }

                Chartingapllication c = new Chartingapllication();
                stackcontainer.Children.Add(c);
                nextcount--;
                nextButtonforterminal.IsEnabled = true;
                return;
            }
            if (nextcount == 3)
            {
                finish.IsEnabled = false ;
                nextButtonforterminal.IsEnabled = true;
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch
                {
                }
                terminal t = new terminal();
                stackcontainer.Children.Add(t);
                nextcount--;
                return;
            }
            if (nextcount == 2)
            {
                try
                {
                    stackcontainer.Children.RemoveAt(0);
                }
                catch
                {
                }

                Introduction c = new Introduction();
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
                catch
                {
                }
                agree.Visibility = Visibility.Visible;
                notagree.Visibility = Visibility.Visible;
                Shubhalabha123.GNUGPL  c = new  Shubhalabha123.GNUGPL ();
                stackcontainer.Children.Add(c);
                nextcount--;
                return;
            }

        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            regKey.SetValue("Wizart", "done");


          
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
            }

            this.Close();
            //string path = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            //string pathtostartprocess = path.Substring(0, path.Length - 18);
            //System.Diagnostics.Process.Start(pathtostartprocess + "Shubharealtime.exe");

        }
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
        private void Window_Closed(object sender, EventArgs e)
        {
            closeallprocess();
        }
    }
}
