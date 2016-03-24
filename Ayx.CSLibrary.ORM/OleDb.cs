using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class OleDb : AyxData
    {
        public OleDb(string connectionString):base(connectionString)
        {}

        public static string CreateExcelString(string filename)
        {
            var result = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename +
                         ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
            return result;
        }

        public static string CreateAccessString(string fileName, string password = null)
        {
            var result = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                         "Data Source=" + fileName;
            if (!string.IsNullOrEmpty(password))
                result += "Jet OLEDB:Database Password=" + password;
            return result;
        }

        public override IDbCommand CreateCommand(string sql, IDbConnection con)
        {
            throw new NotImplementedException();
        }

        public override IDbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }

        public override IDbDataAdapter CreateDataAdapter(IDbCommand cmd)
        {
            throw new NotImplementedException();
        }

        public override IDbDataParameter CreateDataParameter(string field, object value)
        {
            throw new NotImplementedException();
        }
    }
}
