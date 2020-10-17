 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using System.Reflection;

namespace HybridEntityFramework
{
    public partial class DBCtrl 
    {

        public List<SqlScript> Update<TMain>(params TMain[] units) where TMain : class, iTable, new()
        {
            List<SqlScript> sqls = new List<SqlScript>();
            foreach (var item in units)
            {
                sqls.AddRange(item.UpdateByPKScript(paramchar));
            }
            return sqls;
        }
        public List<SqlScript> Update<TMain>(eInsertUpdateType type = eInsertUpdateType.Normal, params TMain[] units) where TMain : class, iTable, new()
        {
            if (type!=eInsertUpdateType.Normal)
                return Create(type, units);
            return Update<TMain>(units);
        }
    }
}
