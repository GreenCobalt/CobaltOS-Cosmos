using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Cosmos.System;
using Cosmos.System.FileSystem;
using System.IO;
using CosmosKernel1.Utils;
using CosmosKernel1.GUI;

namespace CosmosKernel1
{
    class GUIManager
    {
        /*
         * 0 = Desktop
         * 1 = Notepad
         * 2 = Settings
         * 3 = Calculator
         * 
         * 99 = Power Menu
         */
        private static int activeApp = 0;
        private static List<int> openApps = new List<int>();

        /*
         * 0 = General
         * 1 = Personalization
         */
        private static int settingsPage = 0;
        private static Boolean startMenuOpen = false;

        private static int notepadLocX = 10;
        private static int notepadLocY = 10;
        private static int notepadSizeX = 600;
        private static int notepadSizeY = 400;
        private static Boolean notepadFileMenu = false;

        private static int settingsLocX = 10;
        private static int settingsLocY = 10;
        private static int settingsSizeX = 600;
        private static int settingsSizeY = 400;

        private static int calcLocX = 10;
        private static int calcLocY = 10;
        private static int calcSizeX = 260;
        private static int calcSizeY = 330;
        private static Boolean calcAnswer;

        private static int cancelX = 100;
        private static int offX;
        private static int restartX;
        private static int Y = 200;
        private static int cancelSize;
        private static int offSize;
        private static int restartSize;
        private static int timeFormatToggleSize;
        private static int backgroundColorSize;
        private static int resolutionChangeSize;
        private static Boolean resolutionChangeMenu = false;
        private static Boolean bgColorChangeMenu = false;
        private static int typeLocX;
        private static int typeLocY;
        private static Boolean timeFormat = false;
        private static Color backgroundColor = Color.DarkBlue;
        private static List<Char> notePadChars = new List<Char>();
        private static List<Char> dirChars = new List<Char>();
        private static List<Char> calcChars = new List<Char>();
        private static List<int> notePadCharSizes = new List<int>();
        private static CosmosVFS fs;

        private static int screenW;
        private static int screenH;

        private static int taskBarHeight = 50;


        /*
         * 0 = Save
         * 1 = Open
         */
        private static int dirSelectPurpose;
        private static String dirSelectContent;
        private static Boolean dirSelectOpen;
        private static Boolean newGraphics;

        public static void init()
        {
            fs = Kernel.fs;
            newGraphics = Kernel.newGraphics;

            screenW = newGraphics ? VMDisplayDriver.screenW : CanvasDisplayDriver.screenW;
            screenH = newGraphics ? VMDisplayDriver.screenH : CanvasDisplayDriver.screenH;

            if (!File.Exists(@"0:\config.cfg"))
            {
                FileStream tempStream = File.Create(@"0:\config.cfg");
                byte[] toWrite = Encoding.ASCII.GetBytes("false,1,1");
                tempStream.Write(toWrite, 0, toWrite.Length);
                tempStream.Close();
            }
            FileStream stream = File.OpenRead(@"0:\config.cfg");
            byte[] toRead = new byte[stream.Length];
            stream.Read(toRead, 0, (int)stream.Length);

            String[] config = ListUtils.byteListToString(toRead).Split(',');
            timeFormat = Boolean.Parse(config[0]);
            backgroundColor = getColorFromInt(int.Parse(config[1]));
            int[] resolution = getResFromInt(int.Parse(config[2]));
            DisplayDriver.changeRes(resolution[0], resolution[1]);
        }

        private static Color getColorFromInt(int c)
        {
            switch (c)
            {
                case 1: return Color.DarkBlue;
                case 2: return Color.DarkRed;
                case 3: return Color.DarkGreen;
                case 4: return Color.Blue;
                case 5: return Color.Red;
                case 6: return Color.Green;
                case 7: return Color.Orange;
                case 8: return Color.Yellow;
                case 9: return Color.Purple;
                default: return Color.Black;
            }
        }
        private static int getIntFromColor(Color c)
        {
            if (c == Color.DarkBlue) return 1;
            if (c == Color.DarkRed) return 2;
            if (c == Color.DarkGreen) return 3;
            if (c == Color.Blue) return 4;
            if (c == Color.Red) return 5;
            if (c == Color.Green) return 6;
            if (c == Color.Orange) return 7;
            if (c == Color.Yellow) return 8;
            if (c == Color.Purple) return 9;
            return 0;
        }
        private static int getIntFromRes(int x)
        {
            if (x == 640) return 1;
            if (x == 800) return 2;
            if (x == 1024) return 3;
            return 0;
        }
        private static int[] getResFromInt(int i)
        {
            if (i == 1) return new int[] { 640, 480 };
            if (i == 2) return new int[] { 800, 600 };
            if (i == 3) return new int[] { 1024, 768 };
            return new int[] { 160, 120 };
        }


