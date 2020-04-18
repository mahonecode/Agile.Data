
using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PgSqlTest.Models;

namespace PgSqlTest.Demo
{
    public class Demo6_Queue
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### Queue Start ####");

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
            db.Insertable<Order>(new Order() { Name = "a" }).AddQueue();
            db.Insertable<Order>(new Order() { Name = "b" }).AddQueue();
            db.SaveQueues();


            db.Insertable<Order>(new Order() { Name = "a" }).AddQueue();
            db.Insertable<Order>(new Order() { Name = "b" }).AddQueue();
            db.Insertable<Order>(new Order() { Name = "c" }).AddQueue();
            db.Insertable<Order>(new Order() { Name = "d" }).AddQueue();
            var ar = db.SaveQueuesAsync();
            ar.Wait();

            db.Queryable<Order>().AddQueue();
            db.Queryable<Order>().AddQueue();
            db.AddQueue("select * from [Order] where id=@id", new { id = 10000 });
            var result2 = db.SaveQueues<Order, Order, Order>();

            Console.WriteLine("#### Queue End ####");
        }
    }
}
