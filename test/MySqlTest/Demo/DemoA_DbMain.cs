
using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlTest.Models;

namespace MySqlTest.Demo
{
    public class DemoA_DbMain
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### DbMain Start ####");

            AgileClient db = new AgileClient(new ConnectionConfig()
            {
                DbType = DatabaseType.SqlServer,
                ConnectionString = Config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                        Console.WriteLine(string.Join(",", p?.Select(it => it.ParameterName + ":" + it.Value)));
                    }
                }
            });

            var tables = db.DbMaintenance.GetTableInfoList();
            foreach (var table in tables)
            {
                Console.WriteLine(table.Description);
            }
            //more https://github.com/sunkaixuan/SqlSugar/wiki/a.DbMain
            Console.WriteLine("#### DbMain End ####");
        }
    }
}
