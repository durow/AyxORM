using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Model;
using System.Data;
using System.Data.OleDb;

namespace Sample.Data
{
    public class AccessAdoOperator : IDataOperator
    {
        private string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\Temp\TestData\orm.mdb";
        public int Clear()
        {
            var sql = "DELETE FROM TestTable";
            var con = new OleDbConnection(connectionString);
            var cmd = new OleDbCommand(sql, con);
            return cmd.ExecuteNonQuery();
        }

        public IEnumerable<TestData> GetAll()
        {
            var sql = "SELECT * FROM TestTable";
            var con = new OleDbConnection(connectionString);
            var cmd = new OleDbCommand(sql, con);
            con.Open();
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                yield return TestData.FromReader(reader);
            }
            con.Close();
        }

        public int Insert(List<TestData> dataList)
        {
            var sql = @"INSERT INTO TestTable
                              (ShortTextField,IntField,DateTimeField,BoolField,LongTextField,AddField1,AddField2,AddField3,AddField4,AddField5) 
                   VALUES(@ShortTextProperty,@IntProperty,@DateTimeProperty,@BoolField,@LongTextProperty,@AddField1,@AddField2,@AddField3,@AddField4,@AddField5)";
            var con = new OleDbConnection(connectionString);
            con.Open();
            var trans = con.BeginTransaction();
            var cmd = new OleDbCommand(sql, con);
            cmd.Transaction = trans;
            try
            {
                foreach (var item in dataList)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OleDbParameter("@ShortTextProperty", item.ShortTextProperty));
                    cmd.Parameters.Add(new OleDbParameter("@IntProperty", item.IntProperty));
                    cmd.Parameters.Add(new OleDbParameter("@DateTimeProperty", item.DateTimeProperty));
                    cmd.Parameters.Add(new OleDbParameter("@BoolField", item.BoolField));
                    cmd.Parameters.Add(new OleDbParameter("@LongTextProperty", item.LongTextProperty));
                    cmd.Parameters.Add(new OleDbParameter("@AddField1", item.AddField1));
                    cmd.Parameters.Add(new OleDbParameter("@AddField2", item.AddField2));
                    cmd.Parameters.Add(new OleDbParameter("@AddField3", item.AddField3));
                    cmd.Parameters.Add(new OleDbParameter("@AddField4", item.AddField4));
                    cmd.Parameters.Add(new OleDbParameter("@AddField5", item.AddField5));
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return dataList.Count;
            }
            catch
            {
                trans.Rollback();
                con.Close();
                throw;
            }
        }
    }
}
