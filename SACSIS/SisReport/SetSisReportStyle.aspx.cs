using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BLL;

namespace SACSIS.SisReport
{
    public partial class SetSisReportStyle : System.Web.UI.Page
    {
        BLL.BLLReport rep = new BLL.BLLReport();
        protected void Page_Load(object sender, EventArgs e)
        {
            string treeID = "", param = Request["param"];

            if (Request["treeID"] != null)
                treeID = Request["treeID"];

            //if (treeID != "")
            //{
                if (param == "GetIdList")
                {
                    this.GetIList(treeID);// GetIList(treeID);
                }
            //}
        }

        //获取风机期数据
        private void GetIList(string treeID)
        {
            int count = 0;
            string errMsg = "";

            IList<Hashtable> list = rep.GetStyleIList(treeID, out errMsg);

            if (list.Count > 0)
            {
                count = 1;
            }

            object obj = new
            {
                count = count,
                list = list
            };

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
    }
}