using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Data.Oracle
{
    public class ConnectionConfig
    {
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
