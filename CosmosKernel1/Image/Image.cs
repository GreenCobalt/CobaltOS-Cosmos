using CosmosKernel1.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CosmosKernel1.Image
{
    class Image
    {
        public static int[] getRPixels(String s)
        {
            String[] R = s.Split(";")[1].Split(",");
            int[] Ra = Array.ConvertAll(R, r => int.Parse(r));
            return Ra;
        }
        public static int[] getGPixels(String s)
        {
            String[] G = s.Split(";")[2].Split(",");
            int[] Ga = Array.ConvertAll(G, g => int.Parse(g));
            return Ga;
        }
        public static int[] getBPixels(String s)
        {
            String[] B = s.Split(";")[3].Split(",");
            int[] Ba = Array.ConvertAll(B, b => int.Parse(b));
            return Ba;
        }

        public static int[] getImageSize(String s)
        {
            String[] S = s.Split(";")[0].Split(",");
            return new int[] { int.Parse(S[0]), int.Parse(S[1]) };
        }
    }
}
