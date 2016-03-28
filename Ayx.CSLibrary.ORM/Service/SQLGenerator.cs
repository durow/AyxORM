using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ayx.CSLibrary.ORM.Service
{
    public class SQLGenerator
    {
        public static string GetInsertSQL(object item)
        {
            var result = "INSERT INTO {TableName} ({Fields}) VALUES({Values})";
            var type = item.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.Name.ToUpper() == "ID")
                    continue;
            }
            return "";
        }

        public static string GetSelectSQL<T>(string where)
        {
            return @"SELECT * FROM " + DbAttributes.GetDbTableName<T>() + " " + where;
        }

        public static string GetDeleteSQL<T>()
        {
            var keyProperty = DbAttributes.GetPrimaryKeyProperty<T>();
            var tableName = DbAttributes.GetDbTableName<T>();
            return @"DELETE FROM " + tableName + GetKeyWhere(keyProperty);
        }

        public static string GetUpdateSQL<T>(string set)
        {
            var keyProperty = DbAttributes.GetPrimaryKeyProperty<T>();
            return "UPDATE " + DbAttributes.GetDbTableName<T>() +
                      " SET " + set + GetKeyWhere(keyProperty);
        }

        public static string GetSetSQL(PropertyInfo property)
        {
            if (!DbAttributes.IsDbField(property)) return "";
            if (!DbAttributes.IsAutoIncrement(property)) return "";

            var fieldName = DbAttributes.GetDbFieldName(property);
            return fieldName + "=@" + property.Name;
        }

        public static string GetKeyWhere(PropertyInfo keyProperty)
        {
            if (keyProperty == null)
                throw new AyxORMException("did't find primary key,please use PrimaryKey attribute!");
            var fieldName = DbAttributes.GetDbFieldName(keyProperty);
            return " WHERE " + fieldName + "=@" + keyProperty.Name;
        }
    }
}
