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
    public class DALBASE_PLANT
    {
        DBLink dl = new DBLink();
        string errMsg = "";
        private DataTable Get_COMID()
        {
            DataTable dt = null;
            string sql = "select T_COMID from t_base_plant group by T_COMID";
            dt = dl.RunDataTable(sql, out errMsg);
            return dt;
        }
    }
}
