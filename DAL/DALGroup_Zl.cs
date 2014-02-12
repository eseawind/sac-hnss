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
    public class DALGroup_Zl
    {
        DBLink dl = new DBLink();
        string errMsg = "";
        public IList<Hashtable> GetChartData(string id)
        {
            double capacity = 0;
            string errMsg = "";
            DataTable dt = null;
            string sql = "";
            if (id == "1")
            {
                sql = "select T_PLANTID,t_base_plant.T_COMID,T_COMNAME from t_base_plant " +
    "left  join T_BASE_COMPANY on T_BASE_COMPANY.T_COMID =t_base_plant.T_COMID   order by t_base_plant.T_COMID asc";
            }
            else if (id == "2")
            {
                sql = "select T_PLANTID,t_base_plant.T_DEPID as T_COMID,T_DEPNAME  as T_COMNAME from t_base_plant " +
    "left  join T_BASE_DEPARTMENT on T_BASE_DEPARTMENT.T_DEPID =t_base_plant.T_DEPID   order by t_base_plant.T_DEPID asc";
            }
            else if (id == "3")
            {
                sql = "select T_PLANTID,t_base_plant.t_plantid as T_COMID,t_base_plant.t_plantname  as T_COMNAME from t_base_plant inner  join "+
"T_BASE_DEPARTMENT on T_BASE_DEPARTMENT.T_DEPID =t_base_plant.T_DEPID and t_base_plant.T_comid ='C_DIMIAN' order by t_base_plant.t_plantid asc";
            }
            else if (id == "4")
            {
                sql = "select T_PLANTID,t_base_plant.t_plantid as T_COMID,t_base_plant.t_plantname  as T_COMNAME from t_base_plant inner  join " +
"T_BASE_DEPARTMENT on T_BASE_DEPARTMENT.T_DEPID =t_base_plant.T_DEPID and t_base_plant.T_comid ='C_BAPV' order by t_base_plant.t_plantid asc";
            }
            dt = dl.RunDataTable(sql, out errMsg);
            IList<Hashtable> listdata = new List<Hashtable>();
            Hashtable ht = new Hashtable();
            ArrayList ld = new ArrayList();
            ArrayList lt = new ArrayList();

            DAL.DALDefault df = new DALDefault();
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        capacity +=df.GetPowerByDianzhan(dt.Rows[i]["T_PLANTID"].ToString(), DateTime.Now);
                    }
                    else if (dt.Rows[i]["T_COMID"].ToString() == dt.Rows[i - 1]["T_COMID"].ToString())
                    {
                        capacity += df.GetPowerByDianzhan(dt.Rows[i]["T_PLANTID"].ToString(), DateTime.Now);
                    }
                    else
                    {
                        ld.Add(dt.Rows[i - 1]["T_COMNAME"].ToString());
                        ld.Add(Math.Round(capacity, 2));
                        lt.Add(ld);
                        ld = new ArrayList();
                        capacity = df.GetPowerByDianzhan(dt.Rows[i]["T_PLANTID"].ToString(), DateTime.Now);
                    }
                    if (i == dt.Rows.Count - 1)
                    {

                        ld.Add(dt.Rows[i]["T_COMNAME"].ToString());
                        ld.Add(Math.Round(capacity, 2));
                        lt.Add(ld);
                        ht.Add("name", "发电比重");
                        ht.Add("data", lt);
                        listdata.Add(ht);
                    }
                }
                
            }
            return listdata;
        }

        public IList<Hashtable> GetLineData(string id)
        {
            string errMsg = "";
            DataTable dt = null;
            string sql = "";
            if (id == "1")
            {
                sql = "select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,T_BASE_COMPANY.t_comname,T_BASE_COMPANY.t_comid "+
                        "from t_base_points_org  inner join T_BASE_COMPANY on t_base_points_org.t_orgid=T_BASE_COMPANY.t_comid";
            }
            else if (id == "2")
            {
                sql = "select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,t_base_department.t_depname as t_comname  from t_base_points_org  inner join t_base_department on " +
                    "t_base_points_org.t_orgid=t_base_department.t_depid and t_base_department.t_depid ='D_CN'";
            }
            else if (id == "3")
            {
                sql = " select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,t_base_plant.t_plantname as t_comname, "+
    "t_base_plant.t_plantid as T_COMID from t_base_points_org  inner join t_base_plant on "+ 
    "t_base_points_org.t_orgid=t_base_plant.t_plantid and t_base_plant.t_comid='C_DIMIAN' "+
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
                    DataTable DTT = null;//T_INFO_REALTIMEVALUE原本是实时表

                    string sql1 = "select * from (select * from t_info_histvalue where T_TAG='" + dt.Rows[i]["T_POWERTAG"].ToString() + "' and " +
"t_time<=to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS') order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
                    DTT = dl.RunDataTable(sql1, out errMsg);
                    if (DTT.Rows.Count > 0)
                    {

                        ld.Add(Math.Round(Convert.ToDouble(DTT.Rows[0]["D_VALUE"].ToString()), 2));
                            
                            ht.Add("name", dt.Rows[i]["t_comname"].ToString());
                            ht.Add("data", ld);
                            ld = new ArrayList();
                            listdata.Add(ht);
                            ht = new Hashtable();
                    }
                }

            }
            return listdata;
        }
    }
}
