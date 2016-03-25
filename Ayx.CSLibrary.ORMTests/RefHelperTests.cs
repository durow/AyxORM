using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM.Tests
{
    [TestClass()]
    public class RefHelperTests
    {
        class TestClass
        {
            [DbField(FieldName = "test_field")]
            public string TestProperty { get; set; }
        }
        [TestMethod()]
        public void GetAttributeTest()
        {
            var test = new TestClass();
            var prop = test.GetType().GetProperty("TestProperty");
            var attr = RefHelper.GetAttribute<DbFieldAttribute>(prop);
            Assert.IsNotNull(attr);
        }
    }
}