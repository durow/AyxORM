using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
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
    }
}
