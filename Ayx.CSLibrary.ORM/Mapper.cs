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
        private DataColumnCollection _columns;

        public Mapper(DataColumnCollection columns = null)
        {
            ItemType = typeof(T);
            _columns = columns;
        }

        public T From(DataRow dr)
        {
            if (_columns == null)
                _columns = dr.Table.Columns;
            var result = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                SetPropertyValue(result, property, dr);
            }
            return result;
        }

        public void SetPropertyValue(object item, PropertyInfo property, DataRow dr)
        {
            try
            {
                if (!DbAttributes.IsDbField(property)) return;
                var dbFieldName = DbAttributes.GetDbFieldName(property);
                if (!CheckFieldExist(dbFieldName)) return;
                var value = Convert.ChangeType(dr[dbFieldName], property.PropertyType);
                property.SetValue(item, value, null);
            }
            catch { throw; }
        }

        public bool CheckFieldExist(string propertyName)
        {
            return _columns.Contains(propertyName);
        }
    }
}
