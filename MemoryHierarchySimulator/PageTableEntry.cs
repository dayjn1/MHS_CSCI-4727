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
        int ValidBit;               // is translation in PTE valid
        int ProtectionBits;         // read, write, etc
        int PresentBit;             // present - 1, absent - 0
        int DirtyBit;               // page modified since load
        int AccessedBit;            // set to 1 when page is accessed

        public PageTableEntry()
        {

        }
    }
}
