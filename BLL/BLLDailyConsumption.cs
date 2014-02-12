using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
using SAC.Helper;


namespace BLL
{
    public class BLLDailyConsumption
    {
        DALDailyConsumption pt = new DALDailyConsumption();
        DateHelper pb = new DateHelper();
        public IList<Hashtable> GetChartData(string id, string Range, string rating_time, out ArrayList ldd)
        {

            return pt.GetChartData(id,Range,rating_time,out ldd);
        }
    }
}
