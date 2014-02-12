using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SAC.DBOperations;
using SAC.Helper;
using System.Collections;

namespace DAL
{
    public class DALDailyConsumption
    {
        DBLink dl = new DBLink();
        string errMsg = "";
        IList<Hashtable> listdata = new List<Hashtable>();
        public IList<Hashtable> GetChartData(string id,string Range,string rating_time ,out ArrayList ldd)
        {
            string errMsg = "";
            DataTable dt = null;
            string sql = "", sql1 = "";
             
            if (Range == "C_JTZB")
            {
                sql = "select T_PLANTID,t_base_company.t_comname from t_base_plant inner join t_base_company on t_base_company.t_comid = t_base_plant.t_comid order by t_base_plant.t_comid desc";
                dt = dl.RunDataTable(sql,out errMsg);
            }
            else if (Range == "C_JTZB1")
            {
                sql = "select T_PLANTID,t_base_department.t_depname  from t_base_plant inner join t_base_department on t_base_plant.t_depid = t_base_department.t_depid   order by t_depname desc";
                dt = dl.RunDataTable(sql, out errMsg);
            }
            else if (Range.Substring(0, 1) == "C")
            {
                sql = "select T_PLANTID,t_plantname  from t_base_plant where t_base_plant.t_comid ='" + Range + "' order by t_plantname desc";
                dt = dl.RunDataTable(sql, out errMsg);
            }
            else if (Range.Substring(0, 1) == "D")
            {
                sql = "select T_PLANTID,t_plantname   from t_base_plant where t_depid ='" + Range + "' order by t_comid desc";
                dt = dl.RunDataTable(sql, out errMsg);
            }
            IList<Hashtable> listdata = new List<Hashtable>();
            Hashtable ht = new Hashtable();
            ArrayList ld = new ArrayList();
            ArrayList lt = new ArrayList();
            ldd = new ArrayList();
            DataTable DTT = null;
            DateTime _sTime = new DateTime(1970, 1, 1);

            if (Range.Substring(0, 1) == "C")
            {
                string str = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        str +="'"+ dt.Rows[i]["T_PLANTID"].ToString() + "',";
                    }
                    else if (dt.Rows[i][1].ToString() == dt.Rows[i - 1][1].ToString())
                    {
                        str += "'" + dt.Rows[i]["T_PLANTID"].ToString() + "',";
                    }
                    else
                    {
                        str += ";" + dt.Rows[i - 1][1].ToString() + "|'" + dt.Rows[i]["T_PLANTID"].ToString() + "',";
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        str += ";" + dt.Rows[i - 1][1].ToString();
                    }
                }
                int num = 0;
                for (int j = 0; j < str.Split('|').Length; j++)
                {
                    if (id == "1")
                    {
                        sql1 = "select to_char(T_time,'yyyy-mm-dd') s2,sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
    "in (select T_PATITIONID from t_base_partition  where T_PLANTID in(" + str.Split('|')[j].Split(';')[0].TrimEnd(',') + ") and T_UNITTYPE ='DB') and T_TJID ='DL' " +
    "and T_time between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') " +
    "and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')) group by to_char(T_time,'yyyy-mm-dd') order by s2 asc";
    //                    sql1 = "select to_char(T_time,'yyyy-mm-dd HH24') s2,sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
    //"in (select T_PATITIONID from t_base_partition  where T_PLANTID in(" + str.Split('|')[j].Split(';')[0].TrimEnd(',') + ") and T_UNITTYPE ='DB') and T_TJID ='DL' " +
    //"and T_time between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') " +
    //"and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')) group by to_char(T_time,'yyyy-mm-dd HH24') order by s2 asc";
                    }
                    else if (id == "2")
                    {
                        sql1 = "select to_char(T_time,'yyyy-mm') s2,sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
    "in (select T_PATITIONID from t_base_partition  where T_PLANTID in(" + str.Split('|')[j].Split(';')[0].TrimEnd(',') + ") and T_UNITTYPE ='DB') and T_TJID ='DL' " +
    "and T_time between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') " +
    "and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')) group by to_char(T_time,'yyyy-mm') order by s2 asc";
                    }
                    else
                    {
                        sql1 = "select to_char(T_time,'yyyy') s2,sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
    "in (select T_PATITIONID from t_base_partition  where T_PLANTID in(" + str.Split('|')[j].Split(';')[0].TrimEnd(',') + ") and T_UNITTYPE ='DB') and T_TJID ='DL' " +
    "and T_time between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') " +
    "and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')) group by to_char(T_time,'yyyy') order by s2 asc";
                    }
                    DTT = dl.RunDataTable(sql1, out errMsg);
                    ht = new Hashtable();
                    lt = new ArrayList();
                    if (DTT.Rows.Count > 0)
                    {
                        for (int k = 0; k < DTT.Rows.Count; k++)
                        {
                            if (num == 0)
                            {
                                ldd.Add(DTT.Rows[k]["s2"].ToString());
                            }
                            ld = new ArrayList();
                            ld.Add(DTT.Rows[k]["s2"].ToString());
                            ld.Add(Math.Round(Convert.ToDouble(DTT.Rows[k][1].ToString()), 2));
                            lt.Add(ld);
                        }
                        num++;
                        ht.Add("name", str.Split('|')[j].Split(';')[1]);
                        ht.Add("data", lt);
                        listdata.Add(ht);
                    }
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (id == "1")
                    {
                        sql1 = "select to_char(T_time,'yyyy-mm-dd') s2,sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
        "in (select T_PATITIONID from t_base_partition  where T_PLANTID =('" + dt.Rows[i][0].ToString() + "') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
        "and T_time between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') " +
        "and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')) group by to_char(T_time,'yyyy-mm-dd') order by s2 asc";
                    }
                    else if (id == "2")
                    {
                        sql1 = "select to_char(T_time,'yyyy-mm') s2,sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
        "in (select T_PATITIONID from t_base_partition  where T_PLANTID =('" + dt.Rows[i][0].ToString() + "') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
        "and T_time between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') " +
        "and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')) group by to_char(T_time,'yyyy-mm') order by s2 asc";
                    }
                    else
                    {
                        sql1 = "select to_char(T_time,'yyyy') s2,sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
       "in (select T_PATITIONID from t_base_partition  where T_PLANTID =('" + dt.Rows[i][0].ToString() + "') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
       "and T_time between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') " +
       "and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')) group by to_char(T_time,'yyyy') order by s2 asc";
                    }
                    int num = 0;
                    DTT = dl.RunDataTable(sql1, out errMsg);
                    ht = new Hashtable();
                    lt = new ArrayList();
                    if (DTT.Rows.Count > 0)
                    {
                        for (int k = 0; k < DTT.Rows.Count; k++)
                        {
                            if (num == 0)
                            {
                                ldd.Add(DTT.Rows[k]["s2"].ToString());
                            }
                            ld = new ArrayList();
                            ld.Add(DTT.Rows[k]["s2"].ToString());
                            ld.Add(Math.Round(Convert.ToDouble(DTT.Rows[k][1].ToString()), 2));
                            lt.Add(ld);
                        }
                        num++;
                        ht.Add("name", dt.Rows[i]["t_plantname"].ToString());
                        ht.Add("data", lt);
                        listdata.Add(ht);
                    }
                }
            }
            return listdata;
        }

        public static int DateTimeToUTC(DateTime DT)
        {
            long a = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;
            int rtnInt = 0;
            rtnInt = (int)((DT.Ticks - 8 * 3600 * 1e7 - a) / 1e7);
            return rtnInt;
        }
    }
}
