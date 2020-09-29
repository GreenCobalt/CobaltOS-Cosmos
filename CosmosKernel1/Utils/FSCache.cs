using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CosmosKernel1.Utils
{
    class FSCache
    {
        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        public static string getFile(String path)
        {
            if (!dict.ContainsKey(path))
            {
                FileStream f = File.OpenRead(path);
                byte[] a = new byte[f.Length];
                f.Read(a, 0, a.Length);
                String s = ListUtils.byteListToString(a);
                dict[path] = s;
            }
            return dict[path];
        }
    }
}
