using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;

namespace HybridEntityFramework
{
    public partial class DBCtrl 
    {
        public string SerialNo
        {
            get
            {
                lock (this)
                {
                    System.Data.DataTable dt = ReadDataTableBySql(new SqlScript() { sql = @"SELECT REPLACE(REPLACE(REPLACE(REPLACE(CONVERT(varchar, SYSDATETIME(), 121),':',''),' ',''),'-',''),'.','') A" });
                    return dt.Rows[0][0].ToString();
                }
            }
        }
        public TMain Read<TMain, Tpk>(Tpk tpk) where Tpk : iPrimaryKey where TMain : class, iTable, new()
        {
            TMain value = new TMain();
            value.SetByObj(tpk);
            System.Data.DataTable dt = ReadDataTableBySql(value.SelectAllWhereByPKScript(paramchar));
            return dt.ToiDBTable<TMain>().FirstOrDefault();
        }
        public IEnumerable<TMain> Read<TMain>() where TMain : class, iTable, new()
        {
            System.Data.DataTable dt = ReadDataTableBySql(new TMain().SelectAllScript(paramchar));
            return dt.ToiDBTable<TMain>();
        }
        public IEnumerable<TMain> Read<TMain>(SqlScript dbsqlscript) where TMain : class, iTable, new()
        {
            System.Data.DataTable dt = ReadDataTableBySql(dbsqlscript);
            return dt.ToiDBTable<TMain>();
        }

        public DataTable ReadDataTableBySql(SqlScript dbsqlscript)
        {
            System.Data.DataTable dt = null;
            SQLiteConnection scnn = new SQLiteConnection(ConnString);
            try
            {
                dt = new System.Data.DataTable();
                scnn.Open();
                SQLiteCommand command = new SQLiteCommand(dbsqlscript.sql, scnn);

                if (dbsqlscript.dbparameters != null)
                {
                    foreach (var item in dbsqlscript.dbparameters)
                    {
                        if (item.value is Enum)
                            command.Parameters.Add(new SQLiteParameter(item.name, item.value.ToString()));
                        else
                            command.Parameters.Add(new SQLiteParameter(item.name, item.value));
                    }

                }
                new SQLiteDataAdapter(command).Fill(dt);

            }
            finally
            {
                if (scnn != null)
                {
                    scnn.Close();
                    scnn.Dispose();
                }
            }
            return dt;
        }
        /// <summary>
        /// 讀取數量
        /// </summary>
        /// <param name="dbsqlscript"></param>
        /// <returns></returns>
        public int ReadCount(SqlScript dbsqlscript)
        {
            int Qty = 0;
            using (SQLiteConnection con = new SQLiteConnection(ConnString))
            {
                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(dbsqlscript.sql, con))
                {
                    if (dbsqlscript.dbparameters != null)
                    {
                        foreach (var item in dbsqlscript.dbparameters)
                        {
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter(item.name, item.value is Enum ? item.value.ToString() : item.value));
                        }
                    }
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Qty = reader.GetInt32(0);
                        }
                    }
                }
                con.Close();
            }
            return Qty;
        }
        public IEnumerable<T> ReadHandle<T>(SqlScript dbsqlscript, Func<IDataRecord, T> handle)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnString))
            {
                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(dbsqlscript.sql, con))
                {
                    if (dbsqlscript.dbparameters != null)
                    {
                        foreach (var item in dbsqlscript.dbparameters)
                        {
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter(item.name, item.value is Enum ? item.value.ToString() : item.value));
                        }
                    }
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return handle.Invoke(reader);
                        }
                    }
                }
                con.Close();
            }
        }
        public IEnumerable<IAsyncResult> ReadHandleAsync<T>(SqlScript dbsqlscript, Func<IDataRecord, T> handle)
        {
            using (SQLiteConnection con = new SQLiteConnection(ConnString))
            {
                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(dbsqlscript.sql, con))
                {
                    if (dbsqlscript.dbparameters != null)
                    {
                        foreach (var item in dbsqlscript.dbparameters)
                        {
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter(item.name, item.value is Enum ? item.value.ToString() : item.value));
                        }
                    }
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return handle.BeginInvoke(reader, null, null);
                        }
                    }
                }
                con.Close();
            }
        }

    }
}
