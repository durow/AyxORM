using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class SQL
    {
        public string Verb { get; private set; }
        public string Fields { get; private set; }
        public string Where { get; private set; }
        public string TableName { get; private set; }
        public Dictionary<string,object> Param { get; private set; }
        private SQL(string tableName)
        {
            TableName = tableName;
        }

        public static SQL SELECT<T>(string tableName = "")
        {
            return new SQL(GetTableName<T>(tableName))
            {
                Verb = "SELECT",
            };
        }

        public static SQL UPDATE<T>(string tableName = "")
        {
            return new SQL(GetTableName<T>(tableName));
        }

        public static SQL INSERT<T>(string tableName = "")
        {
            return new SQL(GetTableName<T>(tableName));
        }

        public static SQL DELETE<T>(string tableName = "")
        {
            return new SQL(GetTableName<T>(tableName));
        }

        private static string GetTableName<T>(string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
                return tableName;
            return DbAttributes.GetDbTableName<T>();
        }

        
    }
}
