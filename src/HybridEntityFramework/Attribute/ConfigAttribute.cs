using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public class ConfigAttribute : System.Attribute
    {
        /// <summary>
        /// 當由DB或text file去修改預設值時
        /// 可以藉由此欄位知道是否為可供設定之欄位
        /// 避免程式發生錯誤
        /// </summary>
        public bool IsSettingColumn = false;
        /// <summary>
        /// 當由DB或text file去取值時
        /// 如果取不到值自動生成Default值
        /// </summary>
        public bool DefaultIfNoData = false;
        public eParameterUnit Unit;
        public string Desc;

    }
    public class ConfigIntAttribute : System.Attribute
    {
        public int MaxVale;
        public int MinValue;
    }
    public class ConfigFloatAttribute : System.Attribute
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public float MaxVale;
        /// <summary>
        /// 最小值
        /// </summary>
        public float MinValue;
    }
    public class ConfigNumAttribute : System.Attribute
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public float MaxValue;
        /// <summary>
        /// 最小值
        /// </summary>
        public float MinValue;
    }
    /// <summary>
    /// 陣列如果數量已知，可藉此避免設定錯誤
    /// </summary>
    public class ConfigArrayAttribute : System.Attribute
    {
        /// <summary>
        /// Array內容數
        /// </summary>
        public int[] Count;
    }
    public static class ConfigExt
    {
        public static bool ArrayCountCorrect(this PropertyInfo pr,int count,out int correctcount,int index=0)
        {
            correctcount = 0;
            if (pr.GetCustomAttribute<ConfigArrayAttribute>() != null
                && pr.GetCustomAttribute<ConfigArrayAttribute>().Count.Length > index
                && count != pr.GetCustomAttribute<ConfigArrayAttribute>().Count[index])
            {
                
                correctcount = pr.GetCustomAttribute<ConfigArrayAttribute>().Count[index];
                return false;
            }
               
            else
                return true;
        }
        public static bool MinCorrect(this PropertyInfo pr,float ret,out  float min)
        {
            min = 0;
            if (pr.GetCustomAttribute<ConfigNumAttribute>() != null 
                && ret < pr.GetCustomAttribute<ConfigNumAttribute>().MinValue)
            {
                min = (float)pr.GetCustomAttribute<ConfigNumAttribute>().MinValue;
                return false;
            }
            return true;
        }
        public static bool MaxCorrect(this PropertyInfo pr, float ret, out float max)
        {
            max = 0;
            if (pr.GetCustomAttribute<ConfigNumAttribute>() != null
                && ret >pr.GetCustomAttribute<ConfigNumAttribute>().MaxValue)
            {
                max = (float)pr.GetCustomAttribute<ConfigNumAttribute>().MaxValue;
                return false;
            }
            return true;
        }
        public static IEnumerable<PropertyInfo> GetSettingColumn(this IEnumerable<PropertyInfo> props)
        {
            return props.Where(x => x.GetCustomAttribute<ConfigAttribute>() != null && x.GetCustomAttribute<ConfigAttribute>().IsSettingColumn);
        }
        public static IEnumerable<PropertyInfo> GetSettingColumns(this Type type)
        {
            return type.GetProperties().GetSettingColumn();
        }
    }
}
