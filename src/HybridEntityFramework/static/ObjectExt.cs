using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;


namespace HybridEntityFramework
{
    public static partial class ObjectExt
    {
        /// <summary>
        /// object array to Type array
        /// </summary>
        /// <param name="items"></param>
        /// <param name="type"></param>
        /// <param name="performConversion"></param>
        /// <returns></returns>
        public static object ConvertArray(this object[] items, Type type, bool performConversion = false)
        {
            var containedType = type;
            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast)).MakeGenericMethod(containedType);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToArray)).MakeGenericMethod(containedType);

            IEnumerable<object> itemsToCast;

            if (performConversion)
            {
                itemsToCast = items.Select(item => Convert.ChangeType(item, containedType));
            }
            else
            {
                itemsToCast = items;
            }

            var castedItems = castMethod.Invoke(null, new[] { itemsToCast });

            return toListMethod.Invoke(null, new[] { castedItems });
        }
        public static string GetCallMeStackTrace(this object obj)
        {
            // 2:省略目前位置與呼叫目前Function的位置
            // true:顯示檔案資訊
            var stackTrace = new StackTrace(2, true);
            var target = stackTrace.GetFrame(0);
            return new StackTrace(target).ToString();
        }
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
           
        }
        public static IEnumerable<string> ToJsons(this IEnumerable<object> objs)
        {
            foreach (var item in objs)
            {
                yield return item.ToJson();
            }
        }
        public static string ToJsonCSV(this IEnumerable<object> objs)
        {
            if (objs.Count() == 0) return "";
            string ret = "";
            foreach (var item in objs.ToJsons())
            {
                ret += "," + item.Replace(",", " ").Replace("[", "").Replace("]", "").Replace(@"""", "");
            }
            ret = ret.Remove(0, 1);
            return ret;
        }

        public static double[,] To_T_ToolWr(this object obj,int dim)
        {
            if (dim > 1)
                throw new Exception("dim must be under 1");
            if (obj.GetType() == typeof(double[,]))
            {
                double[,] a = (double[,])obj;
                double[,] ret = new double[4, 4];
                if (dim==1)
                {
                    ret[0, 0] = 1; ret[0, 1] = 0; ret[0, 2] = 0; ret[0, 3] = a[0, 0];
                    ret[1, 0] = 0; ret[1, 1] = 1; ret[1, 2] = 0; ret[1, 3] = a[0, 1];
                    ret[2, 0] = 0; ret[2, 1] = 0; ret[2, 2] = 1; ret[2, 3] = a[0, 2];
                    ret[3, 0] = 0; ret[3, 1] = 0; ret[3, 2] = 0; ret[3, 3] = 1;
                }
                else
                {
                    ret[0, 0] = 1; ret[0, 1] = 0; ret[0, 2] = 0; ret[0, 3] = a[0, 0];
                    ret[1, 0] = 0; ret[1, 1] = 1; ret[1, 2] = 0; ret[1, 3] = a[1, 0];
                    ret[2, 0] = 0; ret[2, 1] = 0; ret[2, 2] = 1; ret[2, 3] = a[2, 0];
                    ret[3, 0] = 0; ret[3, 1] = 0; ret[3, 2] = 0; ret[3, 3] = 1;
                }
                return ret;
            }
            else
            {
                throw new Exception($"Type:{obj.GetType().Name} isn't defined for Tool Matrix Convert");
            }


        }

        public static string ToArrayString(this object obj)
        {
            string[] str = null;
            string val = "";
            if (obj == null)
            {
                // 資料如果是null,則設為空字串
                str = new string[1] { "" };
            }
            else if (obj.GetType().GetArrayRank() == 1)//一維
            {
                str = ((System.Collections.IEnumerable)obj)
                    .Cast<object>()
                    .Select(x => x.ToString())
                    .ToArray();
                
                foreach (string itemval in str)
                {
                    val = $"{val},{itemval}";
                }
                val = val.Remove(0, 1);
            }
            else if (obj.GetType().GetArrayRank() == 2)//二維
            {
                var strArray = (Array)obj;
                //string[,] stringArray = new string[strArray.GetLength(0), strArray.GetLength(1)];
                string result = "";
               for(int i=0;i< strArray.GetLength(0); i++)
                {
                    if (i > 0)
                    {
                        result = result + ";";
                    }
                    for (int j = 0; j < strArray.GetLength(1); j++)
                    {
                        if (j == strArray.GetLength(1) - 1)
                            result += strArray.GetValue(i, j).ToString();
                        else 
                            result += strArray.GetValue(i, j).ToString() + ",";
                        
                       // stringArray[i,j] = strArray.GetValue(i,j).ToString();
                    }
                }
                #region old code
                //var results = string.Join(",",
                //     Enumerable.Range(0, stringArray.GetUpperBound(0) + 1)
                //     .Select(x => Enumerable.Range(0, stringArray.GetUpperBound(1) + 1)
                //     .Select(y => stringArray[x, y]))
                //     .Select(z => "{" + string.Join(",", z) + "}"));

                //val = results.Replace("{",string.Empty).Replace("},",";").Replace("}",";");
                #endregion
                val = result + ";";
            }
            return val;
        }
        public static string ToArrayInt(this object obj)
        {
            int[] str = null;
            if (obj == null)
            {
                // 資料如果是null,則設為空字串
                str = new int[1] { 0 };
            }
            else
            {
                str = ((System.Collections.IEnumerable)obj)
                    .Cast<object>()
                    .Select(x => (int)x)
                    .ToArray();
            }
            string val = "";
            foreach (int itemval in str)
            {
                val = $"{val},{itemval.ToString()}";
            }
            val = val.Remove(0, 1);
            return val;
        }
    }
}
