using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using SAC.DBOperations;
using System.Data;

/*----------------------------------------------------------------
// Copyright (C) 2014 南京华盾电力信息安全测评有限公司
// 版权所有。 
//
// 文件名：WebService

// 文件功能描述：数据获取

// 创建标识：刘海杰2014219
 * 
// 修改标识：

// 修改描述：

//----------------------------------------------------------------*/


namespace SACSIS.WebService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SLWCFRIA 
    {
        /// <summary>
        /// 功能描述
        /// 创建：刘海杰 日期：2014-02-19
        /// </summary>
        /// <param name="id">调用页面数据中配置的id</param>
        /// <returns>文本框id以及值</returns>
        [OperationContract]
        public string GetData(string id)
        {
            BLL.BLLDefault bll_df = new BLL.BLLDefault();
            DBLink dl = new DBLink();
            string errMsg = "";
            string sql = "select * from t_info_graphic where t_graphicid ='"+id+"'";


            DataTable dtt = new DataTable();//select * from t_info_graphic where t_graphicid ='1' order by t_controlid asc
            DataTable dt = dl.RunDataTable(sql, out errMsg);
            string str = "";
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {//select * from (select * from t_info_histvalue where T_TAG='GHPV_1_000WG001' order by t_time desc) where ROWNUM =1 order by ROWNUM asc
               // string sql_value = "select * from (select * from t_info_histvalue where T_TAG='" + dt.Rows[i]["t_tag"].ToString() + "' order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
                
                dtt = bll_df.GetDT(dt.Rows[i]["t_tag"].ToString(),DateTime.Now);
                if ((dtt.Rows.Count > 0) && (dtt.Rows[0]["D_VALUE"].ToString() !=""))
                {

                    str += dt.Rows[i]["t_controlid"].ToString() + "," + Convert.ToDouble( dtt.Rows[0]["D_VALUE"].ToString()).ToString("f2") + ";";
                }
                else
                {
                    str += dt.Rows[i]["t_controlid"].ToString() + ",0;";
                }

            }
            
            return str;
        }
        [OperationContract]
        public string GetDataWZ()
        {
            DBLink dl = new DBLink();
            string errMsg = "";
            string sql = "select * from t_info_graphic where t_graphicid ='2'";
            //           string sql ="select g.t_controlid,g.t_tag from t_info_histvalue h "+
            // "inner join t_info_graphic g on h.t_tag =g.t_tag "+
            //"and  g.t_graphicid ='1' group by  g.t_controlid,g.t_tag";


            DataTable dtt = new DataTable();//select * from t_info_graphic where t_graphicid ='1' order by t_controlid asc
            DataTable dt = dl.RunDataTable(sql, out errMsg);
            string str = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {//select * from (select * from t_info_histvalue where T_TAG='GHPV_1_000WG001' order by t_time desc) where ROWNUM =1 order by ROWNUM asc
                string sql_value = "select * from (select * from t_info_histvalue where T_TAG='" + dt.Rows[i]["t_tag"].ToString() + "' order by t_time desc) where ROWNUM =1 order by ROWNUM asc";
                DateTime dt1 = DateTime.Now;
                dtt = dl.RunDataTable(sql_value, out errMsg);
                DateTime dt2 = DateTime.Now;
                double num = (dt2 - dt1).TotalSeconds;
                if ((dtt.Rows.Count > 0) && (dtt.Rows[0]["D_VALUE"].ToString() != ""))
                {
                    str += dt.Rows[i]["t_controlid"].ToString() + "," + Convert.ToDouble(dtt.Rows[0]["D_VALUE"].ToString()).ToString("f2") + ";";
                }
                else
                {
                    str += dt.Rows[i]["t_controlid"].ToString() + ",0;";
                }

            }

            return str;
        }
    }
}
