using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// DB 欄位屬性
    /// </summary>
    public class DBColumnAttribute : System.Attribute
    {
        /// <summary>
        /// 是否為主鍵
        /// </summary>
        public bool IsPrimaryKey;
        /// <summary>
        /// 是否為Serial Key e.g. 201700912234212424
        /// </summary>
        public bool IsSerialKey;
        /// <summary>
        /// 是否為DB時間
        /// </summary>
        public bool IsCreateUpdateDate;
        /// <summary>
        /// 是否為欄位
        /// </summary>
        public bool IsColumn;
        public bool IsNotNull;
        public DBColumnAttribute()
        {

        }
    }
    public static class DBColumnAttributeExt
    {
        public static bool IsRandomChar(this PropertyInfo item,iTable obj, string randomidcharacter)
        {
            return (item.GetCustomAttribute<DBColumnAttribute>() != null
                        && item.GetCustomAttribute<DBColumnAttribute>().IsSerialKey)
                        || (item.GetValue(obj) != null && item.GetValue(obj).ToString() == randomidcharacter);
        }
        public static bool IsCreateUpdateDate(this PropertyInfo item)
        {
            return item.GetCustomAttribute<DBColumnAttribute>() != null
                        && item.GetCustomAttribute<DBColumnAttribute>().IsCreateUpdateDate;
        }
        public static bool IsColumn(this PropertyInfo item, iTable obj)
        {
            return item.GetCustomAttribute<DBColumnAttribute>() != null
                        && item.GetCustomAttribute<DBColumnAttribute>().IsColumn
                        && item.GetValue(obj) != null;
        }
        public static bool IsPrimaryKey(this PropertyInfo item)
        {
            return item.GetCustomAttribute<DBColumnAttribute>() != null
                        && item.GetCustomAttribute<DBColumnAttribute>().IsPrimaryKey
                           && item.GetCustomAttribute<DBColumnAttribute>().IsColumn == true;
        }
        public static bool IsNotPrimaryKey(this PropertyInfo item)
        {
            return item.GetCustomAttribute<DBColumnAttribute>() != null
                        && item.GetCustomAttribute<DBColumnAttribute>().IsPrimaryKey
                           && item.GetCustomAttribute<DBColumnAttribute>().IsColumn == true;
        }
        public static bool IsNotNull(this PropertyInfo item)
        {
            return item.GetCustomAttribute<DBColumnAttribute>() != null
                        && item.GetCustomAttribute<DBColumnAttribute>().IsColumn
                        && item.GetCustomAttribute<DBColumnAttribute>().IsNotNull;
        }
        /// <summary>
        /// 取得此型別所對應的PK property info
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPrimaryKeys(this Type type)
        {
            return type.GetProperties().Where(x => x.IsPrimaryKey());
        }
        /// <summary>
        /// 取得此型別所對應的非PK property info
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetNonPrimaryKeys(this Type type)
        {
            return type.GetProperties().Where(x => x.IsNotPrimaryKey());
        }
        /// <summary>
        /// 取得此型別所對應的欄位所有的 property info
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetColumns(this Type type)
        {
            return from c in type.GetProperties()
                   where c.GetCustomAttribute<DBColumnAttribute>() != null
                         && c.GetCustomAttribute<DBColumnAttribute>().IsColumn
                   select c;
        }
    }
}
