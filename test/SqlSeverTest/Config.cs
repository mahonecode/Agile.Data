using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSeverTest
{
    /// <summary>
    /// Setting up the database name does not require you to create the database
    /// 设置好数据库名不需要你去手动建库
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        public static string ConnectionString = "server=127.0.0.1;uid=sa;pwd=123456;database=AgileDataTEST";

        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        public static string ConnectionString2 = "server=127.0.0.1;uid=sa;pwd=123456;database=AgileDataTEST2";

        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        public static string ConnectionString3 = "server=127.0.0.1;uid=sa;pwd=123456;database=AgileDataTEST3";
    }
}
