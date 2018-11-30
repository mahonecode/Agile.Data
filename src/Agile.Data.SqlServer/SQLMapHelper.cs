using Agile.Data.SqlServer.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Text;
using System.Xml;

namespace Agile.Data.SqlServer
{
    public class SQLMapHelper
    {
        public static SQLMapCommandInfo GetByCode(string sqlMapFileFullPath, string code, NameValueCollection parameter)
        {
            var parameterPrefix = DapperExtensions.SqlDialect.ParameterPrefix;
            var connectionType = DapperExtensions.SqlDialect.DBName;

            SQLMapCommandInfo commandInfo = new SQLMapCommandInfo();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sqlMapFileFullPath);
            XmlElement root = xmlDoc.DocumentElement;
            XmlNode cmdNode = default(XmlNode);
            //通用SQL
            cmdNode = root.SelectSingleNode($"Sql[@Code='{code}']/Command");
            if (cmdNode == null)
            {
                //非通用SQL
                cmdNode = root.SelectSingleNode($"Sql[@Code='{code}']/{connectionType}/Command");
                if (cmdNode == null)
                    throw new Exception($"SQLMap文件{0}中未找到Code为：【{code}】的配置");
            }
            commandInfo.ConfigSQL = cmdNode.InnerText;

            //判断是否存在动态 where
            StringBuilder sbWhere = new StringBuilder();
            //通用SQL
            var whereNode = root.SelectSingleNode($"Sql[@Code='{code}']/Where");
            if (whereNode == null)
            {
                //非通用SQL
                whereNode = root.SelectSingleNode($"Sql[@Code='{code}']/{connectionType}/Where");
            }

            if (whereNode != null && parameter != null)
            {                
                sbWhere.Append(" WHERE ");
                //检查where节点下面的if
                foreach (XmlNode ifNode in whereNode.ChildNodes)
                {
                    var whereText = ifNode.InnerText;
                    if (!string.IsNullOrEmpty(parameter.Get(ifNode.Attributes["Exists"].Value)))
                    {
                        var whereCmd = ParseSqlTransact(whereText);

                        if (sbWhere.ToString().Trim().EndsWith("WHERE"))
                        {
                            sbWhere.Append(whereCmd);
                        }
                        else
                        {
                            sbWhere.AppendFormat("AND {0}", whereCmd);
                        }
                    }
                }

                if (sbWhere.ToString().Trim().EndsWith("WHERE"))
                {
                    sbWhere.Clear();
                }
            }

            commandInfo.TransferedSQL = ParseSqlTransact(commandInfo.ConfigSQL) + sbWhere.ToString();

            return commandInfo;
        }


        private static string ParseSqlTransact(string sqlcmd)
        {
            string OPEN = "#{";
            string CLOSE = "}";

            string newString = sqlcmd;
            if (newString != null)
            {
                int start = newString.IndexOf(OPEN);
                int end = newString.IndexOf(CLOSE);

                while (start > -1 && end > start)
                {
                    string prepend = newString.Substring(0, start);
                    string append = newString.Substring(end + CLOSE.Length);

                    int index = start + OPEN.Length;
                    string propName = newString.Substring(index, end - index);
                    newString = prepend + "@" + propName + append;
                    
                    start = newString.IndexOf(OPEN);
                    end = newString.IndexOf(CLOSE);
                }
            }
            return newString;
        }


        /// <summary>
        /// Replace properties by their values in the given string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static string ParsePropertyTokens(string str, NameValueCollection properties)
        {
            string OPEN = "#{";
            string CLOSE = "}";

            string newString = str;
            if (newString != null && properties != null)
            {
                int start = newString.IndexOf(OPEN);
                int end = newString.IndexOf(CLOSE);

                while (start > -1 && end > start)
                {
                    string prepend = newString.Substring(0, start);
                    string append = newString.Substring(end + CLOSE.Length);

                    int index = start + OPEN.Length;
                    string propName = newString.Substring(index, end - index);
                    string propValue = properties.Get(propName);
                    if (propValue == null)
                    {
                        newString = prepend + propName + append;
                    }
                    else
                    {
                        newString = prepend + propValue + append;
                    }
                    start = newString.IndexOf(OPEN);
                    end = newString.IndexOf(CLOSE);
                }
            }
            return newString;
        }
    }

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
