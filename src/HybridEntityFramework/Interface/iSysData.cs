using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// SYSBASEDATA SYSTRXDATA的資料interface
    /// </summary>
    /// <typeparam name="TSYDID"></typeparam>
    public interface iSysData<TSYDID>
    {
         TSYDID sysid { get; set; }
        string prefix { get; set; }
        string tablename { get; set; }
    }
}
