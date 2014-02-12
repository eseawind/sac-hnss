using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace SACSIS.StationList
{
    public partial class GZJSQNBQ : System.Web.UI.Page
    {
        private StringBuilder st = new StringBuilder();
        private StringBuilder sb = new StringBuilder();
        private static DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"]; string id = "";

            if ((param != "") && (param != null))
            {
                GetInit(param);
            }
        }
        BLL.BLLDefault df = new BLL.BLLDefault();
        private void GetInit(string param)
        {
            dt = df.GetTagId(param);
            //dt = df.RealValueDT(param);
            st.Append("<table width=\"100%\" height=\"92%\" cellpadding=\"4\" cellspacing=\"0\" class=\"style5\" border=\"0\"><tr>");
            st.Append("<td background=\"../img/table-head.jpg\" height=\"30px\" valign=\"middle\" class=\"style6\" colspan=\"2\">");
            st.Append("&nbsp;&nbsp;逆变器参数</td></tr><tr>");
            st.Append("<td valign=\"top\"><table><tr style=\"font-size: 15px; background: #328CD3; color: White; font-weight: bold;width: 30%\">");
            st.Append("<td style=\"width: 25%\" align=\"center\">参数名称</td><td style=\"width: 20%\" align=\"center\">参数实时值</td><td style=\"width: 10%\" align=\"center\">单位</td></tr>");
            int num = 0;
            if (dt.Rows.Count % 2 == 0)
            {
                num = dt.Rows.Count / 2;
            }
            else
            {
                num = (dt.Rows.Count -1) / 2 +1;
            }
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (j < num)
                {
                    if (j % 2 == 0)
                    {
                        st.AppendFormat("<tr style=\"font-size: 14px; color: #48317C; font-weight: bold;\" align=\"left\"><td>&nbsp;&nbsp;{0}</td><td><input type=\"text\" id=\"Text{1}\" value=\"{2}\" /></td><td>{3}</td></tr>", dt.Rows[j]["t_tagdesc"].ToString(), j,df.RealValueDT(dt.Rows[j]["t_tagid"].ToString()), "");
                    }
                    else
                    {
                        st.AppendFormat("<tr style=\"font-size: 14px;background-color: #F8FBFC; color: #48317C; font-weight: bold;\" align=\"left\"><td>&nbsp;&nbsp;{0}</td><td><input type=\"text\" id=\"Text{1}\" value=\"{2}\" /></td><td>{3}</td></tr>", dt.Rows[j]["t_tagdesc"].ToString(), j, df.RealValueDT(dt.Rows[j]["t_tagid"].ToString()), "");
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        sb.AppendFormat("<tr style=\"font-size: 14px; color: #48317C; font-weight: bold;\" align=\"left\"><td>&nbsp;&nbsp;{0}</td><td><input type=\"text\" id=\"Text{1}\" value=\"{2}\" /></td><td>{3}</td></tr>", dt.Rows[j]["t_tagdesc"].ToString(), j, df.RealValueDT(dt.Rows[j]["t_tagid"].ToString()), "");
                    }
                    else
                    {
                        sb.AppendFormat("<tr style=\"font-size: 14px;background-color: #F8FBFC; color: #48317C; font-weight: bold;\" align=\"left\"><td>&nbsp;&nbsp;{0}</td><td><input type=\"text\" id=\"Text{1}\" value=\"{2}\" /></td><td>{3}</td></tr>", dt.Rows[j]["t_tagdesc"].ToString(), j, df.RealValueDT(dt.Rows[j]["t_tagid"].ToString()), "");
                    }
                }
            }

            st.Append("</table></td>");
            st.Append("<td valign=\"top\"><table><tr style=\"font-size: 15px; background: #328CD3; color: White; font-weight: bold;width: 30%\">");
            st.Append("<td style=\"width: 25%\" align=\"center\">参数名称</td><td style=\"width: 20%\" align=\"center\">参数实时值</td><td style=\"width: 10%\" align=\"center\">单位</td></tr>");
            st.Append(sb.ToString());
            st.Append("</table></td></tr></table>");

            object obj = new
            {
                tb = st.ToString()
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
    }
}