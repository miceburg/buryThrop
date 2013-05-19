using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Reg_RC
{
    class Program
    {
        //Declare global variables
        public static RegistryKey rk;
        public static string myKey; 
        public static string restore;
        public static string[] roots = new string[5] {"1 - HKEY_CLASSES_ROOT", "2 - HKEY_CURRENT_USER", "3 - HKEY_LOCAL_MACHINE", "4 - HKEY_USERS", "5 - HKEY_CURRENT_CONFIG"};

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //run function to assign value to myKey
            assignKey();

            //if there is no argument or the argument is "delete", run deleteKey
            if (args.Length == 0 || args[0] == "delete")
            {
                deleteKey();
            }
            //else run restoreKey
            else if (args[0] == "restore")
            {
                restoreKey();
            }
        }

        /// <summary>
        /// Changes the value of "(Default)" to null
        /// </summary>
        static void deleteKey()
        {
            try
            {
                //we want to keep the current value, because some of them are difficult to restore (such as GUID)
                //rk.GetValue(null) refers to "(Default)".
                restore = rk.GetValue(null).ToString();
                //Set the value of "Restore" to the value of "(Default)"
                //If "Restore" does not exist, it will be created)
                rk.SetValue("Restore", restore, RegistryValueKind.String);
                //Set the value of "(Default)" [null] to an empty string
                rk.SetValue(null, "", RegistryValueKind.String);
            }
            catch (Exception ex)
            {
                //If an error is caught, write it to the console
                Console.WriteLine("Error encountered: " + ex.ToString());
                Console.WriteLine("Press Enter to abort...");
                Console.ReadLine();
            }
            //Close the key
            rk.Close();
        }

        static void restoreKey()
        {
            //Get the value from "Restore"
            restore = rk.GetValue("Restore").ToString();
            //Set the value of "(Default)" [null] to the value of "Restore"
            rk.SetValue(null, restore, RegistryValueKind.String);
            //Close the key
            rk.Close();
        }

        static char pickRoot()
        {
            //Prompt user for root directory of the key
            Console.WriteLine("What directory is the key in?");
            Console.WriteLine();
            //Write the possible values to the console using our array
            for (int i = 0; i < roots.Length; i++)
            {
                Console.WriteLine(roots[i]);
            }
            //write an extra line for readability
            Console.WriteLine();

            //Get the character of the key pressed and return it
            return Console.ReadKey().KeyChar;
        }

        static string getKey()
        {
            //Write blank lines for readability
            Console.WriteLine();
            Console.WriteLine();
            //Prompt user for path to key
            Console.WriteLine("What is the path to the key?");
            Console.WriteLine();
            //Get user's response and return it
            return Console.ReadLine();
        }

        static void assignKey()
        {
            //Get value from pickRoot
            char i = pickRoot();

            //Get value from getKey
            myKey = getKey();

            //Based on user's input from pickRoot, open the key we want to edit
            switch (i)
            {
                case '1':
                    rk = Registry.ClassesRoot.OpenSubKey(myKey, true);
                    break;
                case '2':
                    rk = Registry.CurrentUser.OpenSubKey(myKey, true);
                    break;
                case '3':
                    rk = Registry.LocalMachine.OpenSubKey(myKey, true);
                    break;
                case '4':
                    rk = Registry.Users.OpenSubKey(myKey, true);
                    break;
                case '5':
                    rk = Registry.CurrentConfig.OpenSubKey(myKey, true);
                    break;
                default:
                    Console.WriteLine("Invalid entry.  Press any key to abort.");
                    Console.ReadLine();
                    Environment.Exit(0);
                    break;
            }        
        }
    }
}