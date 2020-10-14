using System;
using System.Collections.Generic;
using System.Text;

namespace CobaltOS.Utilities
{
    class Math
    {
        public static bool IsDivisible(int x, int n)
        {
            return (x % n) == 0;
        }
    }
}