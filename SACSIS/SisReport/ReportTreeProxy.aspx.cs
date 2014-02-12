using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BLL;
using Microsoft.Office.Interop.Owc11;

namespace SACSIS.SisReport
{
    public partial class ReportTreeProxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"].ToLower(), paras = "";

            var rptName = HttpUtility.UrlDecode(Request["name"] ?? string.Empty);
            var qTime = Request["time"] ?? string.Empty;
            var orgId = Request["orgid"] ?? string.Empty;
            var tid = Request["treeid"] ?? string.Empty;
            var rptid = Request["rptid"] ?? string.Empty;

            //点查询按钮
            if (param == "query")
            {
                try
                {
                    var bll = new BLL.BLLReport();
                    var styleObj = bll.GetRptStyle(rptid, tid, orgId);
                    var style = styleObj.RptStyle;
                    var dType = styleObj.styleType;

                    if (dType == "D")
                        qTime = DateTime.Parse(qTime).ToString("yyyy-MM-dd 0:00:00");
                    else if (dType == "M" || dType == "MM")
                        qTime = DateTime.Parse(qTime).ToString("yyyy-MM-01 0:00:00");
                    else if (dType == "Y" || dType == "YY")
                        qTime = qTime + "-01-01 0:00:00";

                    if (!string.IsNullOrEmpty(style))
                    {
                        string msg;

                        var ht = GetRptData(dType, rptid, orgId, tid, Convert.ToDateTime(qTime), out msg);
                        CreateReport(ht, style);
                    }
                    else
                    { Response.Write("<span>此机构暂无该报表<span>"); }
                }
                catch (System.Exception ex)
                {
                    Response.Write("<span>" + ex.Message + "<span>");
                }
            }
            else if (param == "isdate")
            {
                var bll = new BLL.BLLReport();
                var styleObj = bll.GetRptStyle(rptid, tid, orgId);
                var dateType = styleObj.styleType;

                object obj = new
                {
                    dateType = dateType
                };

                string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                Response.Write(result);
                Response.End();
            }
        }

        /// <summary>
        /// 获取报表
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="orgId"></param>
        /// <param name="treeId"></param>
        /// <param name="dt"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Hashtable GetRptData(string dType, string rptId, string orgId, string treeId, DateTime dt, out string message)
        {
            var bll = new BLL.BLLReport();
            return bll.GetRptData(dType, rptId, orgId, treeId, dt, out message);
        }

        /// <summary>
        /// 获取报表[风机故障]
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="orgId"></param>
        /// <param name="treeId"></param>
        /// <param name="dt"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Hashtable GetRptData(string dType, string rptId, string orgId, string treeId, string param, DateTime dt, out string message)
        {
            var bll = new BLL.BLLReport();
            return bll.GetRptData(dType, rptId, orgId, treeId, param, dt, out message);
        }

        private void CreateReport(IDictionary idc, string rptStyle)
        {
            var sp1 = new SpreadsheetClass();

            var doc = new XmlDocument();
            doc.LoadXml(rptStyle);
            var xn = new XmlNamespaceManager(doc.NameTable);
            xn.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            var nodes = doc.SelectNodes("//ss:Data", xn);

            //根据key替换数据
            foreach (XmlNode node in nodes)
            {
                var tmpValue = node.InnerText.Trim();
                if (tmpValue.Length > 2 && tmpValue.Substring(0, 1) == "♂")
                {
                    string val = tmpValue.Substring(1);
                    node.InnerText = idc.Contains(val) ? idc[val].ToString() : string.Empty;
                    rptStyle = rptStyle.Replace(tmpValue, node.InnerText);
                }
            }

            //sp1.XMLData = style;

            sp1.XMLData = doc.OuterXml;
            //dvShow.InnerHtml = sp1.HTMLData;

            sp1.XMLData = sp1.XMLData.ToString().Replace("<ss:Table", "<ss:Table ss:id=\"CX\"");

            Response.Write(sp1.HTMLData);
        }

        private void CreateReport2(IDictionary idc, string rptStyle)
        {
            var sp1 = new SpreadsheetClass();

            var doc = new XmlDocument();
            doc.LoadXml(rptStyle);
            var xn = new XmlNamespaceManager(doc.NameTable);
            xn.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            var nodes = doc.SelectNodes("//ss:Data", xn);

            //根据key替换数据
            foreach (XmlNode node in nodes)
            {
                var tmpValue = node.InnerText.Trim();
                if (tmpValue.Length > 2 && tmpValue.Substring(0, 1) == "♂")
                {
                    node.InnerText = idc.Contains(tmpValue) ? idc[tmpValue].ToString() : string.Empty;
                    //style = style.Replace(tmpValue, node.InnerText);
                }
            }

            //sp1.XMLData = style;

            sp1.XMLData = doc.OuterXml;

            var fileName = "e:\\rpt.xls";  //可以考虑把保存文件路径写成配置的
            sp1.Export(fileName, SheetExportActionEnum.ssExportActionOpenInExcel);

        }

        private void CreateReport(IDictionary idc, string rptName, string treeId, string orgId)
        {
            var bll = new BLL.BLLReport();
            //var styleObj = bll.GetRptStyleByName(rptName);
            var styleObj = bll.GetRptStyle(rptName, treeId, orgId);
            var style = styleObj.RptStyle;
            if (!string.IsNullOrEmpty(style))
            {
                var sp1 = new SpreadsheetClass();

                var doc = new XmlDocument();
                doc.LoadXml(style);
                var xn = new XmlNamespaceManager(doc.NameTable);
                xn.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
                var nodes = doc.SelectNodes("//ss:Data", xn);

                //根据key替换数据
                foreach (XmlNode node in nodes)
                {
                    var tmpValue = node.InnerText.Trim();
                    if (tmpValue.Length > 2 && tmpValue.Substring(0, 1) == "♂")
                    {
                        node.InnerText = idc.Contains(tmpValue) ? idc[tmpValue].ToString() : string.Empty;
                        //style = style.Replace(tmpValue, node.InnerText);
                    }
                }

                //sp1.XMLData = style;

                sp1.XMLData = doc.OuterXml;
                //dvShow.InnerHtml = sp1.HTMLData;
                sp1.XMLData = sp1.XMLData.Replace("ss:Table", "ss:Table ss:id=\"CX\"");
                Response.Write(sp1.HTMLData);
            }
            else
            {
                //Response.Write("<script>alert('set style first.')</script>");
                //dvShow.InnerHtml = "报表样式未设置，请先设置报表样式";
                Response.Write("<span>请先设置报表样式<span>");
            }
        }
    }
}