using Cosmos.System.FileSystem.Listing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Sys = Cosmos.System;

namespace CobaltOS.Utilities
{
    class Filesystem
    {
        private static String lastDirFolderPath = "";
        private static List<DirectoryEntry> lastDirFolder = new List<DirectoryEntry>();

        private static String lastDirFilePath = "";
        private static List<DirectoryEntry> lastDirFile = new List<DirectoryEntry>();

        public static void Init()
        {
            Kernel.printLogoConsole();
            Console.Write("<FS> Detected Drives: ");
            Console.WriteLine(DriveInfo.GetDrives().Length);
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                Console.WriteLine("<FS> - " + d.Name + " (" + d.GetType() + ") " + (d.TotalSize / 1048576) + "MB");
            }

            if (!File.Exists(@"0:\fs.cfg"))
            {
                Kernel.printLogoConsole();

                Console.WriteLine(@"The filesystem was not formatted with CobaltOS, so it cannot be used.");
                Console.WriteLine(@"Would you like to format it? (y/n)");
                Console.WriteLine("WARNING: THIS WILL DELETE ALL DATA.\n");
                if (Console.ReadLine().ToLower() == "y" || Console.ReadLine().ToLower() == "yes")
                {
                    Console.WriteLine("\nFormatting...");
                    try
                    {
                        Kernel.fs.Format(@"0:\", "FAT32", true);
                        writeFile(@"0:\fs.cfg", false, "true");
                        Console.WriteLine("Done!");
                    }
                    catch
                    {
                        Kernel.WaitSeconds(5);
                        Kernel.deathScreen("0x0100 Error formatting and initalizing drive!");
                    }
                }
                else
                {
                    Console.WriteLine("\nThe filesystem is being disabled as it was not formatted with CobaltOS.");
                    Kernel.enableFs = false;
                    Kernel.WaitSeconds(2);
                }
            }
        }

        public static String readFile(String path)
        {
            if (File.Exists(path))
            {
                try
                {
                    String st = "";
                    FileStream s = File.OpenRead(path);
                    byte[] a = new byte[s.Length];
                    s.Read(a, 0, a.Length);
                    foreach (Byte b in a)
                    {
                        st += (char)b;
                    }
                    return st;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static Boolean writeFile(String path, Boolean overwrite, String writing)
        {
            FileStream writeStream;
            if (File.Exists(path))
            {
                if (overwrite)
                {
                    File.Delete(path);
                    writeStream = File.Create(path);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                writeStream = File.Create(path);
            }

            try
            {
                byte[] toWrite = Encoding.ASCII.GetBytes(writing);
                writeStream.Write(toWrite, 0, toWrite.Length);
                writeStream.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean deleteFile(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public static Boolean deleteDir(String path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                } catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static List<DirectoryEntry> getDirFolders(String path)
        {
            if (path == lastDirFolderPath)
            {
                return lastDirFolder;
            }
            List<DirectoryEntry> l = new List<DirectoryEntry>();
            foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(path))
            {
                if (d.mEntryType == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                {
                    l.Add(d);
                }
            }
            lastDirFolderPath = path;
            lastDirFolder = l;
            return l;
        }

        public static List<DirectoryEntry> getDirFiles(String path)
        {
            if (path == lastDirFilePath)
            {
                return lastDirFile;
            }
            List<DirectoryEntry> l = new List<DirectoryEntry>();
            foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(path))
            {
                if (d.mEntryType == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                {
                    l.Add(d);
                }
            }
            lastDirFilePath = path;
            lastDirFile = l;
            return l;
        }

        public static void refreshDir(String path)
        {
            lastDirFilePath = "";
            lastDirFolderPath = "";
        }
    }
}
