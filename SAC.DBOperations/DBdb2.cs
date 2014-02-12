using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;
using System.Data;
using IBM.Data.DB2;
using SAC.Helper;
using System.IO;

namespace SAC.DBOperations
{
    public class DBdb2
    {
        public DBdb2()
        { }
        public static string RetConString()
        {
            //<add name="OracleConnectionString1" connectionString="Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.15.11)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl.unipower.cn)(instance_name=orcl)));uid=scott;pwd=sacsis;"/>

            string conn = "Provider=IBMDADB2;Data Source==(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.15.11)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl.unipower.cn)(instance_name=orcl)));uid=scott;pwd=sacsis";
            //string conn = "Provider=IBMDADB2;Data Source= " + IniHelper.ReadIniData("RelationDBbase", "DBName", null) + ";User ID=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";Password=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + ";Default Collection =alfm01;Default Schema=Schema";
            //string conn = "Provider=IBMDADB2;Data Source=CPI;User ID=ADMINISTRATOR;Password=sacsis;";//连自己电脑
            return conn;
        }
        public static string SetConString()
        {
            string conn = "Provider=IBMDADB2;Data Source= " + IniHelper.ReadIniData("RelationDBbase", "DBName", null) + ";User ID=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";Password=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + ";Default Collection =alfm01;Default Schema=Schema";
            //string conn = "Provider=IBMDADB2;Database=ITST;Hostname=172.18.135.15;Protocol=TCPIP; Port=50000;Uid=Administrator;Pwd=eagle123*;";
            //string conn = "Provider=IBMDADB2;Database=ITST;Hostname=10.178.218.241;Protocol=TCPIP; Port=50000;Uid=Administrator;Pwd=df123456;";//连财哥电脑
            //string conn = "Provider=IBMDADB2;Data Source=CPI;User ID=ADMINISTRATOR;Password=sacsis;";//连自己电脑
            //string conn = "Provider=IBMDADB2;Database=" + IniHelper.ReadIniData("RelationDBbase", "DBName", null) + ";Hostname=" + IniHelper.ReadIniData("RelationDBbase", "DBIP", null) + ";Protocol=TCPIP; Port=50000;Uid=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";Pwd=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + ";";
            return conn;
        }

        static public int RunCommand(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            string conn = RetConString();

            using (OleDbConnection connection = new OleDbConnection(conn))
            {
                using (OleDbCommand cmd = new OleDbCommand(sqlCmd, connection))
                {
                    try
                    {
                        connection.Open();
                        int i = cmd.ExecuteNonQuery();
                        return i;
                    }
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        errMsg = e.Message;
                        return 0;
                    }
                    finally { connection.Close(); }
                }
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

        static public OleDbConnection GetConn()
        {
            //string conn = "Provider=IBMDADB2;Data Source= " + IniHelper.ReadIniData("RelationDBbase", "DBName", null) + ";User ID=" + IniHelper.ReadIniData("RelationDBbase", "DBUser", null) + ";Password=" + IniHelper.ReadIniData("RelationDBbase", "DBPwd", null) + ";Default Collection =alfm01;Default Schema=Schema";
            ////System.Configuration.ConfigurationSettings.AppSettings["conStr"];
            string conn = RetConString();
            OleDbConnection olecon = new OleDbConnection(conn);
            if (olecon != null)
            {
                olecon.Open();
            }

            return olecon;
        }

        static public bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            errMsg = "";

            string conn = RetConString();

            using (OleDbConnection connection = new OleDbConnection(conn))
            {
                using (OleDbCommand cmd = new OleDbCommand(sqlCmd, connection))
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

            OleDbCommand comm = new OleDbCommand();

            AddInParamToSqlCommand(comm, "@ID_KEY", ID_KEY);
            AddInParamToSqlCommand(comm, "@qsrq", qsrq);
            AddInParamToSqlCommand(comm, "@jsrq", jsrq);
            AddInParamToSqlCommand(comm, "@bz", bz);

            OleDbParameter outstr = AddOutParamToSqlCommand(comm, "@value", OleDbType.VarChar, 50);

            RunSingle_SP(sp_name, comm, out errMsg); 

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
            OleDbCommand comm = new OleDbCommand();

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

                if (paras.Contains(','))
                    arr = paras.Split(',');
                else
                {
                    arr = new string[1];
                    arr[0] = paras;
                }

                if (value.Contains(','))
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

            OleDbParameter outstr = AddOutParamToSqlCommand(comm, "@value", OleDbType.VarChar, 50);

            RunSingle_SP(sp_name, comm, out errMsg); //RunDataTable_SP("getdata", comm, out err

            return outstr.Value.ToString();
        }

        static public object RunSingle_SP(string sp_name, OleDbCommand comm, out string errMsg)
        {
            errMsg = "";
            OleDbConnection connect = GetConn();
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
                connect.Close();
            }
        }

