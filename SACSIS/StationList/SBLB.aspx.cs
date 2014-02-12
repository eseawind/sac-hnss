using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Collections;

namespace SACSIS.StationList
{
    public partial class SBLB : System.Web.UI.Page
    {
        private StringBuilder st = new StringBuilder();
        private static DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"]; string id = "";
            
            if ((param != "") && (param != null))
            {
                GetInit(param);
            }
        }


        #region 初始化数据
        //初始化风机信息
        private void GetInit(string id)
        {
            BLL.BLLSBLB sb = new BLL.BLLSBLB();
            ArrayList ld= sb.GetPOINTS_UNIT(id);
           
            st.Append("<table height=\"100%\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            st.Append("<tr><td colspan=\"2\" style=\"background-image:url(../img/SY_CZ_bg_1.jpg); height:5px;\"></td></tr>");
            st.Append("<tr><td><div><table height=\"100%\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            for (int i = 1; i <= ld.Count; i++)
            {
                if ((i == 0) && (i % 5 == 0))
                {
                    st.Append("<tr>");
                }
                //else if ((i % 5 == 1)&&(i!=1))
                //{
                //    st.Append("<tr>");
                //}
                st.Append("<td class=\"div_1\"><div class=\"div_top_1\">#" +(i) + "</div><div class=\"div_top_2\">" + sb.GetPointValue(ld[i-1].ToString()) + "</td>");
                if((i!=0)&& (i % 5 == 0))
                {
                    st.Append("</tr><tr>");
                }
                
            }
            
                
             
            st.Append("</tr></table></div></td></tr></table>");

            object obj = new
            {
                tb = st.ToString()
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion
    }
}