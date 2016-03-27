using Ayx.CSLibrary.ORM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class OleDbFactory : IADOFactory
    {
        private string _connectionString;
        public string ConnectionString { get { return _connectionString; } }

        private string _dbType;
        public string DbType { get { return _dbType; } }


        public OleDbFactory(string connectionString,string dbType)
        {
            _connectionString = connectionString;
            _dbType = dbType;
        }

        public static OleDbFactory CreateExcelString(string filename)
        {
            var conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename +
                         ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
            return new OleDbFactory(conStr, "Excel");
        }

        public static OleDbFactory CreateAccessString(string fileName, string password = null)
        {
            var conStr = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                         "Data Source=" + fileName;
            if (!string.IsNullOrEmpty(password))
                conStr += "Jet OLEDB:Database Password=" + password;
            return new OleDbFactory(conStr, "Access");
        }

        public IDbConnection CreateConnection()
        {
            return new OleDbConnection();
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public IDbDataAdapter CreateDataAdapter(IDbCommand cmd)
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateDataParameter(string field, object value)
        {
            throw new NotImplementedException();
        }
    }
}
