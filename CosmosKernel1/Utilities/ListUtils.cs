using System;
using System.Collections.Generic;

namespace CobaltOS.Utilities
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

        public static List<char> stringToCharList(String input)
        {
            List<char> l = new List<char>();
            foreach (Char c in input)
            {
                l.Add(c);
            }
            return l;
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
            foreach (String current in input)
            {
                if (current == needle)
                {
                    return true;
                }
            }
            return false;
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

        public static int[] arrayStringToInt(string[] input)
        {
            int[] returnArray = new int[input.Length];
            for(int i = 0; i < input.Length; i++)
            {
                returnArray[i] = int.Parse(input[i]);
            }
            return returnArray; 
        }
    }
}