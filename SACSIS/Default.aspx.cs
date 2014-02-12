using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SACSIS
{
    public partial class Defalut : System.Web.UI.Page
    {
        public string  User_id="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Cookies["T_USERID"] != null)
            {
                string id = HttpContext.Current.Request.Cookies["T_USERID"].Value;
                 BLL.BLLRole br = new BLL.BLLRole();
                 User_id = br.GetRoleId(id);
            }
            BLL.BLLDefault BD = new BLL.BLLDefault();
            this.lbl_Capacity.Text = BD.GetCapacity(DateTime.Now).ToString();
            this.lbl_Reaload.Text = BD.GetReaload("1=1", 1).ToString();
            this.lbl_Power.Text = BD.GetPower("1=1",DateTime.Now).ToString();
        }
    }
}