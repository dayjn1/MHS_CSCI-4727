﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
    public class PageTableEntry
    {
        public int PFN;                     // page frame number
        public bool ValidBit;               // is translation in PTE valid
        public int ProtectionBits;          // read, write, etc
        public bool DirtyBit;               // page modified since load

        public PageTableEntry()
        {
            this.PFN = -1;
            this.ValidBit = false;
            this.ProtectionBits = 0;
            this.DirtyBit = false;
        }
    }
}
