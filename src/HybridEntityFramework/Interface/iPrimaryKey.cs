using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// Primary Key Interface，識別PK使用
    /// </summary>
    public interface iPrimaryKey
    {
    }
    public static partial class iPrimaryKeyExt
    {
        public static List<SqlScript> DeleteByPKScript(this iPrimaryKey obj, string paramchar)
        {
            Type type = obj.GetType();
            SqlScript sqlscript = new SqlScript();
            string where = "";
            deletesql(type, paramchar, obj, ref where, ref sqlscript.dbparameters);
            if (type.Name.IndexOf('`') > 0)
            {
                sqlscript.sql = $"DELETE FROM {type.Name.Remove(type.Name.IndexOf('`')).Replace("PK_","")} WHERE {where}";
            }
            else
            {

                sqlscript.sql = $"DELETE FROM {type.Name.Replace("`1", "").Replace("PK_", "")}  WHERE {where}";
            }
            return new List<SqlScript>() { sqlscript };
        }
        private static void deletesql(Type type, string paramchar, iPrimaryKey obj, ref string where, ref List<SqlParam> dbparams)
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
        }
    }
}
