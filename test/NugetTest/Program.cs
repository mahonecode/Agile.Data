using System;

namespace NugetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var db = new AgileClient(new ConnectionConfig() {
            //     ConnectionString=Config.ConnectionString,
            //     IsAutoCloseConnection=true,
            //     DbType=DatabaseType.SqlServer
            //});
            //var list = db.Ado.GetInt("select 1");
            //Console.WriteLine("Hello World!");
        }
        public class Config
        {
            public static string ConnectionString = "server=.;uid=sa;pwd=haosql;database=SQLSUGAR4XTEST";
        }
    }
}
