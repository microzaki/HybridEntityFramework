using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public enum eParameterUnit
    {
        NA=0,
        Million_Second,
        Second,
        Pulse,
        Pulse_mm,
        mm,
    }
    public static class eParameterUnitExt
    {
        public static string ToFriendlyString(this eParameterUnit me)
        {
            switch (me)
            {
                case eParameterUnit.NA:
                    return "";
                case eParameterUnit.Million_Second:
                    return "ms";
                case eParameterUnit.Second:
                    return "sec";
                case eParameterUnit.Pulse:
                    return "Pulse";
                case eParameterUnit.Pulse_mm:
                    return "Pulse/mm";
                case eParameterUnit.mm:
                    return "mm";
                default:
                    return "";
            }
        }
    }
}
