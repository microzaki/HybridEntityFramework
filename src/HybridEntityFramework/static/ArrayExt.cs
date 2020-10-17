using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace HybridEntityFramework
{
    public static class ArrayExt
    {
        public static string ToMatlabString(this double[,,] matrix, string DataName,uint dimen)
        {
            string str="";
            List<double[,]> twodim = matrix.GetTwoDim(dimen);
            for (int i = 0; i < twodim.Count(); i++)
            {
                string append = "";
                if (dimen == 0)
                {
                    append = $"{ DataName} (:,:,{ (i + 1).ToString()}){ twodim[i].ToMatlabString("", false)}";
                }
                else if (dimen == 1)
                {
                    append = $"{ DataName} ({ (i + 1).ToString()},:,:){ twodim[i].ToMatlabString("", true)}";
                }
                else
                {
                    append = $"{ DataName} (:,{ (i + 1).ToString()},:){ twodim[i].ToMatlabString("", true)}";
                }
                if (i==0)
                {
                    str = append;
                }
                else
                {
                    str = $@"{str}  {append}";
                }
                
            }
            return str;
        }
        public static string ToMatlabString(this double[,] matrix, string DataName, bool Transpose)
        {
            string str_info = "";
            str_info += $@"{DataName} = [";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string temp = "";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    temp += $"{Math.Round(matrix[i, j],10).ToString()} ";
                }
                temp += @";";
                str_info += temp;
            }
            if (Transpose)
            {
                str_info += "]';";
            }
            else
            {
                str_info += "];";
            }

            return str_info;
        }
        public static List< double[,]> GetTwoDim(this double[,,] matrix,uint dimen)
        {
            List<double[,]> ret = new List<double[,]>();
            for (int a = 0;a< matrix.GetLength((int)dimen); a++)
            {
                double[,] sheet = null;
                if (dimen == 0)
                {
                    int x = matrix.GetLength(1);
                    int y = matrix.GetLength(2);
                    sheet = new double[x, y];
                    for (int i = 0; i < x; i++)
                        for (int j = 0; j < y; j++)
                            sheet[i, j] = matrix[a, i, j];
                }
                else if (dimen == 1)
                {
                    int x = matrix.GetLength(0);
                    int y = matrix.GetLength(2);
                    sheet = new double[x, y];
                    for (int i = 0; i < x; i++)
                        for (int j = 0; j < y; j++)
                            sheet[i, j] = matrix[i, a, j];
                }
                else
                {
                    int x = matrix.GetLength(0);
                    int y = matrix.GetLength(1);
                    sheet = new double[x, y];
                    for (int i = 0; i < x; i++)
                        for (int j = 0; j < y; j++)
                            sheet[i, j] = matrix[i, j, a];
                }
                ret.Add(sheet);
            }
            return ret;
        }
        public static List<double[]> GetOneDim(this double[,] matrix, uint dimen)
        {
            List<double[]> ret = new List<double[]>();
            for (int a = 0; a < matrix.GetLength((int)dimen); a++)
            {
                double[] array = null;
                if (dimen == 0)
                {
                    int len = matrix.GetLength(1);
                    array = new double[len];
                    for (int i = 0; i < len; i++)
                        array[i] = matrix[a, i];
                }
                else
                {
                    int len = matrix.GetLength(0);
                    array = new double[len];
                    for (int i = 0; i < len; i++)
                        array[i] = matrix[i, a];
                }
                ret.Add(array);
            }
            return ret;
        }

        public static T[,] Concat<T>(this T[,] a, T[,] b)
        {
            var r = new T[a.GetLength(0) + b.GetLength(0), a.GetLength(1)];
            var x = a.GetEnumerator();
            var y = b.GetEnumerator();
            for (var i = 0; i < r.GetLength(0); i++)
            {
                for (var j = 0; j < r.GetLength(1); j++)
                {
                    if (i < a.GetLength(0) && j < a.GetLength(1))
                    {
                        if (x.MoveNext()) r[i, j] = (T)x.Current;
                    }
                    else
                    {
                        if (y.MoveNext()) r[i, j] = (T)y.Current;
                    }
                }
            }

            return r;
        }
    }
}
