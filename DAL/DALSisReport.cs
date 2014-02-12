using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAC.Helper;
using System.Collections;
using System.Data;
using SAC.DBOperations;

namespace DAL
{
    /// <summary>
    /// 报表数据生成类
    /// 2013-06-30 更新:添加是否显示全厂数据判断
    /// 更改者:单云龙
    /// </summary>
    public class DALSisReport
    {
        int count;
        int countKey;
        string sql = "";
        string TSMS = "";       //调试模式
        string errMsg = "";     //错误消息  
        string xzTime = "2013-01-01";     //取数限制时间
        string rtDBType = "";   //实时数据库
        string rlDBType = "";   //关系数据库        
        DBLink dl = new DBLink();
        StrHelper sh = new StrHelper();
        DALPubMehtod pm = new DALPubMehtod();
        DALUtilityMehtod um = new DALUtilityMehtod();
        LogHelper lh = new LogHelper();

        private string ip = "", port = "", user = "", pwd = "", dbType = "", calc = "";

      

        /// <summary>
        /// 中电投报表模块(日,月,年,年明细)
        /// </summary>
        /// <param name="dType">报表日期类型</param>
        /// <param name="repName">报表模板ID</param>
        /// <param name="repOrgid">组织机构ID</param>
        /// <param name="treeid">组织机构树ID</param>
        /// <param name="param">SQL参数集合</param>     
        /// <param name="flag">是否计算同期</param>
        /// <param name="isZy">是否计算最优值</param>
        /// <param name="date">日期</param>
        /// <param name="errMsg">错误消息</param>
        /// <returns></returns>
        public Hashtable RetHtRep(string dType, string repName, string repOrgid, string treeid, string param, bool flag, bool isZy, DateTime date, out string errMsg)
        {
            int ret = 0;
            object obj = null;
            double drvA = 0, drvB = 0;
            string dayBt = "", dayEt = "", dayBtT = "", dayEtT = "";
            string monBt = "", monEt = "", monBtT = "", monEtT = "";
            string yearBt = "", yearEt = "", yearBtT = "", yearEtT = "";
            string num = "", unit = "", pName = "", dlID = "", mon = "", bm = "";
            string qsrq = "", jsrq = "", qsrqT = "", jsrqT = "", res = "", sDate = "", sDateT = "", sqlWhere = " where 1=1 ", ParaID;
            string isDay = "0", isMon = "0", isYear = "0";
            bool errDay = false, errMon = false, errYear = false;

            errMsg = "";

            DateTime dtNow = DateTime.Now;
            DateTime dtEr = DateTime.Now;

            Hashtable ht = new Hashtable();
            Hashtable htGY = new Hashtable();
            Hashtable htGYT = new Hashtable();
            Hashtable htUn = new Hashtable();
            Hashtable htUnT = new Hashtable();

            Hashtable htParam = new Hashtable();

           

            #region  日，月，年报表模块

            

            #region 时间
            //Day Time 
            if (date < dtNow && date >= Convert.ToDateTime(xzTime))
            {
                if (dtNow.Year == date.Year && dtNow.Month == date.Month && dtNow.Day == date.Day)
                {
                    isDay = "1";
                    dayBt = date.ToString("yyyy-MM-dd 00:00:00");
                    dayEt = dtNow.AddHours(-1).ToString("yyyy-MM-dd H:00:00");
                }
                else
                {
                    dayBt = date.ToString("yyyy-MM-dd 0:00:00");
                    dayEt = date.AddDays(+1).ToString("yyyy-MM-dd 0:00:00");
                }

                //同期时间
                dayBtT = Convert.ToDateTime(dayBt).AddYears(-1).ToString("yyyy-MM-dd H:mm:ss");
                dayEtT = Convert.ToDateTime(dayEt).AddYears(-1).ToString("yyyy-MM-dd H:mm:ss");
            }
            else
            { errDay = true; }

            //month Time
            if (date < dtNow && date >= Convert.ToDateTime(xzTime))
            {
                if (date.Year == dtNow.Year && date.Month == dtNow.Month)
                {
                    isMon = "1";
                    monBt = date.ToString("yyyy-MM-01 H:mm:ss");
                    monEt = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");

                    monBtT = DateTime.Parse(monBt).AddYears(-1).ToString("yyyy-MM-01 H:mm:ss");
                    monEtT = DateTime.Parse(monEt).AddYears(-1).AddMonths(+1).ToString("yyyy-MM-01 H:mm:ss");
                }
                else
                {
                    monBt = date.ToString("yyyy-MM-01 0:00:00");
                    monEt = sh.LastDayOfMonth(DateTime.Parse(monBt)).AddDays(+1).ToString("yyyy-MM-dd 0:00:00");

                    monBtT = DateTime.Parse(monBt).AddYears(-1).ToString("yyyy-MM-dd H:mm:ss");
                    monEtT = DateTime.Parse(monEt).AddYears(-1).ToString("yyyy-MM-dd H:mm:ss");
                }
            }
            else
            { errMon = true; }
            //year Time
            if (date < dtNow && date >= Convert.ToDateTime(xzTime))
            {
                if (date.Year == dtNow.Year)
                {
                    isYear = "1";
                    yearBt = date.ToString("yyyy-01-01 0:00:00");
                    yearEt = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");

                    yearBtT = DateTime.Parse(yearBt).AddYears(-1).ToString("yyyy-MM-dd H:mm:ss");
                    yearEtT = yearBt;
                }
                else
                {
                    yearBt = date.ToString("yyyy-01-01 0:00:00");
                    yearEt = DateTime.Parse(yearBt).AddYears(+1).ToString("yyyy-MM-dd H:mm:ss");

                    yearBtT = DateTime.Parse(yearBt).AddYears(-1).ToString("yyyy-MM-dd H:mm:ss");
                    yearEtT = yearBt;
                }

                if (dType == "M" || dType == "MM")
                {
                    if (isMon == "1")
                        yearEt = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");
                    else
                        yearEt = sh.LastDayOfMonth(date).ToString("yyyy-MM-dd 0:00:00");
                }
            }
            else
            { errYear = true; }
            #endregion

            #region SQL参数赋值
            if (param != "")
            {
                string[] v = param.TrimEnd('+').Split('+');
                if (v.Length > 0)
                {
                    for (int i = 0; i < v.Length; i++)
                    {
                        string[] v1 = v[i].ToString().Split(':');

                        if (!htParam.Contains(v1[0].ToString()) && v1[1] != "")
                            htParam.Add(v1[0].ToString(), v1[1].ToString());
                    }
                }
            }
            #endregion

            #region 机组数据轮转

            sqlWhere = sqlWhere + " and T_REPORTID='" + repName + "' ";

            if (repOrgid != "")
                sqlWhere += " and t_orgid='" + repOrgid + "' ";

            if (treeid != "")
                sqlWhere += " and T_Treeid='" + treeid + "' ";

            sql = "SELECT DISTINCT T_UNITID FROM T_sheet_sheetPara &where&  and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

            if (sql.Contains("&where&"))
                sql = sql.Replace("&where&", sqlWhere);

            DataTable dtLevUnit = null;
            dtLevUnit = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevUnit = DBsql.RunDataTable(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    dtLevUnit = DBdb2.RunDataTable(sql, out errMsg);
            //else if (rlDBType == "Oracle")
            //    dtLevUnit = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

            if (dtLevUnit != null && dtLevUnit.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "----机组轮转开始!");

                for (int k = 0; k < dtLevUnit.Rows.Count; k++)
                {
                    unit = dtLevUnit.Rows[k]["T_UNITID"].ToString();

                    sql = "SELECT DISTINCT I_FORMULALEVEL FROM T_sheet_sheetPara  &sqlwhere&  and I_FORMULALEVEL!=0 and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y') ";

                    sqlWhere = " where 1=1 and T_REPORTID='" + repName + "' ";

                    if (repOrgid != "")
                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                    if (treeid != "")
                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                    if (sql.Contains("&sqlwhere&"))
                        sql = sql.Replace("&sqlwhere&", sqlWhere);

                    DataTable dtLel = null;
                    dtLel = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtLel = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtLel = DBdb2.RunDataTable(sql, out errMsg);
                    //else
                    //    dtLel = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                    if (dtLel.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtLel.Rows.Count; j++)
                        {
                            string gsjb = dtLel.Rows[j]["I_FORMULALEVEL"].ToString();

                            #region 平均

                            sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='平均' and T_REPORTID='" + repName + "'";

                            sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                            if (repOrgid != "")
                                sqlWhere += " and t_orgid='" + repOrgid + "' ";

                            if (treeid != "")
                                sqlWhere += " and T_Treeid='" + treeid + "' ";

                            if (sql.Contains("&where&"))
                                sql = sql.Replace("&where&", sqlWhere);

                            DataTable dtAvg = null;
                            dtAvg = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);
                            //else
                            //    dtAvg = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                            if (dtAvg != null && dtAvg.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtAvg.Rows.Count; i++)
                                {
                                    //小数点数
                                    num = dtAvg.Rows[i]["I_DECIMALPOINT"].ToString();
                                    //测点名
                                    pName = dtAvg.Rows[i]["T_TAGID"].ToString();
                                    //参数ID
                                    ParaID = dtAvg.Rows[i]["T_PARAID"].ToString();

                                    //对时间类型做判断                                   
                                    switch (dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper())
                                    {
                                        case "D":
                                            sDate = DateTime.Parse(dayEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(dayEtT).AddSeconds(-1).ToString();
                                            break;
                                        case "M":
                                            sDate = DateTime.Parse(monEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(monEtT).AddSeconds(-1).ToString();
                                            break;
                                        case "Y":
                                            sDate = DateTime.Parse(yearEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(yearEtT).AddSeconds(-1).ToString();
                                            break;
                                    }

                                    if (flag == true)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //{
                                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                        //    ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                                        //}
                                        //else if (rtDBType == "HS")
                                        //{
                                        //    hs.GetHistVal(pName, ref drvA, sDate);
                                        //    hs.GetHistVal(pName, ref drvB, sDateT);
                                        //}
                                        //else
                                        //{
                                        //    pk.GetHisValue(pName, sDate, ref drvA);
                                        //    pk.GetHisValue(pName, sDateT, ref drvB);
                                        //}
                                    }
                                    else
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                        //else if (rtDBType == "HS")
                                        //    hs.GetHistVal(pName, ref drvA, sDate);
                                        //else
                                        //    pk.GetHisValue(pName, sDate, ref drvA);
                                    }

                                    if (flag == true)
                                    {
                                        //传出数据
                                        if (!ht.ContainsKey(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper()) && !ht.ContainsKey(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                        {
                                            ht.Add(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(drvA.ToString(), num));
                                            ht.Add(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(drvB.ToString(), num));
                                        }
                                        else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                        //公式数据
                                        if (!htUn.ContainsKey(ParaID) && !htUnT.ContainsKey(ParaID))
                                        {
                                            if (calc == "yes")
                                            {
                                                htUn.Add(ParaID, drvA.ToString());
                                                htUnT.Add(ParaID, drvB.ToString());
                                            }
                                            else
                                            {
                                                htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                                htUnT.Add(ParaID, sh.ShowPoint(drvB.ToString(), num));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //传出参数
                                        if (!ht.ContainsKey(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                            ht.Add(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper(),sh.ShowPoint( drvA.ToString(),num));
                                        else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                        //公式参数
                                        if (!htUn.ContainsKey(ParaID))
                                        {
                                            if (calc == "yes")
                                                htUn.Add(ParaID, drvA.ToString());
                                            else
                                                htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 累计
                            sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='累计' and T_REPORTID='" + repName + "'";

                            sql = "select * from T_sheet_sheetPara &where&  and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                            if (repOrgid != "")
                                sqlWhere += " and t_orgid='" + repOrgid + "' ";

                            if (treeid != "")
                                sqlWhere += " and T_Treeid='" + treeid + "' ";

                            if (sql.Contains("&where&"))
                                sql = sql.Replace("&where&", sqlWhere);

                            DataTable dtDiff = null;
                            dtDiff = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDiff = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                            if (dtDiff != null && dtDiff.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtDiff.Rows.Count; i++)
                                {
                                    //小数点数
                                    num = dtDiff.Rows[i]["I_DECIMALPOINT"].ToString();
                                    //测点名
                                    pName = dtDiff.Rows[i]["T_TAGID"].ToString();
                                    //参数ID
                                    ParaID = dtDiff.Rows[i]["T_PARAID"].ToString();

                                    //对时间类型做判断                                   
                                    switch (dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper())
                                    {
                                        case "D":
                                            sDate = DateTime.Parse(dayEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(dayEtT).AddSeconds(-1).ToString();
                                            break;
                                        case "M":
                                            sDate = DateTime.Parse(monEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(monEtT).AddSeconds(-1).ToString();
                                            break;
                                        case "Y":
                                            sDate = DateTime.Parse(yearEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(yearEtT).AddSeconds(-1).ToString();
                                            break;
                                    }

                                    if (flag == true)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //{
                                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                        //    ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                                        //}
                                        //else if (rtDBType == "HS")
                                        //{
                                        //    hs.GetHistVal(pName, ref drvA, sDate);
                                        //    hs.GetHistVal(pName, ref drvB, sDateT);
                                        //}
                                        //else
                                        //{
                                        //    pk.GetHisValue(pName, sDate, ref drvA);
                                        //    pk.GetHisValue(pName, sDateT, ref drvB);
                                        //}
                                    }
                                    else
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                        //else if (rtDBType == "HS")
                                        //    hs.GetHistVal(pName, ref drvA, sDate);
                                        //else
                                        //    pk.GetHisValue(pName, sDate, ref drvA);
                                    }

                                    if (flag == true)
                                    {
                                        //传出数据
                                        if (!ht.ContainsKey(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper()) && !ht.ContainsKey(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                        {
                                            ht.Add(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(drvA.ToString(), num));
                                            ht.Add(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(drvB.ToString(), num));
                                        }
                                        else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                        //公式数据
                                        if (!htUn.ContainsKey(ParaID) && !htUnT.ContainsKey(ParaID))
                                        {
                                            if (calc == "yes")
                                            {
                                                htUn.Add(ParaID, drvA.ToString());
                                                htUnT.Add(ParaID, drvB.ToString());
                                            }
                                            else
                                            {
                                                htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                                htUnT.Add(ParaID, sh.ShowPoint(drvB.ToString(), num));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //传出参数
                                        if (!ht.ContainsKey(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                            ht.Add(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper(),sh.ShowPoint( drvA.ToString(),num));
                                        else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                        //公式参数
                                        if (!htUn.ContainsKey(ParaID))
                                        {
                                            if (calc == "yes")
                                                htUn.Add(ParaID, drvA.ToString());
                                            else
                                                htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region SQL
                            sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='SQL' and T_REPORTID='" + repName + "'";

                            sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                            if (repOrgid != "")
                                sqlWhere += " and t_orgid='" + repOrgid + "' ";

                            if (treeid != "")
                                sqlWhere += " and T_Treeid='" + treeid + "' ";

                            if (sql.Contains("&where&"))
                                sql = sql.Replace("&where&", sqlWhere);

                            DataTable dtSQL = null;
                            dtSQL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);
                            //else
                            //    dtSQL = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                            if (dtSQL != null && dtSQL.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSQL.Rows.Count; i++)
                                {
                                    //小数点数
                                    num = dtSQL.Rows[i]["I_DECIMALPOINT"].ToString();
                                    //参数ID
                                    ParaID = dtSQL.Rows[i]["T_PARAID"].ToString();

                                    //对时间类型做判断                                   
                                    switch (dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper())
                                    {
                                        case "D":
                                            qsrq = dayBt;
                                            qsrqT = dayBtT;
                                            sDate = DateTime.Parse(dayEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(dayEtT).AddSeconds(-1).ToString();
                                            break;
                                        case "M":
                                            qsrq = monBt;
                                            qsrqT = monBtT;
                                            sDate = DateTime.Parse(monEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(monEtT).AddSeconds(-1).ToString();
                                            break;
                                        case "Y":
                                            qsrq = yearBt;
                                            qsrqT = yearBtT;
                                            sDate = DateTime.Parse(yearEt).AddSeconds(-1).ToString();
                                            sDateT = DateTime.Parse(yearEtT).AddSeconds(-1).ToString();
                                            break;
                                    }

                                    //本期
                                    sql = dtSQL.Rows[i]["T_SQL"].ToString();

                                    sql = sql.Replace("&bt&", qsrq);
                                    sql = sql.Replace("&et&", sDate);

                                    if (sql.Contains("&ORGID&"))
                                        sql = sql.Replace("&ORGID&", repOrgid);

                                    if (htParam.Count > 0)
                                    {
                                        foreach (System.Collections.DictionaryEntry item in htParam)
                                        {
                                            if (sql.Contains("&" + item.Key.ToString() + "&"))
                                                sql.Replace("&" + item.Key.ToString() + "&", item.Value.ToString());
                                        }
                                    }
                                    obj = dl.RunSingle(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    obj = DBsql.RunSingle(sql, out errMsg);
                                    //else if (rlDBType == "DB2")
                                    //    obj = DBdb2.RunSingle(sql, out errMsg);
                                    //else
                                    //    obj = SAC.OracleHelper.OracleHelper.GetSingle(sql);

                                    res = "";
                                    if (obj != null && obj != "")
                                    {
                                        if (obj.ToString() != "")
                                            res = obj.ToString();
                                        else
                                            res = "0";
                                    }
                                    else
                                    { res = "0"; }

                                    //传出数据
                                    if (!ht.ContainsKey(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                        ht.Add(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(res, num));
                                    else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                    //公式数据
                                    if (!htUn.ContainsKey(ParaID))
                                    {
                                        if (calc == "yes")
                                            htUn.Add(ParaID, res);
                                        else
                                            htUn.Add(ParaID, sh.ShowPoint(res, num));
                                    }

                                    //同期
                                    if (flag == true)
                                    {
                                        sql = dtSQL.Rows[i]["T_SQL"].ToString();

                                        sql = sql.Replace("&bt&", qsrqT);
                                        sql = sql.Replace("&et&", sDateT);

                                        if (sql.Contains("&ORGID&"))
                                        {
                                            sql = sql.Replace("&ORGID&", repOrgid);
                                        }

                                        if (htParam.Count > 0)
                                        {
                                            foreach (System.Collections.DictionaryEntry item in htParam)
                                            {
                                                if (sql.Contains("&" + item.Key.ToString() + "&"))
                                                    sql.Replace("&" + item.Key.ToString() + "&", item.Value.ToString());
                                            }
                                        }
                                        obj = dl.RunSingle(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    obj = DBsql.RunSingle(sql, out errMsg);
                                        //else if (rlDBType == "DB2")
                                        //    obj = DBdb2.RunSingle(sql, out errMsg);
                                        //else
                                        //    obj = SAC.OracleHelper.OracleHelper.GetSingle(sql);

                                        res = "";
                                        if (obj != null && obj != "")
                                        {
                                            if (obj.ToString() != "")
                                                res = obj.ToString();
                                            else
                                                res = "0";
                                        }
                                        else
                                        { res = "0"; }

                                        //传出数据
                                        if (!ht.ContainsKey(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                            ht.Add(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(res, num));

                                        //公式数据
                                        if (!htUnT.ContainsKey(ParaID))
                                        {
                                            if (calc == "yes")
                                                htUnT.Add(ParaID, res);
                                            else
                                                htUnT.Add(ParaID, sh.ShowPoint(res, num));
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 电量

                            sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='电量' and T_REPORTID='" + repName + "'";

                            sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                            if (repOrgid != "")
                                sqlWhere += " and t_orgid='" + repOrgid + "' ";

                            if (treeid != "")
                                sqlWhere += " and T_Treeid='" + treeid + "' ";

                            if (sql.Contains("&where&"))
                                sql = sql.Replace("&where&", sqlWhere);

                            DataTable dtDL = null;
                            dtDL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtDL = DBdb2.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDL = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                            if (dtDL.Rows.Count > 0 && dtDL != null)
                            {
                                for (int i = 0; i < dtDL.Rows.Count; i++)
                                {
                                    //电量测点
                                    dlID = dtDL.Rows[i]["T_POWERID"].ToString();
                                    //小数点数
                                    num = dtDL.Rows[i]["I_DECIMALPOINT"].ToString();
                                    //参数ID
                                    ParaID = dtDL.Rows[i]["T_PARAID"].ToString();

                                    //对时间类型做判断                                   
                                    switch (dtDL.Rows[i]["T_TimeType"].ToString().ToUpper())
                                    {
                                        case "D":
                                            qsrq = dayBt;
                                            qsrqT = dayBtT;
                                            jsrq = dayEt;
                                            jsrqT = dayEtT;
                                            break;
                                        case "M":
                                            qsrq = monBt;
                                            qsrqT = monBtT;
                                            jsrq = monEt;
                                            jsrqT = monEtT;
                                            break;
                                        case "Y":
                                            qsrq = yearBt;
                                            qsrqT = yearBtT;
                                            jsrq = yearEt;
                                            jsrqT = yearEtT;
                                            break;
                                    }

                                    //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                                    mon = "";

                                    if (dtDL.Rows[i]["T_FORMULA"].ToString() != "")
                                        mon = drvA.ToString() + dtDL.Rows[i]["T_FORMULA"].ToString();
                                    else
                                        mon = drvA.ToString();

                                    res = "";
                                    try
                                    { res = StrHelper.Cale(mon); }
                                    catch
                                    { res = "0"; }

                                    //传出数据
                                    if (!ht.ContainsKey(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                        ht.Add(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(res, num));
                                    else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                    //公式数据
                                    if (!htUn.ContainsKey(ParaID))
                                    {
                                        if (calc == "yes")
                                            htUn.Add(ParaID, res);
                                        else
                                            htUn.Add(ParaID, sh.ShowPoint(res, num));
                                    }

                                    if (flag == true)
                                    {
                                        //drvB = double.Parse(pc.GetPower(drvB, dlID, qsrqT, jsrqT, bm).ToString());

                                        mon = "";

                                        if (dtDL.Rows[i]["T_FORMULA"].ToString() != "")
                                            mon = drvB.ToString() + dtDL.Rows[i]["T_FORMULA"].ToString();
                                        else
                                            mon = drvB.ToString();

                                        res = "";
                                        try
                                        { res = StrHelper.Cale(mon); }
                                        catch
                                        { res = "0"; }

                                        //传出数据
                                        if (!ht.ContainsKey(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                            ht.Add(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(res, num));

                                        //公式数据
                                        if (!htUnT.ContainsKey(ParaID))
                                        {
                                            if (calc == "yes")
                                                htUnT.Add(ParaID, res);
                                            else
                                                htUnT.Add(ParaID, sh.ShowPoint(res, num));
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region 其它

                            sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='其它' and T_REPORTID='" + repName + "'";

                            sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                            if (repOrgid != "")
                                sqlWhere += " and t_orgid='" + repOrgid + "' ";

                            if (treeid != "")
                                sqlWhere += " and T_Treeid='" + treeid + "' ";

                            if (sql.Contains("&where&"))
                                sql = sql.Replace("&where&", sqlWhere);

                            DataTable dtQT = null;
                            dtQT = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtQT = DBdb2.RunDataTable(sql, out errMsg);
                            //else
                            //    dtQT = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                            if (dtQT.Rows.Count > 0 && dtQT != null)
                            {
                                for (int i = 0; i < dtQT.Rows.Count; i++)
                                {
                                    num = dtQT.Rows[i]["I_DECIMALPOINT"].ToString();
                                    //参数ID
                                    ParaID = dtQT.Rows[i]["T_PARAID"].ToString();

                                    //对时间类型做判断                                   
                                    switch (dtQT.Rows[i]["T_TimeType"].ToString().ToUpper())
                                    {
                                        case "D":
                                            qsrq = dayBt;
                                            qsrqT = dayBtT;
                                            jsrq = dayEt;
                                            jsrqT = dayEtT;
                                            break;
                                        case "M":
                                            qsrq = monBt;
                                            qsrqT = monBtT;
                                            jsrq = monEt;
                                            jsrqT = monEtT;
                                            break;
                                        case "Y":
                                            qsrq = yearBt;
                                            qsrqT = yearBtT;
                                            jsrq = yearEt;
                                            jsrqT = yearEtT;
                                            break;
                                    }
                                }
                            }
                            #endregion

                            #region 公式

                            sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='公式' and T_REPORTID='" + repName + "'";

                            sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                            if (repOrgid != "")
                                sqlWhere += " and t_orgid='" + repOrgid + "' ";

                            if (treeid != "")
                                sqlWhere += " and T_Treeid='" + treeid + "' ";

                            if (sql.Contains("&where&"))
                                sql = sql.Replace("&where&", sqlWhere);

                            DataTable dtMon = null;
                            dtMon = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtMon = DBdb2.RunDataTable(sql, out errMsg);
                            //else
                            //    dtMon = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                            if (dtMon != null && dtMon.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtMon.Rows.Count; i++)
                                {
                                    num = dtMon.Rows[i]["I_DECIMALPOINT"].ToString();
                                    //参数ID
                                    ParaID = dtMon.Rows[i]["T_PARAID"].ToString();

                                    string pMon = dtMon.Rows[i]["T_FORMULA"].ToString();
                                    string paras = dtMon.Rows[i]["T_FORMULAPARA"].ToString();
                                    string month = pm.retMon(htGY, htUn, paras, pMon);

                                    if (month != "")
                                    {
                                        res = "";
                                        try
                                        { res = StrHelper.Cale(month); }
                                        catch
                                        { res = "0"; }

                                        //传出数据
                                        if (!ht.ContainsKey(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                            ht.Add(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(res, num));
                                        else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                        //公式数据
                                        if (!htUn.ContainsKey(ParaID))
                                        {
                                            if (calc == "yes")
                                                htUn.Add(ParaID, res);
                                            else
                                                htUn.Add(ParaID, sh.ShowPoint(res, num));
                                        }
                                    }

                                    if (flag == true)
                                    {
                                        string monthT = pm.retMon(htGYT, htUnT, paras, pMon);

                                        if (monthT != "")
                                        {
                                            res = "";
                                            try
                                            { res = StrHelper.Cale(monthT); }
                                            catch
                                            { res = "0"; }

                                            //传出数据
                                            if (!ht.ContainsKey(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                                ht.Add(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(res, num));

                                            //公式数据
                                            if (!htUnT.ContainsKey(ParaID))
                                            {
                                                if (calc == "yes")
                                                    htUnT.Add(ParaID, res);
                                                else
                                                    htUnT.Add(ParaID, sh.ShowPoint(res, num));
                                            }

                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "----机组轮转结束!");
            }

            #endregion

            #region 全厂数据读取

            //全厂数据
            sqlWhere = "where 1=1 and T_REPORTID='" + repName + "' ";

            if (repOrgid != "")
                sqlWhere += " and t_orgid='" + repOrgid + "' ";

            if (treeid != "")
                sqlWhere += " and T_Treeid='" + treeid + "' ";

            sql = "SELECT DISTINCT I_FORMULALEVEL FROM T_sheet_sheetPara &where& and T_UNITID=0 and I_FORMULALEVEL!=0 and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y') ";

            if (sql.Contains("&where&"))
                sql = sql.Replace("&where&", sqlWhere);

            DataTable dtLevZ = null;
            dtLevZ = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevZ = DBsql.RunDataTable(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    dtLevZ = DBdb2.RunDataTable(sql, out errMsg);
            //else
            //    dtLevZ = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

            if (dtLevZ.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "----全厂开始!");

                for (int j = 0; j < dtLevZ.Rows.Count; j++)
                {
                    string gsjb = dtLevZ.Rows[j]["公式级别"].ToString();

                    #region 平均
                    sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='平均' and T_UNITID=0 and T_REPORTID='" + repName + "'";

                    sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                    if (repOrgid != "")
                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                    if (treeid != "")
                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                    if (sql.Contains("&where&"))
                        sql = sql.Replace("&where&", sqlWhere);

                    DataTable dtAvg = null;
                    dtAvg = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);
                    //else
                    //    dtAvg = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                    if (dtAvg != null && dtAvg.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAvg.Rows.Count; i++)
                        {
                            //小数点数
                            num = dtAvg.Rows[i]["I_DECIMALPOINT"].ToString();
                            //测点名
                            pName = dtAvg.Rows[i]["T_TAGID"].ToString();
                            //参数ID
                            ParaID = dtAvg.Rows[i]["T_PARAID"].ToString();

                            //对时间类型做判断                                   
                            switch (dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper())
                            {
                                case "D":
                                    sDate = DateTime.Parse(dayEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(dayEtT).AddSeconds(-1).ToString();
                                    break;
                                case "M":
                                    sDate = DateTime.Parse(monEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(monEtT).AddSeconds(-1).ToString();
                                    break;
                                case "Y":
                                    sDate = DateTime.Parse(yearEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(yearEtT).AddSeconds(-1).ToString();
                                    break;
                            }

                            //if (flag == true)
                            //{
                            //    if (rtDBType == "EDNA")
                            //    {
                            //        ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //        ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                            //    }
                            //    else if (rtDBType == "HS")
                            //    {
                            //        hs.GetHistVal(pName, ref drvA, sDate);
                            //        hs.GetHistVal(pName, ref drvB, sDateT);
                            //    }
                            //    else
                            //    {
                            //        pk.GetHisValue(pName, sDate, ref drvA);
                            //        pk.GetHisValue(pName, sDateT, ref drvB);
                            //    }
                            //}
                            //else
                            //{
                            //    if (rtDBType == "EDNA")
                            //        ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //    else if (rtDBType == "HS")
                            //    {
                            //        hs.GetHistVal(pName, ref drvA, sDate);
                            //    }
                            //    else
                            //        pk.GetHisValue(pName, sDate, ref drvA);
                            //}

                            if (flag == true)
                            {
                                //传出数据
                                if (!ht.ContainsKey(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper()) && !ht.ContainsKey(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                {
                                    ht.Add(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(drvA.ToString(), num));
                                    ht.Add(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(drvB.ToString(), num));
                                }
                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                //公式数据
                                if (!htUn.ContainsKey(ParaID) && !htUnT.ContainsKey(ParaID))
                                {
                                    if (calc == "yes")
                                    {
                                        htUn.Add(ParaID, drvA.ToString());
                                        htUnT.Add(ParaID, drvB.ToString());
                                    }
                                    else
                                    {
                                        htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                        htUnT.Add(ParaID, sh.ShowPoint(drvB.ToString(), num));
                                    }
                                }
                            }
                            else
                            {
                                //传出参数
                                if (!ht.ContainsKey(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                    ht.Add(ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper(),sh.ShowPoint( drvA.ToString(),num));
                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                //公式参数
                                if (!htUn.ContainsKey(ParaID))
                                {
                                    if (calc == "yes")
                                        htUn.Add(ParaID, drvA.ToString());
                                    else
                                        htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                }

                            }
                        }
                    }
                    #endregion

                    #region 累计
                    sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='累计' and T_UNITID=0 and T_REPORTID='" + repName + "'";

                    sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                    if (repOrgid != "")
                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                    if (treeid != "")
                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                    if (sql.Contains("&where&"))
                        sql = sql.Replace("&where&", sqlWhere);

                    DataTable dtDiff = null;
                    dtDiff = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDiff = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                    if (dtDiff != null && dtDiff.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDiff.Rows.Count; i++)
                        {
                            //小数点数
                            num = dtDiff.Rows[i]["I_DECIMALPOINT"].ToString();
                            //测点名
                            pName = dtDiff.Rows[i]["T_TAGID"].ToString();
                            //参数ID
                            ParaID = dtDiff.Rows[i]["T_PARAID"].ToString();

                            //对时间类型做判断                                   
                            switch (dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper())
                            {
                                case "D":
                                    sDate = DateTime.Parse(dayEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(dayEtT).AddSeconds(-1).ToString();
                                    break;
                                case "M":
                                    sDate = DateTime.Parse(monEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(monEtT).AddSeconds(-1).ToString();
                                    break;
                                case "Y":
                                    sDate = DateTime.Parse(yearEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(yearEtT).AddSeconds(-1).ToString();
                                    break;
                            }

                            //if (flag == true)
                            //{
                            //    if (rtDBType == "EDNA")
                            //    {
                            //        ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //        ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                            //    }
                            //    else if (rtDBType == "HS")
                            //    {
                            //        hs.GetHistVal(pName, ref drvA, sDate);
                            //        hs.GetHistVal(pName, ref drvB, sDateT);
                            //    }
                            //    else
                            //    {
                            //        pk.GetHisValue(pName, sDate, ref drvA);
                            //        pk.GetHisValue(pName, sDateT, ref drvB);
                            //    }
                            //}
                            //else
                            //{
                            //    if (rtDBType == "EDNA")
                            //        ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //    else if (rtDBType == "HS")
                            //        hs.GetHistVal(pName, ref drvA, sDate);
                            //    else
                            //        pk.GetHisValue(pName, sDate, ref drvA);
                            //}

                            if (flag == true)
                            {
                                //传出数据
                                if (!ht.ContainsKey(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper()) && !ht.ContainsKey(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                {
                                    ht.Add(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(drvA.ToString(), num));
                                    ht.Add(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(drvB.ToString(), num));
                                }
                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                //公式数据
                                if (!htUn.ContainsKey(ParaID) && !htUnT.ContainsKey(ParaID))
                                {
                                    if (calc == "yes")
                                    {
                                        htUn.Add(ParaID, drvA.ToString());
                                        htUnT.Add(ParaID, drvB.ToString());
                                    }
                                    else
                                    {
                                        htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                        htUnT.Add(ParaID, sh.ShowPoint(drvB.ToString(), num));
                                    }
                                }
                            }
                            else
                            {
                                //传出参数
                                if (!ht.ContainsKey(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                    ht.Add(ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper(),sh.ShowPoint( drvA.ToString(),num));
                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                //公式参数
                                if (!htUn.ContainsKey(ParaID))
                                {
                                    if (calc == "yes")
                                        htUn.Add(ParaID, drvA.ToString());
                                    else
                                        htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                }
                            }
                        }
                    }

                    #endregion

                    #region SQL
                    sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='SQL' and T_UNITID=0 and T_REPORTID='" + repName + "'";

                    sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                    if (repOrgid != "")
                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                    if (treeid != "")
                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                    if (sql.Contains("&where&"))
                        sql = sql.Replace("&where&", sqlWhere);

                    DataTable dtSQL = null;
                    dtSQL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);
                    //else
                    //    dtSQL = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                    if (dtSQL != null && dtSQL.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSQL.Rows.Count; i++)
                        {
                            //小数点数
                            num = dtSQL.Rows[i]["I_DECIMALPOINT"].ToString();
                            //参数ID
                            ParaID = dtSQL.Rows[i]["T_PARAID"].ToString();

                            //对时间类型做判断                                   
                            switch (dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper())
                            {
                                case "D":
                                    qsrq = dayBt;
                                    qsrqT = dayBtT;
                                    sDate = DateTime.Parse(dayEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(dayEtT).AddSeconds(-1).ToString();
                                    break;
                                case "M":
                                    qsrq = monBt;
                                    qsrqT = monBtT;
                                    sDate = DateTime.Parse(monEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(monEtT).AddSeconds(-1).ToString();
                                    break;
                                case "Y":
                                    qsrq = yearBt;
                                    qsrqT = yearBtT;
                                    sDate = DateTime.Parse(yearEt).AddSeconds(-1).ToString();
                                    sDateT = DateTime.Parse(yearEtT).AddSeconds(-1).ToString();
                                    break;
                            }

                            //本期
                            sql = dtSQL.Rows[i]["T_SQL"].ToString();

                            sql = sql.Replace("&bt&", qsrq);
                            sql = sql.Replace("&et&", sDate);

                            if (sql.Contains("&ORGID&"))
                            {
                                sql = sql.Replace("&ORGID&", repOrgid);
                            }

                            if (htParam.Count > 0)
                            {
                                foreach (System.Collections.DictionaryEntry item in htParam)
                                {
                                    if (sql.Contains("&" + item.Key.ToString() + "&"))
                                        sql.Replace("&" + item.Key.ToString() + "&", item.Value.ToString());
                                }
                            }
                            obj = dl.RunSingle(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    obj = DBsql.RunSingle(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    obj = DBdb2.RunSingle(sql, out errMsg);
                            //else
                            //    obj = SAC.OracleHelper.OracleHelper.GetSingle(sql);

                            res = "";
                            if (obj != null && obj != "")
                            {
                                if (obj.ToString() != "")
                                    res = obj.ToString();
                                else
                                    res = "0";
                            }
                            else
                            { res = "0"; }

                            //传出数据
                            if (!ht.ContainsKey(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                ht.Add(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(res, num));
                            else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                            //公式数据
                            if (!htUn.ContainsKey(ParaID))
                            {
                                if (calc == "yes")
                                    htUn.Add(ParaID, res);
                                else
                                    htUn.Add(ParaID, sh.ShowPoint(res, num));
                            }

                            //同期
                            if (flag == true)
                            {
                                sql = dtSQL.Rows[i]["T_SQL"].ToString();

                                sql = sql.Replace("&bt&", qsrqT);
                                sql = sql.Replace("&et&", sDateT);

                                if (sql.Contains("&ORGID&"))
                                    sql = sql.Replace("&ORGID&", repOrgid);

                                if (htParam.Count > 0)
                                {
                                    foreach (System.Collections.DictionaryEntry item in htParam)
                                    {
                                        if (sql.Contains("&" + item.Key.ToString() + "&"))
                                            sql.Replace("&" + item.Key.ToString() + "&", item.Value.ToString());
                                    }
                                }
                                obj = dl.RunSingle(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    obj = DBsql.RunSingle(sql, out errMsg);
                                //else if (rlDBType == "DB2")
                                //    obj = DBdb2.RunSingle(sql, out errMsg);
                                //else
                                //    obj = SAC.OracleHelper.OracleHelper.GetSingle(sql);

                                res = "";
                                if (obj != null && obj != "")
                                {
                                    if (obj.ToString() != "")
                                        res = obj.ToString();
                                    else
                                        res = "0";
                                }
                                else
                                { res = "0"; }

                                //传出数据
                                if (!ht.ContainsKey(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                    ht.Add(ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(res, num));

                                //公式数据
                                if (!htUnT.ContainsKey(ParaID))
                                {
                                    if (calc == "yes")
                                        htUnT.Add(ParaID, res);
                                    else
                                        htUnT.Add(ParaID, sh.ShowPoint(res, num));
                                }
                            }
                        }
                    }
                    #endregion

                    #region 电量
                    sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='电量' and T_UNITID=0 and T_REPORTID='" + repName + "'";

                    sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                    if (repOrgid != "")
                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                    if (treeid != "")
                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                    if (sql.Contains("&where&"))
                        sql = sql.Replace("&where&", sqlWhere);

                    DataTable dtDL = null;
                    dtDL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtDL = DBdb2.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDL = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                    if (dtDL.Rows.Count > 0 && dtDL != null)
                    {
                        for (int i = 0; i < dtDL.Rows.Count; i++)
                        {
                            //电量测点
                            dlID = dtDL.Rows[i]["T_POWERID"].ToString();
                            //小数点数
                            num = dtDL.Rows[i]["I_DECIMALPOINT"].ToString();
                            //参数ID
                            ParaID = dtDL.Rows[i]["T_PARAID"].ToString();

                            //对时间类型做判断                                   
                            switch (dtDL.Rows[i]["T_TimeType"].ToString().ToUpper())
                            {
                                case "D":
                                    qsrq = dayBt;
                                    qsrqT = dayBtT;
                                    jsrq = dayEt;
                                    jsrqT = dayEtT;
                                    break;
                                case "M":
                                    qsrq = monBt;
                                    qsrqT = monBtT;
                                    jsrq = monEt;
                                    jsrqT = monEtT;
                                    break;
                                case "Y":
                                    qsrq = yearBt;
                                    qsrqT = yearBtT;
                                    jsrq = yearEt;
                                    jsrqT = yearEtT;
                                    break;
                            }

                            //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                            mon = "";

                            if (dtDL.Rows[i]["T_FORMULA"].ToString() != "")
                                mon = drvA.ToString() + dtDL.Rows[i]["T_FORMULA"].ToString();
                            else
                                mon = drvA.ToString();

                            res = "";
                            try
                            { res = StrHelper.Cale(mon); }
                            catch
                            { res = "0"; }

                            //传出数据
                            if (!ht.ContainsKey(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                ht.Add(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(res, num));
                            else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                            //公式数据
                            if (!htUn.ContainsKey(ParaID))
                            {
                                if (calc == "yes")
                                    htUn.Add(ParaID, res);
                                else
                                    htUn.Add(ParaID, sh.ShowPoint(res, num));
                            }

                            if (flag == true)
                            {
                                //drvB = double.Parse(pc.GetPower(drvB, dlID, qsrqT, jsrqT, bm).ToString());

                                mon = "";

                                if (dtDL.Rows[i]["T_FORMULA"].ToString() != "")
                                    mon = drvB.ToString() + dtDL.Rows[i]["T_FORMULA"].ToString();
                                else
                                    mon = drvB.ToString();

                                res = "";
                                try
                                { res = StrHelper.Cale(mon); }
                                catch
                                { res = "0"; }

                                //传出数据
                                if (!ht.ContainsKey(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                    ht.Add(ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(res, num));

                                //公式数据
                                if (!htUnT.ContainsKey(ParaID))
                                {
                                    if (calc == "yes")
                                        htUnT.Add(ParaID, res);
                                    else
                                        htUnT.Add(ParaID, sh.ShowPoint(res, num));
                                }
                            }
                        }
                    }

                    #endregion

                    #region 其它
                    sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='其它' and T_UNITID=0 and T_REPORTID='" + repName + "'";

                    sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                    if (repOrgid != "")
                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                    if (treeid != "")
                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                    if (sql.Contains("&where&"))
                        sql = sql.Replace("&where&", sqlWhere);

                    DataTable dtQT = null;
                    dtQT = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtQT = DBdb2.RunDataTable(sql, out errMsg);
                    //else
                    //    dtQT = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                    if (dtQT.Rows.Count > 0 && dtQT != null)
                    {
                        for (int i = 0; i < dtQT.Rows.Count; i++)
                        {
                            num = dtQT.Rows[i]["I_DECIMALPOINT"].ToString();
                            //参数ID
                            ParaID = dtQT.Rows[i]["T_PARAID"].ToString();

                            //对时间类型做判断                                   
                            switch (dtQT.Rows[i]["T_TimeType"].ToString().ToUpper())
                            {
                                case "D":
                                    qsrq = dayBt;
                                    qsrqT = dayBtT;
                                    jsrq = dayEt;
                                    jsrqT = dayEtT;
                                    break;
                                case "M":
                                    qsrq = monBt;
                                    qsrqT = monBtT;
                                    jsrq = monEt;
                                    jsrqT = monEtT;
                                    break;
                                case "Y":
                                    qsrq = yearBt;
                                    qsrqT = yearBtT;
                                    jsrq = yearEt;
                                    jsrqT = yearEtT;
                                    break;
                            }
                        }
                    }
                    #endregion

                    #region 公式

                    sqlWhere = " where 1=1 and I_FORMULALEVEL=" + gsjb + " and T_PARATYPE='公式' and T_UNITID=0 and T_REPORTID='" + repName + "'";

                    sql = "select * from T_sheet_sheetPara &where& and (T_TimeType='D' or T_TimeType='M' or T_TimeType='Y')";

                    if (repOrgid != "")
                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                    if (treeid != "")
                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                    if (sql.Contains("&where&"))
                        sql = sql.Replace("&where&", sqlWhere);

                    DataTable dtMon = null;
                    dtMon = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtMon = DBdb2.RunDataTable(sql, out errMsg);
                    //else
                    //    dtMon = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                    if (dtMon != null && dtMon.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtMon.Rows.Count; i++)
                        {
                            num = dtMon.Rows[i]["I_DECIMALPOINT"].ToString();
                            //参数ID
                            ParaID = dtMon.Rows[i]["T_PARAID"].ToString();

                            string pMon = dtMon.Rows[i]["T_FORMULA"].ToString();
                            string paras = dtMon.Rows[i]["T_FORMULAPARA"].ToString();
                            string month = pm.retMon(htGY, htUn, paras, pMon);

                            if (month != "")
                            {
                                res = "";
                                try
                                { res = StrHelper.Cale(month); }
                                catch
                                { res = "0"; }

                                //传出数据
                                if (!ht.ContainsKey(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper()))
                                    ht.Add(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper(), sh.ShowPoint(res, num));
                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                //公式数据
                                if (!htUn.ContainsKey(ParaID))
                                {
                                    if (calc == "yes")
                                        htUn.Add(ParaID, res);
                                    else
                                        htUn.Add(ParaID, sh.ShowPoint(res, num));
                                }
                            }

                            if (flag == true)
                            {
                                string monthT = pm.retMon(htGYT, htUnT, paras, pMon);

                                if (monthT != "")
                                {
                                    res = "";
                                    try
                                    { res = StrHelper.Cale(monthT); }
                                    catch
                                    { res = "0"; }

                                    //传出数据
                                    if (!ht.ContainsKey(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T"))
                                        ht.Add(ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T", sh.ShowPoint(res, num));

                                    //公式数据
                                    if (!htUnT.ContainsKey(ParaID))
                                    {
                                        if (calc == "yes")
                                            htUnT.Add(ParaID, res);
                                        else
                                            htUnT.Add(ParaID, sh.ShowPoint(res, num));
                                    }

                                }
                            }
                        }
                    }

                    #endregion
                }

                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "----全厂结束!");
            }

            #endregion

            #region 调试模式
            if (TSMS == "yes")
            {
                ArrayList listGy = new ArrayList(htGY.Keys);
                ArrayList listGyt = new ArrayList(htGYT.Keys);
                ArrayList listUn = new ArrayList(htUn.Keys);
                ArrayList listUnt = new ArrayList(htUnT.Keys);
                ArrayList listHt = new ArrayList(ht.Keys);

                listGy.Sort();
                listGyt.Sort();
                listUn.Sort();
                listUnt.Sort();
                listHt.Sort();

                foreach (string skey in listGy)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "--本期--" + skey + "---" + htGY[skey]); }

                foreach (string skey in listUn)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "--本期--" + skey + "---" + htUn[skey]); }

                foreach (string skey in listGyt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "--同期--" + skey + "---" + htGYT[skey]); }

                foreach (string skey in listUnt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "--同期--" + skey + "---" + htUnT[skey]); }

                foreach (string skey in listHt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "--报表数据输出--" + skey + "---" + ht[skey]); }
            }

            #endregion

            #endregion

            #region 年明细报表模块
            Hashtable htYY = this.RetHtRepYearInfo(repName, repOrgid, treeid, htParam, date, flag, isZy, out errMsg);

            if (htYY.Count > 0)
            {
                foreach (DictionaryEntry de in htYY)
                {
                    ht.Add(de.Key.ToString(), de.Value.ToString());
                }
            }

            #endregion

            //if (rtDBType == "HS")
                //hs.DisConnect();

            return ht;
        }

        /// <summary>
        /// 中电投保表模板(年明细)     
        /// </summary>
        /// <param name="repName">报表模板ID</param>
        /// <param name="repOrgid">组织机构ID</param>
        /// <param name="treeid">组织机构树ID</param>
        /// <param name="param">SQL参数集合</param>     
        /// <param name="date">日期</param>
        /// <param name="flag">是否计算同期</param>
        /// <param name="isZY">是否计算最优</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        private Hashtable RetHtRepYearInfo(string repName, string repOrgid, string treeid, Hashtable param, DateTime date, bool flag, bool isZY, out string errMsg)
        {
            int isDay, ret = 0;
            object obj = null;
            double drvA = 0, drvAT = 0, valGY = 0;
            string sDate1 = "", sDate1T = "", sqlWhere = "", dateCount = "", paraIDB = "", paraIDT = "";
            string num = "", ParaID = "", pName = "", dlID = "", mon = "", bm = "", res = "", resT = "";
            string strVal = "", unit = "";

            string[] qsrq = new string[12]; //取值日期
            string[] jsrq = new string[12];
            string[] sDate = new string[12];
            string[] qsrqT = new string[12];
            string[] jsrqT = new string[12];
            string[] sDateT = new string[12];

            Hashtable ht = new Hashtable();
            Hashtable htZY = new Hashtable();   //内部运算
            Hashtable htZYT = new Hashtable();  //内部运算
            Hashtable htUn = new Hashtable();   //内部运算
            Hashtable htUnT = new Hashtable();  //内部运算
            Hashtable htGY = new Hashtable();   //内部运算
            Hashtable htGYT = new Hashtable();  //内部运算
            DateTime dtNow = DateTime.Now;

            errMsg = "";

            //this.init();

            //CRD_HS_API.RTDB_HS hs = new CRD_HS_API.RTDB_HS();

            //if (rtDBType == "HS")
            //    hs.Connect(ip, port, user, pwd);

            #region 日期

            if (date.Year <= dtNow.Year)
            {
                if (date.Year == dtNow.Year)
                    isDay = 0;
                else
                    isDay = 1;
                //本期时间
                if (isDay == 0)
                {
                    date=DateTime.Parse(date.ToString("yyyy-01-01 0:00:00"));

                    for (int i = 0; i < DateTime.Now.Month; i++)
                    {
                        qsrq[i] =date.AddMonths(+i).ToString("yyyy-MM-dd H:mm:ss");
                        jsrq[i] = date.AddMonths(+i).AddMonths(+1).ToString("yyyy-MM-dd H:mm:ss");
                        sDate[i] = date.AddMonths(+i).AddMonths(+1).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");

                        if (DateTime.Parse(qsrq[i]).Month == DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 0:00:00")).Month)
                        {
                            jsrq[i] = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");
                            sDate[i] = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 0:00:00")).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");
                        }
                        sDate[11] = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");
                    }
                }
                else
                {
                    date=DateTime.Parse(date.ToString("yyyy-01-01 0:00:00"));
                    for (int i = 0; i < 12; i++)
                    {
                        qsrq[i] = date.AddMonths(+i).ToString("yyyy-MM-dd H:mm:ss");
                        jsrq[i] = date.AddMonths(+i).AddMonths(+1).ToString("yyyy-MM-dd H:mm:ss");
                        sDate[i] = date.AddMonths(+i).AddMonths(+1).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");
                    }
                }
                DateTime dtT = DateTime.Parse(qsrq[0]).AddYears(-1);

                //同期时间
                for (int i = 0; i < 12; i++)
                {
                    qsrqT[i] = dtT.AddMonths(i).ToString("yyyy-MM-dd H:mm:ss");
                    jsrqT[i] = dtT.AddMonths(i).AddMonths(+1).ToString("yyyy-MM-dd H:mm:ss");
                    sDateT[i] = dtT.AddMonths(i).AddMonths(+1).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");
                }
            }
            #endregion

            #region 机组数据读取
            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----年明细----机组轮转开始!");

            sqlWhere = "where 1=1 and T_REPORTID='" + repName + "' ";

            if (repOrgid != "")
                sqlWhere += " and t_orgid='" + repOrgid + "' ";

            if (treeid != "")
                sqlWhere += " and T_Treeid='" + treeid + "' ";

            sql = "SELECT DISTINCT T_UNITID FROM T_sheet_sheetPara &where& and T_UNITID!=0 and T_TimeType='YY' ORDER BY T_UNITID ";

            if (sql.Contains("&where&"))
                sql = sql.Replace("&where&", sqlWhere);

            DataTable dtLevUnit = null;
            dtLevUnit = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevUnit = DBsql.RunDataTable(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    dtLevUnit = DBdb2.RunDataTable(sql, out errMsg);
            //else if (rlDBType == "Oracle")
            //    dtLevUnit = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

            if (dtLevUnit != null && dtLevUnit.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repOrgid + "----机组轮转开始!");

                for (int a = 0; a < dtLevUnit.Rows.Count; a++)
                {
                    unit = dtLevUnit.Rows[a]["T_UNITID"].ToString();

                    for (int k = 0; k < 12; k++)
                    {
                        if (k < qsrq.Length)
                        {
                            dateCount = (k + 1).ToString();

                            if (qsrq[k] != null)
                                sDate1 = DateTime.Parse(jsrq[k]).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                            if (qsrqT[k] != null)
                                sDate1T = DateTime.Parse(jsrqT[k]).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                            sqlWhere = " where 1=1 and T_REPORTID ='" + repName + "' and T_UNITID=" + unit + " and I_FORMULALEVEL!=0 and T_TimeType='YY' ";

                            if (repOrgid != "")
                                sqlWhere += " and t_orgid='" + repOrgid + "' ";

                            if (treeid != "")
                                sqlWhere += " and T_Treeid='" + treeid + "' ";

                            sql = "SELECT DISTINCT I_FORMULALEVEL FROM T_sheet_sheetPara &where& ";

                            if (sql.Contains("&where&"))
                                sql = sql.Replace("&where&", sqlWhere);

                            DataTable dtLel = null;
                            dtLel = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtLel = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtLel = DBdb2.RunDataTable(sql, out errMsg);
                            //else
                            //    dtLel = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                            if (dtLel.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtLel.Rows.Count; j++)
                                {
                                    string gsjb = dtLel.Rows[j]["I_FORMULALEVEL"].ToString();

                                    #region 平均

                                    sqlWhere = " where 1=1 ";

                                    sql = "select * from T_sheet_sheetPara &where& and T_REPORTID='" + repName + "' and T_UNITID=" + unit + " and T_PARATYPE='平均' and T_TimeType='YY' and I_FORMULALEVEL=" + gsjb; //"select * from T_sheet_sheetPara &where& ";

                                    if (repOrgid != "")
                                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                                    if (treeid != "")
                                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                                    if (sql.Contains("&where&"))
                                        sql = sql.Replace("&where&", sqlWhere);

                                    DataTable dtAvg = null;
                                    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                                    //else if (rlDBType == "DB2")
                                    //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);
                                    //else
                                    //    dtAvg = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                                    if (dtAvg != null && dtAvg.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtAvg.Rows.Count; i++)
                                        {
                                            //小数点
                                            num = dtAvg.Rows[i]["I_DECIMALPOINT"].ToString();
                                            //测点名
                                            pName = dtAvg.Rows[i]["T_TAGID"].ToString();
                                            //参数ID
                                            ParaID = dtAvg.Rows[i]["T_PARAID"].ToString();
                                            // 
                                            if (qsrq[k] != null)
                                            {
                                                //if (rtDBType == "EDNA")
                                                //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                                                //else if (rtDBType == "HS")
                                                //    hs.GetHistVal(pName, ref drvA, sDate1);
                                                //else
                                                //    pk.GetHisValue(pName, sDate1, ref drvA);
                                            }
                                            else
                                                drvA = 0;

                                            if (flag == true)
                                            {
                                                if (qsrqT[k] != null)
                                                {
                                                    //if (rtDBType == "EDNA")
                                                    //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                                                    //else if (rtDBType == "HS")
                                                    //    hs.GetHistVal(pName, ref drvAT, sDate1T);
                                                    //else
                                                    //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                                                }
                                                else
                                                    drvAT = 0;
                                            }

                                            paraIDB = ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper() + "_" + dateCount;
                                            paraIDT = ParaID + "_" + dtAvg.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T_" + dateCount;

                                            if (flag == true)
                                            {
                                                //传出数据
                                                if (!ht.ContainsKey(paraIDB) && !ht.ContainsKey(paraIDT))
                                                {
                                                    ht.Add(paraIDB, sh.ShowPoint(drvA.ToString(), num));
                                                    ht.Add(paraIDT, sh.ShowPoint(drvAT.ToString(), num));
                                                }
                                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                                //公式数据
                                                if (!htUn.ContainsKey(ParaID) && !htUnT.ContainsKey(ParaID))
                                                {
                                                    if (calc == "yes")
                                                    {
                                                        htUn.Add(ParaID, drvA.ToString());
                                                        htUnT.Add(ParaID, drvAT.ToString());
                                                    }
                                                    else
                                                    {
                                                        htUn.Add(ParaID,sh.ShowPoint( drvA.ToString(),num));
                                                        htUnT.Add(ParaID,sh.ShowPoint( drvAT.ToString(),num));
                                                    }
                                                }
                                                else
                                                {
                                                    if (calc == "yes")
                                                    {
                                                        strVal = htUn[ParaID].ToString() + ',' + drvA.ToString();
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, strVal);

                                                        strVal = htUnT[ParaID].ToString() + ',' + drvAT.ToString();
                                                        htUnT.Remove(ParaID);
                                                        htUnT.Add(ParaID, strVal);
                                                    }
                                                    else
                                                    {
                                                        strVal = htUn[ParaID].ToString() + ',' + sh.ShowPoint(drvA.ToString(), num);
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, strVal);

                                                        strVal = htUnT[ParaID].ToString() + ',' + sh.ShowPoint(drvAT.ToString(), num);
                                                        htUnT.Remove(ParaID);
                                                        htUnT.Add(ParaID, strVal);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //传出参数
                                                if (!ht.ContainsKey(paraIDB))
                                                    ht.Add(paraIDB, sh.ShowPoint( drvA.ToString(),num));
                                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                                //公式参数
                                                if (!htUn.ContainsKey(ParaID))
                                                { 
                                                    if(calc=="yes")
                                                        htUn.Add(ParaID, drvA.ToString()); 
                                                    else
                                                        htUn.Add(ParaID,sh.ShowPoint( drvA.ToString(),num)); 
                                                }
                                                else
                                                {
                                                    if (calc == "yes")
                                                        strVal = htUn[ParaID].ToString() + ',' + drvA.ToString();
                                                    else
                                                        strVal = htUn[ParaID].ToString() + ',' + sh.ShowPoint(drvA.ToString(), num);

                                                    htUn.Remove(ParaID);
                                                    htUn.Add(ParaID, strVal);
                                                }
                                            }

                                            //计算最优值
                                            if (isZY == true)
                                            {
                                                if (!htZY.ContainsKey(ParaID))
                                                { htZY.Add(ParaID, dtAvg.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + drvA.ToString()); }
                                                else
                                                {
                                                    string zy = htZY[ParaID] + "," + drvA.ToString();
                                                    htZY.Remove(ParaID);
                                                    htZY.Add(ParaID, zy);
                                                }

                                                if (flag == true)
                                                {
                                                    if (!htZYT.ContainsKey(ParaID))
                                                    { htZYT.Add(ParaID, dtAvg.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + drvAT.ToString()); }
                                                    else
                                                    {
                                                        string zy = htZYT[ParaID] + "," + drvAT.ToString();
                                                        htZYT.Remove(ParaID);
                                                        htZYT.Add(ParaID, zy);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 累计

                                    sqlWhere = " where 1=1 ";

                                    sql = "select * from T_sheet_sheetPara &where& and T_REPORTID='" + repName + "' and T_UNITID=" + unit + " and T_PARATYPE='累计' and T_TimeType='YY' and I_FORMULALEVEL=" + gsjb; //"select * from T_sheet_sheetPara &where& ";

                                    if (repOrgid != "")
                                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                                    if (treeid != "")
                                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                                    if (sql.Contains("&where&"))
                                        sql = sql.Replace("&where&", sqlWhere);
                                                                      
                                    DataTable dtDiff = null;
                                    dtDiff = dl.RunDataTable(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                                    //else if (rlDBType == "DB2")
                                    //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);
                                    //else
                                    //    dtDiff = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                                    if (dtDiff != null && dtDiff.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtDiff.Rows.Count; i++)
                                        {
                                            //小数点
                                            num = dtDiff.Rows[i]["I_DECIMALPOINT"].ToString();
                                            //测点名
                                            pName = dtDiff.Rows[i]["T_TAGID"].ToString();
                                            //参数ID
                                            ParaID = dtDiff.Rows[i]["T_PARAID"].ToString();

                                            if (qsrq[k] != null)
                                            {
                                                //if (rtDBType == "EDNA")
                                                //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                                                //else if (rtDBType == "HS")
                                                //    hs.GetHistVal(pName, ref drvA, sDate1);
                                                //else
                                                //    pk.GetHisValue(pName, sDate1, ref drvA);
                                            }
                                            else
                                                drvA = 0;

                                            if (flag == true)
                                            {
                                                if (qsrqT[k] != null)
                                                {
                                                    //if (rtDBType == "EDNA")
                                                    //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                                                    //else if (rtDBType == "HS")
                                                    //    hs.GetHistVal(pName, ref drvAT, sDate1T);
                                                    //else
                                                    //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                                                }
                                                else
                                                    drvAT = 0;
                                            }

                                            paraIDB = ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper() + "_" + dateCount;
                                            paraIDT = ParaID + "_" + dtDiff.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T_" + dateCount;

                                            if (flag == true)
                                            {
                                                //传出数据
                                                if (!ht.ContainsKey(paraIDB) && !ht.ContainsKey(paraIDT))
                                                {
                                                    ht.Add(paraIDB, sh.ShowPoint(drvA.ToString(), num));
                                                    ht.Add(paraIDT, sh.ShowPoint(drvAT.ToString(), num));
                                                }
                                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                                //公式数据
                                                if (!htUn.ContainsKey(ParaID) && !htUnT.ContainsKey(ParaID))
                                                {
                                                    if (calc == "yes")
                                                    {
                                                        htUn.Add(ParaID, drvA.ToString());
                                                        htUnT.Add(ParaID, drvAT.ToString());
                                                    }
                                                    else
                                                    {
                                                        htUn.Add(ParaID, sh.ShowPoint( drvA.ToString(),num));
                                                        htUnT.Add(ParaID,sh.ShowPoint( drvAT.ToString(),num));
                                                    }
                                                }
                                                else
                                                {
                                                    if (calc == "yes")
                                                    {
                                                        strVal = htUn[ParaID].ToString() + ',' + drvA.ToString();
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, strVal);

                                                        strVal = htUnT[ParaID].ToString() + ',' + drvAT.ToString();
                                                        htUnT.Remove(ParaID);
                                                        htUnT.Add(ParaID, strVal);
                                                    }
                                                    else
                                                    {
                                                        strVal = htUn[ParaID].ToString() + ',' +sh.ShowPoint( drvA.ToString(),num);
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, strVal);

                                                        strVal = htUnT[ParaID].ToString() + ',' + sh.ShowPoint(drvAT.ToString(), num);
                                                        htUnT.Remove(ParaID);
                                                        htUnT.Add(ParaID, strVal);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //传出参数
                                                if (!ht.ContainsKey(paraIDB))
                                                {  ht.Add(paraIDB,sh.ShowPoint( drvA.ToString(),num)); }
                                                else { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                                //公式参数
                                                if (!htUn.ContainsKey(ParaID))
                                                {
                                                    if (calc == "yes")
                                                        htUn.Add(ParaID, drvA.ToString());
                                                    else
                                                        htUn.Add(ParaID, sh.ShowPoint(drvA.ToString(), num));
                                                }
                                                else
                                                {
                                                    if (calc == "yes")
                                                        strVal = htUn[ParaID].ToString() + ',' + drvA.ToString();
                                                    else
                                                        strVal = htUn[ParaID].ToString() + ',' + sh.ShowPoint(drvA.ToString(), num);
                                                    htUn.Remove(ParaID);
                                                    htUn.Add(ParaID, strVal);
                                                }
                                            }

                                            //计算最优值
                                            if (isZY == true)
                                            {
                                                if (!htZY.ContainsKey(ParaID))
                                                { htZY.Add(ParaID, dtDiff.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + drvA.ToString()); }
                                                else
                                                {
                                                    string zy = htZY[ParaID] + "," + drvA.ToString();
                                                    htZY.Remove(ParaID);
                                                    htZY.Add(ParaID, zy);
                                                }

                                                if (flag == true)
                                                {
                                                    if (!htZYT.ContainsKey(ParaID))
                                                    { htZYT.Add(ParaID, dtDiff.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + drvAT.ToString()); }
                                                    else
                                                    {
                                                        string zy = htZYT[ParaID] + "," + drvAT.ToString();
                                                        htZYT.Remove(ParaID);
                                                        htZYT.Add(ParaID, zy);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region SQL

                                    sqlWhere = " where 1=1 ";

                                    sql = "select * from T_sheet_sheetPara &where& and T_REPORTID='" + repName + "' and T_UNITID=" + unit + " and T_PARATYPE='SQL' and T_TimeType='YY' and I_FORMULALEVEL=" + gsjb; //"select * from T_sheet_sheetPara &where& ";

                                    if (repOrgid != "")
                                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                                    if (treeid != "")
                                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                                    if (sql.Contains("&where&"))
                                        sql = sql.Replace("&where&", sqlWhere);

                                    DataTable dtSQL = null;
                                    dtSQL = dl.RunDataTable(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                                    //else if (rlDBType == "DB2")
                                    //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);
                                    //else
                                    //    dtSQL = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                                    if (dtSQL != null && dtSQL.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtSQL.Rows.Count; i++)
                                        {
                                            num = dtSQL.Rows[i]["I_DECIMALPOINT"].ToString();

                                            sql = dtSQL.Rows[i]["T_SQL"].ToString();
                                            //参数ID
                                            ParaID = dtSQL.Rows[i]["T_PARAID"].ToString();

                                            paraIDB = ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_" + dateCount;
                                            paraIDT = ParaID + "_" + dtSQL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T_" + dateCount;

                                            if (qsrq[k] != null)
                                            {                                            
                                                sql = sql.Replace("&bt&", qsrq[k].ToString());
                                                sql = sql.Replace("&et&", sDate1);
                                               
                                                if (sql.Contains("&ORGID&"))
                                                    sql = sql.Replace("&ORGID&", repOrgid);

                                                if (param.Count > 0)
                                                {
                                                    foreach (System.Collections.DictionaryEntry item in param)
                                                    {
                                                        if (sql.Contains("&" + item.Key.ToString() + "&"))
                                                            sql.Replace("&" + item.Key.ToString() + "&", item.Value.ToString());
                                                    }
                                                }
                                                obj = dl.RunSingle(sql, out errMsg);
                                                //if (rlDBType == "SQL")
                                                //    obj = DBsql.RunSingle(sql, out errMsg);
                                                //else if (rlDBType == "DB2")
                                                //    obj = DBdb2.RunSingle(sql, out errMsg);
                                                //else
                                                //    obj = SAC.OracleHelper.OracleHelper.GetSingle(sql);

                                                res = "";
                                                if (obj != null && obj != "")
                                                {
                                                    if (obj.ToString() != "")
                                                        res = obj.ToString();
                                                    else
                                                        res = "0";
                                                }
                                                else
                                                { res = "0"; }
                                            }
                                            else
                                                res = "0";

                                            //传出数据
                                            if (!ht.ContainsKey(paraIDB))
                                                ht.Add(paraIDB, sh.ShowPoint(res, num));
                                            else
                                            { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                            //公式数据
                                            if (!htUn.ContainsKey(ParaID))
                                            {
                                                if (calc == "yes")
                                                    htUn.Add(ParaID, res);
                                                else
                                                    htUn.Add(ParaID, sh.ShowPoint(res, num));
                                            }
                                            else
                                            {
                                                if (calc == "yes")
                                                    strVal = htUn[ParaID].ToString() + ',' + res;
                                                else
                                                    strVal = htUn[ParaID].ToString() + ',' + sh.ShowPoint(res, num);

                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, strVal);
                                            }

                                            if (flag == true)
                                            {
                                                sql = dtSQL.Rows[i]["T_SQL"].ToString();

                                                if (qsrqT[k] != null)
                                                {                                                   
                                                    sql = sql.Replace("&bt&", qsrqT[k].ToString());
                                                    sql = sql.Replace("&et&", sDate1T);
                                                   
                                                    if (sql.Contains("&ORGID&"))
                                                        sql = sql.Replace("&ORGID&", repOrgid);

                                                    if (param.Count > 0)
                                                    {
                                                        foreach (System.Collections.DictionaryEntry item in param)
                                                        {
                                                            if (sql.Contains("&" + item.Key.ToString() + "&"))
                                                                sql.Replace("&" + item.Key.ToString() + "&", item.Value.ToString());
                                                        }
                                                    }
                                                    obj = dl.RunSingle(sql, out errMsg);
                                                    //if (rlDBType == "SQL")
                                                    //    obj = DBsql.RunSingle(sql, out errMsg);
                                                    //else if (rlDBType == "DB2")
                                                    //    obj = DBdb2.RunSingle(sql, out errMsg);
                                                    //else
                                                    //    obj = SAC.OracleHelper.OracleHelper.GetSingle(sql);

                                                    resT = "";
                                                    if (obj != null && obj != "")
                                                    {
                                                        if (obj.ToString() != "")
                                                            resT = obj.ToString();
                                                        else
                                                            resT = "0";
                                                    }
                                                    else
                                                        resT = "0";
                                                }
                                                else
                                                    resT = "0";

                                                //传出数据
                                                if (!ht.ContainsKey(paraIDT))
                                                    ht.Add(paraIDT, sh.ShowPoint(resT, num));
                                                else
                                                { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                                //公式数据
                                                if (!htUnT.ContainsKey(ParaID))
                                                {
                                                    if (calc == "yes")
                                                        htUnT.Add(ParaID, resT);
                                                    else
                                                        htUnT.Add(ParaID, sh.ShowPoint(resT, num));
                                                }
                                                else
                                                {
                                                    if (calc == "yes")
                                                        strVal = htUnT[ParaID].ToString() + ',' + resT;
                                                    else
                                                        strVal = htUnT[ParaID].ToString() + ',' + sh.ShowPoint(resT, num);

                                                    htUnT.Remove(ParaID);
                                                    htUnT.Add(ParaID, strVal);
                                                }

                                            }
                                            //最优值计算
                                            if (isZY == true)
                                            {
                                                if (!htZY.ContainsKey(ParaID))
                                                { htZY.Add(ParaID, dtSQL.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + res); }
                                                else
                                                {
                                                    string zy = htZY[ParaID] + "," + res;
                                                    htZY.Remove(ParaID);
                                                    htZY.Add(ParaID, zy);
                                                }

                                                if (flag == true)
                                                {
                                                    if (!htZYT.ContainsKey(ParaID))
                                                    { htZYT.Add(ParaID, dtSQL.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + resT); }
                                                    else
                                                    {
                                                        string zy = htZYT[ParaID] + "," + resT;
                                                        htZYT.Remove(ParaID);
                                                        htZYT.Add(ParaID, zy);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 电量

                                    sqlWhere = " where 1=1 ";

                                    sql = "select * from T_sheet_sheetPara &where& and T_REPORTID='" + repName + "' and T_UNITID=" + unit + " and T_PARATYPE='电量' and T_TimeType='YY' and I_FORMULALEVEL=" + gsjb;

                                    if (repOrgid != "")
                                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                                    if (treeid != "")
                                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                                    if (sql.Contains("&where&"))
                                        sql = sql.Replace("&where&", sqlWhere);

                                    DataTable dtDL = null;
                                    dtDL = dl.RunDataTable(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                                    //else if (rlDBType == "DB2")
                                    //    dtDL = DBdb2.RunDataTable(sql, out errMsg);
                                    //else
                                    //    dtDL = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                                    if (dtDL.Rows.Count > 0 && dtDL != null)
                                    {
                                        for (int i = 0; i < dtDL.Rows.Count; i++)
                                        {
                                            num = dtDL.Rows[i]["I_DECIMALPOINT"].ToString();

                                            dlID = dtDL.Rows[i]["T_POWERID"].ToString();
                                            //参数ID
                                            ParaID = dtDL.Rows[i]["T_PARAID"].ToString();

                                            paraIDB = ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_" + dateCount;
                                            paraIDT = ParaID + "_" + dtDL.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T_" + dateCount;

                                            //if (qsrq[k] != null)
                                            //    //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq[k], jsrq[k], bm).ToString());
                                            //else
                                            //    drvA = 0;

                                            mon = "";

                                            if (dtDL.Rows[i]["I_FORMULALEVEL"].ToString() != "")
                                                mon = drvA.ToString() + dtDL.Rows[i]["I_FORMULALEVEL"].ToString();
                                            else
                                                mon = drvA.ToString();

                                            res = "";
                                            try
                                            { res = StrHelper.Cale(mon); }
                                            catch
                                            { res = "0"; }

                                            //传出数据
                                            if (!ht.ContainsKey(paraIDB))
                                                ht.Add(paraIDB, sh.ShowPoint(res, num));
                                            else
                                            { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                            //公式数据
                                            if (!htUn.ContainsKey(ParaID))
                                            {
                                                if (calc == "yes")
                                                    htUn.Add(ParaID, res);
                                                else
                                                    htUn.Add(ParaID, sh.ShowPoint(res, num));
                                            }
                                            else
                                            {
                                                if (calc == "yes")
                                                    strVal = htUn[ParaID].ToString() + ',' + res;
                                                else
                                                    strVal = htUn[ParaID].ToString() + ',' + sh.ShowPoint(res, num);

                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, strVal);
                                            }

                                            if (flag == true)
                                            {
                                                //if (qsrqT[k] != null)
                                                //    //drvAT = double.Parse(pc.GetPower(drvAT, dlID, qsrqT[k], jsrqT[k], bm).ToString());
                                                //else
                                                //    drvAT = 0;

                                                mon = "";

                                                if (dtDL.Rows[i]["I_FORMULALEVEL"].ToString() != "")
                                                    mon = drvAT.ToString() + dtDL.Rows[i]["I_FORMULALEVEL"].ToString();
                                                else
                                                    mon = drvAT.ToString();

                                                resT = "";
                                                try
                                                { resT = StrHelper.Cale(mon); }
                                                catch
                                                { resT = "0"; }

                                                //传出数据
                                                if (!ht.ContainsKey(paraIDT))
                                                    ht.Add(paraIDT, sh.ShowPoint(resT, num));
                                                else
                                                { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                                //公式数据
                                                if (!htUnT.ContainsKey(ParaID))
                                                {
                                                    if (calc == "yes")
                                                        htUnT.Add(ParaID, resT);
                                                    else
                                                        htUnT.Add(ParaID, sh.ShowPoint(resT, num));
                                                }
                                                else
                                                {
                                                    if (calc == "yes")
                                                        strVal = htUnT[ParaID].ToString() + ',' + resT;
                                                    else
                                                        strVal = htUnT[ParaID].ToString() + ',' + sh.ShowPoint(resT, num);

                                                    htUnT.Remove(ParaID);
                                                    htUnT.Add(ParaID, strVal);
                                                }
                                            }

                                            //最优值计算
                                            if (isZY == true)
                                            {
                                                if (!htZY.ContainsKey(ParaID))
                                                { htZY.Add(ParaID, dtDL.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + res); }
                                                else
                                                {
                                                    string zy = htZY[ParaID] + "," + res;
                                                    htZY.Remove(ParaID);
                                                    htZY.Add(ParaID, zy);
                                                }

                                                if (flag == true)
                                                {
                                                    if (!htZYT.ContainsKey(ParaID))
                                                    { htZYT.Add(ParaID, dtDL.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + resT); }
                                                    else
                                                    {
                                                        string zy = htZYT[ParaID] + "," + resT;
                                                        htZYT.Remove(ParaID);
                                                        htZYT.Add(ParaID, zy);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    #region 其它
                                    sqlWhere = " where 1=1 ";

                                    sql = "select * from T_sheet_sheetPara &where& and T_REPORTID='" + repName + "' and T_UNITID=" + unit + " and T_PARATYPE='其它' and T_TimeType='YY' and I_FORMULALEVEL=" + gsjb;

                                    if (repOrgid != "")
                                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                                    if (treeid != "")
                                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                                    if (sql.Contains("&where&"))
                                        sql = sql.Replace("&where&", sqlWhere);

                                    //sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                                    DataTable dtQT = null; 
                                    dtQT = dl.RunDataTable(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                                    //else if (rlDBType == "DB2")
                                    //    dtQT = DBdb2.RunDataTable(sql, out errMsg);
                                    //else
                                    //    dtQT = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                                    if (dtQT.Rows.Count > 0 && dtQT != null)
                                    {
                                        for (int i = 0; i < dtQT.Rows.Count; i++)
                                        {
                                        }
                                    }
                                    #endregion

                                    #region 公式
                                    sqlWhere = " where 1=1 ";

                                    sql = "select * from T_sheet_sheetPara &where& and T_REPORTID='" + repName + "' and T_UNITID=" + unit + " and T_PARATYPE='公式' and T_TimeType='YY' and I_FORMULALEVEL=" + gsjb;

                                    if (repOrgid != "")
                                        sqlWhere += " and t_orgid='" + repOrgid + "' ";

                                    if (treeid != "")
                                        sqlWhere += " and T_Treeid='" + treeid + "' ";

                                    if (sql.Contains("&where&"))
                                        sql = sql.Replace("&where&", sqlWhere);
                                                                     
                                    DataTable dtMon = null;
                                    dtMon = dl.RunDataTable(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                                    //else if (rlDBType == "DB2")
                                    //    dtMon = DBdb2.RunDataTable(sql, out errMsg);
                                    //else
                                    //    dtMon = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

                                    if (dtMon != null && dtMon.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dtMon.Rows.Count; i++)
                                        {
                                            num = dtMon.Rows[i]["I_DECIMALPOINT"].ToString();
                                            //参数ID
                                            ParaID = dtMon.Rows[i]["T_PARAID"].ToString();

                                            string pMon = dtMon.Rows[i]["T_FORMULA"].ToString();
                                            string paras = dtMon.Rows[i]["T_FORMULAPARA"].ToString();

                                            string month = pm.retMonByMonthAndYear(htGY, htUn, paras, pMon);
                                            string monthT = pm.retMonByMonthAndYear(htGYT, htUnT, paras, pMon);

                                            paraIDB = ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper() + "_" + dateCount;
                                            paraIDT = ParaID + "_" + dtMon.Rows[i]["T_TimeType"].ToString().ToUpper() + "_T_" + dateCount;

                                            //本期
                                            if (month != "")
                                            {
                                                res = "";
                                                try
                                                { res = StrHelper.Cale(month); }
                                                catch
                                                { res = "0"; }
                                            }

                                            //传出数据
                                            if (!ht.ContainsKey(paraIDB))
                                                ht.Add(paraIDB, sh.ShowPoint(res, num));
                                            else
                                            { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                            //公式数据
                                            if (!htUn.ContainsKey(ParaID))
                                            {
                                                if (calc == "yes")
                                                    htUn.Add(ParaID, res);
                                                else
                                                    htUn.Add(ParaID, sh.ShowPoint(res,num));
                                            }
                                            else
                                            {
                                                if (calc == "yes")
                                                    strVal = htUn[ParaID].ToString() + ',' + res;
                                                else
                                                    strVal = htUn[ParaID].ToString() + ',' + sh.ShowPoint(res, num);

                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, strVal);
                                            }

                                            //同期
                                            if (flag == true)
                                            {
                                                if (monthT != "")
                                                {
                                                    resT = "";
                                                    try
                                                    { resT = StrHelper.Cale(monthT); }
                                                    catch
                                                    { resT = "0"; }
                                                }
                                                //传出数据
                                                if (!ht.ContainsKey(paraIDT))
                                                    ht.Add(paraIDT, sh.ShowPoint(res, num));
                                                else
                                                { LogHelper.WriteLog(LogHelper.EnLogType.Error, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复"); }

                                                //公式数据
                                                if (!htUnT.ContainsKey(ParaID))
                                                {
                                                    if (calc == "yes")
                                                        htUnT.Add(ParaID, res);
                                                    else
                                                        htUnT.Add(ParaID, sh.ShowPoint(res, num));
                                                }
                                                else
                                                {
                                                    if (calc == "yes")
                                                        strVal = htUnT[ParaID].ToString() + ',' + res;
                                                    else
                                                        strVal = htUnT[ParaID].ToString() + ',' + sh.ShowPoint(res, num);

                                                    htUnT.Remove(ParaID);
                                                    htUnT.Add(ParaID, strVal);
                                                }
                                            }

                                            //最优值计算
                                            if (isZY == true)
                                            {
                                                if (!htZY.ContainsKey(ParaID))
                                                { htZY.Add(ParaID, dtMon.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + res); }
                                                else
                                                {
                                                    string zy = htZY[ParaID] + "," + res;
                                                    htZY.Remove(ParaID);
                                                    htZY.Add(ParaID, zy);
                                                }

                                                if (flag == true)
                                                {
                                                    if (!htZYT.ContainsKey(ParaID))
                                                    { htZYT.Add(ParaID, dtMon.Rows[i]["T_OPTIMALTYPE"].ToString() + "*" + resT); }
                                                    else
                                                    {
                                                        string zy = htZYT[ParaID] + "," + resT;
                                                        htZYT.Remove(ParaID);
                                                        htZYT.Add(ParaID, zy);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
            }

            #region 最优值
            if (isZY == true)
            {
                string strB = "";
                string strT = "";

                foreach (string skey in htZY)
                {
                    strB = this.GetMaxOrMixStr(htZY[skey].ToString());

                    if (strB != "")
                    {
                        if (!ht.ContainsKey(skey + "_ZY"))
                        { if(calc=="yes")
                            ht.Add(skey + "_ZY", strB);
                        else
                            ht.Add(skey + "_ZY", sh.ShowPoint(strB,num));

                        }
                    }
                }

                if (flag == true)
                {
                    foreach (string skey in htZYT)
                    {
                        strT = this.GetMaxOrMixStr(htZYT[skey].ToString());

                        if (strT != "")
                        {
                            if (!ht.ContainsKey(skey + "_ZY"))
                            {
                                if (calc == "yes")
                                    ht.Add(skey + "_ZY", strT);
                                else
                                    ht.Add(skey + "_ZY", sh.ShowPoint(strT, num));
                            }
                        }
                    }
                }
            }

            #endregion

            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----年明细----机组轮转结束!");
            #endregion

            //if (rtDBType == "HS")
            //    hs.DisConnect();

            #region 调试模式
            if (TSMS == "yes")
            {
                ArrayList listGy = new ArrayList(htGY.Keys);
                ArrayList listGyt = new ArrayList(htGYT.Keys);
                ArrayList listUn = new ArrayList(htUn.Keys);
                ArrayList listUnt = new ArrayList(htUnT.Keys);
                ArrayList listHt = new ArrayList(ht.Keys);

                listGy.Sort();
                listGyt.Sort();
                listUn.Sort();
                listUnt.Sort();
                listHt.Sort();

                foreach (string skey in listGy)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--YY--" + repOrgid + "--本期--" + skey + "---" + htGY[skey]); }

                foreach (string skey in listUn)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--YY--" + repOrgid + "--本期--" + skey + "---" + htUn[skey]); }

                foreach (string skey in listGyt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--YY--" + repOrgid + "--同期--" + skey + "---" + htGYT[skey]); }

                foreach (string skey in listUnt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--YY--" + repOrgid + "--同期--" + skey + "---" + htUnT[skey]); }

                foreach (string skey in listHt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--YY--" + repOrgid + "--报表数据输出--" + skey + "---" + ht[skey]); }
            }

            #endregion

            return ht;
        }

        /// <summary>
        /// 获取最优值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetMaxOrMixStr(string str)
        {
            string res = "";
            if (str.Contains("*"))
            {
                string[] strArr = str.Split('*');

                if (strArr.Length > 0)
                {
                    string[] strVal = strArr[1].ToString().TrimEnd(',').Split(',');

                    if (strVal.Length > 0)
                    {
                        string[] arr = this.arraySort(strVal); //sh.arraySort(arr);                                            

                        if (strArr[0].ToString() == "大")
                            return arr[0];
                        else if (strArr[0].ToString() == "小")
                            return arr[arr.Length - 1];
                        else
                            return "/";
                    }
                    else
                    { res = "/"; }

                }
                else { res = "/"; }
            }
            else { res = "/"; }

            return res;
        }

        /// <summary>
        /// 数组排序(冒泡)
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private string[] arraySort(string[] arr)
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
    }
}
