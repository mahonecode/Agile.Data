using Agile.Data;
using Agile.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using XUnitTestData.Model;

namespace XUnitTestData
{
    /// <summary>
    /// ª˘¥° ‘ˆ…æ∏ƒ≤È ≤‚ ‘
    /// </summary>
    public class CrudFixtureTests
    {
        public class InsertMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public InsertMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void AddsEntityToDatabase_ReturnsKey()
            {
                Person p = new Person { Active = true, FirstName = "Foo", LastName = "Bar", DateCreated = DateTime.UtcNow };
                int id = database.agileClient.DBSession.Insert(p);
                Assert.Equal(1, id);
                Assert.Equal(1, p.Id);
            }

            [Fact]
            public void AddsEntityToDatabase_ReturnsCompositeKey()
            {
                Multikey m = new Multikey { Key2 = "key", Value = "foo" };
                var key = database.agileClient.DBSession.Insert(m);
                Assert.Equal(1, key.Key1);
                Assert.Equal("key", key.Key2);
            }

            [Fact]
            public void AddsEntityToDatabase_ReturnsGeneratedPrimaryKey()
            {
                Animal a1 = new Animal { Name = "Foo" };
                database.agileClient.DBSession.Insert(a1);

                var a2 = database.agileClient.DBSession.Get<Animal>(a1.Id);
                Assert.NotEqual(Guid.Empty, a2.Id);
                Assert.Equal(a1.Id, a2.Id);
            }

            [Fact]
            public void AddsMultipleEntitiesToDatabase()
            {
                Animal a1 = new Animal { Name = "Foo" };
                Animal a2 = new Animal { Name = "Bar" };
                Animal a3 = new Animal { Name = "Baz" };

                List<Animal> lst = new List<Animal>() { a1, a2, a3 };
                
                database.agileClient.DBSession.InsertBatch<Animal>(lst);

                var animals = database.agileClient.DBSession.GetList<Animal>().ToList();
                Assert.Equal(3, animals.Count);
            }
        }


        public class GetMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public GetMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void UsingKey_ReturnsEntity()
            {
                Person p1 = new Person
                {
                    Active = true,
                    FirstName = "Foo",
                    LastName = "Bar",
                    DateCreated = DateTime.UtcNow
                };
                int id = database.agileClient.DBSession.Insert(p1);

                Person p2 = database.agileClient.DBSession.Get<Person>(id);
                Assert.Equal(id, p2.Id);
                Assert.Equal("Foo", p2.FirstName);
                Assert.Equal("Bar", p2.LastName);
            }

