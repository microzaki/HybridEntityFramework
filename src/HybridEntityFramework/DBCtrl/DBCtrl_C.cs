using System;
using System.Collections.Generic;

namespace HybridEntityFramework
{
    public partial class DBCtrl
    {
        public List<SqlScript> Create<TMain>(eInsertUpdateType type = eInsertUpdateType.Normal, params TMain[] units) where TMain : class, iTable, new()
        {
            List<SqlScript> sqls = new List<SqlScript>();
            foreach (var item in units)
            {
                sqls.AddRange(item.InsertScript(paramchar, randomidcharacter, type));
            }
            return sqls;
        }
    }
}