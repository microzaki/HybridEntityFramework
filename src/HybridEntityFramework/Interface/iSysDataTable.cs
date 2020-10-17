
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// Interface Of Base Data、Trx Data 
    /// </summary>
    /// <typeparam name="TSYSID"></typeparam>
    /// <typeparam name="TDATATYPE"></typeparam>
    public interface iSysDataTable<TSYSID, TDATATYPE>
    {
        /// <summary>
        /// 系統代碼
        /// </summary>
        TSYSID SYSID { get; }
        /// <summary>
        /// 資料類型
        /// </summary>
        TDATATYPE DATATYPE { get; }
        /// <summary>
        /// 資料類型
        /// </summary>
        string DATAID { get; }
        /// <summary>
        /// 料號，如果不分料號，設定為 *
        /// </summary>
        string MODELNAME { get; }
        /// <summary>
        /// 資料庫為字串類型，stringExtension ToType轉換為需要的格式
        /// </summary>
        string DATAVALUE { get; }
    }
    
    
    public static class iSysDataTableExt
    {
        public static void Read<T, TSYSID, TBASEDATATYPE>(this IEnumerable<iSysDataTable<TSYSID, TBASEDATATYPE>> rows, ref T obj) where T : iSysData<TSYSID>, new()
        {
            foreach (var item in typeof(T).GetProperties().Where(x => x.GetCustomAttribute<ConfigAttribute>() != null && x.GetCustomAttribute<ConfigAttribute>().IsSettingColumn))
            {
                TSYSID ConfigGroup = obj.sysid;
                string typename = typeof(T).Name;
                if (typename.Contains("`"))
                    typename = typename.Split(new string[] { "`" }, StringSplitOptions.RemoveEmptyEntries)[0];
                iSysDataTable<TSYSID, TBASEDATATYPE> syconfig = rows.Where(x => x.SYSID.ToString() == ConfigGroup.ToString()
                                                         && x.DATATYPE.ToString() == typename
                                                         && x.DATAID.Trim() == item.Name).FirstOrDefault();
                if (syconfig == null
                    || string.IsNullOrEmpty(syconfig.DATAVALUE))
                {
                    if (item.GetCustomAttribute<ConfigAttribute>().DefaultIfNoData)
                    {
                        item.SetValue(obj, GetDefaultValue(item.GetType()));
                        continue;
                    }
                    else
                    {
                        continue;
//throw new Exception($"Can't get the setting for SYSID : {obj.sysid} , DATATYPE : { typeof(T).Name}  , DATAID : { item.Name} from {obj.tablename} table");
                    }
                }
                #region enum value setting
                if (item.PropertyType.IsEnum)
                {
                    if (int.TryParse(syconfig.DATAVALUE, out int res))
                        item.SetValue(obj, res);
                    else
                        item.SetValue(obj, Enum.Parse(item.PropertyType, Convert.ToString(syconfig.DATAVALUE)));
                    continue;
                }
                Type elementtype = item.PropertyType.GetElementType();
                if (elementtype != null
                    && elementtype.IsEnum)
                {
                    string[] strs = syconfig.DATAVALUE.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (int.TryParse(strs[0], out int res))
                    {
                        item.SetValue(obj, (object)Array.ConvertAll(syconfig.DATAVALUE.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries), int.Parse));
                    }
                    else
                    {
                        throw new Exception("No spport Enum Array by string, use int instead");
                    }
                    continue;
                }
                #endregion
                item.SetValue(obj, syconfig.DATAVALUE.ToType(item.PropertyType));
            }
        }
        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        public static List<SqlScript> Update<T, TSYSID, TBASEDATATYPE>(this T obj) where T : SysData<TSYSID>
        {
            List<SqlScript> sqls = new List<SqlScript>();
            foreach (var item in typeof(T).GetProperties().Where(x => x.GetCustomAttribute<ConfigAttribute>() != null
                                                                && x.GetCustomAttribute<ConfigAttribute>().IsSettingColumn))
            {
                SqlScript sql = new SqlScript();
                string value = nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.DATAVALUE);
                string ssid = nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.SYSID);
                string model = nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.MODELNAME);
                string datatype = nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.DATATYPE);
                string dataid = nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.DATAID);
                sql.sql = $@"UPDATE {obj.tablename} SET {obj.prefix}{value}=@{value}
