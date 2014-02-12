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
    /// 2012-06-13 更新:添加是否显示全厂数据判断
    /// 更改者:单云龙
    /// </summary>
    public class DALReport
    {
        int count;
        int countKey;
        string sql = "";
        string errMsg = "";
        string TSMS = "";       //调试模式
        string rtDBType = "";   //实时数据库
        string rlDBType = "";   //关系数据库
        StrHelper sh = new StrHelper();
       // PowerCale pc = new PowerCale();
        DALPubMehtod pm = new DALPubMehtod();
        DALUtilityMehtod um = new DALUtilityMehtod();
        LogHelper lh = new LogHelper();
        DBLink dl = new DBLink();
        /// <summary>
        /// 初始化函数
        /// 初始化报表配置
        /// 对全局变量赋值
        /// </summary>
        /// <returns></returns>
        private void init()
        {
            TSMS = IniHelper.ReadIniData("Report", "TSMS", null);
            rtDBType = IniHelper.ReadIniData("RTDB", "DBType", null);
            rlDBType = IniHelper.ReadIniData("RelationDBbase", "DBType", null);
        }

        /// <summary>
        /// 值报
        /// </summary>
        /// <param name="repName"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public ArrayList RetArrsRepVal(string repName, string date, string bName, out string bzID, out string bzTime)
        {
            this.init();

            int id;
            object obj;
            object objVal;

            int ret = 0;
            double drvA = 0;

            string subStr = "";
            string subExc = "";
            string re = "";
            string qsrq = "";
            string jsrq = "";
            string spName = "";   //存储过程名称
            string valGY = "";
            string unit = "";   //机组
            string bm = "";     //电量表码
            string mon = "";    //计算公式
            string res = "";    //计算结果
            string num = "";    //小数点数           
            string dlID = "";   //电量ID
            string sDate = "";  //结束时间(实时值,SQL结束时间)
            string gsjb = "";   //公式级别
            string GYID = "";   //公用库参数ID
            string ParaID = ""; //参数ID

            string repType = "值报";

            bzID = "";
            bzTime = "";

            //基础数据值
            string[] zb = null;
            string[] un = null;
            string[] lay = null;
            string[] dv = null;
            string[] dvExcel = null;

            DateTime dtNow = DateTime.Now;

            DataTable dtGY = null;
            DataTable dtLel = null;     //机组级别
            DataTable dtLevZ = null;    //全厂级别
            DataTable dtLevUnit = null;
            Hashtable htGY = new Hashtable();
            Hashtable htUn = new Hashtable();
            ArrayList list = new ArrayList();

            count = pm.GetCountZ(repName);

            zb = new string[count + 1];
            un = new string[count + 1];
            lay = new string[count + 1];
            dv = new string[count + 1];
            dvExcel = new string[count + 1];

            #region 公用库读取

            sql = "select * from GYZReport where 报表名称='全厂级'";

            //if (rlDBType == "SQL")
            //    dtGY = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtGY = DBdb2.RunDataTable(sql, out errMsg);

            if (dtGY != null && dtGY.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转开始!");

                for (int i = 0; i < dtGY.Rows.Count; i++)
                {
                    bzID = "";
                    valGY = "";
                    bzTime = "";

                    GYID = dtGY.Rows[i]["参数ID"].ToString();

                    sql = "select * from 排班表 where 起始时间 between '" + Convert.ToDateTime(date).ToString("yyyy-MM-dd 0:00:00") + "' and '" + Convert.ToDateTime(date).ToString("yyyy-MM-dd 23:59:59") + "'";

                    DataTable dtLevBZ = null;
                    dtLevBZ = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtLevBZ = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtLevBZ = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtLevBZ != null && dtLevBZ.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtLevBZ.Rows.Count; k++)
                        {
                            qsrq = dtLevBZ.Rows[k]["起始时间"].ToString();

                            jsrq = DateTime.Parse(dtLevBZ.Rows[k]["结束时间"].ToString()).AddSeconds(+1).ToString("yyyy-MM-dd H:00:00");

                            if (dtNow > DateTime.Parse(jsrq))
                            {
                                if (dtGY.Rows[i]["参数类型"].ToString() == "平均" || dtGY.Rows[i]["参数类型"].ToString() == "累计")
                                {
                                    if (dtLevBZ.Rows[k]["班组编号"].ToString() != "" && dtGY.Rows[i]["统计ID"].ToString() != "")
                                        sql = "select 统计值 from " + bName + " where 时间='" + date + "' and 班组=" + dtLevBZ.Rows[k][
                                            "班组编号"].ToString() + " and 统计ID ='" + dtGY.Rows[i]["统计ID"].ToString() + "'";//and 机组=" + unit + " 
                                    else
                                        LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 日值 平均 报程序异常: 班组编号 || 统计ID 为空" + sql);
                                    objVal = dl.RunSingle(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    objVal = DBsql.RunSingle(sql, out errMsg);
                                    //else
                                    //    objVal = DBdb2.RunSingle(sql, out errMsg);

                                    if (objVal != null && objVal.ToString() != "")
                                    {
                                        valGY += dtLevBZ.Rows[k]["班组编号"].ToString().TrimEnd(' ') + "+" + objVal + ",";

                                        if (GYID != "")
                                        {
                                            if (htGY.Contains(GYID))
                                            {
                                                htGY.Remove(GYID);
                                                htGY.Add(GYID, valGY);
                                            }
                                            else
                                                htGY.Add(GYID, valGY);
                                        }
                                    }

                                }
                                else if (dtGY.Rows[i]["参数类型"].ToString() == "电量")
                                {
                                    dlID = dtGY.Rows[i]["电量点"].ToString();

                                    //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                                    mon = "";

                                    if (dtGY.Rows[i]["公式"].ToString() != "")
                                        mon = drvA.ToString() + dtGY.Rows[i]["公式"].ToString();
                                    else
                                        mon = drvA.ToString();

                                    res = "";
                                    try
                                    { res = StrHelper.Cale(mon); }
                                    catch
                                    { res = "0"; }

                                    if (GYID != "")
                                    {
                                        valGY += dtLevBZ.Rows[k]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";

                                        if (htGY.Contains(GYID))
                                        {
                                            htGY.Remove(GYID);
                                            htGY.Add(GYID, valGY);
                                        }
                                        else
                                            htGY.Add(GYID, valGY);
                                    }
                                }
                                else if (dtGY.Rows[i]["参数类型"].ToString() == "SQL")
                                {
                                    sql = dtGY.Rows[i]["SQL语句"].ToString();

                                    if (sql.Contains("&BZ&"))
                                    {
                                        sql = sql.Replace("&BZ&", dtLevBZ.Rows[k]["班组编号"].ToString());
                                        sql = sql.Replace("&bt&", DateTime.Parse(qsrq).ToString("yyyy-MM-dd 0:00:00"));
                                        sql = sql.Replace("&et&", DateTime.Parse(jsrq).AddSeconds(-1).ToString());
                                    }
                                    else if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                    {
                                        sql = sql.Replace("&bt&", qsrq);
                                        sql = sql.Replace("&et&", DateTime.Parse(jsrq).AddSeconds(-1).ToString());
                                    }
                                    obj = dl.RunSingle(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    obj = DBsql.RunSingle(sql, out errMsg);
                                    //else
                                    //    obj = DBdb2.RunSingle(sql, out errMsg);

                                    res = "";
                                    if (obj != null && obj.ToString() != "")
                                    {
                                        if (obj.ToString() != "")
                                            res = obj.ToString();
                                        else
                                            res = "0";
                                    }
                                    else
                                    { res = "0"; }

                                    if (GYID != "")
                                    {
                                        valGY += dtLevBZ.Rows[k]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";

                                        if (htGY.Contains(GYID))
                                        {
                                            htGY.Remove(GYID);
                                            htGY.Add(GYID, valGY);
                                        }
                                        else
                                            htGY.Add(GYID, valGY);
                                    }
                                }
                                else if (dtGY.Rows[i]["参数类型"].ToString() == "存储过程")
                                {
                                    spName = dtGY.Rows[i]["SQL语句"].ToString();
                                    obj = dl.RunSingle_SP(spName, dtGY.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevBZ.Rows[k]["班组编号"].ToString().TrimEnd(' '), out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    obj = DBsql.RunSingle_SP(spName, dtGY.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevBZ.Rows[k]["班组编号"].ToString().TrimEnd(' '), out errMsg);
                                    //else
                                    //    obj = DBdb2.RunSingle_SP(spName, dtGY.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevBZ.Rows[k]["班组编号"].ToString().TrimEnd(' '), out errMsg);

                                    if (obj != null && obj.ToString() != "")
                                    {
                                        if (obj.ToString() != "")
                                            res = obj.ToString();
                                        else
                                            res = "0";
                                    }
                                    else
                                    { res = "0"; }

                                    if (GYID != "")
                                    {
                                        valGY += dtLevBZ.Rows[k]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";

                                        if (htGY.Contains(GYID))
                                        {
                                            htGY.Remove(GYID);
                                            htGY.Add(GYID, valGY);
                                        }
                                        else
                                            htGY.Add(GYID, valGY);
                                    }
                                }
                            }
                        }
                    }

                }
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转结束!");
            }
            #endregion

            #region 机组数据读取

            sql = "SELECT DISTINCT 机组 FROM T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组!=0";
            dtLevUnit = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevUnit = DBsql.RunDataTable(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    dtLevUnit = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLevUnit != null && dtLevUnit.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转开始!");

                for (int k = 0; k < dtLevUnit.Rows.Count; k++)
                {
                    bzID = "";
                    bzTime = "";

                    unit = dtLevUnit.Rows[k]["机组"].ToString();

                    sql = "select * from 排班表 where 起始时间 between '" + Convert.ToDateTime(date).ToString("yyyy-MM-dd 0:00:00") + "' and '" + Convert.ToDateTime(date).ToString("yyyy-MM-dd 23:59:59") + "'";

                    DataTable dtLevBZ = null;
                    dtLevBZ = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtLevBZ = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtLevBZ = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtLevBZ != null && dtLevBZ.Rows.Count > 0)
                    {
                        int a = 1;
                        for (int l = 0; l < dtLevBZ.Rows.Count; l++)
                        {
                            bzID += dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + ";";

                            qsrq = dtLevBZ.Rows[l]["起始时间"].ToString();

                            jsrq = DateTime.Parse(dtLevBZ.Rows[l]["结束时间"].ToString()).AddSeconds(+1).ToString("yyyy-MM-dd H:00:00");

                            bzTime += qsrq + "," + jsrq + ";";

                            if (dtNow > DateTime.Parse(jsrq))
                            {
                                sql = "SELECT DISTINCT 公式级别 FROM T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组!=0 and 公式级别!=0";
                                dtLel = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtLel = DBsql.RunDataTable(sql, out errMsg);
                                //else if (rlDBType == "DB2")
                                //    dtLel = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtLel.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtLel.Rows.Count; j++)
                                    {
                                        //公式级别
                                        gsjb = dtLel.Rows[j]["公式级别"].ToString();

                                        #region 平均
                                        sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='平均' and 公式级别=" + gsjb;

                                        DataTable dtAvg = null;
                                        dtAvg = dl.RunDataTable(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                                        //else
                                        //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                                        if (dtAvg != null && dtAvg.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtAvg.Rows.Count; i++)
                                            {
                                                //小数点数
                                                num = dtAvg.Rows[i]["小数点数"].ToString();
                                                //参数顺序
                                                id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());
                                                //参数ID
                                                ParaID = dtAvg.Rows[i]["参数ID"].ToString();

                                                if (dtLevBZ.Rows[l]["班组编号"].ToString() != "" && dtAvg.Rows[i]["统计ID"].ToString() != "")
                                                    sql = "select 统计值 from " + bName + " where 时间='" + date + "' and 机组=" + unit + " and 班组=" + dtLevBZ.Rows[l][
                                                        "班组编号"].ToString() + " and 统计ID ='" + dtAvg.Rows[i]["统计ID"].ToString() + "'";
                                                else
                                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 日值 平均 报程序异常: 班组编号 || 统计ID 为空" + sql);
                                                objVal = dl.RunSingle(sql, out errMsg);
                                                //if (rlDBType == "SQL")
                                                //    objVal = DBsql.RunSingle(sql, out errMsg);
                                                //else
                                                //    objVal = DBdb2.RunSingle(sql, out errMsg);

                                                res = "";
                                                if (objVal != null && objVal != "")
                                                {
                                                    if (objVal.ToString() != "")
                                                        res = objVal.ToString();
                                                    else
                                                        res = "0";
                                                }
                                                else
                                                { res = "0"; }

                                                if (id != 0)
                                                {
                                                    zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                                    un[id] = dtAvg.Rows[i]["单位"].ToString();
                                                    lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                                    if (a < dtLevBZ.Rows.Count)
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                    }
                                                    else
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                                    }

                                                    dv[id] += subStr;
                                                    dvExcel[id] += subExc;
                                                }

                                                if (ParaID != "")
                                                {
                                                    if (htUn.ContainsKey(ParaID))
                                                    {
                                                        re = htUn[ParaID] + dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, re);
                                                    }
                                                    else
                                                        htUn.Add(ParaID, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                                }
                                            }
                                        }
                                        #endregion

                                        #region 累计
                                        sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='累计' and 公式级别=" + gsjb;

                                        DataTable dtDiff = null;
                                        dtDiff = dl.RunDataTable(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                                        //else
                                        //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                                        if (dtDiff != null && dtDiff.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtDiff.Rows.Count; i++)
                                            {
                                                //小数点数
                                                num = dtDiff.Rows[i]["小数点数"].ToString();
                                                //参数顺序
                                                id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());
                                                //参数ID
                                                ParaID = dtDiff.Rows[i]["参数ID"].ToString();

                                                if (dtLevBZ.Rows[l]["班组编号"].ToString() != "" && dtDiff.Rows[i]["统计ID"].ToString() != "")
                                                    sql = "select 统计值 from " + bName + " where 时间='" + date + "' and 机组=" + unit + " and 班组=" + dtLevBZ.Rows[l][
                                                        "班组编号"].ToString() + " and 统计ID ='" + dtDiff.Rows[i]["统计ID"].ToString() + "'";
                                                else
                                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 日值 平均 报程序异常: 班组编号 || 统计ID 为空" + sql);
                                                objVal = dl.RunSingle(sql, out errMsg);
                                                //if (rlDBType == "SQL")
                                                //    objVal = DBsql.RunSingle(sql, out errMsg);
                                                //else
                                                //    objVal = DBdb2.RunSingle(sql, out errMsg);

                                                res = "";
                                                if (objVal != null && objVal != "")
                                                {
                                                    if (objVal.ToString() != "")
                                                        res = objVal.ToString();
                                                    else
                                                        res = "0";
                                                }
                                                else
                                                { res = "0"; }

                                                if (id != 0)
                                                {
                                                    zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                                    un[id] = dtDiff.Rows[i]["单位"].ToString();
                                                    lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                                    if (a < dtLevBZ.Rows.Count)
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                    }
                                                    else
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                                    }

                                                    dv[id] += subStr;
                                                    dvExcel[id] += subExc;
                                                }

                                                if (ParaID != "")
                                                {
                                                    if (htUn.ContainsKey(ParaID))
                                                    {
                                                        re = htUn[ParaID] + dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, re);
                                                    }
                                                    else
                                                        htUn.Add(ParaID, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                                }
                                            }
                                        }
                                        #endregion

                                        #region 电量
                                        sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='电量' and 公式级别=" + gsjb;

                                        DataTable dtDL = null;
                                        dtDL = dl.RunDataTable(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                                        //else
                                        //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                                        if (dtDL.Rows.Count > 0 && dtDL != null)
                                        {
                                            for (int i = 0; i < dtDL.Rows.Count; i++)
                                            {
                                                dlID = dtDL.Rows[i]["电量点"].ToString();
                                                //小数点数
                                                num = dtDL.Rows[i]["小数点数"].ToString();
                                                //参数顺序
                                                id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());
                                                //参数ID
                                                ParaID = dtDL.Rows[i]["参数ID"].ToString();

                                               // drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                                                mon = "";

                                                if (dtDL.Rows[i]["公式"].ToString() != "")
                                                    mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                                                else
                                                    mon = drvA.ToString();

                                                res = "";
                                                try
                                                { res = StrHelper.Cale(mon); }
                                                catch
                                                { res = "0"; }

                                                if (id != 0)
                                                {
                                                    zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                                    un[id] = dtDL.Rows[i]["单位"].ToString();
                                                    lay[id] = dtDL.Rows[i]["设计值"].ToString();

                                                    if (a < dtLevBZ.Rows.Count)
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                    }
                                                    else
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                                    }

                                                    dv[id] += subStr;
                                                    dvExcel[id] += subExc;
                                                }

                                                if (ParaID != "")
                                                {
                                                    if (htUn.ContainsKey(ParaID))
                                                    {
                                                        re = htUn[ParaID] + dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, re);
                                                    }
                                                    else
                                                        htUn.Add(ParaID, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                                }
                                            }
                                        }

                                        #endregion

                                        #region SQL

                                        sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='SQL' and 公式级别=" + gsjb;

                                        DataTable dtSQL = null;
                                        dtSQL = dl.RunDataTable(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                                        //else
                                        //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                                        if (dtSQL != null && dtSQL.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtSQL.Rows.Count; i++)
                                            {
                                                //小数点数
                                                num = dtSQL.Rows[i]["小数点数"].ToString();
                                                //参数顺序
                                                id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());
                                                //参数ID
                                                ParaID = dtSQL.Rows[i]["参数ID"].ToString();

                                                sql = dtSQL.Rows[i]["SQL语句"].ToString();

                                                if (sql.Contains("&BZ&"))
                                                {
                                                    sql = sql.Replace("&BZ&", dtLevBZ.Rows[l]["班组编号"].ToString());
                                                    sql = sql.Replace("&bt&", DateTime.Parse(qsrq).ToString("yyyy-MM-dd 0:00:00"));
                                                    sql = sql.Replace("&et&", DateTime.Parse(jsrq).AddSeconds(-1).ToString());
                                                }
                                                else if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                                {
                                                    sql = sql.Replace("&bt&", qsrq);
                                                    sql = sql.Replace("&et&", DateTime.Parse(jsrq).AddSeconds(-1).ToString());
                                                }

                                                if (rlDBType == "SQL")
                                                    obj = DBsql.RunSingle(sql, out errMsg);
                                                else
                                                    obj = DBdb2.RunSingle(sql, out errMsg);

                                                res = "";
                                                if (obj != null && obj.ToString() != "")
                                                {
                                                    if (obj.ToString() != "")
                                                        res = obj.ToString();
                                                    else
                                                        res = "0";
                                                }
                                                else
                                                { res = "0"; }

                                                if (id != 0)
                                                {
                                                    zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                                    un[id] = dtSQL.Rows[i]["单位"].ToString();
                                                    lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                                    if (a < dtLevBZ.Rows.Count)
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                    }
                                                    else
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                                    }

                                                    dv[id] += subStr;
                                                    dvExcel[id] += subExc;
                                                }

                                                if (ParaID != "")
                                                {
                                                    if (htUn.ContainsKey(ParaID))
                                                    {
                                                        re = htUn[ParaID] + dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, re);
                                                    }
                                                    else
                                                        htUn.Add(ParaID, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                                }

                                            }
                                        }
                                        #endregion

                                        #region 存储过程

                                        sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='存储过程' and 公式级别=" + gsjb;

                                        DataTable dtSP = null;

                                        if (rlDBType == "SQL")
                                            dtSP = DBsql.RunDataTable(sql, out errMsg);
                                        else
                                            dtSP = DBdb2.RunDataTable(sql, out errMsg);

                                        if (dtSP != null && dtSP.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtSP.Rows.Count; i++)
                                            {
                                                //小数点数
                                                num = dtSP.Rows[i]["小数点数"].ToString();
                                                //参数顺序
                                                id = int.Parse(dtSP.Rows[i]["参数顺序"].ToString());
                                                //参数ID
                                                ParaID = dtSP.Rows[i]["参数ID"].ToString();

                                                spName = dtSP.Rows[i]["SQL语句"].ToString();
                                                obj = dl.RunSingle_SP(spName, dtSP.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), out errMsg);
                                                //if (rlDBType == "SQL")
                                                //    obj = DBsql.RunSingle_SP(spName, dtSP.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), out errMsg);
                                                //else
                                                //    obj = DBdb2.RunSingle_SP(spName, dtSP.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), out errMsg);

                                                if (obj != null && obj.ToString() != "")
                                                {
                                                    if (obj.ToString() != "")
                                                        res = obj.ToString();
                                                    else
                                                        res = "0";
                                                }
                                                else
                                                { res = "0"; }

                                                if (id != 0)
                                                {
                                                    zb[id] = dtSP.Rows[i]["参数名"].ToString();
                                                    un[id] = dtSP.Rows[i]["单位"].ToString();
                                                    lay[id] = dtSP.Rows[i]["设计值"].ToString();

                                                    if (a < dtLevBZ.Rows.Count)
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                    }
                                                    else
                                                    {
                                                        subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                        subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                                    }

                                                    dv[id] += subStr;
                                                    dvExcel[id] += subExc;
                                                }

                                                if (ParaID != "")
                                                {
                                                    if (htUn.ContainsKey(ParaID))
                                                    {
                                                        re = htUn[ParaID] + dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                        htUn.Remove(ParaID);
                                                        htUn.Add(ParaID, re);
                                                    }
                                                    else
                                                        htUn.Add(ParaID, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                                }

                                            }
                                        }
                                        #endregion

                                        #region 其它
                                        sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                                        DataTable dtQT = null;
                                        dtQT = dl.RunDataTable(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                                        //else
                                        //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                                        if (dtQT.Rows.Count > 0 && dtQT != null)
                                        {
                                            for (int i = 0; i < dtQT.Rows.Count; i++)
                                            {
                                                //小数点数
                                                num = dtQT.Rows[i]["小数点数"].ToString();
                                                //参数顺序
                                                id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());
                                                //参数ID
                                                ParaID = dtQT.Rows[i]["参数ID"].ToString();

                                                #region 购网电量
                                                if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                                                {
                                                    //string re = um.gwdl(qsrq, jsrq, unit);

                                                    //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                                    //{
                                                    //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                                    //    dv[id] += sh.ShowPoint(re.ToString(), num) + ",";

                                                    //    dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                                    //}

                                                    //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                                    //{
                                                    //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                                    //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + re + ";";
                                                    //}
                                                }
                                                #endregion

                                                #region 入炉煤低位发热量
                                                if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                                                {
                                                    //string rlm = um.rlmdwfrl("值报", DateTime.Parse(qsrq).ToString("yyyy-MM-dd 0:00:00"), jsrq, unit, dtLevBZ.Rows[k]["班组编号"].ToString());

                                                    //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                                    //{
                                                    //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                                    //    dv[id] += sh.ShowPoint(rlm, num) + ",";

                                                    //    dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                                    //}

                                                    //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                                    //{
                                                    //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                                    //    //du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlm, num) + ";";

                                                    //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + rlm + ";";
                                                    //}
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                        #region 公式
                                        sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='公式' and 公式级别=" + gsjb;

                                        DataTable dtMon = null;
                                        dtMon = dl.RunDataTable(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                                        //else
                                        //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                                        if (dtMon != null && dtMon.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtMon.Rows.Count; i++)
                                            {
                                                //小数点数
                                                num = dtMon.Rows[i]["小数点数"].ToString();
                                                //参数顺序
                                                id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());
                                                //参数ID
                                                ParaID = dtMon.Rows[i]["参数ID"].ToString();

                                                string pMon = dtMon.Rows[i]["公式"].ToString();

                                                string paras = dtMon.Rows[i]["公式参数"].ToString();

                                                string month = pm.retMonByZRep(dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), htGY, htUn, paras, pMon);

                                                if (month != "")
                                                {
                                                    res = "";
                                                    try
                                                    { res = StrHelper.Cale(month); }
                                                    catch
                                                    {
                                                        res = "0";
                                                        LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 日值 公式 报程序异常: " + pMon + ":" + paras + ":" + month);
                                                    }

                                                    if (id != 0)
                                                    {
                                                        zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                                        un[id] = dtMon.Rows[i]["单位"].ToString();
                                                        lay[id] = dtMon.Rows[i]["设计值"].ToString();

                                                        if (a < dtLevBZ.Rows.Count)
                                                        {
                                                            subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                            subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                        }
                                                        else
                                                        {
                                                            subStr = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                            subExc = dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                                        }

                                                        dv[id] += subStr;
                                                        dvExcel[id] += subExc;
                                                    }

                                                    if (ParaID != "")
                                                    {
                                                        if (htUn.ContainsKey(ParaID))
                                                        {
                                                            re = htUn[ParaID] + dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                            htUn.Remove(ParaID);
                                                            htUn.Add(ParaID, re);
                                                        }
                                                        else
                                                            htUn.Add(ParaID, dtLevBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                                    }
                                                }
                                            }
                                        }

                                        #endregion
                                    }
                                }
                            }
                            a++;
                        }
                    }
                }
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转结束!");
            }
            #endregion

            #region 全厂数据读取
            sql = "select * from 排班表 where 起始时间 between '" + Convert.ToDateTime(date).ToString("yyyy-MM-dd 0:00:00") + "' and '" + Convert.ToDateTime(date).ToString("yyyy-MM-dd 23:59:59") + "'";

            DataTable dtLevALLBZ = null;
            dtLevALLBZ = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevALLBZ = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtLevALLBZ = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLevALLBZ != null && dtLevALLBZ.Rows.Count > 0)
            {
                int a = 1;
                for (int l = 0; l < dtLevALLBZ.Rows.Count; l++)
                {
                    bzID += dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + ";";

                    qsrq = dtLevALLBZ.Rows[l]["起始时间"].ToString();

                    jsrq = DateTime.Parse(dtLevALLBZ.Rows[l]["结束时间"].ToString()).AddSeconds(+1).ToString("yyyy-MM-dd H:00:00");

                    bzTime += qsrq + "," + jsrq + ";";

                    if (dtNow > DateTime.Parse(jsrq))
                    {
                        sql = "SELECT DISTINCT 公式级别 FROM T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 公式级别!=0";

                        if (rlDBType == "SQL")
                            dtLel = DBsql.RunDataTable(sql, out errMsg);
                        else if (rlDBType == "DB2")
                            dtLel = DBdb2.RunDataTable(sql, out errMsg);

                        if (dtLel.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtLel.Rows.Count; j++)
                            {
                                //公式级别
                                gsjb = dtLel.Rows[j]["公式级别"].ToString();

                                #region 平均
                                sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='平均' and 公式级别=" + gsjb;

                                DataTable dtAvg = null;
                                dtAvg = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                                //else
                                //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtAvg != null && dtAvg.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtAvg.Rows.Count; i++)
                                    {
                                        //小数点数
                                        num = dtAvg.Rows[i]["小数点数"].ToString();
                                        //参数顺序
                                        id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());
                                        //参数ID
                                        ParaID = dtAvg.Rows[i]["参数ID"].ToString();

                                        if (dtLevALLBZ.Rows[l]["班组编号"].ToString() != "" && dtAvg.Rows[i]["统计ID"].ToString() != "")
                                            sql = "select 统计值 from " + bName + " where 时间='" + date + "' and 机组=0 and 班组=" + dtLevALLBZ.Rows[l][
                                                "班组编号"].ToString() + " and 统计ID ='" + dtAvg.Rows[i]["统计ID"].ToString() + "'";
                                        else
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 日值 平均 报程序异常: 班组编号 || 统计ID 为空" + sql);

                                        if (rlDBType == "SQL")
                                            objVal = DBsql.RunSingle(sql, out errMsg);
                                        else
                                            objVal = DBdb2.RunSingle(sql, out errMsg);

                                        res = "";
                                        if (objVal != null && objVal != "")
                                        {
                                            if (objVal.ToString() != "")
                                                res = objVal.ToString();
                                            else
                                                res = "0";
                                        }
                                        else
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                            un[id] = dtAvg.Rows[i]["单位"].ToString();
                                            lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                            if (a < dtLevALLBZ.Rows.Count)
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                            }
                                            else
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                            }

                                            dv[id] += subStr;
                                            dvExcel[id] += subExc;
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                            {
                                                re = htUn[ParaID] + dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, re);
                                            }
                                            else
                                                htUn.Add(ParaID, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                        }
                                    }
                                }
                                #endregion

                                #region 累计
                                sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='累计' and 公式级别=" + gsjb;

                                DataTable dtDiff = null;
                                dtDiff = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                                //else
                                //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtDiff != null && dtDiff.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtDiff.Rows.Count; i++)
                                    {
                                        //小数点数
                                        num = dtDiff.Rows[i]["小数点数"].ToString();
                                        //参数顺序
                                        id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());
                                        //参数ID
                                        ParaID = dtDiff.Rows[i]["参数ID"].ToString();

                                        if (dtLevALLBZ.Rows[l]["班组编号"].ToString() != "" && dtDiff.Rows[i]["统计ID"].ToString() != "")
                                            sql = "select 统计值 from " + bName + " where 时间='" + date + "' and 机组=0 and 班组=" + dtLevALLBZ.Rows[l][
                                                "班组编号"].ToString() + " and 统计ID ='" + dtDiff.Rows[i]["统计ID"].ToString() + "'";
                                        else
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 日值 平均 报程序异常: 班组编号 || 统计ID 为空" + sql);

                                        if (rlDBType == "SQL")
                                            objVal = DBsql.RunSingle(sql, out errMsg);
                                        else
                                            objVal = DBdb2.RunSingle(sql, out errMsg);

                                        res = "";
                                        if (objVal != null && objVal != "")
                                        {
                                            if (objVal.ToString() != "")
                                                res = objVal.ToString();
                                            else
                                                res = "0";
                                        }
                                        else
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                            un[id] = dtDiff.Rows[i]["单位"].ToString();
                                            lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                            if (a < dtLevALLBZ.Rows.Count)
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                            }
                                            else
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                            }

                                            dv[id] += subStr;
                                            dvExcel[id] += subExc;
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                            {
                                                re = htUn[ParaID] + dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, re);
                                            }
                                            else
                                                htUn.Add(ParaID, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                        }
                                    }
                                }
                                #endregion

                                #region 电量
                                sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='电量' and 公式级别=" + gsjb;

                                DataTable dtDL = null;
                                dtDL = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                                //else
                                //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtDL.Rows.Count > 0 && dtDL != null)
                                {
                                    for (int i = 0; i < dtDL.Rows.Count; i++)
                                    {
                                        dlID = dtDL.Rows[i]["电量点"].ToString();
                                        //小数点数
                                        num = dtDL.Rows[i]["小数点数"].ToString();
                                        //参数顺序
                                        id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());
                                        //参数ID
                                        ParaID = dtDL.Rows[i]["参数ID"].ToString();

                                        //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                                        mon = "";

                                        if (dtDL.Rows[i]["公式"].ToString() != "")
                                            mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                                        else
                                            mon = drvA.ToString();

                                        res = "";
                                        try
                                        { res = StrHelper.Cale(mon); }
                                        catch
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                            un[id] = dtDL.Rows[i]["单位"].ToString();
                                            lay[id] = dtDL.Rows[i]["设计值"].ToString();

                                            if (a < dtLevALLBZ.Rows.Count)
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                            }
                                            else
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                            }

                                            dv[id] += subStr;
                                            dvExcel[id] += subExc;
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                            {
                                                re = htUn[ParaID] + dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, re);
                                            }
                                            else
                                                htUn.Add(ParaID, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                        }
                                    }
                                }

                                #endregion

                                #region SQL

                                sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='SQL' and 公式级别=" + gsjb;

                                DataTable dtSQL = null;
                                dtSQL = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                                //else
                                //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtSQL != null && dtSQL.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtSQL.Rows.Count; i++)
                                    {
                                        //小数点数
                                        num = dtSQL.Rows[i]["小数点数"].ToString();
                                        //参数顺序
                                        id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());
                                        //参数ID
                                        ParaID = dtSQL.Rows[i]["参数ID"].ToString();

                                        sql = dtSQL.Rows[i]["SQL语句"].ToString();

                                        if (sql.Contains("&BZ&"))
                                        {
                                            sql = sql.Replace("&BZ&", dtLevALLBZ.Rows[l]["班组编号"].ToString());
                                            sql = sql.Replace("&bt&", DateTime.Parse(qsrq).ToString("yyyy-MM-dd 0:00:00"));
                                            sql = sql.Replace("&et&", DateTime.Parse(jsrq).AddSeconds(-1).ToString());
                                        }
                                        else if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                        {
                                            sql = sql.Replace("&bt&", qsrq);
                                            sql = sql.Replace("&et&", DateTime.Parse(jsrq).AddSeconds(-1).ToString());
                                        }
                                        obj = dl.RunSingle(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    obj = DBsql.RunSingle(sql, out errMsg);
                                        //else
                                        //    obj = DBdb2.RunSingle(sql, out errMsg);

                                        res = "";
                                        if (obj != null && obj.ToString() != "")
                                        {
                                            if (obj.ToString() != "")
                                                res = obj.ToString();
                                            else
                                                res = "0";
                                        }
                                        else
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                            un[id] = dtSQL.Rows[i]["单位"].ToString();
                                            lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                            if (a < dtLevALLBZ.Rows.Count)
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                            }
                                            else
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                            }

                                            dv[id] += subStr;
                                            dvExcel[id] += subExc;
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                            {
                                                re = htUn[ParaID] + dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, re);
                                            }
                                            else
                                                htUn.Add(ParaID, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                        }

                                    }
                                }
                                #endregion

                                #region 存储过程

                                sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='存储过程' and 公式级别=" + gsjb;

                                DataTable dtSP = null;
                                dtSP = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtSP = DBsql.RunDataTable(sql, out errMsg);
                                //else
                                //    dtSP = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtSP != null && dtSP.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtSP.Rows.Count; i++)
                                    {
                                        //小数点数
                                        num = dtSP.Rows[i]["小数点数"].ToString();
                                        //参数顺序
                                        id = int.Parse(dtSP.Rows[i]["参数顺序"].ToString());
                                        //参数ID
                                        ParaID = dtSP.Rows[i]["参数ID"].ToString();

                                        spName = dtSP.Rows[i]["SQL语句"].ToString();
                                        obj = dl.RunSingle_SP(spName, dtSP.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    obj = DBsql.RunSingle_SP(spName, dtSP.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), out errMsg);
                                        //else
                                        //    obj = DBdb2.RunSingle_SP(spName, dtSP.Rows[i]["ID_KEY"].ToString(), qsrq, jsrq, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), out errMsg);

                                        if (obj != null && obj.ToString() != "")
                                        {
                                            if (obj.ToString() != "")
                                                res = obj.ToString();
                                            else
                                                res = "0";
                                        }
                                        else
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtSP.Rows[i]["参数名"].ToString();
                                            un[id] = dtSP.Rows[i]["单位"].ToString();
                                            lay[id] = dtSP.Rows[i]["设计值"].ToString();

                                            if (a < dtLevALLBZ.Rows.Count)
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                            }
                                            else
                                            {
                                                subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                            }

                                            dv[id] += subStr;
                                            dvExcel[id] += subExc;
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                            {
                                                re = htUn[ParaID] + dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, re);
                                            }
                                            else
                                                htUn.Add(ParaID, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                        }

                                    }
                                }
                                #endregion

                                #region 其它
                                sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='其它' and 公式级别=" + gsjb;

                                DataTable dtQT = null;
                                dtQT = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                                //else
                                //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtQT.Rows.Count > 0 && dtQT != null)
                                {
                                    for (int i = 0; i < dtQT.Rows.Count; i++)
                                    {
                                        //小数点数
                                        num = dtQT.Rows[i]["小数点数"].ToString();
                                        //参数顺序
                                        id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());
                                        //参数ID
                                        ParaID = dtQT.Rows[i]["参数ID"].ToString();

                                        #region 购网电量
                                        if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                                        {
                                            //string re = um.gwdl(qsrq, jsrq, unit);

                                            //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                            //{
                                            //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                            //    dv[id] += sh.ShowPoint(re.ToString(), num) + ",";

                                            //    dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                            //}

                                            //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                            //{
                                            //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                            //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + re + ";";
                                            //}
                                        }
                                        #endregion

                                        #region 入炉煤低位发热量
                                        if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                                        {
                                            //string rlm = um.rlmdwfrl("值报", DateTime.Parse(qsrq).ToString("yyyy-MM-dd 0:00:00"), jsrq, unit, dtLevALLBZ.Rows[k]["班组编号"].ToString());

                                            //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                            //{
                                            //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                            //    dv[id] += sh.ShowPoint(rlm, num) + ",";

                                            //    dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                            //}

                                            //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                            //{
                                            //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                            //    //du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlm, num) + ";";

                                            //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + rlm + ";";
                                            //}
                                        }
                                        #endregion
                                    }
                                }
                                #endregion

                                #region 公式
                                sql = "select * from T_sheet_ZsheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='公式' and 公式级别=" + gsjb;

                                DataTable dtMon = null;
                                dtMon = dl.RunDataTable(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                                //else
                                //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                                if (dtMon != null && dtMon.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtMon.Rows.Count; i++)
                                    {
                                        //小数点数
                                        num = dtMon.Rows[i]["小数点数"].ToString();
                                        //参数顺序
                                        id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());
                                        //参数ID
                                        ParaID = dtMon.Rows[i]["参数ID"].ToString();

                                        string pMon = dtMon.Rows[i]["公式"].ToString();

                                        string paras = dtMon.Rows[i]["公式参数"].ToString();

                                        string month = pm.retMonByZRep(dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' '), htGY, htUn, paras, pMon);

                                        if (month != "")
                                        {
                                            res = "";
                                            try
                                            { res = StrHelper.Cale(month); }
                                            catch
                                            {
                                                res = "0";
                                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 日值 公式 报程序异常: " + pMon + ":" + paras + ":" + month);
                                            }

                                            if (id != 0)
                                            {
                                                zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                                un[id] = dtMon.Rows[i]["单位"].ToString();
                                                lay[id] = dtMon.Rows[i]["设计值"].ToString();

                                                if (a < dtLevALLBZ.Rows.Count)
                                                {
                                                    subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ",";
                                                    subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                }
                                                else
                                                {
                                                    subStr = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + sh.ShowPoint(res, num) + ";";
                                                    subExc = dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ";";
                                                }

                                                dv[id] += subStr;
                                                dvExcel[id] += subExc;
                                            }

                                            if (ParaID != "")
                                            {
                                                if (htUn.ContainsKey(ParaID))
                                                {
                                                    re = htUn[ParaID] + dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",";
                                                    htUn.Remove(ParaID);
                                                    htUn.Add(ParaID, re);
                                                }
                                                else
                                                    htUn.Add(ParaID, dtLevALLBZ.Rows[l]["班组编号"].ToString().TrimEnd(' ') + "+" + res + ",");
                                            }
                                        }
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    a++;
                }
            }
            #endregion

            #region 调试模式
            if (TSMS == "yes")
            {
                ArrayList listGy = new ArrayList(htGY.Keys);
                ArrayList listUn = new ArrayList(htUn.Keys);

                listGy.Sort();
                listUn.Sort();

                foreach (string skey in listGy)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----" + skey + "---" + htGY[skey]); }

                foreach (string skey in listUn)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----" + skey + "---" + htUn[skey]); }
            }
            #endregion

            list.Add(zb);
            list.Add(un);
            list.Add(lay);
            list.Add(dv);
            list.Add(dvExcel);

            return list;
        }

        /// <summary>
        /// 日报
        /// 机组轮转
        /// </summary>
        /// <param name="repName"></param>
        /// <param name="qsrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public ArrayList RetArrsRepDay(string repName, string repType, string qsrq, string jsrq, bool isDay)
        {
            this.init();

            object obj;
            string bm = "";     //电量表码
            string mon = "";    //计算公式
            string res = "";    //计算结果
            string num = "";    //小数点数
            string unit = "";   //机组编码
            string dlID = "";   //电量ID
            string pName = "";  //测点名称
            string sDate = "";  //结束时间(实时值,SQL结束时间)
            string gsjb = "";   //公式级别
            string GYID = "";   //公用库参数ID
            string ParaID = ""; //参数ID

            int id;
            int ret = 0;
            double drvA = 0;
            double valGY = 0;

            //基础数据值
            string[] zb = null;
            string[] un = null;
            string[] lay = null;
            string[] dv = null;
            string[] dvExcel = null;

            DataTable dtGY = null;
            DataTable dtLel = null;     //机组级别
            DataTable dtLevZ = null;    //全厂级别
            DataTable dtLevUnit = null;
            Hashtable htGY = new Hashtable();
            Hashtable htUn = new Hashtable();
            ArrayList list = new ArrayList();

            count = pm.GetCount(repName);

            zb = new string[count + 1];
            un = new string[count + 1];
            lay = new string[count + 1];
            dv = new string[count + 1];
            dvExcel = new string[count + 1];

            if (isDay == true)
                sDate = DateTime.Parse(jsrq).ToString("yyyy-MM-dd H:59:59");
            else
                sDate = DateTime.Parse(jsrq).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");

            #region 公用库读取
            sql = "select * from GYReport where 报表名称='全厂级'";
            dtGY = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtGY = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtGY = DBdb2.RunDataTable(sql, out errMsg);

            if (dtGY != null && dtGY.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转开始!");

                for (int i = 0; i < dtGY.Rows.Count; i++)
                {
                    GYID = dtGY.Rows[i]["参数ID"].ToString();

                    if (dtGY.Rows[i][9].ToString() == "平均" || dtGY.Rows[i][9].ToString() == "累计")
                    {
                        pName = dtGY.Rows[i][repType].ToString();

                        //if (rtDBType == "PI")
                        //    pk.GetHisValue(pName, sDate, ref valGY);
                        //else if (rtDBType == "EDNA")
                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref valGY);

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + GYID + "----在公用库和报表配置表中有重复记录...");
                            else
                                htGY.Add(GYID, valGY);
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "电量")
                    {
                        dlID = dtGY.Rows[i]["电量点"].ToString();

                        //valGY = double.Parse(pc.GetPower(valGY, dlID, qsrq, jsrq, bm).ToString());

                        mon = "";

                        if (dtGY.Rows[i]["公式"].ToString() != "")
                            mon = valGY.ToString() + dtGY.Rows[i]["公式"].ToString();
                        else
                            mon = valGY.ToString();

                        res = "";
                        try
                        { res = StrHelper.Cale(mon); }
                        catch
                        { res = "0"; }

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + GYID + "----在公用库和报表配置表中有重复记录...");
                            else
                                htGY.Add(GYID, res);
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "SQL")
                    {
                        sql = dtGY.Rows[i]["SQL语句"].ToString();

                        if (sql.Contains("&bt&") && sql.Contains("&et&"))
                        {
                            sql = sql.Replace("&bt&", qsrq);
                            sql = sql.Replace("&et&", sDate);
                        }
                        obj = dl.RunSingle(sql, out errMsg);
                        //if (rlDBType == "SQL")
                        //    obj = DBsql.RunSingle(sql, out errMsg);
                        //else
                        //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + GYID + "----在公用库和报表配置表中有重复记录...");
                            else
                                htGY.Add(GYID, res);
                        }
                    }
                }
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转结束!");
            }
            #endregion

            #region 机组数据读取

            sql = "SELECT DISTINCT 机组 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组!=0";
            dtLevUnit = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevUnit = DBsql.RunDataTable(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    dtLevUnit = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLevUnit != null && dtLevUnit.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转开始!");

                for (int k = 0; k < dtLevUnit.Rows.Count; k++)
                {
                    unit = dtLevUnit.Rows[k]["机组"].ToString();

                    sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组!=0 and 公式级别!=0";
                    dtLel = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtLel = DBsql.RunDataTable(sql, out errMsg);
                    //else if (rlDBType == "DB2")
                    //    dtLel = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtLel.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtLel.Rows.Count; j++)
                        {
                            //公式级别
                            gsjb = dtLel.Rows[j]["公式级别"].ToString();

                            #region 平均
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='平均' and 公式级别=" + gsjb;

                            DataTable dtAvg = null;
                            dtAvg = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtAvg != null && dtAvg.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtAvg.Rows.Count; i++)
                                {
                                    //小数点
                                    num = dtAvg.Rows[i]["小数点数"].ToString();
                                    //点名
                                    pName = dtAvg.Rows[i][repType].ToString();
                                    //参数ID
                                    ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                                    //if (rtDBType == "EDNA")
                                    //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                    //else if (rtDBType == "PI")
                                    //    pk.GetHisValue(pName, sDate, ref drvA);

                                    if (id != 0)
                                    {
                                        zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                        un[id] = dtAvg.Rows[i]["单位"].ToString();
                                        lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvExcel[id] += drvA.ToString() + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUn.Add(ParaID, drvA.ToString());
                                    }
                                }
                            }
                            #endregion

                            #region 累计
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='累计' and 公式级别=" + gsjb;

                            DataTable dtDiff = null;
                            dtDiff = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                            //else if (rlDBType == "DB2")
                            //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDiff != null && dtDiff.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtDiff.Rows.Count; i++)
                                {
                                    num = dtDiff.Rows[i]["小数点数"].ToString();

                                    pName = dtDiff.Rows[i][repType].ToString();
                                    //参数ID
                                    ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                                    //if (rtDBType == "EDNA")
                                    //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                    //else if (rtDBType == "PI")
                                    //    pk.GetHisValue(pName, sDate, ref drvA);

                                    if (id != 0)
                                    {
                                        zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                        un[id] = dtDiff.Rows[i]["单位"].ToString();
                                        lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvExcel[id] += drvA.ToString() + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUn.Add(ParaID, drvA.ToString());
                                    }
                                }
                            }

                            #endregion

                            #region SQL

                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='SQL' and 公式级别=" + gsjb;

                            DataTable dtSQL = null;
                            dtSQL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtSQL != null && dtSQL.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSQL.Rows.Count; i++)
                                {
                                    num = dtSQL.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                                    sql = dtSQL.Rows[i]["SQL语句"].ToString();

                                    if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                    {
                                        sql = sql.Replace("&bt&", qsrq);
                                        sql = sql.Replace("&et&", sDate);
                                    }

                                    if (rlDBType == "SQL")
                                        obj = DBsql.RunSingle(sql, out errMsg);
                                    else
                                        obj = DBdb2.RunSingle(sql, out errMsg);

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

                                    if (id != 0)
                                    {
                                        zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                        un[id] = dtSQL.Rows[i]["单位"].ToString();
                                        lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";
                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUn.Add(ParaID, res);
                                    }
                                }
                            }
                            #endregion

                            #region 电量
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='电量' and 公式级别=" + gsjb;

                            DataTable dtDL = null;
                            dtDL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDL.Rows.Count > 0 && dtDL != null)
                            {
                                for (int i = 0; i < dtDL.Rows.Count; i++)
                                {
                                    num = dtDL.Rows[i]["小数点数"].ToString();

                                    //参数ID
                                    ParaID = dtDL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                                    dlID = dtDL.Rows[i]["电量点"].ToString();

                                    //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                                    mon = "";

                                    if (dtDL.Rows[i]["公式"].ToString() != "")
                                        mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                                    else
                                        mon = drvA.ToString();

                                    res = "";
                                    try
                                    { res = StrHelper.Cale(mon); }
                                    catch
                                    { res = "0"; }

                                    if (id != 0)
                                    {
                                        zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                        un[id] = dtSQL.Rows[i]["单位"].ToString();
                                        lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";
                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUn.Add(ParaID, res);
                                    }
                                }
                            }

                            #endregion

                            #region 其它
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                            DataTable dtQT = null;
                            dtQT = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtQT.Rows.Count > 0 && dtQT != null)
                            {
                                for (int i = 0; i < dtQT.Rows.Count; i++)
                                {
                                    //小数点
                                    num = dtQT.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtQT.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                    #region 购网电量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                                    {
                                        string re = um.gwdl(qsrq, jsrq, int.Parse(unit));

                                        if (id != 0)
                                        {
                                            zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                            un[id] = dtSQL.Rows[i]["单位"].ToString();
                                            lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                            dv[id] += sh.ShowPoint(re.ToString(), num) + ",";
                                            dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                            else
                                                htUn.Add(ParaID, re);
                                        }
                                    }
                                    #endregion

                                    #region 入炉煤低位发热量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                                    {
                                        string rlm = um.rlmdwfrl("日报", qsrq, jsrq, 1);

                                        if (id != 0)
                                        {
                                            zb[id] = dtQT.Rows[i]["参数名"].ToString();
                                            un[id] = dtQT.Rows[i]["单位"].ToString();
                                            lay[id] = dtQT.Rows[i]["设计值"].ToString();

                                            dv[id] += sh.ShowPoint(rlm, num) + ",";
                                            dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                        }


                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                            else
                                                htUn.Add(ParaID, rlm);
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region 公式
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='公式' and 公式级别=" + gsjb;

                            DataTable dtMon = null;
                            dtMon = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtMon != null && dtMon.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtMon.Rows.Count; i++)
                                {
                                    //小数点数
                                    num = dtMon.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtMon.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                                    string pMon = dtMon.Rows[i]["公式"].ToString();

                                    string paras = dtMon.Rows[i]["公式参数"].ToString();

                                    string month = pm.retMon(htGY, htUn, paras, pMon);

                                    if (month != "")
                                    {
                                        res = "";
                                        try
                                        {
                                            res = StrHelper.Cale(month);

                                            if (id != 0)
                                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";

                                            if (ParaID != "")
                                            {
                                                if (htUn.ContainsKey(ParaID))
                                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                                else
                                                    htUn.Add(ParaID, res);
                                            }

                                            res = sh.ShowPoint(res, num);
                                        }
                                        catch
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                            un[id] = dtMon.Rows[i]["单位"].ToString();
                                            lay[id] = dtMon.Rows[i]["设计值"].ToString();
                                            dv[id] += sh.ShowPoint(res, num) + ",";
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转结束!");
            }
            #endregion

            #region 全厂数据读取

            sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 公式级别!=0";
            dtLevZ = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevZ = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtLevZ = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLevZ.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----全厂开始!");

                for (int j = 0; j < dtLevZ.Rows.Count; j++)
                {
                    gsjb = dtLevZ.Rows[j]["公式级别"].ToString();

                    #region 平均
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='平均' and 公式级别=" + gsjb;

                    DataTable dtAvg = null;
                    dtAvg = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtAvg != null && dtAvg.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAvg.Rows.Count; i++)
                        {
                            //小数点
                            num = dtAvg.Rows[i]["小数点数"].ToString();
                            //点名
                            pName = dtAvg.Rows[i][repType].ToString();
                            //参数ID
                            ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                            //if (rtDBType == "EDNA")
                            //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //else
                            //    pk.GetHisValue(pName, sDate, ref drvA);

                            if (id != 0)
                            {
                                zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                un[id] = dtAvg.Rows[i]["单位"].ToString();
                                lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvExcel[id] += drvA.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUn.Add(ParaID, drvA.ToString());
                            }
                        }
                    }
                    #endregion

                    #region 累计
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='累计' and 公式级别=" + gsjb;

                    DataTable dtDiff = null;
                    dtDiff = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDiff != null && dtDiff.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDiff.Rows.Count; i++)
                        {
                            num = dtDiff.Rows[i]["小数点数"].ToString();

                            pName = dtDiff.Rows[i][repType].ToString();
                            //参数ID
                            ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                            //if (rtDBType == "EDNA")
                            //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //else
                            //    pk.GetHisValue(pName, sDate, ref drvA);

                            if (id != 0)
                            {
                                zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                un[id] = dtDiff.Rows[i]["单位"].ToString();
                                lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvExcel[id] += drvA.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUn.Add(ParaID, drvA.ToString());
                            }
                        }
                    }

                    #endregion

                    #region SQL

                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='SQL' and 公式级别=" + gsjb;

                    DataTable dtSQL = null;
                    dtSQL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtSQL != null && dtSQL.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSQL.Rows.Count; i++)
                        {
                            num = dtSQL.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                            sql = dtSQL.Rows[i]["SQL语句"].ToString();

                            if (sql.Contains("&bt&") && sql.Contains("&et&"))
                            {
                                sql = sql.Replace("&bt&", qsrq);
                                sql = sql.Replace("&et&", sDate);
                            }
                            obj = dl.RunSingle(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    obj = DBsql.RunSingle(sql, out errMsg);
                            //else
                            //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                            if (id != 0)
                            {
                                zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                un[id] = dtSQL.Rows[i]["单位"].ToString();
                                lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUn.Add(ParaID, res);
                            }
                        }
                    }
                    #endregion

                    #region 电量
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='电量' and 公式级别=" + gsjb;

                    DataTable dtDL = null;
                    dtDL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDL.Rows.Count > 0 && dtDL != null)
                    {
                        for (int i = 0; i < dtDL.Rows.Count; i++)
                        {
                            num = dtDL.Rows[i]["小数点数"].ToString();

                            //参数ID
                            ParaID = dtDL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                            dlID = dtDL.Rows[i]["电量点"].ToString();

                            //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                            mon = "";

                            if (dtDL.Rows[i]["公式"].ToString() != "")
                                mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                            else
                                mon = drvA.ToString();

                            res = "";
                            try
                            { res = StrHelper.Cale(mon); }
                            catch
                            { res = "0"; }

                            if (id != 0)
                            {
                                zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                un[id] = dtSQL.Rows[i]["单位"].ToString();
                                lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUn.Add(ParaID, res);
                            }
                        }
                    }

                    #endregion

                    #region 其它

                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='其它' and 公式级别=" + gsjb;

                    DataTable dtQT = null;
                    dtQT = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtQT.Rows.Count > 0 && dtQT != null)
                    {
                        for (int i = 0; i < dtQT.Rows.Count; i++)
                        {
                            //小数点
                            num = dtQT.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtQT.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                            #region 购网电量
                            if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                            {
                                string re = um.gwdl(qsrq, jsrq, 0);

                                if (id != 0)
                                {
                                    zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                    un[id] = dtSQL.Rows[i]["单位"].ToString();
                                    lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                    dv[id] += sh.ShowPoint(re.ToString(), num) + ",";
                                    dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                }

                                if (ParaID != "")
                                {
                                    if (htUn.ContainsKey(ParaID))
                                        LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                    else
                                        htUn.Add(ParaID, re);
                                }
                            }
                            #endregion

                            #region 入炉煤低位发热量
                            if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                            {
                                string rlm = um.rlmdwfrl("日报", qsrq, jsrq, 1);

                                if (id != 0)
                                {
                                    zb[id] = dtQT.Rows[i]["参数名"].ToString();
                                    un[id] = dtQT.Rows[i]["单位"].ToString();
                                    lay[id] = dtQT.Rows[i]["设计值"].ToString();

                                    dv[id] += sh.ShowPoint(rlm, num) + ",";
                                    dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                }

                                if (ParaID != "")
                                {

                                    if (htUn.ContainsKey(ParaID))
                                        LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                    else
                                        htUn.Add(ParaID, rlm);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 公式

                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='公式' and 公式级别=" + gsjb;

                    DataTable dtMon = null;
                    dtMon = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtMon != null && dtMon.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtMon.Rows.Count; i++)
                        {
                            //小数点数
                            num = dtMon.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtMon.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                            string pMon = dtMon.Rows[i]["公式"].ToString();

                            string paras = dtMon.Rows[i]["公式参数"].ToString();

                            string month = pm.retMon(htGY, htUn, paras, pMon);

                            if (month != "")
                            {
                                res = "";
                                try
                                { res = StrHelper.Cale(month); }
                                catch
                                { res = "0"; }

                                if (id != 0)
                                {
                                    zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                    un[id] = dtMon.Rows[i]["单位"].ToString();
                                    lay[id] = dtMon.Rows[i]["设计值"].ToString();

                                    dv[id] += sh.ShowPoint(res, num) + ",";
                                    dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                }

                                if (ParaID != "")
                                {
                                    if (htUn.ContainsKey(ParaID))
                                        LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                    else
                                        htUn.Add(ParaID, res);
                                }
                            }
                        }
                    }
                    #endregion
                }
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----全厂结束!");
            }

            #endregion

            #region 调试模式
            if (TSMS == "yes")
            {
                ArrayList listGy = new ArrayList(htGY.Keys);
                ArrayList listUn = new ArrayList(htUn.Keys);

                listGy.Sort();
                listUn.Sort();

                foreach (string skey in listGy)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----" + skey + "---" + htGY[skey]); }

                foreach (string skey in listUn)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----" + skey + "---" + htUn[skey]); }
            }
            #endregion

            list.Add(zb);
            list.Add(un);
            list.Add(lay);
            list.Add(dv);
            list.Add(dvExcel);

            return list;
        }

        /// <summary>
        /// 月、季、年报表
        /// 机组轮转
        /// </summary>
        /// <param name="repName"></param>
        /// <param name="repType"></param>
        /// <param name="pType"></param>
        /// <param name="qsrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        public ArrayList RetArrsRepMonth(string repName, string repType, string pType, string qsrq, string jsrq, string qsrqT, string jsrqT, bool isDay)
        {
            this.init();

            object obj;
            string bm = "";
            string mon = "";
            string monT = "";
            string res = "";
            string resT = "";
            string num = "";
            string unit = "";
            string dlID = "";
            string GYID = "";   //公用库参数ID
            string ParaID = "";
            string pName = "";
            string sDate = "";
            string sDateT = "";

            int id;
            int ret = 0;
            double drvA = 0;
            double drvB = 0;
            double valGY = 0;

            string[] zb = null;
            string[] un = null;
            string[] lay = null;
            string[] dv = null;
            string[] dvT = null;
            string[] dvExcel = null;
            string[] dvExcelT = null;

            DataTable dtGY = null;

            Hashtable htGY = new Hashtable();
            Hashtable htGYT = new Hashtable();
            Hashtable htUn = new Hashtable();
            Hashtable htUnT = new Hashtable();
            ArrayList list = new ArrayList();

            count = pm.GetCount(repName);

            zb = new string[count + 1];
            un = new string[count + 1];
            lay = new string[count + 1];
            dv = new string[count + 1];
            dvT = new string[count + 1];
            dvExcel = new string[count + 1];
            dvExcelT = new string[count + 1];

            if (isDay == true)
                sDate = DateTime.Parse(jsrq).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss"); //sDate = DateTime.Parse(jsrq).ToString("yyyy-MM-dd H:59:59");
            else
                sDate = DateTime.Parse(jsrq).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");

            sDateT = DateTime.Parse(jsrqT).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");

            #region 公用库读取
            sql = "select * from GYReport where 报表名称='全厂级'";
            dtGY = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtGY = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtGY = DBdb2.RunDataTable(sql, out errMsg);

            if (dtGY != null && dtGY.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转开始!");

                for (int i = 0; i < dtGY.Rows.Count; i++)
                {
                    GYID = dtGY.Rows[i]["参数ID"].ToString();

                    if (dtGY.Rows[i][9].ToString() == "平均" || dtGY.Rows[i][9].ToString() == "累计")
                    {
                        pName = dtGY.Rows[i][pType].ToString();

                        //if (rtDBType == "EDNA")
                        //{
                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                        //    ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                        //}
                        //else
                        //{
                        //    pk.GetHisValue(pName, sDate, ref drvA);
                        //    pk.GetHisValue(pName, sDateT, ref drvB);
                        //}

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID) || htGYT.ContainsKey(GYID))
                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + GYID + "----在公用库和报表配置表中有重复记录...");
                            else
                            {
                                htGY.Add(GYID, drvA);
                                htGYT.Add(GYID, drvB);
                            }
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "电量")
                    {
                        dlID = dtGY.Rows[i]["电量点"].ToString();

                        //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());
                        //drvB = double.Parse(pc.GetPower(drvB, dlID, qsrqT, jsrqT, bm).ToString());

                        mon = "";
                        monT = "";
                        if (dtGY.Rows[i]["公式"].ToString() != "")
                        {
                            mon = drvA.ToString() + dtGY.Rows[i]["公式"].ToString();
                            monT = drvB.ToString() + dtGY.Rows[i]["公式"].ToString();
                        }
                        else
                        {
                            mon = drvA.ToString();
                            monT = drvB.ToString();
                        }
                        res = "";
                        resT = "";

                        try
                        { res = StrHelper.Cale(mon); resT = StrHelper.Cale(monT); }
                        catch
                        { res = "0"; resT = "0"; }

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID) || htGYT.ContainsKey(GYID))
                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + GYID + "----在公用库和报表配置表中有重复记录...");
                            else
                            {
                                htGY.Add(GYID, res);
                                htGYT.Add(GYID, resT);
                            }
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "SQL")
                    {
                        //本期
                        sql = dtGY.Rows[i]["SQL语句"].ToString();

                        if (sql.Contains("&bt&") && sql.Contains("&et&"))
                        {
                            sql = sql.Replace("&bt&", qsrq);
                            sql = sql.Replace("&et&", sDate);
                        }
                        obj = dl.RunSingle(sql, out errMsg);
                        //if (rlDBType == "SQL")
                        //    obj = DBsql.RunSingle(sql, out errMsg);
                        //else
                        //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + GYID + "----在公用库和报表配置表中有重复记录...");
                            else
                                htGY.Add(GYID, res);
                        }

                        //同期
                        sql = dtGY.Rows[i]["SQL语句"].ToString();

                        if (sql.Contains("&bt&") && sql.Contains("&et&"))
                        {
                            sql = sql.Replace("&bt&", qsrqT);
                            sql = sql.Replace("&et&", sDateT);
                        }

                        if (rlDBType == "SQL")
                            obj = DBsql.RunSingle(sql, out errMsg);
                        else
                            obj = DBdb2.RunSingle(sql, out errMsg);

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

                        if (GYID != "")
                        {
                            if (htGYT.Contains(GYID))
                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + GYID + "----在公用库和报表配置表中有重复记录...");
                            else
                                htGYT.Add(GYID, res);
                        }
                    }
                }
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转结束!");
            }
            #endregion

            #region 机组数据读取
            sql = "SELECT DISTINCT 机组 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组!=0";

            DataTable dtLevUnit = null;
            dtLevUnit = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevUnit = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtLevUnit = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLevUnit != null && dtLevUnit.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转开始!");

                for (int k = 0; k < dtLevUnit.Rows.Count; k++)
                {
                    unit = dtLevUnit.Rows[k]["机组"].ToString();

                    sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组!=0 and 公式级别!=0";

                    DataTable dtLel = null;

                    if (rlDBType == "SQL")
                        dtLel = DBsql.RunDataTable(sql, out errMsg);
                    else
                        dtLel = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtLel.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtLel.Rows.Count; j++)
                        {
                            string gsjb = dtLel.Rows[j]["公式级别"].ToString();

                            #region 平均
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='平均' and 公式级别=" + gsjb;

                            DataTable dtAvg = null;
                            dtAvg = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtAvg != null && dtAvg.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtAvg.Rows.Count; i++)
                                {
                                    num = dtAvg.Rows[i]["小数点数"].ToString();

                                    pName = dtAvg.Rows[i][pType].ToString();
                                    //参数ID
                                    ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                                    //if (rtDBType == "EDNA")
                                    //{
                                    //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                    //    ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                                    //}
                                    //else
                                    //{
                                    //    pk.GetHisValue(pName, sDate, ref drvA);
                                    //    pk.GetHisValue(pName, sDateT, ref drvB);
                                    //}

                                    if (id != 0)
                                    {
                                        zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                        un[id] = dtAvg.Rows[i]["单位"].ToString();
                                        lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvT[id] += sh.ShowPoint(drvB.ToString(), num) + ",";

                                        dvExcel[id] += drvA.ToString() + ",";
                                        dvExcelT[id] += drvB.ToString() + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID) || htUnT.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                        {
                                            htUn.Add(ParaID, drvA.ToString());
                                            htUnT.Add(ParaID, drvB.ToString());
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 累计
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='累计' and 公式级别=" + gsjb;

                            DataTable dtDiff = null;
                            dtDiff = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDiff != null && dtDiff.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtDiff.Rows.Count; i++)
                                {
                                    num = dtDiff.Rows[i]["小数点数"].ToString();

                                    pName = dtDiff.Rows[i][pType].ToString();
                                    //参数ID
                                    ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                                    //if (rtDBType == "EDNA")
                                    //{
                                    //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                    //    ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                                    //}
                                    //else
                                    //{
                                    //    pk.GetHisValue(pName, sDate, ref drvA);
                                    //    pk.GetHisValue(pName, sDateT, ref drvB);
                                    //}

                                    if (id != 0)
                                    {
                                        zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                        un[id] = dtDiff.Rows[i]["单位"].ToString();
                                        lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvT[id] += sh.ShowPoint(drvB.ToString(), num) + ",";

                                        dvExcel[id] += drvA.ToString() + ",";
                                        dvExcelT[id] += drvB.ToString() + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID) || htUnT.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                        {
                                            htUn.Add(ParaID, drvA.ToString());
                                            htUnT.Add(ParaID, drvB.ToString());
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region SQL

                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='SQL' and 公式级别=" + gsjb;

                            DataTable dtSQL = null;
                            dtSQL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtSQL != null && dtSQL.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSQL.Rows.Count; i++)
                                {
                                    num = dtSQL.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                                    //本期
                                    sql = dtSQL.Rows[i]["SQL语句"].ToString();

                                    if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                    {
                                        sql = sql.Replace("&bt&", qsrq);
                                        sql = sql.Replace("&et&", sDate);
                                    }
                                    obj = dl.RunSingle(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    obj = DBsql.RunSingle(sql, out errMsg);
                                    //else
                                    //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                                    if (id != 0)
                                    {
                                        zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                        un[id] = dtSQL.Rows[i]["单位"].ToString();
                                        lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";

                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUn.Add(ParaID, res);
                                    }

                                    //同期
                                    sql = dtSQL.Rows[i]["SQL语句"].ToString();

                                    if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                    {
                                        sql = sql.Replace("&bt&", qsrqT);
                                        sql = sql.Replace("&et&", sDateT);
                                    }
                                    obj = dl.RunSingle(sql, out errMsg);
                                    //if (rlDBType == "SQL")
                                    //    obj = DBsql.RunSingle(sql, out errMsg);
                                    //else
                                    //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                                    if (id != 0)
                                    {
                                        zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                        un[id] = dtSQL.Rows[i]["单位"].ToString();
                                        lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                        dvT[id] += sh.ShowPoint(res, num) + ",";
                                        dvExcelT[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUnT.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUnT.Add(ParaID, res);
                                    }
                                }
                            }
                            #endregion

                            #region 电量

                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='电量' and 公式级别=" + gsjb;

                            DataTable dtDL = null;
                            dtDL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDL.Rows.Count > 0 && dtDL != null)
                            {
                                for (int i = 0; i < dtDL.Rows.Count; i++)
                                {
                                    num = dtDL.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtDL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                                    dlID = dtDL.Rows[i]["电量点"].ToString();

                                    //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                                    mon = "";

                                    if (dtDL.Rows[i]["公式"].ToString() != "")
                                        mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                                    else
                                        mon = drvA.ToString();

                                    res = "";
                                    try
                                    { res = StrHelper.Cale(mon); }
                                    catch
                                    { res = "0"; }

                                    if (id != 0)
                                    {
                                        zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                        un[id] = dtDL.Rows[i]["单位"].ToString();
                                        lay[id] = dtDL.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";

                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUn.Add(ParaID, res);
                                    }

                                    //drvB = double.Parse(pc.GetPower(drvB, dlID, qsrqT, jsrqT, bm).ToString());

                                    mon = "";

                                    if (dtDL.Rows[i]["公式"].ToString() != "")
                                        mon = drvB.ToString() + dtDL.Rows[i]["公式"].ToString();
                                    else
                                        mon = drvB.ToString();

                                    res = "";
                                    try
                                    { res = StrHelper.Cale(mon); }
                                    catch
                                    { res = "0"; }

                                    if (id != 0)
                                    {
                                        zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                        un[id] = dtDL.Rows[i]["单位"].ToString();
                                        lay[id] = dtDL.Rows[i]["设计值"].ToString();

                                        dvT[id] += sh.ShowPoint(res, num) + ",";

                                        dvExcelT[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUnT.ContainsKey(ParaID))
                                            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                        else
                                            htUnT.Add(ParaID, res);
                                    }
                                }
                            }

                            #endregion

                            #region 其它
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                            DataTable dtQT = null;
                            dtQT = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtQT.Rows.Count > 0 && dtQT != null)
                            {
                                for (int i = 0; i < dtQT.Rows.Count; i++)
                                {
                                    num = dtQT.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtQT.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                    #region 购网电量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                                    {
                                        //string re = um.gwdl(qsrq, jsrq, int.Parse(unit));

                                        //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                        //    dv[id] += sh.ShowPoint(re.ToString(), num) + ",";

                                        //    dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                        //}

                                        //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                        //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + re.ToString() + ";";
                                        //}

                                        ////同期
                                        //string reT = um.gwdl(qsrqT, jsrqT, int.Parse(unit));

                                        //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                        //    dvT[id] += sh.ShowPoint(reT.ToString(), num) + ",";

                                        //    dvExcelT[id] += sh.ShowPoint(reT.ToString(), "-1") + ",";
                                        //}

                                        //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                        //    duT[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + reT.ToString() + ";";
                                        //}

                                    }
                                    #endregion

                                    #region 入炉煤低位发热量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                                    {
                                        //string rlm = um.rlmdwfrl("月报", qsrq, jsrq, 1);

                                        //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                        //    dv[id] += sh.ShowPoint(rlm, num) + ",";

                                        //    dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                        //}

                                        //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                        //    //if (num == "")
                                        //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + rlm + ";";
                                        //    //else
                                        //    //  du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlm, num) + ";";
                                        //}

                                        //string rlmT = um.rlmdwfrl(repType, qsrqT, jsrqT, 1);

                                        //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                        //    dvT[id] += sh.ShowPoint(rlmT, num) + ",";

                                        //    dvExcelT[id] += sh.ShowPoint(rlmT, "-1") + ",";
                                        //}

                                        //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                        //    //if (num == "")
                                        //    duT[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + rlmT + ";";
                                        //    //else
                                        //    //    duT[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlmT, num) + ";";
                                        //}
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region 公式
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='公式' and 公式级别=" + gsjb;

                            DataTable dtMon = null;
                            dtMon = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtMon != null && dtMon.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtMon.Rows.Count; i++)
                                {
                                    num = dtMon.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtMon.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                                    string pMon = dtMon.Rows[i]["公式"].ToString();
                                    string paras = dtMon.Rows[i]["公式参数"].ToString();

                                    string month = pm.retMon(htGY, htUn, paras, pMon);
                                    string monthT = pm.retMon(htGYT, htUnT, paras, pMon);

                                    if (month != "")
                                    {
                                        res = "";

                                        try
                                        { res = StrHelper.Cale(month); }
                                        catch
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                            un[id] = dtMon.Rows[i]["单位"].ToString();
                                            lay[id] = dtMon.Rows[i]["设计值"].ToString();

                                            dv[id] += sh.ShowPoint(res, num) + ",";

                                            dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.ContainsKey(ParaID))
                                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                            else
                                                htUn.Add(ParaID, res);
                                        }
                                    }

                                    if (monthT != "")
                                    {
                                        res = "";
                                        try
                                        { res = StrHelper.Cale(monthT); }
                                        catch
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            dvT[id] += sh.ShowPoint(res, num) + ",";
                                            dvExcelT[id] += sh.ShowPoint(res, "-1") + ",";
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUnT.ContainsKey(ParaID))
                                                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                            else
                                                htUnT.Add(ParaID, res);
                                        }
                                    }
                                }
                            }

                            #endregion

                        }
                    }
                }

                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转结束!");
            }
            #endregion

            #region 全厂数据读取

            //全厂数据
            sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 公式级别!=0";

            DataTable dtLevZ = null;
            dtLevZ = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLevZ = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtLevZ = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLevZ.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----全厂开始!");

                for (int j = 0; j < dtLevZ.Rows.Count; j++)
                {
                    string gsjb = dtLevZ.Rows[j]["公式级别"].ToString();

                    #region 平均
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='平均' and 公式级别=" + gsjb;

                    DataTable dtAvg = null;
                    dtAvg = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtAvg != null && dtAvg.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAvg.Rows.Count; i++)
                        {
                            num = dtAvg.Rows[i]["小数点数"].ToString();

                            pName = dtAvg.Rows[i][pType].ToString();
                            //参数ID
                            ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                            //if (rtDBType == "EDNA")
                            //{
                            //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //    ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                            //}
                            //else
                            //{
                            //    pk.GetHisValue(pName, sDate, ref drvA);
                            //    pk.GetHisValue(pName, sDateT, ref drvB);
                            //}

                            if (id != 0)
                            {
                                zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                un[id] = dtAvg.Rows[i]["单位"].ToString();
                                lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvT[id] += sh.ShowPoint(drvB.ToString(), num) + ",";

                                dvExcel[id] += drvA.ToString() + ",";
                                dvExcelT[id] += drvB.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID) || htUnT.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                {
                                    htUn.Add(ParaID, drvA.ToString());
                                    htUnT.Add(ParaID, drvB.ToString());
                                }
                            }
                        }
                    }
                    #endregion

                    #region 累计
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='累计' and 公式级别=" + gsjb;

                    DataTable dtDiff = null;
                    dtDiff = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDiff != null && dtDiff.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDiff.Rows.Count; i++)
                        {
                            num = dtDiff.Rows[i]["小数点数"].ToString();

                            pName = dtDiff.Rows[i][pType].ToString();
                            //参数ID
                            ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                            //if (rtDBType == "EDNA")
                            //{
                            //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //    ek.GetHisValueByTime(pName, sDateT, ref ret, ref drvB);
                            //}
                            //else
                            //{
                            //    pk.GetHisValue(pName, sDate, ref drvA);
                            //    pk.GetHisValue(pName, sDateT, ref drvB);
                            //}

                            if (id != 0)
                            {
                                zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                un[id] = dtDiff.Rows[i]["单位"].ToString();
                                lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvT[id] += sh.ShowPoint(drvB.ToString(), num) + ",";

                                dvExcel[id] += drvA.ToString() + ",";
                                dvExcelT[id] += drvB.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID) || htUnT.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                {
                                    htUn.Add(ParaID, drvA.ToString());
                                    htUnT.Add(ParaID, drvB.ToString());
                                }
                            }
                        }
                    }

                    #endregion

                    #region SQL

                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='SQL' and 公式级别=" + gsjb;

                    DataTable dtSQL = null;
                    dtSQL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtSQL != null && dtSQL.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSQL.Rows.Count; i++)
                        {
                            num = dtSQL.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                            //本期
                            sql = dtSQL.Rows[i]["SQL语句"].ToString();

                            if (sql.Contains("&bt&") && sql.Contains("&et&"))
                            {
                                sql = sql.Replace("&bt&", qsrq);
                                sql = sql.Replace("&et&", sDate);
                            }
                            obj = dl.RunSingle(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    obj = DBsql.RunSingle(sql, out errMsg);
                            //else
                            //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                            if (id != 0)
                            {
                                zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                un[id] = dtSQL.Rows[i]["单位"].ToString();
                                lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUn.Add(ParaID, res);
                            }

                            //同期
                            sql = dtSQL.Rows[i]["SQL语句"].ToString();

                            if (sql.Contains("&bt&") && sql.Contains("&et&"))
                            {
                                sql = sql.Replace("&bt&", qsrqT);
                                sql = sql.Replace("&et&", sDateT);
                            }
                            obj = dl.RunSingle(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    obj = DBsql.RunSingle(sql, out errMsg);
                            //else
                            //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                            if (id != 0)
                            {
                                dvT[id] += sh.ShowPoint(res, num) + ",";
                                dvExcelT[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUnT.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUnT.Add(ParaID, res);
                            }
                        }
                    }
                    #endregion

                    #region 电量
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='电量' and 公式级别=" + gsjb;

                    DataTable dtDL = null;
                    dtDL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDL.Rows.Count > 0 && dtDL != null)
                    {
                        for (int i = 0; i < dtDL.Rows.Count; i++)
                        {
                            num = dtDL.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtDL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                            dlID = dtDL.Rows[i]["电量点"].ToString();

                            //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq, jsrq, bm).ToString());

                            mon = "";

                            if (dtDL.Rows[i]["公式"].ToString() != "")
                                mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                            else
                                mon = drvA.ToString();

                            res = "";
                            try
                            { res = StrHelper.Cale(mon); }
                            catch
                            { res = "0"; }

                            if (id != 0)
                            {
                                zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                un[id] = dtDL.Rows[i]["单位"].ToString();
                                lay[id] = dtDL.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUn.Add(ParaID, res);
                            }

                            //drvB = double.Parse(pc.GetPower(drvB, dlID, qsrqT, jsrqT, bm).ToString());

                            mon = "";

                            if (dtDL.Rows[i]["公式"].ToString() != "")
                                mon = drvB.ToString() + dtDL.Rows[i]["公式"].ToString();
                            else
                                mon = drvB.ToString();

                            res = "";
                            try
                            { res = StrHelper.Cale(mon); }
                            catch
                            { res = "0"; }

                            if (id != 0)
                            {
                                dvT[id] += sh.ShowPoint(res, num) + ",";
                                dvExcelT[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUnT.ContainsKey(ParaID))
                                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                else
                                    htUnT.Add(ParaID, res);
                            }
                        }
                    }

                    #endregion

                    #region 其它
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='其它' and 公式级别=" + gsjb;

                    DataTable dtQT = null;
                    dtQT = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtQT.Rows.Count > 0 && dtQT != null)
                    {
                        for (int i = 0; i < dtQT.Rows.Count; i++)
                        {
                            num = dtQT.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtQT.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                            #region 购网电量
                            if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                            {
                                //string re = um.gwdl(qsrq, jsrq, 0);

                                //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                //    dv[id] += sh.ShowPoint(re.ToString(), num) + ",";

                                //    dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                //}

                                //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + re.ToString() + ";";
                                //}

                                //string reT = um.gwdl(qsrqT, jsrqT, 0);

                                //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                //    dvT[id] += sh.ShowPoint(reT.ToString(), num) + ",";

                                //    dvExcelT[id] += sh.ShowPoint(reT.ToString(), "-1") + ",";
                                //}

                                //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                //    duT[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + reT.ToString() + ";";
                                //}

                            }
                            #endregion

                            #region 入炉煤低位发热量
                            if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                            {
                                //string rlm = um.rlmdwfrl("月报", qsrq, jsrq, 1);

                                //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                //    dv[id] += sh.ShowPoint(rlm, num) + ",";

                                //    dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                //}

                                //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                //    //if (num == "")
                                //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + rlm + ";";
                                //    //else
                                //    //    du[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlm, num) + ";";
                                //}

                                //string rlmT = um.rlmdwfrl(repType, qsrqT, jsrqT, 1);

                                //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                //    dvT[id] += sh.ShowPoint(rlmT, num) + ",";

                                //    dvExcelT[id] += sh.ShowPoint(rlmT, "-1") + ",";
                                //}

                                //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                //    //if (num == "")
                                //    duT[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + rlmT + ";";
                                //    //else
                                //    //    duT[id] += dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlmT, num) + ";";
                                //}
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 公式
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=0 and 参数类型='公式' and 公式级别=" + gsjb;

                    DataTable dtMon = null;
                    dtMon = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtMon != null && dtMon.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtMon.Rows.Count; i++)
                        {
                            num = dtMon.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtMon.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                            string pMon = dtMon.Rows[i]["公式"].ToString();
                            string paras = dtMon.Rows[i]["公式参数"].ToString();

                            string month = pm.retMon(htGY, htUn, paras, pMon);
                            string monthT = pm.retMon(htGYT, htUnT, paras, pMon);

                            if (month != "")
                            {
                                res = "";
                                try
                                { res = StrHelper.Cale(month); }
                                catch
                                { res = "0"; }

                                if (id != 0)
                                {
                                    zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                    un[id] = dtMon.Rows[i]["单位"].ToString();
                                    lay[id] = dtMon.Rows[i]["设计值"].ToString();

                                    dv[id] += sh.ShowPoint(res, num) + ",";
                                    dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                }

                                if (ParaID != "")
                                {
                                    if (htUn.ContainsKey(ParaID))
                                        LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                    else
                                        htUn.Add(ParaID, res);
                                }
                            }

                            if (monthT != "")
                            {
                                res = "";
                                try
                                { res = StrHelper.Cale(monthT); }
                                catch
                                { res = "0"; }

                                if (id != 0)
                                {
                                    dvT[id] += sh.ShowPoint(res, num) + ",";
                                    dvExcelT[id] += sh.ShowPoint(res, "-1") + ",";
                                }

                                if (ParaID != "")
                                {
                                    if (htUnT.ContainsKey(ParaID))
                                        LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "--参数ID--" + ParaID + "----在报表配置表中有重复");
                                    else
                                        htUnT.Add(ParaID, res);
                                }
                            }
                        }
                    }

                    #endregion
                }

                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----全厂结束!");
            }

            #endregion

            #region 调试模式
            if (TSMS == "yes")
            {
                ArrayList listGy = new ArrayList(htGY.Keys);
                ArrayList listGyt = new ArrayList(htGYT.Keys);
                ArrayList listUn = new ArrayList(htUn.Keys);
                ArrayList listUnt = new ArrayList(htUnT.Keys);

                listGy.Sort();
                listGyt.Sort();
                listUn.Sort();
                listUnt.Sort();

                foreach (string skey in listGy)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "--本期--" + skey + "---" + htGY[skey]); }

                foreach (string skey in listUn)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "--本期--" + skey + "---" + htUn[skey]); }

                foreach (string skey in listGyt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "--同期--" + skey + "---" + htGYT[skey]); }

                foreach (string skey in listUnt)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "--同期--" + skey + "---" + htUnT[skey]); }
            }

            #endregion

            list.Add(zb);
            list.Add(un);
            list.Add(lay);
            list.Add(dv);
            list.Add(dvT);
            list.Add(dvExcel);
            list.Add(dvExcelT);

            return list;
        }

        /// <summary>
        /// 月明细报表
        /// 机组轮转
        /// </summary>
        /// <param name="repName"></param>
        /// <param name="repType"></param>
        /// <param name="date"></param>
        /// <param name="isDay"></param>
        /// <returns></returns>
        public ArrayList RetArrsRepMonthInfo(string repName, int unit, DateTime date, int isDay)
        {
            this.init();

            int days;
            object obj;
            string bm = "";
            string mon = "";
            string res = "";
            string num = "";
            string str = "";
            string sDate = "";
            string GYID = "";   //公用库参数ID
            string ParaID = ""; //参数ID 
            string fDay = "";
            string lDay = "";
            string dlID = "";
            string pName = "";
            string dType = "日点";
            string mType = "月点";
            string repType = "月明细";
            int id;
            int ret = 0;
            double drvA = 0;
            double drvB = 0;
            double valGY = 0;

            string[] zb = null;
            string[] un = null;
            string[] lay = null;
            string[] dv = null;
            string[] dvExcel = null;
            string[] qsrq = null;
            string[] jsrq = null;

            DataTable dtGY = null;

            Hashtable htGY = new Hashtable();
            Hashtable htUn = new Hashtable();
            ArrayList list = new ArrayList();

            count = pm.GetCount(repName);
            days = DateTime.DaysInMonth(date.Year, date.Month);

            #region 日期
            if (isDay == 0)
            {
                qsrq = new string[days + 1];
                jsrq = new string[days + 1];

                for (int i = 1; i < DateTime.Now.Day; i++)
                {
                    qsrq[i] = date.Year + "-" + date.Month + "-" + i + " 00:00:00";
                    jsrq[i] = DateTime.Parse(date.Year + "-" + date.Month + "-" + i + " 00:00:00").AddDays(1).ToString();
                }
                fDay = qsrq[1];
                lDay = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");
            }
            else
            {
                qsrq = new string[days + 1];
                jsrq = new string[days + 1];

                for (int i = 1; i < days + 1; i++)
                {
                    qsrq[i] = date.Year + "-" + date.Month + "-" + i + " 00:00:00";
                    jsrq[i] = DateTime.Parse(date.Year + "-" + date.Month + "-" + i + " 00:00:00").AddDays(1).ToString();
                }
                fDay = qsrq[1];
                lDay = jsrq[days];
            }

            #endregion

            zb = new string[count + 1];
            un = new string[count + 1];
            lay = new string[count + 1];
            dv = new string[count + 1];
            dvExcel = new string[count + 1];

            #region 公用库读取

            sql = "select * from GYReport where 报表名称='全厂级'";
            dtGY = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtGY = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtGY = DBdb2.RunDataTable(sql, out errMsg);

            if (dtGY != null && dtGY.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转开始!");

                for (int k = 1; k <= 31; k++)
                {
                    if (k < qsrq.Length)
                    {
                        if (qsrq[k] != null)
                            sDate = DateTime.Parse(qsrq[k]).ToString("yyyy-MM-dd 23:59:59");

                        for (int i = 0; i < dtGY.Rows.Count; i++)
                        {
                            GYID = dtGY.Rows[i]["参数ID"].ToString();

                            if (dtGY.Rows[i][9].ToString() == "平均" || dtGY.Rows[i][9].ToString() == "累计")
                            {
                                pName = dtGY.Rows[i][dType].ToString();

                                if (qsrq[k] != null)
                                {
                                    //if (rtDBType == "PI")
                                    //    pk.GetHisValue(pName, sDate, ref valGY);
                                    //else if (rtDBType == "EDNA")
                                    //    ek.GetHisValueByTime(pName, sDate, ref ret, ref valGY);
                                }
                                else
                                    valGY = 0;

                                if (GYID != "")
                                {
                                    if (htGY.Contains(GYID))
                                    {
                                        str = htGY[GYID] + valGY.ToString() + ",";
                                        htGY.Remove(GYID);
                                        htGY.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = valGY + ",";
                                        htGY.Add(GYID, str);
                                    }
                                }
                            }
                            else if (dtGY.Rows[i][9].ToString() == "电量")
                            {
                                dlID = dtGY.Rows[i]["电量点"].ToString();

                                //if (qsrq[k] != null)
                                //    //valGY = double.Parse(pc.GetPower(valGY, dlID, qsrq[k], jsrq[k], bm).ToString());
                                //else
                                //    valGY = 0;

                                mon = "";

                                if (dtGY.Rows[i]["公式"].ToString() != "")
                                    mon = valGY.ToString() + dtGY.Rows[i]["公式"].ToString();
                                else
                                    mon = valGY.ToString();

                                res = "";
                                try
                                { res = StrHelper.Cale(mon); }
                                catch
                                { res = "0"; }

                                if (GYID != "")
                                {
                                    if (htGY.Contains(GYID))
                                    {
                                        str = htGY[GYID] + res + ",";
                                        htGY.Remove(GYID);
                                        htGY.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = res + ",";
                                        htGY.Add(GYID, str);
                                    }
                                }
                            }
                            else if (dtGY.Rows[i][9].ToString() == "SQL")
                            {
                                sql = dtGY.Rows[i]["SQL语句"].ToString();

                                if (qsrq[k] != null)
                                {
                                    if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                    {
                                        sql = sql.Replace("&bt&", qsrq[k].ToString());
                                        sql = sql.Replace("&et&", sDate);
                                    }

                                    if (rlDBType == "SQL")
                                        obj = DBsql.RunSingle(sql, out errMsg);
                                    else
                                        obj = DBdb2.RunSingle(sql, out errMsg);

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

                                if (GYID != "")
                                {
                                    if (htGY.Contains(GYID))
                                    {
                                        str = htGY[GYID] + res + ",";
                                        htGY.Remove(GYID);
                                        htGY.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = res + ",";
                                        htGY.Add(GYID, str);
                                    }
                                }
                            }
                        }
                    }
                }

                //公用库月值
                sDate = DateTime.Parse(lDay).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                for (int i = 0; i < dtGY.Rows.Count; i++)
                {
                    GYID = dtGY.Rows[i]["参数ID"].ToString();

                    if (dtGY.Rows[i][9].ToString() == "平均" || dtGY.Rows[i][9].ToString() == "累计")
                    {
                        pName = dtGY.Rows[i][mType].ToString();

                        //if (rtDBType == "PI")
                        //    pk.GetHisValue(pName, sDate, ref valGY);
                        //else if (rtDBType == "EDNA")
                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref valGY);

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                            {
                                str = htGY[GYID] + valGY.ToString() + ",";
                                htGY.Remove(GYID);
                                htGY.Add(GYID, str);
                            }
                            else
                            {
                                str = valGY + ",";
                                htGY.Add(GYID, str);
                            }
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "电量")
                    {
                        dlID = dtGY.Rows[i]["电量点"].ToString();

                        //valGY = double.Parse(pc.GetPower(valGY, dlID, fDay, lDay, bm).ToString());

                        mon = "";

                        if (dtGY.Rows[i]["公式"].ToString() != "")
                            mon = valGY.ToString() + dtGY.Rows[i]["公式"].ToString();
                        else
                            mon = valGY.ToString();

                        res = "";
                        try
                        { res = StrHelper.Cale(mon); }
                        catch
                        { res = "0"; }

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                            {
                                str = htGY[GYID] + res + ",";
                                htGY.Remove(GYID);
                                htGY.Add(GYID, str);
                            }
                            else
                            {
                                str = res + ",";
                                htGY.Add(GYID, str);
                            }
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "SQL")
                    {
                        sql = dtGY.Rows[i]["SQL语句"].ToString();

                        if (sql.Contains("&bt&") && sql.Contains("&et&"))
                        {
                            sql = sql.Replace("&bt&", fDay);
                            sql = sql.Replace("&et&", sDate);
                        }
                        obj = dl.RunSingle(sql, out errMsg);
                        //if (rlDBType == "SQL")
                        //    obj = DBsql.RunSingle(sql, out errMsg);
                        //else
                        //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                            {
                                str = htGY[GYID] + res + ",";
                                htGY.Remove(GYID);
                                htGY.Add(GYID, str);
                            }
                            else
                            {
                                str = res + ",";
                                htGY.Add(GYID, str);
                            }
                        }
                    }
                }

                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转结束!");
            }
            #endregion

            #region 机组数据读取

            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转开始!");

            for (int k = 1; k <= 31; k++)
            {
                if (k < qsrq.Length)
                {
                    if (qsrq[k] != null)
                        sDate = DateTime.Parse(qsrq[k]).ToString("yyyy-MM-dd 23:59:59");

                    sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 公式级别!=0";

                    DataTable dtLel = null;
                    dtLel = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtLel = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtLel = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtLel.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtLel.Rows.Count; j++)
                        {
                            string gsjb = dtLel.Rows[j]["公式级别"].ToString();

                            #region 平均
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='平均' and 公式级别=" + gsjb;

                            DataTable dtAvg = null;
                            dtAvg = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtAvg != null && dtAvg.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtAvg.Rows.Count; i++)
                                {
                                    num = dtAvg.Rows[i]["小数点数"].ToString();

                                    pName = dtAvg.Rows[i][dType].ToString();
                                    //参数ID
                                    ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                                    if (qsrq[k] != null)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                        //else
                                        //    pk.GetHisValue(pName, sDate, ref drvA);
                                    }
                                    else
                                        drvA = 0;

                                    if (id != 0)
                                    {
                                        zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                        un[id] = dtAvg.Rows[i]["单位"].ToString();
                                        lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvExcel[id] += drvA.ToString() + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + drvA.ToString() + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = drvA.ToString() + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 累计
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='累计' and 公式级别=" + gsjb;

                            DataTable dtDiff = null;
                            dtDiff = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDiff != null && dtDiff.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtDiff.Rows.Count; i++)
                                {
                                    num = dtDiff.Rows[i]["小数点数"].ToString();

                                    pName = dtDiff.Rows[i][dType].ToString();
                                    //参数ID
                                    ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                                    drvA = 0;

                                    if (qsrq[k] != null)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                                        //else
                                        //    pk.GetHisValue(pName, sDate, ref drvA);
                                    }
                                    else
                                        drvA = 0;

                                    if (id != 0)
                                    {
                                        zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                        un[id] = dtDiff.Rows[i]["单位"].ToString();
                                        lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvExcel[id] += drvA.ToString() + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + drvA.ToString() + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = drvA.ToString() + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region SQL

                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='SQL' and 公式级别=" + gsjb;

                            DataTable dtSQL = null;
                            dtSQL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtSQL != null && dtSQL.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSQL.Rows.Count; i++)
                                {
                                    num = dtSQL.Rows[i]["小数点数"].ToString();

                                    sql = dtSQL.Rows[i]["SQL语句"].ToString();
                                    //参数ID
                                    ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                                    if (qsrq[k] != null)
                                    {
                                        if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                        {
                                            sql = sql.Replace("&bt&", qsrq[k].ToString());
                                            sql = sql.Replace("&et&", sDate);
                                        }
                                        obj = dl.RunSingle(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    obj = DBsql.RunSingle(sql, out errMsg);
                                        //else
                                        //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                                    if (id != 0)
                                    {
                                        zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                        un[id] = dtSQL.Rows[i]["单位"].ToString();
                                        lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";
                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + res + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = res + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 电量
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='电量' and 公式级别=" + gsjb;

                            DataTable dtDL = null;
                            dtDL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDL.Rows.Count > 0 && dtDL != null)
                            {
                                for (int i = 0; i < dtDL.Rows.Count; i++)
                                {
                                    num = dtDL.Rows[i]["小数点数"].ToString();

                                    dlID = dtDL.Rows[i]["电量点"].ToString();
                                    //参数ID
                                    ParaID = dtDL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                                    //if (qsrq[k] != null)
                                    //   // drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq[k], jsrq[k], bm).ToString());
                                    //else
                                    //    drvA = 0;

                                    mon = "";

                                    if (dtDL.Rows[i]["公式"].ToString() != "")
                                        mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                                    else
                                        mon = drvA.ToString();

                                    res = "";
                                    try
                                    { res = StrHelper.Cale(mon); }
                                    catch
                                    { res = "0"; }

                                    if (id != 0)
                                    {
                                        zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                        un[id] = dtDL.Rows[i]["单位"].ToString();
                                        lay[id] = dtDL.Rows[i]["设计值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";
                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + res + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = res + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region 其它
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                            DataTable dtQT = null;
                            dtQT = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtQT.Rows.Count > 0 && dtQT != null)
                            {
                                for (int i = 0; i < dtQT.Rows.Count; i++)
                                {
                                    num = dtQT.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtQT.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                    #region 购网电量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                                    {
                                        //string re = "0";

                                        //if (qsrq[k] != null)
                                        //    re = um.gwdl(qsrq[k].ToString(), jsrq[k].ToString(), unit);

                                        //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                        //    dv[id] += sh.ShowPoint(re.ToString(), num) + ",";

                                        //    dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                        //}

                                        //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                        //    du[id] = dtQT.Rows[i]["参数ID"].ToString() + "," + re.ToString() + ";";
                                        //}
                                    }
                                    #endregion

                                    #region 入炉煤低位发热量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                                    {
                                        //string rlm = um.rlmdwfrl("日报", qsrq[k], jsrq[k], 1);

                                        //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                        //    dv[id] += sh.ShowPoint(rlm, num) + ",";

                                        //    dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                        //}

                                        //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                        //{
                                        //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                        //    //if (num == "")
                                        //    du[id] = dtQT.Rows[i]["参数ID"].ToString() + "," + rlm + ";";
                                        //    //else
                                        //    //    du[id] = dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlm, num) + ";";
                                        //}
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region 公式
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='公式' and 公式级别=" + gsjb;

                            DataTable dtMon = null;
                            dtMon = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtMon != null && dtMon.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtMon.Rows.Count; i++)
                                {
                                    num = dtMon.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtMon.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                                    string pMon = dtMon.Rows[i]["公式"].ToString();

                                    string paras = dtMon.Rows[i]["公式参数"].ToString();

                                    string month = pm.retMonByMonthAndYear(htGY, htUn, paras, pMon);

                                    if (month != "")
                                    {
                                        res = "";
                                        try
                                        { res = StrHelper.Cale(month); }
                                        catch
                                        { res = "0"; }

                                        if (id != 0)
                                        {
                                            zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                            un[id] = dtMon.Rows[i]["单位"].ToString();
                                            lay[id] = dtMon.Rows[i]["设计值"].ToString();

                                            if (qsrq[k] != null)
                                                dv[id] += sh.ShowPoint(res, num) + ",";
                                            else
                                                dv[id] += sh.ShowPoint("0", num) + ",";

                                            if (qsrq[k] != null)
                                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                            else
                                                dvExcel[id] += "0" + ",";
                                        }

                                        if (ParaID != "")
                                        {
                                            if (htUn.Contains(ParaID))
                                            {
                                                str = htUn[ParaID] + res + ",";
                                                htUn.Remove(ParaID);
                                                htUn.Add(ParaID, str);
                                            }
                                            else
                                            {
                                                str = res + ",";
                                                htUn.Add(ParaID, str);
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

            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转结束!");

            #endregion

            #region 全厂数据读取
            sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 公式级别!=0";

            DataTable dtLelMonth = null;
            dtLelMonth = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLelMonth = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtLelMonth = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLelMonth.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----全厂开始!");

                if (isDay == 0)
                    sDate = DateTime.Parse(lDay).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");
                else
                    sDate = DateTime.Parse(lDay).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                for (int j = 0; j < dtLelMonth.Rows.Count; j++)
                {
                    string gsjb = dtLelMonth.Rows[j]["公式级别"].ToString();

                    #region 平均
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='平均' and 公式级别=" + gsjb;

                    DataTable dtAvg = null;
                    dtAvg = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtAvg != null && dtAvg.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAvg.Rows.Count; i++)
                        {
                            num = dtAvg.Rows[i]["小数点数"].ToString();

                            pName = dtAvg.Rows[i][mType].ToString();
                            //参数ID
                            ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                            //if (rtDBType == "EDNA")
                            //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //else
                            //    pk.GetHisValue(pName, sDate, ref drvA);

                            if (id != 0)
                            {
                                zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                un[id] = dtAvg.Rows[i]["单位"].ToString();
                                lay[id] = dtAvg.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvExcel[id] += drvA.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + drvA.ToString() + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = drvA.ToString() + ",";
                                    htUn.Add(ParaID, str);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 累计
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='累计' and 公式级别=" + gsjb;

                    DataTable dtDiff = null;
                    dtDiff = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDiff != null && dtDiff.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDiff.Rows.Count; i++)
                        {
                            num = dtDiff.Rows[i]["小数点数"].ToString();

                            pName = dtDiff.Rows[i][mType].ToString();
                            //参数ID
                            ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                            //if (rtDBType == "EDNA")
                            //    ek.GetHisValueByTime(pName, sDate, ref ret, ref drvA);
                            //else
                            //    pk.GetHisValue(pName, sDate, ref drvA);

                            if (id != 0)
                            {
                                zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                un[id] = dtDiff.Rows[i]["单位"].ToString();
                                lay[id] = dtDiff.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvExcel[id] += drvA.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + drvA.ToString() + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = drvA.ToString() + ",";
                                    htUn.Add(ParaID, str);
                                }
                            }
                        }
                    }

                    #endregion

                    #region SQL

                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='SQL' and 公式级别=" + gsjb;

                    DataTable dtSQL = null;
                    dtSQL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtSQL != null && dtSQL.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSQL.Rows.Count; i++)
                        {
                            num = dtSQL.Rows[i]["小数点数"].ToString();

                            sql = dtSQL.Rows[i]["SQL语句"].ToString();
                            //参数ID
                            ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                            if (sql.Contains("&bt&") && sql.Contains("&et&"))
                            {
                                sql = sql.Replace("&bt&", fDay);
                                sql = sql.Replace("&et&", sDate);
                            }
                            obj = DBsql.RunSingle(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    obj = DBsql.RunSingle(sql, out errMsg);
                            //else
                            //    obj = DBdb2.RunSingle(sql, out errMsg);

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


                            if (id != 0)
                            {
                                zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                un[id] = dtSQL.Rows[i]["单位"].ToString();
                                lay[id] = dtSQL.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + res + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = res + ",";
                                    htUn.Add(ParaID, str);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 电量
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='电量' and 公式级别=" + gsjb;

                    DataTable dtDL = null;
                    dtDL = DBsql.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDL.Rows.Count > 0 && dtDL != null)
                    {
                        for (int i = 0; i < dtDL.Rows.Count; i++)
                        {
                            num = dtDL.Rows[i]["小数点数"].ToString();

                            dlID = dtDL.Rows[i]["电量点"].ToString();
                            //参数ID
                            ParaID = dtDL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                            //drvA = double.Parse(pc.GetPower(drvA, dlID, fDay, lDay, bm).ToString());

                            mon = "";

                            if (dtDL.Rows[i]["公式"].ToString() != "")
                                mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                            else
                                mon = drvA.ToString();

                            res = "";
                            try
                            { res = StrHelper.Cale(mon); }
                            catch
                            { res = "0"; }

                            if (id != 0)
                            {
                                zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                un[id] = dtDL.Rows[i]["单位"].ToString();
                                lay[id] = dtDL.Rows[i]["设计值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + res + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = res + ",";
                                    htUn.Add(ParaID, str);
                                }
                            }
                        }
                    }

                    #endregion

                    #region 其它
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                    DataTable dtQT = null;
                    dtQT = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtQT.Rows.Count > 0 && dtQT != null)
                    {
                        for (int i = 0; i < dtQT.Rows.Count; i++)
                        {
                            num = dtQT.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtQT.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                            #region 购网电量
                            if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                            {
                                //string re = um.gwdl(fDay, lDay, 0);

                                //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                //    dv[id] += sh.ShowPoint(re.ToString(), num) + ",";

                                //    dvExcel[id] += sh.ShowPoint(re.ToString(), "-1") + ",";
                                //}

                                //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                //    du[id] = dtQT.Rows[i]["参数ID"].ToString() + "," + re.ToString() + ";";
                                //}
                            }
                            #endregion

                            #region 入炉煤低位发热量
                            if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                            {
                                //string rlm = um.rlmdwfrl("月报", fDay, lDay, 1);

                                //if (dtQT.Rows[i]["显示"].ToString() == "1")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                //    dv[id] += sh.ShowPoint(rlm, num) + ",";

                                //    dvExcel[id] += sh.ShowPoint(rlm, "-1") + ",";
                                //}

                                //if (dtQT.Rows[i]["参数ID"].ToString() != "")
                                //{
                                //    id = int.Parse(dtQT.Rows[i]["参数key"].ToString());

                                //    //if (num == "")
                                //    du[id] = dtQT.Rows[i]["参数ID"].ToString() + "," + rlm + ";";
                                //    //else
                                //    //    du[id] = dtQT.Rows[i]["参数ID"].ToString() + "," + sh.ShowPoint(rlm, num) + ";";
                                //}
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 公式
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='公式' and 公式级别=" + gsjb;

                    DataTable dtMon = null;
                    dtMon = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtMon != null && dtMon.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtMon.Rows.Count; i++)
                        {
                            num = dtMon.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtMon.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                            string pMon = dtMon.Rows[i]["公式"].ToString();

                            string paras = dtMon.Rows[i]["公式参数"].ToString();

                            string month = pm.retMonByMonthAndYear(htGY, htUn, paras, pMon);

                            if (month != "")
                            {
                                res = "";
                                try
                                { res = StrHelper.Cale(month); }
                                catch
                                { res = "0"; }

                                if (id != 0)
                                {
                                    zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                    un[id] = dtMon.Rows[i]["单位"].ToString();
                                    lay[id] = dtMon.Rows[i]["设计值"].ToString();

                                    dv[id] += sh.ShowPoint(res, num) + ",";
                                    dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                }

                                if (ParaID != "")
                                {
                                    if (htUn.Contains(ParaID))
                                    {
                                        str = htUn[ParaID] + res + ",";
                                        htUn.Remove(ParaID);
                                        htUn.Add(ParaID, str);
                                    }
                                    else
                                    {
                                        str = res + ",";
                                        htUn.Add(ParaID, str);
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }

                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----全厂结束!");
            }

            #endregion

            #region 调试模式
            if (TSMS == "yes")
            {
                ArrayList listGy = new ArrayList(htGY.Keys);
                ArrayList listUn = new ArrayList(htUn.Keys);

                listGy.Sort();
                listUn.Sort();

                foreach (string skey in listGy)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----" + skey + "---" + htGY[skey]); }

                foreach (string skey in listUn)
                { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----" + skey + "---" + htUn[skey]); }
            }
            #endregion

            list.Add(zb);
            list.Add(un);
            list.Add(lay);
            list.Add(dv);
            list.Add(dvExcel);

            return list;
        }

        /// <summary>
        /// 年明细
        /// 机组轮转
        /// </summary>
        /// <param name="repName"></param>
        /// <param name="unit"></param>
        /// <param name="date"></param>
        /// <param name="isDay"></param>
        /// <returns></returns>
        public ArrayList RetArrsRepYearInfo(string repName, int unit, DateTime date, int isDay)
        {
            this.init();

            object obj;

            int id;
            int ret = 0;
            double drvA = 0;
            double drvAT = 0;
            double valGY = 0;

            string bm = "";
            string str = "";
            string mon = "";
            string res = "";
            string monT = "";
            string resT = "";
            string num = "";
            string pName = "";
            string dlID = "";
            string sDate1 = "";
            string sDate1T = "";

            string GYID = "";
            string ParaID = "";
            string yType = "年点";
            string mType = "月点";
            string repType = "年明细";

            string[] zb = null;
            string[] un = null;
            string[] lay = null;
            string[] zy = null;     //最优值描述
            string[] zyB = null;    //最优本期
            string[] zyT = null;    //最优同期

            string[] dv = null;
            string[] dvT = null;
            string[] dvExcel = null;
            string[] dvExcelT = null;


            string[] qsrq = new string[12];
            string[] jsrq = new string[12];
            string[] sDate = new string[12];

            string[] qsrqT = new string[12];
            string[] jsrqT = new string[12];
            string[] sDateT = new string[12];

            DataTable dtGY = null;

            Hashtable htGY = new Hashtable();
            Hashtable htUn = new Hashtable();
            Hashtable htGYT = new Hashtable();
            Hashtable htUnT = new Hashtable();
            ArrayList list = new ArrayList();

            count = pm.GetCount(repName);

            #region 日期

            //本期时间
            if (isDay == 0)
            {
                for (int i = 0; i < DateTime.Now.Month; i++)
                {
                    qsrq[i] = date.AddMonths(+i).ToString("yyyy-MM-dd H:mm:ss");
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
            #endregion

            zb = new string[count + 1];
            un = new string[count + 1];
            lay = new string[count + 1];
            zy = new string[count + 1];     //最优值描述
            zyB = new string[count + 1];    //最优本期
            zyT = new string[count + 1];    //最优同期

            dv = new string[count + 1];
            dvT = new string[count + 1];
            dvExcel = new string[count + 1];
            dvExcelT = new string[count + 1];

            string[] zyDu = new string[count + 1];
            string[] zyDuT = new string[count + 1];

            #region 公用库读取

            sql = "select * from GYReport where 报表名称='全厂级'";

            if (rlDBType == "SQL")
                dtGY = DBsql.RunDataTable(sql, out errMsg);
            else
                dtGY = DBdb2.RunDataTable(sql, out errMsg);

            if (dtGY != null && dtGY.Rows.Count > 0)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转开始!");

                for (int k = 0; k < 12; k++)
                {
                    if (k < qsrq.Length)
                    {
                        if (qsrq[k] != null)
                            sDate1 = DateTime.Parse(jsrq[k]).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                        if (qsrqT[k] != null)
                            sDate1T = DateTime.Parse(jsrqT[k]).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                        for (int i = 0; i < dtGY.Rows.Count; i++)
                        {
                            GYID = dtGY.Rows[i]["参数ID"].ToString();

                            if (dtGY.Rows[i][9].ToString() == "平均" || dtGY.Rows[i][9].ToString() == "累计")
                            {
                                pName = dtGY.Rows[i][mType].ToString();

                                if (qsrq[k] != null)
                                {
                                    //if (rtDBType == "EDNA")
                                    //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                                    //else
                                    //    pk.GetHisValue(pName, sDate1, ref drvA);
                                }
                                else
                                    drvA = 0;

                                if (qsrqT[k] != null)
                                {
                                    //if (rtDBType == "EDNA")
                                    //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                                    //else
                                    //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                                }
                                else
                                    drvAT = 0;

                                if (GYID != "")
                                {
                                    if (htGY.Contains(GYID))
                                    {
                                        str = htGY[GYID] + drvA.ToString() + ",";
                                        htGY.Remove(GYID);
                                        htGY.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = drvA + ",";
                                        htGY.Add(GYID, str);
                                    }

                                    if (htGYT.Contains(GYID))
                                    {
                                        str = htGYT[GYID] + drvAT.ToString() + ",";
                                        htGYT.Remove(GYID);
                                        htGYT.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = drvAT + ",";
                                        htGYT.Add(GYID, str);
                                    }
                                }
                            }
                            else if (dtGY.Rows[i][9].ToString() == "电量")
                            {
                                dlID = dtGY.Rows[i]["电量点"].ToString();

                                //if (qsrq[k] != null)
                                //    //valGY = double.Parse(pc.GetPower(valGY, dlID, qsrq[k], jsrq[k], bm).ToString());
                                //else
                                //    valGY = 0;

                                mon = "";

                                if (dtGY.Rows[i]["公式"].ToString() != "")
                                    mon = valGY.ToString() + dtGY.Rows[i]["公式"].ToString();
                                else
                                    mon = valGY.ToString();

                                res = "";
                                try
                                { res = StrHelper.Cale(mon); }
                                catch
                                { res = "0"; }

                                if (GYID != "")
                                {
                                    if (htGY.Contains(GYID))
                                    {
                                        str = htGY[GYID] + res + ",";
                                        htGY.Remove(GYID);
                                        htGY.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = res + ",";
                                        htGY.Add(GYID, str);
                                    }
                                }

                                valGY = 0;
                                //if (qsrqT[k] != null)
                                //    valGY = double.Parse(pc.GetPower(valGY, dlID, qsrqT[k], jsrqT[k], bm).ToString());
                                //else
                                //    valGY = 0;

                                mon = "";
                                if (dtGY.Rows[i]["公式"].ToString() != "")
                                    mon = valGY.ToString() + dtGY.Rows[i]["公式"].ToString();
                                else
                                    mon = valGY.ToString();

                                res = "";
                                try
                                { res = StrHelper.Cale(mon); }
                                catch
                                { res = "0"; }

                                if (GYID != "")
                                {
                                    if (htGYT.Contains(GYID))
                                    {
                                        str = htGYT[GYID] + res + ",";
                                        htGYT.Remove(GYID);
                                        htGYT.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = res + ",";
                                        htGYT.Add(GYID, str);
                                    }
                                }
                            }
                            else if (dtGY.Rows[i][9].ToString() == "SQL")
                            {
                                sql = dtGY.Rows[i]["SQL语句"].ToString();

                                if (qsrq[k] != null)
                                {
                                    if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                    {
                                        sql = sql.Replace("&bt&", qsrq[k].ToString());
                                        sql = sql.Replace("&et&", sDate1);
                                    }

                                    if (rlDBType == "SQL")
                                        obj = DBsql.RunSingle(sql, out errMsg);
                                    else
                                        obj = DBdb2.RunSingle(sql, out errMsg);

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

                                if (GYID != "")
                                {
                                    if (htGY.Contains(GYID))
                                    {
                                        str = htGY[GYID] + res + ",";
                                        htGY.Remove(GYID);
                                        htGY.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = res + ",";
                                        htGY.Add(GYID, str);
                                    }
                                }

                                sql = dtGY.Rows[i]["SQL语句"].ToString();

                                if (qsrqT[k] != null)
                                {
                                    if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                    {
                                        sql = sql.Replace("&bt&", qsrqT[k].ToString());
                                        sql = sql.Replace("&et&", sDate1T);
                                    }

                                    if (rlDBType == "SQL")
                                        obj = DBsql.RunSingle(sql, out errMsg);
                                    else
                                        obj = DBdb2.RunSingle(sql, out errMsg);

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

                                if (GYID != "")
                                {
                                    if (htGYT.Contains(GYID))
                                    {
                                        str = htGYT[GYID] + res + ",";
                                        htGYT.Remove(GYID);
                                        htGYT.Add(GYID, str);
                                    }
                                    else
                                    {
                                        str = res + ",";
                                        htGYT.Add(GYID, str);
                                    }
                                }
                            }
                        }

                    }
                }
                //公用库年值
                if (isDay == 0)
                    sDate1 = DateTime.Parse(sDate[11]).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");
                else
                    sDate1 = DateTime.Parse(sDate[11]).ToString("yyyy-MM-dd 23:59:59");

                sDate1T = DateTime.Parse(sDateT[11]).ToString("yyyy-MM-dd 23:59:59");

                for (int i = 0; i < dtGY.Rows.Count; i++)
                {
                    GYID = dtGY.Rows[i]["参数ID"].ToString();

                    if (dtGY.Rows[i][9].ToString() == "平均" || dtGY.Rows[i][9].ToString() == "累计")
                    {
                        pName = dtGY.Rows[i][yType].ToString();

                        if (sDate1 != null)
                        {
                            //if (rtDBType == "EDNA")
                            //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                            //else
                            //    pk.GetHisValue(pName, sDate1, ref drvA);
                        }
                        else
                            drvA = 0;

                        if (sDate1T != null)
                        {
                            //if (rtDBType == "EDNA")
                            //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                            //else
                            //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                        }
                        else
                            drvAT = 0;

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                            {
                                str = htGY[GYID] + drvA.ToString() + ",";
                                htGY.Remove(GYID);
                                htGY.Add(GYID, str);
                            }
                            else
                            {
                                str = drvA + ",";
                                htGY.Add(GYID, str);
                            }

                            if (htGYT.Contains(GYID))
                            {
                                str = htGYT[GYID] + drvAT.ToString() + ",";
                                htGYT.Remove(GYID);
                                htGYT.Add(GYID, str);
                            }
                            else
                            {
                                str = drvAT + ",";
                                htGYT.Add(GYID, str);
                            }
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "电量")
                    {
                        dlID = dtGY.Rows[i]["电量点"].ToString();

                        valGY = 0;
                        //valGY = double.Parse(pc.GetPower(valGY, dlID, qsrq[0], DateTime.Parse(sDate[11]).ToString("yyyy-MM-dd H:mm:ss"), bm).ToString());

                        mon = "";

                        if (dtGY.Rows[i]["公式"].ToString() != "")
                            mon = valGY.ToString() + dtGY.Rows[i]["公式"].ToString();
                        else
                            mon = valGY.ToString();

                        res = "";
                        try
                        { res = StrHelper.Cale(mon); }
                        catch
                        { res = "0"; }

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                            {
                                str = htGY[GYID] + res + ",";
                                htGY.Remove(GYID);
                                htGY.Add(GYID, str);
                            }
                            else
                            {
                                str = res + ",";
                                htGY.Add(GYID, str);
                            }
                        }

                        valGY = 0;
                        //valGY = double.Parse(pc.GetPower(valGY, dlID, qsrqT[0], jsrqT[11], bm).ToString());

                        mon = "";
                        if (dtGY.Rows[i]["公式"].ToString() != "")
                            mon = valGY.ToString() + dtGY.Rows[i]["公式"].ToString();
                        else
                            mon = valGY.ToString();

                        res = "";
                        try
                        { res = StrHelper.Cale(mon); }
                        catch
                        { res = "0"; }

                        if (GYID != "")
                        {
                            if (htGYT.Contains(GYID))
                            {
                                str = htGYT[GYID] + res + ",";
                                htGYT.Remove(GYID);
                                htGYT.Add(GYID, str);
                            }
                            else
                            {
                                str = res + ",";
                                htGYT.Add(GYID, str);
                            }
                        }
                    }
                    else if (dtGY.Rows[i][9].ToString() == "SQL")
                    {
                        sql = dtGY.Rows[i]["SQL语句"].ToString();

                        if (sDate1 != null)
                        {
                            if (sql.Contains("&bt&") && sql.Contains("&et&"))
                            {
                                sql = sql.Replace("&bt&", qsrq[0].ToString());
                                sql = sql.Replace("&et&", sDate1);
                            }

                            if (rlDBType == "SQL")
                                obj = DBsql.RunSingle(sql, out errMsg);
                            else
                                obj = DBdb2.RunSingle(sql, out errMsg);

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

                        if (GYID != "")
                        {
                            if (htGY.Contains(GYID))
                            {
                                str = htGY[GYID] + res + ",";
                                htGY.Remove(GYID);
                                htGY.Add(GYID, str);
                            }
                            else
                            {
                                str = res + ",";
                                htGY.Add(GYID, str);
                            }
                        }

                        sql = dtGY.Rows[i]["SQL语句"].ToString();

                        if (sDate1T != null)
                        {
                            if (sql.Contains("&bt&") && sql.Contains("&et&"))
                            {
                                sql = sql.Replace("&bt&", qsrqT[0].ToString());
                                sql = sql.Replace("&et&", sDate1T);
                            }

                            if (rlDBType == "SQL")
                                obj = DBsql.RunSingle(sql, out errMsg);
                            else
                                obj = DBdb2.RunSingle(sql, out errMsg);

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

                        if (GYID != "")
                        {
                            if (htGYT.Contains(GYID))
                            {
                                str = htGYT[GYID] + res + ",";
                                htGYT.Remove(GYID);
                                htGYT.Add(GYID, str);
                            }
                            else
                            {
                                str = res + ",";
                                htGYT.Add(GYID, str);
                            }
                        }
                    }
                }


                LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----公用库轮转结束!");
            }

            #endregion

            #region 机组数据读取

            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转开始!");

            for (int k = 0; k < 12; k++)
            {
                if (k < qsrq.Length)
                {
                    if (qsrq[k] != null)
                        sDate1 = DateTime.Parse(jsrq[k]).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                    if (qsrqT[k] != null)
                        sDate1T = DateTime.Parse(jsrqT[k]).AddSeconds(-1).ToString("yyyy-MM-dd 23:59:59");

                    sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 公式级别!=0";

                    DataTable dtLel = null;

                    if (rlDBType == "SQL")
                        dtLel = DBsql.RunDataTable(sql, out errMsg);
                    else
                        dtLel = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtLel.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtLel.Rows.Count; j++)
                        {
                            string gsjb = dtLel.Rows[j]["公式级别"].ToString();

                            #region 平均
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='平均' and 公式级别=" + gsjb;

                            DataTable dtAvg = null;

                            if (rlDBType == "SQL")
                                dtAvg = DBsql.RunDataTable(sql, out errMsg);
                            else
                                dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtAvg != null && dtAvg.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtAvg.Rows.Count; i++)
                                {
                                    num = dtAvg.Rows[i]["小数点数"].ToString();

                                    pName = dtAvg.Rows[i][mType].ToString();
                                    //参数ID
                                    ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                                    if (qsrq[k] != null)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                                        //else
                                        //    pk.GetHisValue(pName, sDate1, ref drvA);
                                    }
                                    else
                                        drvA = 0;

                                    if (qsrqT[k] != null)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                                        //else
                                        //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                                    }
                                    else
                                        drvAT = 0;

                                    if (id != 0)
                                    {
                                        zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                        un[id] = dtAvg.Rows[i]["单位"].ToString();
                                        lay[id] = dtAvg.Rows[i]["设计值"].ToString();
                                        zy[id] = dtAvg.Rows[i]["最优值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvT[id] += sh.ShowPoint(drvAT.ToString(), num) + ",";

                                        dvExcel[id] += drvA.ToString() + ",";
                                        dvExcelT[id] += drvAT.ToString() + ",";

                                        if (qsrq[k] != null)
                                            zyDu[id] += sh.ShowPoint(drvA.ToString(), num) + ",";

                                        if (qsrqT[k] != null)
                                            zyDuT[id] += sh.ShowPoint(drvAT.ToString(), num) + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + drvA.ToString() + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = drvA.ToString() + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUnT.Contains(ParaID))
                                        {
                                            str = htUnT[ParaID] + drvAT.ToString() + ",";
                                            htUnT.Remove(ParaID);
                                            htUnT.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = drvAT.ToString() + ",";
                                            htUnT.Add(ParaID, str);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 累计
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='累计' and 公式级别=" + gsjb;

                            DataTable dtDiff = null;
                            dtDiff = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDiff != null && dtDiff.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtDiff.Rows.Count; i++)
                                {
                                    num = dtDiff.Rows[i]["小数点数"].ToString();

                                    pName = dtDiff.Rows[i][mType].ToString();
                                    //参数ID
                                    ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                                    drvA = 0;
                                    drvAT = 0;

                                    if (qsrq[k] != null)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                                        //else
                                        //    pk.GetHisValue(pName, sDate1, ref drvA);
                                    }
                                    else
                                        drvA = 0;

                                    if (qsrqT[k] != null)
                                    {
                                        //if (rtDBType == "EDNA")
                                        //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                                        //else
                                        //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                                    }
                                    else
                                        drvAT = 0;

                                    if (id != 0)
                                    {
                                        zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                        un[id] = dtDiff.Rows[i]["单位"].ToString();
                                        lay[id] = dtDiff.Rows[i]["设计值"].ToString();
                                        zy[id] = dtDiff.Rows[i]["最优值"].ToString();

                                        dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                        dvT[id] += sh.ShowPoint(drvAT.ToString(), num) + ",";

                                        dvExcel[id] += drvA.ToString() + ",";
                                        dvExcelT[id] += drvAT.ToString() + ",";

                                        if (qsrq[k] != null)
                                            zyDu[id] += sh.ShowPoint(drvA.ToString(), num) + ",";

                                        if (qsrqT[k] != null)
                                            zyDuT[id] += sh.ShowPoint(drvAT.ToString(), num) + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + drvA.ToString() + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = drvA.ToString() + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUnT.Contains(ParaID))
                                        {
                                            str = htUnT[ParaID] + drvAT.ToString() + ",";
                                            htUnT.Remove(ParaID);
                                            htUnT.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = drvAT.ToString() + ",";
                                            htUnT.Add(ParaID, str);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region SQL

                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='SQL' and 公式级别=" + gsjb;

                            DataTable dtSQL = null;
                            dtSQL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtSQL != null && dtSQL.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtSQL.Rows.Count; i++)
                                {
                                    num = dtSQL.Rows[i]["小数点数"].ToString();

                                    sql = dtSQL.Rows[i]["SQL语句"].ToString();
                                    //参数ID
                                    ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                                    if (qsrq[k] != null)
                                    {
                                        if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                        {
                                            sql = sql.Replace("&bt&", qsrq[k].ToString());
                                            sql = sql.Replace("&et&", sDate1);
                                        }

                                        if (rlDBType == "SQL")
                                            obj = DBsql.RunSingle(sql, out errMsg);
                                        else
                                            obj = DBdb2.RunSingle(sql, out errMsg);

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

                                    sql = dtSQL.Rows[i]["SQL语句"].ToString();

                                    if (qsrqT[k] != null)
                                    {
                                        if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                        {
                                            sql = sql.Replace("&bt&", qsrqT[k].ToString());
                                            sql = sql.Replace("&et&", sDate1T);
                                        }
                                        obj = dl.RunSingle(sql, out errMsg);
                                        //if (rlDBType == "SQL")
                                        //    obj = DBsql.RunSingle(sql, out errMsg);
                                        //else
                                        //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                                    if (id != 0)
                                    {
                                        zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                        un[id] = dtSQL.Rows[i]["单位"].ToString();
                                        lay[id] = dtSQL.Rows[i]["设计值"].ToString();
                                        zy[id] = dtSQL.Rows[i]["最优值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";
                                        dvT[id] += sh.ShowPoint(resT, num) + ",";

                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                        dvExcelT[id] += sh.ShowPoint(resT, "-1") + ",";

                                        if (qsrq[k] != null)
                                            zyDu[id] += sh.ShowPoint(res, num) + ",";

                                        if (qsrqT[k] != null)
                                            zyDuT[id] += sh.ShowPoint(resT, num) + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + res + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = res + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUnT.Contains(ParaID))
                                        {
                                            str = htUnT[ParaID] + resT + ",";
                                            htUnT.Remove(ParaID);
                                            htUnT.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = resT + ",";
                                            htUnT.Add(ParaID, str);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region 电量
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='电量' and 公式级别=" + gsjb;

                            DataTable dtDL = null;
                            dtDL = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtDL.Rows.Count > 0 && dtDL != null)
                            {
                                for (int i = 0; i < dtDL.Rows.Count; i++)
                                {
                                    num = dtDL.Rows[i]["小数点数"].ToString();

                                    dlID = dtDL.Rows[i]["电量点"].ToString();
                                    //参数ID
                                    ParaID = dtDL.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                                    //if (qsrq[k] != null)
                                    //    drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq[k], jsrq[k], bm).ToString());
                                    //else
                                    //    drvA = 0;

                                    mon = "";

                                    if (dtDL.Rows[i]["公式"].ToString() != "")
                                        mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                                    else
                                        mon = drvA.ToString();

                                    res = "";
                                    try
                                    { res = StrHelper.Cale(mon); }
                                    catch
                                    { res = "0"; }

                                    //if (qsrqT[k] != null)
                                    //    drvAT = double.Parse(pc.GetPower(drvAT, dlID, qsrqT[k], jsrqT[k], bm).ToString());
                                    //else
                                    //    drvAT = 0;

                                    mon = "";

                                    if (dtDL.Rows[i]["公式"].ToString() != "")
                                        mon = drvAT.ToString() + dtDL.Rows[i]["公式"].ToString();
                                    else
                                        mon = drvAT.ToString();

                                    resT = "";
                                    try
                                    { resT = StrHelper.Cale(mon); }
                                    catch
                                    { resT = "0"; }

                                    if (id != 0)
                                    {
                                        zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                        un[id] = dtDL.Rows[i]["单位"].ToString();
                                        lay[id] = dtDL.Rows[i]["设计值"].ToString();
                                        zy[id] = dtDL.Rows[i]["最优值"].ToString();

                                        dv[id] += sh.ShowPoint(res, num) + ",";
                                        dvT[id] += sh.ShowPoint(resT, num) + ",";

                                        dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                        dvExcelT[id] += sh.ShowPoint(resT, "-1") + ",";

                                        if (qsrq[k] != null)
                                            zyDu[id] += sh.ShowPoint(res, num) + ",";

                                        if (qsrqT[k] != null)
                                            zyDuT[id] += sh.ShowPoint(resT, num) + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + res + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = res + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUnT.Contains(ParaID))
                                        {
                                            str = htUnT[ParaID] + resT + ",";
                                            htUnT.Remove(ParaID);
                                            htUnT.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = resT + ",";
                                            htUnT.Add(ParaID, str);
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region 其它
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                            DataTable dtQT = null;
                            dtQT = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtQT.Rows.Count > 0 && dtQT != null)
                            {
                                for (int i = 0; i < dtQT.Rows.Count; i++)
                                {
                                    num = dtQT.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtQT.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());

                                    #region 购网电量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "购网电量")
                                    {
                                        //string re = "0";

                                        //if (qsrq[k] != null)
                                        //    re = um.gwdl(qsrq[k], jsrq[k], unit);

                                        //string reT = "0";

                                        //if (qsrqT[k] != null)
                                        //    reT = um.gwdl(qsrqT[k], jsrqT[k], unit);                                      
                                    }
                                    #endregion

                                    #region 入炉煤低位发热量
                                    if (dtQT.Rows[i]["参数名"].ToString() == "入炉煤低位发热量")
                                    {
                                        //string rlm = "";
                                        //string rlmT = "";

                                        //if (qsrq[k] != null)
                                        //    rlm = um.rlmdwfrl("月报", qsrq[k], jsrq[k], 1);
                                        //else
                                        //    rlm = "0";

                                        //if (qsrqT[k] != null)
                                        //    rlmT = um.rlmdwfrl("月报", qsrqT[k], jsrqT[k], 1);
                                        //else
                                        //    rlmT = "0";                                     
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region 公式
                            sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='公式' and 公式级别=" + gsjb;

                            DataTable dtMon = null;
                            dtMon = dl.RunDataTable(sql, out errMsg);
                            //if (rlDBType == "SQL")
                            //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                            //else
                            //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                            if (dtMon != null && dtMon.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtMon.Rows.Count; i++)
                                {
                                    num = dtMon.Rows[i]["小数点数"].ToString();
                                    //参数ID
                                    ParaID = dtMon.Rows[i]["参数ID"].ToString();
                                    //参数顺序
                                    id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                                    string pMon = dtMon.Rows[i]["公式"].ToString();

                                    string paras = dtMon.Rows[i]["公式参数"].ToString();

                                    string month = pm.retMonByMonthAndYear(htGY, htUn, paras, pMon);
                                    string monthT = pm.retMonByMonthAndYear(htGYT, htUnT, paras, pMon);

                                    //本期
                                    if (month != "")
                                    {
                                        res = "";
                                        try
                                        { res = StrHelper.Cale(month); }
                                        catch
                                        { res = "0"; }
                                    }

                                    //同期
                                    if (monthT != "")
                                    {
                                        resT = "";
                                        try
                                        { resT = StrHelper.Cale(monthT); }
                                        catch
                                        { resT = "0"; }
                                    }

                                    if (id != 0)
                                    {
                                        zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                        un[id] = dtMon.Rows[i]["单位"].ToString();
                                        lay[id] = dtMon.Rows[i]["设计值"].ToString();
                                        zy[id] = dtMon.Rows[i]["最优值"].ToString();

                                        if (qsrq[k] != null)
                                        {
                                            dv[id] += sh.ShowPoint(res, num) + ",";
                                            dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                        }
                                        else
                                        {
                                            dv[id] += sh.ShowPoint("0", num) + ",";
                                            dvExcel[id] += "0" + ",";
                                        }

                                        if (qsrqT[k] != null)
                                        {
                                            dvT[id] += sh.ShowPoint(resT, num) + ",";
                                            dvExcelT[id] += sh.ShowPoint(resT, "-1") + ",";
                                        }
                                        else
                                        {
                                            dvT[id] += sh.ShowPoint("0", num) + ",";
                                            dvExcelT[id] += "0" + ",";
                                        }

                                        if (qsrq[k] != null)
                                            zyDu[id] += sh.ShowPoint(res, num) + ",";

                                        if (qsrqT[k] != null)
                                            zyDuT[id] += sh.ShowPoint(resT, num) + ",";
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUn.Contains(ParaID))
                                        {
                                            str = htUn[ParaID] + res + ",";
                                            htUn.Remove(ParaID);
                                            htUn.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = res + ",";
                                            htUn.Add(ParaID, str);
                                        }
                                    }

                                    if (ParaID != "")
                                    {
                                        if (htUnT.Contains(ParaID))
                                        {
                                            str = htUnT[ParaID] + resT + ",";
                                            htUnT.Remove(ParaID);
                                            htUnT.Add(ParaID, str);
                                        }
                                        else
                                        {
                                            str = resT + ",";
                                            htUnT.Add(ParaID, str);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
            }

            #region 最优值
            if (zy != null)
            {
                string strB = "";
                string strT = "";

                for (int i = 0; i < zy.Length; i++)
                {
                    if (zy[i] != null && zy[i] != "")
                    {
                        strB = pm.GetMaxOrMixStr(zyDu[i], zy[i]);
                        strT = pm.GetMaxOrMixStr(zyDuT[i], zy[i]);

                        if (strB != "")
                            zyB[i] = strB;

                        if (strT != "")
                            zyT[i] = strT;
                    }
                    else
                    {
                        zyB[i] = "/";
                        zyT[i] = "/";
                    }
                }
            }
            #endregion

            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "---" + repName + "----" + repType + "----机组轮转结束!");

            #endregion

            #region 全厂数据读取

            sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 公式级别!=0";

            DataTable dtLelYear = null;
             dtLelYear = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    dtLelYear = DBsql.RunDataTable(sql, out errMsg);
            //else
            //    dtLelYear = DBdb2.RunDataTable(sql, out errMsg);

            if (dtLelYear.Rows.Count > 0)
            {
                if (isDay == 0)
                    sDate1 = DateTime.Parse(sDate[11]).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss"); // DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd H:59:59");
                else
                    sDate1 = DateTime.Parse(sDate[11]).ToString("yyyy-MM-dd 23:59:59");

                sDate1T = DateTime.Parse(sDateT[11]).ToString("yyyy-MM-dd 23:59:59");

                for (int j = 0; j < dtLelYear.Rows.Count; j++)
                {
                    string gsjb = dtLelYear.Rows[j]["公式级别"].ToString();

                    #region 平均
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='平均' and 公式级别=" + gsjb;

                    DataTable dtAvg = null;
                    dtAvg = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtAvg = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtAvg = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtAvg != null && dtAvg.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAvg.Rows.Count; i++)
                        {
                            num = dtAvg.Rows[i]["小数点数"].ToString();

                            pName = dtAvg.Rows[i][yType].ToString();
                            //参数ID
                            ParaID = dtAvg.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtAvg.Rows[i]["参数顺序"].ToString());

                            if (sDate1 != "")
                            {
                                //if (rtDBType == "EDNA")
                                //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                                //else
                                //    pk.GetHisValue(pName, sDate1, ref drvA);
                            }
                            else
                                drvA = 0;

                            if (sDate1T != "")
                            {
                            //if (rtDBType == "EDNA")
                            //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                            //else
                            //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                            }
                            else
                                drvAT = 0;

                            if (id != 0)
                            {
                                zb[id] = dtAvg.Rows[i]["参数名"].ToString();
                                un[id] = dtAvg.Rows[i]["单位"].ToString();
                                lay[id] = dtAvg.Rows[i]["设计值"].ToString();
                                zy[id] = dtAvg.Rows[i]["最优值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvT[id] += sh.ShowPoint(drvAT.ToString(), num) + ",";

                                dvExcel[id] += drvA.ToString() + ",";
                                dvExcelT[id] += drvAT.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + drvA.ToString() + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = drvA.ToString() + ",";
                                    htUn.Add(ParaID, str);
                                }

                                if (htUnT.Contains(ParaID))
                                {
                                    str = htUnT[ParaID] + drvAT.ToString() + ",";
                                    htUnT.Remove(ParaID);
                                    htUnT.Add(ParaID, str);
                                }
                                else
                                {
                                    str = drvAT.ToString() + ",";
                                    htUnT.Add(ParaID, str);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 累计
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='累计' and 公式级别=" + gsjb;

                    DataTable dtDiff = null;
                    dtDiff = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDiff = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDiff = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDiff != null && dtDiff.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDiff.Rows.Count; i++)
                        {
                            num = dtDiff.Rows[i]["小数点数"].ToString();

                            pName = dtDiff.Rows[i][yType].ToString();
                            //参数ID
                            ParaID = dtDiff.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDiff.Rows[i]["参数顺序"].ToString());

                            drvA = 0;
                            drvAT = 0;

                            if (sDate1 != "")
                            {
                                //if (rtDBType == "EDNA")
                                //    ek.GetHisValueByTime(pName, sDate1, ref ret, ref drvA);
                                //else
                                //    pk.GetHisValue(pName, sDate1, ref drvA);
                            }
                            else
                                drvA = 0;

                            if (sDate1T != "")
                            {
                                //if (rtDBType == "EDNA")
                                //    ek.GetHisValueByTime(pName, sDate1T, ref ret, ref drvAT);
                                //else
                                //    pk.GetHisValue(pName, sDate1T, ref drvAT);
                            }
                            else
                                drvAT = 0;

                            if (id != 0)
                            {
                                zb[id] = dtDiff.Rows[i]["参数名"].ToString();
                                un[id] = dtDiff.Rows[i]["单位"].ToString();
                                lay[id] = dtDiff.Rows[i]["设计值"].ToString();
                                zy[id] = dtDiff.Rows[i]["最优值"].ToString();

                                dv[id] += sh.ShowPoint(drvA.ToString(), num) + ",";
                                dvT[id] += sh.ShowPoint(drvAT.ToString(), num) + ",";

                                dvExcel[id] += drvA.ToString() + ",";
                                dvExcelT[id] += drvAT.ToString() + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + drvA.ToString() + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = drvA.ToString() + ",";
                                    htUn.Add(ParaID, str);
                                }

                                if (htUnT.Contains(ParaID))
                                {
                                    str = htUnT[ParaID] + drvAT.ToString() + ",";
                                    htUnT.Remove(ParaID);
                                    htUnT.Add(ParaID, str);
                                }
                                else
                                {
                                    str = drvAT.ToString() + ",";
                                    htUnT.Add(ParaID, str);
                                }
                            }
                        }
                    }

                    #endregion

                    #region SQL

                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='SQL' and 公式级别=" + gsjb;

                    DataTable dtSQL = null;
                     dtSQL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtSQL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtSQL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtSQL != null && dtSQL.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSQL.Rows.Count; i++)
                        {
                            num = dtSQL.Rows[i]["小数点数"].ToString();

                            sql = dtSQL.Rows[i]["SQL语句"].ToString();

                            //参数ID
                            ParaID = dtSQL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtSQL.Rows[i]["参数顺序"].ToString());

                            if (sDate1 != "")
                            {
                                if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                {
                                    sql = sql.Replace("&bt&", qsrq[0].ToString());
                                    sql = sql.Replace("&et&", sDate1);
                                }

                                if (rlDBType == "SQL")
                                    obj = DBsql.RunSingle(sql, out errMsg);
                                else
                                    obj = DBdb2.RunSingle(sql, out errMsg);

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

                            sql = dtSQL.Rows[i]["SQL语句"].ToString();

                            if (sDate1T != "")
                            {
                                if (sql.Contains("&bt&") && sql.Contains("&et&"))
                                {
                                    sql = sql.Replace("&bt&", qsrqT[0].ToString());
                                    sql = sql.Replace("&et&", sDate1T);
                                }
                                obj = dl.RunSingle(sql, out errMsg);
                                //if (rlDBType == "SQL")
                                //    obj = DBsql.RunSingle(sql, out errMsg);
                                //else
                                //    obj = DBdb2.RunSingle(sql, out errMsg);

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

                            if (id != 0)
                            {
                                zb[id] = dtSQL.Rows[i]["参数名"].ToString();
                                un[id] = dtSQL.Rows[i]["单位"].ToString();
                                lay[id] = dtSQL.Rows[i]["设计值"].ToString();
                                zy[id] = dtSQL.Rows[i]["最优值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvT[id] += sh.ShowPoint(resT, num) + ",";

                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                dvExcelT[id] += sh.ShowPoint(resT, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + res.ToString() + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = res.ToString() + ",";
                                    htUn.Add(ParaID, str);
                                }

                                if (htUnT.Contains(ParaID))
                                {
                                    str = htUnT[ParaID] + resT.ToString() + ",";
                                    htUnT.Remove(ParaID);
                                    htUnT.Add(ParaID, str);
                                }
                                else
                                {
                                    str = resT.ToString() + ",";
                                    htUnT.Add(ParaID, str);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 电量
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='电量' and 公式级别=" + gsjb;

                    DataTable dtDL = null;
                    dtDL = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtDL = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtDL = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtDL.Rows.Count > 0 && dtDL != null)
                    {
                        for (int i = 0; i < dtDL.Rows.Count; i++)
                        {
                            num = dtDL.Rows[i]["小数点数"].ToString();

                            dlID = dtDL.Rows[i]["电量点"].ToString();
                            //参数ID
                            ParaID = dtDL.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtDL.Rows[i]["参数顺序"].ToString());

                            //drvA = double.Parse(pc.GetPower(drvA, dlID, qsrq[0], DateTime.Parse(sDate[11]).ToString("yyyy-MM-dd H:mm:ss"), bm).ToString());

                            mon = "";

                            if (dtDL.Rows[i]["公式"].ToString() != "")
                                mon = drvA.ToString() + dtDL.Rows[i]["公式"].ToString();
                            else
                                mon = drvA.ToString();

                            res = "";
                            try
                            { res = StrHelper.Cale(mon); }
                            catch
                            { res = "0"; }

                            //drvAT = double.Parse(pc.GetPower(drvAT, dlID, qsrqT[0], jsrqT[11], bm).ToString());

                            mon = "";

                            if (dtDL.Rows[i]["公式"].ToString() != "")
                                mon = drvAT.ToString() + dtDL.Rows[i]["公式"].ToString();
                            else
                                mon = drvAT.ToString();

                            resT = "";
                            try
                            { resT = StrHelper.Cale(mon); }
                            catch
                            { resT = "0"; }

                            if (id != 0)
                            {
                                zb[id] = dtDL.Rows[i]["参数名"].ToString();
                                un[id] = dtDL.Rows[i]["单位"].ToString();
                                lay[id] = dtDL.Rows[i]["设计值"].ToString();
                                zy[id] = dtDL.Rows[i]["最优值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvT[id] += sh.ShowPoint(resT, num) + ",";

                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                dvExcelT[id] += sh.ShowPoint(resT, "-1") + ",";
                            }

                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + res.ToString() + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = res.ToString() + ",";
                                    htUn.Add(ParaID, str);
                                }

                                if (htUnT.Contains(ParaID))
                                {
                                    str = htUnT[ParaID] + resT.ToString() + ",";
                                    htUnT.Remove(ParaID);
                                    htUnT.Add(ParaID, str);
                                }
                                else
                                {
                                    str = resT.ToString() + ",";
                                    htUnT.Add(ParaID, str);
                                }
                            }
                        }
                    }

                    #endregion

                    #region 其它
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='其它' and 公式级别=" + gsjb;

                    DataTable dtQT = null;
                    dtQT = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtQT = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtQT = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtQT.Rows.Count > 0 && dtQT != null)
                    {
                        for (int i = 0; i < dtQT.Rows.Count; i++)
                        {
                            num = dtQT.Rows[i]["小数点数"].ToString();
                            //参数ID
                            ParaID = dtQT.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtQT.Rows[i]["参数顺序"].ToString());


                        }
                    }
                    #endregion

                    #region 公式
                    sql = "select * from T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数类型='公式' and 公式级别=" + gsjb;

                    DataTable dtMon = null;
                    dtMon = dl.RunDataTable(sql, out errMsg);
                    //if (rlDBType == "SQL")
                    //    dtMon = DBsql.RunDataTable(sql, out errMsg);
                    //else
                    //    dtMon = DBdb2.RunDataTable(sql, out errMsg);

                    if (dtMon != null && dtMon.Rows.Count > 0)
                    {
                        string pMon = "";
                        string paras = "";
                        string month = "";
                        string monthT = "";

                        for (int i = 0; i < dtMon.Rows.Count; i++)
                        {
                            num = dtMon.Rows[i]["小数点数"].ToString();

                            //参数ID
                            ParaID = dtMon.Rows[i]["参数ID"].ToString();
                            //参数顺序
                            id = int.Parse(dtMon.Rows[i]["参数顺序"].ToString());

                            pMon = dtMon.Rows[i]["公式"].ToString();

                            paras = dtMon.Rows[i]["公式参数"].ToString();

                            month = pm.retMonByMonthAndYear(htGY, htUn, paras, pMon);
                            monthT = pm.retMonByMonthAndYear(htGYT, htUnT, paras, pMon);
                            //本期
                            if (month != "")
                            {
                                res = "";
                                try
                                { res = StrHelper.Cale(month); }
                                catch
                                { res = "0"; }
                            }

                            //同期
                            if (monthT != "")
                            {
                                resT = "";
                                try
                                { resT = StrHelper.Cale(monthT); }
                                catch
                                { resT = "0"; }
                            }

                            if (id != 0)
                            {
                                zb[id] = dtMon.Rows[i]["参数名"].ToString();
                                un[id] = dtMon.Rows[i]["单位"].ToString();
                                lay[id] = dtMon.Rows[i]["设计值"].ToString();
                                zy[id] = dtMon.Rows[i]["最优值"].ToString();

                                dv[id] += sh.ShowPoint(res, num) + ",";
                                dvT[id] += sh.ShowPoint(resT, num) + ",";

                                dvExcel[id] += sh.ShowPoint(res, "-1") + ",";
                                dvExcelT[id] += sh.ShowPoint(resT, "-1") + ",";
                            }
                            if (ParaID != "")
                            {
                                if (htUn.Contains(ParaID))
                                {
                                    str = htUn[ParaID] + res.ToString() + ",";
                                    htUn.Remove(ParaID);
                                    htUn.Add(ParaID, str);
                                }
                                else
                                {
                                    str = res.ToString() + ",";
                                    htUn.Add(ParaID, str);
                                }

                                if (htUnT.Contains(ParaID))
                                {
                                    str = htUnT[ParaID] + resT.ToString() + ",";
                                    htUnT.Remove(ParaID);
                                    htUnT.Add(ParaID, str);
                                }
                                else
                                {
                                    str = resT.ToString() + ",";
                                    htUnT.Add(ParaID, str);
                                }
                            }
                        }
                    }

                    #endregion
                }
            }
            #endregion

            list.Add(zb);
            list.Add(un);
            list.Add(lay);
            list.Add(dv);
            list.Add(dvT);
            list.Add(zyB);
            list.Add(zyT);
            list.Add(dvExcel);
            list.Add(dvExcelT);

            return list;
        }

        #region

        /// <summary>
        /// 年明细报表
        /// 机组轮转
        /// </summary>
        /// <param name="repName"></param>
        /// <param name="unit"></param>
        /// <param name="date"></param>
        /// <param name="isDay"></param>
        /// <returns></returns>
        public ArrayList RetArrsRepYearInfo111(string repName, int unit, DateTime date, int isDay)
        {
            int id;
            object obj;
            int ret = 0;
            double drvA = 0;
            double drvAT = 0;

            string bm = "";
            string mon = "";
            string res = "";
            string resT = "";
            string num = "";
            string pName = "";
            string dlID = "";
            string sDate1 = "";
            string sDate1T = "";

            string[] zb = null;
            string[] un = null;
            string[] lay = null;
            string[] zy = null;     //最优值描述
            string[] zyB = null;    //最优本期
            string[] zyT = null;    //最优同期

            string[] dv = null;
            string[] du = null;
            string[] dvT = null;
            string[] duT = null;

            string[] dvExcel = null;
            string[] dvExcelT = null;

            string[] qsrq = new string[12];
            string[] jsrq = new string[12];
            string[] sDate = new string[12];

            string[] qsrqT = new string[12];
            string[] jsrqT = new string[12];
            string[] sDateT = new string[12];

            ArrayList list = new ArrayList();

            count = pm.GetCountCyd(repName, 2);
            countKey = pm.GetCountkey(repName, 2);

            this.init();

            #region 日期
            //本期时间
            if (isDay == 0)
            {
                for (int i = 0; i < DateTime.Now.Month; i++)
                {
                    qsrq[i] = date.AddMonths(+i).ToString("yyyy-MM-dd H:mm:ss");
                    jsrq[i] = date.AddMonths(+i).AddMonths(+1).ToString("yyyy-MM-dd H:mm:ss");
                    sDate[i] = date.AddMonths(+i).AddMonths(+1).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");

                    if (DateTime.Parse(qsrq[i]).Month == DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 0:00:00")).Month)
                    {
                        jsrq[i] = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");//DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd")
                        sDate[i] = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 0:00:00")).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss");
                    }

                    sDate[11] = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");

                }
            }
            else
            {
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
            #endregion

            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 年明细报开始.......");

            LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " count Value=" + count + " countKey Value=" + countKey);

            try
            {
                zb = new string[count + 1];
                un = new string[count + 1];
                lay = new string[count + 1];
                zy = new string[count + 1];     //最优值描述
                zyB = new string[count + 1];    //最优本期
                zyT = new string[count + 1];    //最优同期

                dv = new string[count + 1];
                du = new string[countKey + 1];
                dvT = new string[count + 1];
                duT = new string[countKey + 1];

                dvExcel = new string[count + 1];
                dvExcelT = new string[count + 1];

                string[] zyDu = new string[count + 1];
                string[] zyDuT = new string[count + 1];



                #region 年值计算

                sql = "SELECT DISTINCT 公式级别 FROM T_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 公式级别!=0";

                DataTable dtLelYear = null;

                if (rlDBType == "SQL")
                    dtLelYear = DBsql.RunDataTable(sql, out errMsg);
                else
                    dtLelYear = DBdb2.RunDataTable(sql, out errMsg);

                if (dtLelYear.Rows.Count > 0)
                {
                    if (isDay == 0)
                        sDate1 = DateTime.Parse(sDate[11]).AddSeconds(-1).ToString("yyyy-MM-dd H:mm:ss"); // DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd H:59:59");
                    else
                        sDate1 = DateTime.Parse(sDate[11]).ToString("yyyy-MM-dd 23:59:59");

                    sDate1T = DateTime.Parse(sDateT[11]).ToString("yyyy-MM-dd 23:59:59");

                    for (int j = 0; j < dtLelYear.Rows.Count; j++)
                    {
                        string gsjb = dtLelYear.Rows[j]["公式级别"].ToString();


                    }
                }
                else
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 年明细报表,年值部分,公式级别读取失败......");

                #endregion
            }
            catch (Exception ce)
            { LogHelper.WriteLog(LogHelper.EnLogType.Run, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + " 程序异常: " + ce.Message); }

            list.Add(zb);
            list.Add(un);
            list.Add(lay);
            list.Add(dv);
            list.Add(dvT);
            list.Add(zyB);
            list.Add(zyT);
            list.Add(dvExcel);
            list.Add(dvExcelT);

            return list;
        }

        #endregion

        public DataTable RetTableXZ(string unit, string repName)
        {
            string sql = "";
            DataTable dt = null;

           // this.init();
            sql = "select 参数名,参数顺序,参数类型 from t_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数顺序!=0";
            dt = dl.RunDataTable(sql, out errMsg);
            //if (rlDBType == "SQL")
            //{
            //    sql = "select 参数名,参数顺序,参数类型 from t_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数顺序!=0";
            //    dt = DBsql.RunDataTable(sql, out errMsg);
            //}
            //else if (rlDBType == "DB2")
            //{
            //    sql = "select 参数名,参数顺序,参数类型 from t_sheet_sheetPara where 报表名称='" + repName + "' and 机组=" + unit + " and 参数顺序!=0";
            //    dt = DBdb2.RunDataTable(sql, out errMsg);
            //}

            return dt;
        }
    }
}
