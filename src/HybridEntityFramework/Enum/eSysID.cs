using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HybridEntityFramework
{
    /// <summary>
    /// Config Group
    /// </summary>
    public enum eSysID
    {
        Main,
        Alarm,
        Conveyor,
        Insert,
        Calibration,
        Screw,
        Dispenser,
        Feeder,
    }
    public static class eSysIDExt
    {
        public static  List<eBaseDataType> BaseDataTypes(this eSysID sysid)
        {
            switch (sysid)
            {
                case eSysID.Main:
                    return new List<eBaseDataType>() { eBaseDataType.LogParameter, eBaseDataType.SystemParameter };
                case eSysID.Alarm:
                    return new List<eBaseDataType>() { eBaseDataType.AlarmParameter };
                case eSysID.Conveyor:
                    return new List<eBaseDataType>() { eBaseDataType.ConveyorParameter, eBaseDataType.ModelConveyorParameter };
                case eSysID.Insert:
                    return new List<eBaseDataType>() { eBaseDataType.InsertParameter, eBaseDataType.ModelInsertParameter };
                default:
                    return null;
            }
        }
    }

}
