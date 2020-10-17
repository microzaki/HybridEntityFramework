using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public class HWSETTING : PK_HWSETTING, iTable
    {

        [DBColumn(IsColumn = true)]
        public string SETTINGVALUE { get; set; }
     
    }
    public class PK_HWSETTING : iPrimaryKey
    {
        [DBColumn(IsColumn = true)]
        public string SYSTEM { get; set; }
        [DBColumn(IsColumn = true)]
        public string MODULE { get; set; }
        [DBColumn(IsColumn = true)]
        public string COMPONENT { get; set; }
        [DBColumn(IsColumn = true)]
        public string SETTINGNAME { get; set; }
      
        [DBColumn(IsColumn = true)]
        public string CUSTNAME{ get; set; }
    }

    public static partial class DBCtrlExt
    {
        public static List<SqlScript> UpdateHWSETTING(this DBCtrl db, IEnumerable<HWSETTING> settings)
        {
            List<SqlScript> sqls = new List<SqlScript>();
            foreach (var item in settings)
            {
                sqls.AddRange(db.Update(eInsertUpdateType.Replace, item));
            }
            return sqls;
        }
        public static List<SqlScript> UpdateHWSETTING(this DBCtrl db, HWSETTING HWSetting)
        {
            return db.Update(eInsertUpdateType.Replace, HWSetting);
        }
        public static List<SqlScript> DeleteHWSETTING(this DBCtrl db, PK_HWSETTING pk)
        {
            List<SqlScript> sqls = new List<SqlScript>();
          //  sqls.AddRange(db.Delete(pk));
            sqls.Add(new SqlScript() { sql = $"DELETE FROM HW_SETTING WHERE SYSTEM='{pk.SYSTEM}' AND MODULE='{pk.MODULE}' AND CUSTNAME='{pk.CUSTNAME}'" });
         
            return sqls;
            // return db.Delete(pk);
        }
        public static List<HWSETTING> ReadHWSETTING(this DBCtrl db)
        {
            return db.Read<HWSETTING>().ToList();
        }
        #region HWSetting
        public static List<SqlScript> CreateHWSETTING(this DBCtrl db ,HWSETTING pk)
        {
            return db.Create(eInsertUpdateType.Replace, pk);
        }
        #endregion
    }
}
