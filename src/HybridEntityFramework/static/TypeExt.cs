using DataBaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// All Typ Extension Function
    /// </summary>
    public static class TypeExt
    {
        public static bool IsTypeArray<TCompare>(this Type type)
        {
            return type == typeof(IEnumerable<TCompare>)
                            || type == typeof(TCompare[])
                            || type == typeof(List<TCompare>);
        }


        /// <summary>
        /// 取得此型別的所有欄位，對應的SQL select，並且加上prefix
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string SelectSql(this Type type,string prefix)
        {
            string select = "";
            foreach (var item in type.GetColumns())
            {
                string comma = string.IsNullOrEmpty(select) ? "" : ",";
                select = $"{select}{comma}{prefix}.{item.Name} {prefix}_{item.Name}";
            }
            return select;
        }


    }
}
