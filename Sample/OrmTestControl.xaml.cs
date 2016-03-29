using Ayx.CSLibrary.ORM;
using Sample.Data;
using Sample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sample
{
    /// <summary>
    /// OrmTestControl.xaml 的交互逻辑
    /// </summary>
    public partial class OrmTestControl : UserControl
    {
        #region DependencyProperties

        #region DbType


        public DbType DataType
        {
            get { return (DbType)GetValue(DataTypeProperty); }
            set { SetValue(DataTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataTypeProperty =
            DependencyProperty.Register("DataType", typeof(DbType), typeof(OrmTestControl), new PropertyMetadata(DbType.Access));


        #endregion

        #region TestType


        public TestType TestType
        {
            get { return (TestType)GetValue(TestTypeProperty); }
            set { SetValue(TestTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TestType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TestTypeProperty =
            DependencyProperty.Register("TestType", typeof(TestType), typeof(OrmTestControl), new PropertyMetadata(TestType.ORM));


        #endregion

        public IDataOperator DataOperator { get; private set; }

        #endregion

        public OrmTestControl()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            if(TestType == TestType.ORM)
            {
                DataOperator = OrmOperator.GetOrmOperator(DataType);
            }
            if (TestType == TestType.ORMSql)
            {
                DataOperator = OrmSqlOperator.GetOrmSqlOperator(DataType);
            }
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            var dataList = TestData.GetTestData(500).ToList();
            var start = DateTime.Now;
            var count = DataOperator.Insert(dataList);
            var ts = DateTime.Now - start;
            ResultGrid.ItemsSource = null;
            ShowTime(ts, "清除" + count + "条数据");
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var start = DateTime.Now;
            var result = DataOperator.GetAll().ToList();
            var ts = DateTime.Now - start;
            ResultGrid.ItemsSource = result;
            ShowTime(ts, "SELECT返回" + result.Count + "条数据");
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var start = DateTime.Now;
            var count = DataOperator.Clear();
            var ts = DateTime.Now - start;
            ResultGrid.ItemsSource = null;
            ShowTime(ts, "清除" + count + "条数据");
        }

        private void ShowTime(TimeSpan ts, string operation)
        {
            try
            {
                TipText.Text = operation + " 共耗时:" + ts.TotalMilliseconds + "毫秒";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    public enum DbType
    {
        Access,
        SQLite,
        Excel,
    }

    public enum TestType
    {
        ORM,
        ORMSql,
        ADO,
    }
}
