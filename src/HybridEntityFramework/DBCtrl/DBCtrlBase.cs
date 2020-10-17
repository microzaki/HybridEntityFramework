using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HybridEntityFramework
{
    /// <summary>
    /// 資料庫物件基底類別
    /// </summary>
    /// <typeparam name="TSysID"></typeparam>
    /// <typeparam name="TBaseDataType"></typeparam>
    /// <typeparam name="TTrxDataType"></typeparam>
    public partial class DBCtrlBase<TSysID, TBaseDataType, TTrxDataType>
        where TSysID : Enum
        where TBaseDataType : Enum
        where TTrxDataType : Enum
    {
        #region initial
        public DBCtrlBase(FileInfo fi)
            :this(fi.FullName)
        {

        }
        public DBCtrlBase(string filename)
        {

        }
        #endregion


    }
}
