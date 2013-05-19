using System;
using System.Collections.Generic;
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
            string csvA = @"[csv of source files]";
            string csvB = @"[csv of local files]";

            CheckExists(csvA);
            CheckExists(csvB);

            using (FileStream fs = File.Create(csvA))
            { 
            
            }
            using (FileStream fs = File.Create(csvB))
            { 
            
            }

            string sourcePathA = @"[location of source]";
            string sourcePathB = @"[location of replicated files]";
            string fileName;
            string fileExtension;
            string[] listItems = new string[3];
            DateTime dt;

            foreach (var NetworkFile in Directory.GetFiles(sourcePathA))
            {
                fileName = Path.GetFileName(NetworkFile).ToString();
                fileExtension = Path.GetExtension(NetworkFile).ToString();
                dt = File.GetLastAccessTime(sourcePathA + @"\" + fileName);
                listItems[0] = fileName;
                listItems[1] = fileExtension;
                listItems[2] = dt.ToString("yyyy/MM/dd HH:mm:ss");

                ListViewItem lvi = new ListViewItem(listItems);

                Form1.mF.listView1.Items.Add(lvi);

                using (StreamWriter sw = File.AppendText(csvA))
                {
                    sw.WriteLine(listItems[0] + "," + listItems[2]);
                }
                
            }

            listItems = new string[2];

            foreach (var LocalFile in Directory.GetFiles(sourcePathB))
            {
                fileName = Path.GetFileName(LocalFile).ToString();
                dt = File.GetLastAccessTime(sourcePathB + @"\" + fileName);

                listItems[0] = fileName;
                listItems[1] = dt.ToString("yyyy/MM/dd HH:mm:ss");

                ListViewItem lvi = new ListViewItem(listItems);

                Form1.mF.listView2.Items.Add(lvi);

                using (StreamWriter sw = File.AppendText(csvB))
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
