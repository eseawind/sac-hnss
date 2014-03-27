using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using SAC.DBOperations;
namespace SACSIS.WebService
{
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
     [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string GetFHByTimeGF(string pName)
        {
            DBLink dl = new DBLink();
            string fh = "[";

            string dtZr =DateTime.Now.ToString("yyyy-MM-dd 0:00:00").ToString();
            string dtJr = DateTime.Now.ToString();


            string sql = "select max(D_VALUE),to_char(T_time,'HH24') s2 from t_info_histvalue where " +
 "T_TAG=(select t_base_points_org.T_POWERTAG from t_base_points_org where " +
   " t_base_points_org.t_orgid='" + pName + "' )  and " +
     "T_TIME between to_date('" + dtZr + "','yyyy-MM-dd HH24:MI:SS') and " +
      "to_date('" + dtJr + "','yyyy-MM-dd HH24:MI:SS') group by to_char(T_time,'HH24') order by s2 asc";

            string errMsg = "";
            DataTable dt = dl.RunDataTable(sql,out errMsg);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < Convert.ToDouble(dt.Rows[i][1].ToString()); j++)
                        {
                            fh += "null,";
                        }
                        fh += Convert.ToDouble( dt.Rows[i][0].ToString()).ToString("f2")+",";
                    }
                    else
                    {
                        for (int j = 0; j < (Convert.ToDouble(dt.Rows[i][1].ToString()) - Convert.ToDouble(dt.Rows[i-1][1].ToString())) -1; j++)
                        {
                            fh += "null,";
                        }
                        fh += Convert.ToDouble(dt.Rows[i][0].ToString()).ToString("f2") + ",";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        for (int j = 0; j < (24 - Convert.ToDouble(dt.Rows[i][1].ToString()))-1; j++)
                        {
                            fh += "null,";
                        }
                    }
                    
                }
            }
            fh = fh.TrimEnd(',') + "]";

            return "{\"fh\":" + fh + "}";
        }
    }
}
