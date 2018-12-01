using Agile.Data.Oracle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;

namespace Agile.Data.Oracle.SqlMap
{
    public class SQLMapHelper
    {
        public static SQLMapCommandInfo GetByCode(string sqlMapFileFullPath, string code, Dictionary<string, object> parameter)
        {
            var parameterPrefix = DapperExtensions.SqlDialect.ParameterPrefix;
            var connectionType = DapperExtensions.SqlDialect.DBName;

            if (!File.Exists(sqlMapFileFullPath))
                throw new Exception($"SQLMap文件{sqlMapFileFullPath}未找到");

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
            }
            if (cmdNode == null)
                throw new Exception($"SQLMap文件{sqlMapFileFullPath}中未找到Code为：【{code}】的配置");

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

            if (whereNode != null && parameter != null && parameter.Count > 0)
            {
                sbWhere.Append(" WHERE ");
                //检查where节点下面的if
                foreach (XmlNode ifNode in whereNode.ChildNodes)
                {
                    var whereSql = ifNode.InnerText;
                    if (parameter.ContainsKey(ifNode.Attributes["Exists"].Value))
                    {
                        if (sbWhere.ToString().Trim().EndsWith("WHERE"))
                        {
                            sbWhere.Append(whereSql);
                        }
                        else
                        {
                            sbWhere.AppendFormat("AND {0}", whereSql);
                        }
                    }
                }

                if (sbWhere.ToString().Trim().EndsWith("WHERE"))
                {
                    sbWhere.Clear();
                }
            }
            commandInfo.ConfigSQL = commandInfo.ConfigSQL + sbWhere.ToString();
            commandInfo.TransferedSQL = ParseSqlTransact(commandInfo.ConfigSQL, parameterPrefix);

            return commandInfo;
        }


        /// <summary>
        /// 脚本解析
        /// select * from Animal where Name=#{Name} and Age=#{Age}
        /// 解析成
        /// select * from Animal where Name=:Name and Age=:Age
        /// </summary>
        /// <param name="sqlcmd"></param>
        /// <param name="parameterPrefix"></param>
        /// <returns></returns>
        private static string ParseSqlTransact(string sqlcmd, char parameterPrefix)
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
                    newString = prepend + parameterPrefix + propName + append;

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
        private static string ParseSqlReplace(string str, NameValueCollection properties)
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
}
