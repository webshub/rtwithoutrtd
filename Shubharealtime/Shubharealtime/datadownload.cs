//////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////


using System;
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
using Microsoft.Office.Interop.Excel;

using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Collections;
using System.IO.Compression;
using System.Diagnostics;

using System.IO.Packaging;

using System.Text.RegularExpressions;
using System.Data.OleDb;
using ManagedWinapi.Windows;
using ManagedWinapi.Accessibility;
using Microsoft.Win32;
using System.Threading.Tasks ;
using FileHelpers;
namespace Shubharealtime
{
    class datadownload
    {

        //variable declaration 
        string s = "";
        string[] words = null;
        string[] words1 = null;
        string[] luttime = null;
        WebClient Client = new WebClient();
      string   datetimetostore=DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year ;
        Type type;
          Type ExcelType;
        object ExcelInst;
        object[] args = new object[3];
        IntPtr windowHandle = new IntPtr();
        IRtdServer m_server;
        DispatcherTimer DispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
        SystemAccessibleObject sao;
        SystemAccessibleObject finalobject;
        SystemAccessibleObject f;
        List<string> symbolname = new List<string>();
        List<string> mappingsymbol = new List<string>();
        List<string> googlesymbol = new List<string>();

        
          string        Amibrokerdatapath = "" ;
          string        Metastockdatapath = "" ;
        string        fchart = "" ;
           
       


        string timetosave = ConfigurationManager.AppSettings["timetoRT"];
        string targetpath = ConfigurationManager.AppSettings["targetpathforcombo"];
        string amipath = ConfigurationManager.AppSettings["amipath"];
        string nestback = ConfigurationManager.AppSettings["nestbackfill"];
        string tradetiger = ConfigurationManager.AppSettings["tradetiger"];

        string googleback = ConfigurationManager.AppSettings["googlebackfill"];
        string withoutback = ConfigurationManager.AppSettings["withoutbackfill"];
        string terminalname = ConfigurationManager.AppSettings["terminalname"];
        
        string chartingaplication = ConfigurationManager.AppSettings["format"];

        

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessagetowindow(
            IntPtr hWnd,
            uint msg,
            int wParam,
            int lParam
            );
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref System.Windows. Point lpPoint);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
       
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
       
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
        private const int SW_SHOWMAXIMIZED = 3;
        private const UInt32 WM_CLOSE = 0x0010;
        public const UInt32 WM_SCROLL = 277; // Horizontal scroll
        private const UInt32 SB_LINEDOWN = 1; // Scrolls one line down
        private const UInt32 SB_PAGEDOWN = 3; // Scrolls one page down
        private const int SB_PAGEUP = 2; // Scrolls one page up
        const uint WM_KEYDOWN = 0x02;
         const int WM_p = 0x50;
         const int WM_d = 0x43;

