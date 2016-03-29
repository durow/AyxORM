using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Model;
using Ayx.CSLibrary.ORM;

namespace Sample.Data
{
    public class OrmSqlOperator : IDataOperator
    {
        private AyxORM orm;
        public static OrmSqlOperator GetOrmSqlOperator(DbType dbType)
        {
            if (dbType == DbType.Access)
            {
                return new OrmSqlOperator(AyxORM.UseAccess2003(@"C:\Temp\TestData\ormSql.mdb"));
            }
            return null;
        }

        public OrmSqlOperator(AyxORM orm)
        {
            this.orm = orm;
        }

        public int Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TestData> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Insert(List<TestData> dataList)
        {
            throw new NotImplementedException();
        }
    }
}
