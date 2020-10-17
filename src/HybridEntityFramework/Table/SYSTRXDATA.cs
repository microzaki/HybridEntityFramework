
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace HybridEntityFramework
{
    public class SYSTRXDATA<TSYSID, TBASEDATATYPE> : PK_SYSTRXDATA<TSYSID, TBASEDATATYPE>, iTable, iSysDataTable<TSYSID, TBASEDATATYPE>
    {
        /// <summary>
        /// 設定值
        ///e.g. 192.168.1.2
        /// </summary>
        [DBColumn(IsColumn = true)]
        public string TRXDATAVALUE { get; set; }

        #region iBaseTrxDataTable
        public TBASEDATATYPE DATATYPE { get { return TRXDATATYPE; } }
        public string DATAID { get { return TRXDATAID; } }
        public string DATAVALUE { get { return TRXDATAVALUE; } }
        #endregion

    }
    public class PK_SYSTRXDATA<TSYSID, TTRXDATATYPE> : iPrimaryKey
    {
        /// <summary>
        /// 設定群組
        ///e.g.Central Control
        /// </summary>
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
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
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public TTRXDATATYPE TRXDATATYPE { get; set; }

        /// <summary>
        /// 設定ID
        ///e.g.IP
        /// </summary>
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public string TRXDATAID { get; set; }
    }
    public static partial class DBCtrlExt
    {
        public static List<SqlScript> UpdateSystrxdatas(this DBCtrl db, SYSTRXDATA<eSysID, eTrxDataType> data)
        {
            return db.Update(eInsertUpdateType.Replace, data);
        }

        public static IEnumerable<SYSTRXDATA<eSysID, eTrxDataType>> ReadSystrxdatas(this DBCtrl db, string model = null)
        {
            if (model == null)
            {
                return db.Read<SYSTRXDATA<eSysID, eTrxDataType>>();
            }
            else
            {
                return db.Read<SYSTRXDATA<eSysID, eTrxDataType>>(new SqlScript() { sql = $"SELECT * FROM SYSTRXDATA WHERE MODELNAME IN ('*','{model}')" });
            }

        }
        public static TMain ReadSysTrxData<TMain>(this DBCtrl db, eSysID sysid, string modelname = "*") where TMain : SysData<eSysID>, new()
        {
            TMain vpara = new TMain();
            vpara.sysid = sysid;
            vpara.tablename = nameof(SYSTRXDATA<eSysID, eTrxDataType>);
            vpara.prefix = "TRX";
            db.ReadSystrxdatas().Where(x => x.MODELNAME == modelname).Read<TMain, eSysID, eTrxDataType>(ref vpara);
            return vpara;
        }
    }
}