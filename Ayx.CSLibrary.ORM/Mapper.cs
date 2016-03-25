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

        public T ToModel(DataRow dr)
        {
            var type = typeof(T);
            if (_columns == null)
                _columns = dr.Table.Columns;
            var result = Activator.CreateInstance<T>();
            foreach (var property in type.GetProperties())
            {
                var fieldName = GetFieldName(property);
                if (CheckPropertyExist(fieldName))
                {
                    SetModelValue(result, property, dr);
                }
            }
            return result;
        }

        public void SetModelValue(T item, PropertyInfo property, DataRow dr)
        {
            try
            {
                var fieldName = property.Name;

                var t = property.PropertyType;
                var v = Convert.ChangeType(dr[fieldName], t);
                property.SetValue(item, v, null);
                
            }
            catch
            { throw; }
        }

        public string GetFieldName(PropertyInfo prop)
        {
            prop.GetCustomAttributesData().Where(p => p.GetType().Name == "DbFieldAttribute");
            return "";
        }

        public bool CheckPropertyExist(string propertyName)
        {
                return _columns.Contains(propertyName);
        }
    }

    public class MappingInfo : Dictionary<string, string>
    { }
}
