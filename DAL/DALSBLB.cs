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
    public class DALSBLB
    {
        DBLink dl = new DBLink();
        string errMsg = "";

        public ArrayList GetPOINTS_UNIT(string id)
        {
            ArrayList ld = new ArrayList();
            string sql = "select * from t_base_unit where T_PATITIONID ='" + id + "' and t_unittype ='NBQ'";
            DataTable dt = dl.RunDataTable(sql,out errMsg);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                        ld.Add(dt.Rows[i]["T_UNITID"].ToString());
                }
            }
            return ld;
        }

        public double GerUnitNum(string id)
        {
            double unit_num = 0;

            string sql_jtzb = "select count(*) from t_base_unit where t_patitionid in " +
"(select t_patitionid from t_base_partition where t_plantid " +
 "in(select T_PLANTID from t_base_plant where " + id + "))  and t_unittype ='NBQ'";
            DataTable dt = dl.RunDataTable(sql_jtzb, out  errMsg);

            if (dt.Rows[0][0].ToString().Trim() != "")
            {
                unit_num += Convert.ToDouble(dt.Rows[0][0].ToString());
            }
            return unit_num;
        }

        public string  GetPointRealValue(string id)
        {
            string str = "";
            string sql = "select * from t_base_points_org where T_ORGID ='" + id + "'";
            DataTable dt = dl.RunDataTable(sql, out errMsg);
            if(dt.Rows.Count>0)
            {
                for( int i=2;i<dt.Columns.Count;i++)
                {
                    if (dt.Rows[0][i].ToString() == "T_QHHNZ_ZGL")
                    {
                        str += "0,";
                    }
                    else if (dt.Rows[0][i].ToString() != "")
                    {
                        str += GetRealValue(dt.Rows[0][i].ToString()) + ",";
                    }
                    else
                    {
                        str += "0,";
                    }
                }
            }
            return str;
        }

        public double GetElcValue(string id,string plant_id)
        {
            double d_value =0;
            string sql ="";
            if (id == "1")
            {
                sql = "select sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
               "in (select T_PATITIONID from t_base_partition  where T_PLANTID ='" + plant_id + "') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
               "and T_time between to_date('" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") +"','yyyy-MM-dd HH24:MI:SS') " +
                "and  to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS')";
            }
            else if (id == "2")
            {
                sql = "select sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
                "in (select T_PATITIONID from t_base_partition  where T_PLANTID ='" + plant_id + "') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
                "and T_time between to_date('" + DateTime.Now.ToString("yyyy-MM-01 00:00:00") + "','yyyy-MM-dd HH24:MI:SS') " +
                "and  to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS')";
            }
            else
            {
                sql = "select sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
                "in (select T_PATITIONID from t_base_partition  where T_PLANTID ='" + plant_id + "') and T_UNITTYPE ='DB') and T_TJID ='DL' "+
                 "and T_time between to_date('" + DateTime.Now.ToString("yyyy-01-01 00:00:00") + "','yyyy-MM-dd HH24:MI:SS') " +
                "and  to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS')";
                
            }
            DataTable dt = dl.RunDataTable(sql ,out errMsg);
            if(dt.Rows[0][0].ToString().Trim() !="")
            {
                d_value = Convert.ToDouble(dt.Rows[0][0]);
            }
            return Math.Round( d_value,2);
        }

        public double GetRealValue(string id)
        {
            double d_value = 0;//T_INFO_REALTIMEVALUE原本是实时表

            string sql = "select * from (select * from t_info_histvalue where T_TAG='" + id + "' and " +
"t_time<=to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS') order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
            DataTable dt = dl.RunDataTable(sql, out errMsg);
            if (dt.Rows.Count > 0)
            {
                d_value =Convert.ToDouble( dt.Rows[0]["D_VALUE"].ToString());
            }
            return Math.Round(d_value, 2);
        }
        public int GetPointNum(string id)
        {
            int num = 0;
            string sql = "select * from T_BASE_PARTITION where T_PLANTID ='" + id + "'";
            DataTable dt = dl.RunDataTable(sql, out errMsg);
            num = Convert.ToInt32(dt.Rows.Count);
            return num;
        }
        public DataTable GetPoint(string id)
        {
            string sql = "select * from T_BASE_PARTITION where T_PLANTID ='" + id + "'";
            DataTable dt = dl.RunDataTable(sql, out errMsg);

            return dt;
        }
        public double GetPointValue(string id)
        {
            double d_value = 0;
            string sql = "select * from T_BASE_POINTS_UNIT where T_ORGID ='"+id+"'";
            DataTable dt = dl.RunDataTable(sql,out errMsg);
            if (dt.Rows.Count > 0)
            {
                //T_INFO_REALTIMEVALUE原本是实时表
                string sql1 = "select * from (select * from t_info_histvalue where T_TAG='" + dt.Rows[0]["T_POWERTAG"].ToString() + "' and " +
"t_time<=to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS') order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
                 
                DataTable dtt =  dl.RunDataTable(sql1,out errMsg);
                if (dtt.Rows[0][0].ToString() != "")
                {
                    d_value = Convert.ToDouble(dtt.Rows[0][0].ToString());
                }
            }
            return Math.Round(d_value, 2);
        }
    }
}
