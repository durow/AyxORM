/*
 * Description:SqlServer data operation
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class SqlServerDb : AyxData
    {
        #region Constructure
        public SqlServerDb(string connectionString) : base(connectionString)
        { }

        #endregion

        #region Factory
        public static string CreateSqlServerString(string ip, string username, string userpass, string dataName)
        {
            var result = "Data Source=" + ip +
                         ";Initial Catalog=" + dataName +
                         ";User ID=" + username +
                         ";Password=" + userpass;
            return result;
        }

        #endregion

        #region Implented abstract methods

        //创建SqlServer链接
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        //创建SqlServer命令
        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        //创建SqlServer DataAdapter
        public override IDbDataAdapter CreateDataAdapter(IDbCommand cmd)
        {
            return new SqlDataAdapter(cmd as SqlCommand);
        }

        //创建SqlServer查询参数
        public override IDbDataParameter CreateDataParameter(string field, object value)
        {
            return new SqlParameter(field, value);
        }

        #endregion
    }
}
