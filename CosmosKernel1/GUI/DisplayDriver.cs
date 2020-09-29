using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CosmosKernel1.GUI
{
    class DisplayDriver
    {
        private static Boolean newGraphics;

        public static void init(Boolean graphics)
        {
            newGraphics = graphics;
        }

        public static void initScreen()
        {
            if (newGraphics) VMDisplayDriver.initScreen();
            else CanvasDisplayDriver.initScreen();
        }

        public static void changeRes(int x, int y)
        {
            if (newGraphics) VMDisplayDriver.changeRes(x, y);
            else CanvasDisplayDriver.changeRes(x, y);
        }

        public static int addText(int x, int y, Color c, String s)
        {
            if (newGraphics) return VMDisplayDriver.addText(x, y, c, s);
            else return CanvasDisplayDriver.addText(x, y, c, s);
        }
        public static int typeChar(int x, int y, Color c, Char s)
        {
            if (newGraphics) return VMDisplayDriver.typeChar(x, y, c, s);
            else return CanvasDisplayDriver.typeChar(x, y, c, s);
        }

        public static void setFullBuffer(Color c)
        {
            if (newGraphics) VMDisplayDriver.setFullBuffer(c);
            else CanvasDisplayDriver.setFullBuffer(c);
        }

        public static void addMouse(int x, int y)
        {
            if (newGraphics) VMDisplayDriver.addMouse(x, y);
            else CanvasDisplayDriver.addMouse(x, y);
        }

        public static void drawScreen()
        {
            if (newGraphics) VMDisplayDriver.drawScreen();
            else CanvasDisplayDriver.drawScreen();
        }

        public static void addRectangle(int x, int y, int endX, int endY, Color c)
        {
            if (newGraphics) VMDisplayDriver.addRectangle(x, y, endX, endY, c);
            else CanvasDisplayDriver.addRectangle(x, y, endX, endY, c);
        }

        public static void addFilledRectangle(int x, int y, int w, int h, Color c)
        {
            if (newGraphics) VMDisplayDriver.addFilledRectangle(x, y, w, h, c);
            else CanvasDisplayDriver.addFilledRectangle(x, y, w, h, c);
        }

        public static void addImage(String path, int locX, int locY)
        {
            if (newGraphics) VMDisplayDriver.addImage(path, locX, locY);
            else CanvasDisplayDriver.addImage(path, locX, locY);
        }
    }
}
