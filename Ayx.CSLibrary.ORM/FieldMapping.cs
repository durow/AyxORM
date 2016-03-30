/*
 * Instead of Dictionary<PropertyInfo,string> for short.
*/
using System.Collections.Generic;
using System.Reflection;

namespace Ayx.CSLibrary.ORM
{
    public class FieldMapping:Dictionary<PropertyInfo,string>
    {
    }
}
