using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eastAsiaFix
{
    class Program
    {
        /// <summary>
        /// Change the value of the eastAsia attribute from "Times New Roman" to "MS Mincho" in each style of the Normal template
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string normal = @"C:\users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Templates\Normal.dotm";
            string normalOld = @"C:\users\" + Environment.UserName + @"\AppData\Roaming\Microsoft\Templates\Normal.peAf";

            //back up the current Normal
            try
            {
                File.Copy(normal, normalOld, false);
            }
            catch
            {
                return;
            }

            try
            {
                //Open the Normal template RW as a WordprocessingDocument
                using (WordprocessingDocument myDocument = WordprocessingDocument.Open(normal, true))
                {
                    //get the StyleDefinitionsPart of the document
                    StyleDefinitionsPart stylesPart = myDocument.MainDocumentPart.StyleDefinitionsPart;

                    //if the StyleDefinitionsPart doesn't exist, return
                    if (stylesPart == null)
                    {
                        //Console.Out.WriteLine("No styles part found.");
                        return;
                    }

                    //for each style in the document, change the value of the eastAsia attribute to "MS Mincho"
                    foreach (var style in stylesPart.Styles.Descendants<Style>())
                    {
                        foreach (var rf in style.Descendants<RunFonts>())
                        {
                            if (rf.EastAsia != null)
                            {
                                //Console.Out.WriteLine("Found: {0}", rf.EastAsia.Value);
                                rf.EastAsia.Value = "MS Mincho";
                                //Console.Out.WriteLine("Value after change: {0}", rf.EastAsia.Value);
                            }
                        }
                    }
                }
            }

            catch (IOException ex)
            {
                //Console.Out.WriteLine(ex);
                return;
            }

            //Console.ReadLine();
        }
    }
}
