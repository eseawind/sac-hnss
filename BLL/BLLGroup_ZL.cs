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
    public class BLLGroup_ZL
    {
        DALGroup_Zl gz = new DALGroup_Zl();
        DateHelper pb = new DateHelper();
        public IList<Hashtable> GetChartData(string id)
        {
            return gz.GetChartData(id);
        }
        public IList<Hashtable> GetLineData(string id)
        {
            return gz.GetLineData(id);
        }
    }
}
