using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace listReplicated
{
    public partial class Form1 : Form
    {
        public static Form1 mF = null;
        public KILV LV = new KILV();
        public ItemComparer SC = new ItemComparer();

        public Form1()
        {
            InitializeComponent();
            mF = this;
            LV.PopLV();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int test = e.Column;

            ItemComparer sorter = listView1.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer();
                listView1.ListViewItemSorter = sorter;
            }
            else
            {
                // Set the column number that is to be sorted
                sorter.Column = e.Column;
            }
            listView1.Sort();
        }


        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int test = e.Column;

            ItemComparer sorter = listView2.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer();
                listView2.ListViewItemSorter = sorter;
            }
            else
            {
                // Set the column number that is to be sorted
                sorter.Column = e.Column;
            }
            listView2.Sort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            LV.PopLV();
        }
    }
}
