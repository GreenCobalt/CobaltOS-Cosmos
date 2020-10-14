using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CobaltOS.Utilities
{
    class Hardware
    {
        public static Boolean onRealHardware()
        {
            if (Cosmos.HAL.PCI.GetDevice(Cosmos.HAL.VendorID.VMWare, Cosmos.HAL.DeviceID.SVGAIIAdapter) != null || Cosmos.HAL.PCI.GetDevice(Cosmos.HAL.VendorID.Bochs, Cosmos.HAL.DeviceID.BGA) != null || Cosmos.HAL.PCI.GetDevice(Cosmos.HAL.VendorID.VirtualBox, Cosmos.HAL.DeviceID.VBVGA) != null)
            {
                return false;
            }
            return true;
        }
    }

    class Memory
    {
        public static long getFreeRAM()
        {
            return getTotalRAM() - getUsedRAM();
        }
        public static long getUsedRAM()
        {
            return CPU.GetEndOfKernel() + 1024 / 1048576;
        }
        public static uint getTotalRAM()
        {
            return CPU.GetAmountOfRAM();
        }
        public static long getUsedRAMPercent()
        {
            return (getUsedRAM() * 100) / getTotalRAM();
        }
        public static long getFreeRAMPercent()
        {
            return 100 - getUsedRAMPercent();
        }
    }
}