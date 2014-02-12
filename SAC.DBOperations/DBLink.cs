using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAC.Helper;
using System.Data;

namespace SAC.DBOperations
{
    public class DBLink
    {
        /// <summary>
        /// 数据库操作类，外部调用数据库操作方法时直接调用在此类中定义的方法
        /// </summary>
        string rlDBType = "";
        DBHelper dbLink;
        public DBLink()
        {
            string DBtype = this.init();
            if (DBtype == "DB2")
            {
                dbLink = new DB2();
            }
            else if (DBtype == "SQL")
            {
                dbLink = new SQL();
            }
            else if (DBtype == "ORACLE")
            {
                dbLink = new ORACLE();
            }          
        }

        public int RunRowCount(string sqlCmd, out string errMsg)
        {
            return dbLink.RunRowCount(sqlCmd, out errMsg);
        }
        public bool RunNonQuery(string sqlCmd, out string errMsg)
        {
            return dbLink.RunNonQuery(sqlCmd, out errMsg);
        }
        public object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, out string errMsg)
        {
            return dbLink.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, out errMsg);
        }
        public object RunSingle_SP(string sp_name, string ID_KEY, string qsrq, string jsrq, string bz, string paras, string value, out string errMsg)
        {
            return dbLink.RunSingle_SP(sp_name, ID_KEY, qsrq, jsrq, bz, paras, value, out errMsg);
        }
        public object RunSingle(string sqlcmd, out string errMsg)
        {
            return dbLink.RunSingle(sqlcmd, out errMsg);
        }
        public DataSet RunDataSet(string sqlCmd, out string errMsg)
        {
            return dbLink.RunDataSet(sqlCmd, out errMsg);
        }
        public DataTable RunDataTable(string sqlCmd, out string errMsg)
        {
            return dbLink.RunDataTable(sqlCmd, out errMsg);
        }
        public DataRow RunDataRow(string sqlCmd, out string errMsg)
        {
            return dbLink.RunDataRow(sqlCmd, out errMsg);
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
        public DataTable GetS2Enotes(string tableName, string[] cName, string orderName, int sCount, int eCount)
        {
            return dbLink.GetS2Enotes(tableName, cName, orderName, sCount, eCount);
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
        public bool RetBoolUpFile(string tableName, string tabCName, string tabName, string fileCName, byte[] fileBytes, out string errMsg)
        {
            return dbLink.RetBoolUpFile(tableName, tabCName, tabName, fileCName, fileBytes, out errMsg);
        }

        /// <summary> 
        /// 根据提供的数据库表名称,给出该表所有记录的条数 
        /// </summary> 
        /// <param name="tableName">数据库表名称</param> 
        /// <returns>返回所有记录的条数</returns> 
        public int GetCount(string tableName)
        {
            return dbLink.GetCount(tableName);
        }

        /// <summary> 
        /// 将数据库中存储的xml文件下载到工程的指定文件夹里
        /// </summary> 
        /// <param name="fileID">T_XMLID</param> 
        /// <param name="filePath">下载的指定路径</param> 
        public bool DownLoadXml(string fileID, string filePath)
        {
            return dbLink.DownLoadXml(fileID, filePath);
        }

        public string init()
        {
            rlDBType = IniHelper.ReadIniData("RelationDBbase", "DBType", null);
            return rlDBType;
        }
    }
}
