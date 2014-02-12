using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SAC.DBOperations;
using SAC.Helper;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Collections;

namespace DAL
{
    public class DALPowerTrends
    {
        DBLink dl = new DBLink();
        string errMsg = "";
        IList<Hashtable> listdata = new List<Hashtable>();
        public IList<Hashtable> GetChartData(string id,string rating_time)
        {
            string errMsg = "";
            DataTable dt = null;
            string sql = "";
            if (id == "1")
            {
                sql = "select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,T_BASE_COMPANY.t_comname,T_BASE_COMPANY.t_comid " +
                        "from t_base_points_org  inner join T_BASE_COMPANY on t_base_points_org.t_orgid=T_BASE_COMPANY.t_comid";
            }
            else if (id == "2")
            {
                sql = "select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,t_base_department.t_depname as t_comname  from t_base_points_org  inner join t_base_department on " +
                    "t_base_points_org.t_orgid=t_base_department.t_depid";
            }
            else if (id == "3")
            {
                sql = " select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,t_base_plant.t_plantname as t_comname, " +
    "t_base_plant.t_plantid as T_COMID from t_base_points_org  inner join t_base_plant on " +
    "t_base_points_org.t_orgid=t_base_plant.t_plantid and t_base_plant.t_comid='C_DIMIAN' " +
    "order by   t_base_plant.t_plantid asc";
            }
            else if (id == "4")
            {
                sql = " select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,t_base_plant.t_plantname as t_comname, " +
    "t_base_plant.t_plantid as T_COMID from t_base_points_org  inner join t_base_plant on " +
    "t_base_points_org.t_orgid=t_base_plant.t_plantid and t_base_plant.t_comid='C_BAPV' " +
    "order by   t_base_plant.t_plantid asc";
            }
            dt = dl.RunDataTable(sql, out errMsg);
            IList<Hashtable> listdata = new List<Hashtable>();
            Hashtable ht = new Hashtable();
            ArrayList ld = new ArrayList();
            ArrayList lt = new ArrayList();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable DTT = null;
                    ht = new Hashtable();
                    lt = new ArrayList();
                    DateTime _sTime = new DateTime(1970, 1, 1);
                    string sql1 = "select avg(D_VALUE),to_char(T_TIME,'yyyy-MM-dd HH24') s2 from t_info_statiscs where  t_orgid='" + dt.Rows[i]["T_POWERTAG"].ToString() + "'  and T_TIME between to_date('" + rating_time.Split(',')[0] + "','yyyy-MM-dd HH24:MI:SS') and  to_date('" + rating_time.Split(',')[1] + "','yyyy-MM-dd HH24:MI:SS')  group by   to_char(T_TIME,'yyyy-MM-dd HH24')   order by s2 asc";
                    DTT = dl.RunDataTable(sql1, out errMsg);
                    if (DTT.Rows.Count > 400)
                    {
                        for (int j = 0; j < DTT.Rows.Count; j = j + (DTT.Rows.Count / 400))
                        {
                            ld = new ArrayList();
                            string timeStamp = DateTimeToUTC(DateTime.Parse(DTT.Rows[j][1].ToString())).ToString();
                            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                            long lTime = long.Parse(timeStamp + "0000000");
                            TimeSpan toNow = new TimeSpan(lTime);
                            DateTime dtResult = dtStart.Add(toNow);
                            ld.Add(Convert.ToInt64((dtResult - _sTime).TotalMilliseconds.ToString()));
                            ld.Add(Math.Round(Convert.ToDouble(DTT.Rows[j][0].ToString()), 2));
                            lt.Add(ld);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < DTT.Rows.Count; j++)
                        {
                            ld = new ArrayList();
                            string timeStamp = DateTimeToUTC(DateTime.Parse(DTT.Rows[j][1].ToString())).ToString();
                            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                            long lTime = long.Parse(timeStamp + "0000000");
                            TimeSpan toNow = new TimeSpan(lTime);
                            DateTime dtResult = dtStart.Add(toNow);
                            ld.Add(Convert.ToInt64((dtResult - _sTime).TotalMilliseconds.ToString()));
                            ld.Add(Math.Round(Convert.ToDouble(DTT.Rows[j][0].ToString()), 2));
                            lt.Add(ld);
                        }
                    }


                    ht.Add("name", dt.Rows[i]["t_comname"].ToString());
                    ht.Add("data", lt);
                    listdata.Add(ht);
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
