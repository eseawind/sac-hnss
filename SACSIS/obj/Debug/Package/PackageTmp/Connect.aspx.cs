using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Collections;
using BLL;

namespace Web
{
    public partial class Connect : System.Web.UI.Page
    {
        private static DataTable dt = new DataTable();
        BLL.BLLRole bl = new BLL.BLLRole();
        private string id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"];
            if (param != "")
            {
                if (param == "init")
                {
                    id = Request["id"];
                    GetLeftTree(id);
                }
                else if (param == "Tree")
                {
                    id = Request["id"];
                    GetSunTree(id);
                }
            }

        }

        public void GetLeftTree(string id)
        {
            string userId = Request.Cookies["T_USERID"].Value.ToString();
            string roleId = bl.GetRoleId(userId);
            dt = new DataTable();
            GetTreeList();
            DataRow[] _dr = dt.Select("PID='" + id + "'");

            IList<Hashtable> list = new List<Hashtable>();
            for (int i = 0; i < _dr.Length; i++)
            {
                string[] nodeRoleId = _dr[i][5].ToString().TrimStart(',').TrimEnd(',').Split(',');
                if (nodeRoleId.Contains(roleId))
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("ID", _dr[i][0].ToString());
                    ht.Add("NAME", _dr[i][1].ToString());
                    DataRow[] _dr_judge = dt.Select("PID='" + _dr[i][0].ToString() + "'");
                    if (_dr_judge.Length > 0)
                        ht.Add("JUDGE", "1");
                    else
                        ht.Add("JUDGE", "0");
                    list.Add(ht);
                }
            }

            object obj = new
            {
                list = list
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }

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

        #region 获取横向导航菜单
        /// <summary>
        /// 获取横向导航菜单
        /// </summary>
        /// <returns></returns>
        public DataRow[] GetMenuRoot()
        {
            DataRow[] dr = null;
            dr = dt.Select("PID='0'");
            return dr;
        }
        #endregion

        #region 获取子菜单

        private void GetSunTree(string id)
        {
            string treeNode = "";
            PTreeNodes = "";
            dt = new DataTable();
            GetTreeList();
            string userId = Request.Cookies["T_USERID"].Value.ToString();
            string roleId = bl.GetRoleId(userId);
            GetTreeNodeSunList(dt, id, roleId);

            treeNode = PTreeNodes;

            DataRow[] dr = null;
            string url = "";

            if (treeNode != "")
            {
                treeNode = treeNode.Substring(0, treeNode.Length - 1);
                treeNode = "[" + treeNode + "]";
            }
            else
            {
                dr = dt.Select("ID='" + id + "'");
                if (dr.Length > 0)
                    url = dr[0][6].ToString();
            }

            object obj = new
            {
                url = url,
                treeNode = treeNode
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }

        private static string PTreeNodes = "";
        /// <summary>
        /// 获取子类节点Tree
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TreeNodeId"></param>
        public void GetTreeNodeSunList(DataTable dt, string TreeNodeId,string roleId)
        {

            DataRow[] irows = null;
            irows = dt.Select("PID='" + TreeNodeId + "'");
            if (irows.Length > 0)
            {
                for (int k = 0; k < irows.Length; k++)
                {
                    Hashtable ht_1 = new Hashtable();
                    string[] nodeRoleId = irows[k][5].ToString().TrimStart(',').TrimEnd(',').Split(',');
                    if (nodeRoleId.Contains(roleId))
                    {
                        //icon:"../img/right1.bmp
                        PTreeNodes += "{id:'" + irows[k][0] + "',pId:'" + irows[k][2] + "',link:'" + irows[k][6] + "',name:'" + irows[k][1] + "',t:'" + irows[k][2] + "',icon:'../img/right1.bmp'},";
                    }
                    GetTreeNodeSunList(dt, irows[k][0].ToString(),roleId);
                }
            }
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

    }
}