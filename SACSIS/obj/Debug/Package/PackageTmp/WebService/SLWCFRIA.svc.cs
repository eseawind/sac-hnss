using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using SAC.DBOperations;
using System.Data;

namespace SACSIS.WebService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SLWCFRIA 
    {
        [OperationContract]
        public string GetData(string id)
        {
            DBLink dl = new DBLink();
            string errMsg = "";
            string sql = "select * from t_info_graphic where t_graphicid ='1'";
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
