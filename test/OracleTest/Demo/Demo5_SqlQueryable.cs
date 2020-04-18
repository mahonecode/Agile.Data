

using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleTest.Models;

namespace OracleTest.Demo
{
    public class Demo5_SqlQueryable
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### SqlQueryable Start ####");
            AgileClient db = new AgileClient(new ConnectionConfig()
            {
                DbType = DatabaseType.SqlServer,
                ConnectionString = Config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });

            int total = 0;
            var list = db.SqlQueryable<Order>("select * from [order]").ToPageList(1, 2, ref total);


            //by expression
            var list2 = db.SqlQueryable<Order>("select * from [order]").Where(it => it.Id == 1).ToPageList(1, 2);
            //by sql
            var list3 = db.SqlQueryable<Order>("select * from [order]").Where("id=@id", new { id = 1 }).ToPageList(1, 2);

            Console.WriteLine("#### SqlQueryable End ####");
        }
    }
}
