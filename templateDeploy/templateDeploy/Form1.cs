using Outlook = Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace templateDeploy
{
    public partial class Form1 : Form
    {
        // Get the source directory from the application settings
        public string src = ConfigurationManager.AppSettings.Get("srcLoc");

        /// <summary>
        /// Prepare the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            listFiles();
        }

        /// <summary>
        /// List all the files in the deployment folder
        /// </summary>
        public void listFiles()
        {
            foreach (var file in Directory.GetFiles(src))
            {
                string[] myFile = new string[2];
                myFile[0] = Path.GetFileName(file).ToString();
                myFile[1] = File.GetLastWriteTime(file).ToString();
                ListViewItem lvi = new ListViewItem(myFile);
                listView1.Items.Add(lvi);
            }
        }

        /// <summary>
        /// Prepare the files for deployment
        /// </summary>
        public void copyMoveSend()
        {   
            //Target directory for file copy
            string targ = ConfigurationManager.AppSettings.Get("tarLoc");
            
            //Check if Outlook is already open. If so, use the open instance.
            Process[] outlkPcs = Process.GetProcessesByName("OUTLOOK");
            Outlook.Application outlook;

            if (outlkPcs.Length > 0)
            {
                outlook = (Outlook.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Outlook.Application");
            }
            else
            {
                outlook = new Outlook.Application();
            }

            //Create a new message
            Outlook.MailItem mail = outlook.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
            mail.Subject = ConfigurationManager.AppSettings.Get("subject") + DateTime.Today.ToShortDateString();
            mail.To = ConfigurationManager.AppSettings.Get("recipients");
            string body = ConfigurationManager.AppSettings.Get("body");
            body = body.Replace("[target]", targ) + "\n\n";

            //Copy each file to target directory, 
            foreach (var file in Directory.GetFiles(src))
            {
                File.Copy(file, Path.Combine(targ, Path.GetFileName(file)), true);
                body = body + Path.GetFileName(file) + "\n";
                mail.Attachments.Add(file);
                File.Move(file, src + "\\Deployed\\" + Path.GetFileName(file));
            }

            mail.Body = body;
            mail.Save();

            listView1.Clear();
            MessageBox.Show("The files have been copied to the appropriate directories and a draft email has been created for network distribution.  Review and send the email.", "KI Deploy: Files Ready for Distribution");
            mail.Display();
        }

        /// <summary>
        /// Fire copyMoveSend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            copyMoveSend();
        }

        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Display configuration settings form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config myConfig = new Config();
            myConfig.Show();
        }
    }
}
