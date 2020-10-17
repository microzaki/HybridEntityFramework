
using System;
using System.Collections.Generic;
using System.Linq;

namespace HybridEntityFramework
{
    public class PART : PK_PART, iTable
    {
        #region TRAY
        [DBColumn(IsColumn = true)]
        public Int64 MINSTOCK { get; set; } = 36;
        [DBColumn(IsColumn = true)]
        public Int64 XNUM { get; set; } = 6;
        [DBColumn(IsColumn = true)]
        public Int64 YNUM { get; set; } = 6;

        Int64? _TotalCount;
        public Int64 TotalCount
        {
            get
            {
                if (_TotalCount == null)
                    _TotalCount = XNUM * YNUM;
                return (Int64)_TotalCount;
            }
        }
        #endregion

        /// <summary>
        /// Pin腳數量
        /// </summary>
        [DBColumn(IsColumn = true)]
        public Int64 PINNUMBER { get; set; } = 4;

        /// <summary>
        /// 零件高度
        /// </summary>
        [DBColumn(IsColumn = true)]
        public decimal PINHEIGHT { get; set; } = 10M;
        /// <summary>
        /// 插件深度
        /// </summary>
        [DBColumn(IsColumn = true)]
        public decimal PLUGINDEPTH { get; set; } = 2.41M;

        /// <summary>
        /// 計算的插件公差
        /// </summary>
        [DBColumn(IsColumn = true)]
        public decimal CALCULATETOLERANCE { get; set; } = 0.3M;

        //[DBColumn(IsColumn = true)]
        //public string TEMPLATEPATH { get; set; } = "";

        //[DBColumn(IsColumn = true)]
        //public string ROITEMPLATE { get; set; } = "123,123,12;";
        //double[,] _roiTemplate;
        //public double[,] roiTemplate
        //{
        //    get
        //    {
        //        if (_roiTemplate == null)
        //        {
        //            _roiTemplate = ROITEMPLATE.ToMatrix<double>();
        //        }
        //        return _roiTemplate;
        //    }
        //}

        ///// <summary>
        ///// 找 Pin 時調整亮度的比例參數
        ///// </summary>
        //[DBColumn(IsColumn = true)]
        //public double GRAYVALUEADJUST { get; set; } = 0.8;

        ///// <summary>
        ///// 二值化的取值範圍
        ///// </summary>
        //[DBColumn(IsColumn = true)]
        //public string THRESHOLDRANGE { get; set; } = "50,255;";
        //uint[] _thresholdRange;
        //public uint[] thresholdRange
        //{
        //    get
        //    {
        //        if (_thresholdRange == null)
        //        {
        //            uint[,] r = THRESHOLDRANGE.ToMatrix<uint>();
        //            _thresholdRange = new uint[2] { r[0, 0], r[0, 1] };
        //        }
        //        return _thresholdRange;
        //    }
        //}

        ///// <summary>
        ///// 挑Pin時的面積範圍
        ///// </summary>
        //[DBColumn(IsColumn = true)]
        //public string AREARANGE { get; set; } = "20,99999;";
        //uint[] _areaRange;
        //public uint[] areaRange
        //{
        //    get
        //    {
        //        if (_areaRange == null)
        //        {
        //            uint[,] r = AREARANGE.ToMatrix<uint>();
        //            _areaRange = new uint[2] { r[0, 0], r[0, 1] };
        //        }
        //        return _areaRange;
        //    }
        //}
    }
    public class PK_PART : iPrimaryKey
    {
        /// <summary>
        /// 設定群組
        ///e.g.Central Control
        /// </summary>
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public string PARTNAME { get; set; }
    }

    public static partial class DBCtrlExt
    {
        public static List<PART> parts(this DBCtrl db)
        {
            return db.Read<PART>().ToList();
        }
        public static PART ReadPART(this DBCtrl db, string partname)
        {
            return db.parts().Where(x => x.PARTNAME == partname).FirstOrDefault();
        }
        public static List<SqlScript> DeletePART(this DBCtrl db, PART part)
        {
            return db.Delete(part);
        }
        public static List<SqlScript> CreatePART(this DBCtrl db, PART part)
        {
            return db.Create(eInsertUpdateType.Normal, part);
        }
        public static List<SqlScript> UpdatePART(this DBCtrl db, PART part)
        {
            return db.Update(part);
        }
    }
}
