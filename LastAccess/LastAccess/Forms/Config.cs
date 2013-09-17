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
            tbNetFiles.Text = ConfigurationManager.AppSettings.Get("netFiles");
            tbLocFiles.Text = ConfigurationManager.AppSettings.Get("locFiles");
            tbExt.Text = ConfigurationManager.AppSettings.Get("extensions");
            tbCopyDest.Text = ConfigurationManager.AppSettings.Get("copyDest");

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
            if (tbNetFiles.Text.Length < 1 && tbLocFiles.Text.Length < 1)
            {
                MessageBox.Show("No file sources chosen.", "LastAccess: No Files");
                return;
            }
            else if (tbNetFiles.Text.Length < 1 && tbLocFiles.Text.Length >=1)
            {
                rbLocal.Checked = true;
            }
            else if (tbNetFiles.Text.Length >= 1 && tbLocFiles.Text.Length < 1)
            {
                rbNetwork.Checked = true;
            }

            string strWhichFiles = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Name.ToString();

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["netFiles"].Value = tbNetFiles.Text;
            configuration.AppSettings.Settings["locFiles"].Value = tbLocFiles.Text;
            configuration.AppSettings.Settings["extensions"].Value = tbExt.Text;
            configuration.AppSettings.Settings["copyDest"].Value = tbCopyDest.Text;
            configuration.AppSettings.Settings["whichFiles"].Value = strWhichFiles;
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Configuration settings saved.", "LastAccess: Config Saved");
        }
    }
}
