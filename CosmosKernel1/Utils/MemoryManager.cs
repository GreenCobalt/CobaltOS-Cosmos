using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosKernel1.Utils
{
    class MemoryManager
    {
        public static uint getFreeRAM()
        {
            return getTotalRAM() - getUsedRAM();
        }
        public static uint getUsedRAM()
        {
            uint usedRAM = CPU.GetEndOfKernel() + 1024;
            return usedRAM / 1048576;
        }
        public static uint getTotalRAM()
        {
            return CPU.GetAmountOfRAM();
        }
        public static uint getUsedRAMPercent()
        {
            return (getUsedRAM() * 100) / getTotalRAM();
        }
        public static uint getFreeRAMPercent()
        {
            return 100 - getUsedRAMPercent();
        }
    }
}
