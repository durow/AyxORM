/*
 * Description:The base class of data operation
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public abstract class AyxData
    {
        #region Properties

        public string ConnectionString { get; private set; }
        public DbType DbType { get; set; }

        #endregion

        #region Constructure

        protected AyxData(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Base data methods

        public int ExecuteNonQuery(string sql, object param = null, IDbTransaction transaction = null)
        {
            var cmd = GetCommand(sql, param, transaction);

            try
            {
                if (transaction == null)
                {
                    cmd.Connection.Open();
                    var count = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return count;
                }
                else
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                cmd.Connection.Close();
                throw;
            }
        }

        public DataSet ExecuteDataSet(string sql, object param = null, IDbTransaction transaction = null)
        {
            var cmd = GetCommand(sql, param, transaction);
            var da = CreateDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        
        public DataTable ExecuteDataTable(string sql, object param = null, IDbTransaction transaction = null)
        {
            return ExecuteDataSet(sql, param, transaction).Tables[0];
        }

        #endregion

        #region TransactionMethods

        public IDbTransaction GetTransaction()
        {
            var con = CreateConnection();
            con.Open();
            return con.BeginTransaction();
        }

        #endregion

        #region Insert methods

        public int Insert<T>(T item, IDbTransaction transaction = null)
        {
            var sql = SQLGenerator.GetInsertSQL(item);
            return ExecuteNonQuery(sql, item, transaction);
        }

        public int Insert<T>(IList<T> itemList, IDbTransaction transaction = null)
        {
            return 0;
        }

        #endregion

        #region AbstractMethods

        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand();
        public abstract IDbDataAdapter CreateDataAdapter(IDbCommand cmd);
        public abstract IDbDataParameter CreateDataParameter(string field, object value);

        #endregion

        #region PrivateMethods

        //Add parameters to command
        private void AddDataParameters(IDbCommand cmd, object param)
        {
            if (param == null) return;
            var t = param.GetType();
            foreach (var property in t.GetProperties())
            {
                cmd.Parameters.Add(CreateDataParameter("@"+property.Name, property.GetValue(param,null)));
            }
        }

        private IDbConnection GetConnection(IDbTransaction transaction)
        {
            IDbConnection con;
            if (transaction != null)
                con = transaction.Connection;
            else
                con = CreateConnection();
            return con;
        }

        private IDbCommand GetCommand(string sql,object param,IDbTransaction transaction)
        {
            var con = GetConnection(transaction);
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.Transaction = transaction;
            AddDataParameters(cmd, param);
            return cmd;
        }

        #endregion
    }
}
