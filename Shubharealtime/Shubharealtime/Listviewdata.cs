using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shubharealtime
{
     public class ListViewData
    {
        public ListViewData()
        {
            // default constructor
        }

        public ListViewData(string col1, string col2,string col3)
        {
            Col1 = col1;
            Col2 = col2;
            Col3 = col3;


        }
        
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }

    }
}

