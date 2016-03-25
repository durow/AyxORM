using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    //public enum DbType
    //{
    //    SqlServer,
    //    MySql,
    //    Oracle,
    //    Access,
    //    Excel,
    //    Other,
    //}

    public enum DbMode
    {
        Normal,
        Transaction,
    }

    public enum FieldType
    {
        undefined,
        db_char,
        db_varchar,
        db_nchar,
        db_nvarchar,
        db_text,
        db_bit,
        db_bool,
        db_binary,
        db_int,
        db_datetime,
        db_float,
        db_double,
        db_decimal,
    }
}
