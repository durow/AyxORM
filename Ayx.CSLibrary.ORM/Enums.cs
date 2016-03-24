using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public enum DbType
    {
        SqlServer,
        MySql,
        Oracle,
        Access,
        Excel,
        Other,
    }

    public enum DbMode
    {
        Normal,
        Transaction,
    }
}
