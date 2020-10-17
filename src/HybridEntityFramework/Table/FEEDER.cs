using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HybridEntityFramework
{
    public class FEEDER : PK_FEEDER, iTable
    {
        [DBColumn(IsColumn = true)]
        public Int64 SCARATRAYNO { get; set; }
    }
    public class PK_FEEDER: iPrimaryKey
    {
        /// <summary>
        /// 設定群組
        ///e.g.Central Control
        /// </summary>
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public Int64 FEEDERNO { get; set; }
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public Int64 GRIPPERNO { get; set; }
    }
}
