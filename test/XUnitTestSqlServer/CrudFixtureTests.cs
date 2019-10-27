using Agile.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using XUnitTestSqlServer.Model;

namespace XUnitTestSqlServer
{
    /// <summary>
    /// 基础 增删改查 测试
    /// </summary>
    public class CrudFixtureTests
    {
        public class InsertMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public InsertMethod(SqlServerBaseFixture data)
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


        public class GetMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public GetMethod(SqlServerBaseFixture data)
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


        public class DeleteMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public DeleteMethod(SqlServerBaseFixture data)
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


        public class UpdateMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public UpdateMethod(SqlServerBaseFixture data)
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

                database.agileClient.DBSession.Update(p2);

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


        public class GetListMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public GetListMethod(SqlServerBaseFixture data)
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


        public class GetPageMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public GetPageMethod(SqlServerBaseFixture data)
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
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>(1, 2, out allRowsCount, null, sort);
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
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>( 1, 3, out allRowsCount, predicate, sort);
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
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>(2, 2, out allRowsCount, null, sort);
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
                IEnumerable<Person> list = database.agileClient.DBSession.GetPageList<Person>(1, 3, out allRowsCount, predicate, sort);
                Assert.Equal(2, list.Count());
                Assert.True(list.All(p => p.FirstName == "Sigma" || p.FirstName == "Theta"));
            }
        }


        public class CountMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public CountMethod(SqlServerBaseFixture data)
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

                int count = database.agileClient.DBSession.Count<Person>(null);
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


        public class GetMultipleMethod : IClassFixture<SqlServerBaseFixture>
        {
            SqlServerBaseFixture database;

            public GetMultipleMethod(SqlServerBaseFixture data)
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
    }
}

