using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Reflection;
using SAC.Helper;

namespace SAC.DBOperations
{
    public abstract class OracleHelper
    {
        private OracleHelper()
        {

        }
        private static string connString = "Data Source=HJC;password=ZDT;User ID=ZDTAdmin;";

        public static string retStr()
        {
            //@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.210)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=TEST;Password=TEST123"; 
            return connString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + IniHelper.ReadIniData("RelationDBbase", "DBIP", null) + ")(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=" + IniHelper.ReadIniData("RelationDBbase", "SERVICE_NAME", null) + ")(instance_name=" + IniHelper.ReadIniData("RelationDBbase", "DBName", null) + ")));uid=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";pwd=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + "";
            //return connString = "Data Source=ZDT;Persist Security Info=True;User ID=ZDTAdmin;Unicode=True";//"user id=ZDTAdmin; password=ZDT; data source =ZDT;";
            //return connString = "user id=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";password=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + ";Unicode=true;data source=" + IniHelper.ReadIniData("RelationDBbase", "DBNAME", null) + ";";// ConfigurationManager.ConnectionStrings["OracleString"].ConnectionString;
            //return connString = "Data Source=" + IniHelper.ReadIniData("RelationDBbase", "DBNAME", null) + ";user ID=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";Password=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + "";
            //return connString = "Data Source=CPI;User ID=sacsis;Password=sacsis;";
        }

        //Data Source=yellow;user Id=Knemes;Password=oracle
        private static OracleConnection connection = null;

