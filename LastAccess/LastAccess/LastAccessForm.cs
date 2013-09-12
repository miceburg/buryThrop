using Excel = Microsoft.Office.Interop.Excel;
using Office.BP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LastAccess
{
    public partial class LastAccessForm : Form
    {
        public static LastAccessForm mF = null;

        public LastAccessForm()
        {
            InitializeComponent();
            mF = this;
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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LastAccess.ChkAccessDates();
        }
    }
}
