using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Drawing;

namespace MemoryHierarchySimulator
{
    class MMU
    {
        public int TLBindex = 0;
        public int PTindex = 0;
        public int pageOffset = 0;
        public int DCindex = 0;
        public int DCoffset = 0;
        public int L2Index = 0;
        public int L2Offset = 0;
        public static void Main(String[] args)
        {
            //Will hold the addresses from the inputted files
            string[] addressLines;
            string[] address;
            DataCache dc = new DataCache("L1");
            DataCache l2 = new DataCache("L2");
            DTLB tlb = new DTLB();


            //int that will hold the addresses int
            int virtAddress;
            updateConfigSettings();
            addressLines = ParseInputFiles();
            calculateConfig();
            DisplayConfigSettings();

            //runs through each address and sends it through TLB, Page Table, and Cache
            for (int x = 0; x < addressLines.Length; x++)
            {
                address = addressLines[x].Split(":");
                //address[0] is the read or write char

                //address[1] is the address itself
                virtAddress = Convert.ToInt32(address[1], 16);

                //L2 Cache example
                dc.updateWriteCache(132);
                dc.updateWriteCache(132);
                dc.updateWriteCache(388);

                l2.updateWriteCache(132);
                l2.updateWriteCache(132);
                l2.updateWriteCache(388);
                l2.updateWriteCache(900);
                l2.updateWriteCache(644);
                if(l2.updateWriteCache(1412) == CacheHit.CONF)
                {
                    if(l2.dirtyBits[l2.lastIndex])
                    {

                    }
                }

            }

            

            tlb.findTLBVariables(12);
            TlbHit t = tlb.findInTlb();
            

        }


        public static void DisplayConfigSettings()
        {
            Console.WriteLine("Data TLB contains " + ConfigurationManager.AppSettings.Get("TLB Number of sets") + " sets.");
            Console.WriteLine("Each set contains " + ConfigurationManager.AppSettings.Get("TLB Set size") + " entries.");
            Console.WriteLine("Number of bits used for the index is {0}.", ConfigurationManager.AppSettings.Get("TLB Index Bits"));
            Console.WriteLine();
            Console.WriteLine("Number of virtual pages is {0}.", ConfigurationManager.AppSettings.Get("PT Number of virtual pages"));
            Console.WriteLine("Number of physical pages is {0}.", ConfigurationManager.AppSettings.Get("PT Number of physical pages"));
            Console.WriteLine("Each page contains {0} bytes.", ConfigurationManager.AppSettings.Get("PT Page Size"));
            Console.WriteLine("Number of bits used for the page table index is {0}.", ConfigurationManager.AppSettings.Get("Page Table Index Bits"));
            Console.WriteLine("Number of bits used for the page offset {0}.", ConfigurationManager.AppSettings.Get("Page Offset Bits"));
            Console.WriteLine();
            Console.WriteLine("D-cache contains {0} sets.", ConfigurationManager.AppSettings.Get("DC Number of sets"));
            Console.WriteLine("Each set contains {0} entries.", ConfigurationManager.AppSettings.Get("DC Set size"));
            Console.WriteLine("Each line is {0} bytes.", ConfigurationManager.AppSettings.Get("DC Line size"));
            if (ConfigurationManager.AppSettings.Get("DC Write through/no write allocate") == "n")
                Console.WriteLine("The cache uses a write-allocate and write-back policy.");
            else
                Console.WriteLine("The cache uses a write-allocate and no write-back policy.");
            Console.WriteLine("Number of bites used for the index is {0}.", ConfigurationManager.AppSettings.Get("DC Index Bits"));
            Console.WriteLine("Number of bites used for the offset is {0}.", ConfigurationManager.AppSettings.Get("DC Offset Bits"));
            Console.WriteLine();

            Console.WriteLine("L2-cache contains {0} sets.", ConfigurationManager.AppSettings.Get("L2 Number of sets"));
            Console.WriteLine("Each set contains {0} entries.", ConfigurationManager.AppSettings.Get("L2 Set size"));
            Console.WriteLine("Each line is {0} bytes.", ConfigurationManager.AppSettings.Get("L2 Line size"));
            if (ConfigurationManager.AppSettings.Get("L2 Write through/no write allocate") == "n")
                Console.WriteLine("The cache uses a write-allocate and write-back policy.");
            else
                Console.WriteLine("The cache uses a write-allocate and no write-back policy.");
            Console.WriteLine("Number of bites used for the index is {0}.", ConfigurationManager.AppSettings.Get("L2 Index Bits"));
            Console.WriteLine("Number of bites used for the offset is {0}.", ConfigurationManager.AppSettings.Get("L2 Offset Bits"));
            Console.WriteLine();

            if (ConfigurationManager.AppSettings.Get("Virtual address") == "y")
                Console.WriteLine("The addresses read in are virtual addresses.");
            else
                Console.WriteLine("The addresses read in are physical addresses.");

        }