        public static void tick()
        {
            if (activeApp == 99)
            {
                DisplayDriver.setFullBuffer(Color.DarkGray);
            }
            else
            {
                DisplayDriver.setFullBuffer(backgroundColor);
                DisplayDriver.addFilledRectangle(0, screenH - taskBarHeight, screenW, taskBarHeight, Color.FromArgb(255, 50, 50, 50));
                DisplayDriver.addFilledRectangle(10, screenH - taskBarHeight + 10, 30, taskBarHeight - 20, Color.Red);
                DisplayDriver.addText((timeFormat ? screenW - 125 : screenW - 175), screenH - 40, Color.White, (timeFormat ? Cosmos.HAL.RTC.Hour : (Cosmos.HAL.RTC.Hour > 12 ? Cosmos.HAL.RTC.Hour - 12 : (Cosmos.HAL.RTC.Hour == 0 ? 12 : Cosmos.HAL.RTC.Hour))).ToString().PadLeft(2, '0') + ":" + Cosmos.HAL.RTC.Minute.ToString().PadLeft(2, '0') + ":" + Cosmos.HAL.RTC.Second.ToString().PadLeft(2, '0') + (timeFormat ? "" : (Cosmos.HAL.RTC.Hour > 12 ? " PM" : " AM")));
            }

            checkKeyboard();
            addShapes();
            checkMouse();

            DisplayDriver.addMouse(Convert.ToInt32(Cosmos.System.MouseManager.X), Convert.ToInt32(Cosmos.System.MouseManager.Y));
            DisplayDriver.drawScreen();
        }

        public static double round(double input)
        {
            double output = input * 100;
            output = (int)output;
            return output / 100;
        }

