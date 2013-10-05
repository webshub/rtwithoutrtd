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

namespace Shubharealtime
{
    class Backfill
    {
        SystemAccessibleObject backfilldata;
          Type ExcelType;
        object ExcelInst;
        object[] args = new object[3];

        public void writedatatofile(SystemAccessibleObject backfillfile, string targetpath)
        {
            if (!Directory.Exists(targetpath+"\\BackfillFile"))
            {
                Directory.CreateDirectory(targetpath + "\\BackfillFile");

            }

            string filename = targetpath + "\\BackfillFile\\" + backfillfile.Children[0].Name;

            try
            {

                for (int j = 0; j <100; j++)
                {
                    backfilldata = backfillfile.Children[j];
                    string [] datatostore=backfilldata.Description.Split(' ');
                    string data =backfilldata.Name+","+ datatostore[1] + "," + datatostore[2] + datatostore[4] + datatostore[6] + datatostore[8] + datatostore[10] + datatostore[12];
                    using (var writer = new StreamWriter(filename, true))
                        writer.WriteLine(data);
                   // Time: 06-09-2013 09:15:00, Open: 2000.0000, High: 2009.0000, Low: 2000.0000, Close/Price: 2005.1500, Volume: 20352

                }
                ExcelType = Type.GetTypeFromProgID("Broker.Application");
                ExcelInst = Activator.CreateInstance(ExcelType);
                args[0] = Convert.ToInt16(0);
                args[1] = filename ;
                args[2] = "backfill.format";
                ExcelType.InvokeMember("Import", BindingFlags.InvokeMethod | BindingFlags.Public, null,
                     ExcelInst, args);


                System.Windows.MessageBox.Show("Backfill Completed for " + backfillfile.Children[0].Name);
            }
            catch
            {

            }

        }


    }
}
