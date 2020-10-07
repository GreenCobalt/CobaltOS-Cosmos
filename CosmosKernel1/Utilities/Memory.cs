using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosKernel1.Utils
{
    class Memory
    {
        public static uint getFreeRAM()
        {
            return getTotalRAM() - getUsedRAM();
        }
        public static uint getUsedRAM()
        {
            return CPU.GetEndOfKernel() + 1024 / 1048576;
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