        private static void addShapes()
        {
            if (activeApp == 1)
            {
                DisplayDriver.addFilledRectangle(notepadLocX, notepadLocY, notepadSizeX, notepadSizeY, Color.White);

                DisplayDriver.addFilledRectangle(notepadLocX, notepadLocY, notepadSizeX, 36, Color.Gray);
                DisplayDriver.addText(notepadLocX + 5, notepadLocY + 3, Color.White, "Notepad");

                DisplayDriver.addFilledRectangle(notepadLocX, notepadLocY + 36, notepadSizeX, 36, Color.LightGray);
                DisplayDriver.addFilledRectangle(notepadLocX, notepadLocY + 36, 80, 36, (notepadFileMenu ? Color.DarkGray : Color.LightGray));
                DisplayDriver.addText(notepadLocX + 5, notepadLocY + 39, Color.Black, "File");

                notePadCharSizes.Clear();

                typeLocX = notepadLocX + 10;
                typeLocY = notepadLocY + 76;

                int newLines = 0;

                for (int i = 0; i < notePadChars.Count; i++)
                {
                    int size = DisplayDriver.typeChar(typeLocX, typeLocY, Color.Black, notePadChars[i]);
                    typeLocX = typeLocX + size;

                    notePadCharSizes.Add(size);
                    int totalSize = 0;
                    foreach (int charSize in notePadCharSizes)
                    {
                        totalSize = totalSize + charSize;
                        if ((totalSize + 40) - (newLines * (notepadSizeX - 20)) > notepadSizeX)
                        {
                            typeLocY = typeLocY + 35;
                            typeLocX = notepadLocX + 10;
                            newLines = newLines + 1;
                        }
                    }
                }

                DisplayDriver.addFilledRectangle(notepadLocX + 575, notepadLocY + 5, 20, 20, Color.Red);
                DisplayDriver.addRectangle(typeLocX, typeLocY, typeLocX + 2, typeLocY + 24, Color.Black);

                if (notepadFileMenu)
                {
                    DisplayDriver.addFilledRectangle(notepadLocX, notepadLocY + 72, 80, 100, Color.Gray);
                    DisplayDriver.addText(notepadLocX + 5, notepadLocY + 77, Color.Black, "Save");
                    DisplayDriver.addText(notepadLocX + 5, notepadLocY + 117, Color.Black, "Open");
                }

                if (dirSelectOpen)
                {
                    DisplayDriver.addFilledRectangle(notepadLocX + 10, notepadLocY + 10, 500, 300, Color.DarkGray);
                    DisplayDriver.addText(notepadLocX + 40, notepadLocY + 20, Color.White, (dirSelectPurpose == 0 ? "Save" : (dirSelectPurpose == 1 ? "Open" : "")));
                    DisplayDriver.addFilledRectangle(notepadLocX + 40, notepadLocY + 50, 460, 50, Color.White);
                    int dirSize = DisplayDriver.addText(notepadLocX + 50, notepadLocY + 60, Color.Black, new string(dirChars.ToArray()));
                    DisplayDriver.addRectangle(dirSize + 4, notepadLocY + 60, dirSize + 6, notepadLocY + 90, Color.Black);
                    DisplayDriver.addFilledRectangle(notepadLocX + 40, notepadLocY + 110, 100, 50, Color.Gray);
                    DisplayDriver.addFilledRectangle(notepadLocX + 190, notepadLocY + 110, 100, 50, Color.Gray);
                    DisplayDriver.addText(notepadLocX + 50, notepadLocY + 120, Color.White, (dirSelectPurpose == 0 ? "Save" : (dirSelectPurpose == 1 ? "Open" : "")));
                    DisplayDriver.addText(notepadLocX + 200, notepadLocY + 120, Color.White, "Cancel");
                }
            }

            if (activeApp == 2)
            {
                DisplayDriver.addFilledRectangle(settingsLocX, settingsLocY, settingsSizeX, settingsSizeY, Color.White);
                DisplayDriver.addFilledRectangle(settingsLocX, settingsLocY, settingsSizeX, 30, Color.Gray);
                DisplayDriver.addFilledRectangle(settingsLocX + (settingsSizeX - 25), settingsLocY + 5, 20, 20, Color.Red);

                DisplayDriver.addText(settingsLocX + 10, settingsLocY + 40, Color.Black, "Settings");
                DisplayDriver.addFilledRectangle(settingsLocX + 10, settingsLocY + 75, 150, 300, Color.Gray);

                DisplayDriver.addFilledRectangle(settingsLocX + 20, settingsLocY + 85, 130, 40, (settingsPage == 0 ? Color.LightGray : Color.DarkGray));
                DisplayDriver.addText(settingsLocX + 25, settingsLocY + 90, Color.White, "General");

                DisplayDriver.addFilledRectangle(settingsLocX + 20, settingsLocY + 125, 130, 40, (settingsPage == 1 ? Color.LightGray : Color.DarkGray));
                DisplayDriver.addText(settingsLocX + 25, settingsLocY + 130, Color.White, "Colors");

                DisplayDriver.addFilledRectangle(settingsLocX + 20, settingsLocY + 325, 130, 40, (settingsPage == 2 ? Color.LightGray : Color.DarkGray));
                DisplayDriver.addText(settingsLocX + 25, settingsLocY + 330, Color.White, "Info");

                if (settingsPage == 0)
                {
                    DisplayDriver.addText(settingsLocX + 180, settingsLocY + 120, Color.Black, "Time Format:");
                    DisplayDriver.addText(settingsLocX + 350, settingsLocY + 120, Color.Black, (timeFormat ? "24 Hour" : "12 Hour"));
                    timeFormatToggleSize = DisplayDriver.addText(settingsLocX + 480, settingsLocY + 120, Color.Black, "Toggle") + 20;
                    DisplayDriver.addRectangle(settingsLocX + 460, settingsLocY + 100, timeFormatToggleSize, settingsLocY + 170, Color.Black);

                    DisplayDriver.addText(settingsLocX + 180, settingsLocY + 200, Color.Black, "Screen Resolution:   " + screenW + "x" + screenH);
                    resolutionChangeSize = DisplayDriver.addText(settingsLocX + 480, settingsLocY + 260, Color.Black, "Change") + 20;
                    DisplayDriver.addRectangle(settingsLocX + 460, settingsLocY + 240, resolutionChangeSize, settingsLocY + 310, Color.Black);
                }
                else if (settingsPage == 1)
                {
                    DisplayDriver.addText(settingsLocX + 180, settingsLocY + 120, Color.Black, "Background Color:");
                    DisplayDriver.addFilledRectangle(settingsLocX + 410, settingsLocY + 120, 30, 30, backgroundColor);
                    backgroundColorSize = DisplayDriver.addText(settingsLocX + 480, settingsLocY + 120, Color.Black, "Change") + 20;
                    DisplayDriver.addRectangle(settingsLocX + 460, settingsLocY + 100, backgroundColorSize, settingsLocY + 170, Color.Black);
                }
                else if (settingsPage == 2)
                {
                    DisplayDriver.addText(settingsLocX + 180, settingsLocY + 120, Color.Black, "System Information:");
                    DisplayDriver.addText(settingsLocX + 180, settingsLocY + 150, Color.Black, " - CobaltOS Version: " + Kernel.osVersion);
                    DisplayDriver.addText(settingsLocX + 180, settingsLocY + 180, Color.Black, " - CPU: " + Kernel.cpuString);
                    DisplayDriver.addText(settingsLocX + 180, settingsLocY + 210, Color.Black, " - RAM: " + (Cosmos.Core.CPU.GetAmountOfRAM() < 1000 ? Cosmos.Core.CPU.GetAmountOfRAM() + " MB" : round(Cosmos.Core.CPU.GetAmountOfRAM() / 1000.00) + " GB"));

                }

                if (bgColorChangeMenu)
                {
                    DisplayDriver.addFilledRectangle(settingsLocX + 480, settingsLocY + 120, 100, 100, Color.LightGray);
                    DisplayDriver.addFilledRectangle(settingsLocX + 490, settingsLocY + 130, 20, 20, Color.DarkBlue);
                    DisplayDriver.addFilledRectangle(settingsLocX + 520, settingsLocY + 130, 20, 20, Color.DarkRed);
                    DisplayDriver.addFilledRectangle(settingsLocX + 550, settingsLocY + 130, 20, 20, Color.DarkGreen);
                    DisplayDriver.addFilledRectangle(settingsLocX + 490, settingsLocY + 160, 20, 20, Color.Blue);
                    DisplayDriver.addFilledRectangle(settingsLocX + 520, settingsLocY + 160, 20, 20, Color.Red);
                    DisplayDriver.addFilledRectangle(settingsLocX + 550, settingsLocY + 160, 20, 20, Color.Green);
                    DisplayDriver.addFilledRectangle(settingsLocX + 490, settingsLocY + 190, 20, 20, Color.Orange);
                    DisplayDriver.addFilledRectangle(settingsLocX + 520, settingsLocY + 190, 20, 20, Color.Yellow);
                    DisplayDriver.addFilledRectangle(settingsLocX + 550, settingsLocY + 190, 20, 20, Color.Purple);
                }

                if (resolutionChangeMenu)
                {
                    DisplayDriver.addFilledRectangle(settingsLocX + 300, settingsLocY + 290, 200, 100, Color.LightGray);
                    DisplayDriver.addText(settingsLocX + 310, settingsLocY + 300, Color.Black, "640x480");
                    DisplayDriver.addText(settingsLocX + 310, settingsLocY + 330, Color.Black, "800x600");
                }
            }

            if (activeApp == 3)
            {
                int spacingX = 16;
                int spacingY = 13;

                DisplayDriver.addFilledRectangle(calcLocX, calcLocY, calcSizeX, calcSizeY, Color.FromArgb(255, 20, 20, 20));
                DisplayDriver.addFilledRectangle(calcLocX, calcLocY, calcSizeX, 30, Color.Gray);
                DisplayDriver.addFilledRectangle(calcLocX + (calcSizeX - 25), calcLocY + 5, 20, 20, Color.Red);

                DisplayDriver.addFilledRectangle(calcLocX + 10, calcLocY + 40, 240, 40, Color.White);
                DisplayDriver.addText(calcLocX + 15, calcLocY + 45, Color.Black, new string(calcChars.ToArray()));

                DisplayDriver.addFilledRectangle(calcLocX + 10, calcLocY + 90, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 10 + spacingX, calcLocY + 90 + spacingY, Color.Black, "7");
                DisplayDriver.addFilledRectangle(calcLocX + 70, calcLocY + 90, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 70 + spacingX, calcLocY + 90 + spacingY, Color.Black, "8");
                DisplayDriver.addFilledRectangle(calcLocX + 130, calcLocY + 90, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 130 + spacingX, calcLocY + 90 + spacingY, Color.Black, "9");
                DisplayDriver.addFilledRectangle(calcLocX + 200, calcLocY + 90, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 200 + spacingX, calcLocY + 90 + spacingY, Color.Black, "/");
                DisplayDriver.addFilledRectangle(calcLocX + 10, calcLocY + 150, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 10 + spacingX, calcLocY + 150 + spacingY, Color.Black, "4");
                DisplayDriver.addFilledRectangle(calcLocX + 70, calcLocY + 150, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 70 + spacingX, calcLocY + 150 + spacingY, Color.Black, "5");
                DisplayDriver.addFilledRectangle(calcLocX + 130, calcLocY + 150, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 130 + spacingX, calcLocY + 150 + spacingY, Color.Black, "6");
                DisplayDriver.addFilledRectangle(calcLocX + 200, calcLocY + 150, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 200 + spacingX, calcLocY + 150 + spacingY - 8, Color.Black, "x");
                DisplayDriver.addFilledRectangle(calcLocX + 10, calcLocY + 210, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 10 + spacingX, calcLocY + 210 + spacingY, Color.Black, "1");
                DisplayDriver.addFilledRectangle(calcLocX + 70, calcLocY + 210, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 70 + spacingX, calcLocY + 210 + spacingY, Color.Black, "2");
                DisplayDriver.addFilledRectangle(calcLocX + 130, calcLocY + 210, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 130 + spacingX, calcLocY + 210 + spacingY, Color.Black, "3");
                DisplayDriver.addFilledRectangle(calcLocX + 200, calcLocY + 210, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 200 + spacingX, calcLocY + 210 + spacingY, Color.Black, "-");
                DisplayDriver.addFilledRectangle(calcLocX + 10, calcLocY + 270, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 10 + spacingX, calcLocY + 270 + spacingY, Color.Black, "0");
                DisplayDriver.addFilledRectangle(calcLocX + 70, calcLocY + 270, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 70 + spacingX, calcLocY + 270 + spacingY, Color.Black, ".");
                DisplayDriver.addFilledRectangle(calcLocX + 130, calcLocY + 270, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 130 + spacingX, calcLocY + 270 + spacingY, Color.Black, "=");
                DisplayDriver.addFilledRectangle(calcLocX + 200, calcLocY + 270, 50, 50, Color.LightGray); DisplayDriver.addText(calcLocX + 200 + spacingX, calcLocY + 270 + spacingY, Color.Black, "+");
            }

            if (activeApp == 99)
            {
                cancelSize = DisplayDriver.addText(cancelX, Y + 75, Color.Blue, "Cancel");
                DisplayDriver.addFilledRectangle(((cancelX + cancelSize) / 2) - 25, Y, 50, 50, Color.Blue);
                DisplayDriver.addRectangle(cancelX - 10, Y - 10, cancelSize + 10, Y + 125, Color.Blue);

                offX = cancelSize + 50;
                offSize = DisplayDriver.addText(offX, Y + 75, Color.Red, "Power Off");
                DisplayDriver.addFilledRectangle(((offX + offSize) / 2) - 25, Y, 50, 50, Color.Red);
                DisplayDriver.addRectangle(offX - 10, Y - 10, offSize + 10, Y + 125, Color.Red);

                restartX = offSize + 50;
                restartSize = DisplayDriver.addText(restartX, Y + 75, Color.Orange, "Restart");
                DisplayDriver.addFilledRectangle(((restartX + restartSize) / 2) - 25, Y, 50, 50, Color.Orange);
                DisplayDriver.addRectangle(restartX - 10, Y - 10, restartSize + 10, Y + 125, Color.Orange);
            }

            if (startMenuOpen)
            {
                DisplayDriver.addFilledRectangle(10, screenH - taskBarHeight - 300, 300, 300, Color.Gray);

                DisplayDriver.addFilledRectangle(20, screenH - taskBarHeight - 290, 20, 20, Color.LightBlue);
                DisplayDriver.addText(50, screenH - taskBarHeight - 290, Color.White, "Notepad");

                DisplayDriver.addFilledRectangle(20, screenH - taskBarHeight - 255, 20, 20, Color.SandyBrown);
                DisplayDriver.addText(50, screenH - taskBarHeight - 255, Color.White, "Calculator");

                DisplayDriver.addFilledRectangle(20, screenH - taskBarHeight - 80, 20, 20, Color.DarkGray);
                DisplayDriver.addText(50, screenH - taskBarHeight - 80, Color.White, "Settings");

                DisplayDriver.addFilledRectangle(20, screenH - taskBarHeight - 45, 20, 20, Color.Red);
                DisplayDriver.addText(50, screenH - taskBarHeight - 45, Color.White, "Power");
            }
        }

