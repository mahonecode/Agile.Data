using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Data.SqlMap
{
    /// <summary>
    /// SqlMap配置
    /// </summary>
    public class SQLMapConfig
    {
        public string Code { get; set; }
        public string SQLMapFileFolder { get; set; } = "SqlMap";
        public string SQLMapFileName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }

        //分页查询 需要
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int AllRowsCount { get; set; }
    }
}
