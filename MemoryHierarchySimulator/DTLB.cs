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
        public int[] TLB;
        public int[] tlbIndex;
        public int numOfLines, dtlbHits, dtlbMisses, tlbTag, tlbIndexNum;
        public int tag, index, offset;
        private int memoryKicked = 0;
        public int indexMask;
        private Random rand;
        //configuration
        private int indexSize { get; set; }
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
            for (int x = 0; x < TLB.Length; x++)
            {
                tlbIndex[x] = -1;
            }

   



        }
        public void updateTlbTag(int physicalAddress)
        {
            	//Can be reused for the TL
			
            TLB[index + indexSize * memoryKicked] = physicalAddress;
			
            tlbIndex[index + indexSize * memoryKicked] = tag;
        }

        public TlbHit findInTlb()
        {
            for (int x = index; x < TLB.Length; x = x + indexSize)
            {
                if (tlbIndex[x] == -1)
                {
                    index = x;
                    memoryKicked = 0;
                    return TlbHit.MISSED;
                }
                else if (tlbIndex[x] != tag)
                {

                }
                else
                {
                    index = x;
                    memoryKicked = 0;
                    return TlbHit.HIT;
                }
            }
            findRandomOffset();
            return TlbHit.CONF;


        }

        public void findTLBVariables(int inst)
        {
            int address = inst;
            index = address & indexMask;
            address = address >> indexBitAmount;
            tag = address;
        }

        public void clearCache()
        {
            for (int x = 0; x < TLB.Length; x++)
            {
                TLB[x] = -1;
                tlbIndex[x] = 0;
            }
            memoryKicked = 0;
        }

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
    }   



}
