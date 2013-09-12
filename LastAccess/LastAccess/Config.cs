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
            textBox3.Text = ConfigurationManager.AppSettings.Get("extensions");

            string strWhichFiles = ConfigurationManager.AppSettings.Get("whichFiles");

            switch (strWhichFiles)
            {
                case "rbNetwork":
                    rbNetwork.Checked = true;
                    break;
                case "rbLocal":
                    rbLocal.Checked = true;
                    break;
                case "rbBoth":
                    rbBoth.Checked = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strWhichFiles = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name.ToString();

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["netFiles"].Value = textBox1.Text;
            configuration.AppSettings.Settings["locFiles"].Value = textBox2.Text;
            configuration.AppSettings.Settings["whichFiles"].Value = strWhichFiles;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Configuration settings saved.", "LastAccess: Config Saved");
        }
    }
}
