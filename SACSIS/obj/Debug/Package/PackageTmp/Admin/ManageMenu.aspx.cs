using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Collections;
using System.IO;

namespace SACSIS.Admin
{
    public partial class ManageMenu : System.Web.UI.Page
    {
        string webpath = "~/Admin/ManageMenu.aspx";
        private static DataTable dt = new DataTable();
        private static string PTreeNodes = "";
        private static string sTreeNodePId = "";
        private static string judge = "";
        BLL.BLLRole bl = new BLL.BLLRole();
        public static string blackUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"];
            if (param != null)
            {
                if (param == "showTree")
                {
                    string treeNodeId = Request.Form["id"];
                    if (treeNodeId == "")
                    {
                        sTreeNodePId = "";
                    }
                    else
                    {
                        sTreeNodePId = treeNodeId + ",";
                    }
                    BtnSort(sender,e);//对xml文档进行排序
                    GetTreeList(treeNodeId);
                }
                else if (param == "Add")
                {
                    string pId = Request.Form["pId"];
                    string name = Request.Form["name"];
                    string order = Request.Form["order"];
                    string url = Request.Form["url"];
                    string vis = Request.Form["vis"];
                    BtnAdd_Click(pId,name,order,url,vis);
                }
                else if (param == "Edit")
                {
                    string pId = Request.Form["pId"];
                    string id = Request.Form["id"];
                    string name = Request.Form["name"];
                    string order = Request.Form["order"];
                    string url = Request.Form["url"];
                    string vis = Request.Form["vis"];
                    BtnEdit_Click(pId, id, name, order, url, vis);
                }
                else if (param == "Del")
                {
                    string id = Request.Form["id"];
                    BtnDel_Click(id);
                }
            }
            if (!Page.IsPostBack)
            {
                //DownLoadXml("Webmenu");//下载XML文件至指定路径
            }
        }

        #region 获取 DatTable Tree

