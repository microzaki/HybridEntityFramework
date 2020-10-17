
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.IO;

namespace HybridEntityFramework
{
    /// <summary>
    /// Base Data Table
    /// </summary>
    /// <typeparam name="TSYSID">系統代碼</typeparam>
    /// <typeparam name="TBASEDATATYPE">基本資料類型</typeparam>
    public class SYSBASEDATA<TSYSID, TBASEDATATYPE> : PK_SYSBASEDATA<TSYSID, TBASEDATATYPE>,iTable, iSysDataTable<TSYSID, TBASEDATATYPE>
    {
        /// <summary>
        /// 設定值
        ///e.g. 192.168.1.2
        /// </summary>
        [DBColumn(IsColumn = true)]
        public string BASEDATAVALUE { get; set; }

        #region iBaseTrxDataTable
        public TBASEDATATYPE DATATYPE { get { return BASEDATATYPE; } }
        public string DATAID { get { return BASEDATAID; } }
        public string DATAVALUE { get { return BASEDATAVALUE; } }
        #endregion
    }
    /// <summary>
    /// Base Data Table Primary Key
    /// </summary>
    /// <typeparam name="TSYSID">系統代碼</typeparam>
    /// <typeparam name="TBASEDATATYPE">基本資料類型</typeparam>
    public class PK_SYSBASEDATA<TSYSID, TBASEDATATYPE> : iPrimaryKey
    {
        /// <summary>
        /// 設定群組
        ///e.g.Central Control
        /// </summary>
        [DBColumn(IsPrimaryKey =true,IsColumn =true)]
        public TSYSID SYSID { get; set; }
        /// <summary>
        /// 設定群組
        ///e.g.Central Control
        /// </summary>
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public string MODELNAME { get; set; }
        /// <summary>
        /// 設定類型
        ///e.g.ModbusMasterConfigs: Modbus Master連線設定
        /// </summary>
        [DBColumn(IsPrimaryKey = true,IsColumn =true)]
        public TBASEDATATYPE BASEDATATYPE { get; set; }

        /// <summary>
        /// 設定代碼
        ///e.g.IP
        /// </summary>
        [DBColumn(IsPrimaryKey = true,IsColumn =true)]
        public string BASEDATAID { get; set; }
    }
    public static partial class DBCtrlExt
    {
        public static IEnumerable<SqlScript> UpdateSysBaseData(this DBCtrl db, SYSBASEDATA<eSysID, eBaseDataType> data)
        {
            return db.Update(eInsertUpdateType.Replace, data);
        }
        public static TMain ReadSysBaseData<TMain>(this DBCtrl db,eSysID configgroup) where TMain : iSysData<eSysID>, new()
        {
            TMain vpara = new TMain();
            vpara.sysid = configgroup;
            vpara.tablename = nameof(SYSBASEDATA<eSysID, eBaseDataType>);
            vpara.prefix = "BASE";
            SqlScript sql;
            if (!typeof(TMain).Name.Contains("Model"))
            {
                 sql = new SqlScript($@"
SELECT * 
FROM SYSBASEDATA 
WHERE MODELNAME='*' AND SYSID='{configgroup.ToString()}' AND BASEDATATYPE='{typeof(TMain).Name}'");
            }
            else
            {
                 sql = new SqlScript($@"
SELECT * 
FROM SYSBASEDATA A
    JOIN SYSBASEDATA B ON B.SYSID='Main' AND B.BASEDATATYPE='SystemParameter' AND B.BASEDATAID='CurrentSelectModeName' AND A.MODELNAME=B.BASEDATAVALUE
WHERE  A.SYSID='{configgroup.ToString()}' AND A.BASEDATATYPE='{typeof(TMain).Name}'");
            }
            IEnumerable<SYSBASEDATA<eSysID, eBaseDataType>> data = db.Read<SYSBASEDATA<eSysID, eBaseDataType>>(sql);
            if (data.Count() == 0)
                return vpara;
            data.Read<TMain, eSysID, eBaseDataType>(ref vpara);
            return vpara;
        }

        public static Dictionary<eSysID, List<eBaseDataType>> dicSysIDBaseDataType(this DBCtrl db)
        {
            Dictionary<eSysID, List<eBaseDataType>>  _dicSysIDBaseDataType = new Dictionary<eSysID, List<eBaseDataType>>();
            foreach (eSysID enumType in Enum.GetValues(typeof(eSysID)))
            {
                _dicSysIDBaseDataType.Add(enumType, new List<eBaseDataType>());
                Array ary = Enum.GetValues(typeof(eBaseDataType));
                Array.Sort(ary);
                foreach (eBaseDataType item in ary)
                {
                    if (db.Read<SYSBASEDATA<eSysID, eBaseDataType>>()
                        .Where(x => x.SYSID.Equals(enumType))
                        .Where(x => x.BASEDATATYPE.Equals(item)).Count() > 0)
                    {
                        _dicSysIDBaseDataType[enumType].Add(item);
                    }
                }
                _dicSysIDBaseDataType[enumType].Sort(
                    (eBaseDataType x, eBaseDataType y) =>
                    {
                        return string.Compare(x.ToString(), y.ToString(), true);
                    });


            }
            return _dicSysIDBaseDataType;
        }
    }
}
