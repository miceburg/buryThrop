using Outlook = Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace attach2email
{
    public partial class Content : Form
    {
        public Content()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Batch-1: To be used when user wants to add multiple single files to separate templates.
        /// For example, 22 separate PDF files, each needing to be attached to a separate email.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button1_Click(object sender, EventArgs e)
        {
            //Show a folder browswer dialog.
            //If user clicks OK, get all the file names in that directory and pass them in an array to attachEm
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                Attach.attachEm(this, files, "batch-1");
            }

            MessageBox.Show("Templates created successfully.", "Email Template Generator");
        }

        /// <summary>
        /// Single: To be used when user wants to attach multiple files to a single template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //Show the dragFiles form
            dragFiles dF = new dragFiles(this);
            dF.ShowDialog();
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
    }
}
