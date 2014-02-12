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
    public partial class ManageRoleUser : System.Web.UI.Page
    {
        BLLRole br = new BLLRole();
        object obj = null;
        private static DataTable dt = new DataTable();
        private static string PTreeNodes = "";
        DataTable dtb = new DataTable();
        private static string roleId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"];
            if (param != null)
            {
                if (param == "LoadRoles")
                {
                    GetRolesList();
                }
                else if (param == "seachList")//选择树中的节点时运行
                {
                    string roleId = Request.Form["id"];//选定的节点的ID
                    int page = Convert.ToInt32(Request["page"].ToString());
                    int rows = Convert.ToInt32(Request["rows"].ToString());
                    GetUserByRole(roleId, page, rows);
                }
                else if (param == "JudgeMember")
                {
                    string userID = Request.Form["userID"];
                    judgeMember(userID);
                }
                else if (param == "AddMember")
                {
                    string userID = Request.Form["userID"];
                    string userName = HttpUtility.UrlDecode(Request["userName"]);
                    string pwd = HttpUtility.UrlDecode(Request["pwd"]);
                    string path = HttpUtility.UrlDecode(Request["img"]);
                    string treeNodeId = Request.Form["treeNodeId"];//当前角色的ID
                    AddMember(userID, userName, pwd, path, treeNodeId);
                }
                else if (param == "Edit")
                {
                    string id = Request.Form["id"];
                    GetMemberInfo(id);
                }
                else if (param == "EditMemberInfo")
                {
                    string userIDO = Request.Form["oldId"];
                    string userID = Request.Form["id"];
                    string userName = HttpUtility.UrlDecode(Request.Form["name"]);
                    string pwd = HttpUtility.UrlDecode(Request["pwd"]);
                    string path = HttpUtility.UrlDecode(Request["img"]);
                    string treeNodeId = Request.Form["treeNodeId"];//当前组织机构的ID
                    EditMember(userIDO, userID, userName, pwd, path, treeNodeId);
                }
                else if (param == "Remove")
                {
                    string id = Request["id"].ToString();
                    RemoveMember(id);
                }
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

        #region 根据角色找到所有用户
        private void GetUserByRole(string id, int page, int rows)
        {
            string result = "";
            DataTable dtb = null;
            dtb = br.GetUserMenuByRole(id, (page - 1) * rows + 1, page * rows);
            int count = br.GetUserCountByRole(id);
            IList<Hashtable> list = new List<Hashtable>();
            foreach (DataRow row in dtb.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("key", row["ID_KEY"].ToString());
                ht.Add("id", row["T_USERID"].ToString());
                ht.Add("name", row["T_USERNAME"].ToString());
                list.Add(ht);
            }
            object obj = new
            {
                total = count,
                rows = list
            };
            result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
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

        #region 根据用户名判断是否存在该人员
        private void judgeMember(string userID)
        {
            int count = 0;
            if (br.JudgMember(userID))
                count = 1;
            obj = new
            {
                judge = count
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 添加人员
        private void AddMember(string id, string name, string pwd, string path, string treeNodeId)
        {
            string resultInfo = "";
            if (treeNodeId != null && treeNodeId != "")
            {
                if (path != null && path != "")
                {
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
                    BinaryReader brr = new BinaryReader(fs);
                    byte[] imgBytesIn = brr.ReadBytes((int)fs.Length);  //将流读入到字节数组中
                    if (br.AddMember(id, name, pwd, imgBytesIn, treeNodeId))
                        resultInfo = "人员添加成功!";
                    else
                        resultInfo = "人员添加失败!";
                }
                else
                {
                    if (br.AddMember(id, name, pwd, null, treeNodeId))
                        resultInfo = "人员添加成功!";
                    else
                        resultInfo = "人员添加失败!";
                }
            }
            else
            {
                resultInfo = "请选择一个角色!";
            }
            obj = new
            {
                info = resultInfo
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();

        }
        #endregion

        #region 获取编辑的人员信息
        public void GetMemberInfo(string id)
        {
            IList<Hashtable> listMembers = br.GetmemberInfo(id, 0);
            IList<Hashtable> list = null;
            string imgs = "";
            if (listMembers != null)
            {
                list = br.GetmemberInfo(id, 1);
                Hashtable htb = new Hashtable();
                htb = listMembers[0];

                if (htb["B_ATTACHMENT"] != null && htb["B_ATTACHMENT"].ToString() != "")
                {
                    byte[] imgBytes = (byte[])(htb["B_ATTACHMENT"]);

                    string filePath = "../Files/" + htb["T_USERID"] + ".jpg";
                    imgs = filePath;
                    filePath = Server.MapPath(filePath);
                    BinaryWriter bw = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate));
                    bw.Write(imgBytes);
                    bw.Close();
                }

            }
            obj = new
            {
                img = imgs,
                list = list,
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 编辑人员信息
        private void EditMember(string userIDO, string userID, string userName, string pwd, string path, string treeNodeId)
        {
            bool res = false;
            string resultInfo = "";
            byte[] imgBytesIn = null;
            if (path != null && path != "")
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
                BinaryReader brr = new BinaryReader(fs);
                imgBytesIn = brr.ReadBytes((int)fs.Length);  //将流读入到字节数组中
            }
            res = br.EditMemberInfo(userIDO, userID, userName, pwd, imgBytesIn, treeNodeId);

            if (res)
                resultInfo = "人员编辑成功!";
            else
                resultInfo = "人员编辑失败!";
            obj = new
            {
                info = resultInfo
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 删除人员信息
        /// <summary>
        /// 删除人员信息
        /// </summary>
        /// <param name="id">人员编码</param>
        private void RemoveMember(string id)
        {
            string resultInfo = "";
            bool res = br.RemoveMember(id);
            if (res)
                resultInfo = "人员删除成功!";
            else
                resultInfo = "人员删除失败!";
            obj = new
            {
                info = resultInfo
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

    }
}