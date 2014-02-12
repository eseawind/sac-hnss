using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Collections;
using SAC.Model;

namespace BLL
{
    public class BLLReport
    {
        /// <summary>
        /// 保存报表样式
        /// </summary>
        /// <param name="rptStyle">报表对象</param>
        /// <returns></returns>
        public string SaveRptStyle(ReportStyle rptStyle)
        {
            int count = 0;
            string errMsg = "";

            if (rptStyle.RptName.Trim() == string.Empty)
            {
                return "报表名不能为空";
            }
            //if (string.IsNullOrEmpty(rptStyle.OrgId) || string.IsNullOrEmpty(rptStyle.TreeId))
            //{
            //    return "组织机构树和组织机构单元不能为空";
            //}

            var dal = new DAL.DALReportStyle();

            //count = dal.GetCountByTreeID(rptStyle.TreeId, rptStyle.RptName, out errMsg);

            //if (count > 0)
            //    return "报表名称已存在,请修改名称!";

            return dal.SaveRptStyle(rptStyle);
        }

        /// <summary>
        /// 根据报表NAME获取样式
        /// 查找不到返回空
        /// </summary>
        /// <param name="rptName">报表Name</param>
        /// <returns></returns>
        public ReportStyle GetRptStyleByName(string rptName)
        {
            var dal = new DAL.DALReportStyle();
            var dt = dal.GetRptStyleByName(rptName);

            var rpt = new ReportStyle
            {
                RptName = rptName,
            };

            if (dt.Rows.Count > 0)
            {
                rpt.RptName = dt.Rows[0]["REPORTNAME"].ToString();
                rpt.RptStyle = dt.Rows[0]["REPORTSTYLE"].ToString().Replace("&quot;", "\"");
            }

            return rpt;
        }

        /// <summary>
        /// 根据报表名，treeid，orgid查找报表样式
        /// </summary>
        /// <param name="rptName"></param>
        /// <param name="treeId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ReportStyle GetRptStyle(string rptId, string treeId, string orgId)
        {
            var dal = new DAL.DALReportStyle();
            var dt = dal.GetRptStyle(rptId, treeId, orgId);

            var rpt = new ReportStyle
            {
                RptID = rptId,
            };

            if (dt.Rows.Count > 0)
            {
                rpt.RptName = dt.Rows[0]["REPORTNAME"].ToString();
                rpt.RptStyle = dt.Rows[0]["REPORTSTYLE"].ToString().Replace("&quot;", "\"");
                rpt.OrgId = orgId;
                rpt.TreeId = treeId;
                rpt.styleType = dt.Rows[0]["reporttype"].ToString();
            }
            return rpt;
        }

        /// <summary>
        /// 根据报表ID获取样式
        /// </summary>
        /// <param name="rptID"></param>
        /// <returns></returns>
        public ReportStyle GetRptStyleByID(string rptID)
        {
            var dal = new DAL.DALReportStyle();
            
            var dt = dal.GetRptStyleByID(rptID);

            var rpt = new ReportStyle
            {
                RptID = rptID,
            };

            if (dt.Rows.Count > 0)
            {
                rpt.RptName = dt.Rows[0]["REPORTNAME"].ToString();
                rpt.RptStyle = dt.Rows[0]["RPTSTYLE"].ToString().Replace("&quot;", "\"");
            }

            return rpt;
        }

        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="orgId"></param>
        /// <param name="treeId"></param>
        /// <param name="dt"></param>
        /// <param name="message"></param>
        /// <param name="flag"></param>
        /// <param name="isZy"></param>
        /// <returns></returns>
        public Hashtable GetRptData(string dType,string rptId, string orgId, string treeId, DateTime dt,
                                    out string message, bool flag = false, bool isZy = false)
        {
            DAL.DALSisReport dal = new DALSisReport();

            return dal.RetHtRep(dType, rptId, orgId, treeId,"", flag, isZy, dt, out message);
        }            

        //***** The New *****//

        /// <summary>
        /// 获取组织机构下属报表ID 
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public IList<Hashtable> GetStyleIList(string treeID, out string errMsg)
        {
            errMsg = "";
            DALReportStyle drps = new DALReportStyle();

            IList<Hashtable> list = drps.GetStyleIList(treeID, out errMsg);

            return list;
        }
       
        /// <summary>
        /// 获取报表数据[风机故障]
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="orgId"></param>
        /// <param name="treeId"></param>
        /// <param name="periodID"></param>
        /// <param name="uintID"></param>
        /// <param name="fanBrands"></param>
        /// <param name="dt"></param>
        /// <param name="message"></param>
        /// <param name="flag"></param>
        /// <param name="isZy"></param>
        /// <returns></returns>
        public Hashtable GetRptData(string dType,string rptId, string orgId, string treeId,string param, DateTime dt, out string message, bool flag = false, bool isZy = false)
        {
            DAL.DALSisReport dal = new DALSisReport();

            return dal.RetHtRep(dType,rptId, orgId, treeId, param, flag, isZy, dt, out message);
        }
    }

}
