using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BinaryCopy
{
    public partial class Form1 : Form
    {
        //Directory to hold new files
        public string dir = @"C:\users\" + Environment.UserName + @"\Desktop\newBytes";

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On form load, user selects a directory using the folder browser dialog, and we pass file names to copyMyBytes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                copyMyBytes(files);
            }

            this.Close();
        }

        /// <summary>
        /// Copy all bytes from one file to another new file.  This doesn't bring the ADS with it.
        /// </summary>
        /// <param name="myPath"></param>
        public void copyMyBytes(string[] myPath)
        {
            //Creates the new directory if it doesn't exist
            Directory.CreateDirectory(dir);

            //Fore each file, read all the bytes and write them to a new file
            int i;
            for (i = 0; i < myPath.Length; i++)
            {
                //Read all bytes
                byte[] fileBytes = File.ReadAllBytes(myPath[i]);

                //Get the file name of the current file to create a replica
                string newFile = dir + "\\" + Path.GetFileName(myPath[i]);

                //create a binary writer for the new file
                var bw = new BinaryWriter(File.Open(newFile, FileMode.OpenOrCreate));
                //Write, Flush, Close
                bw.Write(fileBytes);
                bw.Flush();
                bw.Close();

                //set fileBytes to null to be sure nothing carries over
                fileBytes = null;
            }   
        }
    }
}