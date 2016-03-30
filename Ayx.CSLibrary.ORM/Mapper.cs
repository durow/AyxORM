using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class Mapper<T>
    {
        public Dictionary<PropertyInfo, string> FieldMapping { get; private set; }

        public T From(DataRow row)
        {
            CheckFieldMapping(row.Table.Columns);
            var result = Activator.CreateInstance<T>();
            foreach (var property in FieldMapping)
            {
                SetPropertyValue(result, property.Key, row[property.Value]);
            }
            return result;
        }

        public IEnumerable<T> From(DataTable table)
        {
            if (table == null) yield break;
            CheckFieldMapping(table.Columns);
            foreach (DataRow dr in table.Rows)
            {
                var temp = Activator.CreateInstance<T>();
                foreach (var map in FieldMapping)
                {
                    SetPropertyValue(temp, map.Key, dr[map.Value]);
                }
                yield return temp;
            }
        }

        public IEnumerable<T> From(IDataReader reader)
        {
            CheckFieldMapping(null);

            while(reader.Read())
            {
                var result = Activator.CreateInstance<T>();
                foreach (var property in FieldMapping)
                {
                    SetPropertyValue(result, property.Key, reader[property.Value]);
                }
                yield return result;
            }
        }

        public void CheckFieldMapping(DataColumnCollection columns)
        {
            if (FieldMapping != null) return;
            FieldMapping = GetSelectMapping(columns);
        }

        public static void SetPropertyValue(object item, PropertyInfo property, object value)
        {
            var newValue = Convert.ChangeType(value, property.PropertyType);
            property.SetValue(item, newValue, null);
        }

        public string GetFieldName(PropertyInfo property, DataColumnCollection columns)
        {
            if (!DbAttributes.IsDbField(property)) return "";
            var dbFieldName = DbAttributes.GetDbFieldName(property);
            if (!columns.Contains(dbFieldName)) return "";
            return dbFieldName;
        }

        public IEnumerable<T> CreateInstanceList(int num)
        {
            if (num <= 0) yield break;
            yield return Activator.CreateInstance<T>();
        }

        public static Dictionary<PropertyInfo, string> GetSelectMapping(DataColumnCollection columns)
        {
            var result = new Dictionary<PropertyInfo, string>();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                if(columns != null)
                    if (!columns.Contains(fieldName)) continue;
                result.Add(property, fieldName);
            }
            return result;
        }

        public static Dictionary<PropertyInfo, string> GetInsertMapping()
        {
            var result = new Dictionary<PropertyInfo, string>();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                if (DbAttributes.IsAutoIncrement(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                result.Add(property, fieldName);
            }
            return result;
        }

        public static Dictionary<PropertyInfo, string> GetUpdateMapping()
        {
            var result = new Dictionary<PropertyInfo, string>();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                if (DbAttributes.IsAutoIncrement(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                result.Add(property, fieldName);
            }
            return result;
        }
    }
}