        public static void updateScreenSize(int x, int y)
        {
            screenW = x;
            screenH = y;
        }

        private static void checkMouse()
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                uint x = Cosmos.System.MouseManager.X;
                uint y = Cosmos.System.MouseManager.Y;

                if (x > 10 && x < 30 && y > 10 && y < 30)
                {
                    DisplayDriver.changeRes(1920, 1080);
                }

                if ((x > 10 && x < 40) && (y > screenH - (taskBarHeight - 10) && y < screenH - 10))
                {
                    startMenuOpen = !startMenuOpen;
                }

                //---------Start Menu---------------------
                if (startMenuOpen && (x > 20 && x < 280) && (y > screenH - taskBarHeight - 290 && y < screenH - taskBarHeight - 265))
                {
                    startMenuOpen = false;
                    typeLocX = notepadLocX + 10;
                    typeLocY = notepadLocY + 76;
                    activeApp = 1;
                    return;
                }
                if (startMenuOpen && (x > 20 && x < 280) && (y > screenH - taskBarHeight - 265 && y < screenH - taskBarHeight - 230))
                {
                    startMenuOpen = false;
                    activeApp = 3;
                    calcChars.Clear();
                    return;
                }
                if (startMenuOpen && (x > 20 && x < 280) && (y > screenH - taskBarHeight - 70 && y < screenH - taskBarHeight - 40))
                {
                    startMenuOpen = false;
                    activeApp = 2;
                    return;
                }
                if (startMenuOpen && (x > 20 && x < 280) && (y > screenH - taskBarHeight - 35 && y < screenH - taskBarHeight - 10))
                {
                    startMenuOpen = false;
                    activeApp = 99;
                    return;
                }
                //---------------------------------------


