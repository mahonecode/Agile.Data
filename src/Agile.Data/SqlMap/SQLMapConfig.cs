using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Agile.Data.SqlMap
{
    /// <summary>
    /// SqlMap配置
    /// </summary>
    public class SQLMapConfig
    {
        /// <summary>
        /// 默认路径 wwwroot/SqlMap
        /// </summary>
        public SQLMapConfig()
        {
            SQLMapFileFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap");
        }

        /// <summary>
        /// 指定文件存放相对路径
        /// </summary>
        /// <param name="relativePath"></param>
        public SQLMapConfig(string relativePath)
        {
            SQLMapFileFolder = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        }

        public string Code { get; set; }
        public string SQLMapFileFolder { get; set; }
        public string SQLMapFileName { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
