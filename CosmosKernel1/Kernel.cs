using System;
using Sys = Cosmos.System;
using Cosmos.Core;
using System.IO;
using Console = System.Console;
using Cosmos.System.FileSystem;
using Cosmos.HAL;
using CosmosKernel1.Utils;
using CosmosKernel1.GUI;
using System.Reflection.Metadata;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System.Text;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        public static double osVersion = 0.1;
        public static readonly String cpuString = getCPU();

        public static CosmosVFS fs;
        private static String cd = @"0:\";

        public static Boolean graphicsMode = false;
        private static Boolean fsMode = false;
        public static Boolean newGraphics = false;
        public static Boolean enableFs = false;

        protected override void BeforeRun()
        {

            Console.WriteLine("Enable FS? (y / n) (Don't use on real hardware!)");
            if (Console.ReadLine() == "y")
            {
                enableFs = true;
                fs = new Sys.FileSystem.CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            }

            Console.Clear();
            Console.WriteLine("    ##### ##### ####  ##### #  #######  ##### #####    ");
            Console.WriteLine("   #     #   # #  #  #   # #     #       #   # #       ");
            Console.WriteLine("  #     #   # ####  ##### #     #         #   # #####  ");
            Console.WriteLine(" #     #   # #   # #   # #     #           #   #     # ");
            Console.WriteLine("##### ##### ##### #   # ##### #             ##### #####");

            Console.WriteLine("CPU: " + cpuString);
            Console.WriteLine("RAM: " + MemoryManager.getTotalRAM() + " MB");

            if (enableFs)
            {
                Console.WriteLine("Filesystem: " + fs.GetFileSystemType("0:/") + ", " + fs.GetAvailableFreeSpace(@"0:\") / 1000000 + " MB");
            }

            //initGUI();
        }

        protected override void Run()
        {
            if (graphicsMode == true)
            {
                GUIManager.tick();
            }
            else if (fsMode == true)
            {
                Console.Write(cd + " >");
                processFSConsole(Console.ReadLine());
            }
            else
            {
                Console.Write("> ");
                processConsole(Console.ReadLine());
            }
        }

        private void initGUI()
        {
            Console.WriteLine("Would you like to use VM or real hardware graphics drivers? (v or r)");
            if (Console.ReadLine() == "v")
            {
                newGraphics = true;
            }
            else
            {
                newGraphics = false;
            }
            graphicsMode = true;
            DisplayDriver.init(newGraphics);
            DisplayDriver.initScreen();
        }

        private void processConsole(String input)
        {
            if (input == "gui")
            {
                initGUI();
                return;
            }
            else if (input == "cpu")
            {
                Console.WriteLine("CPU: " + cpuString);
                return;
            }
            else if (input == "mem")
            {
                Console.WriteLine("RAM: " + (Cosmos.Core.CPU.GetAmountOfRAM() < 1024 ? Cosmos.Core.CPU.GetAmountOfRAM() + " MB" : Cosmos.Core.CPU.GetAmountOfRAM() / 1024.00 + " GB"));
                return;
            }
            else if (input == "time")
            {
                Console.WriteLine("Current Date: " + Cosmos.HAL.RTC.Month + "/" + Cosmos.HAL.RTC.DayOfTheMonth + "/" + Cosmos.HAL.RTC.Year + " " + Cosmos.HAL.RTC.Hour + ":" + Cosmos.HAL.RTC.Minute + ":" + Cosmos.HAL.RTC.Second.ToString().PadLeft(2, '0'));
                return;
            }
            else if (input == "help")
            {
                Console.WriteLine("Help:");
                Console.WriteLine("    cpu: Returns CPU model and speed.");
                Console.WriteLine("    mem: Returns Memory size.");
                Console.WriteLine("    time: Returns current time according to the BIOS.");
                Console.WriteLine("    gui: Activates the GUI.");
                Console.WriteLine("    help: Returns this message.");
                return;
            }
            else if (input == "fs")
            {
                if (enableFs)
                {
                    fsMode = true;
                } else
                {
                    Console.WriteLine("FS is not enabled! Please restart and select 'y' when prompted!");
                }

                return;
            }
            else if (input == "shutdown")
            {
                shutdown(false);
            }
            else if (input == "restart")
            {
                shutdown(true);
            }
            else
            {
                Console.WriteLine("Unknown command! Use 'help' for help!");
                return;
            }
        }

        public static void shutdown(Boolean reboot)
        {
            if (reboot) Cosmos.System.Power.Reboot();
            else Cosmos.System.Power.Shutdown();
        }

        private static void processFSConsole(String input)
        {
            if (input == "exit")
            {
                fsMode = false;
                return;
            }

            var directory_list = fs.GetDirectoryListing(cd);
            if (input == "ls")
            {
                foreach (var directoryEntry in directory_list)
                {
                    if (directoryEntry.mEntryType == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                    {
                        Console.WriteLine(" - " + directoryEntry.mName);
                    }
                    else
                    {
                        Console.WriteLine(directoryEntry.mName);
                    }
                }
            }
            else if (input.Split(" ")[0] == "cd")
            {
                String[] inputSplit = input.Split(" ");
                if (inputSplit.Length > 1)
                {
                    Console.WriteLine("go");
                    String[] pieces = input.Split(new[] { ',' }, 2);
                    Console.WriteLine("go 2");
                    if (Directory.Exists(pieces[1]))
                    {
                        Console.WriteLine("Exists");
                    }
                    else
                    {
                        Console.WriteLine("Not Exists");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid syntax! cd [dir] ");
                }
            }
            else if (input.Split(" ")[0] == "mkdir")
            {
                String[] inputSplit = input.Split(" ");
                if (inputSplit.Length > 1)
                {
                    String dir = "";
                    for (int i = 1; i < inputSplit.Length; i++)
                    {
                        dir = dir + inputSplit[i];
                    }
                    fs.CreateDirectory(cd + dir);
                    Console.WriteLine("Success!");
                }
                else
                {
                    Console.WriteLine("Invalid syntax! mkdir [name] ");
                }
            }
            else if (input.Split(" ")[0] == "delfile")
            {
                String[] inputSplit = input.Split(" ");
                if (inputSplit.Length == 2)
                {
                    File.Delete(@"0:\" + inputSplit[1]);
                    Console.WriteLine("Success!");
                }
                else
                {
                    Console.WriteLine("Invalid Syntax!");
                }
            }
            else if (input.Split(" ")[0] == "rdfile")
            {
                String[] inputSplit = input.Split(" ");
                if (inputSplit.Length == 2)
                {
                    FileStream s = File.OpenRead(inputSplit[1]);
                    byte[] a = new byte[s.Length];
                    s.Read(a, 0, a.Length);
                    foreach (Byte b in a)
                    {
                        Console.Write((char) b);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Syntax!");
                }
            }
            else
            {
                Console.WriteLine("Unknown");
            }
        }

        public static void WaitSeconds(int secNum)
        {
            int StartSec = Cosmos.HAL.RTC.Second;
            int EndSec;
            if (StartSec + secNum > 59)
            {
                EndSec = 0;
            }
            else
            {
                EndSec = StartSec + secNum;
            }
            while (Cosmos.HAL.RTC.Second != EndSec) { }
        }

        private static String getCPU()
        {
            String returnString = Cosmos.Core.CPU.GetCPUVendorName();
            return returnString;
        }
    }
}