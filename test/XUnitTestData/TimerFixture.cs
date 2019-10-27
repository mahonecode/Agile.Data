using Agile.Data;
using Agile.Data.SqlMap;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using XUnitTestData.Model;

namespace XUnitTestData
{
    public class TimerFixture 
    {
        private static int cnt = 10000;


        public class InsertTimes : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public InsertTimes(BaseFixture data)
            {
                database = data;
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
                database.agileClient.DBSession.Insert(p);
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
                    database.agileClient.DBSession.Insert(p2);
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
                database.agileClient.DBSession.Insert(p);
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
                    var id = database.agileClient.DBSession.Insert(p2);
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
                database.agileClient.DBSession.Insert(a);
                DateTime start = DateTime.Now;
                List<Guid> ids = new List<Guid>();
                for (int i = 0; i < cnt; i++)
                {
                    Animal a2 = new Animal { Name = "Name" + i };
                    database.agileClient.DBSession.Insert(a2);
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
                database.agileClient.DBSession.Insert(a);
                DateTime start = DateTime.Now;
                List<Guid> ids = new List<Guid>();
                for (int i = 0; i < cnt; i++)
                {
                    Animal a2 = new Animal { Name = "Name" + i };
                    var id = database.agileClient.DBSession.Insert(a2);
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
                database.agileClient.DBSession.Insert(ca);
                DateTime start = DateTime.Now;
                List<string> ids = new List<string>();
                for (int i = 0; i < cnt; i++)
                {
                    var key = (i + 1).ToString().PadLeft(15, '0');
                    Car ca2 = new Car { Id = key, Name = "Name" + i };
                    database.agileClient.DBSession.Insert(ca2);
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
                database.agileClient.DBSession.Insert(ca);
                DateTime start = DateTime.Now;
                List<string> ids = new List<string>();
                for (int i = 0; i < cnt; i++)
                {
                    var key = (i + 1).ToString().PadLeft(15, '0');
                    Car ca2 = new Car { Id = key, Name = "Name" + i };
                    var id = database.agileClient.DBSession.Insert(ca2);
                    ids.Add(id);
                }

                double total = DateTime.Now.Subtract(start).TotalMilliseconds;
                Console.WriteLine("Total Time:" + total);
                Console.WriteLine("Average Time:" + total / cnt);
            }
        }
    }
}