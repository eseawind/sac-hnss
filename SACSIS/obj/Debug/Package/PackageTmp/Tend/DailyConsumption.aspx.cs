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
    public partial class DailyConsumption : System.Web.UI.Page
    {
        private IList<Hashtable> list = new List<Hashtable>();
        StringBuilder sb = new StringBuilder();
        string id = "", Range = "", rating_time = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request["id"]; Range = Request["Range"];
            rating_time = Request["rating_time"];
            if ((Range != "") && (Range != null))
            {
                GetData(id);
            }
            if (!IsPostBack)
            {
                //this.stime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                this.stime.Value = DateTime.Now.ToString("yyyy-MM-01");
                this.etime.Value = DateTime.Now.ToString("yyyy-MM-dd");
                this.stime1.Value = DateTime.Now.ToString("yyyy-01");
                this.etime1.Value = DateTime.Now.ToString("yyyy-MM");
                this.stime2.Value = DateTime.Now.ToString("yyyy");
            }
        }
        private void GetData(string fb_id)
        {

            if (rating_time == "")
            {
                if (Range == "1")
                {
                    rating_time = DateTime.Now.AddDays(-DateTime.Now.Day + 1) + "," + DateTime.Now;
                }
                else if (Range == "2")
                {
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    rating_time = dt + "," + DateTime.Now;
                }
                else
                {
                    DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
                    rating_time = dt + "," + DateTime.Now;
                }
            }
            else
            {
                if (Range == "1")
                {
                    rating_time = rating_time.Split(',')[0] + " 00:00:00" + "," + rating_time.Split(',')[1] + " 23:59:59";
                }
                else if (Range == "2")
                {
                    DateTime dt = new DateTime(Convert.ToInt32(rating_time.Split(',')[1].Split('-')[0]), Convert.ToInt32(rating_time.Split(',')[1].Split('-')[1]),DateTime.DaysInMonth(Convert.ToInt32(rating_time.Split(',')[1].Split('-')[0]), Convert.ToInt32(rating_time.Split(',')[1].Split('-')[1])));
                    rating_time = rating_time.Split(',')[0] + "-01 00:00:00" + "," + dt.ToString();
                }
                else
                {
                    if (rating_time == DateTime.Now.Year.ToString())
                    {
                        rating_time = rating_time + "-01-01 00:00:00" + "," + DateTime.Now;
                    }
                    else
                    {
                        rating_time = rating_time + "-01-01 00:00:00" + "," + rating_time + "-12-31 23:59:59";
                    }
                }
            }
            ArrayList ldd = new ArrayList();
            BLL.BLLDailyConsumption gz = new BLL.BLLDailyConsumption();
            list = gz.GetChartData(Range,fb_id, rating_time,out ldd);
            object obj = new
            {
                x_data=ldd,
                list = list
            };
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();

        }

    }
}