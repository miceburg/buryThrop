using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LastAccess
{
    public class LastAccess
    {
            public static void ChkAccessDates()
            {
                string fileName;
                string[] listItems = new string[3];
                DateTime dt;

                string sourcePathA = @"[location of source files]"; 
                string sourcePathB = @"[location of local files]";

                foreach (var NetworkFile in Directory.GetFiles(sourcePathA))
                {
                    fileName = Path.GetFileName(NetworkFile).ToString();
                    dt = File.GetLastAccessTime(sourcePathA);
                    listItems[0] = fileName;
                    listItems[1] = dt.ToString();

                    dt = File.GetLastAccessTime(sourcePathB);

                    listItems[2] = dt.ToString();

                    ListViewItem lvi = new ListViewItem(listItems);

                    Form1.mF.listView1.Items.Add(lvi);   
                }

                int n = 0;

                foreach (var LocalFile in Directory.GetFiles(sourcePathB))
                {
                    dt = File.GetLastAccessTime(sourcePathB);
                    Form1.mF.listView1.Items[n].SubItems.Add(dt.ToString());
                    n++;
                }
        }


    }
}
