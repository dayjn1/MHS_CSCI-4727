using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
    internal class DTLB
    {
        public byte[] TLB;
        public int numOfLines;

        //configuration
        private int numOfSets { get; set; }
        private int setSize { get; set; }
        private int numOfBitsForIndex { get; set; }

        public DTLB(byte virtAddress)
        {
            numOfSets = int.Parse(ConfigurationManager.AppSettings.Get("TLB Number of sets"));

            setSize = int.Parse(ConfigurationManager.AppSettings.Get("TLB Set size"));

            numOfLines = numOfSets * setSize;

            TLB = new byte[numOfLines];

        }

        public TlbHit findInTlb()
        {
            return 0;
        }

        public void clearCache()
        {
            
        }
    }   



}
