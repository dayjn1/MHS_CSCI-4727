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
        public byte[][] TLB;
        public int[] tlbIndex;
        public int numOfLines, dtlbHits, dtlbMisses, tlbTag, tlbIndexNum;
        public int tag, index, offset;
        private int memoryKicked = 0;

        //configuration
        private int numOfSets { get; set; }
        private int setSize { get; set; }
        private int numOfBitsForIndex { get; set; }

        public int indexBitAmount { get; set; }

        /**
	    * Method Name: DTLB <br>
	    * Method Purpose: Class constructor
	    * 
	    * <hr>
	    * Date created: 10/15/22 <br>
	    * @author Nick Farmer 
	    */
        public DTLB(byte virtAddress)
        {
            tag = 0;
            index = 0;
            offset = 0;

            numOfSets = int.Parse(ConfigurationManager.AppSettings.Get("TLB Number of sets"));

            setSize = int.Parse(ConfigurationManager.AppSettings.Get("TLB Set size"));
            indexBitAmount = (int)Math.Log(numOfSets, 2);
            numOfLines = numOfSets * setSize;

            TLB = new byte[numOfLines][];
            tlbIndex = new int[numOfLines];



        }
        public void updateCacheTag()
        {
            /*	Can be reused for the TLB
			//Adds new byte of memory into the cache
			byte[] mem = new byte[numberOfBytes];

			for (int x = 0; x < numberOfBytes; x++)
			{
				mem[x] = (byte)memory.MainMemory[address + x];
			}
			
			l1Cache[index + indexSize * memoryKicked] = mem;
			*/
            tlbIndex[index + numOfSets * memoryKicked] = tag;
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
