using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace attach2email
{
    public partial class dragFiles : Form
    {
        public object lb_item = null;
        public Content content = new Content();

        public dragFiles(Content passContent)
        {
            InitializeComponent();
            content = passContent;
        }

        /// <summary>
        /// Close this dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// DragEnter has to exist to allow the action to continue
        /// In this case, it checks to see if the data being dragged is a file or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            //make sure user is dropping files (not text or anything else)
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                //allow them to continue
                //without this, the cursor becomes the "NO" symbol (circle with slash)
                e.Effect = DragDropEffects.All;
            }
            else
            {
                if (lb_item != null)
                {
                    listBox1.Items.Add(lb_item);
                    lb_item = null;
                }
            }
        }

        /// <summary>
        /// Populates the listBox with the files dropped in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            //transfer filenames to a string array
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                //if the user drags a directory in, populate listBox with all files in that directory
                //otherwise, populate with the file(s) dragged in
                if (Directory.Exists(file))
                {
                    string[] fileNames = Directory.GetFiles(file);
                    foreach (string fileName in fileNames)
                    {
                        GetFiles(fileName);
                    }
                }
                else
                {
                    GetFiles(file);
                }
            }
        }

        /// <summary>
        /// Add each item
        /// </summary>
        /// <param name="file"></param>
        private void GetFiles(string file)
        {
            listBox1.Items.Add(file);
        }

        private void listBox1_DragLeave(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            lb_item = lb.SelectedItem;
            lb.Items.Remove(lb.SelectedItem);
        }

        /// <summary>
        /// To permit dragging out, we have to use the MouseDown event
        /// In this case, activate DragDrop for the item clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lb_item = null;

            if (listBox1.Items.Count == 0)
            { 
                return; 
            }

            int index = listBox1.IndexFromPoint(e.X, e.Y);
            try
            {
                string s = listBox1.Items[index].ToString();
                DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.All);
            }
            catch
            { 
                //...
            }
            
        }

        /// <summary>
        /// Attach the files to an email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //If there are no items, throw an alert
            if (listBox1.Items == null)
            {
                MessageBox.Show("No items to attach.", "Email Template Generator");
                return;
            }

            //variables for file names and a counter
            string[] files = new string[listBox1.Items.Count];
            int i;

            //Add all items to an array to pass to attachEm
            for (i = 0; i < listBox1.Items.Count; i++)
            {
                files[i] = listBox1.Items[i].ToString();
            }

            //pass form, array, and request type
            Attach.attachEm(content, files, "single");

            //alert user that template is ready
            MessageBox.Show("Template created successfully.");

            //close the dialog
            this.Close();
        }
    }
}
