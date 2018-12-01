using System;
using System.Collections.Generic;
using System.Text;

namespace Agile.Data.MySql.SqlMap
{
    /// <summary>
    /// SQL-MAP 配置文件命令信息
    /// </summary>
    public class SQLMapCommandInfo
    {
        public SQLMapCommandInfo()
        {
            ParamNames = new List<string>();
        }

        /// <summary>
        /// TwiSQLMap中原始的SQL(即参数的形式为：#参数#)
        /// </summary>
        public string ConfigSQL
        {
            get;
            set;
        }

        /// <summary>
        /// 转换后的参数化SQL，如Oracle参数转换后的形式为":参数"，SQL Server转化后的参数为“@参数”
        /// </summary>
        public string TransferedSQL
        {
            get;
            set;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public List<string> ParamNames
        {
            get;
            set;
        }

        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="dictParameters">参数字典</param>
        /// <returns></returns>
        //public List<DbParameter> GetDbParameters(Dictionary<string, object> dictParameters, DbUtility DB)
        //{
        //    if (ParaNames.Count == 0)
        //    {
        //        return null;
        //    }
        //    List<DbParameter> listParams = new List<DbParameter>();
        //    for (int i = 0; i < ParaNames.Count; i++)
        //    {
        //        listParams.Add(DB.CreateDbParameter(ParaNames[i], dictParameters[ParaNames[i]]));
        //    }
        //    return listParams;
        //}
    }
}
