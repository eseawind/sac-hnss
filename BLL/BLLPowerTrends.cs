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
    public class BLLPowerTrends
    {
        DALPowerTrends pt= new DALPowerTrends();
        DateHelper pb = new DateHelper();
        public IList<Hashtable> GetChartData(string id, string rating_time)
        {

            return pt.GetChartData(id,rating_time); ;
        }
    }
}
