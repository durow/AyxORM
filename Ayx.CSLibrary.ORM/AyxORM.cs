/*
 * Description:The base class of data operation
*/

using Ayx.CSLibrary.ORM.Common;
using Ayx.CSLibrary.ORM.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

        //public int ExecuteNonQuery(string sql, DbParams param = null, IDbTransaction transaction = null)
        //{
        //    var cmd = GetCommand(sql, param, transaction);
        //    return ExecuteNonQuery(cmd);
        //}

        public int ExecuteNonQuery(IDbCommand cmd)
        {
            try
            {
                if (cmd.Transaction == null)
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
            return ExecuteDataSet(cmd);
        }

        //public DataSet ExecuteDataSet(string sql, DbParams param = null, IDbTransaction transaction = null)
        //{
        //    var cmd = GetCommand(sql, param, transaction);
        //    return ExecuteDataSet(cmd);
        //}

        public DataSet ExecuteDataSet(IDbCommand cmd)
        {
            var da = GetAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataTable ExecuteDataTable(string sql, object param = null, IDbTransaction transaction = null)
        {
            return ExecuteDataSet(sql, param, transaction).Tables[0];
        }

        //public DataTable ExecuteDataTable(string sql, DbParams param = null, IDbTransaction transaction = null)
        //{
        //    return ExecuteDataSet(sql, param, transaction).Tables[0];
        //}

        public DataTable ExecuteDataTable(IDbCommand cmd)
        {
            return ExecuteDataSet(cmd).Tables[0];
        }

        #endregion

        #region ORM data methods

        public void CreateTableIfNotExist(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new AyxORMException("no SQL to create table");
            ExecuteNonQuery(sql);
        }

        public IEnumerable<T> ExecuteList<T>(string sql, object param = null , IDbTransaction transaction = null)
        {
            var dt = ExecuteDataTable(sql, param, transaction);
            return new Mapper<T>().From(dt);
        }

        public IEnumerable<T> Select<T>(string where = "", object param= null, IDbTransaction transaction = null)
        {
            var sql = SQLGenerator.GetSelectSQL<T>(where);
            var dt = ExecuteDataTable(sql, param, transaction);
            return new Mapper<T>().From(dt);
        }

        public int Delete<T>(T item, IDbTransaction transaction)
        {
            var sql = SQLGenerator.GetDeleteSQL<T>();
            var cmd = GetCommand(sql, null, transaction);
            var keyProperty = DbAttributes.GetPrimaryKeyProperty<T>();
            AddDataParameter(cmd, "@" + keyProperty.Name, keyProperty.GetValue(item, null));
            return ExecuteNonQuery(cmd);
        }

        public int Update<T>(T item, IDbTransaction transaction = null)
        {
            var cmd = GetCommand("", null, transaction);
            var set = "";
            var type = typeof(T);

            foreach (var property in type.GetProperties())
            {
                set += SQLGenerator.GetSetSQL(property) + ",";
                AddDataParameter(cmd, item, property);
            }
            cmd.CommandText = SQLGenerator.GetUpdateSQL<T>(set.Substring(0, set.Length - 1));
            return ExecuteNonQuery(cmd);
        }

        public int Insert<T>(T item, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public int InsertAndGetID<T>(T item, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(IList<T> items, IDbTransaction transaction = null)
        {
            throw new NotImplementedException();
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

        #region PrivateMethods

        //Add parameters to command
        private void AddDataParameters(IDbCommand cmd, object param)
        {
            if (param == null) return;
            var t = param.GetType();
            foreach (var property in t.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                AddDataParameter(cmd, param, property);
            }
        }

        //private void AddDataParameters(IDbCommand cmd,DbParams paramDict)
        //{
        //    if (paramDict == null) return;
        //    foreach (var param in paramDict)
        //    {
        //        AddDataParameter(cmd, param.Key, param.Value);
        //    }
        //}

        private void AddDataParameter(IDbCommand cmd, string key, object value)
        {
            cmd.Parameters.Add(ADOFactory.CreateDataParameter(key, value));
        }

        private void AddDataParameter<T>(IDbCommand cmd, T item, PropertyInfo property)
        {
            AddDataParameter(cmd, "@" + property.Name, property.GetValue(item, null));
        }
        private IDbConnection GetConnection(IDbTransaction transaction)
        {
            if (transaction != null)
                return transaction.Connection;
            else
                return GetConnection();
        }

        private IDbConnection GetConnection()
        {
            var result = ADOFactory.CreateConnection();
            result.ConnectionString = ConnectionString;
            return result;
        }

        private IDbCommand GetCommand(string sql,object param, IDbTransaction transaction)
        {
            var con = GetConnection(transaction);
            var cmd = ADOFactory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.Transaction = transaction;
            AddDataParameters(cmd, param);
            return cmd;
        }

        //private IDbCommand GetCommand(string sql, DbParams paramList, IDbTransaction transaction)
        //{
        //    var con = GetConnection(transaction);
        //    var cmd = ADOFactory.CreateCommand();
        //    cmd.CommandText = sql;
        //    cmd.Connection = con;
        //    cmd.Transaction = transaction;
        //    AddDataParameters(cmd, paramList);
        //    return cmd;
        //}

        private IDataAdapter GetAdapter(IDbCommand cmd)
        {
            var result = ADOFactory.CreateDataAdapter();
            result.SelectCommand = cmd;
            return result;
        }
        #endregion
    }
}
