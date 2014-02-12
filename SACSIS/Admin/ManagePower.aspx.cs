using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Collections;
using Newtonsoft.Json;
using BLL;
using System.Text;
using System.IO;

namespace SACSIS.Admin
{
    public partial class ManagePower : System.Web.UI.Page
    {
        BLL.BLLRole br = new BLLRole();
        object obj = null;
        private static DataTable dt = new DataTable();
        private static DataTable dtm = new DataTable();
        private static string PTreeNodes = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"];
            if (param != null)
            {
                if (param == "LoadRoles")
                {
                    GetRolesList();
                }
                else if (param == "LoadMenu")
                {
                    string roleName = Request.Form["rName"];
                    string roleId = Request.Form["rId"];
                    GetTreeList(roleName,roleId);
                }
                else if (param == "SaveMenu")
                {
                    string roleId = Request.Form["rId"];
                    string menuId = Request.Form["mId"];
                    SavePower(roleId,menuId);
                }
            }
            if (!Page.IsPostBack)
            {
                DownLoadXml("Webmenu");//下载XML文件至指定路径
            }
        }

        #region 得到角色列表
        private void GetRolesList()
        {
            StringBuilder st = new StringBuilder();
            string ifJuage = "";
            string resultMenu = "";
            dt = br.GetRoleList();
            DataTable dtClass = new DataTable();
            IList<Hashtable> listMembers = br.GetMembers();

            if (dt != null && dt.Rows.Count > 0)
            {
                st.Append("[");
                st.Append("{id:'qianwanbunengdengyu1',pId:'0',name:'全部角色',t:'不存在人员', open:true},");//#1对应页面里的#1
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ifJuage = judgeMemberByClassID(dt.Rows[i]["T_GRPID"].ToString());
                    st.Append("{id:'" + dt.Rows[i]["T_GRPID"] + "',pId:'qianwanbunengdengyu1',name:'" + dt.Rows[i]["T_GRPDESC"] + "',t:'" + ifJuage + "'},");
                }
               
                resultMenu = st.ToString().Substring(0, st.ToString().Length - 1) + "]";
                obj = new
                {
                    count = 1,
                    menu = resultMenu
                };
                string result = JsonConvert.SerializeObject(obj);
                Response.Write(result);
                Response.End();
            }
            else
            {
                obj = new
                {
                    count = 0
                };
                string result = JsonConvert.SerializeObject(obj);
                Response.Write(result);
                Response.End();
            }
        }
        #endregion

        #region 获取 DatTable Tree

        /// <summary>
        /// 获取 DatTable Tree
        /// </summary>
        private void GetTreeList(string roleName,string roleId)
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
            dt = new DataTable();
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
            DataTable dts = dt;
            GetTreeNodesParentID(dts, pIDs, roleId);
            GetTreeNodeSunList(dts, pIDs,roleId);
            pIDs = "000";

            tr = PTreeNodes;
            PTreeNodes = "";
            dt = new DataTable();

            tr = tr.Substring(0, tr.Length - 1);
            tr = "[" + tr + "]";
            xmlDoc = null;
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

        /// <summary>
        /// 获取父类节点Tree
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TreeNodeId"></param>
        public void GetTreeNodesParentID(DataTable dt, string TreeNodeId,string roleId)
        {
            DataRow[] irows = null;
            irows = dt.Select("ID='" + TreeNodeId + "'");
            if (irows.Length > 0)
            {
                string ifChecked = "false";
                string[] val = irows[0][5].ToString().Split(',');
                for (int i = 0; i < val.Length; i++)
                {
                    if (roleId == val[i])
                    {
                        ifChecked = "true";

                        break;
                    }
                }
                PTreeNodes += "{id:'" + irows[0][0] + "',pId:'" + irows[0][2] + "',name:'" + irows[0][1] + "',order:'" + irows[0][3] + "',visible:'" + irows[0][4] + "',FileName:'" + irows[0][6] + "',t:'" + irows[0][3] + "',open:true,checked:" + ifChecked + "},";
                GetTreeNodesParentID(dt, irows[0][2].ToString(),roleId);
            }
        }
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
                    string ifChecked = "false";
                    string[] val = irows[k][5].ToString().Split(',');
                    for (int i = 0; i < val.Length; i++)
                    {
                        if (roleId == val[i])
                        {
                            ifChecked = "true";
                            break;
                        }
                    }
                    Hashtable ht_1 = new Hashtable();
                    //icon:"../img/right1.bmp
                    PTreeNodes += "{id:'" + irows[k][0] + "',pId:'" + irows[k][2] + "',name:'" + irows[k][1] + "',order:'" + irows[k][3] + "',visible:'" + irows[k][4] + "',FileName:'" + irows[k][6] + "',t:'" + irows[k][3] + "',checked:" + ifChecked + "},";
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

        #region 保存修改好的xml
        private void SavePower(string roleId, string menuId)
        {
            string message = "";
            string nOwner = "";
            XmlDocument xmldoc = new XmlDocument();
            string xmlpath = Server.MapPath("../xml/WebMenu.xml");
            xmldoc.Load(xmlpath);
            string[] cMenuList = menuId.Split(',');
            for (int i = 0; i < xmldoc.ChildNodes[1].ChildNodes.Count; i++)
            {
                string[] oldOwner = xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("owner").Value.Split(',');//本菜单项旧owner
                if (cMenuList.Contains(xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("id").Value))//判断本菜单项是被选中的菜单项
                {
                    if (!oldOwner.Contains(roleId))//如果原来owner中不存在本角色，则添加本角色,如果原来owner中已经存在本角色，则什么都不做
                    {
                        nOwner = xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("owner").Value + roleId + ",";
                        xmldoc.ChildNodes[1].ChildNodes[i].Attributes["owner"].Value = nOwner;
                        xmldoc.Save(xmlpath);
                        UpdataXml(xmlpath);
                        nOwner = "";
                    }
                }
                else//如果本菜单项不是被选中的菜单
                {
                    if (oldOwner.Contains(roleId))//如果原来owner中已经存在本角色，如果原来owner中不存在本角色，则什么都不做
                    {
                        string oOwner = xmldoc.ChildNodes[1].ChildNodes[i].Attributes.GetNamedItem("owner").Value;
                        string del = roleId + ",";
                        nOwner = Convert.ToString(oOwner.Replace(del, ""));//则将本角色从本菜单项中删除
                        xmldoc.ChildNodes[1].ChildNodes[i].Attributes["owner"].Value = nOwner;
                        xmldoc.Save(xmlpath);
                        UpdataXml(xmlpath);
                        nOwner = "";
                    }
                }
                
            }
            message = "权限分配成功！";
            object obj = new
            {
                message = message
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 判断两个字符串数组是否存在相同元素
        private bool IsRepeatString(string[] a, string[] b)
        {
            bool flag=false;
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    if (a[i] == b[j])
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == true)
                {
                    break;
                }
            }
            return flag;
        }
        #endregion

        #region 判断某个岗位下面是否存在人员
        /// <summary>
        /// 判断某个岗位下面是否存在人员
        /// </summary>
        /// <param name="id">人员编码</param>
        private string judgeMemberByClassID(string id)
        {
            string ifJudge = "";
            bool res = br.JudgMemberByORGId(id);
            if (res)
                ifJudge = "存在人员";
            else
                ifJudge = "不存在人员";
            return ifJudge;
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
            BinaryReader brr = new BinaryReader(fs);

            byte[] imgBytesIn = brr.ReadBytes((int)fs.Length);  //将流读入到字节数组中

            bool flag = br.UpdataWebMenuXml(imgBytesIn);
            //bool flag2 = XmlForm(sender, e);
            imgBytesIn = null;
            fs.Close();
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

        #region 下载XML文件至指定路径
        protected void DownLoadXml(string xmlID)//下载XML
        {
            if (!br.IsEmptyXml())
            {
                string xmlpath = Server.MapPath("../xml/WebMenu.xml");
                bool ret = br.DownLoadXml("WebMenu", xmlpath);
            }
        }
        #endregion
    }
}