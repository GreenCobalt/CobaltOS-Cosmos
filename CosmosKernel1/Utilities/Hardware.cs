using Cosmos.Core;
using Cosmos.HAL;
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

    class PCI
    {
        private static List<int> venID = new List<int>();
        public static void Init()
        {
            Console.WriteLine("<PCI> Initializing PCI Devices:");

            foreach (PCIDevice p in Cosmos.HAL.PCI.Devices)
            {
                Console.WriteLine("<PCI> - " + p.VendorID + " at " + p.bus + ":" + p.slot);
                if (!venID.Contains(p.VendorID))
                {
                    venID.Add(p.VendorID);
                }
            }

            Console.WriteLine("<PCI> Vendor IDs:");
            foreach (int v in venID)
            {
                Console.WriteLine("<PCI> " + v);
            }

        }
        public static void PCICommand(String[] args)
        {
            if (args[1] == "lsven")
            {
                Console.WriteLine("<PCI> Vendor IDs:");
                foreach (int v in venID)
                {
                    Console.WriteLine("<PCI> " + v);
                }
            }
            else if (args[1] == "lsdev")
            {
                Console.WriteLine("<PCI> PCI Devices:");
                foreach (PCIDevice p in Cosmos.HAL.PCI.Devices)
                {
                    Console.WriteLine("<PCI> - " + p.VendorID + " at " + p.bus + ":" + p.slot);
                }
            }
            else
            {
                Console.WriteLine("Subcommand not found!");
            }
        }
    }
}