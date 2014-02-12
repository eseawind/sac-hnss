using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Collections;
using System.Text;

namespace SACSIS.ProductOverview
{
    public partial class fb_zl : System.Web.UI.Page
    {
        private IList<Hashtable> list = new List<Hashtable>();
        StringBuilder sb = new StringBuilder();
        string Groupzl_id = "", div_id = "", Range = "", Line_id = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Groupzl_id = Request["Groupzl_id"]; div_id = Request["id"]; Range = Request["Range"];
            Line_id = Request["Line_id"];
            if ((Groupzl_id != "") && (Groupzl_id != null))
            {
                GetData(Groupzl_id);
            }
            if ((Line_id != "") && (Line_id != null))
            {
                GetLineData(Line_id);
            }
        }
        private void GetData(string fb_id)
        {
            BLL.BLLGroup_ZL gz = new BLL.BLLGroup_ZL();
            list = gz.GetChartData(Range);
            object obj = new
            {
                name = div_id,
                title = fb_id,
                list = list
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();

        }
        private void GetLineData(string fb_id)
        {
            BLL.BLLGroup_ZL gz = new BLL.BLLGroup_ZL();
            list = gz.GetLineData(Range);
            ArrayList a_list = new ArrayList();
            foreach (Hashtable _ht in list)
            {

                string _data = _ht["name"].ToString();
                a_list.Add(_data);
                //Hashtable _dv1 = new Hashtable();
                //_dv1.Add("lineColor", str2[num1]);
                //_dv1.Add("title", "{text:''}");
                ////_dv.Add("opposite", true);//Y轴右端显示
                //_dv1.Add("lineWidth", 1);
                //list1.Add(_dv1);
                //num1++;

            }
            object obj = new
            {
                x_data = a_list,
                name = div_id,
                title = fb_id,
                list = list
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();

        }
    }
}