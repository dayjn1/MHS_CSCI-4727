using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
    public class PageTableEntry
    {
        int PFN;                    // page frame number
        int PresentAbsentBit;       // present - 1, absent - 0
        int ProtectionBit;          // read, write, etc
        int ReferencedBit;          // set to 1 when page is accessed
        int CachingEnabledDisabled; // enables or diables caching
        int ModifiedBit;            // dirty bit, set to 1 on write

        // the above was pulled from https://www.geeksforgeeks.org/page-table-entries-in-page-table/
        // so they might not be necessary for our purposes

        public PageTableEntry()
        {

        }
    }
}