        #region 建立连接 + static OracleConnection Connection
        /// <summary>
        /// 建立连接
        /// </summary>
        public static OracleConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new OracleConnection(retStr());
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return OracleHelper.connection;
            }
        }
        #endregion

        #region 关闭连接 + static void CloseConnection()
        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void CloseConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.CloseConnection--错误信息：" + ex.Message);
                throw ex;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region 读取 + static OracleDataReader GetReader(string connectionString, string sql, params OracleParameter[] param)
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">数组</param>
        /// <returns></returns>
        public static OracleDataReader GetReader(string connectionString, string sql, params OracleParameter[] param)
        {
            OracleDataReader dr = null;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();
                con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Connection = con;
                if (param != null)
                    cmd.Parameters.AddRange(param);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                con.Close();
                //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.CloseConnection--错误信息：" + ex.Message);
                throw ex;
            }
            return dr;
        }
        #endregion

        #region 存储过程读取 + static OracleDataReader GetProcReader(string connectionString, string procName, params OracleParameter[] param)
        /// <summary>
        /// 存储过程读取
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="param">数组</param>
        /// <returns></returns>
        public static OracleDataReader GetProcReader(string connectionString, string procName, params OracleParameter[] param)
        {
            OracleDataReader dr = null;
            try
            {
                OracleConnection con = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procName;
                cmd.Connection = con;
                if (param != null)
                    cmd.Parameters.AddRange(param);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.CloseConnection--错误信息：" + ex.Message);
                throw ex;
            }
            return dr;
        }
        #endregion

        #region 执行增删该的方法 + static int ExecuteCommand(string connectionString, string sql, params OracleParameter[] param)
        /// <summary>
        /// 执行增删该的方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteCommand(string connectionString, string sql, params OracleParameter[] param)
        {
            int obj = -2;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                using (OracleCommand objCommand = new OracleCommand(sql))
                {
                    try
                    {
                        conn.Open();
                        objCommand.CommandType = CommandType.Text;
                        objCommand.CommandText = sql;
                        objCommand.Connection = conn;
                        if (param != null)
                            objCommand.Parameters.AddRange(param);
                        obj = objCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.ExecuteProcCommand--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return obj;
        }
        #endregion

        #region 执行增删该存储工程的方法 + static object ExecuteProcCommand(string connectionString, string procName, params OracleParameter[] param)
        /// <summary>
        /// 执行增删该存储工程的方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>受影响的行数</returns>
        public static object ExecuteProcCommand(string connectionString, string procName, params OracleParameter[] param)
        {
            object obj = null;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                using (OracleCommand objCommand = new OracleCommand(procName))
                {
                    try
                    {
                        conn.Open();
                        objCommand.CommandType = CommandType.StoredProcedure;
                        objCommand.CommandText = procName;
                        objCommand.Connection = conn;
                        if (param != null)
                            objCommand.Parameters.AddRange(param);
                        obj = objCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.ExecuteProcCommand--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return obj;
        }
        #endregion

        #region 执行查询--返回查询所返回的结果集中第一行的第一列 + static object ExecuteScalar(string connectionString, string sql, params OracleParameter[] param)
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>返回查询所返回的结果集中第一行的第一列</returns>
        public static object ExecuteScalar(string connectionString, string sql, params OracleParameter[] param)
        {
            object obj = null;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                using (OracleCommand objCommand = new OracleCommand(sql))
                {
                    try
                    {
                        conn.Open();
                        objCommand.CommandType = CommandType.Text;
                        objCommand.CommandText = sql;
                        objCommand.Connection = conn;
                        if (param != null)
                            objCommand.Parameters.AddRange(param);
                        obj = objCommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.ExecuteScalar--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return obj;
        }
        #endregion

        #region 执行查询存储过程--返回查询所返回的结果集中第一行的第一列 +static object ExecuteProcScalar(string connectionString, string procName, params OracleParameter[] param)
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>返回查询所返回的结果集中第一行的第一列</returns>
        public static object ExecuteProcScalar(string connectionString, string procName, params OracleParameter[] param)
        {
            object obj = null;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                using (OracleCommand objCommand = new OracleCommand(procName))
                {
                    try
                    {
                        conn.Open();
                        objCommand.CommandType = CommandType.StoredProcedure;
                        objCommand.CommandText = procName;
                        objCommand.Connection = conn;
                        if (param != null)
                            objCommand.Parameters.AddRange(param);
                        obj = objCommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        // SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.ExecuteProcScalar--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return obj;
        }
        #endregion

        #region 执行无参数SQL语句，返回DataSet + static DataSet GetDataSet(string connectionString, string sqlString)
        /// <summary>
        /// 执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sqlString)
        {
            DataSet ds = null;
            using (OracleConnection con = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqlString;
                        cmd.Connection = con;
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            ds = new DataSet();
                            da.Fill(ds);
                        }
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.GetDataSet--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return ds;
        }

        public static DataTable GetDataTable(string sqlString)
        {
            DataSet ds = GetDataSet(sqlString);
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0)
            {
                string errMsg = string.Format("查询{0}返回数据集为空", sqlString);
                return null;
            }
            else
                return ds.Tables[0];
        }
        #endregion

        #region 执行SQL语句，返回DataSet + static DataSet GetDataSet(string connectionString, string sqlString, params OracleParameter[] param)
        /// <summary>
        /// 执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string connectionString, string sqlString, params OracleParameter[] param)
        {
            DataSet ds = null;
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqlString;
                        cmd.Connection = con;
                        if (param != null)
                            cmd.Parameters.AddRange(param);
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            ds = new DataSet();
                            da.Fill(ds);
                        }
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.GetDataSet--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return ds;
        }
        #endregion

        #region 执行存储过程，返回DataSet + static DataSet GetProcDataSet(string connectionString, string procName, params OracleParameter[] param)
        /// <summary>
        /// 执行存储过程，返回DataSet
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet GetProcDataSet(string connectionString, string procName, params OracleParameter[] param)
        {
            DataSet ds = null;
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procName;
                        cmd.Connection = con;
                        if (param != null)
                            cmd.Parameters.AddRange(param);
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            ds = new DataSet();
                            da.Fill(ds);
                        }
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.GetDataSet--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return ds;
        }
        #endregion

        #region 执行存储过程，得到输出参数 + static List<object> GetProcExecuteNonQueryOut(string connectionString, string procName, params OracleParameter[] param)
        /// <summary>
        ///  执行存储过程，得到输出参数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<object> GetProcExecuteNonQueryOut(string connectionString, string procName, params OracleParameter[] param)
        {
            List<object> lists = new List<object>();
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procName;
                        cmd.Connection = con;
                        if (param != null)
                            cmd.Parameters.AddRange(param);
                        cmd.ExecuteNonQuery();
                        foreach (OracleParameter var in param)
                        {
                            if (var.Direction == ParameterDirection.Output)
                                lists.Add(cmd.Parameters[var.ParameterName].Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.GetExecuteNonQueryOut--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            return lists;
        }
        #endregion

        #region 执行sql，查看是否存在此条数据 + static bool IsExist(string connectionString, string sql, OracleParameter[] param)
        /// <summary>
        /// 执行sql，查看是否存在此条数据
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsExist(string connectionString, string sql, OracleParameter[] param)
        {
            object obj = null;
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.Connection = con;
                        if (param != null)
                            cmd.Parameters.AddRange(param);
                        obj = cmd.ExecuteOracleScalar();
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.IsExist--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = Convert.ToInt32(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 执行sql，查看是否存在此条数据--如果是根据主键查必须判断查到的只能是一条数据---严谨版 +  static bool Exist(string connectionString, string sql, params OracleParameter[] param)
        /// <summary>
        /// 执行sql，查看是否存在此条数据--如果是根据主键查必须判断查到的只能是一条数据---严谨版
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool Exist(string connectionString, string sql, params OracleParameter[] param)
        {
            //OracleParameter[] par
            DataSet ds = GetDataSet(connectionString, sql, param);
            if (ds == null)
                return false;
            if (ds.Tables[0].Rows.Count == 1)
                return true;
            return false;
        }
        #endregion

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader GetDataReader(string strSQL)
        {
            OracleConnection connection = new OracleConnection(retStr());
            OracleCommand cmd = new OracleCommand(strSQL, connection);
            try
            {
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行存储过程，查看是否存在此条数据
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static bool IsProcExist(string procName, OracleParameter[] param)
        {
            object obj = null;
            using (OracleConnection con = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procName;
                        cmd.Connection = con;
                        if (param != null)
                            cmd.Parameters.AddRange(param);
                        obj = cmd.ExecuteOracleScalar();
                    }
                    catch (Exception ex)
                    {
                        con.Close();
                        //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.IsExist--错误信息：" + ex.Message);
                        throw ex;
                    }
                }
            }
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = Convert.ToInt32(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public bool GetOracleBulkCopy(DataSet ds)
        //{
        //    bool flag = false;
        //    foreach (DataTable dt in ds.Tables)
        //    {
        //        flag = GetOracleBulkCopyByTable(retStr(), dt);
        //    }
        //    return flag;
        //}
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        //public static bool GetOracleBulkCopyByTable(string connectionString, DataTable dt)
        //{
        //    bool flag = false;
        //    Oracle.DataAccess.Client.OracleBulkCopy bulkCopy = null;
        //    try
        //    {
        //        bulkCopy = new Oracle.DataAccess.Client.OracleBulkCopy(connectionString);
        //        bulkCopy.DestinationTableName = dt.TableName;
        //        bulkCopy.WriteToServer(dt);
        //        flag = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        flag = false;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        bulkCopy = null;
        //    }
        //    return flag;
        //}


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public bool ExecuteOracleTran(string connectionString, ArrayList list)
        {
            bool flag = false;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                OracleTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < list.Count; n++)
                    {
                        string strsql = list[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    flag = true;
                }
                catch (System.Data.OracleClient.OracleException ex)
                {
                    flag = false;
                    tx.Rollback();
                    //  SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper--ExecuteOracleTran错误原因：" + ex.Message);
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
            return flag;
        }

        #region 公用方法

        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Exists(string strSql, params OracleParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        #endregion


        #region  执行简单SQL语句

        static public bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            errMsg = "";

            string conn = retStr();

            using (OracleConnection connection = new OracleConnection(conn))
            {
                using (OracleCommand cmd = new OracleCommand(sqlCmd, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ce)
                    {
                        errMsg = ce.Message;
                        return false;
                    }
                    finally { connection.Close(); }
                }
            }
            return true;
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int updateExecuteSql(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (OracleConnection conn = new OracleConnection(retStr()))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                OracleTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                OracleCommand cmd = new OracleCommand(SQLString, connection);
                System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@content", OracleType.NVarChar);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                OracleCommand cmd = new OracleCommand(strSQL, connection);
                System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@fs", OracleType.LongRaw);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string strSQL)
        {
            OracleConnection connection = new OracleConnection(retStr());
            OracleCommand cmd = new OracleCommand(strSQL, connection);
            try
            {
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OracleClient.OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }


        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OracleParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (OracleConnection conn = new OracleConnection(retStr()))
            {
                conn.Open();
                using (OracleTransaction trans = conn.BeginTransaction())
                {
                    OracleCommand cmd = new OracleCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            OracleParameter[] cmdParms = (OracleParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string SQLString, params OracleParameter[] cmdParms)
        {
            OracleConnection connection = new OracleConnection(retStr());
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.OracleClient.OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程 返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            OracleConnection connection = new OracleConnection(retStr());
            OracleDataReader returnReader;
            connection.Open();
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                OracleDataAdapter sqlDA = new OracleDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }


        /// <summary>
        /// 构建 OracleCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand</returns>
        private static OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (OracleConnection connection = new OracleConnection(retStr()))
            {
                int result;
                connection.Open();
                OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 OracleCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand 对象实例</returns>
        private static OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new OracleParameter("ReturnValue",
                OracleType.Int32, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion

        #region 创建OracleParameter参数集合，并且构造sql语句的后半部分，即where的后半部分 + static OracleParameter[] CreateParameter(Hashtable ht, out string strSql)
        /// <summary>
        /// 创建OracleParameter参数集合，并且构造sql语句的后半部分，即where的后半部分
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static OracleParameter[] CreateParameter(Hashtable ht, out string strSql)
        {
            StringBuilder str = new StringBuilder();

            if (ht.Count <= 0)
            {
                strSql = "";
                return null;
            }
            OracleParameter[] param = new OracleParameter[ht.Count];
            int i = 0;
            foreach (var item in ht.Keys)
            {
                str.Append(" " + item.ToString() + "=:" + Convert.ToString(item) + " and");
                param[i] = new OracleParameter(":" + Convert.ToString(item), ht[item]);
                i++;
            }
            strSql = str.ToString().Substring(0, str.ToString().Length - 3);
            return param;
        }
        #endregion

        #region 将OracleDataReader转换成Model + T ReaderModel<T>(OracleDataReader reader) where T : class,new()
        /// <summary>
        /// 将OracleDataReader转换成Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ReaderModel<T>(OracleDataReader reader) where T : class,new()
        {
            T t = new T();
            if (reader != null && reader.HasRows)
            {
                DataTable dt = reader.GetSchemaTable();
                List<string> list = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    list.Add(dr[0].ToString());
                }
                Type tt = t.GetType();
                foreach (PropertyInfo item in tt.GetProperties())
                {
                    Type proType = item.PropertyType;
                    if (!list.Contains(item.Name))
                    {
                        continue;
                    }
                    object value = reader[item.Name];
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                        continue;
                    if (proType == typeof(string))
                    {
                        item.SetValue(t, value, null);
                    }
                    else if (proType == typeof(Int32))
                    {
                        item.SetValue(t, value, null);
                    }
                    else if (proType == typeof(Nullable<Int32>))
                    {
                        item.SetValue(t, value == null ? null : (int?)Convert.ToInt32(value), null);
                    }
                    else if (proType == typeof(decimal))
                    {
                        item.SetValue(t, value, null);
                    }
                    else if (proType == typeof(decimal?))
                    {
                        item.SetValue(t, value == null ? null : (decimal?)Convert.ToInt32(value), null);
                    }
                    else if (proType == typeof(TimeSpan))
                    {
                        item.SetValue(t, value, null);
                    }
                    else if (proType == typeof(DateTime))
                    {
                        item.SetValue(t, value, null);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            return t;
        }
        #endregion

        #region 胡进财

        /// <summary>
        /// 获取序列  2012/15/12
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static long GetSequence(string sequence)
        {
            long count = 0;

            using (OracleConnection con = new OracleConnection(retStr()))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                        { con.Open(); }
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select " + sequence + ".nextval as val from dual";
                        cmd.Connection = con;
                        count = Convert.ToInt64(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return count;
        }
        #endregion

        #region 执行无参数SQL语句，返回DataSet + static DataSet QueryDataSet(string connectionString, string sqlString)
        /// <summary>
        /// 执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataSet QueryDataSet(string sqlString)
        {
            DataSet ds = null;
            //using (Oracle.DataAccess.Client.OracleConnection con = new Oracle.DataAccess.Client.OracleConnection(connString))
            //{
            //    using (Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand())
            //    {
            //        try
            //        {
            //            con.Open();
            //            cmd.CommandType = CommandType.Text;
            //            cmd.CommandText = sqlString;
            //            cmd.Connection = con;
            //            using (Oracle.DataAccess.Client.OracleDataAdapter da = new Oracle.DataAccess.Client.OracleDataAdapter(cmd))
            //            {
            //                ds = new DataSet();
            //                da.Fill(ds);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            //SWJC.ErrorLog.ErrHandler.WriteError("OracleHelper.GetDataSet--错误信息：" + ex.Message);
            //            throw ex;
            //        }
            //    }
            //}
            return ds;
        }
        #endregion

        public static string ExecuteNonQuery(string sqlText, OracleParameter[] parameters, out string errMsg)
        {
            errMsg = string.Empty;
            var cmd = new OracleCommand(sqlText, connection); ;
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return errMsg;
        }

        /// <summary> 
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数 
        /// </summary> 
        /// <param name="command">要处理的OracleCommand</param> 
        /// <param name="connection">数据库连接</param> 
        /// <param name="transaction">一个有效的事务或者是null值</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param> 
        /// <param name="commandParameters">和命令相关联的OracleParameter参数数组,如果没有参数为'null'</param> 
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param> 
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it 
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // 给命令分配一个数据库连接. 
            command.Connection = connection;

            // 设置命令文本(存储过程名或SQL语句) 
            command.CommandText = commandText;

            // 分配事务 
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // 设置命令类型. 
            command.CommandType = commandType;

            // 分配命令参数 
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        /// <summary> 
        /// 将OracleParameter参数数组(参数值)分配给OracleCommand命令. 
        /// 这个方法将给任何一个参数分配DBNull.Value; 
        /// 该操作将阻止默认值的使用. 
        /// </summary> 
        /// <param name="command">命令名</param> 
        /// <param name="commandParameters">SqlParameters数组</param> 
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (OracleParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value. 
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        #region ExecuteNonQuery命令

        /// <summary> 
        /// 执行指定连接字符串,类型的OracleCommand. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders"); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名称或SQL语句</param> 
        /// <returns>返回命令影响的行数</returns> 
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary> 
        /// 执行指定连接字符串,类型的OracleCommand.如果没有提供参数,不返回结果. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名称或SQL语句</param> 
        /// <param name="commandParameters">OracleParameter参数数组</param> 
        /// <returns>返回命令影响的行数</returns> 
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary> 
        /// 执行指定数据库连接对象的命令 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders"); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary> 
        /// 执行指定数据库连接对象的命令 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">T存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // 创建OracleCommand命令,并进行预处理 
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Finally, execute the command 
            int retval = cmd.ExecuteNonQuery();

            // 清除参数,以便再次使用. 
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        /// <summary> 
        /// 执行带事务的OracleCommand. 
        /// </summary> 
        /// <remarks> 
        /// 示例.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders"); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (OracleParameter[])null);
        }

        /// <summary> 
        /// 执行带事务的OracleCommand(指定参数). 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 预处理 
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // 执行 
            int retval = cmd.ExecuteNonQuery();

            // 清除参数集,以便再次使用. 
            cmd.Parameters.Clear();
            return retval;
        }

        #endregion ExecuteNonQuery方法结束
    }
}

