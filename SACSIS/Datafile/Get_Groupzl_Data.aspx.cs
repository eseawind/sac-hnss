using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace SACSIS.Datafile
{
    public partial class Get_Groupzl_Data : System.Web.UI.Page
    {
        public string id = "", Reaload_id="",fb_id="",num="";
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"]; Reaload_id = Request["Reaload_id"]; fb_id = Request["fb_id"]; num = Request["num"];
            StringBuilder sb = new StringBuilder();
            if ((id != "") && (id != null))
            {
                BLL.BLLDefault BD = new BLL.BLLDefault();
                sb.Append(BD.GetCapacity(DateTime.Now).ToString() + "," + BD.GetReaload(id, 1).ToString() + "," + BD.GetPower("1=1", DateTime.Now).ToString());
                
            }
            else if ((Reaload_id != "") && (Reaload_id != null))
            {
                BLL.BLLDefault BD = new BLL.BLLDefault();
                sb.Append(BD.GetReaload(Reaload_id,Convert.ToInt32( num)).ToString());
            }
            else if ((fb_id != "") && (fb_id != null))
            {
                string str ="";
                if ((fb_id.Split(',')[1] != "") || (fb_id.Split(',')[2] != ""))
                {
                    if (fb_id.Split(',')[1] != "")
                    {
                        str += " t_base_plant.T_COMID='" + fb_id.Split(',')[1] + "'";
                    }
                    if ((fb_id.Split(',')[2] != "") && (fb_id.Split(',')[1] != ""))
                    {
                        str += " and t_base_plant.T_DEPID='" + fb_id.Split(',')[2] + "'";

                    }
                    else if (fb_id.Split(',')[2] != "")
                    {
                        str += " t_base_plant.T_DEPID='" + fb_id.Split(',')[2] + "'";
                    }
                }
                else
                {
                    str += " 1=1";
                }
                BLL.BLLDefault BD = new BLL.BLLDefault();
                sb.Append(BD.GetCapacityByDepid(str, DateTime.Now).ToString() + "," + BD.GetReaload("t_orgid='" + fb_id.Split(',')[1] + "'", 1).ToString() + "," + BD.GetPower(str, DateTime.Now).ToString());

            }
            Response.Clear();
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}