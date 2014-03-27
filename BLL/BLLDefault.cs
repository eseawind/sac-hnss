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
    public class BLLDefault
    {
        DALDefault dr = new DALDefault();
        DateHelper pb = new DateHelper();
        #region 页面数据
        //获取总装机量-根据部门
        public double GetCapacityByDepid(string id, DateTime d_time)
        {
            return dr.GetCapacityByDepid(id, d_time);
        }
        public string RealValue(string id)
        {
            return dr.RealValue(id);

        }
        public string RealValueDT(string id)
        {
            return dr.RealValueDT(id);
        }

        public DataTable GetTagId(string id)
        {
            return dr.GetTagId(id);
        }
        //获取总装机量
        public double GetCapacity(DateTime d_time)
        {
            return dr.GetCapacity(d_time);
        }
        //获取实时负荷
        public double GetReaload(string id,int num)
        {
            return dr.GetReaload(id,num);
        }
        //获取总发电量
        public double GetPower(string id,DateTime d_time)
        {
            return dr.GetPower(id,d_time);
        }

        //获取总发电量
        public double GetPower(string id, DateTime d_time,int num)
        {
            return dr.GetPower(id, d_time,num);
        }

        public double GetPowerDianzhan(string id, string s_time, string e_time)
        {
            return dr.GetPowerDianzhan(id, s_time, e_time);
        }
        public double GetPowerByDianzhan(string id, DateTime d_time)
        {
            return dr.GetPowerByDianzhan(id, d_time);

        }

        public DataTable GetDT(string id, DateTime t_time)
        {
            return dr.GetDT(id,t_time);
        }
        #endregion

    }
}
