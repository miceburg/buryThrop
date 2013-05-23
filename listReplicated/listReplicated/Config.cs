using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace listReplicated
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigurationManager.AppSettings.Get("netCSV");
            textBox2.Text = ConfigurationManager.AppSettings.Get("locCSV");
            textBox3.Text = ConfigurationManager.AppSettings.Get("netFiles");
            textBox4.Text = ConfigurationManager.AppSettings.Get("locFiles");
        }
    }
}
