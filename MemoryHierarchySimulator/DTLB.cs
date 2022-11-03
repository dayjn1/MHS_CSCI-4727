using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
    internal class DTLB
    {
        public int[] TLB;
        public int[] tlbIndex;
        public LRU[] lru;

        public int numOfLines, address;
        public int tag, index;
        //association set up
        private int memoryKicked = 0;
        public int indexMask;
        private Random rand;
        //configuration
        private int indexSize { get; set; }
        private int setSize { get; set; }
        private int numOfBitsForIndex { get; set; }
        public int indexBitAmount { get; set; }
        public int lastAddress { get; set; }
        public int lastIndex { get; set; }

        int[] test = new int[] { 6, 4, 0, 6, 2, 0, 0, 6, 0 };
        int wewew = 0;


        /**
	    * Method Name: DTLB <br>
	    * Method Purpose: Class constructor
	    * 
	    * <hr>
	    * Date created: 10/15/22 <br>
	    * @author Nick Farmer 
	    */
        public DTLB()
        {
            rand = new Random();
            tag = 0;
            index = 0;

            indexSize = int.Parse(ConfigurationManager.AppSettings.Get("TLB Number of sets"));

            setSize = int.Parse(ConfigurationManager.AppSettings.Get("TLB Set size"));

            indexBitAmount = (int)Math.Log(indexSize, 2);

            numOfLines = indexSize * setSize;
            indexMask = (int)Math.Pow(2, indexBitAmount) - 1;


            TLB = new int[numOfLines];
            tlbIndex = new int[numOfLines];
            lru = new LRU[indexSize];
            //set up TLB
            for (int x = 0; x < tlbIndex.Length; x++)
            {
                tlbIndex[x] = -1;
            }

            for (int x = 0; x < indexSize; x++)
            {
                lru[x] = new LRU();
            }





        }

        public TlbHit updateTLB(int address)
        {
            findTLBVariables(address);
            TlbHit hit = findInTlb();
            updateTlbTag(address);
            return hit;

        }
        /// <summary>Updates the TLB.</summary>
       
        public void updateTlbTag(int physicalAddress)
        {
            TLB[index + indexSize * memoryKicked] = physicalAddress;
            tlbIndex[index + indexSize * memoryKicked] = tag;
            lru[index % indexSize].ComputeAddress(address);
        }
        /// <summary>Finds whether the instruction hit or missed in the TLB.</summary>
        public TlbHit findInTlb()
        {
            for (int x = index; x < TLB.Length; x = x + indexSize)
            {
                if (tlbIndex[x] == -1)
                {
                    index = x;
                    memoryKicked = 0;
                    return TlbHit.MISS;
                }
                else if (tlbIndex[x] != tag)
                {

                }
                else
                {
                    index = x;
                    memoryKicked = 0;
                    lru[index % indexSize].ComputeAddress(address);
                    return TlbHit.HIT;
                }
            }
            findLRU();
            return TlbHit.CONF;


        }

        public void findTLBVariables(int inst)
        {
            address = inst;
            index = address & indexMask;
            address = address >> indexBitAmount;
            tag = address;
        }
        /// <summary>Clears the TLB.</summary>
        public void clearTLB()
        {
            for (int x = 0; x < TLB.Length; x++)
            {
                TLB[x] = -1;
                tlbIndex[x] = 0;
            }
            memoryKicked = 0;
        }
        /// <summary>Finds the random offset for the eviction policy.</summary>
        public void findRandomOffset()
        {
            switch (numOfLines)
            {
                case 1:
                    memoryKicked = 0;
                    break;
                case 2:
                case 4:
                    memoryKicked = rand.Next(numOfLines);
                    break;
            }
        }

        public void findLRU()
        {

            if (setSize == 1)
            {
                memoryKicked = 0;
                lastAddress = lru[index].GetLRU();
            }
            else
            {
                lastAddress = lru[index % indexSize].GetLRU();
                int lastTag = lastAddress >> (indexBitAmount);
                //int lastTag = test[wewew];
                //wewew++;
                for (int x = 0; x < setSize; x++)
                {
                    if (tlbIndex[index + indexSize * x] == lastTag) 
                    {
                        memoryKicked = x;
                        lastIndex = index + indexSize * x;
                    }
                }
            }
        }


    }
}
