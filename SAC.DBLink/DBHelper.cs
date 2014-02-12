using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAC.DBOperations;

namespace SAC.DBLink
{
    public class DBHelper
    {
        public virtual string a()
        {
            return "";
        }
        public virtual string b()
        {
            return "";
        }
        public virtual int RunCommand(string sqlCmd, out string errMsg)
        {
            errMsg = "";
            return 0;
        }
    }

    public class DB2 : DBHelper
    {
        SAC.DBOperations.DBdb2 db2 = new SAC.DBOperations.DBdb2();
        public override int RunCommand(string sqlCmd, out string errMsg)
        {
            return db2.RunCommand(sqlCmd, out errMsg);
        }
    }

    public class SQL : DBHelper
    {
        public override string a()
        {
            return "sql:a";
        }
    }

}
