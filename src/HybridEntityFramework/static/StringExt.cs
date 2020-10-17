using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Linq.Expressions;
namespace HybridEntityFramework
{
    public static partial class StringExt
    {
        public static string IsLetter(this string str, params Char[] Extra)
        {
            return new String(str.Where(x => Char.IsLetter(x) || Extra.Contains(x)).ToArray());
        }
        public static string Parse(string str)
        {
            return str;
        }
        public static object ToType(this string str,Type type)
        {
            if (type == typeof(double[,]))
            {
                return (object)str.ToMatrix<double>();
            }
            else if (type == typeof(FileInfo))
            {
                return (object)new FileInfo(str);
            }
            else if (type == typeof(string))
            {
                return str;
            }
            if (type.IsArray == false)
            {
                MethodInfo[] mi = type.GetMethods().Where(x => x.GetParameters().Count() == 1
                    && x.GetParameters()[0].ParameterType == typeof(string)
                    && x.ReturnType == type).ToArray();
                if (mi.Count()==0)
                    throw new Exception($"{type.FullName} must have method with string input and self type output");
                if (mi.Length > 1)
                    mi = mi.Where(x => x.Name == "Parse").ToArray();
                if (mi.Length==0)
                    throw new Exception($"{type.FullName} has duplicated method with string input and self type output, please write one [Parse] Method");
                return mi.FirstOrDefault().Invoke(null, new object[] { str });
            }
            else if (type.IsArray)
            {
                MethodInfo  mi = type.GetElementType().GetMethods().Where(x => x.GetParameters().Count() == 1
                    && x.GetParameters()[0].ParameterType == typeof(string)
                    && x.ReturnType == type.GetElementType()).FirstOrDefault();
                if (mi == null)
                    throw new Exception($"{type.GetElementType().FullName} must have method with string input and self type output");
                List<object> objs = new List<object>();
                string[] strs;
                if (str.Contains(";"))
                    strs = str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                else
                    strs = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in strs)
                {
                    objs.Add(item.ToType(type.GetElementType()));
                }
                return objs.ToArray().ConvertArray(type.GetElementType());
            }
            else
                throw new Exception($"Type: {type} isn't defined!");
        }
        
        public static T[] ToArray<T>(this string str,char cha= ',')
        {
            string[] s=str.Split(new char[] { cha });
            T[] ret = new T[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                ret[i] = (T)Convert.ChangeType(s[i], typeof(T));
            }
            return ret;
        }
        /// <summary>
        /// string to matrix by spliter
        /// </summary>
        /// <typeparam name="T">matrix type</typeparam>
        /// <param name="str"></param>
        /// <param name="firstsep">first separator</param>
        /// <param name="secsep">second separator</param>
        /// <returns></returns>
        public static T[,] ToMatrix<T>(this string str, string firstsep= ";", string secsep= ",")
        {
            T[,] ret = new T[0, 0];
            string[] split1 = str.Split(new string[] { firstsep }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < split1.Length; i++)
            {
                string[] split2 = split1[i].Split(new string[] { secsep }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < split2.Length; j++)
                {
                    if (i == 0 && j == 0)
                        ret = new T[split1.Length, split2.Length];
                    ret[i, j] = (T)Convert.ChangeType(split2[j].Replace(" ", ""), typeof(T));
                }
            }
            return ret;
        }
        /// <summary>
        /// to dictionary 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="str"></param>
        /// <param name="firstsep">first separator</param>
        /// <param name="secsep">second separator</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDic<TKey, TValue>(this string str, string firstsep= ";", string secsep = ",")
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>();
            string[] split1 = str.Split(new string[] { firstsep }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < split1.Length; i++)
            {
                string[] split2 = split1[i].Split(new string[] { secsep }, StringSplitOptions.RemoveEmptyEntries);
                if (split2.Length != 2)
                {
                    throw new Exception($"{split1[i]} can't be splited  by {secsep}");
                }
                ret.Add((TKey)Convert.ChangeType(split2[0].Replace(" ", ""), typeof(TKey)), (TValue)Convert.ChangeType(split2[1].Replace(" ", ""), typeof(TValue)));

            }
            return ret;
        }
        /// <summary>
        ///  to dictionary which key is enum
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="str"></param>
        /// <param name="firstsep"></param>
        /// <param name="secsep"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToEnumKeyDic<TKey, TValue>(this string str, string firstsep= ";", string secsep= ",")
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>();
            string[] split1 = str.Split(new string[] { firstsep }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < split1.Length; i++)
            {
                string[] split2 = split1[i].Split(new string[] { secsep }, StringSplitOptions.RemoveEmptyEntries);
                if (split2.Length != 2)
                {
                    throw new Exception($"{split1[i]} can't be splited  by {secsep}");
                }
                ret.Add((TKey)Enum.Parse(typeof(TKey), split2[0].Replace(" ", "")), (TValue)Convert.ChangeType(split2[1].Replace(" ", ""), typeof(TValue)));
            }
            return ret;
        }
        public static T ToT<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public static T ToObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public static IEnumerable<ushort> Toushorts(this string str,bool inverse=false)
        {
            byte[] UTF8bytes = Encoding.UTF8.GetBytes(str);
            List<ushort> ret = new List<ushort>();
            for (int i = 0; i < UTF8bytes.Length; i=i+2)
            {
                if ((i + 1) >= UTF8bytes.Length
                    && UTF8bytes.Length % 2 != 0)
                    break;
                ret.Add( UTF8bytes.ToUInt16(i,inverse));
            }
            if (UTF8bytes.Length % 2 != 0)
            {
                if (inverse)
                    ret.Add(((byte)0,UTF8bytes[UTF8bytes.Length - 1]).ToUInt16());
                else
                    ret.Add(( UTF8bytes[UTF8bytes.Length - 1], (byte)0).ToUInt16());
            }
            return ret;
        }
        public static bool IsNumeric(String strNumber)
        {
            Regex NumberPattern = new Regex("[^0-9.-]");
            return !NumberPattern.IsMatch(strNumber);
        }
        public static string GetBracketsValue(string stringContent)
        {
            string pattern = @"(?<=\()[^]]*(?=\))";
            //string pattern = @"\(.*?\)"; 
            Match match = Regex.Match(stringContent, pattern);
            return match.Value;
        }
        public static string GetNonBracketsValue(string stringContent)
        {
            char spiltChar = '(';
            int index = stringContent.IndexOf(spiltChar);
            return stringContent.Substring(0, index);
        }
    }
}
