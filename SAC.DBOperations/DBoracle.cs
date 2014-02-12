using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SAC.Helper;
using System.Data.OracleClient;
using System.IO;

namespace SAC.DBOperations
{
    public class DBoracle
    {
        public DBoracle()
        {
        }

        static public int RunRowCount(string sqlCmd, out string errMsg)
        {
            object o = RunSingle(sqlCmd, out errMsg);
            if (o == null) return -1;
            return intParse(o.ToString());
        }

        static public bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            errMsg = "";

            string conn = OracleHelper.retStr();

            using (OracleConnection connection = new OracleConnection(conn))
            {
                using (OracleCommand omd = new OracleCommand(sqlCmd, connection))
                {
                    try
                    {
                        connection.Open();
                        omd.ExecuteNonQuery();
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

        static public int intParse(string value)
        {
            return (value.Length == 0) ? -1 : int.Parse(value);
        }

        static public object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, out string errMsg)
        {

            OracleCommand comm = new OracleCommand();

            
            AddInParamToSqlCommand(comm, "@ID_KEY", ID_KEY);
            AddInParamToSqlCommand(comm, "@qsrq", qsrq);
            AddInParamToSqlCommand(comm, "@jsrq", jsrq);
            AddInParamToSqlCommand(comm, "@bz", bz);

            OracleParameter outstr = AddOutParamToSqlCommand(comm, "@value", OracleType.VarChar, 50);

            RunSingle_SP(sp_name, comm, out errMsg); //RunDataTable_SP("getdata", comm, out err

            return outstr.Value.ToString();
        }

        static public object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, string paras, string value, out string errMsg)
        {
            OracleCommand comm = new OracleCommand();

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

            OracleParameter outstr = AddOutParamToSqlCommand(comm, "@value", OracleType.VarChar, 50);

            RunSingle_SP(sp_name, comm, out errMsg); //RunDataTable_SP("getdata", comm, out err

            return outstr.Value.ToString();
        }

        static public object RunSingle_SP(string sp_name, OracleCommand comm, out string errMsg)
        {
            errMsg = "";
            string conn = OracleHelper.retStr();
            OracleConnection connect = new OracleConnection(conn);
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

        static public object RunSingle(string sqlcmd, out string errMsg)
        {
            errMsg = "";
            string conn = OracleHelper.retStr();

            using (OracleConnection connection = new OracleConnection(conn))
            {
                using (OracleCommand omd = new OracleCommand(sqlcmd, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = omd.ExecuteScalar();
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
                        errMsg = e.Message;
                        return null;
                    }
                    finally { connection.Close(); }
                }
            }
        }

        static public DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            string conn = OracleHelper.retStr();
            OracleConnection connection = new OracleConnection(conn);
            DataSet ds = new DataSet();
            try
            {
                OracleDataAdapter command = new OracleDataAdapter(sqlCmd, connection);
                command.Fill(ds, "ds");
            }
            catch (System.Data.OracleClient.OracleException ce)
            {
                errMsg = ce.Message;
                return null;
            }
            finally
            {
                connection.Close();
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

        static public OracleParameter AddInParamToSqlCommand(OracleCommand comm, string pName, object pValue)
        {
            OracleParameter param = new OracleParameter(pName, pValue);
            comm.Parameters.Add(param);
            return param;
        }
        static public OracleParameter AddOutParamToSqlCommand(OracleCommand comm, string pName, OracleType pType, int size)
        {
            OracleParameter param = new OracleParameter(pName, pType, size);
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);
            return param;
        }
        static public OracleParameter AddOutParamToSqlCommand(OracleCommand comm, string pName, OracleType pType)
        {
            OracleParameter param = new OracleParameter(pName, pType);
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);
            return param;
        }

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
            string sql = "select * from (select ";           
            for (int i = 0; i < cName.Count(); i++)
            {
                sql = sql + cName[i] + ",";
            }
            sql = sql + "ROWNUM rn from " + tableName + " WHERE ROWNUM <= " + eCount + ") WHERE rn >= " + sCount + "";
            string errMsg;
            DataTable dt = null;
            try
            {
                dt = RunDataTable(sql, out errMsg);
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
                string sql = "update T_SYS_MENU set " + fileCName + "=:blobtodb where " + tabCName + "='" + tabName + "'";
                string conn = OracleHelper.retStr();

                OracleConnection con = new OracleConnection(conn);
                try
                {
                    con.Open();
                    OracleCommand orlcmd = new OracleCommand(sql, con);
                    orlcmd.Parameters.Add("blobtodb", fileBytes);
                    if (orlcmd.ExecuteNonQuery() > 0)
                    {
                        flag = true;
                    }
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
            string errMsg = "";
            try
            {
                string conn = OracleHelper.retStr();

                OracleConnection con = new OracleConnection(conn);
                string sqlstr = "select * from T_SYS_MENU where T_XMLID='" + fileID + "'";
                OracleCommand Orlcmd = new OracleCommand(sqlstr, con);
                con.Open();
                OracleDataReader Orlreader = Orlcmd.ExecuteReader();
                string FileName = filePath;
                if (!Orlreader.Read())
                {
                    FileName = "";
                }
                else
                {
                    byte[] bytes = (byte[])Orlreader["B_XML"];
                    FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }
                Orlreader.Close();
                con.Close();
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

