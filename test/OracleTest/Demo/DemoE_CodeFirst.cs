using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleTest.Models;

namespace OracleTest.Demo
{
    public class DemoE_CodeFirst
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### CodeFirst Start ####");
            AgileClient db = new AgileClient(new ConnectionConfig()
            {
                DbType = DatabaseType.SqlServer,
                ConnectionString = Config.ConnectionString3,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            db.DbMaintenance.CreateDatabase(); 
            db.CodeFirst.InitTables(typeof(CodeFirstTable1));//Create CodeFirstTable1 
            db.Insertable(new CodeFirstTable1() { Name = "a", Text="a" }).ExecuteCommand();
            var list = db.Queryable<CodeFirstTable1>().ToList();
            Console.WriteLine("#### CodeFirst end ####");
        }
    }

    public class CodeFirstTable1
    {
        [AgileColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [AgileColumn(ColumnDataType = "Nvarchar(255)")]//custom
        public string Text { get; set; }
        [AgileColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; }
    }
}
