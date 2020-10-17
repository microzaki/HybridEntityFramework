using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public class MODEL : PK_MODEL, iTable
    {

    }
    public class PK_MODEL : iPrimaryKey
    {
        [DBColumn(IsPrimaryKey =true, IsColumn = true)]
        public string MODELNAME { get; set; }
    }
    public static partial class DBCtrlExt
    {
        public static List<MODEL> MODELs(this DBCtrl db)
        {
            return db.Read<MODEL>().ToList();
        }
        public static List<SqlScript> CreateMODEL(this DBCtrl db,MODEL pk)
        {
             return db.Create<MODEL>(eInsertUpdateType.Replace, pk);
        }
        public static List<SqlScript>  DeleteMODEL(this DBCtrl db, PK_MODEL pk)
        {
            List<SqlScript> sqls = new List<SqlScript>();
            sqls.AddRange(db.Delete(pk));
            sqls.Add(new SqlScript() { sql = $"DELETE FROM MODELINPUT WHERE MODELNAME='{pk.MODELNAME}'" });
            sqls.Add(new SqlScript() { sql = $"DELETE FROM SYSBASEDATA WHERE MODELNAME='{pk.MODELNAME}'" });
            return sqls;
        }
    }
}
