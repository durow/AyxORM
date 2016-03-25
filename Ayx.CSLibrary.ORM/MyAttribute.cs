using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false,Inherited = false)]
    public sealed class DbFieldAttribute : Attribute
    {
        public string FieldName { get; set; }
        public FieldType FieldType { get; set; }
        public int MaxLength { get; set; }

        public DbFieldAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DbTableAttribute : Attribute
    {
        public string TableName { get; private set; }
        public DbTableAttribute(string TableName)
        {
            this.TableName = TableName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PrimaryKeyAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class AutoIncrementAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property,Inherited = false, AllowMultiple =false)]
    public sealed class NotDbFieldAttribute : Attribute
    {

    }
}
