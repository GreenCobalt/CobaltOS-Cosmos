using Cosmos.Debug.Kernel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CosmosKernel1.Utils
{
    class Calculator
    {

        public static Boolean executeCalc(List<Char> calcChars)
        {
            String r = calcNumber(ListUtils.charListToString(calcChars));
            calcChars.Clear();
            foreach (Char c in r)
            {
                calcChars.Add(c);
            }
            return true;
        }

        public static String calcNumber(String input)
        {
            Double returnNum = 1.0;
            List<string> nums = new List<string>();
            List<Char> sign = new List<Char>();
            List<int> signLoc = new List<int>();
            int lastSignIndex = 0;

            signLoc.Add(0);
            for (int i = 0, len = (input.Length + 1); i < len; i++)
            {
                if (input[i] == '+' || input[i] == '-' || input[i] == '/' || input[i] == 'x')
                {
                    sign.Add(input[i]);
                    nums.Add(input.Substring(lastSignIndex, i - lastSignIndex + 1));

                    signLoc.Add(i);
                    lastSignIndex = i;
                }
            }

            if (lastSignIndex == 0)
            {
                return "Invalid Syntax!";
            }

            //nums.Add(input.Substring(lastSignIndex + 1, input.Length - (lastSignIndex + 1) + 1));
            try
            {
                return stringSubstring(input, lastSignIndex + 1, input.Length);
            } catch
            {
                return "Err2";
            }

            try
            {
                return nums[0] + " " + sign[0] + " " + nums[1];

                signLoc.RemoveAt(0);

                if (ListUtils.listContains(nums, ""))
                {
                    return "0.0";
                }

                if (sign[0] == '+')
                {
                    returnNum = double.Parse(nums[0]) + double.Parse(nums[1]);
                }
                else if (sign[0] == '-')
                {
                    returnNum = double.Parse(nums[0]) - double.Parse(nums[1]);
                }
                else if (sign[0] == 'x')
                {
                    returnNum = double.Parse(nums[0]) * double.Parse(nums[1]);
                }
                else if (sign[0] == '/')
                {
                    returnNum = double.Parse(nums[0]) / double.Parse(nums[1]);
                }
            }
            catch
            {
                return "Calc Error 3!";
            }

            return returnNum.ToString().PadRight(2, '0');
        }

        public static String stringSubstring(String input, int start, int end)
        {
            StringBuilder sb = new StringBuilder("", 50);
            int i;
            for (i = 0; i < input.Length; i++) {
                if (i > (start - 1))
                {
                    if (i < (end + 1))
                    {
                        sb.Append(input[i]);
                    }
                }
            }
            return sb.ToString();
        }
    }
}