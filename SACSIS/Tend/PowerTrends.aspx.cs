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


namespace SACSIS.Tend
{
    public partial class PowerTrends : System.Web.UI.Page
    {
        private IList<Hashtable> list = new List<Hashtable>();
        StringBuilder sb = new StringBuilder();
        string id = "", Range = "", rating_time="";
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"]; Range = Request["Range"];
            rating_time = Request["rating_time"];
            if ((id != "") && (id != null))
            {
                GetData(id);
            }
            if (!IsPostBack)
            {
                this.stime.Value = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToShortDateString().ToString() + " 00:00:00";
                this.etime.Value = DateTime.Now.ToString();

            }
        }

        private void GetData(string fb_id)
        {
            
            if (rating_time == "")
            {
                rating_time = DateTime.Now.AddDays(-DateTime.Now.Day + 1) + "," + DateTime.Now;
            }

            BLL.BLLPowerTrends gz = new BLL.BLLPowerTrends();
            list = gz.GetChartData(fb_id, rating_time);
            object obj = new
            {
                title = fb_id,
                list = list
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();

        }
    }
}