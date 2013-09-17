using Excel = Microsoft.Office.Interop.Excel;
using Office.BP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LastAccess
{
    public partial class LastAccessForm : Form
    {
        public static LastAccessForm mF = null;
        private static ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        public LastAccessForm()
        {
            InitializeComponent();
            mF = this;
            this.listView1.ListViewItemSorter = lvwColumnSorter;
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config config = new Config();
            config.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentProcess CP = new CurrentProcess();
            Excel.Workbook wb = (Excel.Workbook)CP.useCurrentProcess("EXCEL");
            Excel.Worksheet ws = wb.Worksheets.Add();

            int i = 1;
            int i2 = 1;

            foreach (ListViewItem lvi in listView1.Items)
            {
                foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                {
                    ws.Cells[i2, i] = lvs.Text;
                    i++;
                }

                i2++;
                i = 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            LastAccess.ChkAccessDates();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (LastAccess.copyDest == null || LastAccess.copyDest == "")
            {
                MessageBox.Show("No copy destination designated. Check configuration settings.", "LastAccess: No Copy Destination");
                return;
            }

            foreach (ListViewItem item in listView1.Items)
            {
                if (!File.Exists(Path.Combine(LastAccess.copyDest.ToString(), item.SubItems[0].Text)))
                {
                    File.Copy(item.SubItems[3].Text, Path.Combine(LastAccess.copyDest.ToString(), item.SubItems[0].Text), false);
                }
                else
                {
                    File.Copy(item.SubItems[3].Text, Path.Combine(LastAccess.copyDest.ToString(), 
                                                                                                    item.SubItems[0].Text 
                                                                                                    + " - " 
                                                                                                    + Regex.Replace(item.SubItems[3].Text, @"\\", "-").Substring(4)), 
                                                                                                    false);
                }
            }

            MessageBox.Show("Copy completed", "LastAccess: Copy");
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView myListView = (ListView)sender;

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            myListView.Sort();
        }
    }
}
