using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;


namespace SAC.Helper
{
    public class DateHelper
    {
        /// <summary>
        /// 判断某年是否为闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsLeapYear(int year)
        {
            if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
                return true;
            return false;
        }

        /// <summary>
        /// 获取时间所在季度第一天
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
        /// 获取时间所在季度最后一天
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
        /// 获取时间所在季度
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetQuarter(DateTime time)
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

        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public DateTime GetFirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public DateTime GetLastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="datetime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public DateTime GetFirstDayOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        /// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public DateTime GetLastDayOfPrdviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }

        /// <summary>
        /// 获取两个日期之间日期数组
        /// </summary>
        /// <param name="DateTime1">bt</param>
        /// <param name="DateTime2">et</param>
        /// <returns></returns>
        public string[] GetDateDiffByDays(DateTime DateTime1, DateTime DateTime2)
        {
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

        /// <summary>
        /// 获取两个日期之间小时数组
        /// </summary>
        /// <param name="DateTime1">bt</param>
        /// <param name="DateTime2">et</param>
        /// <returns></returns>
        public string[] GetDateDiffByHours(DateTime DateTime1, DateTime DateTime2)
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

        /// <summary>
        /// 获取两个日期之间天数值
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public int GetDaysOfDateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);

            TimeSpan ts = DateTime1 - DateTime2;
            return ts.Days;
        }

        #region 从DataTable转化为List 胡进财 2013/02/27
        /// <summary>
        /// 从DataTable转化为List
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <returns>List集合</returns>
        public IList<Hashtable> DataTableToList(DataTable dt)
        {
            IList<Hashtable> list = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                list = new List<Hashtable>();
                Hashtable ht = null;
                foreach (DataRow row in dt.Rows)
                {
                    ht = new Hashtable();
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (row[col.ColumnName] != null && !string.IsNullOrEmpty(Convert.ToString(row[col.ColumnName])))
                        {
                            ht.Add(col.ColumnName.ToUpper(), row[col.ColumnName]);
                        }
                        else
                        {
                            ht.Add(col.ColumnName.ToUpper(), "");
                        }
                    }
                    list.Add(ht);
                }
            }
            return list;
        }
        #endregion
    }
}
