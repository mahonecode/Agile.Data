
using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqliteTest.Models;

namespace SqliteTest.Demo
{
    public class Demo9_EntityMain
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### EntityMain Start ####");

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
            var entityInfo = db.EntityProvider.GetEntityInfo<Order>();
            foreach (var column in entityInfo.Columns)
            {
                Console.WriteLine(column.DbColumnName);
            }

            var dbColumnsName = db.EntityProvider.GetDbColumnName<EntityMapper>("Name");

            var dbTableName = db.EntityProvider.GetTableName<EntityMapper>();

            //more https://github.com/sunkaixuan/SqlSugar/wiki/9.EntityMain
            Console.WriteLine("#### EntityMain End ####");
        }
    }
}
