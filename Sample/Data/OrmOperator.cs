using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Model;
using Ayx.CSLibrary.ORM;

namespace Sample.Data
{
    public class OrmOperator : IDataOperator
    {
        private AyxORM orm;

        public static OrmOperator GetOrmOperator(DbType dbType)
        {
            if(dbType == DbType.Access)
            {
                return new OrmOperator(AyxORM.UseAccess2003(@"D:\Temp\TestData\orm.mdb"));
            }
            return null;
        }

        public OrmOperator(AyxORM orm)
        {
            this.orm = orm;
        }

        public int Clear()
        {
            return orm.ClearTable<TestData>();
        }

        public IEnumerable<TestData> GetAll()
        {
            return orm.Select<TestData>();
        }

        public int Insert(List<TestData> dataList)
        {
            return orm.InsertList(dataList);
        }
    }
}
