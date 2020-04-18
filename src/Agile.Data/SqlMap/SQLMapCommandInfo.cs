using Agile.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Data.SqlMap
{
    /// <summary>
    /// SQL-MAP 配置文件命令信息
    /// </summary>
    public class SQLMapCommandInfo
    {
        public SQLMapCommandInfo()
        {
            ParamNames = new List<AgileParameter>();
        }

        /// <summary>
        /// SQLMap中原始配置的SQL(即参数的形式为：#{参数})
        /// </summary>
        public string ConfigSQL
        {
            get;
            set;
        }

        /// <summary>
        /// SQLMap转换后的参数化SQL，如Oracle参数转换后的形式为":参数"，SQL Server转化后的参数为“@参数”
        /// </summary>
        public string TransferedSQL
        {
            get;
            set;
        }

        /// <summary>
        /// 参数
        /// </summary>
        public List<AgileParameter> ParamNames
        {
            get;
            set;
        }
    }
}
