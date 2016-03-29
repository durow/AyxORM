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
            DependencyProperty.Register("DataType", typeof(DbType), typeof(OrmTestControl), new PropertyMetadata(null));


        #endregion

        #region TestType


        public TestType TestType
        {
            get { return (TestType)GetValue(TestTypeProperty); }
            set { SetValue(TestTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TestType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TestTypeProperty =
            DependencyProperty.Register("TestType", typeof(TestType), typeof(OrmTestControl), new PropertyMetadata(null));


        #endregion

        public IDataOperator DataOperator { get; private set; }

        #endregion

        public OrmTestControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataOperator == null)
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
            if(TestType == TestType.ADO)
            {
                if (DataType == DbType.Access)
                    DataOperator = new AccessAdoOperator();
            }
            DataTypeText.Text = "数据库类型:" + DataType.ToString() + "  连接访问方式:" + TestType.ToString();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            var insert = 0;
            try { insert = int.Parse(InsertCountText.Text); }
            catch { return; }
            var dataList = TestData.GetTestData(insert).ToList();
            var start = DateTime.Now;
            var count = DataOperator.Insert(dataList);
            var ts = DateTime.Now - start;
            ResultGrid.ItemsSource = null;
            ShowTime(ts, "插入" + count + "条数据");
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
        ORMSql,
        ADO,
        ORM,
    }
}
