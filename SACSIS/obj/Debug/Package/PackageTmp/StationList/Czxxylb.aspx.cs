using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using BLL;
using System.Text;
using Newtonsoft.Json;
using SAC.DBOperations;

namespace SACSIS.StationList
{
    public partial class Czxxylb : System.Web.UI.Page
    {
        //private static string orgid = "10001";//"10034";
        //private static string xmlName = "";
        private string errMsg = "";
        //private int count = 0;

        //private StringBuilder _st = new StringBuilder();
        //private StringBuilder _st_body = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = Request["param"];
            if ((param != "")&&(param != null))
            {
                GetShowInfo(param);
            }
        }
        //BLL_STATISCS _bllst = new BLL_STATISCS();
        //UnitBLL _bllunit = new UnitBLL();
        //BasePointsOrgBLL _basebllpoint = new BasePointsOrgBLL();
        //PointBLL _bllPoint = new PointBLL();
        DBLink dl = new DBLink();
        BLL.BLLDefault df = new BLLDefault();
        private double GerUnitNum(string id)
        {
            double unit_num = 0;

            string sql_jtzb = "select count(*) from t_base_unit where t_patitionid in " +
"(select t_patitionid from t_base_partition where t_plantid " +
 "in(select T_PLANTID from t_base_plant where " + id + "))  and t_unittype ='NBQ'";
            DataTable dt = dl.RunDataTable(sql_jtzb,out  errMsg);

            if (dt.Rows[0][0].ToString().Trim() != "")
            {
                unit_num += Convert.ToDouble(dt.Rows[0][0].ToString());
            }
            return unit_num;
        }

