using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public static class UintExt
    {
        public static uint[] multiply(this uint[] val, uint multiplyvalue)
        {
            for (int i = 0; i < val.Length; i++)
            {
                val[i] = val[i] * multiplyvalue;
            }
            return val;
        }
    }
}
