using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.PowerPoint;

namespace adsPutterBacker
{
    /// <summary>
    /// Class containing functions for manipulating data in the ListView
    /// </summary>
    class adsListView
    {
        //declare and instantiate a ConBin object
        ConBin cb = new ConBin();
        //Create a string for storing the target name and filepath
        public string destinationFilePath = "";

        /// <summary>
        /// Function to populate the listView
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="fileName"></param>
        public void PopListBox(string sourcePath, string fileName)
        {
            //if the source directory doesn't exist, tell user there are no backup files
            if (!System.IO.Directory.Exists(sourcePath))
            {
                MessageBox.Show("There are no backup files to open.");
            }
            //otherwise, populate the listView
            else
            {
                //For each file in the backup location
                foreach (var SourceFile in System.IO.Directory.GetFiles(sourcePath))
                {
                    //Get the last modified date
                        //Since the files often have unintelligible names,
                        //this will help users differentiate
                    DateTime dt = System.IO.File.GetLastWriteTime(SourceFile);
                    
                    //String to store the path and filename
                    string fullPath = System.IO.Path.Combine(sourcePath, SourceFile);
                    //String to store the last modified date
                    string lm = dt.ToString();
                    //Array to use in collecting all info in a single listViewItem
                    string[] listItems = new string[4];

                    //Get the filename
                    fileName = System.IO.Path.GetFileName(SourceFile).ToString();

                    //Determine whether the file was made by Word, Excel, or PowerPoint
                    //Behave accordingly
                    if (fileName.Contains(".asd"))
                    {
                        listItems[0] = "Word";
                        AddLVI(ref listItems, fileName, lm, fullPath);
                    }
                    else if (fileName.Contains(".xar"))
                    {
                        listItems[0] = "Excel";
                        //Haven't figured out how to parse ASCII text out of the Excel containers
                        //Can't provide preview
                        listItems[3] = "No preview available.";
                        AddLVI(ref listItems, fileName, lm, fullPath);
                    }
                    else if (fileName.Contains(".tmp") && fileName.Contains("ppt"))
                    {
                        listItems[0] = "PowerPoint";
                        //Haven't figured out how to parse ASCII text out of the PowerPoint containers
                        //Can't provide preview
                        listItems[3] = "No preview available.";
                        AddLVI(ref listItems, fileName, lm, fullPath);
                    }
                }
            }
        }

        /// <summary>
        /// Function completes array population and adds items to the listView
        /// </summary>
        /// <param name="listItems"></param>
        /// <param name="fileName"></param>
        /// <param name="lm"></param>
        /// <param name="fullPath"></param>
        public void AddLVI(ref string[] listItems, string fileName, string lm,string fullPath)
        {
            //1 - file name
            //2 - last modified time
            listItems[1] = fileName;
            listItems[2] = lm;
            //3 - for Word files, needs to be populated
            if (listItems[3] != "No preview available.")
            {
                //Call the function to convert binary to text
                //provide the full filepath
                listItems[3] = cb.BinHexText(fullPath);
            }

            //add the listViewItem
            ListViewItem lvi = new ListViewItem(listItems);
            Form1.myForm.listView1.Items.Add(lvi);
        }

        /// <summary>
        /// Function to run when the user wants to open a file in the list
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="fileArray"></param>
        public void PickFile(string sourcePath, string targetPath, string[] fileArray)
        {
            //for each file in fileArray
            for (int i = 0; i < fileArray.Length; i++)
            {
                //Combine the source path with the current file name in the array
                string sourceFilePath = System.IO.Path.Combine(sourcePath, fileArray[i]);
                //All the temp paths are in under the same parent directory
                string targetPathBase = @"C:\users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\";

                //Determine if the file is from Word, Excel, PowerPoint
                //Add the appropriate directory to the base
                //Run SetSend
                //Run the method for opening that file type
                if (fileArray[i].ToString().Contains(".asd"))
                {
                    targetPath = targetPathBase + "Word";
                    SetSend(ref destinationFilePath, sourceFilePath, targetPath, fileArray, i);
                    OpenASD(destinationFilePath);
                }
                else if (fileArray[i].ToString().Contains(".xar"))
                {
                    targetPath = targetPathBase + "Excel";
                    SetSend(ref destinationFilePath, sourceFilePath, targetPath, fileArray, i);
                    OpenXAR(destinationFilePath);
                }
                else if (fileArray[i].ToString().Contains(".tmp") && fileArray[i].ToString().Contains("ppt"))
                {
                    targetPath = targetPathBase + "Powerpoint";
                    SetSend(ref destinationFilePath, sourceFilePath, targetPath, fileArray, i);
                    OpenPNT(destinationFilePath);
                }
            }
        }

