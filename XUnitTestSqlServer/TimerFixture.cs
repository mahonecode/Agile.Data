using Agile.Data.SqlServer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using XUnitTestSqlServer.Model;

namespace XUnitTestSqlServer
{
    public class TimerFixture 
    {
        private static int cnt = 10000;


        public class InsertTimes : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture Base;

            public InsertTimes(SqlServerBaseFixture sqlserverbase)
            {
                Base = sqlserverbase;
            }

            [Fact]
            public void IdentityKey_UsingEntity()
            {
                Person p = new Person
                               {
                                   FirstName = "FirstName",
                                   LastName = "LastName",
                                   DateCreated = DateTime.Now,
                                   Active = true
                               };
                Base.agileClient.DBSession.Insert(p);
                DateTime start = DateTime.Now;
                List<int> ids = new List<int>();
                for (int i = 0; i < cnt; i++)
                {
                    Person p2 = new Person
                                    {
                                        FirstName = "FirstName" + i,
                                        LastName = "LastName" + i,
                                        DateCreated = DateTime.Now,
                                        Active = true
                                    };
                    Base.agileClient.DBSession.Insert(p2);
                    ids.Add(p2.Id);
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }

            [Fact]
            public void IdentityKey_UsingReturnValue()
            {
                Person p = new Person
                               {
                                   FirstName = "FirstName",
                                   LastName = "LastName",
                                   DateCreated = DateTime.Now,
                                   Active = true
                               };
                Base.agileClient.DBSession.Insert(p);
                DateTime start = DateTime.Now;
                List<int> ids = new List<int>();
                for (int i = 0; i < cnt; i++)
                {
                    Person p2 = new Person
                                    {
                                        FirstName = "FirstName" + i,
                                        LastName = "LastName" + i,
                                        DateCreated = DateTime.Now,
                                        Active = true
                                    };
                    var id = Base.agileClient.DBSession.Insert(p2);
                    ids.Add(id);
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }

            [Fact]
            public void GuidKey_UsingEntity()
            {
                Animal a = new Animal { Name = "Name" };
                Base.agileClient.DBSession.Insert(a);
                DateTime start = DateTime.Now;
                List<Guid> ids = new List<Guid>();
                for (int i = 0; i < cnt; i++)
                {
                    Animal a2 = new Animal { Name = "Name" + i };
                    Base.agileClient.DBSession.Insert(a2);
                    ids.Add(a2.Id);
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }

            [Fact]
            public void GuidKey_UsingReturnValue()
            {
                Animal a = new Animal { Name = "Name" };
                Base.agileClient.DBSession.Insert(a);
                DateTime start = DateTime.Now;
                List<Guid> ids = new List<Guid>();
                for (int i = 0; i < cnt; i++)
                {
                    Animal a2 = new Animal { Name = "Name" + i };
                    var id = Base.agileClient.DBSession.Insert(a2);
                    ids.Add(id);
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }

            [Fact]
            public void AssignKey_UsingEntity()
            {
                Car ca = new Car { Id = string.Empty.PadLeft(15, '0'), Name = "Name" };
                Base.agileClient.DBSession.Insert(ca);
                DateTime start = DateTime.Now;
                List<string> ids = new List<string>();
                for (int i = 0; i < cnt; i++)
                {
                    var key = (i + 1).ToString().PadLeft(15, '0');
                    Car ca2 = new Car { Id = key, Name = "Name" + i };
                    Base.agileClient.DBSession.Insert(ca2);
                    ids.Add(ca2.Id);
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }

            [Fact]
            public void AssignKey_UsingReturnValue()
            {
                Car ca = new Car { Id = string.Empty.PadLeft(15, '0'), Name = "Name" };
                Base.agileClient.DBSession.Insert(ca);
                DateTime start = DateTime.Now;
                List<string> ids = new List<string>();
                for (int i = 0; i < cnt; i++)
                {
                    var key = (i + 1).ToString().PadLeft(15, '0');
                    Car ca2 = new Car { Id = key, Name = "Name" + i };
                    var id = Base.agileClient.DBSession.Insert(ca2);
                    ids.Add(id);
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }








            [Fact]
            public void Ext_RunInTransaction_UsingAction()
            {
                Action<IDbSession> processDelegate = (IDbSession session) =>
                {
                    Multikey m = new Multikey { Key2 = "key", Value = "foo" };
                    var key = session.Insert(m);

                    Animal a1 = new Animal { Name = "Foo" };
                    session.Insert(a1);

                    //session.ExecuteCommad("select * from dual");
                    Assert.Equal(1, key.Key1);
                    //Assert.Equal("key", key.Key2);

                    //Task.Factory.StartNew(() => {
                    //    var dt = session.ExecuteDataTable("select * from Multikey");
                    //    //var dt2 = session.ExecuteDataTable("select * from Animal");
                    //});
                };
                Base.agileClient.RunInTransaction(processDelegate);
            }


            [Fact]
            public void Ext_RunInTransaction_UsingFunc()
            {
                Func<IDbSession,int> processDelegate = (IDbSession session) =>
                {
                    Multikey m = new Multikey { Key2 = "key", Value = "foo" };
                    var key = session.Insert(m);

                    Animal a1 = new Animal { Name = "Foo" };
                    session.Insert(a1);

                    //session.ExecuteCommad("select * from dual");
                    Assert.Equal(1, key.Key1);
                    //Assert.Equal("key", key.Key2);

                    //Task.Factory.StartNew(() => {
                    //    var dt = session.ExecuteDataTable("select * from Multikey");
                    //    //var dt2 = session.ExecuteDataTable("select * from Animal");
                    //});

                    return key.Key1;
                };
                int ret= Base.agileClient.RunInTransaction<int>(processDelegate);
                Assert.Equal(1, ret);
            }



            [Fact]
            public void Ext_Multithread_UsingEntity()
            {
                Animal a = new Animal { Name = "Name" };
                Base.agileClient.DBSession.Insert(a);
                DateTime start = DateTime.Now;
                List<Guid> ids = new List<Guid>();
                for (int i = 0; i < cnt; i++)
                {
                    Animal a2 = new Animal { Name = "Name" + i };
                    Task.Factory.StartNew(() =>
                    {   
                        //多线程测试并发
                        Base.agileClient.DBSession.Insert(a2);
                        ids.Add(a2.Id);
                    });
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }



            [Fact]
            public void Ext_SqlCommand_SqlMap()
            {
                Animal a1 = new Animal { Name = "Foo" };
                Animal a2 = new Animal { Name = "Bar" };
                Animal a3 = new Animal { Name = "Baz" };

                List<Animal> lst = new List<Animal>() { a1, a2, a3 };

                Base.agileClient.DBSession.InsertBatch<Animal>(lst);
                var animals = Base.agileClient.DBSession.GetList<Animal>().ToList();
                Assert.Equal(3, animals.Count);

                var fileName = "AgileSqlMap.xml";
                var filePath = $"{Path.Combine(Environment.CurrentDirectory, "SqlMap")}{Path.DirectorySeparatorChar}{fileName}";

                NameValueCollection collection = new NameValueCollection();
                collection.Add("Name", "Foo");
                var cmd = SQLMapHelper.GetByCode(filePath, "test4", collection);
                string sql = cmd.TransferedSQL;

                dynamic param = new { Name = "Foo" };
                var a = Base.agileClient.DBSession.ExecuteQuerySingle<Animal>(sql, param);
                Assert.Equal(a1.Id, a.Id);
            }
        }
    }
}