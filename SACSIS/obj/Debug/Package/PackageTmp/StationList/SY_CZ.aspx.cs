using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Data;

namespace SACSIS.StationList
{
    public partial class SY_CZ : System.Web.UI.Page
    {
        string plant_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            plant_id = Request["plant_id"];
            if ((plant_id != "") && (plant_id != null))
            {
                GetPlant(plant_id);

            }
        }
        private void GetPlant(string id)
        {
            string fz = "",fs="",wd="",sd="",gl="";
            BLL.BLLSBLB sb = new BLL.BLLSBLB();
            
            StringBuilder st = new StringBuilder();
            DataTable dt = sb.GetPoint(id);
            string str = sb.GetPointRealValue(id);
            if (str != "")
            {
                gl = str.Split(',')[0];
                fz = str.Split(',')[1];
                sd=str.Split(',')[2];
                wd=str.Split(',')[3];
                fs = str.Split(',')[4];
            }
            string s = "{\"gl\":[" + str.Split(',')[0] + "]}";
            BLL.BLLDefault df = new BLL.BLLDefault();
            string d_dl = sb.GetElcValue("1", id).ToString("f0");
           // string m_dl = sb.GetElcValue("2", id).ToString("f0");
            string y_dl = sb.GetElcValue("3", id).ToString("f0");
            string day = "{\"day\":[" + d_dl + "]}";
            //string mon = "{\"mon\":[" + m_dl + "]}";
            string rz = "{\"rz\":[" + y_dl + "]}";
            double rl= df.GetCapacityByDepid("t_plantid ='"+id+"'",DateTime.Now);
            double num = sb.GerUnitNum("t_plantid ='" + id + "'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                st.Append("<input name=\"btn" + i + 1 + "\" type=\"button\" value=\"" + dt.Rows[i]["t_patitionname"].ToString() + "\" class=\"button\" onclick=\"query('" + dt.Rows[i]["T_PATITIONID"].ToString() + "')\" />");
            }
            object obj = new
            {
                fj_count = num,
                fz = fz,//辐照
                sd = sd,//湿度
                wd = wd,//温度
                fs = fs,//风速
                rl =rl,//容量
                gl = gl,
                glb = s,
                 d_dl = d_dl,
            // m_dl = m_dl,
             y_dl = y_dl,
             day = day,
             //mon = mon,
             rz = rz,
                tb = st.ToString()
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
    }
}