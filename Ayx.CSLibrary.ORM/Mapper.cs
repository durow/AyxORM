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
        public MappingInfo MappingInfo { get; private set; }
        private DataColumnCollection _columns;

        public Mapper(DataColumnCollection columns = null, MappingInfo mappingInfo = null)
        {
            ItemType = typeof(T);
            MappingInfo = mappingInfo;
            _columns = columns;
        }

        public T DataRowToModel(DataRow dr)
        {
            var type = typeof(T);
            if (_columns == null)
                _columns = dr.Table.Columns;
            var result = Activator.CreateInstance<T>();
            foreach (var property in type.GetProperties())
            {
                if (!CheckPropertyExist(property.Name))
                    continue;
                SetModelValue(result, property, dr);
            }
            return result;
        }

        public void SetModelValue(T item, PropertyInfo property, DataRow dr)
        {
            try
            {
                var fieldName = property.Name;
                if (MappingInfo != null)
                {
                    fieldName = MappingInfo[property.Name];
                }
                var t = property.PropertyType;
                var v = Convert.ChangeType(dr[fieldName], t);
                property.SetValue(item, v, null);
                
            }
            catch
            { throw; }
        }

        public bool CheckPropertyExist(string propertyName)
        {
            if (_columns == null)
                return false;
            if (MappingInfo != null)
                return MappingInfo.ContainsKey(propertyName)
                    && _columns.Contains(MappingInfo[propertyName]);
            else
                return _columns.Contains(propertyName);
        }
    }

    public class MappingInfo : Dictionary<string, string>
    { }
}
