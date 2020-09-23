using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosKernel1.Utils
{
    class ListUtils
    {
        public static String charListToString(List<Char> input)
        {
            String r = "";
            foreach (Char c in input)
            {
                r = r + c;
            }
            return r;
        }

        public static String byteListToString(byte[] input)
        {
            String r = "";
            foreach (Byte c in input)
            {
                r += (Char) c;
            }
            return r;
        }

        public static Boolean listContains(List<string> input, String needle)
        {
            Boolean output = false;
            foreach (String current in input)
            {
                if (current == needle)
                {
                    output = true;
                }
            }
            return output;
        }

        public static int countList(List<int> input)
        {
            int counter = 0;
            foreach (int s in input)
            {
                counter++;
            }
            return counter;
        }

        public static String subString(String input, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex + 1;
            List<Char> s = new List<char>();
            for (int i = 0; i < input.Length; i++)
            {
                if (i > startIndex && i < endIndex)
                {
                    s.Add(input[i]);
                }
            }
            return charListToString(s);
        }
    }
}