        //Close window by its handle 
        void CloseWindow(IntPtr hwnd)
        {
            SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        //Find all child window by its handle 
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
        //Process nest and now backfill data 
        public void Executenestnowbackfillrocessing(string strBSECSVArr, string datetostore, string name, int count, string mappingsymbol)
        {


            FileHelperEngine engineBSECSV1 = new FileHelperEngine(typeof(nestnow));

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            var chartforbackfill = regKey.GetValue("chartingappforbackfill");
          
            Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath").ToString();
            Metastockdatapath = regKey.GetValue("Metastockdatapath").ToString();


            //Get BSE Equity Filename day, month, year
            string[] words = strBSECSVArr.Split('\\');

            string strbseequityfilename = words[words.Length - 1];


            nestnow[] resbsecsv1 = engineBSECSV1.ReadFile(strBSECSVArr) as nestnow[];


            nestnowfinal[] finalarr = new nestnowfinal[resbsecsv1.Length];
            int icntr = 0;


            while (icntr < resbsecsv1.Length)
            {
                finalarr[icntr] = new nestnowfinal();

                finalarr[icntr].ticker = strbseequityfilename.Substring(0, strbseequityfilename.Length - 4);
               // finalarr[icntr].name = strbseequityfilename.Substring(0, strbseequityfilename.Length - 4); ;

               // finalarr[icntr].ticker = resbsecsv1[icntr].Name;
                string[] datetime = resbsecsv1[icntr].datetime.Split(' ');
                datetostore = datetime[0];
                string timetostore = datetime[1];
                finalarr[icntr].date = datetostore; // String.Format("{0:yyyyMMdd}", myDate);
                finalarr[icntr].open = resbsecsv1[icntr].OPEN_PRICE;
                finalarr[icntr].high = resbsecsv1[icntr].HIGH_PRICE;
                finalarr[icntr].low = resbsecsv1[icntr].LOW_PRICE;
                finalarr[icntr].close = resbsecsv1[icntr].CLOSE_PRICE;
                finalarr[icntr].volume = resbsecsv1[icntr].volume;
                finalarr[icntr].time = timetostore;
                if (chartforbackfill.ToString() == "Fchart")
                {
                    finalarr[icntr].time = timetostore.Substring(0,timetostore.Length-3);

                }
                if (chartforbackfill.ToString() == "Metastock")
                {
                    //add PER column befor date column 
                    finalarr[icntr].date ="I,"+ finalarr[icntr].date;

                }

                icntr++;
            }

            FileHelperEngine engineBSECSVFINAL = new FileHelperEngine(typeof(nestnowfinal));
            if (chartforbackfill.ToString() == "Metastock")
                {
                    engineBSECSVFINAL.HeaderText = "<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>";
                 //set file path as metastock backfill 
                
            }
            
            engineBSECSVFINAL.WriteFile(strBSECSVArr, finalarr);




            if (chartforbackfill.ToString() == "Metastock")
            {


                if (!Directory.Exists(Metastockdatapath  + "\\Intraday\\Metastock"))
                {
                    Directory.CreateDirectory(Metastockdatapath + "\\Intraday\\Metastock");
                }
                // commandpromptcall(filename, targetpath + "\\Intraday\\Metastock\\realtimemetastock");
                try
                {

                    string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                    string processtostart = filepath.Substring(0, filepath.Length - 18) + "asc2ms.exe";

                    File.Copy(processtostart, Metastockdatapath + "\\asc2ms.exe", true);
                }
                catch
                {
                }
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                //startInfo.Arguments = "/C  C:\\asc2ms.exe -f C:\\data\\Metastock\\M.csv -r r -o C:\\data\\Metastock\\google\\e";
                startInfo.Arguments = "/C  " + Metastockdatapath + "\\asc2ms.exe -f " + strBSECSVArr + " -r r -o " + Metastockdatapath + "\\Intraday\\Metastock\\" + finalarr[0].ticker;
                // startInfo.Arguments = @"/C  C:\asc2ms.exe -f C:\Documents and Settings\maheshwar\My Documents\BSe\Downloads\Googleeod -r r -o C:\Documents and Settings\maheshwar\My Documents\BSe\Downloads\Googleeod\Metastock\a" ;



                process.StartInfo = startInfo;
                process.Start();


            }





            return;














        }
        //take symbol name from saved file 
        public void getsymbolname()
        {

            try
            {
                using (var reader = new StreamReader("C:\\myshubhalabha\\Symbolname.csv"))
                {
                    string line = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] name = line.Split(',');
                        symbolname.Add(name[0]);
                        googlesymbol.Add(name[1]);
                        mappingsymbol.Add(name[2]);
                    }
                }
            }
            catch
            {
            }

        }
        
        //Starting backfill data 
        public  void startdownload()
        {


          


            if (nestback == "True")
            {

            Process[] processes = null ;

            if (terminalname=="NEST")
            {
            try
            {
                //checking nest is running as admin or not 
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
                    //checking now is running as admin or not 

                    type = Type.GetTypeFromProgID("now.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    processes = Process.GetProcessesByName("NOW");

                }
                catch
                {
                    System.Windows.MessageBox.Show(" Please start NOW  as Run as Administrator and again start Realtime combo");
                    closeallprocess();
                    return;
                }
            }
            getsymbolname();

           

            IntPtr abcd=new IntPtr();
            IntPtr abcd1 = new IntPtr();
            IntPtr abcd2 = new IntPtr();
            List<Thread > processtostartback = new List<Thread >();

          //find window by its handle
            foreach (Process p in processes)
            {
                windowHandle = p.MainWindowHandle;

                //System.Windows.Forms.MessageBox.Show(p.HandleCount.ToString());

                abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");

                // do something with windowHandle
            }
          
            SystemWindow a = new SystemWindow(abcd1);
            ShowWindow(windowHandle, SW_SHOWMAXIMIZED);
            
            
           // sao = SystemAccessibleObject.FromPoint(4, 200);
            try
            {
                string marketwatch=abcd1.ToString();
                if (marketwatch=="0")
                {
                    System.Windows.MessageBox.Show("Nest or now  is not running or Market Watch not present check out and run real time combo again \n     thank you  ");
                 //   closeallprocess();
                    return;
                }
                sao = SystemAccessibleObject.FromWindow(a, AccessibleObjectID.OBJID_WINDOW);
            }
            catch
            {
                System.Windows.MessageBox.Show("Market Watch not found ");
                return;
            }
            SystemAccessibleObject datatable;
           

            f = sao.Children[3];
           

            if (!Directory.Exists(targetpath + "\\NESTbackfill"))
            {
                Directory.CreateDirectory(targetpath + "\\NESTbackfill");
            }

                //take no of symbols for backfill from registry 
            int nosymbol = 15;
            try
            {
                RegistryKey regKey1 = Registry.CurrentUser;
                regKey1 = regKey1.CreateSubKey(@"Windows-xpRT\");
                var noofsymbol = regKey1.GetValue("noofsymbol");
                nosymbol = Convert.ToInt32(noofsymbol);
            }

            catch
            {
            }

                //start backfill data 
            for (int i = 0; i < nosymbol; i++)
            {
                SetForegroundWindow(windowHandle);
               // SendMessage(windowHandle, WM_SCROLL, IntPtr.Zero, IntPtr.Zero);
                if (i == 0)
                {
                    abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                    abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");
                    SendMessage(abcd1, WM_SCROLL, (IntPtr)SB_PAGEUP , IntPtr.Zero);

                    Thread.Sleep(2000);
                }
                if (i == 15 || i==30 | i==45 || i==60)
                {
                    abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                    abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");
                    SendMessage(abcd1, WM_SCROLL, (IntPtr)SB_PAGEDOWN, IntPtr.Zero);
                    
                     Thread.Sleep(2000);
                   
                }
                string filepathforbackfill = "";
                string symbolnametostore = "";
                int mappingsymbolpresentornot = 0;
             //check if mapping symbol present or not 
                for (int j = 0; j  < mappingsymbol.Count() - 1;j++ )
                {
                    try
                    {
                        if (f.Children[i].Name == symbolname[j])
                        {
                            mappingsymbolpresentornot = 1;
                            symbolnametostore = mappingsymbol[j];
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                if(mappingsymbolpresentornot==0)
                {
                    symbolnametostore = f.Children[i].Name;
                }
                try
                {
                    if (!File.Exists(targetpath + "\\NESTbackfill\\" + symbolnametostore + ".csv"))
                    {
                        backfill(f.Children[i]);

                        SetForegroundWindow(windowHandle);
                       
                        SendKeys.SendWait("P");
                        SendKeys.SendWait("D");
                        int flagfordatatable = 0;
                       // Thread.Sleep(12000);

                        try
                        {
                            //check datatable for 20 sec 
                            for (int datacount = 0; datacount < 60;datacount++ )
                            {
                                Thread.Sleep(1000);

                                abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770 (Dialog)", "");

                                abcd1 = FindChildWindow(abcd, IntPtr.Zero, null, "DataTable : " + f.Children[i].Name);
                                abcd2 = FindChildWindow(abcd1, IntPtr.Zero, "SysListView32", null);
                                string datatablepresent1 = abcd1.ToString();
                                if (datatablepresent1 != "0")
                                {
                                    //datatable found exit for loop 
                                    datacount = 65;
                                    flagfordatatable = 1;
                                }

                            }
                            //if data table found then goto next process else dont do following command 

                            if (flagfordatatable == 1)
                            {
                                ShowWindow(abcd1, SW_SHOWMAXIMIZED);
                                //Thread.Sleep(2000);

                                string datatablepresent = abcd1.ToString();

                                if (datatablepresent != "0")
                                {
                                    SetForegroundWindow(abcd2);

                                    SystemWindow table = new SystemWindow(abcd2);

                                    datatable = SystemAccessibleObject.FromWindow(table, AccessibleObjectID.OBJID_WINDOW);

                                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(f.Children[i ].Location.X + 50), Convert.ToInt32(f.Children[i ].Location.Y + 50));
                                    //Thread.Sleep(2000);

                                    VirtualMouse.LeftClick();

                                    SendKeys.SendWait("Shift+E");
                                    filepathforbackfill = "  C:\\myshubhalabha\\NESTbackfill\\" + symbolnametostore + ".csv";

                                    //  filepathforbackfill = symbolnametostore + ".csv";

                                    SendKeys.SendWait(filepathforbackfill.ToString());
                                    VirtualMouse.LeftClick();
                                    Thread.Sleep(1000);



                                    SendKeys.SendWait("{ENTER}");
                                    SendKeys.SendWait("{ENTER}");
                                    // Thread.Sleep(3000);

                                    IntPtr saveas = FindChildWindow(windowHandle, IntPtr.Zero, "#32770 (Dialog)", "");


                                    IntPtr saveas1 = FindChildWindow(saveas, IntPtr.Zero, null, "Save As");
                                    string saveaswindowpresentornot = saveas1.ToString();
                                    if (saveaswindowpresentornot != "0")
                                    {
                                        CloseWindow(saveas1);
                                        SendKeys.SendWait("{ENTER}");
                                    }

                                    CloseWindow(abcd1);

                                    Thread.Sleep(3000);
                                }
                            }
                        }
                        catch
                        {

                        }

                    }

                }
                catch
                {

                }
                
                
                    
                    
            }


             //load data into charting application 
            string[] nestnowfilePaths = Directory.GetFiles(@"C:\myshubhalabha\NESTbackfill\", "*.csv");

            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            var Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath");
            var chartforbackfill = regKey.GetValue("chartingappforbackfill");

            if (chartforbackfill.ToString() == "Amibroker")
            {
                ExcelType = Type.GetTypeFromProgID("Broker.Application");
                ExcelInst = Activator.CreateInstance(ExcelType);
                ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                          ExcelInst, new object[1] { true });

                ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                    ExcelInst, new string[1] { Amibrokerdatapath.ToString( )});
            }
           string datetostore = "";
           for (int i = 0; i < nestnowfilePaths.Count(); i++)
           {
               try
               {
                  
                   //process backfill data from nest/now
                   if (chartforbackfill.ToString() == "Fchart")
                   {
                       fchart = regKey.GetValue("fchart").ToString();
                   }
                   Executenestnowbackfillrocessing(nestnowfilePaths[i], datetostore, "GOOGLEEOD", i, nestnowfilePaths[i].ToString());
                  
                 
               }
               catch
               {
               }
           }
           if (chartforbackfill.ToString() == "Amibroker")
           {
               for (int i = 0; i < nestnowfilePaths.Count(); i++)
               {

                   args[0] = Convert.ToInt16(0);
                   args[1] = nestnowfilePaths[i];
                   args[2] = "nestbackfill.format";


                   ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                             ExcelInst, args);
               }
           }
            }
            else if(googleback=="True")
            {


                //getsymbol name from google file 
                getsymbolname();

                googlebackfill();

                string[] nestnowfilePaths = Directory.GetFiles(@"C:\myshubhalabha\GoogleBackfill\", "*.csv");

                //load all files to charting applciation
                var Amibrokerdatapath = "";

                try
                {
                    RegistryKey regKey = Registry.CurrentUser;
                    regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                    Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath").ToString();
                }
                catch
                {
                }
                ExcelType = Type.GetTypeFromProgID("Broker.Application");
                ExcelInst = Activator.CreateInstance(ExcelType);
                ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                          ExcelInst, new object[1] { true });

                ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                    ExcelInst, new string[1] { Amibrokerdatapath });

                for (int i = 0; i < nestnowfilePaths.Count(); i++)
                {

                    args[0] = Convert.ToInt16(0);
                    args[1] = nestnowfilePaths[i];
                    args[2] = "Shubhabackfill.format";


                    ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                              ExcelInst, args);
                }
             

            }
            else if (withoutback=="True")
            {
                ExcelType = Type.GetTypeFromProgID("Broker.Application");
                ExcelInst = Activator.CreateInstance(ExcelType);
                ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                          ExcelInst, new object[1] { true });

                ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                    ExcelInst, new string[1] { amipath });
            }

            System.Windows.MessageBox.Show("Backfill completed");

        }

        //start real time data download and load it to charting applcaition 
        public void startRealtime()
        {







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
                    System.Windows.MessageBox.Show(" Please start NOW  as Run as Administrator and again start Realtime combo");
                    closeallprocess();
                    return;
                }
            }
            getsymbolname();



            IntPtr abcd = new IntPtr();
            IntPtr abcd1 = new IntPtr();
            List<Thread> processtostartback = new List<Thread>();


            foreach (Process p in processes)
            {
                windowHandle = p.MainWindowHandle;

                //System.Windows.Forms.MessageBox.Show(p.HandleCount.ToString());

                abcd = FindChildWindow(windowHandle, IntPtr.Zero, "#32770", "");
                abcd1 = FindChildWindow(abcd, IntPtr.Zero, "SysListView32", "");

                // do something with windowHandle
            }

            SystemWindow a = new SystemWindow(abcd1);
            ShowWindow(windowHandle, SW_SHOWMAXIMIZED);


            // sao = SystemAccessibleObject.FromPoint(4, 200);
            try
            {
                string marketwatch = abcd1.ToString();
                if (marketwatch == "0")
                {
                    System.Windows.MessageBox.Show("Nest or now  is not running or Market Watch not present check out and run real time combo again \n     thank you  ");
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

            try
            {
                RegistryKey regKey = Registry.CurrentUser;
                regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                 Amibrokerdatapath = regKey.GetValue("Amibrokerdatapath").ToString();
                 Metastockdatapath = regKey.GetValue("Metastockdatapath").ToString();
                fchart = regKey.GetValue("fchart").ToString();
                
           
            }
            catch
            {
            }
            //satrt amiboker 
            if (chartingaplication == "Amibroker")
            {
                ExcelType = Type.GetTypeFromProgID("Broker.Application");
                ExcelInst = Activator.CreateInstance(ExcelType);
                ExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null,
                          ExcelInst, new object[1] { true });

                ExcelType.InvokeMember("LoadDatabase", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                    ExcelInst, new string[1] { Amibrokerdatapath });
            }
            //call process as user given time 
            RtdataRecall();

        }


        //Google backfill 
      public   void googlebackfill()
        {
            string targetpath = ConfigurationManager.AppSettings["targetpathforcombo"];

            string strYearDir = targetpath + "\\Downloads\\Googleeod";

            if (!Directory.Exists(strYearDir))
                Directory.CreateDirectory(strYearDir);
          



            //{ "LICHSGFIN.nse","ADANIENT.nse","ADANIPOWE.nse","ADFFOODS.nse","ADHUNIK.nse","ADORWELD.nse","ADSL.nse","ADVANIHOT.nse","ADVANTA.nse","AEGISCHEM.nse","AFL.nse","AFTEK.nse","AREVAT&D.nse","M&M.nse",".AEX,indexeuro",".AORD,indexasx",".HSI,indexhangseng",",.N225,indexnikkei",".NSEI,nse",".NZ50,nze",".TWII,tpe","000001,sha","CNX100,nse","CNX500,nse","CNXENERGY,nse","CNXFMCG,nse","CNXINFRA,nse","CNXIT,nse"};
           
           

            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            int  googleday =Convert.ToInt32( regKey.GetValue("googleday"));
            var googletime =Convert.ToInt32(  regKey.GetValue("googletime"));
            string  timeforgoogle = "0";
          if(googletime==1)
          {
              timeforgoogle = "60";
          }
          if (googletime == 5)
          {
              timeforgoogle = "300";
          }
            for (int i = 0; i < googlesymbol.Count(); i++)
            {
                string[] words = googlesymbol[i].Split(':');

                //No symbol selected then dont do backfill 
                if (words[1] != "No symbol selected")
                {



                    strYearDir = targetpath + "\\Downloads\\Googleeod\\" + words[1] + ".csv";
                    //string baseurl = "http://www.google.com/finance/getprices?q=" + words[1] + "&x=" + words[0] + "&i="+timeforgoogle +"&p="+Convert.ToInt32 (googleday) +"d&f=d,o,h,l,c,v&df=cpct&auto=1&ts=1266701290218";
                    string baseurl = "http://www.google.com/finance/getprices?q=" + words[1] + "&x=" + words[0] + "&i=60&p=6d&f=d,o,h,l,c,v&df=cpct&auto=1&ts=1266701290218";

                  //  string baseurl = "http://www.google.com/finance/getprices?q=" + GoogleEod[i] + "&x=" + GoogleEodExchang[i] + "&i=" + mindata + "&p=" + Convert.ToInt32(Daysforgoogle.SelectedItem) + "d&f=d,o,h,l,c,v&df=cpct&auto=1&ts=1266701290218";
                    
                    // "http://www.google.com/finance/getprices?q=LICHSGFIN&x=LICHSGFIN&i=d&p=15d&f=d,o,h,l,c,v"
                    //http://www.google.com/finance/getprices?q=RELIANCE&x=NSE&i=60&p=5d&f=d,c,o,h,l&df=cpct&auto=1&ts=1266701290218 [^]

                    
                    //download file from googlefin 
                    downliaddata(strYearDir, baseurl);



                    try
                    {
                        string[] csvFileNames = new string[1] { "" };
                        csvFileNames[0] = targetpath + "\\Downloads\\Googleeod\\" + words[1] + ".csv";




                        string datetostore = "";

                        if (mappingsymbol[i]=="NO")
                      {
                          mappingsymbol[i] = words[1];
                      }
                        //process downloaded file 
                        ExecuteYAHOOProcessing(csvFileNames, datetostore, "GOOGLEEOD", i, mappingsymbol[i]);
                        if (!Directory.Exists(targetpath + "\\STD_CSV\\\\GoogleEod"))
                        {
                            Directory.CreateDirectory(targetpath + "\\STD_CSV\\\\GoogleEod");
                        }
                        if (!Directory.Exists(targetpath + "\\STD_CSV\\GoogleEod"))
                        {
                            Directory.CreateDirectory(targetpath + "\\GoogleEod");
                        }

                        if (!Directory.Exists(targetpath + "\\GoogleBackfill"))
                        {
                            Directory.CreateDirectory(targetpath + "\\GoogleBackfill");
                        }
                       //join all csv files 
                        JoinCsvFiles(csvFileNames, targetpath + "\\GoogleBackfill\\" +mappingsymbol[i] + ".csv");





                      







                    }
                    catch 
                    {
                        
                    }
                }
            }
                   

                

        }

        //join csv files present in perticular folder
      private static void JoinCsvFiles(string[] csvFileNames, string outputDestinationPath)
      {
          StringBuilder sb = new StringBuilder();

          bool columnHeadersRead = false;

          foreach (string csvFileName in csvFileNames)
          {
              TextReader tr = new StreamReader(csvFileName);

              string columnHeaders = tr.ReadLine();

              // Skip appending column headers if already appended
              if (!columnHeadersRead)
              {
                  sb.AppendLine(columnHeaders);
                  columnHeadersRead = true;
              }




              sb.AppendLine(tr.ReadToEnd());

              tr.Close();


          }


          File.WriteAllText(outputDestinationPath, sb.ToString());


      }
        //Processing of google backfill file
      public void ExecuteYAHOOProcessing(string[] strBSECSVArr, string datetostore, string name, int count, string mappingsymbol)
      {

          if (name == "GOOGLEEOD")
          {
              FileHelperEngine engineBSECSV1 = new FileHelperEngine(typeof(GOOGLE));




              foreach (string obj in strBSECSVArr)
              {

                  //Get BSE Equity Filename day, month, year
                  string[] words = obj.Split('\\');

                  string strbseequityfilename = words[words.Length - 1];

                  //load file 
                  GOOGLE[] resbsecsv1 = engineBSECSV1.ReadFile(obj) as GOOGLE[];


                  GOOGLEFINAL[] finalarr = new GOOGLEFINAL[resbsecsv1.Length];
                  int icntr = 0;
                 
                  DateTime timefromyahoo = DateTime.Today;

                 
                  long valueforgoogletime = 1;
                  
                  while (icntr < resbsecsv1.Length)
                  {
                      finalarr[icntr] = new GOOGLEFINAL();
                      if (resbsecsv1[icntr].Name.Contains('a'))
                      {
                          valueforgoogletime = Convert.ToInt64(resbsecsv1[icntr].Name.Substring(1, resbsecsv1[icntr].Name.Length - 1));
                      }

                      timefromyahoo = new DateTime(1970, 1, 1, 5, 30, 0).AddSeconds(valueforgoogletime);

                      RegistryKey regKey = Registry.CurrentUser;
                      regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                      int googleday = Convert.ToInt32(regKey.GetValue("googleday"));
                      var googletime = Convert.ToInt32(regKey.GetValue("googletime"));
                      int mindata = 60;
                     
                      if (googletime == 1)
                      {
                          mindata = 60;
                      }
                      if (googletime == 5)
                      {
                          mindata = 300;
                      }
                     

                      valueforgoogletime = valueforgoogletime + mindata;

                      string timetostore = timefromyahoo.Hour.ToString() + ":" + timefromyahoo.Minute.ToString() + ":" + timefromyahoo.Millisecond.ToString();



                      datetostore =DateTime.Today.Day+"-"+DateTime.Today.Month+"-"+DateTime.Today.Year  ;
                      //finalarr[icntr].ticker = strbseequityfilename.Substring(0, strbseequityfilename.Length - 4);
                      //finalarr[icntr].name = strbseequityfilename.Substring(0, strbseequityfilename.Length - 4); ;

                      finalarr[icntr].ticker = mappingsymbol;
                      finalarr[icntr].name = mappingsymbol;

                     
                      finalarr[icntr].date = datetostore; // String.Format("{0:yyyyMMdd}", myDate);
                      finalarr[icntr].open = resbsecsv1[icntr].OPEN_PRICE;
                      finalarr[icntr].high = resbsecsv1[icntr].HIGH_PRICE;
                      finalarr[icntr].low = resbsecsv1[icntr].LOW_PRICE;
                      finalarr[icntr].close = resbsecsv1[icntr].CLOSE_PRICE;
                      finalarr[icntr].volume = resbsecsv1[icntr].volume;
                      finalarr[icntr].time = timetostore;

                      finalarr[icntr].openint = 0;  //enint;


                      icntr++;
                  }

                  FileHelperEngine engineBSECSVFINAL = new FileHelperEngine(typeof(GOOGLEFINAL));
                 

                  engineBSECSVFINAL.WriteFile(obj, finalarr);
                 
              }
              return;


          }











      }
      public string yahootime(DateTime timetostore)
      {


          if (timetostore.Hour == 03)
          {
              if (timetostore.Minute > 30)
              {
                  return "19:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "20:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          if (timetostore.Hour == 04)
          {
              if (timetostore.Minute > 30)
              {
                  return "20:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "21:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          if (timetostore.Hour == 05)
          {
              if (timetostore.Minute > 30)
              {
                  return "21:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "22:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          if (timetostore.Hour == 06)
          {
              if (timetostore.Minute > 30)
              {
                  return "22:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "23:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          if (timetostore.Hour == 07)
          {
              if (timetostore.Minute > 30)
              {
                  return "23:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "24:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          if (timetostore.Hour == 08)
          {
              if (timetostore.Minute > 30)
              {
                  return "24:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "24:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }

          if (timetostore.Hour == 13)
          {
              if (timetostore.Minute > 30)
              {
                  return "00:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "00:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }



          else if (timetostore.Hour == 14)
          {
              if (timetostore.Minute < 30)
              {
                  return "00:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "01:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 15)
          {
              if (timetostore.Minute < 30)
              {
                  return "01:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "02:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 16)
          {
              if (timetostore.Minute < 30)
              {
                  return "02:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "03:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }

          else if (timetostore.Hour == 17)
          {
              if (timetostore.Minute < 30)
              {
                  return "03:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "04:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 18)
          {
              if (timetostore.Minute < 30)
              {
                  return "04:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "05:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }

          else if (timetostore.Hour == 19)
          {
              if (timetostore.Minute < 30)
              {
                  return "05:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "06:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 20)
          {
              if (timetostore.Minute < 30)
              {
                  return "06:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "07:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 21)
          {
              if (timetostore.Minute < 30)
              {
                  return "07:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "08:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 22)
          {
              if (timetostore.Minute < 30)
              {
                  return "08:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "09:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 23)
          {
              if (timetostore.Minute < 30)
              {
                  return "09:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "10:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }
          else if (timetostore.Hour == 24)
          {
              if (timetostore.Minute < 30)
              {
                  return "10:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
              else
              {
                  return "11:" + timetostore.Minute.ToString() + ":" + timetostore.Second.ToString();

              }
          }


          return null;
      }
        //Download Google backfill data
      private void downliaddata(string path, string url)
      {
          //download data from internet 

          try
          {
      
              Client.Headers.Add("Accept", "application/zip");
              Client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
              Client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1");
              Client.DownloadFile(url, path);

          }
          catch (Exception ex)
          {
              if ((ex.ToString().Contains("404")) || (ex.ToString().Contains("400")))
              {
                 

              }
          }


      }
        //Timer for call realtime function
        private void dispatcherTimerForRT_Tick(object sender, EventArgs e)
        {
            if (terminalname == "NEST")
            {
                LoadTree(f);
            }
            if (terminalname == "NOW")
            {
                Nowdata (f);
            }
          //  RtdataRecall();

        }
        //Timer for call realtime function

        private void RtdataRecall()
        {


            
                 RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
            var timesec = regKey.GetValue("timesec");

          

            int sec = Convert.ToInt32(timesec);
            if (sec==0)
            {
                sec = 1;
            }
            else if (sec == 1)
            {
                sec = 3;
            }
            else if (sec == 2)
            {
                sec = 5;
            }
            else if (sec == 3)
            {
                sec = 10;
            }
            else if (sec == 4)
            {
                sec = 20;
            }
            else if (sec == 5)
            {
                sec = 30;
            }
            DispatcherTimer1.Tick += new EventHandler(dispatcherTimerForRT_Tick);
            DispatcherTimer1.Interval = new TimeSpan(0, 0, sec);
            DispatcherTimer1.Start();
            CommandManager.InvalidateRequerySuggested();

        }

        //Mouse click on perticular point
        private void ClickOnPoint(IntPtr wndHandle,System.Windows.Point  clientPoint)
        {

            /// get screen coordinates
            ClientToScreen(wndHandle, ref clientPoint);
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(clientPoint.X), Convert.ToInt32(clientPoint.Y));


            /// set cursor on coords, and press mouse
            mouse_event(0x00000002, 0, 0, 0, UIntPtr.Zero); /// left mouse button down
            mouse_event(0x00000004, 0, 0, 0, UIntPtr.Zero); /// left mouse button up
                                                            /// 
            VirtualMouse.RightClick();
             
                                                            /// 
            /// return mouse 
        }
       
        //Mouse click on Symbol name point
        public void backfill(SystemAccessibleObject backfillsymbolname)
        {
           System.Windows. Point a1 = new System.Windows.Point (Convert.ToDouble(backfillsymbolname.Location.X), Convert.ToDouble(backfillsymbolname.Location.Y));
          
            ClickOnPoint(windowHandle, a1);

        }

        //Checking all require fieldes from NEST terminal 
        public int  checknestfiled()
        {
            Process[] processes = null;

           
                try
                {
                    type = Type.GetTypeFromProgID("nest.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);
                    processes = Process.GetProcessesByName("NestTrader");


                }
                catch
                {
                    System.Windows.MessageBox.Show(" Please start Nest as Run as Administrator and again start Realtime combo");
                    return 0;
                }
            
           
            IntPtr abcd = new IntPtr();
            IntPtr abcd1 = new IntPtr();
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
                return 0;
            }

            SystemAccessibleObject finalobject;
            SystemAccessibleObject f = sao.Children[3];
            int flag = 0;

            for (int j = 0; j < 1;j++ )
            {
                finalobject = f.Children[j ];
                string s1 = finalobject.Description;
                string[] checkterminalcol = s1.Split(',');
                string marketwathrequiredfield = "";

                for (int i = 0; i < checkterminalcol.Count(); i++)
                {
                    marketwathrequiredfield = marketwathrequiredfield + checkterminalcol[i].ToString();
                }


                if (!marketwathrequiredfield.Contains("LTT"))
                {
                    flag = 1;

                }
                else
                {
                    flag = 0;
                }
                if (!marketwathrequiredfield.Contains("LTP"))
                {
                    flag = 1;

                }
                else
                {
                    flag = 0;
                }
                if (!marketwathrequiredfield.Contains("Volume Traded Today"))
                {
                    flag = 1;

                }
                else
                {
                    flag = 0;
                }
                if (!marketwathrequiredfield.Contains("Open Interest"))
                {
                    flag = 1;

                }
                else
                {
                    flag = 0;
                }
                if (!checkterminalcol[0].Contains("LTT"))
                {
                    flag = 1;
                    // System.Windows.MessageBox.Show("LTT Should be at second position in the market watch (Trading symbol ,LTT and so on ...)");

                }
                else
                {
                    flag = 0;
                }
            }
            if (flag == 1)
            {
                System.Windows.MessageBox.Show("Mandatory fields are missing or value LTT not present   in nest Terminal please set mandatory fields as below \nTrading symbol | LTT | LUT | LTP | Volume Traded Today | Open Interest  \n Note- Please set LTT adjacent to symbol name  ");
                return 0;
            }

            return 1;
        }
        //Checking all require fieldes from NOW terminal 

        public void checknowfiled()
        {
            Process[] processes = null;

         
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
                    System.Windows.MessageBox.Show(" Please start NOW as Run as Administrator and again start Realtime combo");
                    closeallprocess();
                    return;
                }
            }
            IntPtr abcd = new IntPtr();
            IntPtr abcd1 = new IntPtr();
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


            if (!marketwathrequiredfield.Contains("Last Trade Time"))
            {
                flag = 1;

            }
            if (!marketwathrequiredfield.Contains("Last Traded Price"))
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
            if (!checkterminalcol[0].Contains("Last Trade Time"))
            {
                flag = 1;
                // System.Windows.MessageBox.Show("LTT Should be at second position in the market watch (Trading symbol ,LTT and so on ...)");

            }
            if (flag == 1)
            {
                System.Windows.MessageBox.Show("Mandatory fields are missing in now Terminal please set mandatory fields as below \nTrading symbol | LTT | LUT | LTP | Volume Traded Today | Open Interest  \n Note- Please set LTT adjacent to symbol name  ");
                closeallprocess();
            }
        }
        //Close all running process of our application 
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

        //Real time data of NEST terminal 
        private void LoadTree(SystemAccessibleObject f )
        {





            if (terminalname == "NEST")
            {
                try
                {
                    type = Type.GetTypeFromProgID("nest.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);

                }
                catch
                {
                    System.Windows.MessageBox.Show(" Please start NEST as Run as Administrator and again start Realtime combo");
                    closeallprocess();
                    return;
                }
            }
            try
            {
                CommandManager.InvalidateRequerySuggested();





                string datatostore = "";
                string LTP = "";
                string volume = "";
                string LTT = "";
                string openint = "";
                string symbolnametosave = "";

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
                    System.Windows.MessageBox.Show("Some required fileds are missing in market watch please add that fileds Otherwise it wil give wrong data  ");

                   // closeallprocess();
                }
                 
               

                //read data from nest window 
                for (int i = 0; i < f.Children.Count() - 1; i++)
                {
                    LTP = "";
                    volume = "";
                    LTT = "";
                    openint = "";
                    symbolnametosave = "";

                    finalobject = f.Children[i] ;
                     s = finalobject.Description;

                  //   string result = Regex.Replace(f.Children[i].Description, @"[^0-9,.]+", "");
                  //   result = f.Children[i].Name  + "," + datetimetostore + "," + result;
                  //datatostore = datatostore+result+"\r\n";
                     words = s.Split(',');
                    symbolnametosave = finalobject.Name;

                    int mappingsymbolpresentornot = 0;
                    //mapping symbol 
                    for (int j = 0; j < mappingsymbol.Count() - 1; j++)
                    {
                        try
                        {
                            if (finalobject.Name.ToString() == symbolname[j].ToString())
                            {
                                mappingsymbolpresentornot = 1;
                                symbolnametosave = mappingsymbol[j].ToString();
                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (mappingsymbolpresentornot == 0)
                    {
                        symbolnametosave = f.Children[i].Name;
                    }




                    int i1 = words.Count();
                    for (int j = 0; j < i1; j++)
                    {

                         words1 = words[j].Split(':');

                       
                      


                        if (words1[0] == " LTP")
                        {
                            LTP = words1[1];
                        }
                        if (words1[0] == "LTT")
                        {
                            if (words1[1] != "")
                            {
                                LTT = DateTime.Today.Date.ToShortDateString() + "," + words1[1] + ":" + words1[2] + ":" + words1[3];
                                
                                LTT = datetimetostore  + "," + words1[1] + ":" + words1[2] + ":" + words1[3];
                                if (chartingaplication == "Fchart")
                                {
                                    LTT = datetimetostore + "," + words1[1] + ":" + words1[2];

                                }
                            }
                        }

                        if (words1[0] == " Volume Traded Today")
                        {
                            volume = words1[1];
                        }
                        if (words1[0] == " Open Interest")
                        {
                            volume = words1[1];
                        }
                        if (words1[0] == " LUT")
                        {
                             luttime = words1[1].Split(' ');

                            LTT = DateTime.Today.Date.ToShortDateString() + "," + luttime[2] + ":" + words1[2] + ":" + words1[3];
                        }

                    }
                    if (openint == "")
                    {
                        openint = "0";
                    }
                    if (LTT != "")
                    {

                        if (chartingaplication == "Amibroker")
                        {
                            datatostore = datatostore + symbolnametosave + "," + LTT + "," + LTP + "," + volume + "," + openint + "\r\n";
                        }
                        if (chartingaplication == "Metastock")
                        {
                            // "<TICKER>,<NAME>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>\r\n"
                            datatostore = datatostore + symbolnametosave + "," + symbolnametosave + "," + "I" + "," + LTT + "," + LTP + "," + LTP + "," + LTP + "," + LTP + "," + volume + "\r\n";
                        }
                        if (chartingaplication == "Fchart")
                        {
                            datatostore = datatostore + symbolnametosave + "," + LTT + "," + LTP + "," + LTP + "," + LTP + "," + LTP + "," + volume + "\r\n";

                        }
                    }
                    else
                    {
                        try
                        {
                            RegistryKey regKey = Registry.CurrentUser;
                            regKey = regKey.CreateSubKey(@"Windows-xpRT\");
                            var lttreg = regKey.GetValue("LTT");
                            string lttreg1 = "";
                            if (lttreg!=null )
                            {
                             lttreg1 = lttreg.ToString();
                            }

                            if (lttreg1=="1")
                            {
                                LTT = DateTime.Today.Date.ToShortDateString() + "," + DateTime.Today.TimeOfDay.Hours + ":" + DateTime.Today.TimeOfDay.Minutes + ":" + DateTime.Today.TimeOfDay.Seconds ;
                                
                            datatostore = datatostore + symbolnametosave + "," + LTT + "," + LTP + "," + volume + "," + openint + "\r\n";

                            if (chartingaplication == "Amibroker")
                            {
                                datatostore = datatostore + symbolnametosave + "," + LTT + "," + LTP + "," + volume + "," + openint + "\r\n";
                            }
                            if (chartingaplication == "Metastock")
                            {
                                // "<TICKER>,<NAME>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>\r\n"
                                datatostore = datatostore + symbolnametosave + "," + symbolnametosave + "," + "I" + "," + LTT + "," + LTP + "," + LTP + "," + LTP + "," + LTP + "," + volume + "\r\n";
                            }
                            if (chartingaplication == "Fchart")
                            {
                                datatostore = datatostore + symbolnametosave + "," + LTT + "," + LTP + "," + LTP + "," + LTP + "," + LTP + "," + volume + "\r\n";

                            }
                            }
                        }
                        catch
                        {

                        }
                       
                    }


                }

                //load files into charting application 
                if (chartingaplication == "Amibroker")
                {
                    //write data into file 
                    string filename = targetpath + "\\Realtimeamibrokerdata.txt";
                    using (var writer = new StreamWriter(filename))
                        writer.WriteLine(datatostore);

                    ExcelType = Type.GetTypeFromProgID("Broker.Application");
                    ExcelInst = Activator.CreateInstance(ExcelType);
                    args[0] = Convert.ToInt16(0);
                    args[1] = targetpath + "\\Realtimeamibrokerdata.txt";
                    args[2] = "RT.format";
                    ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                         ExcelInst, args);



                    ExcelType.InvokeMember("RefreshAll", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                           ExcelInst, new object[1] { "" });
                    return;
                }
                if (chartingaplication == "Metastock")
                {
                    string filename = targetpath + "\\RealtimeMetastockdata.txt";
                    datatostore = "<TICKER>,<NAME>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>\r\n" + datatostore;
                    using (var writer = new StreamWriter(filename))
                        writer.WriteLine(datatostore);


                    if (!Directory.Exists(Metastockdatapath  + "\\Intraday\\Metastock"))
                    {
                        Directory.CreateDirectory(Metastockdatapath + "\\Intraday\\Metastock");
                    }
                    // commandpromptcall(filename, targetpath + "\\Intraday\\Metastock\\realtimemetastock");
                    try
                    {

                        string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                        string processtostart = filepath.Substring(0, filepath.Length - 18) + "asc2ms.exe";

                        File.Copy(processtostart, targetpath + "\\asc2ms.exe", true);
                    }
                    catch
                    {
                    }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C  " + targetpath + "\\asc2ms.exe -f " + filename + " -r r -o " + Metastockdatapath  + "\\Intraday\\Metastock\\realtimemetastock  --forceWrite=yes --verbosity high";



                    process.StartInfo = startInfo;
                    process.Start();

                    return;

                }
                if (chartingaplication == "Fchart")
                {
                    if (!Directory.Exists(fchart  + "\\Fchart"))
                    {
                        Directory.CreateDirectory(fchart  + "\\Fchart");
                    }
                    string filename = fchart  + "\\Fchart\\RealtimeFchartdata.txt";
                    using (var writer = new StreamWriter(filename))
                        writer.WriteLine(datatostore);


                    return;
                }
                
               
                f = null;
                
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message );
            }
         //   Thread.Sleep(100);
         //  System.GC.Collect();
        }
        //Real time data of NOW terminal 

        private void Nowdata(SystemAccessibleObject f)
        {
            //check now required filed of now terminal 
            
            if (terminalname == "NOW")
            {
                try
                {
                    type = Type.GetTypeFromProgID("now.scriprtd");

                    m_server = (IRtdServer)Activator.CreateInstance(type);

                }
                catch
                {
                    System.Windows.MessageBox.Show(" Please start NOW as Run as Administrator and again start Realtime combo");
                    closeallprocess();
                    return;
                }
            }
            try
            {
                CommandManager.InvalidateRequerySuggested();





                string datatostore = "";
                string LTP = "";
                string volume = "";
                string LTT = "";
                string openint = "";
                string symbolnametosave = "";

                finalobject = f.Children[0];
                string s1 = finalobject.Description;
                int flag = 0;
                string[] checkterminalcol = s1.Split(',');
                string marketwathrequiredfield = "";
                for (int i = 0; i < checkterminalcol.Count(); i++)
                {
                    marketwathrequiredfield = marketwathrequiredfield + checkterminalcol[i].ToString();
                }


                if (!marketwathrequiredfield.Contains("Last Trade Time"))
                {
                    flag = 1;

                }
                if (!marketwathrequiredfield.Contains("Last Traded Price"))
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
                if (!checkterminalcol[0].Contains("Last Trade Time"))
                {
                    flag = 1;

                }
                if (flag == 1)
                {
                    System.Windows.MessageBox.Show("Some required fileds are missing in market watch please add that fileds and try shubha real time combo again \n Thank you  ");

                    closeallprocess();
                }
                 

                //load data from window handle 
                for (int i = 0; i < f.Children.Count() - 1; i++)
                {
                    LTP = "";
                    volume = "";
                    LTT = "";
                    openint = "";
                    symbolnametosave = "";
                    finalobject = f.Children[i];
                    string s = finalobject.Description;


                    string[] words = s.Split(',');
                    symbolnametosave = finalobject.Name;

                    int mappingsymbolpresentornot = 0;
                    for (int j = 0; j < mappingsymbol.Count() - 1; j++)
                    {
                        try
                        {
                            if (finalobject.Name.ToString() == symbolname[j].ToString())
                            {
                                mappingsymbolpresentornot = 1;
                                symbolnametosave = mappingsymbol[j].ToString();
                                break;
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (mappingsymbolpresentornot == 0)
                    {
                        symbolnametosave = f.Children[i].Name;
                    }




                    int i1 = words.Count();
                    for (int j = 0; j < i1; j++)
                    {

                        string[] words1 = words[j].Split(':');





                        if (words1[0] == " Last Traded Price")
                        {
                            LTP = words1[1];
                        }
                        if (words1[0] == "Last Trade Time")
                        {
                            if (words1[1] != "")
                            {
                                // LTT = DateTime.Today.Date.ToShortDateString() + "," + words1[1] + ":" + words1[2] + ":" + words1[3];

                                LTT = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year + "," + words1[1] + ":" + words1[2] + ":" + words1[3];
                            }
                        }

                        if (words1[0] == " Volume Traded Today")
                        {
                            volume = words1[1];
                        }
                        if (words1[0] == " Open Interest")
                        {
                            volume = words1[1];
                        }
                        

                    }
                    //  datatostore = datatostore + "\r\n";
                    if (openint == "")
                    {
                        openint = "0";
                    }
                    if (LTT != "")
                    {
                        //datatostore = datatostore + symbolnametosave + "," + LTT + "," + LTP + "," + volume + "," + openint + "\r\n";
                        if (chartingaplication == "Amibroker")
                        {
                            datatostore = datatostore + symbolnametosave + "," + LTT + "," + LTP + "," + volume + "," + openint + "\r\n";
                        }
                        if (chartingaplication == "Metastock")
                        {
                            //"<TICKER>,<NAME>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>\r\n"
                            datatostore = datatostore + symbolnametosave + "," + symbolnametosave + "," + "I" + "," + LTT + "," + LTP + "," + LTP + "," + LTP + "," + LTP + "," + volume + "\r\n";
                        }
                        if (chartingaplication == "Fchart")
                        {
                            datatostore = datatostore + symbolnametosave +  "," + LTT + "," + LTP + "," + LTP + "," + LTP + "," + LTP + "," + volume + "\r\n";

                        }
                    }

                    //   System.Windows.MessageBox.Show(words1[1]);

                }

                //load files into charting application 
                if (chartingaplication == "Amibroker")
                {
                    string filename = targetpath + "\\Realtimeamibrokerdata.txt";
                    //   System.Windows.MessageBox.Show(realtimemetastock);
                    using (var writer = new StreamWriter(filename))
                        writer.WriteLine(datatostore);

                    ExcelType = Type.GetTypeFromProgID("Broker.Application");
                    ExcelInst = Activator.CreateInstance(ExcelType);
                    args[0] = Convert.ToInt16(0);
                    args[1] = targetpath + "\\Realtimeamibrokerdata.txt";
                    args[2] = "RT.format";
                    ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                         ExcelInst, args);


                    CommandManager.InvalidateRequerySuggested();

                    ExcelType.InvokeMember("RefreshAll", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                           ExcelInst, new object[1] { "" });
                }
                if (chartingaplication == "Metastock")
                {
                    string filename = targetpath + "\\RealtimeMetastockdata.txt";
                    datatostore = "<TICKER>,<NAME>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOLUME>\r\n" + datatostore;
                    using (var writer = new StreamWriter(filename))
                        writer.WriteLine(datatostore);


                    if (!Directory.Exists(targetpath + "\\Intraday\\Metastock"))
                    {
                        Directory.CreateDirectory(targetpath + "\\Intraday\\Metastock");
                    }
                    // commandpromptcall(filename, targetpath + "\\Intraday\\Metastock\\realtimemetastock");
                    try
                    {

                        string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
                        string processtostart = filepath.Substring(0, filepath.Length - 18) + "asc2ms.exe";

                        File.Copy(processtostart, targetpath + "\\asc2ms.exe", true);
                    }
                    catch
                    {
                    }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    //startInfo.Arguments = "/C  C:\\asc2ms.exe -f C:\\data\\Metastock\\M.csv -r r -o C:\\data\\Metastock\\google\\e";
                    startInfo.Arguments = "/C  " + targetpath + "\\asc2ms.exe -f " + filename + " -r r -o " + targetpath + "\\Intraday\\Metastock\\realtimemetastock";
                    // startInfo.Arguments = @"/C  C:\asc2ms.exe -f C:\Documents and Settings\maheshwar\My Documents\BSe\Downloads\Googleeod -r r -o C:\Documents and Settings\maheshwar\My Documents\BSe\Downloads\Googleeod\Metastock\a" ;



                    process.StartInfo = startInfo;
                    process.Start();



                }
                if (chartingaplication == "Fchart")
                {
                    if (!Directory.Exists(targetpath + "\\Fchart"))
                    {
                        Directory.CreateDirectory(targetpath + "\\Fchart");
                    }
                    string filename = targetpath + "\\Fchart\\RealtimeFchartdata.txt";
                    using (var writer = new StreamWriter(filename))
                        writer.WriteLine(datatostore);
                }


            }
            catch (Exception ex)
            {
               // System.Windows.MessageBox.Show(ex.Message);
            }


        }
    }
    
}
