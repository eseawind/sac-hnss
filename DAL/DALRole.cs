using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SAC.DBOperations;
using SAC.Helper;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace DAL
{
    public class DALRole
    {
        DBLink dl = new DBLink();
        #region 角色管理
        //获取所有角色信息
        public DataTable GetRoleList()
        {
            string errMsg = "";
            DataTable dt = null;
            string sql = "select * from T_SYS_GROUP order by ID_KEY asc";
            dt = dl.RunDataTable(sql, out errMsg);
            return dt;
        }
        //根据Grid每页显示多少条记录返回所有角色信息
        public DataTable GetAllRole(int sCount, int eCount)
        {
            DataTable dt = null;
            string[] cName = new string[]{"ID_KEY","T_GRPID","T_GRPDESC"};
            dt = dl.GetS2Enotes("T_SYS_GROUP",cName,"ID_KEY",sCount,eCount);
            return dt;
        }
        //共有多少条角色记录
        public int GetRoleCount()
        {
            return dl.GetCount("T_SYS_GROUP");
        }
        //保存新的角色信息
        public bool SaveRole(string rId, string rName, out string errMsg)
        {
            errMsg = "";
            bool flag = false;
            string sql1 = "select * from T_SYS_GROUP where T_GRPID='" + rId + "'";
            DataTable dt = null;
            dt = dl.RunDataTable(sql1,out errMsg);
            if (dt == null || dt.Rows.Count == 0)
            {
                string sql2 = "insert into T_SYS_GROUP (T_GRPID,T_GRPDESC) values ('" + rId + "','" + rName + "')";
                dl.RunNonQuery(sql2, out errMsg);
                if (errMsg == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //编辑原有的职别（班组）信息
        public bool UpDateRole(string OrId, string rId, string rName, out string errMsg)
        {
            errMsg = "";
            if (rId == OrId)
            {
                string sql1 = "update T_SYS_GROUP set T_GRPDESC='" + rName + "' where T_GRPID='" + OrId + "'";
                dl.RunNonQuery(sql1, out errMsg);
                if (errMsg == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                string sql1 = "select * from T_SYS_GROUP where T_GRPID='" + rId + "'";

                DataTable dt = null;
                dt = dl.RunDataTable(sql1, out errMsg);
                if (dt == null || dt.Rows.Count == 0)
                {
                    string sql2 = "update T_SYS_GROUP set T_GRPID='" + rId + "',T_GRPDESC='" + rName + "' where T_GRPID='" + OrId + "'";
                    dl.RunNonQuery(sql2, out errMsg);
                    if (errMsg == "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        //删除原有的职别（班组）信息
        public bool DeleteRole(string rId, out string errMsg)
        {
            errMsg = "";
            bool flag = true;

            string sql = "delete from T_SYS_GROUP where T_GRPID='" + rId + "'";
            dl.RunNonQuery(sql, out errMsg);
            
            if (errMsg == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 权限管理
        /// <summary>
        /// 人员数据集
        /// </summary>
        /// <returns></returns>
        public DataTable GetMembers()
        {
            string errMsg = "";
            DataTable dt = null;
            string sql = "select T_USERID,T_USERNAME from T_SYS_MEMBERINFO order by ID_KEY asc";
            try
            {
                dt = dl.RunDataTable(sql, out errMsg);
            }
            catch (Exception ex)
            {
                
            }
            
            return dt;
        }
        /// <summary>
        /// 判断某个岗位下面是否存在人员
        /// </summary>
        /// <param name="name">人员ID</param>
        /// <returns></returns>
        public bool JudgMemberByORGId(string id)
        {
            bool result = false;
            string errMsg = "";
            string sql = "select count(*) from T_SYS_MEMBERGRP where T_GRPID='" + id + "'";
            int count = dl.RunRowCount(sql, out errMsg);
            if (count > 0)
                result = true;
            else
                result = false;
            return result;
        }
        #endregion

        #region 角色人员管理

        #region 根据每页显示多少条数据返回用户信息
        public DataTable GetUserMenuByRole(string id, int sCount, int eCount)
        {
            int t1 = eCount - sCount + 1;
            int t2 = sCount - 1;
            string rlDBType = dl.init();
            string sql = "select top " + t1 + " * from (select ID_KEY,T_USERID,T_USERNAME from(select T_SYS_MEMBERINFO.ID_KEY,T_SYS_MEMBERINFO.T_USERID,T_SYS_MEMBERINFO.T_USERNAME,T_SYS_MEMBERGRP.T_GRPID from T_SYS_MEMBERINFO left JOIN T_SYS_MEMBERGRP ON T_SYS_MEMBERGRP.T_USERID=T_SYS_MEMBERINFO.T_USERID)as a where a.T_GRPID='" + id + "')as b where (b.ID_KEY not in ( select top " + t2 + " ID_KEY from(select T_SYS_MEMBERINFO.ID_KEY,T_SYS_MEMBERINFO.T_USERID,T_SYS_MEMBERINFO.T_USERNAME,T_SYS_MEMBERGRP.T_GRPID from T_SYS_MEMBERINFO left JOIN T_SYS_MEMBERGRP ON T_SYS_MEMBERGRP.T_USERID=T_SYS_MEMBERINFO.T_USERID)as a where a.T_GRPID='" + id + "'))";
            string sql1 = "select * from ( select a.ID_KEY,a.T_USERID,a.T_USERNAME,rownumber() over(order by ID_KEY asc ) as rowid from (select T_SYS_MEMBERINFO.ID_KEY,T_SYS_MEMBERINFO.T_USERID,T_SYS_MEMBERINFO.T_USERNAME,T_SYS_MEMBERGRP.T_GRPID from T_SYS_MEMBERINFO left JOIN T_SYS_MEMBERGRP ON T_SYS_MEMBERGRP.T_USERID=T_SYS_MEMBERINFO.T_USERID ORDER BY T_SYS_MEMBERINFO.ID_KEY)as a where a.T_GRPID='" + id + "') as b where b.rowid between " + sCount + " and " + eCount + "";
            string sql2 = "select * from(select ID_KEY,T_USERID,T_USERNAME,ROWNUM rn from(select T_SYS_MEMBERINFO.ID_KEY,T_SYS_MEMBERINFO.T_USERID,T_SYS_MEMBERINFO.T_USERNAME,T_SYS_MEMBERGRP.T_GRPID from T_SYS_MEMBERINFO left JOIN T_SYS_MEMBERGRP ON T_SYS_MEMBERGRP.T_USERID=T_SYS_MEMBERINFO.T_USERID ORDER BY T_SYS_MEMBERINFO.ID_KEY) where T_GRPID='" + id + "' and ROWNUM <= " + eCount + ")WHERE rn >= " + sCount + "";

            string errMsg;
            DataTable dt = null;
            if (rlDBType == "SQL")
            {
                try
                {
                    dt = dl.RunDataTable(sql, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "DB2")
            {
                try
                {
                    dt = dl.RunDataTable(sql1, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "ORACLE")
            {
                try
                {
                    dt = dl.RunDataTable(sql2, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            return dt;
        }
        #endregion
        #region 根据角色ID返回所有用户信息的条数
        public int GetUserCountByRole(string id)
        {
            string rlDBType = dl.init();
            string sql = "select COUNT(*) from(select T_SYS_MEMBERINFO.ID_KEY,T_SYS_MEMBERINFO.T_USERID,T_SYS_MEMBERINFO.T_USERNAME,T_SYS_MEMBERGRP.T_GRPID from T_SYS_MEMBERINFO left JOIN T_SYS_MEMBERGRP ON T_SYS_MEMBERGRP.T_USERID=T_SYS_MEMBERINFO.T_USERID)as a where a.T_GRPID='" + id + "'";
            string sql1 = "select count(*) from ( select a.ID_KEY,a.T_USERID,a.T_USERNAME from (select T_SYS_MEMBERINFO.ID_KEY,T_SYS_MEMBERINFO.T_USERID,T_SYS_MEMBERINFO.T_USERNAME,T_SYS_MEMBERGRP.T_GRPID from T_SYS_MEMBERINFO left JOIN T_SYS_MEMBERGRP ON T_SYS_MEMBERGRP.T_USERID=T_SYS_MEMBERINFO.T_USERID ORDER BY T_SYS_MEMBERINFO.ID_KEY)as a where a.T_GRPID='" + id + "')as b";
            string sql2 = "select count(*) from ( select ID_KEY,T_USERID,T_USERNAME from (select T_SYS_MEMBERINFO.ID_KEY,T_SYS_MEMBERINFO.T_USERID,T_SYS_MEMBERINFO.T_USERNAME,T_SYS_MEMBERGRP.T_GRPID from T_SYS_MEMBERINFO left JOIN T_SYS_MEMBERGRP ON T_SYS_MEMBERGRP.T_USERID=T_SYS_MEMBERINFO.T_USERID ORDER BY T_SYS_MEMBERINFO.ID_KEY) where T_GRPID='" + id + "')";
            string errMsg;
            int count = 0;
            if (rlDBType == "SQL")
            {
                try
                {
                    count = dl.RunRowCount(sql, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "DB2")
            {
                try
                {
                    count = dl.RunRowCount(sql1, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "ORACLE")
            {
                try
                {
                    DataTable dt = dl.RunDataTable(sql, out errMsg);
                    count = int.Parse(dt.Rows[0]["COUNT(*)"].ToString());
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            return count;
        }
        #endregion
        #region 判断是否存在该人员
        /// <summary>
        /// 判断是否存在该人员
        /// </summary>
        /// <param name="id">人员ID</param>
        /// <returns></returns>
        public bool JudgMember(string id)
        {
            string rlDBType = dl.init();
            string errMsg;
            int count = 0;
            bool result = false;
            if (rlDBType == "SQL")
            {
                try
                {
                    string sql = "select count(*) from T_SYS_MEMBERINFO where T_USERID='" + id + "'";
                    count = dl.RunRowCount(sql, out errMsg);
                    if (count > 0)
                        result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "DB2")
            {
                try
                {
                    string sql = "select count(*) from T_SYS_MEMBERINFO where T_USERID='" + id + "'";
                    count = dl.RunRowCount(sql, out errMsg);
                    if (count > 0)
                        result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "ORACLE")
            {
                try
                {
                    string sql = "select count(*) from T_SYS_MEMBERINFO where T_USERID='" + id + "'";
                    DataTable dt = dl.RunDataTable(sql, out errMsg);
                    count = int.Parse(dt.Rows[0]["count(*)"].ToString());
                    if (count > 0)
                        result = true;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            return result;
        }
        #endregion
        #region 添加人员信息
        /// <summary>
        /// 添加人员信息
        /// </summary>
        /// <param name="id">用户编码</param>
        /// <param name="name">真实姓名</param>
        /// <param name="pwd">登陆密码</param>
        /// <param name="img">图片</param>
        /// <returns></returns>
        public bool AddMember(string id, string name, string pwd, byte[] img, string orgID)
        {
            string rlDBType = dl.init();
            string errMsg;
            string sql1 = "";
            string sql2 = "";
            bool result = false;
            if (rlDBType == "SQL")
            {
                if (img != null && img.Length > 0)
                {
                    sql1 = "insert into T_SYS_MEMBERINFO(T_USERID,T_USERNAME,T_PASSWD,B_ATTACHMENT) values(@T_USERID,@T_USERNAME,@T_PASSWD,@B_ATTACHMENT);";
                }
                else
                {
                    sql1 = "insert into T_SYS_MEMBERINFO(T_USERID,T_USERNAME,T_PASSWD) values(@T_USERID,@T_USERNAME,@T_PASSWD);";
                }
                sql2 = "insert into T_SYS_MEMBERGRP(T_USERID,T_GRPID) values('" + id + "','" + orgID + "')";
                try
                {
                    SqlConnection sqlconn = SAC.DBOperations.DBsql.GetConnection();
                    SqlCommand sqlcmd = new SqlCommand(sql1, sqlconn);
                    if (img != null && img.Length > 0)
                    {
                        sqlcmd.Parameters.Add("@T_USERID", id);
                        sqlcmd.Parameters.Add("@T_USERNAME", name);
                        sqlcmd.Parameters.Add("@T_PASSWD", pwd);
                        sqlcmd.Parameters.Add("@B_ATTACHMENT", img);
                    }
                    else
                    {
                        sqlcmd.Parameters.Add("@T_USERID", id);
                        sqlcmd.Parameters.Add("@T_USERNAME", name);
                        sqlcmd.Parameters.Add("@T_PASSWD", pwd);
                    }
                    if (sqlcmd.ExecuteNonQuery() > 0)
                        result = true;
                    sqlconn.Close();
                    dl.RunNonQuery(sql2, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "DB2")
            {
                if (img != null && img.Length > 0)
                {
                    sql1 = "insert into T_SYS_MEMBERINFO(T_USERID,T_USERNAME,T_PASSWD,B_ATTACHMENT) values(?,?,?,?);";
                }
                else
                {
                    sql1 = "insert into T_SYS_MEMBERINFO(T_USERID,T_USERNAME,T_PASSWD) values(?,?,?);";
                }
                sql2 = "insert into T_SYS_MEMBERGRP(T_USERID,T_GRPID) values('" + id + "','" + orgID + "')";
                try
                {
                    OleDbConnection con = new OleDbConnection(SAC.DBOperations.DBdb2.SetConString());
                    con.Open();
                    OleDbCommand oledbcom = new OleDbCommand(sql1, con);
                    if (img != null && img.Length > 0)
                    {
                        oledbcom.Parameters.Add("?", id);
                        oledbcom.Parameters.Add("?", name);
                        oledbcom.Parameters.Add("?", pwd);
                        oledbcom.Parameters.Add("?", img);
                    }
                    else
                    {
                        oledbcom.Parameters.Add("?", id);
                        oledbcom.Parameters.Add("?", name);
                        oledbcom.Parameters.Add("?", pwd);
                    }
                    if (oledbcom.ExecuteNonQuery() > 0)
                        result = true;
                    con.Close();
                    dl.RunNonQuery(sql2, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "ORACLE")
            {
                if (img != null && img.Length > 0)
                {
                    sql1 = "insert into T_SYS_MEMBERINFO(T_USERID,T_USERNAME,T_PASSWD,B_ATTACHMENT) values(:blobtodb,:blobtodb,:blobtodb,:blobtodb)";
                }
                else
                {
                    sql1 = "insert into T_SYS_MEMBERINFO(T_USERID,T_USERNAME,T_PASSWD) values(:blobtodb,:blobtodb,:blobtodb)";
                }
                sql2 = "insert into T_SYS_MEMBERGRP(T_USERID,T_GRPID) values('" + id + "','" + orgID + "')";
                try
                {
                    OracleConnection con = new OracleConnection(SAC.DBOperations.OracleHelper.retStr());
                    con.Open();
                    OracleCommand oledbcom = new OracleCommand(sql1, con);
                    if (img != null && img.Length > 0)
                    {
                        oledbcom.Parameters.Add("blobtodb", id);
                        oledbcom.Parameters.Add("blobtodb", name);
                        oledbcom.Parameters.Add("blobtodb", pwd);
                        oledbcom.Parameters.Add("blobtodb", img);
                    }
                    else
                    {
                        oledbcom.Parameters.Add("blobtodb", id);
                        oledbcom.Parameters.Add("blobtodb", name);
                        oledbcom.Parameters.Add("blobtodb", pwd);
                    }
                    if (oledbcom.ExecuteNonQuery() > 0)
                        result = true;
                    con.Close();
                    dl.RunNonQuery(sql2, out errMsg);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            return result;
        }
        #endregion
        #region 查询人员信息
        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="id">人员编号</param>
        /// <returns></returns>
        public DataTable GetmemberInfo(string id, int i)
        {
            string sql;
            string errMsg;
            DataTable dt = null;
            if (i == 1)
                sql = "select T_USERID,T_USERNAME,T_PASSWD from T_SYS_MEMBERINFO where T_USERID='" + id + "'";
            else
                sql = "select * from T_SYS_MEMBERINFO where T_USERID='" + id + "'";
            
            try
            {
                dt = dl.RunDataTable(sql, out errMsg);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
            }
            
            return dt;
        }
        #endregion

        #region 编辑人员信息
        public bool EditMemberInfo(string userIDO, string userID, string userName, string pwd, byte[] img, string treeNodeId)
        {
            string rlDBType = dl.init();
            string sql = "";
            string errMsg = "";
            bool result = false;
            if (rlDBType == "SQL")
            {
                try
                {
                    if (img != null)
                    {
                        SqlConnection sqlconn = SAC.DBOperations.DBsql.GetConnection();
                        if (userIDO == userID)
                        {
                            sql += "update T_SYS_MEMBERINFO set T_USERNAME=@T_USERNAME,T_PASSWD=@T_PASSWD,B_ATTACHMENT=@B_ATTACHMENT where T_USERID='" + userIDO + "'";
                            SqlCommand sqlcmd = new SqlCommand(sql, sqlconn);
                            sqlcmd.Parameters.Add("@T_USERNAME", userName);
                            sqlcmd.Parameters.Add("@T_PASSWD", pwd);
                            sqlcmd.Parameters.Add("@B_ATTACHMENT", img);
                            if (sqlcmd.ExecuteNonQuery() > 0)
                                result = true;
                        }
                        else
                        {
                            sql += "update T_SYS_MEMBERINFO set T_USERID=@T_USERID,T_USERNAME=@T_USERNAME,T_PASSWD=@T_PASSWD,B_ATTACHMENT=@B_ATTACHMENT where T_USERID='" + userIDO + "'";
                            SqlCommand sqlcmd = new SqlCommand(sql, sqlconn);
                            sqlcmd.Parameters.Add("@T_USERID", userID);
                            sqlcmd.Parameters.Add("@T_USERNAME", userName);
                            sqlcmd.Parameters.Add("@T_PASSWD", pwd);
                            sqlcmd.Parameters.Add("@B_ATTACHMENT", img);
                            result = DBsql.RunNonQuery("update T_SYS_MEMBERGRP set T_USERID='" + userID + "' where T_USERID='" + userIDO + "';", out errMsg);
                            if (sqlcmd.ExecuteNonQuery() > 0 && result == true)
                                result = true;
                        }
                        sqlconn.Close();
                    }
                    else
                    {
                        if (userIDO == userID)
                            sql += "update T_SYS_MEMBERINFO set T_USERNAME='" + userName + "',T_PASSWD='" + pwd + "' where T_USERID='" + userIDO + "';";
                        else
                            sql += "update T_SYS_MEMBERINFO set T_USERID='" + userID + "',T_USERNAME='" + userName + "',T_PASSWD='" + pwd + "' where T_USERID='" + userIDO + "';update T_SYS_MEMBERGRP set T_USERID='" + userID + "' where T_USERID='" + userIDO + "';";
                        result = dl.RunNonQuery(sql, out errMsg);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "DB2")
            {
                try
                {
                    if (img != null)
                    {
                        OleDbConnection con = new OleDbConnection(DBdb2.SetConString());
                        con.Open();
                        if (userIDO == userID)
                        {
                            sql += "update T_SYS_MEMBERINFO set T_USERNAME=?,T_PASSWD=?,B_ATTACHMENT=? where T_USERID='" + userIDO + "'";
                            OleDbCommand oledbcom = new OleDbCommand(sql, con);
                            oledbcom.Parameters.Add("?", userName);
                            oledbcom.Parameters.Add("?", pwd);
                            oledbcom.Parameters.Add("?", img);
                            if (oledbcom.ExecuteNonQuery() > 0)
                                result = true;
                        }
                        else
                        {
                            sql += "update T_SYS_MEMBERINFO set T_USERID=?,T_USERNAME=?,T_PASSWD=?,B_ATTACHMENT=? where T_USERID='" + userIDO + "'";
                            OleDbCommand oledbcom = new OleDbCommand(sql, con);
                            oledbcom.Parameters.Add("?", userID);
                            oledbcom.Parameters.Add("?", userName);
                            oledbcom.Parameters.Add("?", pwd);
                            oledbcom.Parameters.Add("?", img);
                            result = DBdb2.RunNonQuery("update T_SYS_MEMBERGRP set T_USERID='" + userID + "' where T_USERID='" + userIDO + "';", out errMsg);
                            if (oledbcom.ExecuteNonQuery() > 0 && result == true)
                                result = true;
                        }
                        con.Close();
                    }
                    else
                    {
                        if (userIDO == userID)
                            sql += "update T_SYS_MEMBERINFO set T_USERNAME='" + userName + "',T_PASSWD='" + pwd + "' where T_USERID='" + userIDO + "';";
                        else
                            sql += "update T_SYS_MEMBERINFO set T_USERID='" + userID + "',T_USERNAME='" + userName + "',T_PASSWD='" + pwd + "' where T_USERID='" + userIDO + "';update T_SYS_MEMBERGRP set T_USERID='" + userID + "' where T_USERID='" + userIDO + "';";
                        result = dl.RunNonQuery(sql, out errMsg);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            else if (rlDBType == "ORACLE")
            {
                try
                {
                    if (img != null)
                    {
                        OracleConnection con = new OracleConnection(SAC.DBOperations.OracleHelper.retStr());
                        con.Open();
                        if (userIDO == userID)
                        {
                            sql += "update T_SYS_MEMBERINFO set T_USERNAME=:blobtodb,T_PASSWD=:blobtodb,B_ATTACHMENT=:blobtodb where T_USERID='" + userIDO + "'";
                            OracleCommand orlcmd = new OracleCommand(sql, con);
                            orlcmd.Parameters.Add("blobtodb", userName);
                            orlcmd.Parameters.Add("blobtodb", pwd);
                            orlcmd.Parameters.Add("blobtodb", img);
                            if (orlcmd.ExecuteNonQuery() > 0)
                                result = true;
                        }
                        else
                        {
                            sql += "update T_SYS_MEMBERINFO set T_USERID=:blobtodb,T_USERNAME=:blobtodb,T_PASSWD=:blobtodb,B_ATTACHMENT=:blobtodb where T_USERID='" + userIDO + "'";
                            OracleCommand orlcmd = new OracleCommand(sql, con);
                            orlcmd.Parameters.Add("blobtodb", userID);
                            orlcmd.Parameters.Add("blobtodb", userName);
                            orlcmd.Parameters.Add("blobtodb", pwd);
                            orlcmd.Parameters.Add("blobtodb", img);
                            result = SAC.DBOperations.DBoracle.RunNonQuery("update T_SYS_MEMBERGRP set T_USERID='" + userID + "' where T_USERID='" + userIDO + "';", out errMsg);
                            if (orlcmd.ExecuteNonQuery() > 0 && result == true)
                                result = true;
                        }
                        con.Close();
                    }
                    else
                    {
                        if (userIDO == userID)
                            sql += "update T_SYS_MEMBERINFO set T_USERNAME='" + userName + "',T_PASSWD='" + pwd + "' where T_USERID='" + userIDO + "';";
                        else
                            sql += "update T_SYS_MEMBERINFO set T_USERID='" + userID + "',T_USERNAME='" + userName + "',T_PASSWD='" + pwd + "' where T_USERID='" + userIDO + "';update T_SYS_MEMBERGRP set T_USERID='" + userID + "' where T_USERID='" + userIDO + "';";
                        result = dl.RunNonQuery(sql, out errMsg);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
                }
            }
            return result;
        }
        #endregion
        #region 删除人员信息
        /// <summary>
        /// 删除人员信息
        /// </summary>
        /// <param name="id">人员编码</param>
        /// <returns></returns>
        public bool RemoveMember(string id)
        {
            string errMsg;
            bool result = false;
            string sql = "delete from T_SYS_MEMBERINFO where T_USERID in (" + id + ");delete from T_SYS_MEMBERGRP where T_USERID in (" + id + ");";
            try
            {
                result = dl.RunNonQuery(sql, out errMsg);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
            }
            
            return result;
        }
        #endregion
        #endregion

        #region 菜单管理
        public bool UpdataWebMenuXml(byte[] fileBytes)
        {
            string errMsg = "";
            string sql1 = "select * from T_SYS_MENU where T_XMLID='WebMenu'";
            DataTable dt = dl.RunDataTable(sql1,out errMsg);
            if (dt == null || dt.Rows.Count == 0)
            {
                string sql2 = "insert into T_SYS_MENU (T_XMLID,T_XMLNAME) values ('WebMenu','系统菜单')";
                dl.RunNonQuery(sql2, out errMsg);
            }
            return dl.RetBoolUpFile("T_SYS_MENU", "T_XMLID", "WebMenu", "B_XML", fileBytes,out errMsg);
        }

        public bool IsEmptyXml()
        {
            string errMsg = "";
            string sql = "select * from T_SYS_MENU where T_XMLID='WebMenu'";
            DataTable dt = dl.RunDataTable(sql, out errMsg);
            if (dt.Rows.Count == 0 || dt == null)
            {
                return true;
            }
            else
                return false;
        }
        public bool DownLoadXml(string fileID, string filePath)
        {
            return dl.DownLoadXml(fileID,filePath);
        }
        #endregion

        #region 登陆使用
        public DataTable GetUserInfo(string userName)
        {
            DataTable dt = null;
            string errMsg="";
            //string sql = "select * from dept";
            string sql = "select * from T_SYS_MEMBERINFO where T_USERID='" + userName + "'";
            dt = dl.RunDataTable(sql,out errMsg);
            return dt;
        }
        public string GetRoleId(string userId)
        {
            DataTable dt = null;
            string errMsg = "";
            string sql = "select * from T_SYS_MEMBERGRP where T_USERID='" + userId + "'";
            dt = dl.RunDataTable(sql, out errMsg);
            string roleId = dt.Rows[0]["T_GRPID"].ToString();
            return roleId;
        }
        #endregion
    }
}
