using System;
using System.Collections.Generic;

namespace HybridEntityFramework
{
    public partial class DBCtrl
    {
        public List<SqlScript> Delete<TMain>(params TMain[] units) where TMain : class, iPrimaryKey, new()
        {
            List<SqlScript> sqls = new List<SqlScript>();
            foreach (var item in units)
            {
                sqls.AddRange(item.DeleteByPKScript(paramchar));
            }
            return sqls;
        }

    }
}