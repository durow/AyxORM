using Sample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data
{
    public interface IDataOperator
    {
        IEnumerable<TestData> GetAll();
        int Clear();
        int Insert(List<TestData> dataList);
    }
}
