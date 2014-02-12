using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAC.Helper;
using System.Data;
using System.Collections;
using SAC.DBOperations;

namespace DAL
{
    /// <summary>
    /// 公用计算方法
    /// </summary>
    public class DALUtilityMehtod
    {
        string mon;
        string bm = "";
        string sql = "";
        string errMsg = "";
        string rtDBType = "";

        StrHelper sh = new StrHelper();
        DBLink dl = new DBLink();

        private string init()
        {
            rtDBType = IniHelper.ReadIniData("RelationDBbase", "DBType", null);

            return rtDBType;
        }

        /// <summary>
        /// 统计小时
        /// </summary>
        /// <param name="qsrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public string tjxs(string qsrq, string jsrq, int unit)
        {
            object obj = null;

            //this.init();
            sql = " SELECT sum(运行小时) FROM 参数运行小时统计表 where 统计参数='统计时间' and 机组=" + unit + " and 时间>='" + qsrq + "' and 时间<'" + jsrq + "'";
            obj = dl.RunSingle(sql, out errMsg);
            //if (rtDBType == "SQL")
            //{
            //    sql = " SELECT sum(运行小时) FROM 参数运行小时统计表 where 统计参数='统计时间' and 机组=" + unit + " and 时间>='" + qsrq + "' and 时间<'" + jsrq + "'";
            //    obj = DBsql.RunSingle(sql, out errMsg);
            //}
            //else if (rtDBType == "DB2")
            //{
            //    sql = " SELECT sum(运行小时) FROM 参数运行小时统计表 where 统计参数='统计时间' and 机组=" + unit + " and 时间>='" + qsrq + "' and 时间<'" + jsrq + "'";
            //}

            if (obj != null && obj != "" && obj != " ")
            {
                if (obj.ToString() != "")
                    return obj.ToString();
                else
                    return "0";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 入炉煤低位热值
        /// </summary>
        /// <param name="repType"></param>
        /// <param name="qsrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public string rlmdwfrl(string repType, string qsrq, string jsrq, int unit)
        {
            object objML1;
            object objML2;
            string val1 = "";
            string val2 = "";
            double res = 0;
            string mon = "";
            string re = "";

            string dwfrl1 = "0";
            string dwfrl2 = "0";
            string dwfrl3 = "0";

            string mz1ml1 = "0";     //煤量1
            string mz1ml2 = "0";
            string mz1ml3 = "0";

            string mz2ml1 = "0";     //煤量2
            string mz2ml2 = "0";
            string mz2ml3 = "0";

            string val = "";

            if (repType == "值报")
            {
                string sql = "select * from 入炉煤质分析表 where 日期='" + qsrq + "'";

                DataTable dtmt = dl.RunDataTable(sql, out errMsg);

                if (dtmt.Rows.Count > 0)
                {
                    if (dtmt.Rows.Count == 1)
                    {
                        mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                        mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();

                        dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                    }
                    else if (dtmt.Rows.Count == 2)
                    {
                        mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                        mz1ml2 = dtmt.Rows[1]["煤量1"].ToString();
                        mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();
                        mz2ml2 = dtmt.Rows[1]["煤量2"].ToString();

                        dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                        dwfrl2 = dtmt.Rows[1]["低位发热量"].ToString();
                    }
                    else if (dtmt.Rows.Count == 3)
                    {
                        mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                        mz1ml2 = dtmt.Rows[1]["煤量1"].ToString();
                        mz1ml3 = dtmt.Rows[2]["煤量1"].ToString();
                        mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();
                        mz2ml2 = dtmt.Rows[1]["煤量2"].ToString();
                        mz2ml3 = dtmt.Rows[2]["煤量2"].ToString();

                        dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                        dwfrl2 = dtmt.Rows[1]["低位发热量"].ToString();
                        dwfrl3 = dtmt.Rows[2]["低位发热量"].ToString();
                    }

                }
            }
            else if (repType == "日报")
            {
                string sqlCmd = "SELECT 低位热值 FROM 入炉煤质日数据表 where 日期='" + Convert.ToDateTime(qsrq).ToShortDateString() + "'";

                object objrlm = DBsql.RunSingle(sqlCmd, out errMsg);

                if (objrlm == null)
                {
                    string sql = "select * from 入炉煤质分析表 where 日期='" + qsrq + "'";

                    DataTable dtmt = DBsql.RunDataTable(sql, out errMsg);

                    #region
                    if (dtmt.Rows.Count > 0)
                    {
                        if (dtmt.Rows.Count == 1)
                        {
                            mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                            mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();

                            dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                        }
                        else if (dtmt.Rows.Count == 2)
                        {
                            mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                            mz1ml2 = dtmt.Rows[1]["煤量1"].ToString();
                            mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();
                            mz2ml2 = dtmt.Rows[1]["煤量2"].ToString();

                            dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                            dwfrl2 = dtmt.Rows[1]["低位发热量"].ToString();
                        }
                        else if (dtmt.Rows.Count == 3)
                        {
                            mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                            mz1ml2 = dtmt.Rows[1]["煤量1"].ToString();
                            mz1ml3 = dtmt.Rows[2]["煤量1"].ToString();
                            mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();
                            mz2ml2 = dtmt.Rows[1]["煤量2"].ToString();
                            mz2ml3 = dtmt.Rows[2]["煤量2"].ToString();

                            dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                            dwfrl2 = dtmt.Rows[1]["低位发热量"].ToString();
                            dwfrl3 = dtmt.Rows[2]["低位发热量"].ToString();
                        }

                    }
                    #endregion

                    try
                    {
                        mon = "((" + dwfrl1 + "*((" + mz1ml1 + ")+(" + mz2ml1 + ")))+(" + dwfrl2 + "*((" + mz1ml2 + ")+(" + mz2ml2 + ")))+(" + dwfrl3 + "*((" + mz1ml3 + ")+(" + mz2ml3 + "))))/((" + mz1ml1 + ") + (" + mz1ml2 + ") + (" + mz1ml3 + ")+(" + mz2ml1 + ") + (" + mz2ml2 + ") + (" + mz2ml3 + "))";
                        re = StrHelper.Cale(mon);
                        re = StrHelper.checkCatch(re);
                        res = double.Parse(re.ToString());
                    }
                    catch
                    { res = 0; }

                }
                else
                    res = double.Parse(objrlm.ToString());
            }
            else if (repType == "月报" || repType == "年报")
            {
                string[] date = sh.DateDiff(DateTime.Parse(qsrq), DateTime.Parse(jsrq));

                for (int i = 0; i < date.Length - 1; i++)
                {
                    string sql = "SELECT SUM(低位热值) FROM 入炉煤质日数据表 where 日期>='" + DateTime.Parse(date[i]).ToString("yyyy-MM-dd 0:00:00") + "' and  日期<'" + DateTime.Parse(date[i]).ToString("yyyy-MM-dd 23:59:59") + "'";

                    object objrlm = dl.RunSingle(sql, out errMsg);

                    if (objrlm == null || objrlm.ToString() == "")
                    {
                        sql = "select * from 入炉煤质分析表 where 日期='" + DateTime.Parse(date[i]).ToString("yyyy-MM-dd 0:00:00") + "'";

                        DataTable dtmt = dl.RunDataTable(sql, out errMsg);

                        #region
                        if (dtmt.Rows.Count > 0)
                        {
                            if (dtmt.Rows.Count == 1)
                            {
                                mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                                mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();

                                dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                            }
                            else if (dtmt.Rows.Count == 2)
                            {
                                mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                                mz1ml2 = dtmt.Rows[1]["煤量1"].ToString();
                                mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();
                                mz2ml2 = dtmt.Rows[1]["煤量2"].ToString();

                                dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                                dwfrl2 = dtmt.Rows[1]["低位发热量"].ToString();
                            }
                            else if (dtmt.Rows.Count == 3)
                            {
                                mz1ml1 = dtmt.Rows[0]["煤量1"].ToString();
                                mz1ml2 = dtmt.Rows[1]["煤量1"].ToString();
                                mz1ml3 = dtmt.Rows[2]["煤量1"].ToString();
                                mz2ml1 = dtmt.Rows[0]["煤量2"].ToString();
                                mz2ml2 = dtmt.Rows[1]["煤量2"].ToString();
                                mz2ml3 = dtmt.Rows[2]["煤量2"].ToString();

                                dwfrl1 = dtmt.Rows[0]["低位发热量"].ToString();
                                dwfrl2 = dtmt.Rows[1]["低位发热量"].ToString();
                                dwfrl3 = dtmt.Rows[2]["低位发热量"].ToString();
                            }

                        }
                        #endregion

                        try
                        {
                            mon = "((" + dwfrl1 + "*((" + mz1ml1 + ")+(" + mz2ml1 + ")))+(" + dwfrl2 + "*((" + mz1ml2 + ")+(" + mz2ml2 + ")))+(" + dwfrl3 + "*((" + mz1ml3 + ")+(" + mz2ml3 + "))))/((" + mz1ml1 + ") + (" + mz1ml2 + ") + (" + mz1ml3 + ")+(" + mz2ml1 + ") + (" + mz2ml2 + ") + (" + mz2ml3 + "))";
                            re = StrHelper.Cale(mon);
                            re = StrHelper.checkCatch(re);
                        }
                        catch
                        { re = "0"; }

                        string re1 = "";
                        try
                        {
                            mon = "((" + mz1ml1 + ") + (" + mz1ml2 + ") + (" + mz1ml3 + ")+(" + mz2ml1 + ") + (" + mz2ml2 + ") + (" + mz2ml3 + "))";
                            re1 = StrHelper.Cale(mon);
                            re1 = StrHelper.checkCatch(re1);
                        }
                        catch
                        { re1 = "0"; }

                        val1 += "(" + re + "*" + re1 + ")+";
                        val2 += "(" + re1 + ")+";
                    }
                    else
                    {
                        sql = "select sum(煤量1)+sum(煤量2) from 入炉煤质分析表 where 日期='" + DateTime.Parse(date[i]).ToShortDateString() + "'";

                        objML1 = dl.RunSingle(sql, out errMsg);

                        val1 += "(" + objrlm.ToString() + "*" + objML1.ToString() + ")+";
                        val2 += "(" + objML1.ToString() + ")+";
                    }
                }


                val = "(" + val1.TrimEnd('+') + ")/(" + val2.TrimEnd('+') + ")";

                try
                {
                    re = StrHelper.Cale(val);
                    re = StrHelper.checkCatch(re);
                }
                catch { re = "0"; }
                res = double.Parse(re);
            }

            return res.ToString();
        }

        public string rlmdwfrl(string repType, string qsrq, string jsrq, int unit, string bz)
        {
            string val = "";

            if (repType == "值报")
            {
                string sql = "select 低位发热量 from 入炉煤质分析表 where 日期='" + qsrq + "' and 班值=" + bz;

                object obj = DBsql.RunSingle(sql, out errMsg);

                if (obj != null && obj.ToString() != "")
                { val = obj.ToString(); }
                else { val = "0"; }
            }

            return val;
        }

        /// <summary>
        /// 购网电量
        /// </summary>
        /// <param name="qsrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public string gwdl(string qsrq, string jsrq, int unit)
        {
            object obj1;
            object obj2;

            string sql1;
            string sql2;
            string mon = "";
            string res = "";

            double drv = 0;
            double gwdl = 0;

            sql1 = " SELECT SUM(1-运行小时) AS Expr1 FROM 机组运行小时数据表 WHERE (设备类型='发电机') AND (机组 = 1) AND (日期 >= '" + qsrq + "') AND (日期 < '" + jsrq + "')";

            obj1 = dl.RunSingle(sql1, out errMsg);

            sql2 = " SELECT SUM(1-运行小时) AS Expr1 FROM 机组运行小时数据表 WHERE (设备类型='发电机') AND (机组 = 2) AND (日期 >= '" + qsrq + "') AND (日期 < '" + jsrq + "')";

            obj2 = dl.RunSingle(sql2, out errMsg);

            string fm = obj1.ToString() + "+" + obj2.ToString();

            try
            {
                res = StrHelper.Cale(fm);
            }
            catch (Exception ce)
            { errMsg = ce.Message; }

            //drv = double.Parse(pc.GetPower(drv, "qbb1zxyg", qsrq, jsrq, bm).ToString()) / 10000;

            if (obj1.ToString() == "0" && obj2.ToString() == "0")
            {
                if (unit == 1 || unit == 2)
                    gwdl = drv / 2;
                else
                    gwdl = drv;
            }
            else if (obj1.ToString() != "0" || obj2.ToString() != "0")
            {
                if (unit == 1)
                    mon = obj1.ToString() + "/" + res + "*" + drv.ToString();
                else if (unit == 2)
                    mon = obj2.ToString() + "/" + res + "*" + drv.ToString();
                else
                    mon = drv.ToString();

                try
                {
                    res = StrHelper.Cale(mon);
                    gwdl = double.Parse(res);
                }
                catch { gwdl = 0; }
            }
            else
            {
                gwdl = 0;
            }
            return gwdl.ToString();
        }

    }


}
