/*
 * Description:The base class of data operation
*/

using Ayx.CSLibrary.ORM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class AyxORM
    {
        #region Properties

        public IADOFactory ADOFactory { get; set; }
        public string DbType { get { return ADOFactory.DbType; } }
        public string ConnectionString { get{ return ADOFactory.ConnectionString; } }

        #endregion

        #region Constructure

        protected AyxORM(IADOFactory adoFactory)
        {
            ADOFactory = adoFactory;
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
            var da = ADOFactory.CreateDataAdapter();
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
            var con = GetConnection();
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

        #region PrivateMethods

        //Add parameters to command
        private void AddDataParameters(IDbCommand cmd, object param)
        {
            if (param == null) return;
            var t = param.GetType();
            foreach (var property in t.GetProperties())
            {
                cmd.Parameters.Add(ADOFactory.CreateDataParameter("@"+property.Name, property.GetValue(param,null)));
            }
        }

        private IDbConnection GetConnection(IDbTransaction transaction)
        {
            IDbConnection con;
            if (transaction != null)
                con = transaction.Connection;
            else
                return GetConnection();

            return con;
        }

        private IDbConnection GetConnection()
        {
            var result = ADOFactory.CreateConnection();
            result.ConnectionString = ConnectionString;
            return result;
        }

        private IDbCommand GetCommand(string sql,object param,IDbTransaction transaction)
        {
            var con = GetConnection(transaction);
            var cmd = ADOFactory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.Transaction = transaction;
            AddDataParameters(cmd, param);
            return cmd;
        }

        #endregion
    }
}
