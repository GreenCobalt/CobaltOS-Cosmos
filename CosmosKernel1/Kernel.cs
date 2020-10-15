using System;
using Sys = Cosmos.System;
using System.IO;
using Console = System.Console;
using Cosmos.System.FileSystem;
using CobaltOS.GUI;
using System.Text;
using CobaltOS.Utilities;
using Cosmos.HAL;
using System.Collections.Generic;
using CobaltOS.Network;

namespace CobaltOS
{
    public class Kernel : Sys.Kernel
    {
        public static string file;

        public static double osVersion = 0.1;

        public static readonly String cpuString = getCPU(false);
        public static readonly String cpuStringShort = getCPU(true);

        public static CosmosVFS fs;
        private static String cd = @"0:\";

        public static Boolean graphicsMode = false;
        private static Boolean fsMode = false;
        public static Boolean newGraphics = false;
        public static Boolean enableFs = false;

        public static Boolean systemExists = true;
        public static CobaltOS.Network.IPV4.Config LocalNetworkConfig;

        public static List<string> networkInterfaces = new List<string>();

        protected override void BeforeRun()
        {
            enableFs = true;
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            printLogoConsole();
            Console.Write("Detected Drives: ");
            Console.WriteLine(DriveInfo.GetDrives().Length);
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                Console.WriteLine(" - " + d.Name + " (" + d.GetType() + ") " + (d.TotalSize / 1048576) + "MB");
            }
            WaitSeconds(2);

