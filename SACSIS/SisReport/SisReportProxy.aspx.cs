using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAC.Model;
using Newtonsoft.Json;

namespace SACSIS.SisReport
{
    public partial class SisReportProxy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var action = Request.Form["act"] ?? string.Empty;
            var p = Request.Form["param"] ?? string.Empty;
            var result = new ExecResult();
            try
            {
                result = Execute(action, p);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrMessage = "客户端参数错误：" + ex.Message;
                throw;
            }

            var strResult = JsonConvert.SerializeObject(result);
            Response.Clear();
            Response.Write(strResult);
            Response.End();
        }

        private ExecResult Execute(string action, string data)
        {
            var result = new ExecResult();
            switch (action.ToUpper())
            {
                case "SAVESTYLE":
                    var rptData = JsonConvert.DeserializeObject<ReportStyle>(data);
                    result = SaveStyle(rptData);
                    break;
                case "GETTAGDETAIL":
                    result = GetTagDetail(data);
                    break;
                case "GETSPELL":
                    result = GetSpell();
                    break;
                case "GETSTYLE":
                    result = GetRptStyle(JsonConvert.DeserializeObject<ReportStyle>(data));
                    break;
                default:
                    break;
            }

            return result;
        }

        private ExecResult SaveStyle(ReportStyle rpt)
        {
            var bll = new BLL.BLLReport();
            var s = bll.SaveRptStyle(rpt);
            var objResult = new ExecResult()
            {
                Status = true,
            };
            if (s != string.Empty)
            {
                objResult.ErrMessage = s;
                objResult.Status = false;
            }

            return objResult;

        }

        private ExecResult GetTagDetail(string tagID)
        {
            ExecResult result = new ExecResult();

            return result;
        }

        private ExecResult GetSpell()
        {
            ExecResult result = new ExecResult();

            return result;
        }

        private ExecResult GetRptStyle(ReportStyle report)
        {
            //var rptName = report.RptName;
            var rptId = report.RptID;
            var treeId = report.TreeId;
            var orgId = report.OrgId;
            var bll = new BLL.BLLReport();
            var rpt = bll.GetRptStyle(rptId, treeId, orgId);

            var result = new ExecResult()
            {
                ResultData = JsonConvert.SerializeObject(rpt),
            };

            return result;
        }
    }
}