using System;
using System.Collections.Generic;
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
                    nums.Add(input.Substring(lastSignIndex, lastSignIndex + 2 - i));
                    signLoc.Add(i);
                    lastSignIndex = i;
                }
            }
            nums.Add(input.Substring(lastSignIndex + 1, input.Length - 2));
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

            return returnNum + "";
        }
    }
}