        /// <summary>
        /// 获取 DatTable Tree
        /// </summary>
        private void GetTreeList(string treeNodeId)
        {
            string tr = "";
            string pIDs = "000";
            XmlTextReader reader = new XmlTextReader(
              Server.MapPath("../xml/WebMenu.xml"));

            reader.WhitespaceHandling = WhitespaceHandling.None;
            XmlDocument xmlDoc = new XmlDocument();
            //将文件加载到XmlDocument对象中
            xmlDoc.Load(reader);
            pIDs = xmlDoc.ChildNodes[1].ChildNodes[0].Attributes["id"].Value.ToString();
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
            xnod = null;
            DataTable dts = dt;
            
            GetSTreeNodesParentID(dts, treeNodeId);
            string[] sTreeNodePIds = sTreeNodePId.ToString().TrimEnd(',').Split(',');
            GetTreeNodesParentID(dts, pIDs);
            GetTreeNodeSunList(dts, pIDs, sTreeNodePIds);
            pIDs = "000";

            tr = PTreeNodes;
            PTreeNodes = "";
            dt = new DataTable();

            tr = tr.Substring(0, tr.Length - 1);
            tr = "[" + tr + "]";

            object obj = new
            {
                menu = tr,
                //trID = TreeID,
                //orgID = pIDs
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 获取菜单字符串

        public void GetSTreeNodesParentID(DataTable dt, string sTreeNodeId)//获取被点击菜单项的所有父级id
        {
            DataRow[] irows = null;
            irows = dt.Select("ID='" + sTreeNodeId + "'");
            if (irows.Length > 0)
            {
                for (int k = 0; k < irows.Length; k++)
                {
                    sTreeNodePId = sTreeNodePId + irows[k][2] + ",";
                    GetSTreeNodesParentID(dt, irows[k][2].ToString());
                }
            }
        }

        /// <summary>
        /// 获取父类节点Tree
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TreeNodeId"></param>
        public void GetTreeNodesParentID(DataTable dt, string TreeNodeId)
        {
            DataRow[] irows = null;
            irows = dt.Select("ID='" + TreeNodeId + "'");
            if (irows.Length > 0)
            {
                PTreeNodes += "{id:'" + irows[0][0] + "',pId:'" + irows[0][2] + "',name:'" + irows[0][1] + "',order:'" + irows[0][3] + "',visible:'" + irows[0][4] + "',FileName:'" + irows[0][6] + "',t:'" + irows[0][3] + "',open:true},";
                GetTreeNodesParentID(dt, irows[0][2].ToString());
            }
        }
        /// <summary>
        /// 获取子类节点Tree
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TreeNodeId"></param>
        public void GetTreeNodeSunList(DataTable dt, string TreeNodeId, string[] sTreeNodePIds)
        {

            DataRow[] irows = null;
            irows = dt.Select("PID='" + TreeNodeId + "'");
            if (irows.Length > 0)
            {
                for (int k = 0; k < irows.Length; k++)
                {
                    Hashtable ht_1 = new Hashtable();
                    //icon:"../img/right1.bmp
                    if (sTreeNodePIds.Contains(irows[k][0]))
                    {
                        PTreeNodes += "{id:'" + irows[k][0] + "',pId:'" + irows[k][2] + "',name:'" + irows[k][1] + "',order:'" + irows[k][3] + "',visible:'" + irows[k][4] + "',FileName:'" + irows[k][6] + "',t:'" + irows[k][3] + "',open:true},";
                    }
                    else
                    {
                        PTreeNodes += "{id:'" + irows[k][0] + "',pId:'" + irows[k][2] + "',name:'" + irows[k][1] + "',order:'" + irows[k][3] + "',visible:'" + irows[k][4] + "',FileName:'" + irows[k][6] + "',t:'" + irows[k][3] + "'},";
                    }
                    GetTreeNodeSunList(dt, irows[k][0].ToString(), sTreeNodePIds);
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

        #region 上传用户导入XML
        protected void BtnPutin_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            int count = 0;
            string info = "";
            string flPath = FileUpload1.PostedFile.FileName.ToString();
            FileStream fs = new FileStream(flPath, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);

            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中

            bool flag = bl.UpdataWebMenuXml(imgBytesIn);
            //bool flag2 = XmlForm(sender, e);
            bool flag2 = true;

            if (errMsg == "")
            {
                if (flag2 == true)
                {
                    info = "菜单文件导入成功!";
                    count = 1;
                    Response.Redirect(webpath);
                }
                else
                {
                    Response.Write("<script>alert('菜单文件导入失败!请仔细检查导入的xml文件是否存在格式错误');</script>");
                }
            }
            else
                info = errMsg;        
        }
        #endregion

        #region 添加菜单
        protected void BtnAdd_Click(string pId,string name,string order,string url,string vis)
        {
            string message = "";
            XmlDocument xmldoc = new XmlDocument();
            string xmlpath = Server.MapPath("../xml/WebMenu.xml");
            xmldoc.Load(xmlpath);
            double maxOrder = 0;
            double temp = 0;
            double maxId = 0;
            double tempId = 0;
            for (int i = 0; i < xmldoc.ChildNodes[1].ChildNodes.Count; i++)
            {
                tempId = Convert.ToDouble(xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("id").Value);
                if (tempId > maxId)
                {
                    maxId = tempId;
                }
                if (xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("pId").Value == pId)
                {
                    temp = Convert.ToDouble(xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("order").Value);
                    if (temp > maxOrder)
                    {
                        maxOrder = temp;
                    }
                }
            }
            if (order == "")
            {
                order = (maxOrder+1).ToString();
            }

            XmlElement nodeE = xmldoc.CreateElement("parment");
            //设置需要添加的节点的标示和各项属性
            nodeE.SetAttribute("id", (maxId+1).ToString());
            nodeE.SetAttribute("name", name);
            nodeE.SetAttribute("pId", pId);
            nodeE.SetAttribute("order", order);
            nodeE.SetAttribute("visible", vis);
            nodeE.SetAttribute("owner", ",");
            nodeE.SetAttribute("FileName", url);
            xmldoc.ChildNodes[1].AppendChild(nodeE);//添加节点
            xmldoc.Save(xmlpath);//保存xml文件
            UpdataXml(xmlpath);
            message = "菜单添加成功！";
            xmldoc = null;
            object obj = new
            {
                message = message
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
            
        }
        #endregion

        #region 编辑菜单
        protected void BtnEdit_Click(string pId,string id, string name, string order, string url, string vis)
        {
            string message = "";
            XmlDocument xmldoc = new XmlDocument();
            string xmlpath = Server.MapPath("../xml/WebMenu.xml");
            xmldoc.Load(xmlpath);
            double maxOrder = 0;
            double temp = 0;
            
            if (order == "")
            {
                if (pId != "")
                {
                    for (int i = 0; i < xmldoc.ChildNodes[1].ChildNodes.Count; i++)
                    {
                        if (xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("pId").Value == pId)
                        {
                            temp = Convert.ToDouble(xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("order").Value);
                            if (temp > maxOrder)
                            {
                                maxOrder = temp;
                            }
                        }
                    }
                }
                order = (maxOrder + 1).ToString();
            }
            for (int j = 0; j < xmldoc.ChildNodes[1].ChildNodes.Count; j++)
            {
                if (xmldoc.ChildNodes[1].ChildNodes[j].Attributes.GetNamedItem("id").Value == id)
                {
                    xmldoc.ChildNodes[1].ChildNodes[j].Attributes["name"].Value = name;
                    xmldoc.ChildNodes[1].ChildNodes[j].Attributes["order"].Value = order;
                    xmldoc.ChildNodes[1].ChildNodes[j].Attributes["visible"].Value = vis;
                    xmldoc.ChildNodes[1].ChildNodes[j].Attributes["FileName"].Value = url;
                    xmldoc.Save(xmlpath);
                    UpdataXml(xmlpath);
                    message = "菜单编辑成功！";
                    break;
                }
            }
            xmldoc = null;
            object obj = new
            {
                message = message
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();

        }
        #endregion

        #region 删除菜单
        protected void BtnDel_Click(string id)
        {
            string message = "";
            XmlDocument xmldoc = new XmlDocument();
            string xmlpath = Server.MapPath("../xml/WebMenu.xml");
            xmldoc.Load(xmlpath);
            bool HasChildNodes = false;
            int parmentId = 0;
            for (int i = 0; i < xmldoc.ChildNodes[1].ChildNodes.Count; i++)
            {
                if (xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("pId").Value == id)
                {
                    HasChildNodes=true;
                    break;
                }
            }
            for (int j = 0; j < xmldoc.ChildNodes[1].ChildNodes.Count; j++)
            {
                if (xmldoc.ChildNodes[1].ChildNodes[j].Attributes.GetNamedItem("id").Value == id)
                {
                    parmentId = j;
                    break;
                }
            }
            if (HasChildNodes == true)
            {
                message = "此菜单存在子菜单，请勿删除";
            }
            else if (HasChildNodes == false && xmldoc.ChildNodes[1].ChildNodes.Count <= 1)
            {
                message = "请勿删除根菜单";
            }
            else
            {
                xmldoc.ChildNodes[1].RemoveChild(xmldoc.ChildNodes[1].ChildNodes[parmentId]);
                xmldoc.Save(xmlpath);
                UpdataXml(xmlpath);
                message = "菜单删除成功！";
            }
            xmldoc = null;
            object obj = new
            {
                message = message
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 对菜单进行排序（排序规则为：相同父id的菜单，order值越小排名越靠前）
        protected void BtnSort(object sender, EventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            string xmlpath = Server.MapPath("../xml/WebMenu.xml");
            xmldoc.Load(xmlpath);
            for (int i = 0; i < xmldoc.ChildNodes[1].ChildNodes.Count; i++)
            {
                for (int j = i+1; j < xmldoc.ChildNodes[1].ChildNodes.Count; j++)
                {
                    if (xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("pId").Value == xmldoc.ChildNodes[1].ChildNodes[j].Attributes.GetNamedItem("pId").Value)
                    {
                        if (Convert.ToDouble(xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("order").Value) > Convert.ToDouble(xmldoc.ChildNodes[1].ChildNodes[j].Attributes.GetNamedItem("order").Value))
                        {
                            xmldoc.ChildNodes[1].InsertBefore(xmldoc.ChildNodes[1].ChildNodes[j], xmldoc.ChildNodes[1].ChildNodes[i]);
                        }
                    }
                }
            }
            xmldoc.Save(xmlpath);
            UpdataXml(xmlpath);
            xmldoc = null;
        }
        #endregion

        #region 下载XML文件至指定路径
        protected void DownLoadXml(string xmlID)//下载XML
        {
            if (!bl.IsEmptyXml())
            {
                string xmlpath = Server.MapPath("../xml/WebMenu.xml");
                bool ret = bl.DownLoadXml("WebMenu", xmlpath);
            }
        }
        #endregion

        #region 将修改好的XML文档上传至数据库
        protected void UpdataXml(string xmlpath)//上传XML
        {
            string errMsg = "";
            int count = 0;
            string info = "";
            string flPath = xmlpath;
            FileStream fs = new FileStream(flPath, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);

            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);  //将流读入到字节数组中

            bool flag = bl.UpdataWebMenuXml(imgBytesIn);
            imgBytesIn = null;
            fs.Close();
            //bool flag2 = XmlForm(sender, e);

            if (errMsg == "")
            {
                if (flag == true)
                {
                    info = "菜单文件导入成功!";
                    count = 1;
                }
                else
                {
                    Response.Write("<script>alert('菜单文件导入失败!请仔细检查导入的xml文件是否存在格式错误');</script>");
                }
            }
            else
                info = errMsg;       
        }
        #endregion
    }
}