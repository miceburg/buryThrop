using Outlook = Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace attach2email
{
    public static class Attach
    {
        public static void attachEm(Content content, string[] files, string choice)
        {
            //Check if Outlook is already open. If so, use the open instance.
            Process[] outlkPcs = Process.GetProcessesByName("OUTLOOK");
            Outlook.Application outlook;

            if (outlkPcs.Length > 0)
            {
                outlook = (Outlook.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Outlook.Application");
            }
            else
            {
                outlook = new Outlook.Application();
            }

            //place to create OFT files
            string myOFT = @"C:\users\" + Environment.UserName + @"\Desktop\OFT";

            //If directory doesn't exist, create it
            if (!Directory.Exists(myOFT))
            {
                Directory.CreateDirectory(myOFT);
            }

            //we need random numbers to distinguish the templates
            Random rnd = new Random();
            int random;

            //variables for filename, body of email, name of the test, and an Outlook mail item
            string myFile;
            string body;
            string testName;
            Outlook.MailItem mail;

            
            switch (choice)
            {
                //If it's a batch-1 request, attach each file to a different template
                case "batch-1":

                    //Attach each file to an email 
                    foreach (var file in files)
                    {
                        myFile = Path.GetFileName(file) + Environment.NewLine;
                        testName = content.textBox1.Text;
                        random = rnd.Next(1, 1000);

                        //Create a new message
                        mail = outlook.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
                        mail.To = content.textBox3.Text;
                        mail.Subject = content.textBox4.Text + (char)95 + myFile;
                        body = content.textBox5.Text;
                        body = body + Environment.NewLine + myFile;
                        mail.Attachments.Add(file);
                        mail.Body = body;

                        //Save the message as a template
                        mail.SaveAs(myOFT + (char)92 + testName + (char)95 + random.ToString() + ".oft", Outlook.OlSaveAsType.olTemplate);
                    }
                    break;

                //If it's a single request, attach all files to a single template
                case "single":

                    testName = content.textBox1.Text;
                    random = rnd.Next(1, 1000);

                    //Create a new message
                    mail = outlook.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
                    mail.To = content.textBox3.Text;
                    mail.Subject = content.textBox4.Text;
                    body = content.textBox5.Text;

                    //Attach all files file to a template
                    foreach (var file in files)
                    {
                        myFile = Path.GetFileName(file) + Environment.NewLine;
                        body = body + Environment.NewLine + myFile;
                        mail.Attachments.Add(file);
                        mail.Body = body;
                    }

                    //Save the template
                    mail.SaveAs(myOFT + (char)92 + testName + (char)95 + random.ToString() + ".oft", Outlook.OlSaveAsType.olTemplate);
                    break;

                default:
                    break;
            }
        }
    }
}
