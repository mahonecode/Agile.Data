
using Agile.Data;
using Agile.Data.Entities;
using Agile.Data.Enums;
using Agile.Data.ExpressionsToSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlTest.Models;
using Agile.Data.Infrastructure;
using System.IO;
using Agile.Data.SqlMap;

namespace MySqlTest.Demo
{
    public  class Demo7_Ado
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### Ado Start ####");

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
            //sql
            var dt = db.Ado.GetDataTable("select * from [order] where  @id>0 or name=@name", new List<AgileParameter>(){
              new AgileParameter("@id",1),
              new AgileParameter("@name","2")
            });

            //sql  
            var dt2 = db.Ado.GetDataTable("select * from [order] where @id>0  or name=@name", new { id = 1, name = "2" });

            //Stored Procedure
            //var dt3 = db.Ado.UseStoredProcedure().GetDataTable("sp_school", new { name = "张三", age = 0 }); 
            //var nameP = new AgileParameter("@name", "张三");
            //var ageP = new AgileParameter("@age", null, true);//isOutput=true
            //var dt4 = db.Ado.UseStoredProcedure().GetDataTable("sp_school", nameP, ageP);

            

            //There are many methods to under db.ado
            var list= db.Ado.SqlQuery<Order>("select * from [order] ");
            var intValue=db.Ado.SqlQuerySingle<int>("select 1");
            db.Ado.ExecuteCommand("delete [order] where id>1000");
            //db.Ado.xxx
            Console.WriteLine("#### Ado End ####");



            Console.WriteLine("#### Ado sqlmap Start ####");
            //sqlmap   sql  param
            Dictionary<string, object> dicParams = new Dictionary<string, object>();
            dicParams.Add("OrderId", 1);
            dicParams.Add("Price", 0);

            var cmd = db.Ado.GetSqlMap(new SQLMapConfig("SqlMap") { SQLMapFileName = "AgileSqlMap.xml", Code = "test5", Parameters = dicParams });
            var dt5 = db.Ado.GetDataTable(cmd.TransferedSQL, cmd.ParamNames);
            Console.WriteLine(dt5.Rows.Count);
            Console.WriteLine("#### Ado sqlmap End ####");
        }
    }
}
