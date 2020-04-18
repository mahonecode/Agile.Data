
using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSeverTest.Models;

namespace SqlSeverTest.Demo
{
    public class DemoC_GobalFilter
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### Filter Start ####");
            var db = GetInstance();


            var sql = db.Queryable<Order>().ToSql();
            //SELECT [Id],[Name],[Price],[CreateTime] FROM [Order]  WHERE  isDelete=0 
            Console.WriteLine(sql);


            var sql2 = db.Queryable<Order,OrderItem>((main,ot)=> main.Id==ot.OrderId).ToSql();
            //SELECT [Id],[Name],[Price],[CreateTime] FROM [Order] main  ,[OrderDetail]  ot  WHERE ( [main].[Id] = [ot].[OrderId] )  AND  main.isDelete=0 
            Console.WriteLine(sql2);


            var sql3 = db.Queryable<Order>().Filter("Myfilter").ToSql();// Myfilter+Gobal 
            //SELECT [Id],[Name],[Price],[CreateTime] FROM [Order]  WHERE Name='jack'    AND  isDelete=0 
            Console.WriteLine(sql3);

            var sql4 = db.Queryable<Order>().Filter("Myfilter",isDisabledGobalFilter:true).ToSql();//only Myfilter
            //SELECT [Id],[Name],[Price],[CreateTime] FROM [Order]  WHERE Name='jack'  
            Console.WriteLine(sql4);
            Console.WriteLine("#### Filter End ####");
        }
    
 
        public static AgileClient GetInstance()
        {
            AgileClient db = new AgileClient(new ConnectionConfig() { DbType = DatabaseType.SqlServer, ConnectionString = Config.ConnectionString, IsAutoCloseConnection = true });

            //single table query gobal filter
            db.QueryFilter.Add(new SqlFilterItem()
            {
                FilterValue = filterDb =>
                {
                    //Writable logic
                    return new SqlFilterResult() { Sql = " isDelete=0"  };//Global string perform best
                }
            });

            //Multi-table query gobal filter
            db.QueryFilter.Add(new SqlFilterItem()
            {
                FilterValue = filterDb =>
                {
                    //Writable logic
                    return new SqlFilterResult() { Sql = " main.isDelete=0" };
                },
                IsJoinQuery=true
            });

            //Specific filters
            db.QueryFilter.Add(new SqlFilterItem()
            {
                FilterName= "Myfilter",
                FilterValue = filterDb =>
                {
                    //Writable logic
                    return new SqlFilterResult() { Sql = "Name='jack'" };
                }
            });
            return db;
        }
    }
}
