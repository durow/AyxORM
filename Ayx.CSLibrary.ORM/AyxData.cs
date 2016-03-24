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
        public DbMode DbMode { get; set; } = DbMode.Normal;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

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

        #region AbstractMethods

        //创建链接
        public abstract IDbConnection CreateConnection();
        //创建命令
        public abstract IDbCommand CreateCommand();
        //创建数据适配器
        public abstract IDbDataAdapter CreateDataAdapter(IDbCommand cmd);
        //创建查询参数
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
