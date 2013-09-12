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
        private  static string netFiles = ConfigurationManager.AppSettings.Get("netFiles");
        private static string locFiles = ConfigurationManager.AppSettings.Get("locFies");
        private static string strWhichFiles = ConfigurationManager.AppSettings.Get("whichFiles");
        private static string extensions = ConfigurationManager.AppSettings.Get("extensions");
        private static string[] listItems = new string[3];

        public static void ChkAccessDates()
        {
            try
            {
                switch (strWhichFiles)
                {
                    case "rbNetwork":
                        networkFiles();
                        break;
                    case "rbLocal":
                        localFiles();
                        break;
                    case "rbBoth":
                        networkFiles();
                        localFiles();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "LastAccess: Error");
            }
        }

        private static void networkFiles()
        {
            foreach (var NetworkFile in Directory.GetFiles(netFiles, extensions, SearchOption.AllDirectories))
            {
                listItems[0] = Path.GetFileName(NetworkFile).ToString();
                listItems[1] = File.GetLastAccessTime(NetworkFile).ToString();
                listItems[2] = "Network";
                listItems[3] = Path.GetFullPath(NetworkFile);

                ListViewItem lvi = new ListViewItem(listItems);

                LastAccessForm.mF.listView1.Items.Add(lvi);
            }

        }

        private static void localFiles()
        {
            foreach (var LocalFile in Directory.GetFiles(locFiles, extensions, SearchOption.AllDirectories))
            {
                listItems[0] = Path.GetFileName(LocalFile).ToString();
                listItems[1] = File.GetLastAccessTime(LocalFile).ToString();
                listItems[2] = "Local";
                listItems[3] = Path.GetFullPath(LocalFile);

                ListViewItem lvi = new ListViewItem(listItems);

                LastAccessForm.mF.listView1.Items.Add(lvi);
            }
        }
    }
}
