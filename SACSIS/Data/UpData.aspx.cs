using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Collections;

namespace SACSIS.Data
{
    public partial class UpData : System.Web.UI.Page
    {
        private static string treeID = "";
        private static string fid = "ZZJRL";

        private DataTable dt = null;

        BLL.FormBLL bll = new BLL.FormBLL();

        StringBuilder st = new StringBuilder();
        private static string tableName = "";//存储数据表 名称
        private static string timeName = "";//存储数据表  时间字段名称
        private static string org = "";     //组织机构编号
        private static string columns = ""; //参数 ID
        private static string utype = "";   //上传类型  1 指标   2 组织维度   3 时间维度
        private static string orgId = "";


        private static string[] sends = null;
        private static string time = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                sends = Request["id"].Split('@');
                if (sends[1] == "1")
                {
                    dv_day.Visible = true;
                    dv_month.Visible = false;
                    dv_year.Visible = false;
                }
                else if (sends[1] == "2")
                {
                    dv_day.Visible = false;
                    dv_month.Visible = true;
                    dv_year.Visible = false;
                }
                else if (sends[1] == "3")
                {
                    dv_day.Visible = false;
                    dv_month.Visible = false;
                    dv_year.Visible = true;
                }

                orgId = sends[2];
                fid = sends[0];

