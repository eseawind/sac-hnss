using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Web
{
    public partial class Login : System.Web.UI.Page
    {
        BLL.BLLRole bl = new BLL.BLLRole();
        string errMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //登录按钮
        protected void btnSure_Click(object sender, EventArgs e)
        {
            string UserName = this.UserName.Text;
            string PassWordinput = this.PassWord.Text;
            this.UserName.Text = "";
            this.PassWord.Text = "";
            if (UserName == "")
            {
                Response.Write("<script>alert('请输入用户名！')</script>");
            }
            else if (PassWordinput == "")
            {
                Response.Write("<script>alert('请输入密码！')</script>");
            }
            if (UserName != "" && PassWordinput != "")
            {
                DataTable dt = null;
                dt = bl.GetUserInfo(UserName);
                if (dt.Rows.Count == 0 || dt == null)
                {
                    Response.Write("<script>alert('用户名不存在，请检查后重新输入！')</script>");
                }
                else 
                {
                    string PassWordReal = dt.Rows[0]["T_PASSWD"].ToString();
                    if (PassWordinput != PassWordReal)
                    {
                        Response.Write("<script>alert('密码不正确，请检查后重新输入！')</script>");
                    }
                    else if (PassWordinput == PassWordReal)
                    {
                        string userId = dt.Rows[0]["T_USERID"].ToString();
                        HttpCookie cookieID = new HttpCookie("ID_KEY", userId);
                        HttpCookie cookieUN = new HttpCookie("T_USERID", UserName);
                        Response.Cookies.Add(cookieID);
                        Response.Cookies.Add(cookieUN);
                        Server.Transfer("Main.aspx");
                    }                   
                }
            }

        }
    }
}
