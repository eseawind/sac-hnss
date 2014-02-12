using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAC.DBLink;

namespace SAC.DBType
{
    public class DBType
    {
        string rlDBType = "";
        string rtDBType = "";

        string pGl1 = "WHSIS.U1APSH.U1A04013";
        string pGl2 = "WHSIS.U2APSH.U2A04013";
        DBHelper dbLink;
        public DBType()
        {
            string DBtype = "SQL";
            if (DBtype == "DB2")
            {
                dbLink = new DB2();
            }
            else if (DBtype == "SQL")
            {
                dbLink = new SQL();
            }
        }

        public int RunCommand(string sqlCmd, out string errMsg)
        {
            return dbLink.RunCommand(sqlCmd,out errMsg);
        }

        private string init()
        {
            rlDBType = IniHelper.ReadIniData("RelationDBbase", "DBType", null);
            rtDBType = IniHelper.ReadIniData("RTDB", "DBType", null);
            pGl1 = IniHelper.ReadIniData("Report", "FH1", null);
            pGl2 = IniHelper.ReadIniData("Report", "FH2", null);

            return rlDBType;
        }

        public string GetDBtype()
        {
            this.init();
            string DBtype = rlDBType;
            return DBtype;
        }
    }
}