        /// <summary>
        /// Function that sends the file back to the temp directory.
        /// Office temp files will only open (I think) from their designated temp directories.
        /// </summary>
        /// <param name="destinationFilePath"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="fileArray"></param>
        /// <param name="i"></param>
        public void SetSend(ref string destinationFilePath, string sourceFilePath, string targetPath, string[] fileArray, int i)
        {
            //Combine path and file name
            destinationFilePath = System.IO.Path.Combine(targetPath, fileArray[i]);
            //Copy the file from the backup location to the default temp directory
            BackupCopy(sourceFilePath, destinationFilePath);
        }

        /// <summary>
        /// Function to open temp Word file
        /// </summary>
        /// <param name="destinationFilePath"></param>
        public void OpenASD(string destinationFilePath)
        {
            //Check for running instances of the desired application
            Process[] wdPcs = Process.GetProcessesByName("WINWORD");
            //If there are any instances currently running, use those
            if (wdPcs.Length > 0)
            {
                Microsoft.Office.Interop.Word.Application word;
                //Get the active instance of the application
                word = (Microsoft.Office.Interop.Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
                word.Activate();
                Document doc = word.Documents.Open(destinationFilePath);
            }
            //otherwise, open a new instance
            else
            {
                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                word.Visible = true;
                Document doc = word.Documents.Open(destinationFilePath);
            }
        }

        /// <summary>
        /// Function to open temp Excel file
        /// </summary>
        /// <param name="destinationFilePath"></param>
        public void OpenXAR(string destinationFilePath)
        {
            //Check for running instances of the desired application
            Process[] xlPcs = Process.GetProcessesByName("EXCEL");
            //If there are any instances currently running, use those
            if (xlPcs.Length > 0)
            {
                Microsoft.Office.Interop.Excel.Application excel;
                //Get the active instance of the application
                excel = (Microsoft.Office.Interop.Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                excel.Application.Visible = true;
                Workbook wkbk = excel.Workbooks.Open(destinationFilePath);
            }
            //otherwise, open a new instance
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Workbook wkbk = excel.Workbooks.Open(destinationFilePath);
            }
        }

        /// <summary>
        /// Function to open temp PowerPoint files
        /// </summary>
        /// <param name="destinationFilePath"></param>
        public void OpenPNT(string destinationFilePath)
        {
            //Check for running instances of the desired application
            Process[] pnPcs = Process.GetProcessesByName("POWERPNT");
            //If there are any instances currently running, use those
            if (pnPcs.Length > 0)
            {
                Microsoft.Office.Interop.PowerPoint.Application powerpoint;
                //Get the active instance of the application
                powerpoint = (Microsoft.Office.Interop.PowerPoint.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Powerpoint.Application");
                powerpoint.Activate();
                Presentation presi = powerpoint.Presentations.Open(destinationFilePath);
            }
            //otherwise, open a new instance
            else
            {
                Microsoft.Office.Interop.PowerPoint.Application powerpoint = new Microsoft.Office.Interop.PowerPoint.Application();
                powerpoint.Visible = (Microsoft.Office.Core.MsoTriState)Microsoft.Office.Core.MsoTriState.msoTrue;
                Presentation presi = powerpoint.Presentations.Open(destinationFilePath);
            }
        }

        /// <summary>
        /// Copy the file from the source to the target
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        public void BackupCopy(string sourceFilePath, string destinationFilePath)
        {
            System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
        }
    }
}