                if (activeApp == 1)
                {
                    if ((x > settingsLocX + 575 && x < settingsLocX + 595) && (y > settingsLocY + 5 && y < settingsLocY + 25))
                    {
                        activeApp = 0;
                        dirSelectOpen = false;
                    }

                    if ((x > notepadLocX + 2 && x < notepadLocX + 80) && (y > notepadLocY + 37 && y < notepadLocY + 77) && (!dirSelectOpen))
                    {
                        notepadFileMenu = !notepadFileMenu;
                    }

                    if ((x > notepadLocX + 5 && x < notepadLocX + 105) && (y > notepadLocY + 77 && y < notepadLocY + 117) && notepadFileMenu)
                    {
                        notepadFileMenu = !notepadFileMenu;
                        dirSelectOpen = true;
                        dirSelectPurpose = 0;
                        dirSelectContent = new string(notePadChars.ToArray());
                        dirChars.Clear();
                        dirChars.Add('0');
                        dirChars.Add(':');
                        dirChars.Add('\\');
                    }
                    if ((x > notepadLocX + 5 && x < notepadLocX + 105) && (y > notepadLocY + 117 && y < notepadLocY + 157) && notepadFileMenu)
                    {
                        notepadFileMenu = !notepadFileMenu;
                        dirSelectOpen = true;
                        dirSelectPurpose = 1;
                        dirChars.Clear();
                        dirChars.Add('0');
                        dirChars.Add(':');
                        dirChars.Add('\\');
                    }

                    if (dirSelectOpen && (x > notepadLocX + 40 && x < notepadLocX + 140) && (y > notepadLocY + 110 && y < notepadLocY + 160))
                    {
                        if (dirSelectPurpose == 0)
                        {
                            FileStream f = File.Create(new string(dirChars.ToArray()));
                            byte[] toWrite = Encoding.ASCII.GetBytes(dirSelectContent);
                            f.Write(toWrite, 0, toWrite.Length);

                            dirSelectOpen = false;
                        }
                        else if (dirSelectPurpose == 1)
                        {
                            if (File.Exists(new string(dirChars.ToArray())))
                            {
                                FileStream f = File.OpenRead(new string(dirChars.ToArray()));
                                byte[] toRead = new byte[f.Length];
                                f.Read(toRead, 0, (int)f.Length);
                                notePadChars.Clear();
                                for (int i = 0; i < toRead.Length; i++)
                                {
                                    notePadChars.Add(Encoding.ASCII.GetString(toRead)[i]);
                                }
                            }
                            else
                            {
                                String s = "File not found!";
                                notePadChars.Clear();
                                for (int i = 0; i < s.Length; i++)
                                {
                                    notePadChars.Add(s[i]);
                                }
                                Kernel.WaitSeconds(1);
                                notePadChars.Clear();
                            }
                            dirSelectOpen = false;
                        }
                    }
                    if (dirSelectOpen && (x > notepadLocX + 190 && x < notepadLocX + 290) && (y > notepadLocY + 110 && y < notepadLocY + 160))
                    {
                        dirSelectOpen = false;
                        dirSelectPurpose = 0;
                    }
                }

