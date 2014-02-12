using System.Data.OleDb;
using System.Data.SqlClient;
using SAC.Helper;
using SAC.Model;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.Collections;
using SAC.DBOperations;

namespace DAL
{
    public class DALReportStyle
    {
        DALPubMehtod pm = new DALPubMehtod();
        DBLink dl = new DBLink();
        #region 报表样式

        private string dbTypeValue
        {
            get
            {
                return IniHelper.ReadIniData("RelationDBbase", "DBType", null);
            }
        }

        public string SaveRptStyle(ReportStyle rpt)
        {
            //要加入时间类型判断

            var rptId = rpt.RptID;
            var rptName = rpt.RptName;
            var rptStyle = rpt.RptStyle;
            var orgId = rpt.OrgId;
            var treeId = rpt.TreeId;
            var styleType = rpt.styleType;

            string errMsg = string.Empty;
            switch (dbTypeValue)
            {
                case "SQL":
                    {
                        var sqlInsert = "INSERT INTO T_INFO_SISREPORT(REPORTTYPE,REPORTID,REPORTNAME,REPORTSTYLE,ORGID,TREEID) VALUES(@p1,@p2,@p3,@p4,@p5,@p6)";
                        var sqlDelete = "DELETE FROM T_INFO_SISREPORT WHERE REPORTID=@p1 AND ORGID=@p2 AND TREEID=@p3";
                        using (var conn = new SqlConnection(DBsql.GetConnectionstr()))
                        {
                            conn.Open();
                            SqlTransaction transaction = conn.BeginTransaction();
                            var pDel = new[]
                                {
                                    new SqlParameter("@p1",rptId),
                                    new SqlParameter("@p2",orgId),
                                    new SqlParameter("@p3",treeId)
                                };
                            var pIns = new[]
                                {
                                    new SqlParameter("@p1",styleType),
                                    new SqlParameter("@p2",rptId),
                                    new SqlParameter("@p3",rptName),
                                    new SqlParameter("@p4",rptStyle),
                                    new SqlParameter("@p5",orgId),
                                    new SqlParameter("@p6",treeId)                                   
                                };
                            try
                            {
                                DBsql.ExecuteNonQuery(transaction, CommandType.Text, sqlDelete, pDel);
                                DBsql.ExecuteNonQuery(transaction, CommandType.Text, sqlInsert, pIns);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                errMsg = ex.Message;
                            }
                        }
                    }
                    break;
                case "ORACLE":
                    {
                        //修改Oracle后按照SqlServer对照一次，否则出错
                        var sqlInsert = "INSERT INTO T_INFO_SISREPORT(REPORTID,REPORTNAME,REPORTSTYLE,ORGID,TREEID,REPORTTYPE) VALUES(:p1,:p2,:p3,:p4,:p5,:p6)";
                        var sqlDelete = "DELETE FROM T_INFO_SISREPORT WHERE REPORTNAME=:p1 AND ORGID=:p2 AND TREEID=:p3";
                        var pDel = new[]
                            {
                                new OracleParameter(":p1",rptName),
                                new OracleParameter(":p2",orgId),
                                new OracleParameter(":p3",treeId)
                            };
                        var pIns = new[]
                            {
                                new OracleParameter(":p1",rptId),
                                new OracleParameter(":p2",rptName),
                                new OracleParameter(":p3",rptStyle),
                                new OracleParameter(":p4",orgId),
                                new OracleParameter(":p5",treeId),
                                new OracleParameter(":p6",styleType)
                            };

                        using (var conn = new OracleConnection(OracleHelper.retStr()))
                        {
                            conn.Open();
                            var transaction = conn.BeginTransaction();
                            try
                            {
                                SAC.DBOperations.OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sqlDelete, pDel);
                                SAC.DBOperations.OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sqlInsert, pIns);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                errMsg = ex.Message;
                            }
                        }
                    }
                    break;
                default://db2
                    {
                        var sqlInsert = "INSERT INTO T_INFO_SISREPORT(REPORTID,REPORTNAME,REPORTSTYLE,ORGID,TREEID) VALUES(?,?,?,?,?)";
                        var sqlDelete = "DELETE FROM T_INFO_SISREPORT WHERE REPORTNAME=? AND ORGID=? AND TREEID=?";

                        var pDel = new[]
                            {
                                new OleDbParameter("?",rptName), 
                                new OleDbParameter("?",orgId),
                                new OleDbParameter("?",treeId)
                            };
                        var pIns = new[]
                            {
                                new OleDbParameter("?",rptId),
                                new OleDbParameter("?",rptName),
                                new OleDbParameter("?",rptStyle),
                                new OleDbParameter("?",orgId),
                                new OleDbParameter("?",treeId)
                            };
                        using (var conn = DBdb2.GetConn())
                        {
                            conn.Open();
                            var transaction = conn.BeginTransaction();
                            try
                            {
                                DBdb2.ExecuteNonQuery(transaction, CommandType.Text, sqlDelete, pDel);
                                DBdb2.ExecuteNonQuery(transaction, CommandType.Text, sqlInsert, pIns);

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                errMsg = ex.Message;
                            }
                        }
                    }
                    break;
            }

