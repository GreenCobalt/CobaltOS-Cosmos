using CosmosKernel1.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CosmosKernel1.Image
{
    class Image
    {
        public static int[] getPixels(String s)
        {
            String[] R = s.Split(";")[1].Split(",");
            String[] G = s.Split(";")[2].Split(",");
            String[] B = s.Split(";")[3].Split(",");

            int[] Ra = Array.ConvertAll(R, r => int.Parse(r));
            int[] Ga = Array.ConvertAll(G, g => int.Parse(g));
            int[] Ba = Array.ConvertAll(B, b => int.Parse(b));

            int[] returnA = { };
            int currentList = 0;
            for (int i = 0; i < Ra.Length+Ba.Length+Ga.Length+1; i++)
            {
                if (currentList == 0 && i > Ra.Length - 1)
                {
                    currentList++;
                    i++;
                    returnA[i] = 256;
                    continue;
                }
                else if (currentList == 0 && i > Ra.Length + Ga.Length - 1)
                {
                    currentList++;
                    i++;
                    returnA[i] = 256;
                    continue;
                }
                returnA[i] = (currentList == 0 ? Ra[i] : (currentList == 1 ? Ga[i - Ra.Length] : Ba[i - Ga.Length - Ra.Length]));
            }
            return returnA;
        }

        public static int[] getImageSize(String s)
        {
            String[] S = s.Split(";")[0].Split(",");
            return new int[] { int.Parse(S[0]), int.Parse(S[1]) };
        }
    }
}