        public static void updateConfigSettings()
        {
            string[] lines;
            string fileName = "trace.config";
            string[] config = new string[16] {"","","","","","","","","","","","","","","",""};
            //Gets the size and coordinates from Test.txt 
            try
            {
                string path = Path.GetFullPath(fileName);     //Gets the path to Test.txt
                lines = System.IO.File.ReadAllLines(path);
                int index = 0;
                foreach(string l in lines)
                {
                    config[index++] = l.Split(":")[1];
                }
                ConfigurationManager.AppSettings.Set("TLB Number of sets", config[0]);
                ConfigurationManager.AppSettings.Set("TLB Set size", config[1]);
                ConfigurationManager.AppSettings.Set("PT Number of virtual pages", config[2]);
                ConfigurationManager.AppSettings.Set("PT Number of physical pages", config[3]);
                ConfigurationManager.AppSettings.Set("PT Page Size", config[4]);
                ConfigurationManager.AppSettings.Set("DC Number of sets", config[5]);
                ConfigurationManager.AppSettings.Set("DC Set size", config[6]);
                ConfigurationManager.AppSettings.Set("DC Line size", config[7]);
                ConfigurationManager.AppSettings.Set("DC Write through/no write allocate", config[8]);
                ConfigurationManager.AppSettings.Set("L2 Number of sets", config[9]);
                ConfigurationManager.AppSettings.Set("L2 Set size", config[10]);
                ConfigurationManager.AppSettings.Set("L2 Line size", config[11]);
                ConfigurationManager.AppSettings.Set("L2 Write through/no write allocate", config[12]);
                ConfigurationManager.AppSettings.Set("Virtual address", config[13]);
                ConfigurationManager.AppSettings.Set("TLB", config[14]);
                ConfigurationManager.AppSettings.Set("L2 cache", config[15]);
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Configuration file does not exist!");
                return;
            }
        }

        public static void calculateConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationManager.AppSettings.Set("TLB Index Bits", Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("TLB Number of sets")), 2).ToString());
            ConfigurationManager.AppSettings.Set("Page Table Index Bits", Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("PT Number of virtual pages")), 2).ToString());
            ConfigurationManager.AppSettings.Set("Page Offset Bits", Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("PT Page Size")), 2).ToString());
            ConfigurationManager.AppSettings.Set("DC Index Bits", Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("DC Number of sets")), 2).ToString());
            ConfigurationManager.AppSettings.Set("DC Offset Bits", Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("DC Line size")), 2).ToString());
            ConfigurationManager.AppSettings.Set("L2 Index Bits", Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("L2 Number of sets")), 2).ToString());
            ConfigurationManager.AppSettings.Set("L2 Offset Bits", Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("L2 Line size")), 2).ToString());
            config.Save(ConfigurationSaveMode.Modified, true);
        }


        /// <summary>Gets all the addresses from an inputted file</summary>
        /// <returns>String Array with addresses</returns>
        public static string[]? ParseInputFiles()
        {
            string fileName = "real_tr.dat";
            string[] lines;
            string[] configuration;
            
            
            
            //Uncomment the next two lines when ready for production


            //Console.WriteLine("Enter the file name you would like to open (real_tr.dat/trace.dat)");
            //fileName = Console.ReadLine();
            //Gets the size and coordinates from Test.txt 
            try
            {
                string path = Path.GetFullPath(fileName);     //Gets the path to Test.txt
                lines = System.IO.File.ReadAllLines(path);
                Console.Clear();
                return lines;
            }
            catch (Exception)
            {
                Console.WriteLine("File does not exist!");
                return null;
            }
            
        }

    }
}