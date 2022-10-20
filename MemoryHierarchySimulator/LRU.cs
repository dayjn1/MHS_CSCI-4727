using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
	internal class LRU
	{
        List<int> addresses = new List<int>();

        //Process an address and place it at the end of the list
        public void ComputeAddress(int address)
        {
            //if an address is in the list then remove it and then add it again so that it is the most recent in the list
            if (ContainsAddress(address))
            {
                addresses.Remove(address);
            }
            Add(address);
        }

        //Checks if an address already exits in the list
        private bool ContainsAddress(int address)
        {
            if (addresses.Contains(address))
                return true;
            else
                return false;
        }

        //Test function to see the contents of the list
        public void Print()
        {
            foreach (int i in addresses)
            {
                Console.WriteLine(i);
            }
        }

        //Adds address to the list
        private void Add(int address)
        {
            addresses.Add(address);
        }

        public int GetCount()
        {
            return addresses.Count;
        }

        //Returns the LRU address which is always at the 0 index and removes it from the list
        public int GetLRU()
        {
            int retAddress = addresses[0];
            addresses.RemoveAt(0);
            return retAddress;
        }
    }
}
