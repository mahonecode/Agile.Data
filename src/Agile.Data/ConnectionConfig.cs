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

        /// <summary>
        /// 是否开启脚本日志
        /// </summary>
        public bool IsEnableLogEvent { get; set; }

        /// <summary>
        /// 脚本日志回调输出
        /// </summary>
        public Action<string, object> OnLogExecuted { get; set; }
    }

    public class SqlLoger
    {
        private ConnectionConfig _currentConnectionConfig;

        public SqlLoger(ConnectionConfig config)
        {
            _currentConnectionConfig = config;
        }

        public void DebugSql(string sql, object param)
        {
            if (_currentConnectionConfig != null && _currentConnectionConfig.IsEnableLogEvent)
            {
                _currentConnectionConfig.OnLogExecuted?.Invoke(sql, param);
            }
        }
    }
}
