using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryHierarchySimulator
{
    internal class DataCache
    {
		public byte[][] l1Cache;
		//Used to check if the tags are the same from addresses
		public int[] tagIndexCache;
		//Number of bytes that are going to be in a block

		//Sets up the association for the cache
		private int association = 1;
		private int memoryKicked = 0;
		private Random rand;

		public int tag, index, offset, cacheLines;

		//Allows for configuration
		public int offsetBitAmount { get; set; }
		public int indexBitAmount { get; set; }
		public int indexSize { get; set; }
		//Number of words allowed in the cache
		public int numberOfBytes { get; set; }
		public int offsetMask;
		public int indexMask;

		/**
	    * Method Name: DataCache <br>
	    * Method Purpose: Class constructor
	    * 
	    * <hr>
	    * Date created: 4/19/21 <br>
	    * @author Samuel Reynolds
	    */
		public DataCache()
		{
			tag = 0;
			index = 0;
			offset = 0;
			//association = config.associativity;

			rand = new Random();
			//Finds how many bits will be allowed in the offset
			offsetBitAmount = (int)Math.Log(int.Parse(ConfigurationManager.AppSettings.Get("DC Line size")), 2);
			//Finds how many bits will be in the index
			indexSize = int.Parse(ConfigurationManager.AppSettings.Get("DC Number of sets"));
			
			association = int.Parse(ConfigurationManager.AppSettings.Get("DC Set size"));
			indexBitAmount = (int)Math.Log(indexSize, 2);

			cacheLines = int.Parse(ConfigurationManager.AppSettings.Get("DC Number of sets")) * association;

			numberOfBytes = (int)Math.Pow(2, offsetBitAmount) + 2;
			//This should be configurable in the future to allow 2/4 way association
			offsetMask = (int)Math.Pow(2, offsetBitAmount) - 1;
			indexMask = (int)Math.Pow(2, indexBitAmount) - 1;

			l1Cache = new byte[cacheLines][];
			tagIndexCache = new int[cacheLines];
			//Setting up the cache to hold memory
			for (int x = 0; x < l1Cache.Length; x++)
			{
				tagIndexCache[x] = -1;
			}
		}
		
		/// <summary>Updates the cache.</summary>
		/// <param name="address">The address.</param>
		/// <param name="memory">The main memory.</param>
		///   <para>
		/// The cache.
		/// </para>
		/// </param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void updateCacheTag()
		{
			/*	Can be reused for the TLB
			//Adds new byte of memory into the cache
			byte[] mem = new byte[numberOfBytes];

			for (int x = 0; x < numberOfBytes; x++)
			{
				mem[x] = (byte)memory.MainMemory[address + x];
			}
			
			l1Cache[index + indexSize * memoryKicked] = mem;
			*/
			tagIndexCache[index + indexSize * memoryKicked] = tag;
		}

		/* May be useful for TLB
		/// <summary>Updates the cache with write instructions.</summary>
		/// <param name="result">The result.</param>
		public void updateWriteCache(int result)
		{
			l1Cache[index][offset] = (byte)((result & 16711680) >> 16);     //Stores the MSB value of r0 at the address in cache
			l1Cache[index][offset + 1] = (byte)((result & 65280) >> 8);     //Stores the TSB value of r0 at the address in cache
			l1Cache[index][offset + 2] = (byte)(result & 255);              //Stores the LSB value of r0 at the address in cache
		}
		*/

		/// <summary>
		///   <para>
		/// Finds the cache variables in order to check if the address is in the cache.
		/// </para>
		/// </summary>
		/// <param name="inst">The instruction.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="index">The index.</param>
		/// <param name="tag">The tag.</param>
		public void findCacheVariables(int inst)
		{
			int address = inst;
			offset = address & offsetMask;
			address = address >> offsetBitAmount;
			index = address & indexMask;
			address = address >> indexBitAmount;
			tag = address;
		}

		/// <summary>Finds whether the instruction hit or missed in the cache.</summary>
		/// <param name="instruction">The instruction.</param>
		public CacheHit findInstructionInCache()
		{
			//finds the offset for the types of association
			
			for (int x = index; x < l1Cache.Length; x = x + indexSize)
			{
				if (tagIndexCache[x] == -1)
				{
					index = x;
					memoryKicked = 0;
					return CacheHit.MISSED;
				}
				else if (tagIndexCache[x] != tag)
				{

				}
				else
				{
					index = x;
					memoryKicked = 0;
					return CacheHit.HIT;
				}
			}
			findRandomOffset();
			return CacheHit.CONF;
		}

        /// <summary>Clears the cache.</summary>
        public void clearCache()
		{
			for (int x = 0; x < l1Cache.Length; x++)
			{
				l1Cache[x] = null;
				tagIndexCache[x] = 0;
			}
			memoryKicked = 0;
		}

        /// <summary>Finds the random offset for the eviction policy.</summary>
        public void findRandomOffset()
		{
			switch (association)
			{
				case 1:
					memoryKicked = 0;
					break;
				case 2:
				case 4:
					memoryKicked = rand.Next(association);
					break;
			}
		}
	}

}