        private void GetShowInfo(string para_id)
        {

            #region 初始化数据变量
            #endregion
            StringBuilder _stjt = new StringBuilder();
            _stjt.Append("<table border=\"1\" width=\"98%\" align=\"center\" class=\"bg\" valign=\"top\" bordercolorlight=\"#999999\" bordercolordark=\"#FFFFFF\" cellspacing=\"0\" cellpadding=\"1\">");
            _stjt.Append(" <tr bgcolor=\"#CFE6FC\" align=\"center\"><td height=\"28px\" align=\"center\" width=\"18%\">机构</td><td align=\"center\" width=\"8%\">装机容量(MW)</td><td align=\"center\" width=\"8%\">逆变器</td><td align=\"center\" width=\"8%\">有功功率(kW)</td><td align=\"center\" width=\"12%\">出力率</td><td align=\"center\" width=\"8%\">日发电量(kWh)</td><td align=\"center\" width=\"8%\">月发电量(kWh)</td><td align=\"center\" width=\"8%\">年发电量(kWh)</td></tr>");
            if (para_id == "C_JTZB")
            {
                _stjt.Append("<tr style=\"background-color:#FAD09C;\"><td>集团总部</td>");
                _stjt.Append("<td>"+df.GetCapacity(DateTime.Now)+"</td>");
                _stjt.Append("<td>" + GerUnitNum("1=1") + "</td>");
                //_stjt.Append("<td>0</td>");
                _stjt.Append("<td>" + df.GetReaload("1=1",1) + "</td>");
                _stjt.Append("<td> <div id=\"Div4\"> &nbsp;</div></td>");
                _stjt.Append("<td>" + df.GetPower("3=3", DateTime.Now,1) + "</td>");
                _stjt.Append("<td>" + df.GetPower("2=2", DateTime.Now,2) + "</td>");
                _stjt.Append("<td>" + df.GetPower("1=1", DateTime.Now) + "</td>");
                _stjt.Append("</tr>");
                string sql = "select * from T_BASE_COMPANY order by T_COMNAME desc";
                DataTable dt = dl.RunDataTable(sql,out errMsg);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        _stjt.Append("<tr style=\"background-color:#CAFFCE;\"><td>&nbsp;&nbsp;&nbsp;&nbsp;" + dt.Rows[i]["T_COMNAME"].ToString());
                        _stjt.Append("<td>" + df.GetCapacityByDepid("t_base_plant.t_comid ='" + dt.Rows[i]["T_COMID"].ToString() + "'", DateTime.Now) + "</td>");
                        _stjt.Append("<td>" + GerUnitNum("t_base_plant.t_comid ='" + dt.Rows[i]["T_COMID"].ToString() + "'") + "</td>");
                        //_stjt.Append("<td>0</td>");
                        _stjt.Append("<td>" + df.GetReaload("T_BASE_COMPANY.t_comid ='" + dt.Rows[i]["T_COMID"].ToString() + "'", 1) + "</td>");
                        _stjt.Append("<td> <div id=\"Div4\"> &nbsp;</div></td>");
                        _stjt.Append("<td>" + df.GetPower("t_base_plant.t_comid ='" + dt.Rows[i]["T_COMID"].ToString() + "'", DateTime.Now, 1) + "</td>");
                        _stjt.Append("<td>" + df.GetPower("t_base_plant.t_comid ='" + dt.Rows[i]["T_COMID"].ToString() + "'", DateTime.Now, 2) + "</td>");
                        _stjt.Append("<td>" + df.GetPower("t_base_plant.t_comid ='" + dt.Rows[i]["T_COMID"].ToString() + "'", DateTime.Now) + "</td>");
                        _stjt.Append("</tr>");
                        string sql2 = "select  * from t_base_plant  where t_comid ='" + dt.Rows[i]["T_COMID"].ToString() + "'";
                        DataTable dtt = dl.RunDataTable(sql2, out errMsg);
                        if (dtt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtt.Rows.Count; j++)
                            {
                                _stjt.Append("<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dtt.Rows[j]["t_plantname"].ToString());
                                _stjt.Append("<td>" + df.GetCapacityByDepid("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now) + "</td>");
                                _stjt.Append("<td>" + GerUnitNum("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'") + "</td>");
                                //_stjt.Append("<td>0</td>");
                                //if (dtt.Rows[j]["t_plantid"].ToString() != "T_QHHNZ")
                                //{
                                    _stjt.Append("<td>" + df.GetReaload("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", 3) + "</td>");
                                //}
                                //else
                                //{
                                //    _stjt.Append("<td>0</td>");
                                //}

                                _stjt.Append("<td> <div id=\"Div4\"> &nbsp;</div></td>");
                                _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now, 1) + "</td>");
                                _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now, 2) + "</td>");
                                _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now) + "</td>");
                                _stjt.Append("</tr>");
                            }
                        }
                    }
                }
                string sql3 = "select  * from t_base_department ";
                DataTable dt_3 = dl.RunDataTable(sql3, out errMsg);
                if (dt_3.Rows.Count > 0)
                {
                    for (int k = 0; k < dt_3.Rows.Count; k++)
                    {
                        if (dt_3.Rows[k]["t_depid"].ToString() != "D_BAPV")
                        {
                            _stjt.Append("<tr style=\"background-color:#CAFFCE;\"><td>&nbsp;&nbsp;&nbsp;&nbsp;" + dt_3.Rows[k]["t_depname"].ToString());
                            _stjt.Append("<td>" + df.GetCapacityByDepid("t_base_plant.t_depid ='" + dt_3.Rows[k]["t_depid"].ToString() + "'", DateTime.Now) + "</td>");
                            _stjt.Append("<td>" + GerUnitNum("t_base_plant.t_depid ='" + dt_3.Rows[k]["t_depid"].ToString() + "'") + "</td>");
                            //_stjt.Append("<td>0</td>");
                            _stjt.Append("<td>" + df.GetReaload("t_base_plant.t_depid ='" + dt_3.Rows[k]["t_depid"].ToString() + "'", 3) + "</td>");
                            _stjt.Append("<td> <div id=\"Div4\"> &nbsp;</div></td>");
                            _stjt.Append("<td>" + df.GetPower("t_base_plant.t_depid ='" + dt_3.Rows[k]["t_depid"].ToString() + "'", DateTime.Now, 1) + "</td>");
                            _stjt.Append("<td>" + df.GetPower("t_base_plant.t_depid ='" + dt_3.Rows[k]["t_depid"].ToString() + "'", DateTime.Now, 2) + "</td>");
                            _stjt.Append("<td>" + df.GetPower("t_base_plant.t_depid ='" + dt_3.Rows[k]["t_depid"].ToString() + "'", DateTime.Now) + "</td>");
                            _stjt.Append("</tr>");

                            string sql4 = "select  * from t_base_plant  where t_depid ='" + dt_3.Rows[k]["t_depid"].ToString() + "'";
                            DataTable dtt_1 = dl.RunDataTable(sql4, out errMsg);
                            if (dtt_1.Rows.Count > 0)
                            {
                                for (int l = 0; l < dtt_1.Rows.Count; l++)
                                {
                                    _stjt.Append("<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dtt_1.Rows[l]["t_plantname"].ToString());
                                    _stjt.Append("<td>" + df.GetCapacityByDepid("t_base_plant.t_plantid ='" + dtt_1.Rows[l]["t_plantid"].ToString() + "'", DateTime.Now) + "</td>");
                                    _stjt.Append("<td>" + GerUnitNum("t_base_plant.t_plantid ='" + dtt_1.Rows[l]["t_plantid"].ToString() + "'") + "</td>");
                                    //_stjt.Append("<td>0</td>");
                                    _stjt.Append("<td>" + df.GetReaload("t_base_plant.t_plantid ='" + dtt_1.Rows[l]["t_plantid"].ToString() + "'", 3) + "</td>");
                                    _stjt.Append("<td> <div id=\"Div4\"> &nbsp;</div></td>");
                                    _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt_1.Rows[l]["t_plantid"].ToString() + "'", DateTime.Now, 1) + "</td>");
                                    _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt_1.Rows[l]["t_plantid"].ToString() + "'", DateTime.Now, 2) + "</td>");
                                    _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt_1.Rows[l]["t_plantid"].ToString() + "'", DateTime.Now) + "</td>");
                                    _stjt.Append("</tr>");
                                }
                            }
                        }
                    }
                }
                _stjt.Append("</tr></table>");
            }
            else 
            {
                if (para_id == "C_DIMIAN")
                {
                    _stjt.Append("<tr style=\"background-color:#FAD09C;\"><td>地面电站</td>");
                }
                else
                {
                    _stjt.Append("<tr style=\"background-color:#FAD09C;\"><td>BAPV</td>");
                }
                _stjt.Append("<td>" + df.GetCapacityByDepid("t_base_plant.t_comid ='" + para_id + "'", DateTime.Now) + "</td>");
                _stjt.Append("<td>" + GerUnitNum("t_base_plant.t_comid ='" + para_id + "'") + "</td>");
                //_stjt.Append("<td>0</td>");
                _stjt.Append("<td>" + df.GetReaload("T_BASE_COMPANY.t_comid ='" + para_id + "'", 1) + "</td>");
                _stjt.Append("<td> <div id=\"Div4\"> &nbsp;</div></td>");
                _stjt.Append("<td>" + df.GetPower("t_base_plant.t_comid ='" + para_id + "'", DateTime.Now, 1) + "</td>");
                _stjt.Append("<td>" + df.GetPower("t_base_plant.t_comid ='" + para_id + "'", DateTime.Now, 2) + "</td>");
                _stjt.Append("<td>" + df.GetPower("t_base_plant.t_comid ='" + para_id + "'", DateTime.Now) + "</td>");
                _stjt.Append("</tr>");
                
                string sql2 = "select  * from t_base_plant  where t_comid ='" + para_id + "'";
                DataTable dtt = dl.RunDataTable(sql2, out errMsg);
                if (dtt.Rows.Count > 0)
                {
                    for (int j = 0; j < dtt.Rows.Count; j++)
                    {
                        _stjt.Append("<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;" + dtt.Rows[j]["t_plantname"].ToString());
                        _stjt.Append("<td>" + df.GetCapacityByDepid("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now) + "</td>");
                        _stjt.Append("<td>" + GerUnitNum("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'") + "</td>");
                        //_stjt.Append("<td>0</td>");
                        //if (dtt.Rows[j]["t_plantid"].ToString() != "T_QHHNZ")
                        //{
                            _stjt.Append("<td>" + df.GetReaload("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", 3) + "</td>");
                        //}
                        //else
                        //{
                        //    _stjt.Append("<td>0</td>");
                        //}
                        _stjt.Append("<td> <div id=\"Div4\"> &nbsp;</div></td>");
                        _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now, 1) + "</td>");
                        _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now, 2) + "</td>");
                        _stjt.Append("<td>" + df.GetPower("t_base_plant.t_plantid ='" + dtt.Rows[j]["t_plantid"].ToString() + "'", DateTime.Now) + "</td>");
                        _stjt.Append("</tr>");
                    }
                }
                _stjt.Append("</tr></table>");
            }
             object _obj = new
            {
                list = _stjt.ToString()
            };
             string result = JsonConvert.SerializeObject(_obj);
             Response.Write(result);
             Response.End();
        }
    }
}