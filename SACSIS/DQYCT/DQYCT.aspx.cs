using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SACSIS.DQYCT
{
    public partial class DQYCT : System.Web.UI.Page
    { public string Page_Url="";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Request.UrlReferrer.AbsoluteUri
            //string s = Request.QueryString.GetKey(0).ToString();
            //string page_url = HttpContext.Current.Request.Url.PathAndQuery;
            //aa = "MainPage";
            Page_Url = Request.QueryString.GetValues(0).GetValue(0).ToString();
            //Page_Url = "GZJSQDQT";
        }
    }
}