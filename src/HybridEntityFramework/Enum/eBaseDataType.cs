using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    /// <summary>
    /// Config Type
    /// </summary>
    public enum eBaseDataType
    {
        #region Main
        LogParameter,
        SystemParameter,
        #endregion

        #region Feeder
        FeederParameter,
        ModelFeederParameter,
        #endregion

        #region Alarm
        AlarmParameter,
        #endregion

        #region Coveyor
        ConveyorParameter,
        ModelConveyorParameter,

        ConveyorModuleParameter,
        #endregion

        #region Insert 
        InsertParameter,
        ModelInsertParameter,

        InsertModuleParameter,

        #region Calibration
        CalibrationParameter_LowerCam,
        CalibrationParameter_TCP,
        CalibrationParameter_UpperCam,
        CalibrationParameter_WorkPlane,
        CalibrationParameter_CpntTemp,
        CalibrationParameter_PCBTemp,

        RobotParameter,
        ToolCalibratorParameter,
        FilePathParameter,
        #endregion
        #endregion

        DeltaSCARAParameter,
    }
}
