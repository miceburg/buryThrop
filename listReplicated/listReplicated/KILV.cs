using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace listReplicated
{
    public class KILV
    {
        public void PopLV()
        {
            string netCSV = ConfigurationManager.AppSettings.Get("netCSV");
            string locCSV = ConfigurationManager.AppSettings.Get("locCSV");

            CheckExists(netCSV);
            CheckExists(locCSV);

            using (FileStream fs = File.Create(netCSV))
            { 
            
            }
            using (FileStream fs = File.Create(locCSV))
            { 
            
            }

            string netFiles = ConfigurationManager.AppSettings.Get("netFiles");
            string locFiles = ConfigurationManager.AppSettings.Get("locFiles");
            string fileName;
            string fileExtension;
            string[] listItems = new string[3];
            DateTime dt;

            foreach (var NetworkFile in Directory.GetFiles(netFiles))
            {
                fileName = Path.GetFileName(NetworkFile).ToString();
                fileExtension = Path.GetExtension(NetworkFile).ToString();
                dt = File.GetLastAccessTime(netFiles + @"\" + fileName);
                listItems[0] = fileName;
                listItems[1] = fileExtension;
                listItems[2] = dt.ToString("yyyy/MM/dd HH:mm:ss");

                ListViewItem lvi = new ListViewItem(listItems);

                Form1.mF.listView1.Items.Add(lvi);

                using (StreamWriter sw = File.AppendText(netCSV))
                {
                    sw.WriteLine(listItems[0] + "," + listItems[2]);
                }
                
            }

            listItems = new string[2];

            foreach (var LocalFile in Directory.GetFiles(locFiles))
            {
                fileName = Path.GetFileName(LocalFile).ToString();
                dt = File.GetLastAccessTime(locFiles + @"\" + fileName);

                listItems[0] = fileName;
                listItems[1] = dt.ToString("yyyy/MM/dd HH:mm:ss");

                ListViewItem lvi = new ListViewItem(listItems);

                Form1.mF.listView2.Items.Add(lvi);

                using (StreamWriter sw = File.AppendText(locCSV))
                {
                    sw.WriteLine(listItems[0] + "," + listItems[1]);
                }

            }
        }

        public void CheckExists(string path)
        { 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
        }
    }
}
