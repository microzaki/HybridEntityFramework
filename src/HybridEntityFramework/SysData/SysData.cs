using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// System Data(基本資料)的基底類別 (from SYSBASEDATA SYSTRXDATA)
    /// </summary>
    /// <typeparam name="TSYDID"></typeparam>
    [Serializable]
    public class SysData<TSYDID>:iSysData<TSYDID>
    {
        public TSYDID sysid { get; set; }
        public string prefix { get; set; }
        public string tablename { get; set; }
    }
    

}
