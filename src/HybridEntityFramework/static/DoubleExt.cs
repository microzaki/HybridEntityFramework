using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HybridEntityFramework
{
    public static partial class DoubleExt
    {
        public static double Max(this double val, params double[] vals)
        {
            double max = val;
            foreach (var item in vals)
            {
                max = Math.Max(item, max);
            }
            return max;
        }

        /// <summary>
        /// 轉成MODBUS 的 double word
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort[] ToModbusDW(this double value)
        {
            ushort[] rets = new ushort[2];
            int modbosvalue = (int)(Math.Round(value, 3) * 1000);
            rets[0]=(ushort)(modbosvalue & 0xFFFF);
            rets[1] = (ushort)(modbosvalue >> 16);
            return rets;
        }
        /// <summary>
        /// 轉成MODBUS 的 double word
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<ushort> ToModbusDW(this IEnumerable<double> values)
        {
            List<ushort> rets = new List<ushort>();
            foreach (var item in values)
            {
                rets.AddRange(item.ToModbusDW());
            }
            return rets;
        }
        /// <summary>
        /// 角度-->徑度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToRadian(this double value)
        {
            return ((double)value / (double)180) * Math.PI;
        }
        /// <summary>
        /// 徑度-->角度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDegree(this double value)
        {
            return value * (180.0 / Math.PI);
        }
        /// <summary>
        /// double array --> string
        /// </summary>
        /// <param name="values"></param>
        /// <param name="firstsplit"></param>
        /// <param name="secondsplit"></param>
        /// <returns></returns>
        public static string ToString(this double[,] values,string firstsplit,string secondsplit)
        {
            string ret=string.Empty;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                if (string.IsNullOrEmpty(ret) == false)
                    ret = $"{ret};";
                string ext=string.Empty;
                for (int j = 0; j < values.GetLength(1); j++)
                {
                   if (string.IsNullOrEmpty(ext)==false)
                   {
                        ext= $"{ext},";
                   }
                    ext= $"{ext}{values[i,j]}";
                }
                ret = $"{ret}{ext}";
            }
            return ret;
        }


        public static string ToNumpyArrrayString(this IEnumerable<double> dous)
        {
            string ret = $"{dous.Count().ToString()};";
            foreach (var item in dous)
            {
                ret = $"{ret}{item.ToString()},";
            }
            if (ret.Last().Equals(','))
                ret=ret.Remove(ret.Count()-1);
            return ret;
        }
        public static double[,] ToTwoDim(this IEnumerable<double> dous)
        {
            List<double> r = dous.ToList();
            double[,] ret = new double[r.Count(), 1];
            for (int i = 0; i < ret.GetLength(0); i++)
            {
                ret[i, 0] = r[i];
            }
            return ret;
        }
    }
}