WHERE {ssid}=@{ssid}
      AND {model}='*'
      AND {obj.prefix}{datatype}=@{datatype}
      AND {obj.prefix}{dataid}=@{dataid}";
                if (item.PropertyType.IsArray
                    && item.PropertyType.Name.Contains(",") == false)
                {
                    sql.dbparameters.Add(new SqlParam(value, item.GetValue(obj).ToArrayString()));
                }
                else if (item.PropertyType.IsEquivalentTo(typeof(double[,])))
                {
                    double[,] ary = item.GetValue(obj) as double[,];
                    if (ary == null)
                        continue;
                    string val = "";
                    for (int i = 0; i < ary.GetLength(0); i++)
                    {
                        val = val + ";";
                        for (int j = 0; j < ary.GetLength(1); j++)
                        {
                            if (j < ary.GetLength(1) - 1)
                                val = val + ary[i, j].ToString() + ",";
                            else
                                val = val + ary[i, j].ToString();
                        }
                    }
                    val = val + ";";
                    val = val.Remove(0, 1);
                    sql.dbparameters.Add(new SqlParam(value, val.ToString()));
                }
                else
                    sql.dbparameters.Add(new SqlParam(value, item.GetValue(obj).ToString()));
                sql.dbparameters.Add(new SqlParam(ssid, obj.sysid));
                sql.dbparameters.Add(new SqlParam(datatype, typeof(T).Name));
                sql.dbparameters.Add(new SqlParam(dataid, item.Name));
                sqls.Add(sql);
            }
            return sqls;
        }

        public static List<SqlScript> InsertOrReplace<T, TSYSID, TBASEDATATYPE>(this T obj,string modelnamevalue,bool replace=false) where T : SysData<TSYSID>
        {
            List<SqlScript> sqls = new List<SqlScript>();
            foreach (var item in typeof(T).GetProperties().Where(x => x.GetCustomAttribute<ConfigAttribute>() != null
                                                                && x.GetCustomAttribute<ConfigAttribute>().IsSettingColumn))
            {
                SqlScript sql = new SqlScript();
                string value =  obj.prefix +nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.DATAVALUE);
                string ssid = nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.SYSID);
                string modelname = nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.MODELNAME);
                string datatype = obj.prefix + nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.DATATYPE);
                string dataid = obj.prefix + nameof(iSysDataTable<TSYSID, TBASEDATATYPE>.DATAID);
                string replacestr = "";
                if (replace)
                {
                    replacestr = "OR REPLACE";
                }
                sql.sql = $@"INSERT {replacestr} INTO {obj.tablename} ({ssid},{modelname},{datatype},{dataid},{value}) VALUES(:{ssid},:{modelname},:{datatype},:{dataid},:{value})";
                if (item.PropertyType.IsArray
                    && item.PropertyType.Name.Contains(",") == false)
                {
                    sql.dbparameters.Add(new SqlParam(value, item.GetValue(obj).ToArrayString()));
                }
                else if (item.PropertyType.IsEquivalentTo(typeof(double[,])))
                {
                    double[,] ary = item.GetValue(obj) as double[,];
                    if (ary == null)
                        continue;
                    string val = "";
                    for (int i = 0; i < ary.GetLength(0); i++)
                    {
                        val = val + ";";
                        for (int j = 0; j < ary.GetLength(1); j++)
                        {
                            if (j < ary.GetLength(1) - 1)
                                val = val + ary[i, j].ToString() + ",";
                            else
                                val = val + ary[i, j].ToString();
                        }
                    }
                    val = val + ";";
                    val = val.Remove(0, 1);
                    sql.dbparameters.Add(new SqlParam(value, val.ToString()));
                }
                else
                    sql.dbparameters.Add(new SqlParam(value, item.GetValue(obj).ToString()));
                sql.dbparameters.Add(new SqlParam(ssid, obj.sysid));
                sql.dbparameters.Add(new SqlParam(modelname, modelnamevalue));
                sql.dbparameters.Add(new SqlParam(datatype, typeof(T).Name));
                sql.dbparameters.Add(new SqlParam(dataid, item.Name));
                sqls.Add(sql);
            }
            return sqls;
        }
    }
    


}
