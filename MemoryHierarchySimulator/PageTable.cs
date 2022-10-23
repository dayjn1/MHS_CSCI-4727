using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
    // set up as a linear PT
    // OSTEP Notes:
    // The simplest form is called a linear page table, which is just an array.
    // The OS indexes the array by the virtual page number(VPN), and looks up
    // the page-table entry(PTE) at that index in order to find the desired physical frame number(PFN)

    public class PageTable
    {
        PageTableEntry[] PT;

        public PageTable(int NumVirtPages, int NumPhysPages, int PageSize)
        {
            PT = new PageTableEntry[NumVirtPages];

            for(int i = 0; i < PT.Length; i++)  // create all pages, setting each to invalid
            {
                PT[i] = new PageTableEntry();
            }
            
        }

        public int CheckVPN(int VPN)
        {
            if (PT[VPN].PFN != -1)
                return PT[VPN].PFN;
            else
                return -1;          // signal that VPN does not map to a frame, load data into frame using LRU replacement alg
        }


    }
}
