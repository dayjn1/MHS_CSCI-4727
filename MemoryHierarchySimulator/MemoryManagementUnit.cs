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
        public static int VIRTpageNumber = 0;
        public static int TLBtag = 0;
        public static int TLBindex = 0;
        public static int PTindex = 0;
        public static int pageOffset = 0;
        public static int DCtag = 0;
        public static int DCindex = 0;
        public static int DCoffset = 0;
        public static int L2tag = 0;
        public static int L2Index = 0;
        public static int L2Offset = 0;
        public static int TLBhit, TLBmiss, PThit, PTmiss, DChit, DCmiss, L2hit, L2miss,
            reads, writes, MMrefs, DiskRefs, PTrefs;
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
            int physicalPageNum = 0;
            string TLBresult = "MISS", PTresult = "MISS", DCresult = "MISS", L2result = "MISS";
            updateConfigSettings();
            addressLines = ParseInputFiles();
            calculateConfig();
            DisplayConfigSettings();

            Console.WriteLine("Virtual  Virt.  Page TLB    TLB TLB  PT   Phys        DC  DC          L2  L2");
            Console.WriteLine("Address  Page # Off  Tag    Ind Res. Res. Pg # DC Tag Ind Res. L2 Tag Ind Res.");
            Console.WriteLine("-------- ------ ---- ------ --- ---- ---- ---- ------ --- ---- ------ --- ----");

            //runs through each address and sends it through TLB, Page Table, and Cache
            for (int x = 0; x < addressLines.Length; x++)
            {
                address = addressLines[x].Split(":");
                //address[0] is the read or write char

                //address[1] is the address itself
                virtAddress = Convert.ToInt32(address[1], 16);

                //TLB checks to see if the physical address exists

                //TLB HIT, Skip PageTable

                //TLB MISS, Access PageTable

                //PageTable finds the physical address for the virtual address

                //PT MISS, Update the PT, return physical page number

                //PT HIT, return the physical page number

                //TLB Update regardless

                //Access the Data Cache with the physical address

                //DC READ HIT, bypass the L2 cache

                //DC READ CONF/MISS, Pass address to the L2 cache to see if it hits or misses

                //DC WRITE HIT Write-Back, Update DC Cache

                //DC WRITE CONF Write-Back, Update L2 Cache with the address that is being overwritten

                //DC WRITE HIT/CONF Write-Through, Update DC and L2 Cache

                //DC MISS Write-Back, Update DC and L2 Cache

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

                Console.WriteLine("{0,8} {1,6} {2,4} {3,6} {4,3} {5,4} {6,4} {7,4} {8,6} {9,3} {10,4} {11,6} {12,3} {13,4}", 
                    address[1], VIRTpageNumber, pageOffset, TLBtag, TLBindex, TLBresult, PTresult, physicalPageNum, DCtag, DCindex, 
                    DCresult, L2tag, L2Index, L2result);

            }


            tlb.findTLBVariables(12);
            TlbHit t = tlb.findInTlb();

            DisplayFinalStats();

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
        //Prints out the final stats for the program
        public static void DisplayFinalStats()
        {
            Console.WriteLine("\n\n\nSimulation Statistics\n");
            Console.WriteLine("dtlb hits: {0}", TLBhit);
            Console.WriteLine("dtlb misses: {0}", TLBmiss);
            try
            {
                Console.WriteLine("dtlb hit ratio: {0}\n", TLBhit / TLBmiss);
            }
            catch(Exception ex)
            {
                Console.WriteLine("dtlb hit ratio: 1\n");
            }

            Console.WriteLine("pt hits: {0}", PThit);
            Console.WriteLine("pt faults: {0}", PTmiss);
            try
            {
                Console.WriteLine("pt hit ratio: {0}\n", PThit / PTmiss);
            }
            catch (Exception ex)
            {
                Console.WriteLine("pt hit ratio: 1\n");
            }

            Console.WriteLine("dc hits: {0}", DChit);
            Console.WriteLine("dc misses: {0}", DCmiss);
            try
            {
                Console.WriteLine("dc hit ratio: {0}\n", DChit / DCmiss);
            }
            catch (Exception ex)
            {
                Console.WriteLine("dc hit ratio: 1\n");
            }

            Console.WriteLine("L2 hits: {0}", L2hit);
            Console.WriteLine("L2 misses: {0}", L2miss);
            try
            {
                Console.WriteLine("L2 hit ratio: {0}\n", L2hit / L2miss);
            }
            catch (Exception ex)
            {
                Console.WriteLine("L2 hit ratio: 1\n");
            }

            Console.WriteLine("Total Reads: {0}", reads);
            Console.WriteLine("Total Writes: {0}", writes);
            try
            {
                Console.WriteLine("Ratio of Reads: {0}\n", reads / writes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ratio of Reads: 1\n");
            }
   

            Console.WriteLine("main memory refs : {0}", MMrefs);
            Console.WriteLine("page table refs : {0}", PTrefs);
            Console.WriteLine("disk refs : {0}", DiskRefs);




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