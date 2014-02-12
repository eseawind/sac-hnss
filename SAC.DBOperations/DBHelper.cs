using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAC.DBOperations;
using System.Data;

namespace SAC.DBOperations
{
    public class DBHelper
    {
        /// <summary>
        /// 数据库操作父类，在此类定义数据库操作要用到的方法
        /// </summary>
        public virtual int RunRowCount(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            return 0;
        }
        public virtual bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            return true;
        }
        public virtual object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, out string errMsg)
        {
            errMsg = "";
            return null;
        }
        public virtual object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, string paras, string value, out string errMsg)
        {
            errMsg = "";
            return null;
        }
        public virtual object RunSingle(string sqlcmd, out string errMsg)
        {
            errMsg = "";
            return null;
        }
        public virtual DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            return null;
        }
        public virtual DataTable RunDataTable(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            return null;
        }
        public virtual DataRow RunDataRow(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            return null;
        }

        public virtual DataTable GetS2Enotes(string tableName, string[] cName, string orderName, int sCount, int eCount)
        {
            DataTable dt = null;
            return dt;
        }

        public virtual bool RetBoolUpFile(string tableName, string tabCName, string tabName, string fileCName, byte[] fileBytes, out string errMsg)
        {
            errMsg = "";
            bool flag = false;
            return flag;
        }
        public virtual int GetCount(string tableName)
        {
            return 0;
        }

        public virtual bool DownLoadXml(string fileID, string filePath)
        {
            return true;
        }

    }

    public class DB2 : DBHelper
    {
        public override int RunRowCount(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunRowCount(sqlCmd, out errMsg);
        }
        public override bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunNonQuery(sqlCmd, out errMsg);
        }
        public override object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, out errMsg);
        }
        public override object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, string paras, string value, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, paras, value, out errMsg);
        } 
        public override object RunSingle(string sqlcmd, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunSingle(sqlcmd, out errMsg);
        }
        public override DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunDataSet(sqlCmd, out errMsg);
        }
        public override DataTable RunDataTable(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunDataTable(sqlCmd, out errMsg);
        }
        public override DataRow RunDataRow(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RunDataRow(sqlCmd, out errMsg);
        }
        public override DataTable GetS2Enotes(string tableName, string[] cName, string orderName, int sCount, int eCount)
        {
            return SAC.DBOperations.DBdb2.GetS2Enotes(tableName, cName, orderName, sCount, eCount);
        }
        public override bool RetBoolUpFile(string tableName, string tabCName, string tabName, string fileCName, byte[] fileBytes, out string errMsg)
        {
            return SAC.DBOperations.DBdb2.RetBoolUpFile(tableName, tabCName, tabName, fileCName, fileBytes, out errMsg);
        }
        public override int GetCount(string tableName)
        {
            return SAC.DBOperations.DBdb2.GetCount(tableName);
        }
        public override bool DownLoadXml(string fileID, string filePath)
        {
            return SAC.DBOperations.DBdb2.DownLoadXml(fileID, filePath);
        }
    }

    public class SQL : DBHelper
    {
        public override int RunRowCount(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunRowCount(sqlCmd, out errMsg);
        }
        public override bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunNonQuery(sqlCmd, out errMsg);
        }
        public override object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, out errMsg);
        }
        public override object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, string paras, string value, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, paras, value, out errMsg);
        }
        public override object RunSingle(string sqlcmd, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunSingle(sqlcmd, out errMsg);
        }
        public override DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunDataSet(sqlCmd, out errMsg);
        }
        public override DataTable RunDataTable(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunDataTable(sqlCmd, out errMsg);
        }
        public override DataRow RunDataRow(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RunDataRow(sqlCmd, out errMsg);
        }
        public override DataTable GetS2Enotes(string tableName, string[] cName, string orderName, int sCount, int eCount)
        {
            return SAC.DBOperations.DBsql.GetS2Enotes(tableName, cName, orderName, sCount, eCount);
        }
        public override bool RetBoolUpFile(string tableName, string tabCName, string tabName, string fileCName, byte[] fileBytes, out string errMsg)
        {
            return SAC.DBOperations.DBsql.RetBoolUpFile(tableName, tabCName, tabName, fileCName, fileBytes, out errMsg);
        }
        public override int GetCount(string tableName)
        {
            return SAC.DBOperations.DBsql.GetCount(tableName);
        }
        public override bool DownLoadXml(string fileID, string filePath)
        {
            return SAC.DBOperations.DBsql.DownLoadXml(fileID, filePath);
        }
    }

    public class ORACLE : DBHelper
    {
        public override int RunRowCount(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunRowCount(sqlCmd, out errMsg);
        }
        public override bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunNonQuery(sqlCmd, out errMsg);
        }
        public override object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, out errMsg);
        }
        public override object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, string paras, string value, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, paras, value, out errMsg);
        }
        public override object RunSingle(string sqlcmd, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunSingle(sqlcmd, out errMsg);
        }
        public override DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunDataSet(sqlCmd, out errMsg);
        }
        public override DataTable RunDataTable(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunDataTable(sqlCmd, out errMsg);
        }
        public override DataRow RunDataRow(string sqlCmd, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RunDataRow(sqlCmd, out errMsg);
        }
        public override DataTable GetS2Enotes(string tableName, string[] cName, string orderName, int sCount, int eCount)
        {
            return SAC.DBOperations.DBoracle.GetS2Enotes(tableName, cName, orderName, sCount, eCount);
        }
        public override bool RetBoolUpFile(string tableName, string tabCName, string tabName, string fileCName, byte[] fileBytes, out string errMsg)
        {
            return SAC.DBOperations.DBoracle.RetBoolUpFile(tableName, tabCName, tabName, fileCName, fileBytes, out errMsg);
        }
        public override int GetCount(string tableName)
        {
            return SAC.DBOperations.DBoracle.GetCount(tableName);
        }
        public override bool DownLoadXml(string fileID, string filePath)
        {
            return SAC.DBOperations.DBoracle.DownLoadXml(fileID, filePath);
        }
    }

}
