using Ayx.CSLibrary.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime;

namespace Test.TestCase
{
    class AttributeTest
    {
        public static void Test()
        {
            var test = new TestData();
            var t = test.GetType();
            foreach (var attr in t.GetCustomAttributesData())
            {
                
                Console.WriteLine(attr.Constructor.DeclaringType.Name);
                foreach (var arg in attr.ConstructorArguments)
                {
                    Console.WriteLine(arg.Value);
                }
            }
            Console.WriteLine();

            foreach (var p in t.GetProperties())
            {
                
                foreach (var attr in p.GetCustomAttributesData())
                {
                    Console.WriteLine("attribute type:" + attr.Constructor.DeclaringType.Name);
                    foreach (var v in attr.NamedArguments)
                    {
                        Console.WriteLine(v.MemberInfo.Name + "  " + v.TypedValue.Value);
                    }
                }
                Console.WriteLine();
            }
        }
        }
    }

[DbTable("TestTable")]
class TestData
{
    [DbField(FieldName = "string_field", FieldType = "nvarchar", MaxLength = 50)]
    public string StringProperty { get; set; }

    [DbField(FieldName = "int_field", FieldType = "int")]
    public int IntProperty { get; set; }

    [PrimaryKey]
    [AutoIncrement]
    [DbField(FieldName = "id")]
    public int ID { get; set; }

    [NotDbField]
    public bool IsRead { get; set; }

    public TestData()
    {
        StringProperty = "TestString";
        IntProperty = 1234;
        ID = 1;
    }
}
