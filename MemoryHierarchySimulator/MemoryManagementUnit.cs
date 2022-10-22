using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace MemoryHierarchySimulator
{
    class MMU
    {
        static public void Main(String[] args)
        {
            //Will hold the addresses from the inputted files
            string[] addressLines;
            string[] address;
            DataCache dc = new DataCache();

            //int that will hold the addresses int
            int virtAddress;
            DisplayConfigSettings();
            addressLines = ParseInputFiles();

            //runs through each address and sends it through TLB, Page Table, and Cache
            for(int x = 0; x < addressLines.Length; x++)
            {
                address = addressLines[x].Split(":");
                //address[0] is the read or write char

                //address[1] is the address itself
                virtAddress = Convert.ToInt32(address[1], 16);
                dc.findCacheVariables(virtAddress);
            }
            //Cache example

            CacheHit ch = dc.findInstructionInCache();
            dc.updateCacheTag();

        }


        public static void DisplayConfigSettings()
        {
            Console.WriteLine("Data TLB contains " + ConfigurationManager.AppSettings.Get("TLB Number of sets") + " sets.");
            Console.WriteLine("Each set contains " + ConfigurationManager.AppSettings.Get("TLB Set size") + " entries.");
            Console.WriteLine("Number of bits used for the index is <INSERT CALCULATION>.");
            Console.WriteLine();
            Console.WriteLine("Number of virtual pages is {0}.", ConfigurationManager.AppSettings.Get("PT Number of virtual pages"));
            Console.WriteLine("Number of physical pages is {0}.", ConfigurationManager.AppSettings.Get("PT Number of physical pages"));
            Console.WriteLine("Each page contains {0} bytes.", ConfigurationManager.AppSettings.Get("PT Page Size"));
            Console.WriteLine("Number of bits used for the page table index is {0}.", "<INSERT CALCULATION>");
            Console.WriteLine("Number of bits used for the page offset {0}.", "<INSERT CALCULATION>");
            Console.WriteLine();
            Console.WriteLine("D-cache contains {0} sets.", ConfigurationManager.AppSettings.Get("DC Number of sets"));
            Console.WriteLine("Each set contains {0} entries.", ConfigurationManager.AppSettings.Get("DC Set size"));
            Console.WriteLine("Each line is {0} bytes.", ConfigurationManager.AppSettings.Get("DC Line size"));
            if (ConfigurationManager.AppSettings.Get("DC Write through/no write allocate") == "n")
                Console.WriteLine("The cache uses a write-allocate and write-back policy.");
            else
                Console.WriteLine("The cache uses a write-allocate and no write-back policy.");
            Console.WriteLine("Number of bites used for the index is {0}.", "<INSERT CALCULATION>");
            Console.WriteLine("Number of bites used for the offset is {0}.", "<INSERT CALCULATION>");
            Console.WriteLine();

            Console.WriteLine("L2-cache contains {0} sets.", ConfigurationManager.AppSettings.Get("L2 Number of sets"));
            Console.WriteLine("Each set contains {0} entries.", ConfigurationManager.AppSettings.Get("L2 Set size"));
            Console.WriteLine("Each line is {0} bytes.", ConfigurationManager.AppSettings.Get("L2 Line size"));
            if (ConfigurationManager.AppSettings.Get("L2 Write through/no write allocate") == "n")
                Console.WriteLine("The cache uses a write-allocate and write-back policy.");
            else
                Console.WriteLine("The cache uses a write-allocate and no write-back policy.");
            Console.WriteLine("Number of bites used for the index is {0}.", "<INSERT CALCULATION>");
            Console.WriteLine("Number of bites used for the offset is {0}.", "<INSERT CALCULATION>");
            Console.WriteLine();

            if (ConfigurationManager.AppSettings.Get("Virtual address") == "y")
                Console.WriteLine("The addresses read in are virtual addresses.");
            else
                Console.WriteLine("The addresses read in are physical addresses.");

        }

        /// <summary>Gets all the addresses from an inputted file</summary>
        /// <returns>String Array with addresses</returns>
        public static string[] ParseInputFiles()
        {
            string fileName = "";
            string[] lines;
            Console.WriteLine("Enter the file name you would like to open (real_tr.dat/trace.dat)");
            fileName = Console.ReadLine();
            //Gets the size and coordinates from Test.txt 
            try
            {
                string path = Path.GetFullPath(fileName);     //Gets the path to Test.txt
                lines = System.IO.File.ReadAllLines(path);
                return lines;
            }
            catch(Exception ex)
            {
                Console.WriteLine("File does not exist!");
                return null;
            }
        }

    }
}
