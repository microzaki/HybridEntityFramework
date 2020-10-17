using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public class MODELFEEDER : PK_MODELFEEDER, iTable
    {
        [DBColumn(IsColumn = true)]
        public string PARTNAME { get; set; }

    }
    public class PK_MODELFEEDER : iPrimaryKey
    {
        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public string MODELNAME { get; set; }

        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public string ACTIONNO { get; set; }

        [DBColumn(IsPrimaryKey = true, IsColumn = true)]
        public long FEEDERNO { get; set; }
    }
}
