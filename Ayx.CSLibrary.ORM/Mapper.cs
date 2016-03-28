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
        public Type ItemType { get; private set; }
        public Dictionary<PropertyInfo, string> FieldMapping { get; private set; }

        public Mapper(DataColumnCollection columns = null)
        {
            ItemType = typeof(T);
            if (columns != null)
                CheckFieldMapping(columns);
        }

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
            if (table == null) return new List<T>();
            CheckFieldMapping(table.Columns);
            var result = CreateInstanceList(table.Rows.Count).ToList();
            foreach (var property in FieldMapping)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    SetPropertyValue(result, property.Key, table.Rows[i][property.Value]);
                }
            }
            return result;
        }

        public void CheckFieldMapping(DataColumnCollection columns)
        {
            if (FieldMapping != null) return;
            FieldMapping = GetSelectMapping<T>(columns);
        }

        public void SetPropertyValue(object item, PropertyInfo property, object value)
        {
            var newValue = Convert.ChangeType(value, property.PropertyType);
            property.SetValue(item, value, null);
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

        public static Dictionary<PropertyInfo, string> GetSelectMapping<TModel>(DataColumnCollection columns)
        {
            var result = new Dictionary<PropertyInfo, string>();
            var type = typeof(TModel);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                if (!columns.Contains(fieldName)) continue;
                result.Add(property, fieldName);
            }
            return result;
        }

        public static Dictionary<PropertyInfo, string> GetInsertMapping<TModel>()
        {
            var result = new Dictionary<PropertyInfo, string>();
            var type = typeof(TModel);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                if (!DbAttributes.IsAutoIncrement(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                result.Add(property, fieldName);
            }
            return result;
        }

        public static Dictionary<PropertyInfo, string> GetUpdateMapping<TModel>()
        {
            var result = new Dictionary<PropertyInfo, string>();
            var type = typeof(TModel);
            foreach (var property in type.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                var fieldName = DbAttributes.GetDbFieldName(property);
                result.Add(property, fieldName);
            }
            return result;
        }
    }
}
