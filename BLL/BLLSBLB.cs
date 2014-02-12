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
    public class BLLSBLB
    {
        DALSBLB pt = new DALSBLB();
        public ArrayList GetPOINTS_UNIT(string id)
        {
            return pt.GetPOINTS_UNIT(id);
        }

        public double GerUnitNum(string id)
        {
            return pt.GerUnitNum(id);
        }
        public string GetPointRealValue(string id)
        {
            return pt.GetPointRealValue(id);
        }
        public DataTable GetPoint(string id)
        {
            return pt.GetPoint(id);
        }
        public double GetPointValue(string id)
        {
            return pt.GetPointValue(id);
        }
        public double GetElcValue(string id, string plant_id)
        {
            return pt.GetElcValue(id,plant_id);
        }
        public int GetPointNum(string id)
        {
            return pt.GetPointNum(id);
        }
    }
}
