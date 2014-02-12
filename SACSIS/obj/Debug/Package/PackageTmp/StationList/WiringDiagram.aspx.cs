using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SAC.DBOperations;

namespace SACSIS.StationList
{
    public partial class WiringDiagram : System.Web.UI.Page
    {
        string plant_id = "",id="";
        protected void Page_Load(object sender, EventArgs e)
        {
            plant_id = Request["plant_id"]; id = Request["id"];
            if ((plant_id != "") && (plant_id != null))
            {
                GetPlant();

            }
            if ((id != "") && (id != null))
            {
                

            }
        }
        DBLink dl = new DBLink();
        string errMsg = "";
        DataTable dt = new DataTable();
        private void GetPlant()
        {
            string str_sb="";

            string str ="'JSQP_A2_001Q','JSQP_A3_001Q','JSQP_A4_001Q','JSQP_A2_001Q.F','JSQP_A3_001Q.F','JSQP_A4_001Q.F',"+
                "'JSQP_A2_001Q.P','JSQP_A3_001Q.P','JSQP_A4_001Q.P','JSQP_A2_001Q.G','JSQP_A3_001Q.G','JSQP_A4_001Q.G','JSQP_A2_003NQ001.D','JSQP_A2_003NQ001.S', "+
                "'JSQP_A2_004NQ001.D','JSQP_A2_004NQ001.S','JSQP_A2_NQ001.D','JSQP_A2_NQ001.S','JSQP_A3_005NQ001.D','JSQP_A3_005NQ001.S','JSQP_A3_006NQ001.D', "+
                "'JSQP_A3_006NQ001.S','JSQP_A3_NQ001.D','JSQP_A3_NQ001.S','JSQP_A4_NQ001.D','JSQP_A4_NQ001.S','JSQP_A6_001Q','JSQP_A7_001Q','JSQP_A8_001Q',"+
                "'JSQP_A6_001Q.F','JSQP_A7_001Q.F','JSQP_A8_001Q.F','JSQP_A6_001Q.P','JSQP_A7_001Q.P','JSQP_A8_001Q.P','JSQP_A6_001Q.G','JSQP_A7_001Q.G', "+
                "'JSQP_A8_001Q.G','JSQP_A6_001NQ001.D','JSQP_A6_001NQ001.S','JSQP_A6_002NQ001.D','JSQP_A6_002NQ001.S','JSQP_A6_NQ001.D','JSQP_A6_NQ001.S', "+
                "'JSQP_A7_NQ001.D','JSQP_A7_NQ001.S','JSQP_A8_NQ001.D','JSQP_A8_NQ001.S' ,'JSQP_C1_001Q','JSQP_C2_001Q','JSQP_C1_001Q.F','JSQP_C2_001Q.F',"+
                "'JSQP_C1_001Q.P','JSQP_C2_001Q.P','JSQP_C1_001Q.G','JSQP_C2_001Q.G','JSQP_C1_NQ001.D','JSQP_C1_NQ001.S','JSQP_C2_NQ001.D','JSQP_C2_NQ001.S',"+
                "'JSQP_C3_001Q','JSQP_C4_001Q','JSQP_C5_001Q','JSQP_C3_001Q.F','JSQP_C4_001Q.F','JSQP_C5_001Q.F','JSQP_C3_001Q.P','JSQP_C4_001Q.P',"+
                "'JSQP_C5_001Q.P','JSQP_C3_001Q.G','JSQP_C4_001Q.G','JSQP_C5_001Q.G','JSQP_C3_007NQ001.D','JSQP_C3_007NQ001.S','JSQP_C3_008NQ001.D','JSQP_C3_008NQ001.S',"+
                "'JSQP_C3_NQ001.D','JSQP_C3_NQ001.S','JSQP_C4_NQ001.D','JSQP_C4_NQ001.S','JSQP_C5_NQ001.D','JSQP_C5_NQ001.S'";
            string[] str_shuzu = new string[82] { "JSQP_A2_001Q", "JSQP_A3_001Q", "JSQP_A4_001Q", "JSQP_A2_001Q.F", "JSQP_A3_001Q.F", "JSQP_A4_001Q.F", 
                "JSQP_A2_001Q.P", "JSQP_A3_001Q.P", "JSQP_A4_001Q.P", "JSQP_A2_001Q.G", "JSQP_A3_001Q.G", "JSQP_A4_001Q.G", "JSQP_A2_003NQ001.D", "JSQP_A2_003NQ001.S",
                "JSQP_A2_004NQ001.D", "JSQP_A2_004NQ001.S", "JSQP_A2_NQ001.D", "JSQP_A2_NQ001.S", "JSQP_A3_005NQ001.D", "JSQP_A3_005NQ001.S", "JSQP_A3_006NQ001.D", 
                "JSQP_A3_006NQ001.S", "JSQP_A3_NQ001.D", "JSQP_A3_NQ001.S", "JSQP_A4_NQ001.D", "JSQP_A4_NQ001.S","JSQP_A6_001Q", "JSQP_A7_001Q", "JSQP_A8_001Q",
                "JSQP_A6_001Q.F", "JSQP_A7_001Q.F", "JSQP_A8_001Q.F", "JSQP_A6_001Q.P", "JSQP_A7_001Q.P", "JSQP_A8_001Q.P", "JSQP_A6_001Q.G", "JSQP_A7_001Q.G", 
                "JSQP_A8_001Q.G", "JSQP_A6_001NQ001.D", "JSQP_A6_001NQ001.S", "JSQP_A6_002NQ001.D", "JSQP_A6_002NQ001.S", "JSQP_A6_NQ001.D", "JSQP_A6_NQ001.S", 
                "JSQP_A7_NQ001.D", "JSQP_A7_NQ001.S", "JSQP_A8_NQ001.D", "JSQP_A8_NQ001.S" ,"JSQP_C1_001Q", "JSQP_C2_001Q", "JSQP_C1_001Q.F", "JSQP_C2_001Q.F",
                "JSQP_C1_001Q.P","JSQP_C2_001Q.P", "JSQP_C1_001Q.G", "JSQP_C2_001Q.G", "JSQP_C1_NQ001.D", "JSQP_C1_NQ001.S", "JSQP_C2_NQ001.D", "JSQP_C2_NQ001.S",
                "JSQP_C3_001Q", "JSQP_C4_001Q", "JSQP_C5_001Q", "JSQP_C3_001Q.F", "JSQP_C4_001Q.F", "JSQP_C5_001Q.F", "JSQP_C3_001Q.P", "JSQP_C4_001Q.P",
                "JSQP_C5_001Q.P", "JSQP_C3_001Q.G", "JSQP_C4_001Q.G", "JSQP_C5_001Q.G", "JSQP_C3_007NQ001.D", "JSQP_C3_007NQ001.S", "JSQP_C3_008NQ001.D", "JSQP_C3_008NQ001.S",
                "JSQP_C3_NQ001.D", "JSQP_C3_NQ001.S", "JSQP_C4_NQ001.D", "JSQP_C4_NQ001.S", "JSQP_C5_NQ001.D", "JSQP_C5_NQ001.S"};
            //T_INFO_REALTIMEVALUE原本是实时表
            //string sql = "select  D_VALUE,t_tag from T_INFO_HISTVALUE  where  T_TAG in(" + str + ")";
    //        string sql = "select  D_VALUE,t_tag from T_INFO_HISTVALUE where T_time between to_date('" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','yyyy-MM-dd HH24:MI:SS') " +
    //"and  to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS')";

                //dt = dl.RunDataTable(sql, out errMsg);
                //DataView dtView = new DataView(dt);
            //DateTime dt1 = DateTime.Now;
            for (int i = 0; i < str_shuzu.Length; i++)
            {

                string sql = "select * from (select * from t_info_histvalue where T_TAG='" + str_shuzu[i] + "' and " +
"t_time>to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS')  and  t_time<=to_date('" + DateTime.Now + "','yyyy-MM-dd HH24:MI:SS') order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
                dt = dl.RunDataTable(sql, out errMsg);
                if ((dt.Rows.Count > 0) && (dt.Rows[0]["D_VALUE"].ToString().Trim() != ""))
                {
                    str_sb += Convert.ToDouble(dt.Rows[0]["D_VALUE"].ToString()).ToString("f2") + ",";
                }
                else
                {
                    str_sb += "0.0,";
                }
            }
            //DateTime dt2 = DateTime.Now;
            //double num = (dt2 - dt1).TotalSeconds;
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        for (int j = 0; j < str_shuzu.Length; j++)
                //        {
                //            if (dt.Rows[i][1].ToString() == str_shuzu[j])
                //            {
                //                str_shuzu[j] = dt.Rows[i][0].ToString();
                //                break;
                //            }
                //        }
                //    }

                    //for (int k = 0; k < str_shuzu.Length; k++)
                    //{
                    //    str_sb += Convert.ToDouble(str_shuzu[k]).ToString("f2") + ",";
                    //}
                       
                //}

            //string[] str = new string[82] { "JSQP_A2_001Q", "JSQP_A3_001Q", "JSQP_A4_001Q", "JSQP_A2_001Q.F", "JSQP_A3_001Q.F", "JSQP_A4_001Q.F", 
            //    "JSQP_A2_001Q.P", "JSQP_A3_001Q.P", "JSQP_A4_001Q.P", "JSQP_A2_001Q.G", "JSQP_A3_001Q.G", "JSQP_A4_001Q.G", "JSQP_A2_003NQ001.D", "JSQP_A2_003NQ001.S",
            //    "JSQP_A2_004NQ001.D", "JSQP_A2_004NQ001.S", "JSQP_A2_NQ001.D", "JSQP_A2_NQ001.S", "JSQP_A3_005NQ001.D", "JSQP_A3_005NQ001.S", "JSQP_A3_006NQ001.D", 
            //    "JSQP_A3_006NQ001.S", "JSQP_A3_NQ001.D", "JSQP_A3_NQ001.S", "JSQP_A4_NQ001.D", "JSQP_A4_NQ001.S","JSQP_A6_001Q", "JSQP_A7_001Q", "JSQP_A8_001Q",
            //    "JSQP_A6_001Q.F", "JSQP_A7_001Q.F", "JSQP_A8_001Q.F", "JSQP_A6_001Q.P", "JSQP_A7_001Q.P", "JSQP_A8_001Q.P", "JSQP_A6_001Q.G", "JSQP_A7_001Q.G", 
            //    "JSQP_A8_001Q.G", "JSQP_A6_001NQ001.D", "JSQP_A6_001NQ001.S", "JSQP_A6_002NQ001.D", "JSQP_A6_002NQ001.S", "JSQP_A6_NQ001.D", "JSQP_A6_NQ001.S", 
            //    "JSQP_A7_NQ001.D", "JSQP_A7_NQ001.S", "JSQP_A8_NQ001.D", "JSQP_A8_NQ001.S" ,"JSQP_C1_001Q", "JSQP_C2_001Q", "JSQP_C1_001Q.F", "JSQP_C2_001Q.F",
            //    "JSQP_C1_001Q.P","JSQP_C2_001Q.P", "JSQP_C1_001Q.G", "JSQP_C2_001Q.G", "JSQP_C1_NQ001.D", "JSQP_C1_NQ001.S", "JSQP_C2_NQ001.D", "JSQP_C2_NQ001.S",
            //    "JSQP_C3_001Q", "JSQP_C4_001Q", "JSQP_C5_001Q", "JSQP_C3_001Q.F", "JSQP_C4_001Q.F", "JSQP_C5_001Q.F", "JSQP_C3_001Q.P", "JSQP_C4_001Q.P",
            //    "JSQP_C5_001Q.P", "JSQP_C3_001Q.G", "JSQP_C4_001Q.G", "JSQP_C5_001Q.G", "JSQP_C3_007NQ001.D", "JSQP_C3_007NQ001.S", "JSQP_C3_008NQ001.D", "JSQP_C3_008NQ001.S",
            //    "JSQP_C3_NQ001.D", "JSQP_C3_NQ001.S", "JSQP_C4_NQ001.D", "JSQP_C4_NQ001.S", "JSQP_C5_NQ001.D", "JSQP_C5_NQ001.S"};
            
            //for (int i = 0; i < str.Length; i++)
            //{
            //    //str_sb += str[i]+",";
            //    string sql = "select  D_VALUE from T_INFO_REALTIMEVALUE  where  T_TAG='" + str[i] + "' order by T_TIME desc ";

            //    dt = dl.RunDataTable(sql, out errMsg);
            //    if (dt.Rows.Count > 0)
            //    {
            //        str_sb += Convert.ToDouble(dt.Rows[0][0].ToString()).ToString("f2") + ",";
            //    }
            //    else
            //    {
            //        str_sb += "1.0,";
            //    }
            //}
            Response.Write(str_sb.Trim(','));
            Response.End();
            Response.Clear();
        }
    }
}