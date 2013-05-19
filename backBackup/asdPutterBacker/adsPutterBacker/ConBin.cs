using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace adsPutterBacker
{
    /// <summary>
    /// Class containing functions to convert binary data to ASCII text
    /// </summary>
    class ConBin
    {
        /// <summary>
        /// Function to convert binary (decimal in this case) to ASCII text
        /// </summary>
        /// <param name="myFile"></param>
        /// <returns></returns>
        public string BinHexText(string myFile)
        {
            /* 1 hex key = 1 byte 
             * 16 hex keys per line (16 * 1 = 16)
             * 16 lines per bloc (16 * 16 = 256)
             * 10 blocs before main body of text (256 * 10 = 2560)
             */

            //Read all the bytes in the file
            byte[] fileBytes = File.ReadAllBytes(myFile).Skip(2560).Take(140).ToArray();
            //Initialize a new StringBuilder
            StringBuilder sb = new StringBuilder();

            //string to represent hex conversion to char
            string bin2text = "";

            ////Regex to check for binary
            //Regex isBinary = new Regex(@"[01]");

            //string to store and output our byte translation
            string finalMessage = "";

            //Convert each byte to a string and append it to the StringBuilder
            foreach (byte b in fileBytes)
            {
                //The bytes are stored in a decimal format
                //Decimals between 31 and 126 represent the alphabet (upper and lower), numbers, and standard punctuation
                //Decimal 13 is a Return, so we want to catch those, too
                if ( b > 31 && b < 126 || b == 13)
                {
                    //Reset StringBuilder to 0, to make sure no previous characters remain
                    sb.Length = 0;

                    if (b == 13)
                    {
                        //Represent the return with two spaces
                        finalMessage = finalMessage + "  ";
                    }
                    //else the hex is not a space, and we want to convert it to a char
                    else
                    {
                        //Set StringBuilder length back to 0 to clear it
                        sb.Length = 0;

                        //Convert b to unsigned, 32-bit integer
                            //Convert that to a character
                                //Append the character to the StringBuilder
                        sb.Append(Convert.ToChar(Convert.ToUInt32(b)));
                        //Convert the StringBuilder to string.  
                        //We now have the character represented by the byte
                        bin2text = sb.ToString();

                        //Append the character to the current string
                        finalMessage = finalMessage + bin2text;
                    }
                }
            }
            //send the string out
            return finalMessage;
        }
    }
}
