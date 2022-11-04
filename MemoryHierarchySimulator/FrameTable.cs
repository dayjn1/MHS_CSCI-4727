using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
    class FrameTable
    {
        public List<int> Table;

        /// <summary>Creates a Frame table, which is a basic list<int> that is reordered as the simulation runs.</summary>
        public FrameTable(int NumPhysPages)
        {
            Table = new List<int>();

            for(int i = 0; i < NumPhysPages; i++)
            {
                Table.Add(i);
            }
        }

        /// <summary>Readjusts the frame table to keep LRU algorithm straight. Whenever a FN is used, it needs to be at the 
        /// end of the list.</summary>
        public void ReAdjustFrameTable(int PFN)
        {           
            Table.Remove(PFN);
            Table.Add(PFN);
        }

        /// <summary>Performs a basic LRU algorithm to get the next frame number.</summary>
        /// <returns>LRU frame number result.</returns>
        public int GetLRU()
        {
            int PFN = Table[0];
            Table.RemoveAt(0);
            Table.Add(PFN);
            return PFN;
        }
    }
}