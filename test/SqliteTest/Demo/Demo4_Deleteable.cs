
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
    public class Demo4_Deleteable
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### Deleteable Start ####");

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
            //by entity
            db.Deleteable<Order>().Where(new Order() { Id = 1111 }).ExecuteCommand();

            //by primary key
            db.Deleteable<Order>().In(1111).ExecuteCommand();

            //by primary key array
            db.Deleteable<Order>().In(new int[] { 1111, 2222 }).ExecuteCommand();

            //by expression
            db.Deleteable<Order>().Where(it => it.Id == 11111).ExecuteCommand();

            Console.WriteLine("#### Deleteable End ####");

        }
    }
}