            [Fact]
            public void UsingCompositeKey_ReturnsEntity()
            {
                Multikey m1 = new Multikey { Key2 = "key", Value = "bar" };
                var key = database.agileClient.DBSession.Insert(m1);

                Multikey m2 = database.agileClient.DBSession.Get<Multikey>(new { key.Key1, key.Key2 });
                Assert.Equal(1, m2.Key1);
                Assert.Equal("key", m2.Key2);
                Assert.Equal("bar", m2.Value);
            }
        }


        public class DeleteMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public DeleteMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void UsingKey_DeletesFromDatabase()
            {
                Person p1 = new Person
                {
                    Active = true,
                    FirstName = "Foo",
                    LastName = "Bar",
                    DateCreated = DateTime.UtcNow
                };
                int id = database.agileClient.DBSession.Insert(p1);

                Person p2 = database.agileClient.DBSession.Get<Person>(id);
                database.agileClient.DBSession.Delete(p2);
                Assert.Null(database.agileClient.DBSession.Get<Person>(id));
            }

            [Fact]
            public void UsingCompositeKey_DeletesFromDatabase()
            {
                Multikey m1 = new Multikey { Key2 = "key", Value = "bar" };
                var key = database.agileClient.DBSession.Insert(m1);

                Multikey m2 = database.agileClient.DBSession.Get<Multikey>(new { key.Key1, key.Key2 });
                database.agileClient.DBSession.Delete(m2);
                Assert.Null(database.agileClient.DBSession.Get<Multikey>(new { key.Key1, key.Key2 }));
            }

            [Fact]
            public void UsingPredicate_DeletesRows()
            {
                Person p1 = new Person { Active = true, FirstName = "Foo", LastName = "Bar", DateCreated = DateTime.UtcNow };
                Person p2 = new Person { Active = true, FirstName = "Foo", LastName = "Bar", DateCreated = DateTime.UtcNow };
                Person p3 = new Person { Active = true, FirstName = "Foo", LastName = "Barz", DateCreated = DateTime.UtcNow };
                database.agileClient.DBSession.Insert(p1);
                database.agileClient.DBSession.Insert(p2);
                database.agileClient.DBSession.Insert(p3);

                var list = database.agileClient.DBSession.GetList<Person>();
                Assert.Equal(3, list.Count());

                IPredicate pred = Predicates.Field<Person>(p => p.LastName, Operator.Eq, "Bar");
                var result = database.agileClient.DBSession.Delete<Person>(pred);
                Assert.True(result);

                list = database.agileClient.DBSession.GetList<Person>();
                Assert.Single(list);
            }

            [Fact]
            public void UsingObject_DeletesRows()
            {
                Person p1 = new Person { Active = true, FirstName = "Foo", LastName = "Bar", DateCreated = DateTime.UtcNow };
                Person p2 = new Person { Active = true, FirstName = "Foo", LastName = "Bar", DateCreated = DateTime.UtcNow };
                Person p3 = new Person { Active = true, FirstName = "Foo", LastName = "Barz", DateCreated = DateTime.UtcNow };
                database.agileClient.DBSession.Insert(p1);
                database.agileClient.DBSession.Insert(p2);
                database.agileClient.DBSession.Insert(p3);

                var list = database.agileClient.DBSession.GetList<Person>();
                Assert.Equal(3, list.Count());

                var result = database.agileClient.DBSession.Delete<Person>(new { LastName = "Bar" });
                Assert.True(result);

                list = database.agileClient.DBSession.GetList<Person>();
                Assert.Single(list);
            }
        }


        public class UpdateMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public UpdateMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void UsingKey_UpdatesEntity()
            {
                Person p1 = new Person
                {
                    Active = true,
                    FirstName = "Foo",
                    LastName = "Bar",
                    DateCreated = DateTime.UtcNow
                };
                int id = database.agileClient.DBSession.Insert(p1);

                var p2 = database.agileClient.DBSession.Get<Person>(id);
                p2.FirstName = "Baz";
                p2.Active = false;

                database.agileClient.DBSession.Update(p2,new { LastName = "Bar" });

                var p3 = database.agileClient.DBSession.Get<Person>(id);
                Assert.Equal("Baz", p3.FirstName);
                Assert.Equal("Bar", p3.LastName);
                Assert.False(p3.Active);
            }

            [Fact]
            public void UsingCompositeKey_UpdatesEntity()
            {
                Multikey m1 = new Multikey { Key2 = "key", Value = "bar" };
                var key = database.agileClient.DBSession.Insert(m1);

                Multikey m2 = database.agileClient.DBSession.Get<Multikey>(new { Key1 = key.Key1, Key2 = key.Key2 });
                m2.Key2 = "key";
                m2.Value = "barz";
                database.agileClient.DBSession.Update(m2);

                Multikey m3 = database.agileClient.DBSession.Get<Multikey>(new { Key1 = 1, Key2 = "key" });
                Assert.Equal(1, m3.Key1);
                Assert.Equal("key", m3.Key2);
                Assert.Equal("barz", m3.Value);
            }
        }


        public class GetListMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public GetListMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void UsingNullPredicate_ReturnsAll()
            {
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "a", LastName = "a1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "b", LastName = "b1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "c", LastName = "c1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "d", LastName = "d1", DateCreated = DateTime.UtcNow });

                IEnumerable<Person> list = database.agileClient.DBSession.GetList<Person>();
                Assert.Equal(4, list.Count());
            }

            [Fact]
            public void UsingPredicate_ReturnsMatching()
            {
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "a", LastName = "a1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "b", LastName = "b1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "c", LastName = "c1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "d", LastName = "d1", DateCreated = DateTime.UtcNow });

                var predicate = Predicates.Field<Person>(f => f.Active, Operator.Eq, true);
                IEnumerable<Person> list = database.agileClient.DBSession.GetList<Person>(predicate, null);
                Assert.Equal(2, list.Count());
                Assert.True(list.All(p => p.FirstName == "a" || p.FirstName == "c"));
            }

            [Fact]
            public void UsingObject_ReturnsMatching()
            {
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "a", LastName = "a1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "b", LastName = "b1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "c", LastName = "c1", DateCreated = DateTime.UtcNow });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "d", LastName = "d1", DateCreated = DateTime.UtcNow });

                var predicate = new { Active = true, FirstName = "c" };
                IEnumerable<Person> list = database.agileClient.DBSession.GetList<Person>(predicate, null);
                Assert.Single(list);
                Assert.True(list.All(p => p.FirstName == "c"));
            }
        }


        public class GetPageMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public GetPageMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void UsingNullPredicate_ReturnsMatching()
            {
                var id1 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Sigma", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id2 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Delta", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id3 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Theta", LastName = "Gamma", DateCreated = DateTime.UtcNow });
                var id4 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Iota", LastName = "Beta", DateCreated = DateTime.UtcNow });

                IList<ISort> sort = new List<ISort>
                                    {
                                        Predicates.Sort<Person>(p => p.LastName),
                                        Predicates.Sort<Person>(p => p.FirstName)
                                    };

                int allRowsCount = 0;
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>(0, 2, out allRowsCount, null, sort);
                Assert.Equal(2, list.Count());
                Assert.Equal(id2, list.First().Id);
                Assert.Equal(id1, list.Skip(1).First().Id);
            }

            [Fact]
            public void UsingPredicate_ReturnsMatching()
            {
                var id1 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Sigma", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id2 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Delta", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id3 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Theta", LastName = "Gamma", DateCreated = DateTime.UtcNow });
                var id4 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Iota", LastName = "Beta", DateCreated = DateTime.UtcNow });

                var predicate = Predicates.Field<Person>(f => f.Active, Operator.Eq, true);
                IList<ISort> sort = new List<ISort>
                                    {
                                        Predicates.Sort<Person>(p => p.LastName),
                                        Predicates.Sort<Person>(p => p.FirstName)
                                    };

                int allRowsCount = 0;
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>( 0, 2, out allRowsCount, predicate, sort);
                Assert.Equal(2, list.Count());
                Assert.True(list.All(p => p.FirstName == "Sigma" || p.FirstName == "Theta"));
            }

            [Fact]
            public void NotFirstPage_Returns_NextResults()
            {
                var id1 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Sigma", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id2 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Delta", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id3 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Theta", LastName = "Gamma", DateCreated = DateTime.UtcNow });
                var id4 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Iota", LastName = "Beta", DateCreated = DateTime.UtcNow });

                IList<ISort> sort = new List<ISort>
                                    {
                                        Predicates.Sort<Person>(p => p.LastName),
                                        Predicates.Sort<Person>(p => p.FirstName)
                                    };

                int allRowsCount = 0;
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>(1, 2, out allRowsCount, null, sort);
                Assert.Equal(2, list.Count());
                Assert.Equal(id4, list.First().Id);
                Assert.Equal(id3, list.Skip(1).First().Id);
            }

            [Fact]
            public void UsingObject_ReturnsMatching()
            {
                var id1 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Sigma", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id2 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Delta", LastName = "Alpha", DateCreated = DateTime.UtcNow });
                var id3 = database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "Theta", LastName = "Gamma", DateCreated = DateTime.UtcNow });
                var id4 = database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "Iota", LastName = "Beta", DateCreated = DateTime.UtcNow });

                var predicate = new { Active = true };
                IList<ISort> sort = new List<ISort>
                                    {
                                        Predicates.Sort<Person>(p => p.LastName),
                                        Predicates.Sort<Person>(p => p.FirstName)
                                    };

                int allRowsCount = 0;
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>(0, 2, out allRowsCount, predicate, sort);
                Assert.Equal(2, list.Count());
                Assert.True(list.All(p => p.FirstName == "Sigma" || p.FirstName == "Theta"));
            }
        }


        public class CountMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public CountMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void UsingNullPredicate_Returns_Count()
            {
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "a", LastName = "a1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "b", LastName = "b1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "c", LastName = "c1", DateCreated = DateTime.UtcNow.AddDays(-3) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "d", LastName = "d1", DateCreated = DateTime.UtcNow.AddDays(-1) });

                int count = database.agileClient.DBSession.Count<Person>();
                Assert.Equal(4, count);
            }

            [Fact]
            public void UsingPredicate_Returns_Count()
            {
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "a", LastName = "a1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "b", LastName = "b1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "c", LastName = "c1", DateCreated = DateTime.UtcNow.AddDays(-3) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "d", LastName = "d1", DateCreated = DateTime.UtcNow.AddDays(-1) });

                var predicate = Predicates.Field<Person>(f => f.DateCreated, Operator.Lt, DateTime.UtcNow.AddDays(-5));
                int count = database.agileClient.DBSession.Count<Person>(predicate);
                Assert.Equal(2, count);
            }

            [Fact]
            public void UsingObject_Returns_Count()
            {
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "a", LastName = "a1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "b", LastName = "b1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "c", LastName = "c1", DateCreated = DateTime.UtcNow.AddDays(-3) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "d", LastName = "d1", DateCreated = DateTime.UtcNow.AddDays(-1) });

                var predicate = new { FirstName = new[] { "b", "d" } };
                int count = database.agileClient.DBSession.Count<Person>(predicate);
                Assert.Equal(2, count);
            }
        }


        public class GetMultipleMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public GetMultipleMethod(BaseFixture data)
            {
                database = data;
            }

            [Fact]
            public void ReturnsItems()
            {
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "a", LastName = "a1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "b", LastName = "b1", DateCreated = DateTime.UtcNow.AddDays(-10) });
                database.agileClient.DBSession.Insert(new Person { Active = true, FirstName = "c", LastName = "c1", DateCreated = DateTime.UtcNow.AddDays(-3) });
                database.agileClient.DBSession.Insert(new Person { Active = false, FirstName = "d", LastName = "d1", DateCreated = DateTime.UtcNow.AddDays(-1) });

                database.agileClient.DBSession.Insert(new Animal { Name = "Foo" });
                database.agileClient.DBSession.Insert(new Animal { Name = "Bar" });
                database.agileClient.DBSession.Insert(new Animal { Name = "Baz" });

                MultiplePredicate predicate = new MultiplePredicate();
                predicate.Add<Person>(null);
                predicate.Add<Animal>(Predicates.Field<Animal>(a => a.Name, Operator.Like, "Ba%"));
                predicate.Add<Person>(Predicates.Field<Person>(a => a.LastName, Operator.Eq, "c1"));

                var result = database.agileClient.DBSession.GetMultiple(predicate);
                var people = result.Read<Person>().ToList();
                var animals = result.Read<Animal>().ToList();
                var people2 = result.Read<Person>().ToList();

                Assert.Equal(4, people.Count);
                Assert.Equal(2, animals.Count);
                Assert.Single(people2);
            }
        }



        public class SQLCommandMethod : IClassFixture<BaseFixture>
        {
            BaseFixture database;

            public SQLCommandMethod(BaseFixture data)
            {
                database = data;
            }


            [Fact]
            public void Test_ExecuteSql()
            {
                database.agileClient.DBSession.ExecuteSql("delete from car");
                //////////////////////////////≤Â»Î/////////////////////////////
                //∆¥–¥sql
                var sql = "insert car(id,name) values ('1','≤‚ ‘√˚≥∆1')";
                int affectedRows = database.agileClient.DBSession.ExecuteSql(sql);

                //≤Œ ˝ªØsql ƒ‰√˚¿‡  µÃÂ¿‡ ◊÷µ‰
                var paramSql = "insert car(id,name) values (@id,@name)";

                //“ªÃı
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new { id = "2", name = "≤‚ ‘2" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Car { Id = "3", Name = "≤‚ ‘3" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Dictionary<string, object> { { "Id", "4" }, { "Name", "≤‚ ‘4" } });

                //∂‡Ãı
                var paramList1 = new[]
                {
                    new { id = "5", name = "≤‚ ‘5" },
                    new { id = "6", name = "≤‚ ‘6" },
                    new { id = "7", name = "≤‚ ‘7" }
                };
                var paramList2 = new List<Car>
                {
                    new Car { Id = "8", Name = "≤‚ ‘8" },
                    new Car { Id = "9", Name = "≤‚ ‘9" },
                    new Car { Id = "10", Name = "≤‚ ‘10" }
                };
                var paramList3 = new[]
                {
                    new Dictionary<string, object> { { "Id", "11" }, { "Name", "≤‚ ‘11" } },
                    new Dictionary<string, object> { { "Id", "12" }, { "Name", "≤‚ ‘12" } },
                    new Dictionary<string, object> { { "Id", "13" }, { "Name", "≤‚ ‘13" } }
                };
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList1);
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList2);
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList3);


                ///////////////////////////////∏¸–¬////////////////////////////////
                //∆¥–¥sql
                sql = "update car set name='–ﬁ∏ƒ√˚≥∆10-–ﬁ∏ƒ' where id='10'";
                affectedRows = database.agileClient.DBSession.ExecuteSql(sql);

                //≤Œ ˝ªØsql ƒ‰√˚¿‡  µÃÂ¿‡ ◊÷µ‰
                paramSql = "update car set name=@name where id=@id";

                //“ªÃı
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new { id = "2", name = "≤‚ ‘2-–ﬁ∏ƒ" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Car { Id = "3", Name = "≤‚ ‘3-–ﬁ∏ƒ" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Dictionary<string, object> { { "Id", "4" }, { "Name", "≤‚ ‘4-–ﬁ∏ƒ" } });

                //∂‡Ãı
                paramList1 = new[]
                {
                    new { id = "5", name = "≤‚ ‘5-–ﬁ∏ƒ" },
                    new { id = "6", name = "≤‚ ‘6-–ﬁ∏ƒ" },
                    new { id = "7", name = "≤‚ ‘7-–ﬁ∏ƒ" }
                };
                paramList2 = new List<Car>
                {
                    new Car { Id = "8", Name = "≤‚ ‘8-–ﬁ∏ƒ" },
                    new Car { Id = "9", Name = "≤‚ ‘9-–ﬁ∏ƒ" },
                    new Car { Id = "10", Name = "≤‚ ‘10-–ﬁ∏ƒ" }
                };
                paramList3 = new[]
                {
                    new Dictionary<string, object> { { "Id", "11" }, { "Name", "≤‚ ‘11-–ﬁ∏ƒ" } },
                    new Dictionary<string, object> { { "Id", "12" }, { "Name", "≤‚ ‘12-–ﬁ∏ƒ" } },
                    new Dictionary<string, object> { { "Id", "13" }, { "Name", "≤‚ ‘13-–ﬁ∏ƒ" } }
                };
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList1);
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList2);
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList3);


                ///////////////////////////////…æ≥˝////////////////////////////////
                //∆¥–¥sql
                sql = "delete from car  where id='10'";
                affectedRows = database.agileClient.DBSession.ExecuteSql(sql);

                //≤Œ ˝ªØsql ƒ‰√˚¿‡  µÃÂ¿‡ ◊÷µ‰
                paramSql = "delete from car where id=@id";

                //“ªÃı
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new { id = "2", name = "≤‚ ‘2" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Car { Id = "3", Name = "≤‚ ‘3" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Dictionary<string, object> { { "Id", "4" }, { "Name", "≤‚ ‘4" } });

                //∂‡Ãı
                paramList1 = new[]
                {
                    new { id = "5", name = "≤‚ ‘5" },
                    new { id = "6", name = "≤‚ ‘6" },
                    new { id = "7", name = "≤‚ ‘7" }
                };
                paramList2 = new List<Car>
                {
                    new Car { Id = "8", Name = "≤‚ ‘8" },
                    new Car { Id = "9", Name = "≤‚ ‘9" },
                    new Car { Id = "10", Name = "≤‚ ‘10" }
                };
                paramList3 = new[]
                {
                    new Dictionary<string, object> { { "Id", "11" }, { "Name", "≤‚ ‘11" } },
                    new Dictionary<string, object> { { "Id", "12" }, { "Name", "≤‚ ‘12" } },
                    new Dictionary<string, object> { { "Id", "13" }, { "Name", "≤‚ ‘13" } }
                };
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList1);
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList2);
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, paramList3);


                Assert.True(affectedRows > 0);
            }

            [Fact]
            public void Test_ExecuteScalar()
            {
                database.agileClient.DBSession.ExecuteSql("delete from car");
                //////////////////////////////≤Â»Î/////////////////////////////
                //∆¥–¥sql
                var sql = "insert car(id,name) values ('1','≤‚ ‘√˚≥∆1')";
                int affectedRows = database.agileClient.DBSession.ExecuteSql(sql);

                //≤Œ ˝ªØsql ƒ‰√˚¿‡  µÃÂ¿‡ ◊÷µ‰
                var paramSql = "insert car(id,name) values (@id,@name)";

                //“ªÃı
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new { id = "2", name = "≤‚ ‘2" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Car { Id = "3", Name = "≤‚ ‘3" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Dictionary<string, object> { { "Id", "4" }, { "Name", "≤‚ ‘4" } });



                var name = database.agileClient.DBSession.ExecuteScalar("select name from car where id='2'");
                var id = database.agileClient.DBSession.ExecuteScalar<int>("select id from car where id='2'");

                Assert.True(id == 2);
            }


            [Fact]
            public void Test_Query()
            {
                database.agileClient.DBSession.ExecuteSql("delete from car");
                //////////////////////////////≤Â»Î/////////////////////////////
                //∆¥–¥sql
                var sql = "insert car(id,name) values ('1','≤‚ ‘√˚≥∆1')";
                int affectedRows = database.agileClient.DBSession.ExecuteSql(sql);

                //≤Œ ˝ªØsql ƒ‰√˚¿‡  µÃÂ¿‡ ◊÷µ‰
                var paramSql = "insert car(id,name) values (@id,@name)";

                //“ªÃı
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new { id = "2", name = "≤‚ ‘2" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Car { Id = "3", Name = "≤‚ ‘3" });
                affectedRows = database.agileClient.DBSession.ExecuteSql(paramSql, new Dictionary<string, object> { { "Id", "4" }, { "Name", "≤‚ ‘4" } });

                var model1 = database.agileClient.DBSession.QueryFirst<Car>("select * from car");
                var model2 = database.agileClient.DBSession.QueryFirstOrDefault<Car>("select * from car where id='111'");
                var model3 = database.agileClient.DBSession.QuerySingle<Car>("select * from car where id='2'");
                var model4 = database.agileClient.DBSession.QuerySingleOrDefault<Car>("select * from car where id='111'");

                int allCount = 0;

                var lst = database.agileClient.DBSession.QueryList<Car>("select * from car");
                var lst2 = database.agileClient.DBSession.QueryPageList<Car>("select * from car", 0, 2, out allCount);

                var dt = database.agileClient.DBSession.QueryDataTable("select * from car");
                var dt2 = database.agileClient.DBSession.QueryPageDataTable("select * from car where id=@id", 0, 2, out allCount, new { id = "2", name = "≤‚ ‘2" });



                Assert.True(model1 != null);
            }










            [Fact]
            public void Ext_RunInTransaction_UsingAction()
            {
                Action<IDbSession> processDelegate = (IDbSession db) =>
                {
                    Multikey m = new Multikey { Key2 = "key", Value = "foo" };
                    var key = db.Insert(m);

                    Animal a1 = new Animal { Name = "Foo" };
                    db.Insert(a1);

                    //session.ExecuteCommad("select * from dual");
                    Assert.Equal(1, key.Key1);
                    //Assert.Equal("key", key.Key2);

                    //Task.Factory.StartNew(() => {
                    //    var dt = session.ExecuteDataTable("select * from Multikey");
                    //    //var dt2 = session.ExecuteDataTable("select * from Animal");
                    //});
                };
                database.agileClient.RunInTransaction(processDelegate);
            }


            [Fact]
            public void Ext_RunInTransaction_UsingFunc()
            {
                Func<IDbSession, int> processDelegate = (IDbSession db) =>
                {
                    Multikey m = new Multikey { Key2 = "key", Value = "foo" };
                    var key = db.Insert(m);

                    Animal a1 = new Animal { Name = "Foo" };
                    db.Insert(a1);

                    //session.ExecuteCommad("select * from dual");
                    Assert.Equal(1, key.Key1);
                    //Assert.Equal("key", key.Key2);

                    //Task.Factory.StartNew(() => {
                    //    var dt = session.ExecuteDataTable("select * from Multikey");
                    //    //var dt2 = session.ExecuteDataTable("select * from Animal");
                    //});

                    return key.Key1;
                };
                int ret = database.agileClient.RunInTransaction<int>(processDelegate);
                Assert.Equal(1, ret);
            }



            //[Fact]
            //public void Ext_Multithread_UsingEntity()
            //{
            //    Animal a = new Animal { Name = "Name" };
            //    database.agileClient.DBSession.Insert(a);
            //    DateTime start = DateTime.Now;
            //    List<Guid> ids = new List<Guid>();
            //    for (int i = 0; i < cnt; i++)
            //    {
            //        Animal a2 = new Animal { Name = "Name" + i };
            //        Task.Factory.StartNew(() =>
            //        {
            //            //∂‡œﬂ≥Ã≤‚ ‘≤¢∑¢
            //            database.agileClient.DBSession.Insert(a2);
            //            ids.Add(a2.Id);
            //        });
            //    }

            //    double total = DateTime.Now.Subtract(start).TotalMilliseconds;
            //    Console.WriteLine("Total Time:" + total);
            //    Console.WriteLine("Average Time:" + total / cnt);
            //}



            //[Fact]
            //public void Ext_SqlCommand_SqlMap()
            //{
            //    Animal a1 = new Animal { Name = "Foo" };
            //    Animal a2 = new Animal { Name = "Bar" };
            //    Animal a3 = new Animal { Name = "Baz" };

            //    List<Animal> lst = new List<Animal>() { a1, a2, a3 };

            //    database.agileClient.DBSession.InsertBatch<Animal>(lst);
            //    var animals = database.agileClient.DBSession.GetList<Animal>().ToList();
            //    Assert.Equal(3, animals.Count);

            //    var fileName = "AgileSqlMap.xml";
            //    var filePath = $"{Path.Combine(Environment.CurrentDirectory, "SqlMap")}{Path.DirectorySeparatorChar}{fileName}";

            //    Dictionary<string, object> collection = new Dictionary<string, object>();
            //    collection.Add("Name", "Foo");
            //    var cmd = SQLMapHelper.GetByCode(filePath, "test4", collection);
            //    string sql = cmd.TransferedSQL;

            //    dynamic param = new { Name = "Foo" };
            //    var a = database.agileClient.DBSession.QueryFirstOrDefault<Animal>(sql, param);
            //    Assert.Equal(a1.Id, a.Id);
            //}


        }
    }
}