            return errMsg;
        }

        /// <summary>
        /// 根据报表ID获取报表样式
        /// 所有的保存都在这一个方法里面，不区分insert或者update
        /// </summary>
        /// <param name="rptID">报表ID</param>
        /// <returns></returns>
        public DataTable GetRptStyleByID(string rptID)
        {
            var query = "SELECT * FROM T_INFO_SISREPORT WHERE REPORTID='" + rptID + "'";
            var errMsg = string.Empty;
            var dt = new DataTable();
            if (dbTypeValue == "SQL")
            {
                dt = DBsql.RunDataTable(query, out errMsg);
            }
            else
            {
                dt = DBdb2.RunDataTable(query, out errMsg);
            }

            if (errMsg != string.Empty)
            {
                throw new Exception(errMsg);
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// 根据报表名称获取报表样式
        /// </summary>
        /// <param name="rptName">报表NAME</param>
        /// <returns></returns>
        public DataTable GetRptStyleByName(string rptName)
        {
            var query = "SELECT * FROM T_INFO_SISREPORT WHERE REPORTNAME='" + rptName + "'";
            var errMsg = string.Empty;
            var dt = new DataTable();
            if (dbTypeValue == "SQL")
            {
                dt = DBsql.RunDataTable(query, out errMsg);
            }
            else if (dbTypeValue == "ORACLE")
            {
                dt = OracleHelper.Query(query).Tables[0];
            }
            else
            {
                dt = DBdb2.RunDataTable(query, out errMsg);
            }

            if (errMsg != string.Empty)
            {
                throw new Exception(errMsg);
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// 通过报表名称，ORGID,TREEID获取报表样式
        /// </summary>
        /// <param name="rptName"></param>
        /// <param name="treeId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable GetRptStyle(string rptId, string treeId, string orgId)
        {
            //var query = "SELECT * FROM T_INFO_SISREPORT WHERE REPORTNAME='" + rptName + "' AND ORGID='" + orgId +
            //            "' AND TREEID='" + treeId + "'";

            var query = "";
            
            if(treeId !="" && orgId!="")
                query="SELECT * FROM T_INFO_SISREPORT WHERE reportid='" + rptId + "' AND ORGID='" + orgId +
                        "' AND TREEID='" + treeId + "'";
            else
                query="SELECT * FROM T_INFO_SISREPORT WHERE reportid='" + rptId + "'";
            

            var errMsg = string.Empty;
            DataSet ds;
            if (dbTypeValue == "SQL")
            {
                ds = DBsql.RunDataSet(query, out errMsg);
            }
            else if (dbTypeValue == "ORACLE")
            {
                ds = OracleHelper.Query(query);
            }
            else
            {
                ds = DBdb2.RunDataSet(query, out errMsg);
                //dt = DBdb2.RunDataTable(query, out errMsg);
            }

            if (string.IsNullOrEmpty(errMsg))
            {
                return ds.Tables[0] ?? new DataTable();
            }
            else
            {
                throw new Exception(errMsg);
            }

        }

        //************//

        /// <summary>
        /// 判断报表模板名称是否存在
        /// </summary>
        /// <param name="treeID"></param>
        /// <param name="repName"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int GetCountByTreeID(string treeID,string repName,out string  errMsg) 
        {
            errMsg = "";
            int count = 0;
            DataTable dr = null;

            string sql = "select * from T_INFO_SISREPORT where Treeid='" + treeID + "' and reportname='" + repName + "'";
            dr = dl.RunDataTable(sql, out errMsg);
            //if (dbTypeValue == "SQL")
            //    dr = DBsql.RunDataTable(sql, out errMsg);
            //else if (dbTypeValue == "DB2")
            //    dr = DBdb2.RunDataTable(sql, out errMsg);
            //else if (dbTypeValue == "Oracle")
            //    dr = SAC.OracleHelper.OracleHelper.GetDataTable(sql);

            if (dr.Rows.Count > 0)
                count = dr.Rows.Count;

            return count;
        }

        /// <summary>
        /// 获取报表模板IList
        /// </summary>
        /// <param name="orgID"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public IList<Hashtable> GetStyleIList(string treeID, out string errMsg)
        {
            errMsg = "";
            DataTable dt = null;
            IList<Hashtable> list = null;

            string sql = "";
            
            if(treeID!="")
                sql="select distinct REPORTID,REPORTNAME,REPORTTYPE,REPORTNAME  from T_INFO_SISREPORT where treeid ='" + treeID + "'";// "select * from T_INFO_SISREPORT where treeid ='" + treeID + "'";
            else
                sql = "select distinct REPORTID,REPORTNAME,REPORTTYPE,REPORTNAME  from T_INFO_SISREPORT ";
            dt = dl.RunDataTable(sql, out errMsg);
            //if (dbTypeValue == "SQL")
            //    dt = dl.RunDataTable(sql, out errMsg);
            //else if (dbTypeValue == "DB2")
            //    dt = dl.RunDataTable(sql, out errMsg);
            //else if (dbTypeValue == "Oracle")
            //    dt = dl.RunDataTable(sql, out errMsg);

            if (dt != null && dt.Rows.Count > 0)
            { list = pm.DataTableToList(dt); }

            return list;
        } 

        #endregion
    }
}
