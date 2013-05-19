using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace templateDeploy
{
    public partial class Config : Form
    {
        public int first = 0;

        public Config()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Read and load the application settings into the respective textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Config_Load(object sender, EventArgs e)
        {
            textBox1.Text = ConfigurationManager.AppSettings.Get("srcLoc");
            textBox2.Text = ConfigurationManager.AppSettings.Get("tarLoc");
            textBox3.Text = ConfigurationManager.AppSettings.Get("recipients");
            textBox4.Text = ConfigurationManager.AppSettings.Get("subject");

            string body = ConfigurationManager.AppSettings.Get("body");
            body = body + "\r\n\r\n[Files listed here]";

            textBox5.Text = body;

        }

        /// <summary>
        /// Close the Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Apply and save the settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["srcLoc"].Value = textBox1.Text.ToString();
            configuration.AppSettings.Settings["tarLoc"].Value = textBox2.Text.ToString();
            configuration.AppSettings.Settings["recipients"].Value = textBox3.Text.ToString();
            configuration.AppSettings.Settings["subject"].Value = textBox4.Text.ToString();

            string body = textBox5.Text;
            string target = "[target]";
            string list = "[Files listed here]";
            string issue = "The following text is missing from the body or misspelled and must be restored: \n\n";

            //If the placeholder text is missing from the body, don't allow it to be saved.
            if (body.Contains(list) == true && body.Contains(target) == true)
            {
                configuration.AppSettings.Settings["body"].Value = textBox5.Text.Replace(list,"");
            }
            else if (body.Contains(list) == false && body.Contains(target) == false)
            {
                MessageBox.Show(issue + target + "\n" + list);
                toolStripStatusLabel1.Text = "An error occurred.";
                return;
            }
            else if (body.Contains(list) == false && body.Contains(target) == true)
            {
                MessageBox.Show(issue + list);
                toolStripStatusLabel1.Text = "An error occurred.";
                return;
            }
            else if (body.Contains(list) == true && body.Contains(target) == false)
            {
                MessageBox.Show(issue + target);
                toolStripStatusLabel1.Text = "An error occurred.";
                return;
            }

            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");

            toolStripStatusLabel1.Text = "Settings saved.";
        }

        /// <summary>
        /// Display a warning before allowing users to edit the body of the email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (first == 0)
            {
                MessageBox.Show("Do not edit the text between the braces, \"[]\".  They are placeholders that the program will look for when composing the body of the email.", "KI Deploy - Warning");
                ++first;
            }
        }
    }
}