            if (!Directory.Exists(@"0:\System\"))
            {
                fs.CreateDirectory(@"0:\System\");
            }

            if (!File.Exists(@"0:\SYS\fs.cfg"))
            {
                printLogoConsole();

                Console.WriteLine(@"The filesystem was not formatted with CobaltOS, so it cannot be used.");
                Console.WriteLine(@"Would you like to format it? (y/n)");
                Console.WriteLine("WARNING: THIS WILL DELETE ALL DATA.\n");
                if (Console.ReadLine() == "y")
                {
                    Console.WriteLine("\nFormatting");
                    try
                    {
                        fs.Format(@"0:\", "FAT32", true);
                        Console.WriteLine("Successfully Formatted!");
                        WaitSeconds(1);
                        Console.WriteLine("Adding fs.cfg file");
                        File.Create(@"0:\SYS\fs.cfg");
                        FileStream writeStream = File.OpenWrite(@"0:\SYS\fs.cfg");
                        Console.WriteLine("Added");
                        WaitSeconds(1);

                        byte[] toWrite = Encoding.ASCII.GetBytes("true");
                        writeStream.Write(toWrite, 0, toWrite.Length);
                        writeStream.Close();
                    }
                    catch
                    {
                        deathScreen("0x0100 Error formatting and initalizing drive!");
                    }
                }
                else
                {
                    Console.WriteLine("\nThe filesystem is being disabled as it was not formatted with CobaltOS.");
                    enableFs = false;
                    WaitSeconds(2);
                }
            }

            printLogoConsole();

            Network.NetworkInit.Init();
            WaitSeconds(1);
            Network.NetworkInit.Enable();
            Network.NetworkInterfaces.Init();
            WaitSeconds(3);

            printLogoConsole();
            Console.WriteLine("CPU: " + cpuString);
            Console.WriteLine("RAM: " + Memory.getTotalRAM() + " MB");
            Console.WriteLine("Real Hardware: " + Hardware.onRealHardware());

            if (enableFs)
            {
                Console.WriteLine("Filesystem: " + fs.GetFileSystemType("0:/") + ", " + fs.GetTotalSize(@"0:\") / 1048576 + " MB");
            }

            //initGUI();
        }

        private static void printLogoConsole()
        {
            Console.Clear();
            Console.WriteLine("    ##### ##### ####  ##### #  #######  ##### #####    ");
            Console.WriteLine("   #     #   # #  #  #   # #     #       #   # #       ");
            Console.WriteLine("  #     #   # ####  ##### #     #         #   # #####  ");
            Console.WriteLine(" #     #   # #   # #   # #     #           #   #     # ");
            Console.WriteLine("##### ##### ##### #   # ##### #             ##### #####");
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
            graphicsMode = true;
            DisplayDriver.init(!Hardware.onRealHardware());
            DisplayDriver.initScreen();
        }

        private void processConsole(String input)
        {
            if (input.StartsWith("ping "))
            {
                Network.Ping.c_Ping(input.Split(" ")[1]);
                return;
            }
            else if (input.StartsWith("ipconfig"))
            {
                c_IPConfig(input);
            }
            else if (input == "gui")
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
                Console.WriteLine("    miv: Activates the MIV text editor.");
                return;
            }
            else if (input == "miv")
            {
                MIV.StartMIV();
            }
            else if (input == "fs")
            {
                if (enableFs)
                {
                    fsMode = true;
                }
                else
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

        public static void deathScreen(String error)
        {
            DisplayDriver.exitGUI();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Clear();
            int sec = 20;
            while (sec > 0)
            {
                Console.WriteLine("Your system was disabled due to an internal error. Please see the error message below and contact us on GitHub if you don't know what happened.");
                Console.WriteLine();
                Console.WriteLine(error);
                Console.WriteLine();
                Console.WriteLine("Your system will automatically reboot in " + sec + " seconds, or press any key to restart now.");
                sec--;
                WaitSeconds(1);
                if (System.Console.KeyAvailable == true)
                {
                    shutdown(true);
                }
                Console.Clear();
            }
            shutdown(true);
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
                        Console.WriteLine(" > " + directoryEntry.mName);
                    }
                }
            }
            else if (input.Split(" ")[0] == "cd")
            {
                String[] inputSplit = input.Split(" ");
                if (inputSplit.Length > 1)
                {
                    String[] pieces = input.Split(" ");
                    if (Directory.Exists(pieces[1]))
                    {
                        cd = pieces[1];
                    }
                    else Console.WriteLine("Directory does not exist!");
                }
                else
                {
                    Console.WriteLine("Invalid syntax! cd [dir] ");
                }
            }
            else if (input.Split(" ")[0] == "format")
            {
                Console.WriteLine("Formatting...");
                fs.Format(@"0:\", "FAT32", true);
                Console.WriteLine("Successfully Formatted!");
                WaitSeconds(1);
                Console.WriteLine("Adding fs.cfg file");
                File.Create(@"0:\SYS\fs.cfg");
                FileStream writeStream = File.OpenWrite(@"0:\SYS\fs.cfg");
                Console.WriteLine("Added");
                WaitSeconds(1);

                byte[] toWrite = Encoding.ASCII.GetBytes("true");
                writeStream.Write(toWrite, 0, toWrite.Length);
                writeStream.Close();
                Console.WriteLine("Regenerating network config files...");
                Network.NetworkInit.Init();
                Network.NetworkInit.Enable();
                Network.NetworkInterfaces.Init();
                Console.WriteLine("Done!");
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
                        Console.Write((char)b);
                    }
                    Console.Write("\n");
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

        private static String getCPU(Boolean shortened)
        {
            String returnString = Cosmos.Core.CPU.GetCPUBrandString();
            if (shortened)
            {
                return returnString.Substring(0, (returnString.Length > 22 ? 22 : returnString.Length)) + (returnString.Length > 22 ? "..." : "").Replace(" ", "");
            }
            else
            {
                return returnString;
            }
        }

        public static void printToConsole(String print)
        {
            Console.WriteLine(print);
        }


        public static void c_IPConfig(string cmd)
        {
            string[] args = cmd.Split(' ');

            if (args.Length == 1)
            {
                Console.WriteLine("IP Config");
                return;
            }

            if (args[1] == "/release")
            {
                Network.DHCP.Core.SendReleasePacket();
            } 
            else if (args[1] == "/interfaces")
            {
                Utilities.Settings settings = new Utilities.Settings(@"0:\netinterface.conf");
                int i = 0;
                foreach (String s in NetworkInterfaces.CustomName)
                {
                    Console.WriteLine(s + " " + settings.Get(s) + " " + NetworkInterfaces.PCIName[i]);
                    i++;
                }
            }
            else if (args[1] == "/interface")
            {
                Console.WriteLine(NetworkInterfaces.Interface(args[2]));
            }
            else if (args[1] == "/set")
            {
                if (args.Length <= 3)
                {
                    Console.WriteLine("Usage : " + args[0] + " /set {interface} {IPv4} {Subnet} -g {Gateway} -d {PrimaryDNS}");
                    //ipconfig /set PCNETII 192.168.1.32 255.255.255.0 -g 192.168.1.254 -d 8.8.8.8
                }
                else
                {
                    if (NetworkInterfaces.Interface(args[2]) != "null")
                    {
                        Utilities.Settings settings = new Utilities.Settings(@"0:\" + NetworkInterfaces.Interface(args[2]) + ".conf");
                        NetworkStack.RemoveAllConfigIP();
                        ApplyIP(args, settings);
                        settings.Push();
                        NetworkInit.Enable();
                    }
                    else
                    {
                        Console.WriteLine("This interface doesn't exists.");
                    }
                }
            }
            else if (args[1] == "/renew")
            {
                Network.DHCP.Core.SendDiscoverPacket();
            }
            else
            {
                Console.WriteLine("IP Config");
            }
        }
        private static void ApplyIP(string[] args, Utilities.Settings settings)
        {
            int args_count = args.Length;
            switch (args_count)
            {
                default:
                    Console.WriteLine("Usage : " + args[0] + " /set {interface} {IPv4} {Subnet} -g {Gateway} -d {PrimaryDNS}");
                    break;
                case 5:
                    if (Utilities.Misc.IsIpv4Address(args[3]) && Utilities.Misc.IsIpv4Address(args[4]))
                    {
                        settings.Edit("ipaddress", args[3]);
                        settings.Edit("subnet", args[4]);
                        settings.Edit("gateway", "0.0.0.0");
                        settings.Edit("dns01", "0.0.0.0");
                    }
                    else
                    {
                        //notcorrectaddress
                    }
                    break;
                case 7:
                    if (Utilities.Misc.IsIpv4Address(args[3]) && Utilities.Misc.IsIpv4Address(args[4]) && Utilities.Misc.IsIpv4Address(args[6]))
                    {
                        settings.Edit("ipaddress", args[3]);
                        settings.Edit("subnet", args[4]);
                        if (args[5] == "-g")
                        {
                            settings.Edit("gateway", args[6]);
                            settings.Edit("dns01", "0.0.0.0");
                        }
                        else if (args[5] == "-d")
                        {
                            settings.Edit("dns01", args[6]);
                            settings.Edit("gateway", "0.0.0.0");
                        }
                        else
                        {
                            Console.WriteLine("Usage : " + args[0] + " /set {interface} {IPv4} {Subnet} -g {Gateway} -d {PrimaryDNS}");
                            settings.Edit("gateway", "0.0.0.0");
                            settings.Edit("dns01", "0.0.0.0");
                        }
                    }
                    else
                    {
                        //notcorrectaddress
                    }
                    break;
                case 9:
                    if (Utilities.Misc.IsIpv4Address(args[3]) && Utilities.Misc.IsIpv4Address(args[4]) && Utilities.Misc.IsIpv4Address(args[6]) && Utilities.Misc.IsIpv4Address(args[8]))
                    {
                        settings.Edit("ipaddress", args[3]);
                        settings.Edit("subnet", args[4]);
                        if (args[5] == "-g")
                        {
                            settings.Edit("gateway", args[6]);
                        }
                        else if (args[5] == "-d")
                        {
                            settings.Edit("dns01", args[6]);
                        }
                        else
                        {
                            Console.WriteLine("Usage : " + args[0] + " /set {interface} {IPv4} {Subnet} -g {Gateway} -d {PrimaryDNS}");
                            settings.Edit("gateway", "0.0.0.0");
                            settings.Edit("dns01", "0.0.0.0");
                        }

                        if (args[7] == "-g")
                        {
                            settings.Edit("gateway", args[8]);
                        }
                        else if (args[7] == "-d")
                        {
                            settings.Edit("dns01", args[8]);
                        }
                        else
                        {
                            Console.WriteLine("Usage : " + args[0] + " /set {interface} {IPv4} {Subnet} -g {Gateway} -d {PrimaryDNS}");
                            settings.Edit("gateway", "0.0.0.0");
                            settings.Edit("dns01", "0.0.0.0");
                        }
                    }
                    break;

            }
        }
    }
}