using Agile.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Agile.Data.SqlMap
{
    public class SQLMapHelper
    {
        public static SQLMapCommandInfo GetByCode(SQLMapConfig config, string databaseType,string parameterPrefix)
        {
            string sqlMapFileFullName = Path.Combine(config.SQLMapFileFolder, config.SQLMapFileName);
            string code = config.Code;
            Dictionary<string, object> parameter = config.Parameters;

            if (!File.Exists(sqlMapFileFullName))
                throw new Exception($"SQLMap文件{sqlMapFileFullName}未找到");

            SQLMapCommandInfo commandInfo = new SQLMapCommandInfo();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sqlMapFileFullName);
            XmlElement root = xmlDoc.DocumentElement;
            XmlNode cmdNode = default(XmlNode);
            //通用SQL
            cmdNode = root.SelectSingleNode($"Sql[@Code='{code}']/Command");
            if (cmdNode == null)
            {
                //非通用SQL
                cmdNode = root.SelectSingleNode($"Sql[@Code='{code}']/{databaseType}/Command");
            }
            if (cmdNode == null)
                throw new Exception($"SQLMap文件{sqlMapFileFullName}中未找到Code为：【{code}】的配置");

            commandInfo.ConfigSQL = cmdNode.InnerText;

            //判断是否存在动态 where
            StringBuilder sbWhere = new StringBuilder();
            //通用SQL
            var whereNode = root.SelectSingleNode($"Sql[@Code='{code}']/Where");
            if (whereNode == null)
            {
                //非通用SQL
                whereNode = root.SelectSingleNode($"Sql[@Code='{code}']/{databaseType}/Where");
            }
            //动态and条件集合
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
            commandInfo.TransferedSQL = ParseSqlReplace(ParseSqlTransact(commandInfo.ConfigSQL, parameterPrefix), parameter);
            //字典参数 转 agileparameter
            if (parameter != null && parameter.Count > 0)
            {
                foreach (var item in parameter)
                {
                    commandInfo.ParamNames.Add(new AgileParameter(parameterPrefix + item.Key, item.Value));
                }
            }
            
            return commandInfo;
        }


        /// <summary>
        /// 参数化查询
        /// 脚本解析
        /// select * from Animal where Name=#{Name} and Age=#{Age}
        /// 解析成
        /// select * from Animal where Name=:Name and Age=:Age
        /// </summary>
        /// <param name="sqlcmd"></param>
        /// <param name="parameterPrefix"></param>
        /// <returns></returns>
        private static string ParseSqlTransact(string sqlcmd, string parameterPrefix)
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
        /// 非参数化查询，针对in的情况，做参数值文本替换拼接sql
        /// 脚本解析
        /// select * from Animal where Name in(${Name})
        /// 解析成
        /// select * from Animal where Name in('name1Value','name2Value','name3Value')
        /// </summary>
        /// <param name="sqlcmd"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static string ParseSqlReplace(string sqlcmd, Dictionary<string, object> parameter)
        {
            string OPEN = "${";
            string CLOSE = "}";

            string newString = sqlcmd;
            if (newString != null && parameter != null && parameter.Count > 0)
            {
                int start = newString.IndexOf(OPEN);
                int end = newString.IndexOf(CLOSE);

                while (start > -1 && end > start)
                {
                    string prepend = newString.Substring(0, start);
                    string append = newString.Substring(end + CLOSE.Length);

                    int index = start + OPEN.Length;
                    string propName = newString.Substring(index, end - index);
                    if (parameter.ContainsKey(propName))
                    {
                        newString = prepend + parameter[propName] + append;
                    }
                    else
                    {
                        newString = prepend + propName + append;
                    }
                    start = newString.IndexOf(OPEN);
                    end = newString.IndexOf(CLOSE);
                }
            }
            return newString;
        }
    }
}
