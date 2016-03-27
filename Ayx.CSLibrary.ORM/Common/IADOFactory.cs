using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM.Common
{
    public interface IADOFactory
    {
        string ConnectionString { get; }
        string DbType { get; }
        IDbConnection CreateConnection();
        IDbCommand CreateCommand();
        IDbDataAdapter CreateDataAdapter(IDbCommand cmd);
        IDbDataParameter CreateDataParameter(string field, object value);
    }
}
