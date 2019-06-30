using Agile.Data;
using System.Collections.Generic;
using System.IO;

namespace XUnitTestData
{
    public class BaseFixture
    {
        public AgileClient agileClient;

        public BaseFixture()
        {
            //string connectionString = "Data Source=192.168.200.154;Initial Catalog=xjtc;User Id=sa;Password=Sendinfo.2017;Persist Security Info=True;";
            string connectionString = "Data Source=127.0.0.1;Initial Catalog=dapper;User Id=sa;Password=123456;Persist Security Info=True;";

            agileClient = new AgileClient(new ConnectionConfig { ConnectionString = connectionString, DbType = DatabaseType.SqlServer });

            //开启日志
            agileClient.IsEnableLogEvent = true;
            agileClient.OnLogExecuted = (sql, param) =>
            {
                var strSql = sql;
                var strParam = Newtonsoft.Json.JsonConvert.SerializeObject(param);
                System.Console.WriteLine($"SQL语句:{strSql},SQL参数：{strParam}");
            };

            var files = new List<string>
                                {
                                    ReadScriptFile("CreateAnimalTable"),
                                    ReadScriptFile("CreateFooTable"),
                                    ReadScriptFile("CreateMultikeyTable"),
                                    ReadScriptFile("CreatePersonTable"),
                                    ReadScriptFile("CreateCarTable")
                                };

            foreach (var setupFile in files)
            {
                agileClient.DBSession.ExecuteSql(setupFile);
            }
        }


        /// <summary>
        /// 读取初始化脚本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ReadScriptFile(string name)
        {
            string fileName = GetType().Namespace + ".Sql." + name + ".sql";
            using (Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }

    }
}