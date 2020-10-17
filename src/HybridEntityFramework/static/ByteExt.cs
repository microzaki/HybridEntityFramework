using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public static class ByteExt
    {
        /// <summary>
        /// Low Byte,High Byte, inverse=true High byte Low byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort ToUInt16(this (byte lowbyte,byte highbyte) value)
        {
            return BitConverter.ToUInt16(new byte[2] { value.lowbyte, value.highbyte }, 0);
        }
        /// <summary>
        /// Low Byte,High Byte , inverse=true High byte Low byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort ToUInt16(this byte[] value, int lowbyteindex = 0, bool inverse = false)
        {
            if (inverse)
                return  (value[lowbyteindex+1], value[lowbyteindex ]).ToUInt16();
            else
               return BitConverter.ToUInt16(value, lowbyteindex);
        }
    }
}
