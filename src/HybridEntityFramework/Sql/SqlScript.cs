using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// 資料庫交易物件
    /// </summary>
    public class SqlScript
    {
        public string sql { get; set; }
        /// <summary>
        /// 通用格式
        /// </summary>
        public List<SqlParam> dbparameters = new List<SqlParam>();
        public SqlScript()
        {

        }
        public SqlScript(string sql)
            => (this.sql) = (sql);
        public SqlScript(string sql, params SqlParam[] dbparameters)
            => (this.sql, this.dbparameters) = (sql, dbparameters.ToList());
        /// <summary>
        /// MSSQL使用的parameter
        /// </summary>
        public IEnumerable<System.Data.SqlClient.SqlParameter> mssqlparameters
        {
            get
            {
                foreach (var item2 in dbparameters)
                {
                    yield return new System.Data.SqlClient.SqlParameter(item2.name, item2.value);
                }
            }
        }

        public IEnumerable<SQLiteParameter> sqliteparameters
        {
            get
            {
                foreach (var item2 in dbparameters)
                {
                    yield return new SQLiteParameter(item2.name, item2.value);
                }
            }
        }
    }
}
