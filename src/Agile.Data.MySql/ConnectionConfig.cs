using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Data.MySql
{
    public class ConnectionConfig
    {
        /// <summary>
        ///DbType.SqlServer Or Other
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        ///Database Connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// true does not need to close the connection
        /// </summary>
        public bool IsAutoCloseConnection { get; set; } = true;
    }
}