                if (activeApp == 2)
                {
                    if ((x > settingsLocX + 575 && x < settingsLocX + 595) && (y > settingsLocY + 5 && y < settingsLocY + 25))
                    {
                        activeApp = 0;
                        bgColorChangeMenu = false;
                    }
                    else if (settingsPage == 0 && (x > settingsLocX + 460 && x < resolutionChangeSize) && (y > settingsLocY + 240 && y < settingsLocY + 310))
                    {
                        resolutionChangeMenu = true;
                    }
                    else if (settingsPage == 0 && (x > settingsLocX + 460 && x < timeFormatToggleSize) && (y > settingsLocY + 100 && y < settingsLocY + 170))
                    {
                        timeFormat = !timeFormat;

                        FileStream readStream = File.OpenRead(@"0:\config.cfg");
                        byte[] toRead = new byte[readStream.Length];
                        readStream.Read(toRead, 0, (int)readStream.Length);
                        String s = timeFormat.ToString() + "," + ListUtils.byteListToString(toRead).Split(',')[1] + "," + ListUtils.byteListToString(toRead).Split(',')[2];
                        readStream.Close();

                        File.Delete(@"0:\config.cfg");
                        FileStream writeStream = File.Create(@"0:\config.cfg");
                        byte[] toWrite = Encoding.ASCII.GetBytes(s);
                        writeStream.Write(toWrite, 0, toWrite.Length);
                        writeStream.Close();
                    }
                    else if (settingsPage == 1 && (x > settingsLocX + 460 && x < backgroundColorSize) && (y > settingsLocY + 100 && y < settingsLocY + 170) && !bgColorChangeMenu)
                    {
                        bgColorChangeMenu = true;
                    }
                    else if ((x > settingsLocX + 20 && x < settingsLocX + 150) && (y > settingsLocY + 85 && y < settingsLocY + 125))
                    {
                        settingsPage = 0;
                        bgColorChangeMenu = false;
                    }
                    else if ((x > settingsLocX + 20 && x < settingsLocX + 150) && (y > settingsLocY + 125 && y < settingsLocY + 165))
                    {
                        settingsPage = 1;
                    }
                    else if ((x > settingsLocX + 20 && x < settingsLocX + 150) && (y > settingsLocY + 325 && y < settingsLocY + 365))
                    {
                        settingsPage = 2;
                    }
                    else if (bgColorChangeMenu)
                    {
                        if ((x > settingsLocX + 490 && x < settingsLocX + 510) && (y > settingsLocY + 130 && y < settingsLocY + 150))
                        {
                            backgroundColor = Color.DarkBlue;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 520 && x < settingsLocX + 540) && (y > settingsLocY + 130 && y < settingsLocY + 150))
                        {
                            backgroundColor = Color.DarkRed;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 550 && x < settingsLocX + 570) && (y > settingsLocY + 130 && y < settingsLocY + 150))
                        {
                            backgroundColor = Color.DarkGreen;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 490 && x < settingsLocX + 510) && (y > settingsLocY + 160 && y < settingsLocY + 180))
                        {
                            backgroundColor = Color.Blue;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 520 && x < settingsLocX + 540) && (y > settingsLocY + 160 && y < settingsLocY + 180))
                        {
                            backgroundColor = Color.Red;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 550 && x < settingsLocX + 570) && (y > settingsLocY + 160 && y < settingsLocY + 180))
                        {
                            backgroundColor = Color.Green;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 490 && x < settingsLocX + 510) && (y > settingsLocY + 190 && y < settingsLocY + 210))
                        {
                            backgroundColor = Color.Orange;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 520 && x < settingsLocX + 540) && (y > settingsLocY + 190 && y < settingsLocY + 210))
                        {
                            backgroundColor = Color.Yellow;
                            bgColorChangeMenu = false;
                        }
                        else if ((x > settingsLocX + 550 && x < settingsLocX + 570) && (y > settingsLocY + 190 && y < settingsLocY + 210))
                        {
                            backgroundColor = Color.Purple;
                            bgColorChangeMenu = false;
                        }

                        FileStream readStream = File.OpenRead(@"0:\config.cfg");
                        byte[] toRead = new byte[readStream.Length];
                        readStream.Read(toRead, 0, (int)readStream.Length);
                        String s = ListUtils.byteListToString(toRead).Split(',')[0] + "," + getIntFromColor(backgroundColor) + "," + ListUtils.byteListToString(toRead).Split(',')[2];
                        readStream.Close();

                        File.Delete(@"0:\config.cfg");
                        FileStream writeStream = File.Create(@"0:\config.cfg");
                        byte[] toWrite = Encoding.ASCII.GetBytes(s);
                        writeStream.Write(toWrite, 0, toWrite.Length);
                        writeStream.Close();
                    }
                    else if (resolutionChangeMenu)
                    {
                        if (x > 310 && x < 500 && y > 330 && y < 360) DisplayDriver.changeRes(640, 480);
                        else if (x > 310 && x < 500 && y > 360 && y < 390) DisplayDriver.changeRes(800, 600);
                        else return;

                        resolutionChangeMenu = false;

                        FileStream readStream = File.OpenRead(@"0:\config.cfg");
                        byte[] toRead = new byte[readStream.Length];
                        readStream.Read(toRead, 0, (int)readStream.Length);
                        String s = ListUtils.byteListToString(toRead).Split(',')[0] + "," + ListUtils.byteListToString(toRead).Split(',')[1] + "," + getIntFromRes(screenW);
                        readStream.Close();

                        File.Delete(@"0:\config.cfg");
                        FileStream writeStream = File.Create(@"0:\config.cfg");
                        byte[] toWrite = Encoding.ASCII.GetBytes(s);
                        writeStream.Write(toWrite, 0, toWrite.Length);
                        writeStream.Close();
                    }
                }

                if (activeApp == 3)
                {
                    if ((x > calcLocX + calcSizeX - 25 && x < calcLocX + calcSizeX - 5) && (y > calcLocY + 5 && y < calcLocY + 25))
                    {
                        activeApp = 0;
                    }
                    else if ((x > calcLocX + 10 && x < calcLocX + 60) && (y > calcLocY + 90 && y < calcLocY + 140))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('7');
                    }
                    else if ((x > calcLocX + 70 && x < calcLocX + 120) && (y > calcLocY + 90 && y < calcLocY + 140))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('8');
                    }
                    else if ((x > calcLocX + 130 && x < calcLocX + 180) && (y > calcLocY + 90 && y < calcLocY + 140))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('9');
                    }
                    else if ((x > calcLocX + 200 && x < calcLocX + 250) && (y > calcLocY + 90 && y < calcLocY + 140))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('/');
                    }
                    else if ((x > calcLocX + 10 && x < calcLocX + 60) && (y > calcLocY + 150 && y < calcLocY + 200))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('4');
                    }
                    else if ((x > calcLocX + 70 && x < calcLocX + 120) && (y > calcLocY + 150 && y < calcLocY + 200))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('5');
                    }
                    else if ((x > calcLocX + 130 && x < calcLocX + 180) && (y > calcLocY + 150 && y < calcLocY + 200))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('6');
                    }
                    else if ((x > calcLocX + 200 && x < calcLocX + 250) && (y > calcLocY + 150 && y < calcLocY + 200))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('x');
                    }
                    else if ((x > calcLocX + 10 && x < calcLocX + 60) && (y > calcLocY + 210 && y < calcLocY + 260))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('1');
                    }
                    else if ((x > calcLocX + 70 && x < calcLocX + 120) && (y > calcLocY + 210 && y < calcLocY + 260))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('2');
                    }
                    else if ((x > calcLocX + 130 && x < calcLocX + 180) && (y > calcLocY + 210 && y < calcLocY + 260))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('3');
                    }
                    else if ((x > calcLocX + 200 && x < calcLocX + 250) && (y > calcLocY + 210 && y < calcLocY + 260))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('-');
                    }
                    else if ((x > calcLocX + 10 && x < calcLocX + 60) && (y > calcLocY + 270 && y < calcLocY + 320))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('0');
                    }
                    else if ((x > calcLocX + 70 && x < calcLocX + 120) && (y > calcLocY + 270 && y < calcLocY + 320))
                    {
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                        calcChars.Add('.');
                    }
                    else if ((x > calcLocX + 130 && x < calcLocX + 180) && (y > calcLocY + 270 && y < calcLocY + 320))
                    {
                        calcAnswer = Calculator.executeCalc(calcChars);
                    }
                    else if ((x > calcLocX + 200 && x < calcLocX + 250) && (y > calcLocY + 270 && y < calcLocY + 320))
                    {
                        calcChars.Add('+');
                        if (calcAnswer)
                        {
                            calcAnswer = false;
                            calcChars.Clear();
                        }
                    }
                }

                if (activeApp == 99)
                {
                    if ((x > cancelX - 10 && x < cancelSize + 10) && (y > Y - 10 && y < Y + 125))
                    {
                        activeApp = 0;
                    }
                    if ((x > offX - 10 && x < offSize + 10) && (y > Y - 10 && y < Y + 125))
                    {
                        Kernel.shutdown(false);
                    }
                    if ((x > restartX - 10 && x < restartSize + 10) && (y > Y - 10 && y < Y + 125))
                    {
                        Kernel.shutdown(true);
                    }
                }

                while (MouseManager.MouseState == MouseState.Left)
                {
                    ;
                }
            }
        }


        private static void checkKeyboard()
        {
            if (System.Console.KeyAvailable == true)
            {

                ConsoleKeyInfo key = System.Console.ReadKey(true);
                Boolean caps = false;
                Char currentChar;

                if (KeyboardManager.ShiftPressed || KeyboardManager.CapsLock)
                {
                    caps = true;
                    if (KeyboardManager.ShiftPressed && KeyboardManager.CapsLock)
                    {
                        caps = false;
                    }
                }
                if (key.Key == ConsoleKey.A)
                {
                    currentChar = (caps ? 'A' : 'a');
                }
                else if (key.Key == ConsoleKey.B)
                {
                    currentChar = (caps ? 'B' : 'b');
                }
                else if (key.Key == ConsoleKey.C)
                {
                    currentChar = (caps ? 'C' : 'c');
                }
                else if (key.Key == ConsoleKey.D)
                {
                    currentChar = (caps ? 'D' : 'd');
                }
                else if (key.Key == ConsoleKey.E)
                {
                    currentChar = (caps ? 'E' : 'e');
                }
                else if (key.Key == ConsoleKey.F)
                {
                    currentChar = (caps ? 'F' : 'f');
                }
                else if (key.Key == ConsoleKey.G)
                {
                    currentChar = (caps ? 'G' : 'g');
                }
                else if (key.Key == ConsoleKey.H)
                {
                    currentChar = (caps ? 'H' : 'h');
                }
                else if (key.Key == ConsoleKey.I)
                {
                    currentChar = (caps ? 'I' : 'i');
                }
                else if (key.Key == ConsoleKey.J)
                {
                    currentChar = (caps ? 'J' : 'j');
                }
                else if (key.Key == ConsoleKey.K)
                {
                    currentChar = (caps ? 'K' : 'k');
                }
                else if (key.Key == ConsoleKey.L)
                {
                    currentChar = (caps ? 'L' : 'l');
                }
                else if (key.Key == ConsoleKey.M)
                {
                    currentChar = (caps ? 'M' : 'm');
                }
                else if (key.Key == ConsoleKey.N)
                {
                    currentChar = (caps ? 'N' : 'n');
                }
                else if (key.Key == ConsoleKey.O)
                {
                    currentChar = (caps ? 'O' : 'o');
                }
                else if (key.Key == ConsoleKey.P)
                {
                    currentChar = (caps ? 'P' : 'p');
                }
                else if (key.Key == ConsoleKey.Q)
                {
                    currentChar = (caps ? 'Q' : 'q');
                }
                else if (key.Key == ConsoleKey.R)
                {
                    currentChar = (caps ? 'R' : 'r');
                }
                else if (key.Key == ConsoleKey.S)
                {
                    currentChar = (caps ? 'S' : 's');
                }
                else if (key.Key == ConsoleKey.T)
                {
                    currentChar = (caps ? 'T' : 't');
                }
                else if (key.Key == ConsoleKey.U)
                {
                    currentChar = (caps ? 'U' : 'u');
                }
                else if (key.Key == ConsoleKey.V)
                {
                    currentChar = (caps ? 'V' : 'v');
                }
                else if (key.Key == ConsoleKey.W)
                {
                    currentChar = (caps ? 'W' : 'w');
                }
                else if (key.Key == ConsoleKey.X)
                {
                    currentChar = (caps ? 'X' : 'x');
                }
                else if (key.Key == ConsoleKey.Y)
                {
                    currentChar = (caps ? 'Y' : 'y');
                }
                else if (key.Key == ConsoleKey.Z)
                {
                    currentChar = (caps ? 'Z' : 'z');
                }
                else if (key.Key == ConsoleKey.D0)
                {
                    currentChar = ('0');
                }
                else if (key.Key == ConsoleKey.D1)
                {
                    currentChar = (caps ? '!' : '1');
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    currentChar = ('2');
                }
                else if (key.Key == ConsoleKey.D3)
                {
                    currentChar = ('3');
                }
                else if (key.Key == ConsoleKey.D4)
                {
                    currentChar = ('4');
                }
                else if (key.Key == ConsoleKey.D5)
                {
                    currentChar = ('5');
                }
                else if (key.Key == ConsoleKey.D6)
                {
                    currentChar = ('6');
                }
                else if (key.Key == ConsoleKey.D7)
                {
                    currentChar = ('7');
                }
                else if (key.Key == ConsoleKey.D8)
                {
                    currentChar = ('8');
                }
                else if (key.Key == ConsoleKey.D9)
                {
                    currentChar = ('9');
                }
                else if (key.Key == ConsoleKey.Spacebar)
                {
                    currentChar = (' ');
                }
                else if (key.Key == ConsoleKey.OemPeriod)
                {
                    currentChar = ('.');
                }
                else if (key.Key == ConsoleKey.OemComma)
                {
                    currentChar = (',');
                }
                else if (key.Key == ConsoleKey.Oem1)
                {
                    currentChar = (caps ? ':' : ';');
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    currentChar = '◄';
                }
                else if (key.KeyChar == '\u005C')
                {
                    currentChar = '\u005C';
                }
                else if (key.KeyChar == '\u002F')
                {
                    currentChar = '\u002F';
                }
                else if (key.KeyChar == '?')
                {
                    currentChar = '?';
                }
                else if (key.KeyChar == '\u000D')
                {
                    currentChar = '\u000D';
                }
                else
                {
                    currentChar = '�';
                }

                if (activeApp == 1)
                {
                    if (!dirSelectOpen)
                    {
                        if (currentChar == '◄')
                        {
                            if (notePadChars.Count > 0)
                            {
                                notePadChars.RemoveAt(notePadChars.Count - 1);
                            }
                        }
                        else
                        {
                            notePadChars.Add(currentChar);
                        }
                    }
                    else
                    {
                        if (currentChar == '◄')
                        {
                            if (dirChars.Count > 0)
                            {
                                dirChars.RemoveAt(dirChars.Count - 1);
                            }
                        }
                        else
                        {
                            dirChars.Add(currentChar);
                        }
                    }
                }
            }
        }
    }
}