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
        FrameTable FT;
        int Hits = 0;
        int Misses = 0;

        public PageTable(int NumVirtPages, int NumPhysPages, int PageSize)
        {
            PT = new PageTableEntry[NumVirtPages];
            FT = new FrameTable(NumPhysPages);

            for(int i = 0; i < PT.Length; i++)  // create all pages, setting each to invalid
            {
                PT[i] = new PageTableEntry();
            }
            
        }

        public void ProcessMemoryReference(string Reference)
        {
            string AccessType = Reference.Substring(0, 1);
        }

        public PageTableEntry PassToTLB()
        {

        }

        public int GetPFN(int VPN)
        {
            if (PT[VPN].PFN != -1 && PT[VPN].ValidBit)
            {
                FT.ReAdjustFrameTable(PT[VPN].PFN);
                Hits++;
                return PT[VPN].PFN;
            }
            else
            {
                Misses++;
                PT[VPN].PFN = FT.GetLRU();
                PT[VPN].ValidBit = true;

                InvalidateEntries(PT[VPN].PFN);

                return PT[VPN].PFN;
            }
                
        }

        public void InvalidateEntries(int PFN)
        {
            foreach(var entry in PT)
            {
                if(entry.PFN == PFN)
                {
                    entry.ValidBit = false;
                }
            }
        }

    }
}