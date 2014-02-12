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
    public class DALPubMehtod
    {
        string sql = "";
        string errMsg = "";
        string rtDBType = "";   //实时数据库
        string rlDBType = "";   //关系数据库

        StrHelper sh = new StrHelper();
        DBLink dl = new DBLink();
        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <returns></returns>
        private void init()
        {
            rtDBType = IniHelper.ReadIniData("RTDB", "DBType", null);
            rlDBType = IniHelper.ReadIniData("RelationDBbase", "DBType", null);
        }

        public int GetCountByChart(string trendid)
        {
            int count = 0;

            string sql = "";
            //this.init();
            sql = "SELECT count(*) FROM T_CHART_CHARTPARA  WHERE CHARTID = " + trendid;
            count = dl.RunRowCount(sql, out errMsg);

            return count;
        }


        /// <summary>
        /// 获得班值信息
        /// </summary>
        /// <returns></returns>
        public string[] DutyValue(string date)
        {
            object obj = null;

            string[] args = new string[3];
            string[] sqls = new string[3];

            this.init();

            sqls[0] = "SELECT 班组编号 FROM 排班表 WHERE (结束时间 = '" + date + " 7:59:59')";

            sqls[1] = "SELECT 班组编号 FROM 排班表 WHERE (结束时间 = '" + date + " 15:59:59')";

            sqls[2] = "SELECT 班组编号 FROM 排班表 WHERE (结束时间 = '" + date + " 23:59:59')";

            for (int i = 0; i < 3; i++)
            {
                obj = dl.RunSingle(sqls[i], out errMsg);
                //if (rlDBType == "SQL")
                //    obj = dl.RunSingle(sqls[i], out errMsg);
                //else if (rlDBType == "DB2")
                //    obj = dl.RunSingle(sqls[i], out errMsg);

                if (obj != null)
                {
                    args[i] = obj.ToString();
                }
            }

            return args;
        }

        /// <summary>
        /// 数据条数
        /// </summary>
        /// <returns></returns>
        public int GetCount(string repName, int unit)
        {
            int count = 0;

            string sql = "";
            this.init();
            sql = "SELECT count(*) FROM T_Sheet_SheetPara  WHERE (报表名称 = '" + repName + "') AND (机组 = " + unit + ") and 显示=1";
            count = dl.RunRowCount(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    count = DBsql.RunRowCount(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    count = DBdb2.RunRowCount(sql, out errMsg);

            return count;
        }

        public int GetCountCyd(string repName, int unit)
        {
            int count = 0;

            string sql = "";

            this.init();

            sql = "SELECT count(*) FROM T_Sheet_SheetPara  WHERE (报表名称 = '" + repName + "') AND (机组 = " + unit + ") and 显示=1";//
            count = dl.RunRowCount(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    count = DBsql.RunRowCount(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    count = DBdb2.RunRowCount(sql, out errMsg);

            return count;
        }

        public int GetCountkey(string repName, int unit)
        {
            int count = 0;
            string sql = "";
            object obj = null;
            this.init();

            if (rlDBType == "SQL")
            {
                sql = "select top 1 参数key from T_sheet_sheetPara WHERE (报表名称 = '" + repName + "') and  参数key !=0 order by 参数key desc";
                obj = dl.RunSingle(sql, out errMsg);
            }
            else if (rlDBType == "DB2")
            {
                sql = "Select 参数key FROM T_sheet_sheetPara WHERE (报表名称 = '" + repName + "') and  参数key !=0 order BY 参数key DESC FETCH FIRST 1 ROWS ONLY";
                obj = dl.RunSingle(sql, out errMsg);
            }

            if (obj.ToString() != "")
                count = int.Parse(obj.ToString());

            return count;
        }

        public int GetCountData(string repName)
        {
            int count = 0;

            string sql = "";
            this.init();
            sql = "select count(*) from T_Sheet_SheetPara where 报表名称='" + repName + "' and  机组=1 and (参数类型='平均' or 参数类型='累计')";
            count = dl.RunRowCount(sql, out errMsg);
            //if (rlDBType == "SQL")
            //    count = DBsql.RunRowCount(sql, out errMsg);
            //else if (rlDBType == "DB2")
            //    count = DBdb2.RunRowCount(sql, out errMsg);

            return count;
        }


        /// <summary>
        /// 获取某数组最大值或最小值
        /// </summary>
        /// <param name="arr">数组</param>
        /// <param name="type">值类型(大,小)</param>
        /// <returns></returns>
        public string GetMaxOrMixStr(string[] arr, string type)
        {
            string[] str;

            if (arr != null)
                str = sh.arraySort(arr);
            else
                return "";

            if (type == "大")
                return arr[0];
            else if (type == "小")
                return arr[arr.Length - 1];
            else
                return "/";
        }

        public string GetMaxOrMixStr(string[,] arr, string type)
        {
            return null;
        }

        //机组轮转
        public string GetMaxOrMixStr(string str, string type)
        {
            if (str != "" && str != null)
            {
                string[] arr;

                str = str.TrimEnd(',');

                if (str.Contains(','))
                    arr = str.Split(',');
                else
                {
                    arr = new string[1];
                    arr[0] = str;
                }

                if (arr != null)
                    arr = sh.arraySort(arr);
                else
                    return "";

                if (type == "大")
                    return arr[0];
                else if (type == "小")
                    return arr[arr.Length - 1];
                else
                    return "/";
            }
            else
                return "&nbsp;";


        }

        /// <summary>
        /// 拆解公式
        /// </summary>
        /// <param name="month">公式</param>
        /// <param name="paras">公式参数</param>
        /// <param name="paraID">ID</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public string retMon(string month, string paras, string[] paraID, string[] value)
        {
            if (paras != "")
            {
                string[] para = null;

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    for (int j = 0; j < paraID.Length; j++)
                    {
                        if (paraID[j] == para[i])
                        {
                            month = month.Replace(para[i], value[j]);
                        }
                    }
                }
            }

            return month;
        }

        /// <summary>
        /// 拆解全厂公式
        /// </summary>
        /// <param name="month">公式</param>
        /// <param name="paras">公式参数</param>
        /// <param name="paraID">ID</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public string retMon(string month, string paras, string[] paraID, string[] value, string[] pabID, string[] pabVal)
        {
            if (paras != "")
            {
                string[] para = null;

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }
                for (int i = 0; i < para.Length; i++)
                {
                    for (int j = 0; j < paraID.Length; j++)
                    {
                        if (paraID[j] != null)
                        {
                            if (paraID[j] == para[i])
                            {
                                month = month.Replace(para[i], value[j]);
                            }
                        }
                    }
                }

                for (int i = 0; i < para.Length; i++)
                {
                    for (int j = 0; j < pabID.Length; j++)
                    {
                        if (pabID[j] != null)
                        {
                            if (pabID[j] == para[i])
                            {
                                month = month.Replace(para[i], pabVal[j]);
                            }
                        }
                    }

                }
            }

            return month;
        }

        ///*******************///

        public string retMon(string mon, string paras, string[] du, string unit)
        {
            string pID = "";  //参数ID
            string pVal = ""; //参数值

            string[] idVal = null;
            string[] idVal2 = null;

            if (paras != "")
            {
                string[] para = null; //公式参数数组

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    for (int j = 0; j < du.Length; j++)
                    {
                        if (du[j] != null)
                        {
                            if (du[j].Contains(';'))
                            {
                                int a = int.Parse(unit) - 1;

                                idVal = du[j].Split(';');

                                if (idVal[a].Contains(','))
                                {
                                    idVal2 = idVal[a].Split(',');

                                    pID = idVal2[0].ToString();
                                    pVal = idVal2[1].ToString();

                                    if (pID == para[i])
                                        mon = mon.Replace(para[i], pVal);
                                }
                            }
                        }
                    }
                }
            }
            return mon;
        }

        public string retMon(string mon, string paras, string[] du)
        {
            string pID = "";  //参数ID
            string pVal = ""; //参数值

            string[] idVal = null;
            string[] idVal2 = null;

            if (paras != "")
            {
                string[] para = null; //公式参数数组

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    for (int j = 0; j < du.Length; j++)
                    {
                        if (du[j] != null)
                        {
                            if (du[j].Contains(';'))
                            {
                                idVal = du[j].Split(';');

                                for (int k = 0; k < idVal.Length; k++)
                                {
                                    if (idVal[k] != "" && idVal[k].Contains(','))
                                    {
                                        idVal2 = idVal[k].Split(',');

                                        pID = idVal2[0].ToString();
                                        pVal = idVal2[1].ToString();

                                        if (pID == para[i])
                                            mon = mon.Replace(para[i], pVal);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return mon;
        }

        //************************************//
        /// <summary>
        /// 解析公式
        /// </summary>
        /// <param name="htGY"></param>
        /// <param name="htUn"></param>
        /// <param name="paras"></param>
        /// <param name="mon"></param>
        /// <returns></returns>
        public string retMon(Hashtable htGY, Hashtable htUn, string paras, string mon)
        {
            if (paras != "")
            {
                string[] para = null; //公式参数数组

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    if (htGY.Count > 0)
                    {
                        if (htGY.ContainsKey(para[i]))
                            mon = mon.Replace(para[i].ToString(), htGY[para[i]].ToString());
                    }

                    if (htUn.Count > 0)
                    {
                        if (htUn.ContainsKey(para[i]))
                            mon = mon.Replace(para[i].ToString(), htUn[para[i]].ToString());
                    }
                }
            }

            return mon;
        }

        /// <summary>
        /// 月明细 年明细 解析公式
        /// </summary>
        /// <param name="htGY"></param>
        /// <param name="htUn"></param>
        /// <param name="paras"></param>
        /// <param name="mon"></param>
        /// <returns></returns>
        public string retMonByMonthAndYear(Hashtable htGY, Hashtable htUn, string paras, string mon)
        {
            string str = "";
            string[] arr = null;

            if (paras != "")
            {
                string[] para = null; //公式参数数组

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    if (htGY.Count > 0)
                    {
                        if (htGY.ContainsKey(para[i]))
                        {
                            str = htGY[para[i].ToString()].ToString().TrimEnd(',');
                            arr = str.Split(',');
                            mon = mon.Replace(para[i].ToString(), arr[arr.Length - 1].ToString());
                        }

                    }

                    if (htUn.Count > 0)
                    {
                        if (htUn.ContainsKey(para[i]))
                        {
                            str = htUn[para[i].ToString()].ToString().TrimEnd(',');
                            arr = str.Split(',');
                            mon = mon.Replace(para[i].ToString(), arr[arr.Length - 1].ToString());
                        }
                    }
                }
            }

            return mon;
        }

        /// <summary>
        /// 值报解析公式
        /// </summary>
        /// <param name="htGY"></param>
        /// <param name="htUn"></param>
        /// <param name="paras"></param>
        /// <param name="mon"></param>
        /// <returns></returns>
        public string retMonByZRep(string BZ, Hashtable htGY, Hashtable htUn, string paras, string mon)
        {
            string str = "";
            string[] arr = null;
            string[] arrPara = null;
            string[] value = null;

            if (paras != "")
            {
                string[] para = null; //公式参数数组

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    if (htGY.Count > 0)
                    {
                        if (htGY.ContainsKey(para[i]))
                        {
                            str = htGY[para[i].ToString()].ToString();
                            arrPara = str.Split(',');

                            if (arrPara != null && arrPara.Length > 0)
                            {
                                for (int j = 0; j < arrPara.Length; j++)
                                {
                                    value = arrPara[j].Split('+');

                                    if (value[0].ToString() == BZ)
                                        mon = mon.Replace(para[i].ToString(), value[1].ToString());
                                }
                            }
                        }
                    }

                    if (htUn.Count > 0)
                    {
                        if (htUn.ContainsKey(para[i]))
                        {
                            str = htUn[para[i].ToString()].ToString();
                            arrPara = str.Split(',');

                            if (arrPara != null && arrPara.Length > 0)
                            {
                                for (int j = 0; j < arrPara.Length; j++)
                                {
                                    value = arrPara[j].Split('+');

                                    if (value[0].ToString() == BZ)
                                        mon = mon.Replace(para[i].ToString(), value[1].ToString());
                                }
                            }
                        }
                    }
                }
            }

            return mon;
        }

        /// <summary>
        /// 获取显示数组最大KEY
        /// </summary>
        /// <param name="repName"></param>
        /// <returns></returns>
        public int GetCount(string repName)
        {
            int count = 0;
            string sql = "";
            object obj = null;

            this.init();

            if (rlDBType == "SQL")
            {
                sql = "select top 1 参数顺序 from T_sheet_sheetPara where 报表名称='" + repName + "'  and 参数顺序 !=0 order by 参数顺序 desc";
                obj = dl.RunSingle(sql, out errMsg);
            }
            else if (rlDBType == "DB2")
            {
                sql = "Select 参数顺序 FROM T_sheet_sheetPara WHERE (报表名称 = '" + repName + "') and  参数顺序 !=0 order BY 参数顺序 DESC FETCH FIRST 1 ROWS ONLY";
                obj = dl.RunSingle(sql, out errMsg);
            }

            if (obj.ToString() != "")
                count = int.Parse(obj.ToString());

            return count;
        }

        /// <summary>
        /// 获取显示数组最大KEY
        /// </summary>
        /// <param name="repName"></param>
        /// <returns></returns>
        public int GetCountZ(string repName)
        {
            int count = 0;
            string sql = "";
            object obj = null;

            this.init();

            if (rlDBType == "SQL")
            {
                sql = "select top 1 参数顺序 from T_sheet_ZsheetPara where 报表名称='" + repName + "'  and 参数顺序 !=0 order by 参数顺序 desc";
                obj = dl.RunSingle(sql, out errMsg);
            }
            else if (rlDBType == "DB2")
            {
                sql = "Select 参数顺序 FROM T_sheet_ZsheetPara WHERE (报表名称 = '" + repName + "') and  参数顺序 !=0 order BY 参数顺序 DESC FETCH FIRST 1 ROWS ONLY";
                obj = dl.RunSingle(sql, out errMsg);
            }

            if (obj.ToString() != "")
                count = int.Parse(obj.ToString());

            return count;
        }

        /// <summary>
        /// 值报存储过程解析存储过程参数
        /// 多班值配置
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="htGY"></param>
        /// <param name="htUn"></param>
        /// <returns></returns>
        public string RetMonSPPara(string paras, Hashtable htGY, Hashtable htUn)
        {
            string res = "";
            string str = "";

            if (paras != "" && paras != null)
            {
                string[] para = null; //公式参数数组

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    if (htGY.Count > 0)
                    {
                        if (htGY.ContainsKey(para[i]))
                            res += htGY[para[i].ToString()].ToString() + ',';
                    }

                    if (htUn.Count > 0)
                    {
                        if (htUn.ContainsKey(para[i]))
                            res += htUn[para[i].ToString()].ToString() + ',';
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 值报解析公式
        /// 多班值配置
        /// </summary>
        /// <param name="htGY"></param>
        /// <param name="htUn"></param>
        /// <param name="paras"></param>
        /// <param name="mon"></param>
        /// <returns></returns>
        public string retMonByValPara(Hashtable htGY, Hashtable htUn, string paras, string mon)
        {
            string str = "";
         
            if (paras != "")
            {
                string[] para = null; //公式参数数组

                if (paras.Contains(','))
                    para = paras.Split(',');
                else
                {
                    para = new string[1];
                    para[0] = paras;
                }

                for (int i = 0; i < para.Length; i++)
                {
                    if (htGY.Count > 0)
                    {
                        if (htGY.ContainsKey(para[i]))
                        {
                            str = htGY[para[i].ToString()].ToString();

                            mon = mon.Replace(para[i], str);
                        }
                    }

                    if (htUn.Count > 0)
                    {
                        if (htUn.ContainsKey(para[i]))
                        {
                            str = htUn[para[i].ToString()].ToString();

                            mon = mon.Replace(para[i].ToString(), str);
                        }
                    }
                }
            }

            return mon;
        }
        
        /// <summary>
        /// Table 转换 IList
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divider"></param>
        /// <param name="valAvg"></param>
        /// <param name="valRemain"></param>
        public void GetValue(string dividend, string divider,out string valAvg,out string valRemain)
        {
            valAvg = "";
            valRemain = "";

            string res = "";
            
            res=StrHelper.Cale(dividend+"/("+divider+"-1)");

            if (res.Contains('.'))
            {
                string[] resVal = res.Split('.');

                valAvg = resVal[0];
                valRemain = StrHelper.Cale(dividend + "-(" + divider+"-1)*"+resVal[0]);
            }
            else {
                //整除
                valAvg = res;
                valRemain = "0";
            }
        }
    }
}
