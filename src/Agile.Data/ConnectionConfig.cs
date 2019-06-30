using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Data
{
    public class ConnectionConfig
    {
        /// <summary>
        ///DatabaseType.SqlServer Or Other
        /// </summary>
        public DatabaseType DbType { get; set; }

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
