using Cosmos.HAL;
using Cosmos.HAL.Drivers.USB;
using System;
using System.Collections.Generic;

namespace CobaltOS.Utilities
{
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
            } else if (args[1] == "lsdev")
            {
                Console.WriteLine("<PCI> PCI Devices:");
                foreach (PCIDevice p in Cosmos.HAL.PCI.Devices)
                {
                    Console.WriteLine("<PCI> - " + p.VendorID + " at " + p.bus + ":" + p.slot);
                }
            } else
            {
                Console.WriteLine("Subcommand not found!");
            }
        }
    }
}
