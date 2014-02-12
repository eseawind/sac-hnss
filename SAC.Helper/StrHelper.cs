using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.JScript.Vsa;
using System.IO;
using Microsoft.Vsa;
using System.Globalization;

namespace SAC.Helper
{
    public class StrHelper
    {
        /// <summary>
        /// 判断某一年是否为闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        static public bool IsLeapYear(int year)
        {
            if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
                return true;
            return false;
        }

        /// <summary>
        /// 计算方法
        /// </summary>
        /// <param name="formule">数字公式</param>
        /// <returns>Value(Type:string)</returns>
        static public string Cale(string formule)
        {
            try
            {
                Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                return Microsoft.JScript.Eval.JScriptEvaluate(formule, ve).ToString();
            }
            catch (Exception ce)
            {
                return "0";
            }
        }

        /// <summary>
        /// 处理非数字和正无穷大
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string checkCatch(string str)
        {
            string s;
            if (str == "非数字")
            { s = "0"; }
            else if (str == "正无穷大")
            { s = "0"; }
            else if (str == "负无穷大")
            { s = "0"; }
            else
            {
                s = str;
            }
            return s;
        }

        /// <summary>
        /// 显示小数点位数
        /// </summary>
        /// <param name="res"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public string ShowPoint(string res, string num)
        {
            double val = 0;

            if (num != "-1")
            {
                if (num == "")
                    num = "6";

                if (res == "" || res == null)
                { return "0"; }
                else if (res == "0")
                { return "0"; }
                else if (res == "&nbsp;")
                { return "&nbsp;"; }
                else if (res == "0.00")
                { return "0"; }
                else if (res == "非数字")
                { return "0"; }
                else if (res == "正无穷大")
                { return "0"; }
                else if (res == "负无穷大")
                { return "0"; }
                else if (res == "/")
                { return "/"; }
                else
                {
                    try
                    { val = Convert.ToDouble(res); }
                    catch
                    { val = 0; }
                }
                return val.ToString("f" + num);

            }
            else
            {
                if (res == "&nbsp;")
                { return "&nbsp;"; }
                else if (res == "0.00")
                { return "0"; }
                else if (res == "非数字")
                { return "0"; }
                else if (res == "正无穷大")
                { return "0"; }
                else if (res == "负无穷大")
                { return "0"; }

                return res;
            }

        }

        /// 
        /// 将一位数字转换成中文大写数字
        /// 
        public string ConvertChinese(string str)
        {
            //"零壹贰叁肆伍陆柒捌玖拾佰仟萬億圆整角分"
            string cstr = "";

            switch (str)
            {
                //case "0": cstr = "零"; break;
                //case "1": cstr = "壹"; break;
                //case "2": cstr = "贰"; break;
                //case "3": cstr = "叁"; break;
                //case "4": cstr = "肆"; break;
                //case "5": cstr = "伍"; break;
                //case "6": cstr = "陆"; break;
                //case "7": cstr = "柒"; break;
                //case "8": cstr = "捌"; break;
                //case "9": cstr = "玖"; break;

                case "0": cstr = "零"; break;
                case "1": cstr = "一"; break;
                case "2": cstr = "二"; break;
                case "3": cstr = "三"; break;
                case "4": cstr = "四"; break;
                case "5": cstr = "五"; break;
                case "6": cstr = "六"; break;
                case "7": cstr = "七"; break;
                case "8": cstr = "八"; break;
                case "9": cstr = "九"; break;
            }
            return cstr + "值";
        }

        /// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="datetime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public DateTime FirstDayOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        /// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public DateTime LastDayOfPrdviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }

        /// <summary>
        /// 两个日起之间的日期数组
        /// </summary>
        /// <param name="DateTime1">开始日期</param>
        /// <param name="DateTime2">结束日期</param>
        /// <returns></returns>
        public string[] DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            //计算两个日起之间天数
            int dateDiff = 0;
            string[] numDate = null;

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            dateDiff = ts.Days;

            if (dateDiff != 0)
            {
                numDate = new string[dateDiff + 1];

                for (int i = 0; i < dateDiff + 1; i++)
                {
                    numDate[i] = DateTime1.AddDays(i).ToShortDateString();
                }
            }
            return numDate;
        }

        public string[] DateDiffHours(DateTime DateTime1, DateTime DateTime2)
        {
            //计算两个日起之间天数
            int dateDiff = 0;
            string[] numDate = null;

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();

            dateDiff = ts.Hours;

            if (dateDiff != 0)
            {
                numDate = new string[dateDiff + 1];

                for (int i = 0; i < dateDiff + 1; i++)
                {
                    numDate[i] = DateTime2.AddHours(i).ToString("yyyy-MM-dd H:mm:ss");
                }
            }
            return numDate;
        }

        public int DateDiffDays(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            //TimeSpan ts = ts1.Subtract(ts2).Duration();

            TimeSpan ts = DateTime1 - DateTime2;
            return ts.Days;
        }

        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 处理形如'{0}'，而参数中又有单引号的情况，将参数中的单引号double
        /// 如果args中的单引号不是作为文本的话，不要用这个方法，直接用string.format()
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static public string stringFormat(string format, params object[] args)
        {
            if (format.IndexOf("'") == -1) //没有在单引号内的占位符
                return string.Format(format, args);

            ArrayList al = new ArrayList();
            for (int i = 0; i < args.Length; i++)
                al.Add(args[i].ToString().Replace("'", "''")); // 处理单引号
            return string.Format(format, al.ToArray());
        }

        /// <summary>
        /// 数组排序(冒泡)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public string[] arraySort(string[] arr)
        {
            string temp;

            for (int j = 0; j < arr.Length; j++)
            {
                for (int i = arr.Length - 1; i > j; i--)
                {
                    if (arr[i] != null && arr[j] != null)
                    {
                        if (double.Parse(arr[j]) < double.Parse(arr[i]))
                        {
                            temp = arr[j];
                            arr[j] = arr[i];
                            arr[i] = temp;
                        }
                    }
                }
            }

            return arr;
        }

        /// <summary>
        /// 获取此时间所在季度第一天
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetInceDay(DateTime time)
        {
            string val;

            int month = time.Month;

            if (month >= 1 && month < 4)
            {
                val = time.Year.ToString() + "-1-1";
            }
            else if (month >= 4 && month < 7)
            {
                val = time.Year + "-4-1";
            }
            else if (month >= 7 && month < 10)
            {
                val = time.Year + "-7-1";
            }
            else
            {
                val = time.Year + "-10-1";
            }
            return val;
        }

        /// <summary>
        /// 获取此时间所在季度最后一天
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetEndDay(DateTime time)
        {
            string val;
            int month = time.Month;

            if (month >= 1 && month < 4)
            {
                val = time.Year + "-3-31";
            }
            else if (month >= 4 && month < 7)
            {
                val = time.Year + "-6-30";
            }
            else if (month >= 7 && month < 10)
            {
                val = time.Year + "-9-30";
            }
            else
            {
                val = time.Year + "-12-31";
            }
            return val;
        }


        /// <summary>
        /// 获取此时间所在季度
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetJidu(DateTime time)
        {
            string val;

            int month = time.Month;

            if (month >= 1 && month < 4)
            {
                val = "1";
            }
            else if (month >= 4 && month < 7)
            {
                val = "2";
            }
            else if (month >= 7 && month < 10)
            {
                val = "3";
            }
            else
            {
                val = "4";
            }
            return val;
        }

    }
}
