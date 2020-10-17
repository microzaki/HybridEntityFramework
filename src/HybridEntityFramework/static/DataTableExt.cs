using DataBaseAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// All DataTable Extension Function
    /// </summary>
    public static partial class DataTableExt
    {
        /// <summary>
        /// DataTable --> table object
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="DataTableValue"></param>
        /// <param name="columnnameprefix">欄位名稱prefix</param>
        /// <returns></returns>
        public static IEnumerable<TResult> ToiDBTable<TResult>(this DataTable DataTableValue, string columnnameprefix="") where TResult : class, iTable,new()
        {
            Type type = typeof(TResult);
            List<PropertyInfo> pr_List = type.GetColumns().Where(x => DataTableValue.Columns.IndexOf(columnnameprefix + x.Name) != -1).ToList();
            foreach (DataRow item in DataTableValue.Rows)
            {
                TResult tr = new TResult();
                foreach (var item1 in pr_List.Where(x => item[columnnameprefix + x.Name] != DBNull.Value))
                {
                    item1.SetValue(tr, item1.PropertyType.IsEnum ? Enum.Parse(item1.PropertyType, item[columnnameprefix + item1.Name].ToString()) : item[columnnameprefix + item1.Name], null);
                }
                yield return tr;
            }

        }
    }
}
