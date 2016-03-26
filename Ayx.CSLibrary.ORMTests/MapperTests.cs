using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ayx.CSLibrary.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ayx.CSLibrary.ORM.Tests
{
    [TestClass()]
    public class MapperTests
    {
        static TestModel testModel;
        static DataTable testTable;
        static DataTable mapTable;

        static MapperTests()
        {
            testTable = new DataTable("TestModel");
            testTable.Columns.Add("Property1");
            testTable.Columns.Add("Property2");
            testTable.Columns.Add("Property3");
            testTable.Columns.Add("NotField");
            testTable.Rows.Add("row1", 1111, true,"not show");
            testTable.Rows.Add("row2", 2222, false, "not show");
            testTable.Rows.Add("row3", 3333, true, "not show");

            mapTable = new DataTable("TestModel");
            mapTable.Columns.Add("field1");
            mapTable.Columns.Add("field2");
            mapTable.Columns.Add("field3");
            mapTable.Rows.Add("test1", 9999, false);
            mapTable.Rows.Add("test2", 8888, true);
            mapTable.Rows.Add("test3", 7777, false);

        }

        [TestMethod()]
        public void CheckPropertyExistTest()
        {
            var mapper = new Mapper<TestModel>(testTable.Columns);
            var test1 = mapper.CheckFieldExist("Property2");
            var mapper2 = new Mapper<TestModel>(mapTable.Columns);
            var test2 = mapper2.CheckFieldExist("Property2");

            Assert.IsTrue(test1 == true &&
                                 test2 == false,
                                 test1 + "  " + test2);
        }

        [TestMethod()]
        public void DataRowToModelTest()
        {
            try
            {
                var mapper = new Mapper<TestModel>();
                var test = mapper.From(testTable.Rows[0]);
                Assert.IsTrue(test.Property1 == "row1" &&
                                     test.Property2 == 1111 &&
                                     test.Property3 == true &&
                                     test.NotField == null,
                                     test.Property1 + "  " +
                                     test.Property2 + "  " +
                                     test.Property3 + "  " +
                                     test.NotField);
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod()]
        public void DataRowToModelMapTest()
        {
            try
            {
                var test = new Mapper<MapModel>().From(mapTable.Rows[0]);
                Assert.IsTrue(test.Property1 == "test1" &&
                                     test.Property2 == 9999 &&
                                     test.Property3 == false &&
                                     test.NotField == null,
                                     test.Property1 + "  " +
                                     test.Property2 + "  " +
                                     test.Property3 + "  " +
                                     test.NotField);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

    }

    class TestModel
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
        public bool Property3 { get; set; }
        [NotDbField]
        public string NotField { get; set; }
    }

    class MapModel
    {
        [DbField(FieldName = "field1")]
        public string Property1 { get; set; }

        [DbField(FieldName = "field2")]
        public int Property2 { get; set; }

        [DbField(FieldName = "field3")]
        public bool Property3 { get; set; }

        [NotDbField]
        public string NotField { get; set; }
    }
}