        /// VarChar和NVarChar等字符串类型需要Size参数
        static public OleDbParameter AddOutParamToSqlCommand(OleDbCommand comm, string pName, OleDbType pType, int size)
        {
            OleDbParameter param = new OleDbParameter(pName, pType, size);
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);
            return param;
        }

        static public OleDbParameter AddInParamToSqlCommand(OleDbCommand comm, string pName, object pValue)
        {
            OleDbParameter param = new OleDbParameter(pName, pValue);
            comm.Parameters.Add(param);
            return param;
        }

        static public OleDbParameter AddOutParamToSqlCommand(OleDbCommand comm, string pName, OleDbType pType)
        {
            OleDbParameter param = new OleDbParameter(pName, pType);
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);
            return param;
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn"></param>
        static protected void CloseConn(OleDbConnection conn)
        {
            conn.Close();
        }

        /// 获取单一对象的值
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="sqlcmd"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        static public object RunSingle(string sqlcmd, out string errMsg)
        {
            errMsg = "";
            string conn = RetConString();

            using (OleDbConnection connection = new OleDbConnection(conn))
            {
                using (OleDbCommand cmd = new OleDbCommand(sqlcmd, connection))
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
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        errMsg = e.Message;
                        return null;
                    }
                    finally { connection.Close(); }
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
            //string conn = System.Configuration.ConfigurationSettings.AppSettings["conStr"];
            string conn = RetConString();
            using (OleDbConnection connection = new OleDbConnection(conn))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
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
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            //string conn = System.Configuration.ConfigurationSettings.AppSettings["conStr"];
            string conn = RetConString();
            using (OleDbConnection connection = new OleDbConnection(conn))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OleDb.OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 返回 DataSet
        /// </summary>
        /// <param name="sqlCmd">sql语句</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        static public DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            OleDbConnection db = GetConn();

            DataSet ds = new DataSet();
            try
            {
                OleDbDataAdapter command = new OleDbDataAdapter(sqlCmd, db);
                command.Fill(ds, "ds");
            }
            catch (System.Data.OleDb.OleDbException ce)
            {
                errMsg = ce.Message;
                return null;
            }
            finally
            {
                CloseConn(db);
            }
            return ds;
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

        #region  执行多条SQL语句，实现数据库事务。
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">ArrayList</param>
        public static void ExecuteSqlTran(ArrayList sqlList, string ConString)
        {
            bool mustCloseConnection = false;
            using (DB2Connection conn = new DB2Connection(ConString))
            {
                conn.Open();
                using (DB2Transaction trans = conn.BeginTransaction())
                {
                    DB2Command cmd = new DB2Command();
                    try
                    {
                        for (int i = 0; i < sqlList.Count; i++)
                        {
                            string cmdText = sqlList[i].ToString();
                            PrepareCommand(cmd, conn, trans, CommandType.Text, cmdText, null, out mustCloseConnection);
                            int val = cmd.ExecuteNonQuery();

                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                        cmd.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param name="command">要处理的db2Command</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">一个有效的事务或者是null值</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param>
        /// <param name="commandParameters">和命令相关联的db2Parameter参数数组,如果没有参数为'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
        private static void PrepareCommand(DB2Command command, DB2Connection connection, DB2Transaction transaction, CommandType commandType, string commandText, DB2Parameter[] commandParameters, out bool mustCloseConnection)
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
        /// 将db2Parameter参数数组(参数值)分配给db2Command命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>
        /// <param name="command">命令名</param>
        /// <param name="commandParameters">DB2Parameters数组</param>

        private static void AttachParameters(DB2Command command, DB2Parameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (DB2Parameter p in commandParameters)
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


        #endregion

        public static string ExecuteNonQuery(string sqlText, OleDbParameter[] parameters, out string errMsg)
        {
            errMsg = string.Empty;
            var connect = GetConn();
            var cmd = new OleDbCommand(sqlText, connect);
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
        /// <param name="command">要处理的OleDbCommand</param> 
        /// <param name="connection">数据库连接</param> 
        /// <param name="transaction">一个有效的事务或者是null值</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param> 
        /// <param name="commandParameters">和命令相关联的OleDbParameter参数数组,如果没有参数为'null'</param> 
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param> 
        private static void PrepareCommand(OleDbCommand command, OleDbConnection connection, OleDbTransaction transaction, CommandType commandType, string commandText, OleDbParameter[] commandParameters, out bool mustCloseConnection)
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
        /// 将OleDbParameter参数数组(参数值)分配给OleDbCommand命令. 
        /// 这个方法将给任何一个参数分配DBNull.Value; 
        /// 该操作将阻止默认值的使用. 
        /// </summary> 
        /// <param name="command">命令名</param> 
        /// <param name="commandParameters">OleDbParameters数组</param> 
        private static void AttachParameters(OleDbCommand command, OleDbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (OleDbParameter p in commandParameters)
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
        /// 执行指定连接字符串,类型的OleDbCommand. 
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
            return ExecuteNonQuery(connectionString, commandType, commandText, (OleDbParameter[])null);
        }

        /// <summary> 
        /// 执行指定连接字符串,类型的OleDbCommand.如果没有提供参数,不返回结果. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名称或SQL语句</param> 
        /// <param name="commandParameters">OleDbParameter参数数组</param> 
        /// <returns>返回命令影响的行数</returns> 
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OleDbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (OleDbConnection connection = new OleDbConnection(connectionString))
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
        public static int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (OleDbParameter[])null);
        }

        /// <summary> 
        /// 执行指定数据库连接对象的命令 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">T存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText, params OleDbParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // 创建OleDbCommand命令,并进行预处理 
            OleDbCommand cmd = new OleDbCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OleDbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Finally, execute the command 
            int retval = cmd.ExecuteNonQuery();

            // 清除参数,以便再次使用. 
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        /// <summary> 
        /// 执行带事务的OleDbCommand. 
        /// </summary> 
        /// <remarks> 
        /// 示例.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders"); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回影响的行数/returns> 
        public static int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (OleDbParameter[])null);
        }

        /// <summary> 
        /// 执行带事务的OleDbCommand(指定参数). 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OleDbParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型(存储过程,命令文本或其它.)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText, params OleDbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 预处理 
            OleDbCommand cmd = new OleDbCommand();
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
        static public DataTable GetS2Enotes(string tableName, string[] cName,string orderName,int sCount, int eCount)
        {
            string sqlDB2 = "select * from (select ";
            for(int i=0;i<cName.Count();i++)
            {
                sqlDB2 = sqlDB2 + cName[i] + ",";
            }
            sqlDB2 = sqlDB2 + "rownumber() over(order by " + orderName + " asc ) as rowid  from " + tableName + ")as a where a.rowid between " + sCount + " and " + eCount + "";
            string errMsg;
            DataTable dt = null;
            try
            {
                dt = RunDataTable(sqlDB2, out errMsg);
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
                string sql = "update T_SYS_MENU set " + fileCName + "=? where " + tabCName + "='" + tabName + "'";

                OleDbConnection con = new OleDbConnection(DBdb2.SetConString());
                try
                {
                    con.Open();
                    OleDbCommand oledbcom = new OleDbCommand(sql, con);

                    oledbcom.Parameters.Add("?", fileBytes);

                    if (oledbcom.ExecuteNonQuery() > 0)
                        flag = true;

                    con.Close();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }
                finally { con.Close(); }
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
            string sql = "select count(*) from " + tableName + "";
            count = RunRowCount(sql, out errMsg);
            return count;
        }

        static public bool DownLoadXml(string fileID, string filePath)
        {
            bool ret = true;
            string errMsg="";
            try
            {
                OleDbConnection db2conn = new OleDbConnection(DBdb2.SetConString());
                string sqlstr = "select * from T_SYS_MENU where T_XMLID='" + fileID + "'";
                OleDbCommand db2cmd = new OleDbCommand(sqlstr, db2conn);
                db2conn.Open();
                OleDbDataReader db2reader = db2cmd.ExecuteReader();
                string FileName = filePath;
                if (!db2reader.Read())
                {
                    FileName = "";
                }
                else
                {
                    byte[] bytes = (byte[])db2reader["B_XML"];
                    FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }
                db2reader.Close();
                db2cmd.Dispose();
                db2conn.Close();
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
