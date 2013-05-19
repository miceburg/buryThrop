using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace adsPutterBacker
{
    /// <summary>
    /// Class contains code for Form1
    /// </summary>
    public partial class Form1 : Form
    {
        //Instantiate some public objects
        public static Form1 myForm = new Form1();
        adsListView LV = new adsListView();

        //set the source path
        public string sourcePath = @"C:\obk";
        //set the target path
        public string targetPath = @"";
        //instantiate a variable we can use to hold the file name
        public string fileName = "";

        /// <summary>
        /// Basic form initialization
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            myForm = this;
            //populate our listview with the file names
            LV.PopListBox(sourcePath, fileName);
        }

        /// <summary>
        /// Open the selected items from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Create and instantiate the string[] to hold the filenames
            //The size of the array is the count of the selected items
            string[] fileArray = new string[listView1.SelectedItems.Count];

            //Populate the array
            for (int i = 0; i < fileArray.Length; i++)
            {
                fileArray[i] = listView1.SelectedItems[i].SubItems[1].Text.ToString();
            }

            //Clear selected items from the list.  Those files are getting moved.
            listView1.SelectedItems.Clear();
            //Run PickFile
            LV.PickFile(sourcePath, targetPath, fileArray);
        }

        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
