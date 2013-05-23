using System;
using System.Collections.Generic;
using System.Configuration;
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

            string netFiles = ConfigurationManager.AppSettings.Get("netFiles"); 
            string locFiles = ConfigurationManager.AppSettings.Get("locFies");

            foreach (var NetworkFile in Directory.GetFiles(netFiles))
            {
                fileName = Path.GetFileName(NetworkFile).ToString();
                dt = File.GetLastAccessTime(netFiles);
                listItems[0] = fileName;
                listItems[1] = dt.ToString();

                Form1.mF.listView1.Items.Add(lvi);   
            }

            foreach (var LocalFile in Directory.GetFiles(locFiles))
            {
                for (int i = 0; i < Form1.mF.listView1.Items.Count; i++)
                {
                    if (Form1.mF.listView1.Items[i].SubItems[0].Text == LocalFile)
                    {
                        dt = File.GetLastAccessTime(LocalFile);
                        Form1.mF.listView1.Items[i].SubItems[2].Text == dt.ToString();
                        break;
                    }
                }
            }
        }
    }
}
