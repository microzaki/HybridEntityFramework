using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public static class UShortExt
    {

        /// <summary>
        /// ushort array to uint23
        /// </summary>
        /// <param name="int16s"></param>
        /// <returns></returns>
        public static IEnumerable<int> ToInt32(this IEnumerable<ushort> int16s)
        {
            for (int i = 1; i <= int16s.Count(); i = i + 2)
            {
                yield return (int)((int16s.ToList().ElementAt(i-1) << 16) | int16s.ToList().ElementAt(i));
            }
        }
        /// <summary>
        /// ushort array to double
        /// </summary>
        /// <param name="int16s"></param>
        /// <returns></returns>
        public static IEnumerable<double> ToDouble(this IEnumerable<ushort> int16s)
        {
            for (int i = 1; i <= int16s.Count(); i = i + 2)
            {
               yield return ((int16s.ToList().ElementAt(i - 1) << 16) | int16s.ToList().ElementAt(i)) / 1000;
            }
        }
        /// <summary>
        /// ushort array to byte(default : low byte,high byte)
        /// </summary>
        /// <param name="int16s"></param>
        /// <returns></returns>
        public static IEnumerable<byte> ToByte(this IEnumerable<ushort> int16s,bool reverse=true)
        {
            List<byte> rett = new List<byte>();
            foreach (var item in int16s)
            {
                if (reverse)
                    //High Byte,Low Byte,High Byte,Low Byte
                    rett.AddRange(BitConverter.GetBytes(item).Reverse());
                else
                    //Low Byte,High Byte,Low Byte,High Byte
                    rett.AddRange(BitConverter.GetBytes(item));
            }
            return rett;
        }
        /// <summary>
        /// ushort array to string 
        /// </summary>
        /// <param name="int16s"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public static string ToASCII(this IEnumerable<ushort> int16s, bool reverse = true)
        {
            return Encoding.ASCII.GetString(int16s.ToByte(reverse).ToArray());
        }
        public static ushort Next(this ushort data, ushort MaxValue = ushort.MaxValue )
        {
            return data == MaxValue ? ushort.MinValue : (ushort)(data + 1);
        }
    }
}
