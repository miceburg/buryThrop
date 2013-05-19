using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace asdGrabber
{
    class Program
    {
        static void Main(string[] args)
        {
            //instantiate variables to designate the source and target paths for copying
            string sourcePath = @"";
            string targetPath = @"C:\obk";

            //If the directory C:\obk doesn't exist, create it
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            //Run checkModify
            checkModify(targetPath);

            //set the source path to Word's temp folder and run BackupCopy
            sourcePath = @"C:\users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Word";
            BackupCopy(sourcePath, targetPath);

            //set the source path to Excel's temp folder and run BackupCopy
            sourcePath =@"C:\users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Excel";
            BackupCopy(sourcePath, targetPath);

            //set the source path to PowerPoint's temp folder and run BackupCopy
            sourcePath =@"C:\users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Powerpoint";
            BackupCopy(sourcePath, targetPath);
        }

        //Copies every file in a certain directory.
        //Does not copy files in subdirectories
        static void BackupCopy(string sourcePath, string targetPath)
        {
            //For each file in the source directory
            foreach (var sourceFilePath in Directory.GetFiles(sourcePath))
            {
                //get the file name
                string fileName = Path.GetFileName(sourceFilePath);
                //designate the target path and target file name (same as current file name)
                string destinationFilePath = Path.Combine(targetPath, fileName);
                
                //Copy the file from the source to the target
                if (!File.Exists(destinationFilePath))
                {
                    try
                    {
                        File.Copy(sourceFilePath, destinationFilePath, true);
                    }
                    catch (System.IO.IOException e)
                    {
                        //do nothing
                    }
                    finally
                    {
                        //do nothing
                    }
                }
                else
                {
                    if (fileName.Contains("."))
                    {
                        string[] splitFile = fileName.Split('.');
                        splitFile[0] = splitFile[0] + DateTime.Today.ToString("_yyMMdd") + "." + splitFile[1];
                        fileName = splitFile[0];
                        destinationFilePath = Path.Combine(targetPath, fileName);
                        File.Copy(sourceFilePath, destinationFilePath, true);
                    }
                    else
                    {
                        fileName = fileName + DateTime.Today.ToString("_yyMMdd");
                        destinationFilePath = Path.Combine(targetPath, fileName);
                        File.Copy(sourceFilePath, destinationFilePath, true);
                    }
                }
            }
        }

        //Checks the "Date modified" property of all the files in C:\obk
        //to see if they are 7 days old or older.
        //Operating on the assumption here that if they haven't come looking for them
        //after seven days, they probably don't need them.

        static void checkModify(string targetPath)
        {
            //for each of the files in the array
            foreach (string fileName in Directory.GetFiles(targetPath))
            {
                //combine the target path with the file name
                string myFile = Path.Combine(targetPath, fileName);
               //Console.WriteLine(myFile);
                
                //check the last time the file was written to, or modified
                DateTime DT = File.GetLastWriteTime(myFile);
                //Console.WriteLine(DT.ToString());
                //Console.ReadLine();

                string now = DateTime.Now.AddDays(-7).ToString();
                //Console.WriteLine(now.ToString());

                //if last write time is less than today minus seven days
                if (DT < DateTime.Now.AddDays(-7))
                {
                    //delete the file
                    File.Delete(myFile);
                }
            }
        }
    }
}
