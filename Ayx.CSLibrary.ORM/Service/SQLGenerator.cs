using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ayx.CSLibrary.ORM.Service
{
    public class SQLGenerator
    {
        public static string GetInsertSQL<T>(Dictionary<PropertyInfo, string> mapping)
        {
            var tableName = DbAttributes.GetDbTableName<T>();
            var result = "INSERT INTO " + tableName +
                              "({fields}) VALUES({values})";
            var fieldSB = new StringBuilder();
            var valueSB = new StringBuilder();
            foreach (var map in mapping)
            {
                fieldSB.Append(map.Value).Append(",");
                valueSB.Append("@").Append(map.Key.Name).Append(",");
            }
            return result
                .Replace("{fields}", fieldSB.ToString(0, fieldSB.Length - 1))
                .Replace("{values}", valueSB.ToString(0, valueSB.Length - 1));
        }

        public static string GetInsertAndGetIDSQL<T>(Dictionary<PropertyInfo, string> mapping)
        {
            return GetInsertSQL<T>(mapping) + ";SELECT IDENT_CURRENT("
                + DbAttributes.GetDbTableName<T>() + ")";
        }

        public static string GetSelectSQL<T>(string where)
        {
            if (!string.IsNullOrEmpty(where))
                where = " WHERE " + where;
            return @"SELECT * FROM " + DbAttributes.GetDbTableName<T>() + where;
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

        public static string GetClearSQL<T>()
        {
            var tableName = DbAttributes.GetDbTableName<T>();
            return "DELETE FROM " + tableName;
        }
    }
}
