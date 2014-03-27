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

namespace DAL
{
    public class DALDefault
    {
        DBLink dl = new DBLink();
        string errMsg = "";
        //获取所有总装机量
        public double GetCapacity(DateTime d_time)
        {
            double capacity = 0;

            DataTable dt = null;
            string sql = "select * from t_base_plant order by ID_KEY asc";
            dt = dl.RunDataTable(sql, out errMsg);

            if (dt.Rows.Count > 0)
            {
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable DTT = null;


                    string sql1 = "select 容量 from(select row_number() over(order by T_time desc) seq,t.* from  t_info_rl t where T_ORGID='" + dt.Rows[i]["T_PLANTID"].ToString() + "' and T_time <= to_date('" + d_time + "','yyyy-MM-dd HH24:MI:SS')  ) where seq=1  order by T_TIME desc";
                    DTT = dl.RunDataTable(sql1, out errMsg);
                    if (DTT.Rows.Count > 0)
                    {
                        capacity += Convert.ToDouble(DTT.Rows[0][0].ToString());
                    }
                }
            }
            return Math.Round(capacity, 2); 
        }
        //获取所有总装机量-根据部门
        public double GetCapacityByDepid(string id, DateTime d_time)
        {
            double capacity = 0;
            DataTable dt = null;
            string sql = "select * from t_base_plant where "+id+" order by ID_KEY asc";
            dt = dl.RunDataTable(sql, out errMsg);

            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable DTT = null;
                    string sql1 = "select 容量 from(select row_number() over(order by T_time desc) seq,t.* from  t_info_rl t where T_ORGID='" + dt.Rows[i]["T_PLANTID"].ToString() + "' and T_time <=  to_date('" + d_time + "','yyyy-MM-dd HH24:MI:SS')  ) where seq=1  order by T_TIME desc";
                    // T_time between to_date('" + DateTime.Now.Year + "-01-01 00:00:00" + "','yyyy-MM-dd HH24:MI:SS') and  to_date('" + d_time + "','yyyy-MM-dd HH24:MI:SS') 
                    DTT = dl.RunDataTable(sql1, out errMsg);
                    if (DTT.Rows.Count > 0)
                    {
                        capacity += Convert.ToDouble(DTT.Rows[0][0].ToString());
                    }
                }
            }
            return Math.Round(capacity, 2); 
        }
        //获取实时负荷
        public double GetReaload(string id,int num)
        {
            double realroad = 0;
            DataTable dt = null;
            string sql="";
            if(num==1)
            {
                sql="select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,T_BASE_COMPANY.t_comid  from t_base_points_org   inner join T_BASE_COMPANY "+
                    " on t_base_points_org.t_orgid=T_BASE_COMPANY.t_comid and " + id;
            }
            else if (num == 2)
            {
                sql = " select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,t_base_department.t_depname,t_base_department.t_depid " +
 "from t_base_points_org  inner join t_base_department on t_base_points_org.t_orgid=t_base_department.t_depid  and " + id;
            }
            else if(num==3)
            {
                sql = "select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,T_BASE_COMPANY.t_comname,t_base_plant.t_comid  " +
                "from t_base_points_org  inner join t_base_plant on t_base_points_org.t_orgid=t_base_plant.t_plantid and    " + id + " inner join T_BASE_COMPANY on t_base_plant.t_comid=T_BASE_COMPANY.t_comid";
            }
             
            dt = dl.RunDataTable(sql, out errMsg);

            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                        DataTable DTT = GetDT(dt.Rows[i]["T_POWERTAG"].ToString(), DateTime.Now);//T_INFO_REALTIMEVALUE原本是实时表
//                    string sql1 = "select * from (select * from t_info_histvalue where T_TAG='" + dt.Rows[i]["T_POWERTAG"].ToString() + "' and "+
//"t_time<=to_date('"+DateTime.Now+"','yyyy-MM-dd HH24:MI:SS') order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
//                    DTT = dl.RunDataTable(sql1, out errMsg);
                    if (DTT.Rows.Count > 0)
                    {
                        realroad += Convert.ToDouble(DTT.Rows[0]["D_VALUE"].ToString());
                    }
                }
            }
            return Math.Round(realroad, 2); 
        }
        //获取总发电量
        public double GetPower(string id,DateTime d_time,int num)
        {
            double power = 0;
            DataTable dt = null;
            string sql = "select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,T_BASE_COMPANY.t_comname,t_base_plant.t_comid  " +
                "from t_base_points_org  inner join t_base_plant on t_base_points_org.t_orgid=t_base_plant.t_plantid and   " + id + "  inner join T_BASE_COMPANY on t_base_plant.t_comid=T_BASE_COMPANY.t_comid";
            dt = dl.RunDataTable(sql, out errMsg);
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //日
                    if (num == 1)
                    {
                        power += GetPowerByDianzhan(dt.Rows[i]["t_orgid"].ToString(), d_time, "1");
                    }
                    //月
                    else if (num == 2)
                    {
                        power += GetPowerByDianzhan(dt.Rows[i]["t_orgid"].ToString(), d_time, "2");
                    }
                    else
                    {
                        power += GetPowerByDianzhan(dt.Rows[i]["t_orgid"].ToString(), d_time);
                    }
                }
            }
            return Math.Round(power, 2); 

        }

        public double GetPower(string id, DateTime d_time)
        {
            double power = 0;
            DataTable dt = null;
            string sql = "select t_base_points_org.t_orgid,t_base_points_org.T_POWERTAG,T_BASE_COMPANY.t_comname,t_base_plant.t_comid  " +
                "from t_base_points_org  inner join t_base_plant on t_base_points_org.t_orgid=t_base_plant.t_plantid and   " + id + "  inner join T_BASE_COMPANY on t_base_plant.t_comid=T_BASE_COMPANY.t_comid";
            dt = dl.RunDataTable(sql, out errMsg);
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    power += GetPowerByDianzhan(dt.Rows[i]["t_orgid"].ToString(), d_time);
                }
            }
            return Math.Round(power, 2); 

        }

         public double GetPowerByDianzhan(string id,DateTime d_time) 
         {
             double power = 0;
            DataTable dt = null;
            string sql = "select sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
            "in (select T_PATITIONID from t_base_partition  where T_PLANTID='"+id+"') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
            "and T_time between to_date('" + d_time.Year + "-01-01 00:00:00" + "','yyyy-MM-dd HH24:MI:SS') and  to_date('" + d_time + "','yyyy-MM-dd HH24:MI:SS') ";
           
            dt = dl.RunDataTable(sql, out errMsg);
            if ((dt.Rows.Count > 0) && (dt.Rows[0][0].ToString().Trim()!=""))
            {
                power += Convert.ToDouble(dt.Rows[0][0].ToString());
            }
            return Math.Round( power,2);
         }
         public double GetPowerByDianzhan(string id, DateTime d_time,string num)
         {
             double power = 0;
             string errMsg = "";
             DataTable dt = null;
             string sql = "";
             if (num == "2")
             {
                 sql = "select sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
                 "in (select T_PATITIONID from t_base_partition  where T_PLANTID='" + id + "') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
                 "and T_time between to_date('" + d_time.Year + "-" + d_time.Month + "-01 00:00:00" + "','yyyy-MM-dd HH24:MI:SS') and  to_date('" + d_time + "','yyyy-MM-dd HH24:MI:SS') ";
             }
             else
             {
                 sql = "select sum(D_VALUE) from t_info_statiscs where T_ORGID in (select T_UNITID from t_base_unit  where T_PATITIONID " +
                 "in (select T_PATITIONID from t_base_partition  where T_PLANTID='" + id + "') and T_UNITTYPE ='DB') and T_TJID ='DL' " +
                 "and T_time between to_date('" + d_time.ToString("yyyy-MM-dd") + " 00:00:00" + "','yyyy-MM-dd HH24:MI:SS') and  to_date('" + d_time + "','yyyy-MM-dd HH24:MI:SS') ";
             }
             
             dt = dl.RunDataTable(sql, out errMsg);
             if ((dt.Rows.Count > 0) && (dt.Rows[0][0].ToString().Trim() != ""))
             {
                 power += Convert.ToDouble(dt.Rows[0][0].ToString());
             }
             return Math.Round(power, 2);
         }


         public double GetPowerDianzhan(string id, string s_time, string e_time)
         {
             double power = 0;
             DataTable dt = null;
             string sql = "";

             sql = "select sum(D_VALUE) from t_info_statiscs where T_ORGID like '" + id + "%' and T_TJID ='DL' " +
            "and T_time between to_date('" + s_time + "','yyyy-MM-dd HH24:MI:SS') and  to_date('" + e_time + "','yyyy-MM-dd HH24:MI:SS') ";
            

             dt = dl.RunDataTable(sql, out errMsg);
             if ((dt.Rows.Count > 0) && (dt.Rows[0][0].ToString().Trim() != ""))
             {
                 power += Convert.ToDouble(dt.Rows[0][0].ToString());
             }
             return Math.Round(power, 2);
         }

        public string  RealValue(string id)
        {
            string[] str = new string[51]{"运行","待机","停机","故障","启动中","关机中","设备状态","调度运行","告警运行","初始待机","按键关机","紧急停机","A相电流","B相电流","C相电流","A-B线电压","B-C线电压","A-C线电压","直流电流","直流电压","直流功率","直流过流","直流过压","直流欠压","电网过流","电网过压","电网频率","电网欠压","模块过温","蓄电池过压","蓄电池欠压","变压器过温","逆变过压","频率异常","输出过载","硬件故障","孤岛故障","接地故障","温度故障","模块故障","继电器故障","接触器故障","绝缘阻抗故障","功率因数","无功功率","日发电量","日发电量/K","总发电量/K","总有功功率","总运行时间/K","机内空气温度" };
            
            DataTable dt = null;//T_INFO_REALTIMEVALUE原本是实时表
            string sql = "select t_base_tag.t_tagdesc,t.D_VALUE  from  t_base_tag right join (select T_TAG,D_VALUE from T_INFO_REALTIMEVALUE  where  " +
"T_INFO_REALTIMEVALUE.T_TAG in (select t_tagid from  t_base_tag  where t_tagdesc like '%" + id + "%')) t on t.T_TAG = t_base_tag.t_tagid";
            dt = dl.RunDataTable(sql, out errMsg);
            
            for(int i=0;i<dt.Rows.Count;i++)
            {
                if((id+str[i]) ==dt.Rows[i]["T_TAG"].ToString())
                {
                    str[i] = dt.Rows[i]["D_VALUE"].ToString();
                    break;
                }
            }
            string str_sb="";
            for(int j=0;j<str.Length;j++)
            {
                str_sb+=str[j]+",";
            }
            return str_sb;
        }

        public DataTable GetTagId(string id)
        {
            DataTable dt = null;//T_INFO_REALTIMEVALUE原本是实时表
            string sql = "select t_tagid,t_tagdesc from  t_base_tag  where t_tagdesc like '%" + id + "%'";
            dt = dl.RunDataTable(sql, out errMsg);
            return dt;
        }

        public string RealValueDT(string id)
        {
            //string[] str = new string[51] { "运行", "待机", "停机", "故障", "启动中", "关机中", "设备状态", "调度运行", "告警运行", "初始待机", "按键关机", "紧急停机", "A相电流", "B相电流", "C相电流", "A-B线电压", "B-C线电压", "A-C线电压", "直流电流", "直流电压", "直流功率", "直流过流", "直流过压", "直流欠压", "电网过流", "电网过压", "电网频率", "电网欠压", "模块过温", "蓄电池过压", "蓄电池欠压", "变压器过温", "逆变过压", "频率异常", "输出过载", "硬件故障", "孤岛故障", "接地故障", "温度故障", "模块故障", "继电器故障", "接触器故障", "绝缘阻抗故障", "功率因数", "无功功率", "日发电量", "日发电量/K", "总发电量/K", "总有功功率", "总运行时间/K", "机内空气温度" };
            string num = "0";
           // DataTable dt = null;//T_INFO_REALTIMEVALUE原本是实时表
//            string sql = "select D_VALUE from (select D_VALUE from t_info_histvalue where T_TAG='" + id+ "' and " +
//"t_time<=to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS') order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
//            dt = dl.RunDataTable(sql, out errMsg);
            DataTable dt = GetDT(id, DateTime.Now);
            if ((dt.Rows.Count > 0) && (dt.Rows[0]["D_VALUE"].ToString().Trim() != ""))
            {
                num = Convert.ToDouble(dt.Rows[0]["D_VALUE"].ToString()).ToString("f2");
            }
            return num;
        }


        public DataTable GetDT(string id,DateTime t_time)
        {
            DataTable dt = null;
//            string sql = "select * from (select * from T_INFO_REALTIMEVALUE where T_TAG='" + id + "' and " +
//"t_time<=to_date('" + t_time + "','yyyy-MM-dd HH24:MI:SS') order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
            string sql = "select * from (select * from T_INFO_REALTIMEVALUE where T_TAG='" + id + "' order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
            dt = dl.RunDataTable(sql, out errMsg);
            return dt;
        }
    }
}
