using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Data.SQLite;

namespace HybridEntityFramework
{
   public partial  class DBCtrl 
    {
        public DBCtrl()
        {
            this.ConnString = $"Data Source={nameof(DBCtrl) + ".db"};Version=3;UseUTF16Encoding=True;";
            this.commit = (sqlscripts) => commit(sqlscripts.ToArray());

            void commit(params SqlScript[] sqlscripts)
            {
                SQLiteConnection sqlliteconn = new SQLiteConnection(ConnString);
                SQLiteTransaction litetrx;

                using (sqlliteconn = new SQLiteConnection(ConnString))
                {
                    sqlliteconn.Open();
                    using (litetrx = sqlliteconn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in sqlscripts)
                            {
                                SQLiteCommand command = new SQLiteCommand(item.sql, sqlliteconn, litetrx);
                                command.Parameters.AddRange(item.sqliteparameters.ToArray());
                                command.ExecuteNonQuery();
                            }
                            litetrx.Commit();
                        }
                        catch (Exception)
                        {
                            litetrx.Rollback();
                            throw;
                        }
                    }
                }
            }
        }


        #region parameter
        public const string randomidcharacter = "RandomID";
        public const string paramchar = "@";
        readonly string ConnString;
        #endregion

        #region commit
        public Action<IEnumerable<SqlScript>> commit { get; private set; }
        #endregion


    }
}
