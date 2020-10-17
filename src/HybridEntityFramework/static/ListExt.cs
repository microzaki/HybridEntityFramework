using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public static class ListExt
    {
        public static void Add<T>(this List<T> data,params T[] adddata)
        {
            data.AddRange(adddata);
        }
        public static void AddRange<T>(this List<T> data, params IEnumerable<T>[] adddata)
        {
            foreach (var item in adddata)
            {
                data.AddRange(item);
            }
        }
    }
}
