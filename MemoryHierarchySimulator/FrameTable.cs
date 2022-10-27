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

        // Since no connection to memory, just creating a basic list of all possible PFN and reordering it as simulation runs
        public FrameTable(int NumPhysPages)
        {
            Table = new List<int>();

            for(int i = 0; i < NumPhysPages; i++)
            {
                Table.Add(i);
            }
        }

        // All PFN are loaded on creation of class, no need to check if it exists. Just updates the list
        public void ReAdjustFrameTable(int PFN)
        {           
            Table.Remove(PFN);
            Table.Add(PFN);
        }
       
        // Gets the LRU PFN and readds to the back of the list
        public int GetLRU()
        {
            int PFN = Table[0];
            Table.RemoveAt(0);
            Table.Add(PFN);
            return PFN;
        }
    }
}