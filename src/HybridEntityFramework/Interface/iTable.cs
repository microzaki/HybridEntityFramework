
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// Table物件的interface，識別Table物件使用
    /// 目前無任何成員
    /// </summary>
    public interface iTable
    {

    }
    /// <summary>
    /// All iDBClass Extension Function
    /// </summary>
    public static partial class iTableExt
    {
        private const string serialvalue = "REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(varchar, SYSDATETIME(), 121),':',''),' ',''),'-',''),'.','')";
        private const string datevalue = "SYSDATETIME()";
        public static string TableName(this iTable obj)
        {
            return new string(obj.GetType().Name.Where(p => char.IsLetter(p) || p == '_').ToArray());
        }
        /// <summary>
        /// Table 物件 --> SQL Script for select all
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paramchar"></param>
        /// <returns></returns>
        public static SqlScript SelectAllScript(this iTable obj, string paramchar)
        {
            SqlScript dbsqlscript = new SqlScript();
            string wheresql = "";
            foreach (var item in typeof(object).GetProperties())
            {
                object provalue = item.GetValue(obj);
                if (provalue == null)
                    continue;
                wheresql = $"{item.Name}={paramchar}{item.Name}";
                dbsqlscript.dbparameters.Add(new SqlParam(item.Name, provalue));
            }
            dbsqlscript.sql = $@"
SELECT *
FROM {obj.TableName()}
{wheresql}";
            return dbsqlscript;
        }
        /// <summary>
        /// Table 物件 --> SQL Script for select by PK
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paramchar"></param>
        /// <returns></returns>
        public static SqlScript SelectAllWhereByPKScript(this iTable obj, string paramchar)
        {
            SqlScript dbsqlscript = new SqlScript();
            string PKWhere = "";
            foreach (var item in obj.GetType().GetPrimaryKeys())
            {
                if (string.IsNullOrEmpty(PKWhere))
                    PKWhere = $"{item.Name}={paramchar}{item.Name}";
                else
                    PKWhere = $"{PKWhere} AND {item.Name}={paramchar}{item.Name}";
                dbsqlscript.dbparameters.Add(new SqlParam(name: item.Name, value: item.GetValue(obj)));
            }
            dbsqlscript.sql = $@"
SELECT *
FROM {obj.TableName()}
WHERE {PKWhere}";
            return dbsqlscript;
        }
        

        /// <summary>
        /// Update Data By PK 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paramchar"></param>
        /// <returns></returns>
        public static List<SqlScript> UpdateByPKScript(this iTable obj, string paramchar)
        {
            Type type = obj.GetType();
            SqlScript sqlscript = new SqlScript();
            string set = "";
            string where = "";
            updatesql(type, paramchar, obj, ref set, ref where, ref sqlscript.dbparameters);
            if (type.Name.IndexOf('`') > 0)
            {
                sqlscript.sql = $"UPDATE {type.Name.Remove(type.Name.IndexOf('`'))} SET {set} WHERE {where}";
            }
            else
            {

                sqlscript.sql = $"UPDATE {type.Name.Replace("`1", "")} SET {set} WHERE {where}";
            }
            return new List<SqlScript>() { sqlscript };
        }
        public static List<SqlScript> UpdateByPKScript(this IEnumerable<iTable> obj, string paramchar)
        {
            List<SqlScript> sqlscripts = new List<SqlScript>();
            foreach (var item in obj)
            {
                sqlscripts.AddRange(item.UpdateByPKScript(paramchar));
            }
            return sqlscripts;
        }
        /// <summary>
        /// Insert Data
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paramchar"></param>
        /// <param name="randomidcharacter"></param>
        /// <returns></returns>
        public static List<SqlScript> InsertScript(this iTable obj, string paramchar, string randomidcharacter, eInsertUpdateType inserttype = eInsertUpdateType.Normal)
        {
            Type type = obj.GetType();
            SqlScript sqlscript = new SqlScript();
            string into = "";
            string value = "";
            insertsql(type, paramchar, randomidcharacter, obj, ref into, ref value, ref sqlscript.dbparameters);
            string insert = "INSERT";
            switch (inserttype)
            {
                case eInsertUpdateType.Normal:
                    break;
                case eInsertUpdateType.Replace:
                    insert += " OR REPLACE ";
                    break;
                case eInsertUpdateType.Ignore:
                    insert += " OR IGNORE ";
                    break;
                default:
                    break;
            }
            if (type.Name.IndexOf('`') > 0)
            {
                sqlscript.sql = $"{insert} INTO { type.Name.Remove(type.Name.IndexOf('`'))} ({into}) VALUES ({value})";
            }
            else
            {
                sqlscript.sql = $"{insert} INTO {type.Name.Replace("`1", "")} ({into}) VALUES ({value})";
            }
            // sqlscript.sql = $"INSERT INTO {type.Name.Replace("`1", "")} ({into}) VALUES ({value})";
            //   sqlscript.sql = $"INSERT INTO { type.Name.Remove(type.Name.IndexOf('`'))} ({into}) VALUES ({value})";
            return new List<SqlScript>() { sqlscript };
        }

        public static List<SqlScript> InsertSelectScript(this iTable obj, string paramchar, string randomidcharacter)
        {
            Type type = obj.GetType();
            SqlScript sqlscript = new SqlScript();
            string into = "";
            string value = "";
            string set = "";
            string where = "";
            updatesql(type, paramchar, obj, ref set, ref where, ref sqlscript.dbparameters);
            insertsql(type, paramchar, randomidcharacter, obj, ref into, ref value, ref sqlscript.dbparameters);
            if (type.Name.IndexOf('`') > 0)
            {
                sqlscript.sql = $@"
begin tran
 
  
    UPDATE {type.Name.Remove(type.Name.IndexOf('`'))} SET {set} WHERE {where}
   if @@rowcount = 0
   begin
      INSERT INTO {type.Name.Remove(type.Name.IndexOf('`'))} ({into}) VALUES ({value})
   end
commit tran";
            }
            else
            {
                sqlscript.sql = $@"
begin tran
 
    UPDATE {type.Name.Replace("`1", "")} SET {set} WHERE {where} 
 
   if @@rowcount = 0
   begin
      INSERT INTO {type.Name.Replace("`1", "")} ({into}) VALUES ({value})
   end
commit tran";
            }

            return new List<SqlScript>() { sqlscript };
        }
        private static void insertsql(Type type, string paramchar, string randomidcharacter, iTable obj, ref string into, ref string value, ref List<SqlParam> dbparam)
        {
            foreach (var item in type.GetProperties())
            {
                if (item.IsRandomChar(obj, randomidcharacter))
                {
                    into = string.IsNullOrEmpty(into) ? $"{item.Name}" : $"{into},{item.Name}";
                    value = string.IsNullOrEmpty(value) ? serialvalue : $"{value},{serialvalue}";
                }
                else if (item.IsCreateUpdateDate())
                {
                    into = string.IsNullOrEmpty(into) ? $"{item.Name}" : $"{into},{item.Name}";
                    value = string.IsNullOrEmpty(value) ? datevalue : $"{value},{datevalue}";
                }
                else if (item.IsColumn(obj))
                {
                    into = string.IsNullOrEmpty(into) ? $"{item.Name}" : $"{into},{item.Name}";
                    value = string.IsNullOrEmpty(value) ? $"{paramchar}{item.Name}" : $"{value},{paramchar}{item.Name}";
                    if (dbparam.Where(x => x.name == item.Name).Count() == 0)
                    {
                        if (item.PropertyType.IsEnum)
                            dbparam.Add(new SqlParam(item.Name, item.GetValue(obj).ToString()));
                        else
                            dbparam.Add(new SqlParam(item.Name, item.GetValue(obj)));
                    }
                }
            }
        }
        private static void updatesql(Type type, string paramchar, iTable obj, ref string set, ref string where, ref List<SqlParam> dbparams)
        {
            foreach (var item in type.GetPrimaryKeys())
            {
                if (string.IsNullOrEmpty(where))
                    where = $"{item.Name}={paramchar}{item.Name}";
                else
                    where = $"{where} AND {item.Name}={paramchar}{item.Name}";
                if (dbparams.Where(x => x.name == item.Name).Count() == 0)
                    dbparams.Add(new SqlParam(item.Name, item.GetValue(obj)));
            }
            foreach (var item in type.GetNonPrimaryKeys())
            {
                if (string.IsNullOrEmpty(set))
                    set = $"{item.Name}={paramchar}{item.Name}";
                else
                    set = $"{set},{item.Name}={paramchar}{item.Name}";
                if (item.GetValue(obj) == null)
                    dbparams.Add(new SqlParam(item.Name, DBNull.Value));
                else
                {
                    if (dbparams.Where(x => x.name == item.Name).Count() == 0)
                    {
                        if (item.PropertyType.IsEnum)
                            dbparams.Add(new SqlParam(item.Name, item.GetValue(obj).ToString()));
                        else
                            dbparams.Add(new SqlParam(item.Name, item.GetValue(obj)));
                    }
                }
            }
        }

        /// <summary>
        /// Set the table object by another smilar object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cloneobj"></param>
        public static void SetByObj(this iTable obj, object cloneobj)
        {
            PropertyInfo[] pros = obj.GetType().GetProperties();
            foreach (PropertyInfo p in cloneobj.GetType().GetProperties())
            {
                var v = from c in pros
                        where c.Name == p.Name && c.PropertyType == p.PropertyType
                        select c;
                if (v.Count() > 0)
                    p.SetValue(obj, p.GetValue(cloneobj, null), null);
            }
        }

        public static SqlScript DropScript(this iTable obj)
        {
            return new SqlScript() { sql = $"DROP TABLE {obj.TableName()}" };
        }
        public static SqlScript CreateScript(this Type obj)
        {
            SqlScript dbsqlscript = new SqlScript() { sql = $@"
CREATE TABLE {obj.Name.IsLetter()} (
" };
            string wheresql = "";
            string pk = "";
            List<PropertyInfo> props = obj.GetProperties().Where(item => item.GetCustomAttribute<DBColumnAttribute>() != null
                          && item.GetCustomAttribute<DBColumnAttribute>().IsColumn).ToList();
            for (int i = 0; i < props.Count(); i++)
            {
                string itemstr = props[i].Name;
                if (props[i].PropertyType.IsEnum || props[i].PropertyType == typeof(string))
                {
                    itemstr += " TEXT ";
                }
                else if (props[i].PropertyType == typeof(int))
                {
                    itemstr += " INTEGER ";
                }
                else if (props[i].PropertyType == typeof(double))
                {
                    itemstr += " REAL ";
                }

                if (props[i].IsPrimaryKey())
                {
                    if (i < props.Count() - 1)
                        pk += $" {props[i].Name}, ";
                    else
                        pk += $" {props[i].Name} ";
                }
                if (props[i].IsNotNull())
                {
                    itemstr += " NOT NULL ";
                }
                if (i < props.Count() - 1)
                {
                    wheresql += itemstr + @",
";
                }
                else
                    wheresql += itemstr;
            }
            if (string.IsNullOrEmpty(pk) == false)
            {
                wheresql +=$@",
PRIMARY KEY({pk})";
            }
            dbsqlscript.sql += wheresql + @"
)";
            return dbsqlscript;
        }
    }
}
