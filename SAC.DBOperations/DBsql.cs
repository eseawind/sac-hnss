using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;
using SAC.Helper;
using System.IO;

namespace SAC.DBOperations
{
    public class DBsql
    {
        public DBsql()
        {
        }
        static public SqlConnection GetConnection()
        {
            StringBuilder constr = new StringBuilder();

            constr.Append("server=" + IniHelper.ReadIniData("RelationDBbase", "DBIP", null) + ";");
            constr.Append("uid=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";");//ConfigurationManager.AppSettings["Uid"]
            constr.Append("pwd=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + ";");//ConfigurationManager.AppSettings["Pwd"]
            constr.Append("database=" + IniHelper.ReadIniData("RelationDBbase", "DBName", null));// ConfigurationManager.AppSettings["DBName"]
            SqlConnection connect = new SqlConnection(constr.ToString());
            connect.Open();
            return connect;

        }

        static public string GetConnectionstr()
        {
            StringBuilder constr = new StringBuilder();

            string conn = "server=" + IniHelper.ReadIniData("RelationDBbase", "DBIP", null) + ";" +
            "uid=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";" +
            "pwd=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + ";" +
            "database=" + IniHelper.ReadIniData("RelationDBbase", "DBName", null);
            return conn;

        }

        static public void RunCommand(ArrayList list, out string errMsg)
        {
            errMsg = "";
            SqlConnection connect = GetConnection();

            try
            {
                SqlCommand cmd;
                for (int i = 0; i < list.Count; i++)
                {
                    cmd = new SqlCommand(list[i].ToString(), connect);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                errMsg = e.Message;

            }
            finally
            {
                CloseConnection(connect);
            }
        }

        static public void CloseConnection(SqlConnection connect)
        {
            connect.Close();
        }

        static public bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            SqlConnection connect = GetConnection();
            try
            {
                SqlHelper.ExecuteNonQuery(connect, CommandType.Text, sqlCmd);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return false;
            }
            finally
            {
                CloseConnection(connect);

            }
            return true;
        }

        static public object RunSingle(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            SqlConnection connect = GetConnection();
            try
            {
                return SqlHelper.ExecuteScalar(connect, CommandType.Text, sqlCmd);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
            finally
            {
                CloseConnection(connect);
            }
        }

        static public DataTable RunDataTable(string sql, SqlParameter[] parms, out string errMsg)
        {

            errMsg = "";
            SqlConnection connect = GetConnection();
            SqlDataReader dr = null;
            DataTable dt = null;
            object[] value = null;
            SqlCommand comm = new SqlCommand(sql, connect);
            try
            {


                for (int i = 0; i < parms.Length; i++)
                {
                    comm.Parameters.Add(parms[i]);
                }
                dr = comm.ExecuteReader();
                if (dr.HasRows)
                {
                    dt = CreateTableBySchemaTable(dr.GetSchemaTable());
                    value = new object[dr.FieldCount];
                    while (dr.Read())
                    {
                        dr.GetValues(value);
                        dt.LoadDataRow(value, true);
                    }
                    value = null;
                }

            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
            finally
            {
                //if (dr.IsClosed)
                //{
                //    dr.Close();
                //}
                CloseConnection(connect);
                comm.Dispose();

            }
            return dt;
        }

        public static DataTable CreateTableBySchemaTable(DataTable pSchemaTable)
        {
            DataTable dtReturn = new DataTable();
            DataColumn dc = null;
            DataRow dr = null;

            for (int i = 0; i < pSchemaTable.Rows.Count; i++)
            {
                dr = pSchemaTable.Rows[i];
                dc = new DataColumn(dr["ColumnName"].ToString(), dr["DataType"] as Type);
                dtReturn.Columns.Add(dc);
            }

            dr = null;
            dc = null;

            return dtReturn;
        }

        static public DataRow RunDataRow(string sqlCmd, out string errMsg)
        {
            DataTable table = RunDataTable(sqlCmd, out errMsg);
            if (table == null || table.Rows == null || table.Rows.Count == 0)
            {
                errMsg = string.Format("查询{0}返回数据集为空", sqlCmd);
                return null;
            }
            else
                return table.Rows[0];
        }

        static public DataTable RunDataTable(string sqlCmd, out string errMsg)
        {
            DataSet ds = RunDataSet(sqlCmd, out errMsg);
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0)
            {
                errMsg = string.Format("查询{0}返回数据集为空", sqlCmd);
                return null;
            }
            else
                return ds.Tables[0];
        }

        static public DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            SqlConnection connect = GetConnection();
            try
            {
                return SqlHelper.ExecuteDataset(connect, CommandType.Text, sqlCmd);
            }
            catch (Exception e)
            {

                errMsg = e.Message;
                return null;
            }
            finally
            {
                CloseConnection(connect);
            }
        }

        static public bool RunNonQuery_SP(string sp_name, SqlCommand comm, out string errMsg)
        {
            errMsg = "";
            SqlConnection connect = GetConnection();
            try
            {
                comm.Connection = connect;
                comm.CommandText = sp_name;
                comm.CommandType = CommandType.StoredProcedure;
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
                errMsg = e.Message;

                return false;
            }
            finally
            {
                CloseConnection(connect);
            }
            return true;
        }

        static public object RunSingle_SP(string sp_name, SqlCommand comm, out string errMsg)
        {
            errMsg = "";
            SqlConnection connect = GetConnection();
            try
            {
                comm.Connection = connect;
                comm.CommandText = sp_name;
                comm.CommandType = CommandType.StoredProcedure;
                return comm.ExecuteScalar();

            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
            finally
            {
                CloseConnection(connect);
            }
        }

        static public DataRow RunDataRow_SP(string sp_name, SqlCommand comm, out string errMsg)
        {
            DataTable table = RunDataTable_SP(sp_name, comm, out errMsg);
            if (table == null || table.Rows == null || table.Rows.Count == 0)
            {
                errMsg = string.Format("存储过程{0}返回数据集为空", sp_name);
                return null;
            }
            else
                return table.Rows[0];
        }

        static public DataTable RunDataTable_SP(string sp_name, SqlCommand comm, out string errMsg)
        {
            errMsg = "";
            SqlConnection connect = GetConnection();
            try
            {
                comm.Connection = connect;
                comm.CommandText = sp_name;
                comm.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = comm.ExecuteReader();
                if (reader == null)
                    return null;
                DataTable dt = new DataTable();
                DataColumn[] cols = new DataColumn[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    cols[i] = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                dt.Columns.AddRange(cols);

                while (reader.Read())
                {
                    object[] values = new object[reader.FieldCount];
                    reader.GetValues(values);
                    dt.Rows.Add(values);
                }
                return dt;
            }
            catch (Exception e)
            {
                errMsg = e.Message;
                return null;
            }
            finally
            {
                CloseConnection(connect);
            }
        }

        ///// <summary>
        ///// 分页显示数据
        ///// </summary>
        ///// <param name="select">需要的参数类</param>
        ///// <param name="errMsg"></param>
        ///// <returns></returns>
        //static public DataTable RunDataTable_SP(Model.SelsecBE select, out string errMsg)
        //{
        //    SqlCommand comm = new SqlCommand();
        //    AddInParamToSqlCommand(comm, "@pagesize", select.PageSize);
        //    AddInParamToSqlCommand(comm, "@pageIndex", select.PageIndex);
        //    AddInParamToSqlCommand(comm, "@Pramfield", select.Pramfield);
        //    AddInParamToSqlCommand(comm, "@tableName", select.TtableName);
        //    AddInParamToSqlCommand(comm, "@getfield", select.GetField);
        //    AddInParamToSqlCommand(comm, "@strwhere", select.StrWhere);
        //    AddInParamToSqlCommand(comm, "@strorder", select.StrOrder);
        //    //SqlParameter outstr = AddOutParamToSqlCommand(comm, "@strout", SqlDbType.VarChar);
        //    //str = outstr.Value.ToString();
        //    return RunDataTable_SP("getdata", comm, out errMsg);
        //}

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="ID_KEY"></param>
        /// <param name="bName"></param>
        /// <param name="qsrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="BZ"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        static public object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, out string errMsg)
        {
            SqlCommand comm = new SqlCommand();
            AddInParamToSqlCommand(comm, "@ID_KEY", ID_KEY);
            AddInParamToSqlCommand(comm, "@qsrq", qsrq);
            AddInParamToSqlCommand(comm, "@jsrq", jsrq);
            AddInParamToSqlCommand(comm, "@bz", bz);

            SqlParameter outstr = AddOutParamToSqlCommand(comm, "@value", SqlDbType.VarChar, 50);

            RunSingle_SP(sp_name, comm, out errMsg); //RunDataTable_SP("getdata", comm, out err

            return outstr.Value.ToString();
        }

        /// <summary>
        /// 调用存储过程[多班值值报]
        /// </summary>
        /// <param name="ID_KEY"></param>
        /// <param name="bName"></param>
        /// <param name="qsrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="BZ"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        static public object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, string paras, string value, out string errMsg)
        {
            SqlCommand comm = new SqlCommand();
            AddInParamToSqlCommand(comm, "@ID_KEY", ID_KEY);
            AddInParamToSqlCommand(comm, "@qsrq", qsrq);
            AddInParamToSqlCommand(comm, "@jsrq", jsrq);
            AddInParamToSqlCommand(comm, "@bz", bz);

            if (paras != null && paras != "")
            {
                string str = "";
                string strRes = "";
                string[] arr = null;
                string[] val = null;

                if (paras.Contains(","))
                    arr = paras.Split(',');
                else
                {
                    arr = new string[1];
                    arr[0] = paras;
                }

                if (value.Contains(","))
                    val = value.Split(',');
                else
                {
                    val = new string[1];
                    val[0] = value;
                }

                for (int i = 0; i < arr.Length; i++)
                {
                    str = "@" + arr[i].ToString();
                    strRes = val[i].ToString();

                    AddInParamToSqlCommand(comm, str, strRes);
                }
            }

            SqlParameter outstr = AddOutParamToSqlCommand(comm, "@value", SqlDbType.VarChar, 50);

            RunSingle_SP(sp_name, comm, out errMsg); //RunDataTable_SP("getdata", comm, out err

            return outstr.Value.ToString();
        }

        static public bool RunRowExist(string sqlCmd)
        {
            string errMsg;
            DataRow dr = RunDataRow(sqlCmd, out errMsg);
            if (dr != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static public int RunRowCount(string sqlCmd, out string errMsg)
        {
            object o = RunSingle(sqlCmd, out errMsg);
            if (o == null) return -1;
            return intParse(o.ToString());
        }

        static public int intParse(string value)
        {
            return (value.Length == 0) ? -1 : int.Parse(value);
        }

        static public double doubleParse(string value)
        {
            return (value.Length == 0) ? -1 : double.Parse(value);
        }

        static public bool boolParse(string value)
        {
            return (value.Length == 0) ? false : bool.Parse(value);
        }

        static public DateTime datetimeParse(string value)
        {
            return (value.Length == 0) ? DateTime.Now : DateTime.Parse(value);
        }

        static public string BuildArrayStr(ArrayList array)
        {
            string result = "";
            if (array.Count == 0) return result;
            result += "(";
            foreach (object o in array)
                result += o.ToString() + ',';
            result = result.TrimEnd(',');
            result += ")";
            return result;
        }

        /// <summary>
        /// 处理形如'{0}'，而参数中又有单引号的情况，将参数中的单引号double
        /// 如果args中的单引号不是作为文本的话，不要用这个方法，直接用string.format()
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static public string stringFormat(string format, params object[] args)
        {
            if (format.IndexOf("'") == -1) //没有在单引号内的占位符
                return string.Format(format, args);

            ArrayList al = new ArrayList();
            for (int i = 0; i < args.Length; i++)
                al.Add(args[i].ToString().Replace("'", "''")); // 处理单引号
            return string.Format(format, al.ToArray());
        }

        /// <summary>
        /// 对于单项检测是否存在
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        static public bool FindColumn(string table, string column, string value, out string errMsg)
        {
            string sql = string.Format("select {1} from {0} where {1} = '{2}'", table, column, value);
            DataRow dr = RunDataRow(sql, out errMsg);
            return (dr != null);
        }

        static public bool DelItem(string table, int id, out string errMsg)
        {
            string sql = string.Format("delete from {0} where id = {1}", table, id);
            return RunNonQuery(sql, out errMsg);
        }

        static public bool DelItems(string table, ArrayList ids, out string errMsg)
        {
            errMsg = "id列表为空";
            string idStr = BuildArrayStr(ids);
            if (idStr.Length == 0) return false;
            string sql = string.Format("delete from {0} where id in {1}", table, idStr);
            return RunNonQuery(sql, out errMsg);
        }

        static public SqlParameter AddInParamToSqlCommand(SqlCommand comm, string pName, object pValue)
        {
            SqlParameter param = new SqlParameter(pName, pValue);
            comm.Parameters.Add(param);
            return param;
        }

        static public SqlParameter AddOutParamToSqlCommand(SqlCommand comm, string pName, SqlDbType pType)
        {
            SqlParameter param = new SqlParameter(pName, pType);
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);
            return param;
        }

        /// VarChar和NVarChar等字符串类型需要Size参数
        static public SqlParameter AddOutParamToSqlCommand(SqlCommand comm, string pName, SqlDbType pType, int size)
        {
            SqlParameter param = new SqlParameter(pName, pType, size);
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);
            return param;
        }
        public static string ExecuteNonQuery(string sqlText, SqlParameter[] parameters, out string errMsg)
        {
            errMsg = string.Empty;
            var connect = GetConnection();
            var cmd = new SqlCommand(sqlText, connect);
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
                connect.Close();
                connect.Dispose();
            }

            return errMsg;
        }

        /// <summary> 
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数 
        /// </summary> 
        /// <param name="command">要处理的SqlCommand</param> 
        /// <param name="connection">数据库连接</param> 
        /// <param name="transaction">一个有效的事务或者是null值</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param> 
        /// <param name="commandParameters">和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param> 
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param> 
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
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
        /// 将SqlParameter参数数组(参数值)分配给SqlCommand命令. 
        /// 这个方法将给任何一个参数分配DBNull.Value; 
        /// 该操作将阻止默认值的使用. 
        /// </summary> 
        /// <param name="command">命令名</param> 
        /// <param name="commandParameters">SqlParameters数组</param> 
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
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
        /// 执行指定连接字符串,类型的SqlCommand. 
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
            return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary> 
        /// 执行指定连接字符串,类型的SqlCommand.如果没有提供参数,不返回结果. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名称或SQL语句</param> 
        /// <param name="commandParameters">SqlParameter参数数组</param> 
        /// <returns>返回命令影响的行数</returns> 
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
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
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary> 
        /// 执行指定数据库连接对象的命令 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">T存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // 创建SqlCommand命令,并进行预处理 
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Finally, execute the command 
            int retval = cmd.ExecuteNonQuery();

            // 清除参数,以便再次使用. 
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        /// <summary> 
        /// 执行带事务的SqlCommand. 
        /// </summary> 
        /// <remarks> 
        /// 示例.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders"); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回影响的行数/returns> 
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary> 
        /// 执行带事务的SqlCommand(指定参数). 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 预处理 
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            // 执行 
            int retval = cmd.ExecuteNonQuery();

            // 清除参数集,以便再次使用. 
            cmd.Parameters.Clear();
            return retval;
        }

        #endregion ExecuteNonQuery方法结束

        /// <summary> 
        /// 根据提供的数据库表名称，列名，整数S和E给出该表的第S到第E条记录 
        /// </summary> 
        /// <param name="tableName">数据库表名称</param> 
        /// <param name="cName">需要得到的列的名称的数组</param> 
        /// <param name="orderName">用于排序的字段名称</param> 
        /// <param name="sCount">开始的记录的行数</param> 
        /// <param name="eCount">结束的记录的行数</param> 
        /// <returns>返回所有记录的集合</returns> 
        static public DataTable GetS2Enotes(string tableName, string[] cName, string orderName, int sCount, int eCount)
        {
            int t1 = eCount - sCount + 1;
            int t2 = sCount - 1;
            string sqlSQL = "SELECT TOP " + t1 + " ";
            for (int i = 0; i < cName.Count(); i++)
            {
                sqlSQL = sqlSQL + cName[i] + ",";
            }
            sqlSQL = sqlSQL.TrimEnd(',');
            sqlSQL = sqlSQL + "* FROM " + tableName + " WHERE (" + orderName + " NOT IN (SELECT TOP " + t2 + " " + orderName + " id FROM " + tableName + "))";

            string errMsg;
            DataTable dt = null;
            try
            {
                dt = RunDataTable(sqlSQL, out errMsg);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogHelper.EnLogType.Run, "发生时间：" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "/n错误信息：" + ex.Message);
            }
            return dt;
        }

        /// <summary> 
        /// 将流文件以二进制形式存进数据库，执行此方法之前要确保存在记录，因为二进制文件只能以updata方式存储
        /// </summary> 
        /// <param name="tableName">数据库表名称</param> 
        /// <param name="tabCName">用于标示字段的列名称</param> 
        /// <param name="tabName">用于标示字段的真实记录</param> 
        /// <param name="fileCName">存储文件字段的列名称</param> 
        /// <param name="fileBytes">要存储的文件流</param> 
        /// <param name="errMsg">返回错误信息</param> 
        /// <returns>返回是否成功</returns> 
        static public bool RetBoolUpFile(string tableName, string tabCName, string tabName, string fileCName, byte[] fileBytes, out string errMsg)
        {
            errMsg = "";
            bool flag = false;
            if (fileBytes.Length > 0)
            {
                string sql = "update T_SYS_MENU set " + fileCName + "=@xmlfile where " + tabCName + "='" + tabName + "'";

                SqlConnection sqlconn = new SqlConnection(DBsql.GetConnectionstr());
                try
                {
                    sqlconn.Open();
                    SqlCommand sqlcmd = new SqlCommand(sql, sqlconn);
                    sqlcmd.Parameters.Add("@xmlfile", fileBytes);
                    if (sqlcmd.ExecuteNonQuery() > 0)
                        flag = true;

                    sqlconn.Close();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }
                finally { sqlconn.Close(); }
            }
            return flag;
        }

        /// <summary> 
        /// 根据提供的数据库表名称,给出该表所有记录的条数 
        /// </summary> 
        /// <param name="tableName">数据库表名称</param> 
        /// <returns>返回所有记录的条数</returns> 
        static public int GetCount(string tableName)
        {
            string errMsg = "";
            int count = 0;
            string sql = "select count(*) as c  from " + tableName + "";
            count = RunRowCount(sql, out errMsg);
            return count;
        }

        static public bool DownLoadXml(string fileID, string filePath)
        {
            bool ret = true;
            string errMsg = "";
            try
            {
                SqlConnection sqlconn = new SqlConnection(DBsql.GetConnectionstr());
                string sqlstr = "select * from T_SYS_MENU where T_XMLID='" + fileID + "'";
                SqlCommand sqlcmd = new SqlCommand(sqlstr, sqlconn);
                SqlDataReader sqlreader = sqlcmd.ExecuteReader();
                string FileName = filePath;
                if (!sqlreader.Read())
                {
                    FileName = "";
                }
                else
                {
                    byte[] bytes = (byte[])sqlreader["B_XML"];
                    FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }
                sqlreader.Close();
                sqlconn.Close();
            }
            catch (Exception ce)
            {
                errMsg = ce.Message;
                ret = false;
            }
            return ret;
        }
    }
}
