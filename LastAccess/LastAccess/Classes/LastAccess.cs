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
        //private static string netFiles = ConfigurationManager.AppSettings.Get("netFiles");
        //private static string locFiles = ConfigurationManager.AppSettings.Get("locFies");
        //private static string strWhichFiles = ConfigurationManager.AppSettings.Get("whichFiles");
        //private static string extensions = ConfigurationManager.AppSettings.Get("extensions");
        //internal static string copyDest = ConfigurationManager.AppSettings.Get("copyDest");

        private static string netFiles;
        private static string locFiles;
        private static string strWhichFiles;
        private static string extensions;
        internal static string copyDest;
        internal static List<myObject> network = new List<myObject>();
        internal static List<myObject> local = new List<myObject>();

        private static string[] listItems = new string[4];

        public static void ChkAccessDates()
        {
            netFiles = ConfigurationManager.AppSettings.Get("netFiles");
            locFiles = ConfigurationManager.AppSettings.Get("locFiles");
            strWhichFiles = ConfigurationManager.AppSettings.Get("whichFiles");
            extensions = ConfigurationManager.AppSettings.Get("extensions");
            copyDest = ConfigurationManager.AppSettings.Get("copyDest");

            if (extensions.Length < 5)
            {
                extensions = "*.*";
            }

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
            myObject fileInfo = new myObject();

            foreach (var NetworkFile in Directory.GetFiles(netFiles, extensions, SearchOption.AllDirectories))
            {
                fileInfo.fileName = listItems[0] = Path.GetFileName(NetworkFile).ToString();
                fileInfo.lastAccess = listItems[1] = File.GetLastAccessTime(NetworkFile).ToString();
                listItems[2] = "Network";
                fileInfo.fullPath = listItems[3] = Path.GetFullPath(NetworkFile);

                ListViewItem lvi = new ListViewItem(listItems);
                LastAccessForm.mF.listView1.Items.Add(lvi);

                network.Add(fileInfo);
            }

        }

        private static void localFiles()
        {
            myObject fileInfo = new myObject();

            foreach (var LocalFile in Directory.GetFiles(locFiles, extensions, SearchOption.AllDirectories))
            {
                fileInfo.fileName = listItems[0] = Path.GetFileName(LocalFile).ToString();
                fileInfo.lastAccess = listItems[1] = listItems[1] = File.GetLastAccessTime(LocalFile).ToString();
                listItems[2] = "Local";
                fileInfo.fullPath = listItems[3] = Path.GetFullPath(LocalFile);

                ListViewItem lvi = new ListViewItem(listItems);
                LastAccessForm.mF.listView1.Items.Add(lvi);

                local.Add(fileInfo);
            }
        }
    }
}
