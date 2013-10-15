//////////////////////////////////////////////////
//This software (released under GNU GPL V3) and you are welcome to redistribute it under certain conditions as per license 
///////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Shubharealtime
{
    public class MyData
    {
        /// <summary>
        /// Saves items to Symbolfile.txt file in bin folder.
        /// </summary>
        /// <param name="items"></param>
        public void Save(System.Windows.Data.CollectionView items)
        {
            string filename = "C:\\myshubhalabha\\Symbolname.csv";

           if(File.Exists(filename ))
           {
               File.Delete(filename);
           }

            foreach (var item in items)
            {
                ListViewData lvc = (ListViewData)item;

               
                string datatostore = lvc.Col1 + "," + lvc.Col2 + "," + lvc.Col3;
            using (var writer = new StreamWriter(filename,true ))
                writer.WriteLine(datatostore);

            }
            System.Windows.MessageBox.Show("Symbol file saved ");
        }

        /// <summary>
        /// Gets data from Symbolfile.txt as rows.  
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetRows()
        {
            List<ListViewData> rows = new List<ListViewData>();

            if (File.Exists("C:\\myshubhalabha\\Symbolfile.txt"))
            {
                // Create the query 
                var rowsFromFile = from c in XDocument.Load(
                            "Symbolfile.txt").Elements(
                            "Data").Elements("Rows").Elements("Row")
                                   select c;

                // Execute the query 
                foreach (var row in rowsFromFile)
                {
                    rows.Add(new ListViewData(row.Element("col1").Value,
                            row.Element("col2").Value, row.Element("col3").Value));
                }
            }
            return rows;
        }
    }
}
