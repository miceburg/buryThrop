using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LastAccess
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
            textBox1.Text = ConfigurationManager.AppSettings.Get("netFiles");
            textBox2.Text = ConfigurationManager.AppSettings.Get("locFiles");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration configuration = new Configuration();
            configuration.AppSettings.Settings["netFiles"].Value = textBox1.Text;
            configuration.AppSettings.Settings["locFiles"].Value = textBox2.Text;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
