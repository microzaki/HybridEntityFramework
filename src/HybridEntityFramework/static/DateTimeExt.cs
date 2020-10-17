using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public static class DateTimeExt
    {
        public static string ToSerialNo(this DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmssff");
        }
        public static DateTime ChangeTime(this DateTime dt,DateTime time)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, time.Hour, time.Minute, time.Second);
        }
    }
}
