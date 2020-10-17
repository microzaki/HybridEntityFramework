using DataBaseAccess;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HybridEntityFramework
{
    public static partial class DBctrlExt
    {
        public static List<SqlScript> UpdateParameter<T>(this DBCtrl db, T Main, eSysID sysid, eBaseDataType dataType)
        {
            List<SqlScript> sqls = new List<SqlScript>();

            foreach (PropertyInfo item in Main.GetType().GetProperties().Where(x => x.GetCustomAttribute<ConfigAttribute>() != null && x.GetCustomAttribute<ConfigAttribute>().IsSettingColumn))
            {
                SYSBASEDATA<eSysID, eBaseDataType> data = new SYSBASEDATA<eSysID, eBaseDataType>();
                // 將設定值寫入DB
                data.SYSID = sysid;
                data.MODELNAME = "*";
                data.BASEDATATYPE = dataType;
                data.BASEDATAID = item.Name;
                data.BASEDATAVALUE = item.PropertyType.IsArray ? data.BASEDATAVALUE = item.GetValue(Main).ToArrayString() : data.BASEDATAVALUE = item.GetValue(Main).ToString();

                sqls.AddRange(db.UpdateSysBaseData(data));
            }

            return sqls;
        }
    }
}
