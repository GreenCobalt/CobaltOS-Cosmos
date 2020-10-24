using System;
using System.Drawing;
using CobaltOS.GUI;

namespace CobaltOS.Font
{
    public static class FontDrawer
    {
        private static void DrawTextChar(int x, int y, byte[] Data, Color color)
        {
            int c = 0;
            for (int p = 0; p < 9; p++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Data[c] == 1)
                    {
                        DisplayDriver.setPixel(x + i * 2, y + p * 2, color);
                    }

                    c = c + 1;
                }
            }
        }

        public static void DrawArray(int x, int y, byte[] Data, Color color)
        {
            int c = 0;
            for (int p = 0; p < 16; p++)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (Data[c] == 1)
                    {
                        DisplayDriver.setPixel(x + i, y + p, color);
                    }
                    c = c + 1;
                }
            }
        }

        public static int WriteText(String Text, int Textx, int Texty, Color Color)
        {
            int spacing = 12;
            foreach (char a in Text)
            {
                if (a == 'A')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapA, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'a')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlA, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'B')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapB, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'b')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlB, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'C')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapC, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'c')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlC, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'D')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapD, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'd')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlD, Color);
                    Textx = Textx + spacing;
                }

                else if (a == 'E')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapE, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'e')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlE, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'F')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapF, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'f')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlF, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'G')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapG, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'g')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlG, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'H')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapH, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'h')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlH, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'I')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapI, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'i')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlI, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'J')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapJ, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'j')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlJ, Color);
                    Textx = Textx + spacing;
                }

                else if (a == 'K')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapK, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'k')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlK, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'L')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapL, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'l')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlL, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'M')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapM, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'm')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlM, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'N')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapN, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'n')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlN, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'O')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapO, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'o')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlO, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'P')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapP, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'p')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlP, Color);
                    Textx = Textx + spacing;
                }

                else if (a == 'Q')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapQ, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'q')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlQ, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'R')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapR, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'r')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlR, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'S')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapS, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 's')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlS, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'T')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapT, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 't')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlT, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'U')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapU, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'u')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlU, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'V')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapV, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'v')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlV, Color);
                    Textx = Textx + spacing;
                }

                else if (a == 'W')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapW, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'w')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlW, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'X')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapX, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'x')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlX, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'Y')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapY, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'y')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlY, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'Z')
                {
                    DrawTextChar(Textx, Texty, Font8x8.CapZ, Color);
                    Textx = Textx + spacing;
                }
                else if (a == 'z')
                {
                    DrawTextChar(Textx, Texty, Font8x8.SmlZ, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '.')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Dot1, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '=')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Eqls, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '/')
                {
                    DrawTextChar(Textx, Texty, Font8x8.FrSl, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '+')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Plus, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '\\')
                {
                    DrawTextChar(Textx, Texty, Font8x8.BkSl, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '!')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Excl, Color);
                    Textx = Textx + spacing;
                }
                else if (a == ' ')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Null, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '0')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Zero, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '1')
                {
                    DrawTextChar(Textx, Texty, Font8x8.One, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '2')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Two, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '3')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Three, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '4')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Four, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '5')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Five, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '6')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Six, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '7')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Seven, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '8')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Eight, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '9')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Nine, Color);
                    Textx = Textx + spacing;
                }
                else if (a == ':')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Colon, Color);
                    Textx = Textx + spacing;
                }
                else if (a == '-')
                {
                    DrawTextChar(Textx, Texty, Font8x8.Dash, Color);
                    Textx = Textx + spacing;
                }
            }
            return Textx;
        }
    }
}