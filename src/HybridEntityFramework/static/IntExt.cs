using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public static class IntExt
    {
        public static int Max(this int val, params int[] vals)
        {
            int max = val;
            foreach (var item in vals)
            {
                max=Math.Max(item, max);
            }
            return max;
        }
        public static int Max(this int[] vals)
        {
            int max = vals[0];
            foreach (var item in vals)
            {
                max = Math.Max(item, max);
            }
            return max;
        }
        public static int Max(this IEnumerable<int>[] vals)
        {
            return Max(vals.ToArray());
        }
        public static int[] multiply(this int[] val,int multiplyvalue)
        {
            for (int i = 0; i < val.Length; i++)
            {
                val[i] = val[i] * multiplyvalue;
            }
            return val;
        }
    }
}
