using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Text;

namespace Web
{
    public partial class Main : System.Web.UI.Page
    {
        BLL.BLLRole bl = new BLL.BLLRole();
        private static DataTable dt = new DataTable();
        private StringBuilder sb = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"];
            if (param != "")
            {
            }
            if (!Page.IsPostBack)
            {
                DownLoadXml("Webmenu");//下载XML文件至指定路径
                GetMenuRoot();
            }

        }

        #region 获取横向导航菜单
        /// <summary>
        /// 获取横向导航菜单
        /// </summary>
        /// <returns></returns>
        public void GetMenuRoot()
        {
            string userId = Request.Cookies["T_USERID"].Value.ToString();
            string roleId = bl.GetRoleId(userId);
            DataTable udt = bl.GetUserInfo(userId);
            string realName = udt.Rows[0]["T_USERNAME"].ToString();
            //BLLRole bl = new BLLRole();
            //string errMsg = "";
            //string realName = bl.GetUserRealNameById(userId, out errMsg);
            dt = new DataTable();
            GetTreeList();
            DataRow[] dr = null;
            dr = dt.Select("PID='00'");
            int c = 0;
            for (int i = 0; i < dr.Length; i++)
            {
                string[] nodeRoleId = dr[i][5].ToString().TrimStart(',').TrimEnd(',').Split(',');
                if (nodeRoleId.Contains(roleId))
                {
                    c = c + 1;
                    DataRow[] _dr = dt.Select("PID='" + dr[i][0]+"'");
                    if (_dr.Length > 0)
                    {
                        //if (i == 0)
                        //    sb.AppendFormat("<a id=\"menu{4}\" runat=\"server\"  href=\"javascript:SetUrl('{0}','{1}','{2}')\" onclick=\"changeMenu({4})\" hidefocus=\"true\">{3}</a>", 0, dr[i][0], dr[i][6], dr[i][1], c);
                        //else
                        sb.AppendFormat("<a id=\"menu{4}\" runat=\"server\"  href=\"javascript:SetUrl('{0}','{1}','{2}')\" onclick=\"changeMenu({4})\" hidefocus=\"true\">{3}</a>", 1, dr[i][0], dr[i][6], dr[i][1], c);
                    }
                    else
                    {
                        if (i == 0)
                            sb.AppendFormat("<a id=\"menu{4}\" runat=\"server\"  href=\"javascript:SetUrl('{0}','{1}','{2}')\" onclick=\"changeMenu({4})\" hidefocus=\"true\">{3}</a>", 0, dr[i][0], dr[i][6], dr[i][1], c);
                        else
                            sb.AppendFormat("<a id=\"menu{4}\" runat=\"server\"  href=\"javascript:SetUrl('{0}','{1}','{2}')\" onclick=\"changeMenu({4})\" hidefocus=\"true\">{3}</a>", 0, dr[i][0], dr[i][6], dr[i][1], c);
                    }
                }

            }
            this.lblUserWelcome.Text = realName;
            this.mu.Value = (c + 1).ToString();
            this.dv_Menu_left.InnerHtml = sb.ToString();
        }
        #endregion

        #region 获取 DatTable Tree

        /// <summary>
        /// 获取 DatTable Tree
        /// </summary>
        private void GetTreeList()
        {
            XmlTextReader reader = new XmlTextReader(
              Server.MapPath("xml/WebMenu.xml"));

            reader.WhitespaceHandling = WhitespaceHandling.None;
            XmlDocument xmlDoc = new XmlDocument();
            //将文件加载到XmlDocument对象中
            xmlDoc.Load(reader);

            //关闭连接
            reader.Close();

            XmlNode xnod = xmlDoc.DocumentElement;

            if (!dt.Columns.Contains("ID"))
                dt.Columns.Add("ID");
            if (!dt.Columns.Contains("NAME"))
                dt.Columns.Add("NAME");
            if (!dt.Columns.Contains("PID"))
                dt.Columns.Add("PID");
            if (!dt.Columns.Contains("ORDER"))
                dt.Columns.Add("ORDER");
            if (!dt.Columns.Contains("VISIBLE"))
                dt.Columns.Add("VISIBLE");
            if (!dt.Columns.Contains("OWER"))
                dt.Columns.Add("OWER");
            if (!dt.Columns.Contains("FILNAME"))
                dt.Columns.Add("FILNAME");
            XmlToDataTable(xnod);
        }
        #endregion

        #region 将XML转换成 DataTable
        /// <summary>
        /// 将XML转换成 DataTable
        /// </summary>
        /// <param name="xnod"></param>
        /// <param name="intLevel"></param>
        private void XmlToDataTable(XmlNode xnod)
        {
            DataRow dr = dt.NewRow();
            XmlNode xnodWorking;

            //如果是元素节点，获取它的属性
            if (xnod.NodeType == XmlNodeType.Element)
            {
                XmlNamedNodeMap mapAttributes = xnod.Attributes;
                if (mapAttributes.Count > 0)
                {
                    dr[0] = mapAttributes.Item(0).Value;
                    dr[1] = mapAttributes.Item(1).Value;
                    dr[2] = mapAttributes.Item(2).Value;
                    dr[3] = mapAttributes.Item(3).Value;
                    dr[4] = mapAttributes.Item(4).Value;
                    dr[5] = mapAttributes.Item(5).Value;
                    dr[6] = mapAttributes.Item(6).Value;
                    dt.Rows.Add(dr);
                }

                //如果还有子节点，就递归地调用这个程序
                if (xnod.HasChildNodes)
                {
                    xnodWorking = xnod.FirstChild;
                    while (xnodWorking != null)
                    {
                        XmlToDataTable(xnodWorking);
                        xnodWorking = xnodWorking.NextSibling;
                    }
                }
            }
        }
        #endregion

        #region 判断两个数组中是否有重复的元素，如果有则返回true
        protected bool IfRepeat(string[] A, string[] B)
        {
            bool repeat = false;
            int na = A.Count();
            int nb = B.Count();
            int mix = 0;
            for (int i = 0; i < nb; i++)
            {
                for (int j = 0; j < na; j++)
                {
                    if (A[j] == B[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 下载XML文件至指定路径
        protected void DownLoadXml(string xmlID)//下载XML
        {
            if (!bl.IsEmptyXml())
            {
                string xmlpath = Server.MapPath("xml/WebMenu.xml");
                bool ret = bl.DownLoadXml("WebMenu", xmlpath);
            }
        }
        #endregion

        protected void linkBtnLogout_Click(object sender, EventArgs e)
        {
            Request.Cookies.Clear();
            Response.Write("<script type='text/javascript'>top.window.location.href='Login.aspx'; </script>");//整个主页面跳转到登录界面
        }

    }
}