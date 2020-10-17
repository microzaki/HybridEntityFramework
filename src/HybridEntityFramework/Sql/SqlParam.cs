using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// 參數型式的SQL script，所使用的通用格式
    /// </summary>
    public class SqlParam
    {
        public SqlParam(string name, object value)
        {
            this.name = name;
            if (value.GetType().IsEnum)
                this.value = value.ToString();
            else
                this.value = value;
        }
        /// <summary>
        /// 參數名稱
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 參數值
        /// </summary>
        public object value { get; private set; }
    }
}
