
using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using OracleTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleTest.UnitTest
{
    public partial class NewUnitTest
    {
       public static  AgileClient Db=> new AgileClient(new ConnectionConfig()
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

        public static void RestData()
        {
            Db.DbMaintenance.TruncateTable<Order>();
            Db.DbMaintenance.TruncateTable<OrderItem>();
        }
        public static void Init()
        {
            CodeFirst();
            Updateable();
            Json();
            Ado();
            Queryable();
            QueryableAsync();
            Thread();
            Thread2();
            Thread3();
        }
    }
}
