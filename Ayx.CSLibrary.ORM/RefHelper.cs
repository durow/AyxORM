using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class RefHelper
    {
        public static CustomAttributeData GetAttribute<T>(PropertyInfo property)
        {
            return property.GetCustomAttributesData()
                                   .Where(a => a.Constructor.DeclaringType == typeof(T))
                                   .FirstOrDefault();
        }
    }
}