                txtDay.Value = DateTime.Now.ToString("yyyy-MM-dd");
                txtMonth.Value = DateTime.Now.ToString("yyyy-MM");
                txtYear.Value = DateTime.Now.ToString("yyyy");
            }
            string param = Request["param"];
            if (param != "")
            {
                if (param == "Init")
                {
                    ShowInfo();
                }
                else if (param == "query")
                {
                    string value = HttpUtility.UrlDecode(Request["value"]);
                    if (sends[1] == "1")
                        time = Request["dTime"] + " 0:00:00";
                    else if (sends[1] == "2")
                        time = Request["mTime"] + "-1 0:00:00";
                    else if (sends[1] == "3")
                        time = Request["yTime"] + "-1-1 0:00:00";
                    if (utype == "1")
                    {
                        UpDates(time, value);
                    }
                    else if (utype == "2")
                    {
                        UpDates(tableName, time, timeName, columns, org, value);
                    }
                    else
                    {

                    }
                }
                else if (param == "reckon")
                {
                    string value = HttpUtility.UrlDecode(Request["value"]);
                    Reckon(value);
                }
            }

        }

        string showMessage = "";

           #region 公式计算
        /// <summary>
        /// 填报数据
        /// </summary>
        /// <param name="value"></param>
        public void Reckon(string value)
        {
            Hashtable ht = new Hashtable();
            string[] values = value.Split('`');
            for (int i = 0; i < values.Length; i++)
            {
                ht.Add(values[i].Split('~')[0], values[i].Split('~')[1]);
            }
            string key = "";

            DataTable dtGrade = new DataTable();
            string formula = "";
            //获取公式等级
            dtGrade = bll.GetFormGrade(fid);

            Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            for (int i = 0; i < dtGrade.Rows.Count; i++)
            {
                DataTable dtGradeList = new DataTable();
                dtGradeList = bll.GetFormGradeList(fid, dtGrade.Rows[i][0].ToString());
                if (formType == "0")
                {
                    for (int j = 0; j < dtGradeList.Rows.Count; j++)
                    {
                        formula = dtGradeList.Rows[j]["T_FORMULA"].ToString();
                        string[] cs = dtGradeList.Rows[j]["T_FORMULAPARA"].ToString().Split(',');
                        for (int k = 0; k < cs.Length; k++)
                        {
                            formula = formula.Replace(cs[k], ht[cs[k]].ToString());
                        }

                        key += dtGradeList.Rows[j]["T_PARAID"] + "*" + Microsoft.JScript.Eval.JScriptEvaluate(formula, ve).ToString() + ";";
                    }
                }
                else if (formType == "1")
                {
                    for (int j = 0; j < dtGradeList.Rows.Count; j++)
                    {
                        formula = dtGradeList.Rows[j]["T_FORMULA"].ToString();
                        string[] cs = dtGradeList.Rows[j]["T_FORMULAPARA"].ToString().Split(',');

                        DataTable dtOrg = new DataTable();
                        dtOrg = bll.GetDataOrgInfo(fid, "2");

                        string[] points = new string[cs.Length * dtOrg.Rows.Count];
                        string[] formulas = new string[dtOrg.Rows.Count];
                        string[] orgKey = new string[dtOrg.Rows.Count];
                        for (int k = 0; k < dtOrg.Rows.Count; k++)
                        {
                            formulas[k] = formula;
                        }

                        string keyPara = "";
                        for (int k = 0; k < cs.Length; k++)
                        {
                            keyPara += "'" + cs[k] + "',";
                        }
                        keyPara = keyPara.Substring(0, keyPara.Length - 1);

                        DataTable dtParam = bll.GetDataParameter(fid, keyPara);

                        #region 还原公式
                        int num = 0;
                        //循环组织机构
                        for (int k = 0; k < dtOrg.Rows.Count; k++)
                        {
                            //循环公式参数
                            for (int l = 0; l < cs.Length; l++)
                            {
                                for (int u = 0; u < dtParam.Rows.Count; u++)
                                {
                                    if (cs[l] == dtParam.Rows[u][1].ToString())
                                    {
                                        formulas[k] = formulas[k].Replace(cs[l].ToString().Trim(), dtOrg.Rows[k][0].ToString().Trim() + cs[l].ToString().Trim() + (Convert.ToInt32(dtParam.Rows[u][0].ToString()) - 1));
                                        points[num] = dtOrg.Rows[k][0].ToString().Trim() + cs[l].ToString().Trim() + (Convert.ToInt32(dtParam.Rows[u][0].ToString()) - 1);
                                        break;
                                    }
                                }
                                num++;
                            }
                            orgKey[k] = dtOrg.Rows[k][0].ToString().Trim() + dtGradeList.Rows[j]["T_PARAID"].ToString() + (Convert.ToInt32(dtGradeList.Rows[j]["I_ORDER"].ToString()) - 1);
                        }
                        #endregion

                        //获取公式参数数据
                        for (int k = 0; k < formulas.Length; k++)
                        {
                            for (int l = 0; l < points.Length; l++)
                            {
                                if (points[l] != null)
                                    if (formulas[k].Contains(points[l]))
                                        formulas[k] = formulas[k].Replace(points[l], ht[points[l]].ToString());
                            }
                            key += orgKey[k] + "*" + Microsoft.JScript.Eval.JScriptEvaluate(formulas[k], ve).ToString() + ";";
                        }
                        key = key.Substring(0, key.Length - 1);
                    }
                }
            }
            object obj = new
            {
                type = formType,
                key = key
            };

            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 数据填报  填报指标数据
        private void UpDates(string time, string value)
        {
            //添加数据库列  填报数据
            //if (bll.CreateColumns(tableName, columns) && bll.UpZBData(tableName, time, timeName,treeID,orgId, value))
            if (bll.UpZBData(tableName, time, timeName, orgId, value, fid))
                showMessage = "数据填报成功!";
            else
                showMessage = "数据填报失败!";
            object obj = new
            {
                info = showMessage
            };

            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 填报数据 组织维度数据
        private void UpDates(string table, string time, string tName, string columnsID, string oId, string values)
        {
            //添加数据库列  填报数据
            // if (bll.CreateColumns(tableName, columns) && bll.UpData(table, time, tName, columnsID, oId, values))
            if (bll.UpData(table, time, tName, columnsID, oId, values))
                showMessage = "数据填报成功!";
            else
                showMessage = "数据填报失败!";
            object obj = new
            {
                info = showMessage
            };

            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion

        #region 初始化填报数据
        private string timeType = "";
        private static string formType = "";
        private string title = "";
        private string id = "";
        /// <summary>
        /// 初始化填报页面
        /// </summary>
        public void ShowInfo()
        {
            int kind_count = 0;
            //数据填报详细信息
            dt = bll.GetCreateInfo(fid, "1,2");//SCYXQKHZB  FDQQXMBB
            if (dt.Rows.Count > 0)
            {
                timeType = dt.Rows[0]["T_TIMETYPE"].ToString();//时间类型
                formType = dt.Rows[0]["I_FORMTYPE"].ToString();//表单类型:指标   正向   纵向
                title = dt.Rows[0]["T_FORMNAME"].ToString();
                timeName = dt.Rows[0]["T_TIMEFIELD"].ToString();
                tableName = dt.Rows[0]["T_TABLE"].ToString();

                DataTable dtValue = new DataTable();

                if (formType == "0")
                {
                    //获取到所有的表单数据分类  类型
                    DataTable dtType = bll.GetDataType(fid);
                    DataRow[] dr = null;
                    for (int v = 0; v < dtType.Rows.Count; v++)
                    {
                        //获取到某种类型的填报数据
                        dr = dt.Select("T_TYPE='" + dtType.Rows[v]["T_TYPE"] + "'");
                        if (dr.Length > 0)
                        {
                            st.Append("<table class=\"admintable\" width=\"98%\">");

                            if (formType == "0")
                            {
                                int countJudge = 0;
                                for (int n = 0; n < dt.Rows.Count; n++)
                                {
                                    if (dt.Rows[n]["I_INPUTTYPE"].ToString() == "2")
                                    {
                                        countJudge = 1;
                                        break;
                                    }
                                }
                                if (countJudge == 0)
                                {
                                    #region 短文本 数字
                                    if (v == 0)
                                    {
                                        st.Append("<tr><th class=\"adminth\" colspan=\"6\" style=\"color:black;\">" + title + "</th></tr>");
                                        if (dtType.Rows[v][0] != null && dtType.Rows[v][0].ToString() != "" && dtType.Rows[v][0].ToString() != " ")
                                            st.Append("<tr><td class=\"adminth\" align=\"center\"  color=\"black\" colspan=\"6\" height=\"30px\"><h3>" + dtType.Rows[v][0] + "</h3></td></tr>");
                                    }
                                    else
                                        st.Append("<tr><td class=\"adminth\" align=\"center\"  color=\"black\" colspan=\"6\" height=\"30px\"><h3>" + dtType.Rows[v][0] + "</h3></td></tr>");
                                    string cl = "";
                                    for (int i = 0; i < dr.Length; i++)
                                    {
                                        cl += dr[i][8] + ",";
                                    }

                                    cl = cl.Substring(0, cl.Length - 1);

                                    if (timeType == "1")
                                        dtValue = bll.GetCreateValueZB(cl, tableName, timeName, DateTime.Now.ToString("yyyy-MM-dd 0:00:00"), orgId);
                                    else if (timeType == "2")
                                        dtValue = bll.GetCreateValueZB(cl, tableName, timeName, DateTime.Now.Year + "-" + DateTime.Now.Month + "-1 0:00:00", orgId);
                                    else if (timeType == "3")
                                        dtValue = bll.GetCreateValueZB(cl, tableName, timeName, DateTime.Now.Year.ToString() + "-1-1 0:00:00", orgId);

                                    int k = 0;
                                    for (int i = 0; i < dr.Length / 3; i++)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k + 1][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k + 2][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k + 2][8].ToString()] + "\"/></td>");
                                                st.Append("</tr>");
                                            }
                                            else
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k][6] + "\" type=\"text\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 1][6] + "\" type=\"text\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k + 2][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 2][6] + "\" type=\"text\"/></td>");
                                                st.Append("</tr>");
                                            }
                                        }
                                        else
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k + 1][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k + 2][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k + 2][8].ToString()] + "\"/></td>");
                                                st.Append("</tr>");
                                            }
                                            else
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k][6] + "\" type=\"text\"/></td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 1][6] + "\" type=\"text\"/></td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k + 2][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[k + 2][6] + "\" type=\"text\"/></td>");
                                                st.Append("</tr>");
                                            }
                                        }
                                        id += dr[k][6] + "*" + dr[k + 1][6] + "*" + dr[k + 2][6] + "*";
                                        k += 3;
                                    }

                                    int num = dr.Length % 3;

                                    if (num == 1)
                                    {
                                        if (dr.Length / 3 % 2 == 0)
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                //if (dr.Length != 1)
                                                //{
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                st.Append("</tr>");
                                                //}
                                                //else
                                                //{
                                                //    st.Append("<tr>");
                                                //    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                //    st.Append("<td class=\"admincls0\" colspan=\"5\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"\"/></td>");
                                                //    st.Append("</tr>");
                                                //}
                                            }
                                            else
                                            {
                                                if (dr.Length != 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" colspan=\"5\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                if (dr.Length != 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" colspan=\"5\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                            else
                                            {
                                                if (dr.Length != 1)
                                                {
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" colspan=\"5\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                    st.Append("<tr>");
                                                }
                                            }
                                        }
                                        id += dr[dr.Length - 1][6];
                                    }
                                    else if (num == 2)
                                    {
                                        if (dr.Length / 3 % 2 == 0)
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                if (dt.Rows.Count != 2)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" colspan=\"3\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                            else
                                            {
                                                if (dr.Length != 2)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\"  align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" colspan=\"3\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                if (dr.Length / 3 % 2 == 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                } else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" colspan=\"3\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                            else
                                            {
                                                if (dr.Length / 3 % 2 == 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" colspan=\"3\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_zb\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                        }
                                        id += dr[dr.Length - 2][6] + "*" + dr[dr.Length - 1][6];
                                    }

                                    string index = id.Remove(0, id.Length - 1);
                                    if (index == "*")
                                        id = id.Substring(0, id.Length - 1);

                                    columns = id;
                                    columns = columns + "*" + timeName;

                                    st.Append("</table>");
                                    utype = "1";
                                    #endregion
                                }
                                else
                                {
                                    #region 长文本
                                    if (v == 0)
                                    {
                                        st.Append("<tr><th class=\"adminth\" colspan=\"4\" style=\"color:black;\">" + title + "</th></tr>");
                                        if (dtType.Rows[v][0] != null && dtType.Rows[v][0].ToString() != "" && dtType.Rows[v][0].ToString() != " ")
                                            st.Append("<tr><td class=\"adminth\" align=\"center\"  color=\"black\" colspan=\"4\" height=\"30px\"><h3>" + dtType.Rows[v][0] + "</h3></td></tr>");
                                    }
                                    else
                                        st.Append("<tr><td class=\"adminth\" align=\"center\"  color=\"black\" colspan=\"4\" height=\"30px\"><h3>" + dtType.Rows[v][0] + "</h3></td></tr>");
                                    string cl = "";
                                    for (int i = 0; i < dr.Length; i++)
                                    {
                                        cl += dr[i][8] + ",";
                                    }

                                    cl = cl.Substring(0, cl.Length - 1);

                                    if (timeType == "1")
                                        dtValue = bll.GetCreateValueZB(cl, tableName, timeName, DateTime.Now.ToString("yyyy-MM-dd 0:00:00"), orgId);
                                    else if (timeType == "2")
                                        dtValue = bll.GetCreateValueZB(cl, tableName, timeName, DateTime.Now.Year + "-" + DateTime.Now.Month + "-1 0:00:00", orgId);
                                    else if (timeType == "3")
                                        dtValue = bll.GetCreateValueZB(cl, tableName, timeName, DateTime.Now.Year.ToString() + "-1-1 0:00:00", orgId);

                                    int k = 0;
                                    for (int i = 0; i < dr.Length / 2; i++)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k + 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k + 1][8].ToString()] + "\"/></td>");
                                                st.Append("</tr>");
                                            }
                                            else
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k][6] + "\" type=\"text\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k + 1][6] + "\" type=\"text\"/></td>");
                                                st.Append("</tr>");
                                            }
                                        }
                                        else
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k + 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[k + 1][8].ToString()] + "\"/></td>");
                                                st.Append("</tr>");
                                            }
                                            else
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k][6] + "\" type=\"text\"/></td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k + 1][7] + "</td>");
                                                st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[k + 1][6] + "\" type=\"text\"/></td>");
                                                st.Append("</tr>");
                                            }
                                        }
                                        id += dr[k][6] + "*" + dr[k + 1][6] + "*";
                                        k += 2;
                                    }

                                    int num = dr.Length % 2;

                                    if (num == 1)
                                    {
                                        if (dr.Length / 2 % 2 == 0)
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                st.Append("<tr>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                st.Append("</tr>");
                                            }
                                            else
                                            {
                                                if (dr.Length != 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" colspan=\"3\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                if (dr.Length != 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" colspan=\"3\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                            else
                                            {
                                                if (dr.Length != 1)
                                                {
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" colspan=\"3\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                    st.Append("<tr>");
                                                }
                                            }
                                        }
                                        id += dr[dr.Length - 1][6];
                                    }
                                    else if (num == 2)
                                    {
                                        if (dr.Length / 2 % 2 == 0)
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                if (dt.Rows.Count != 2)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                            else
                                            {
                                                if (dr.Length != 2)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\"  align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls0\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (dtValue != null && dtValue.Rows.Count > 0)
                                            {
                                                if (dr.Length / 2 % 2 == 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 2][8].ToString()] + "\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\" value=\"" + dtValue.Rows[0][dr[dr.Length - 1][8].ToString()] + "\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                            else
                                            {
                                                if (dr.Length / 3 % 2 == 1)
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                                else
                                                {
                                                    st.Append("<tr>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 2][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 2][6] + "\" type=\"text\"/></td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">" + dr[dr.Length - 1][7] + "</td>");
                                                    st.Append("<td class=\"admincls1\" align=\"center\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=\"ipt_txt\" id=\"" + dr[dr.Length - 1][6] + "\" type=\"text\"/></td>");
                                                    st.Append("</tr>");
                                                }
                                            }
                                        }
                                        id += dr[dr.Length - 2][6] + "*" + dr[dr.Length - 1][6];
                                    }

                                    DataTable dt_kind = bll.GetCreateInfo(fid, "3");//获取大量文本列
                                    if (dt_kind != null && dt_kind.Rows.Count > 0)
                                    {
                                        id += "~";
                                        for (int ke = 0; ke < dt_kind.Rows.Count; ke++)
                                        {
                                            st.Append("<tr>");
                                            st.Append("<td class=\"admincls1\" align=\"center\">" + dt_kind.Rows[ke][7] + "</td>");
                                            st.Append("<td class=\"admincls0\" colspan=" + (dr.Length) + " align=\"center\"><textarea name=\"content_" + ke + "\" style=\"width: 800px; height: 200px;\"></textarea>&nbsp;</td>");
                                            st.Append("</tr>");
                                            id += dt_kind.Rows[ke][6] + "~";
                                            kind_count++;
                                        }

                                        string index = id.Remove(0, id.Length - 1);
                                        if (index == "~")
                                            id = id.Substring(0, id.Length - 1);

                                        columns = id;
                                    }
                                    else
                                    {
                                        string index = id.Remove(0, id.Length - 1);
                                        if (index == "*")
                                            id = id.Substring(0, id.Length - 1);

                                        columns = id;
                                    }
                                    columns = columns + "*" + timeName;

                                    st.Append("</table>");
                                    utype = "1";
                                    #endregion
                                }
                            }
                        }
                    }
                }
                else if (formType == "1")
                {
                    DataRow[] dr = null;
                    DataRow[] drOrg = null;
                    columns = "";
                    org = "";
                    //获取指标参数(维护几列数据：各场站的属性：容量，在建容量)
                    dr = dt.Select("T_PARATYPE='0'");

                    //获取组织维度参数（维护几行：各场站）
                    drOrg = dt.Select("T_PARATYPE='2'");

                    st.Append("<table class=\"admintable\">");
                    if (drOrg.Length > 0)
                    {
                        int countJudge = 0;
                        for (int n = 0; n < dt.Rows.Count; n++)
                        {
                            if (dt.Rows[n]["I_INPUTTYPE"].ToString() == "2")
                            {
                                countJudge = 1;
                                break;
                            }
                        }

                        if (countJudge == 0)
                        {
                            #region 二维数据表 短文本 数字
                            for (int i = -1; i < drOrg.Length; i++)
                            {
                                if (i == -1)
                                {
                                    st.Append("<tr>");
                                    //循环指标
                                    for (int k = -1; k < dr.Length; k++)
                                    {
                                        if (k == -1)
                                        {
                                            st.Append("<td class=\"admincls1\" align=\"center\" width=\"200px\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>");
                                        }
                                        else
                                        {
                                            if (dr.Length == 1)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"80%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 2)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"40%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 3)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"30%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 4)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"22%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 5)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"18%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 6)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"16%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 7)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"13%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 8)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"11%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 9)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"10%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 10)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"9%\">" + dr[k][7] + "</td>");
                                            else
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k][7] + "</td>");
                                            columns += dr[k][8].ToString() + "*";
                                            //columns += dr[k][6].ToString() + "*";
                                        }
                                    }
                                    st.Append("</tr>");
                                    if (timeType == "1")
                                        dtValue = bll.GetCreateValue(columns, tableName, timeName, DateTime.Now.ToString("yyyy-MM-dd 0:00:00"), orgId, fid);
                                    else if (timeType == "2")
                                        dtValue = bll.GetCreateValue(columns, tableName, timeName, DateTime.Now.Year + "-" + DateTime.Now.Month + "-1 0:00:00", orgId, fid);
                                    else if (timeType == "3")
                                        dtValue = bll.GetCreateValue(columns, tableName, timeName, DateTime.Now.Year.ToString() + "-1-1 0:00:00", orgId, fid);

                                }
                                else
                                {
                                    st.Append("<tr>");
                                    st.Append("<td class=\"admincls0\" align=\"center\"><div style=\"width:200px;\">" + drOrg[i][7] + "</div></td>");
                                    //循环输入框
                                    for (int k = 0; k < dr.Length; k++)
                                    {
                                        if (dtValue != null && dtValue.Rows.Count > 0)
                                        {
                                            for (int d = 0; d < dtValue.Rows.Count; d++)
                                            {
                                                if (dtValue.Rows[d]["T_ORGID"].ToString().Trim() == drOrg[i][6].ToString().Trim())
                                                {
                                                    st.Append("<td class=\"admincls0\" align=\"center\"><input class=\"ipt\" id=\"" + drOrg[i][6] + dr[k][6] + k + "\" type=\"text\" value=\"" + dtValue.Rows[d][dr[k][8].ToString()] + "\"/>&nbsp;</td>");
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            st.Append("<td class=\"admincls0\" align=\"center\"><input class=\"ipt\" id=\"" + drOrg[i][6] + dr[k][6] + k + "\" type=\"text\"/>&nbsp;</td>");
                                        }

                                        id += drOrg[i][6] + dr[k][6].ToString() + k + "*";
                                    }
                                    st.Append("</tr>");
                                }
                                if (i != -1)
                                    org += drOrg[i][6] + "*";
                            }
                            st.Append("</table>");
                            columns += "T_ORGID*" + timeName;
                            org = org.Substring(0, org.Length - 1);
                            if (id.Length > 0)
                                id = id.Substring(0, id.Length - 1);
                            utype = "2";
                            #endregion
                        }
                        else
                        {
                            #region 二维数据长文本
                            for (int i = -1; i < drOrg.Length; i++)
                            {
                                if (i == -1)
                                {
                                    st.Append("<tr>");
                                    //循环指标
                                    for (int k = -1; k < dr.Length; k++)
                                    {
                                        if (k == -1)
                                        {
                                            st.Append("<td class=\"admincls1\" align=\"center\" width=\"200px\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>");
                                        }
                                        else
                                        {
                                            if (dr.Length == 1)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"80%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 2)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"40%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 3)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"30%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 4)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"22%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 5)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"18%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 6)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"16%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 7)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"13%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 8)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"11%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 9)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"10%\">" + dr[k][7] + "</td>");
                                            else if (dr.Length == 10)
                                                st.Append("<td class=\"admincls1\" align=\"center\" width=\"9%\">" + dr[k][7] + "</td>");
                                            else
                                                st.Append("<td class=\"admincls1\" align=\"center\">" + dr[k][7] + "</td>");
                                            columns += dr[k][8].ToString() + "*";
                                            //columns += dr[k][6].ToString() + "*";
                                        }
                                    }
                                    st.Append("</tr>");
                                    if (timeType == "1")
                                        dtValue = bll.GetCreateValue(columns, tableName, timeName, DateTime.Now.ToString("yyyy-MM-dd 0:00:00"), orgId, fid);
                                    else if (timeType == "2")
                                        dtValue = bll.GetCreateValue(columns, tableName, timeName, DateTime.Now.Year + "-" + DateTime.Now.Month + "-1 0:00:00", orgId, fid);
                                    else if (timeType == "3")
                                        dtValue = bll.GetCreateValue(columns, tableName, timeName, DateTime.Now.Year.ToString() + "-1-1 0:00:00", orgId, fid);

                                }
                                else
                                {
                                    st.Append("<tr>");
                                    st.Append("<td class=\"admincls0\" align=\"center\"><div style=\"width:200px;\">" + drOrg[i][7] + "</div></td>");
                                    //循环输入框
                                    for (int k = 0; k < dr.Length; k++)
                                    {
                                        if (dtValue != null && dtValue.Rows.Count > 0)
                                        {
                                            for (int d = 0; d < dtValue.Rows.Count; d++)
                                            {
                                                if (dtValue.Rows[d]["T_ORGID"].ToString().Trim() == drOrg[i][6].ToString().Trim())
                                                {
                                                    st.Append("<td class=\"admincls0\" align=\"center\"><input class=\"ipt_txt\" id=\"" + drOrg[i][6] + dr[k][6] + k + "\" type=\"text\" value=\"" + dtValue.Rows[d][dr[k][8].ToString()] + "\"/>&nbsp;</td>");
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            st.Append("<td class=\"admincls0\" align=\"center\"><input class=\"ipt_txt\" id=\"" + drOrg[i][6] + dr[k][6] + k + "\" type=\"text\"/>&nbsp;</td>");
                                        }

                                        id += drOrg[i][6] + dr[k][6].ToString() + k + "*";
                                    }
                                    st.Append("</tr>");
                                }
                                if (i != -1)
                                    org += drOrg[i][6] + "*";
                            }
                            st.Append("</table>");
                            columns += "T_ORGID*" + timeName;
                            org = org.Substring(0, org.Length - 1);
                            if (id.Length > 0)
                                id = id.Substring(0, id.Length - 1);
                            utype = "2";
                            #endregion
                        }

                        //DataTable dt_area = bll.GetCreateInfo(fid, "3");
                        //if (dt_area != null && dt_area.Rows.Count > 0)
                        //{
                        //    st.Append("<tr>");
                        //    st.Append("<td class=\"admincls1\" align=\"center\"></td>");
                        //    st.Append("<td class=\"admincls0\" colspan=" + (dr.Length - 1) + " align=\"center\"><textarea name=\"content_0\" style=\"width: 800px; height: 200px;\"></textarea>&nbsp;</td>");
                        //    st.Append("</tr>");
                        //}
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            else
            {
                string num = "";
                if (fid == "SCRL")
                {
                    num = "1";
                }
                else if (fid == "SCMSJ")
                {
                    num = "2";
                }
                else if (fid == "SCYSJ")
                {
                    num = "3";
                }
            }

            object obj = new
            {
                kind_count = kind_count,
                num = timeType,
                key = id,
                table = st.ToString()
            };
            string s = st.ToString();
            string result = JsonConvert.SerializeObject(obj);
            Response.Write(result);
            Response.End();
        }
        #endregion
    }
}