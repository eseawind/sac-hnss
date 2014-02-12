using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data;
using BLL;


namespace SACSIS.Admin
{
    public partial class ManageRole : System.Web.UI.Page
    {
        BLLRole br = new BLLRole();
        IList<Hashtable> list = null;
        object obj = null;
        int count = 0;
        string errMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"];
            if (param != "")
            {
                if (param == "seachList")
                {
                    GetRoleList();
                }
                else if (param == "Add")
                {
                    string id = Request.Form["id"];
                    string name = Request.Form["name"];
                    btnSure_Click(id, name, "");
                }
                else if (param == "Edit")
                {
                    string id = Request["id"].ToString();
                    string name = Request["name"].ToString();
                    string oid = Request["oid"].ToString();
                    btnSure_Click(id, name, oid);
                }
                else if (param == "Remove")
                {
                    string id = Request["id"].ToString();
                    gridBC_RowDeleting(id);
                }
            }
        }

        protected void btnSure_Click(string roleId, string roleName, string OroleId)
        {
            string message = "";
            try
            {
                if (OroleId == "")
                {
                    if (br.SaveRole(roleId,roleName,out errMsg))
                    {
                        message = "添加角色成功！";
                    }
                    else
                    {
                        message = "添加角色失败！请检查是否存在相同的职别编号！";
                    }
                }
                else
                {
                    if (br.UpDateRole(OroleId,roleId,roleName,out errMsg))
                    {
                        message = "修改角色成功";
                    }
                    else
                    {
                        message = "修改角色失败！请检查是否存在相同的角色编号！";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "添加角色失败！失败信息：" + ex.Message.ToString();
            }

            obj = new
            {
                message = message
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();

        }

        protected void gridBC_RowDeleting(string rId)
        {
            //string BcId = GridBC.DataKeys[e.RowIndex].Value.ToString();

            string message = "";
            if (br.DeleteRole(rId,out errMsg))
            {
                message = "删除角色成功";
            }
            else
            {
                message = "修改角色失败！请检查是否存在相同的班次名！";
            }
            obj = new
            {
                message = message
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }

        #region 所有角色信息
        public void GetRoleList()
        {
            int page = Convert.ToInt32(Request["page"].ToString());
            int rows = Convert.ToInt32(Request["rows"].ToString());
            DataTable dt = br.GetAllRole((page - 1) * rows + 1, page * rows);
            count = br.GetRoleCount();
            IList<Hashtable> list = new List<Hashtable>();

            foreach (DataRow item in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("ID_KEY", item["ID_KEY"]);
                ht.Add("T_GRPID", item["T_GRPID"].ToString());
                ht.Add("T_GRPDESC", item["T_GRPDESC"].ToString());
                list.Add(ht);
            }
            object obj = new
            {
                total = count,
                rows = list
            };

            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion
    }
}