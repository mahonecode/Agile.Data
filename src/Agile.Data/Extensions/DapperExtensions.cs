using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Agile.Data.Extensions
{
    /// <summary>
    /// dapper扩展
    /// </summary>
    public static class DapperExtensions
    {
        #region other
        private readonly static object _lock = new object();
        
        private static IDapperImplementor _instance;
        private static Func<IDapperExtensionsConfiguration, IDapperImplementor> _instanceFactory;
        

        static DapperExtensions()
        {
            Configure(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect());
        }

        /// <summary>
        /// Gets the Dapper Extensions Implementation
        /// </summary>
        public static IDapperImplementor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = InstanceFactory(Configuration);
                        }
                    }
                }

                return _instance;
            }
        }

        public static IDapperExtensionsConfiguration Configuration { get; private set; }


        /// <summary>
        /// Get or sets the Dapper Extensions Implementation Factory.
        /// </summary>
        public static Func<IDapperExtensionsConfiguration, IDapperImplementor> InstanceFactory
        {
            get
            {
                if (_instanceFactory == null)
                {
                    _instanceFactory = config => new DapperImplementor(new SqlGeneratorImpl(config), config.ConnConfig);
                }

                return _instanceFactory;
            }
            set
            {
                _instanceFactory = value;
                Configure(Configuration.DefaultMapper, Configuration.MappingAssemblies, Configuration.Dialect);
            }
        }


        /// <summary>
        /// Gets or sets the default class mapper to use when generating class maps. If not specified, AutoClassMapper<T> is used.
        /// DapperExtensions.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static Type DefaultMapper
        {
            get
            {
                return Configuration.DefaultMapper;
            }
            set
            {
                Configure(value, Configuration.MappingAssemblies, Configuration.Dialect);
            }
        }

        /// <summary>
        /// Gets or sets the type of sql to be generated.
        /// DapperExtensions.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static ISqlDialect SqlDialect
        {
            get
            {
                return Configuration.Dialect;
            }
            set
            {
                Configure(Configuration.DefaultMapper, Configuration.MappingAssemblies, value);
            }
        }



        /// <summary>
        /// Add other assemblies that Dapper Extensions will search if a mapping is not found in the same assembly of the POCO.
        /// </summary>
        /// <param name="assemblies"></param>
        public static void SetMappingAssemblies(IList<Assembly> assemblies)
        {
            Configure(Configuration.DefaultMapper, assemblies, Configuration.Dialect);
        }

        /// <summary>
        /// Configure DapperExtensions extension methods. typeof(AutoClassMapper<>), new List<Assembly>(),sqlDialect
        /// </summary>
        /// <param name="sqlDialect"></param>
        public static void Configure(ISqlDialect sqlDialect)
        {
            IDapperExtensionsConfiguration configuration = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), sqlDialect);
            Configure(configuration);
        }


        /// <summary>
        /// Configure DapperExtensions extension methods.
        /// </summary>
        /// <param name="defaultMapper"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="sqlDialect"></param>
        public static void Configure(Type defaultMapper, IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect)
        {
            IDapperExtensionsConfiguration configuration = new DapperExtensionsConfiguration(defaultMapper, mappingAssemblies, sqlDialect);
            Configure(configuration);
        }


        /// <summary>
        /// Configure DapperExtensions extension methods.
        /// </summary>
        /// <param name="defaultMapper"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="sqlDialect"></param>
        public static void Configure(IDapperExtensionsConfiguration configuration)
        {
            _instance = null;
            Configuration = configuration;
        }

        #endregion



        #region Insert
        /// <summary>
        /// Executes an insert query for the specified entity.
        /// </summary>
        public static void Insert<T>(this IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Insert<T>(connection, entities, null, null, transaction, commandTimeout);

        public static void Insert<T>(this IDbConnection connection, IEnumerable<T> entities, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Insert<T>(connection, entities, tableName, null, transaction, commandTimeout);

        public static void Insert<T>(this IDbConnection connection, IEnumerable<T> entities, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Instance.Insert<T>(connection, entities, transaction, commandTimeout, tableName, schemaName);
        }

        /// <summary>
        /// Executes an insert query for the specified entity, returning the primary key.  
        /// If the entity has a single key, just the value is returned.  
        /// If the entity has a composite key, an IDictionary&lt;string, object&gt; is returned with the key values.
        /// The key value for the entity will also be updated if the KeyType is a Guid or Identity.
        /// </summary>
        public static dynamic Insert<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Insert<T>(connection, entity, null, null, transaction, commandTimeout);

        public static dynamic Insert<T>(this IDbConnection connection, T entity, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Insert<T>(connection, entity, tableName, null, transaction, commandTimeout);

        public static dynamic Insert<T>(this IDbConnection connection, T entity, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Insert<T>(connection, entity, transaction, commandTimeout, tableName, schemaName);
        }

        #endregion

        #region Update
        /// <summary>
        /// Executes an update query for the specified entity.
        /// </summary>
        public static bool Update<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(connection, entity, null, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public static bool Update<T>(this IDbConnection connection, T entity, string tableName, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(connection, entity, tableName, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public static bool Update<T>(this IDbConnection connection, T entity, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
        {
            return Instance.Update<T>(connection, entity, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);
        }

        public static bool Update<T>(this IDbConnection connection, T entity, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(connection, entity, null, null, predicate, transaction, commandTimeout, ignoreAllKeyProperties);

        public static bool Update<T>(this IDbConnection connection, T entity, string tableName, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => Update<T>(connection, entity, tableName, null, predicate, transaction, commandTimeout, ignoreAllKeyProperties);

        public static bool Update<T>(this IDbConnection connection, T entity, string tableName, string schemaName, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
        {
            return Instance.Update<T>(connection, entity, predicate, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);
        }

        public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(connection, entity, null, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entity, string tableName, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(connection, entity, tableName, null, transaction, commandTimeout, ignoreAllKeyProperties);

        public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entity, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
        {
            return await Instance.UpdateAsync<T>(connection, entity, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);
        }
        public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entity, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(connection, entity, null, null, predicate, transaction, commandTimeout, ignoreAllKeyProperties);

        public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entity, string tableName, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
            => await UpdateAsync<T>(connection, entity, tableName, null, predicate, transaction, commandTimeout, ignoreAllKeyProperties);

        public static async Task<bool> UpdateAsync<T>(this IDbConnection connection, T entity, string tableName, string schemaName, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool ignoreAllKeyProperties = false) where T : class
        {
            return await Instance.UpdateAsync<T>(connection, entity, predicate, transaction, commandTimeout, tableName, schemaName, ignoreAllKeyProperties);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Executes a delete query for the specified entity.
        /// </summary>
        public static bool Delete<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Delete<T>(connection, entity, null, null, transaction, commandTimeout);

        public static bool Delete<T>(this IDbConnection connection, T entity, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Delete<T>(connection, entity, tableName, null, transaction, commandTimeout);

        public static bool Delete<T>(this IDbConnection connection, T entity, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Delete<T>(connection, entity, transaction, commandTimeout, tableName, schemaName);
        }

        public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(connection, entity, null, null, transaction, commandTimeout);

        public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, T entity, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(connection, entity, tableName, null, transaction, commandTimeout);

        public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, T entity, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.DeleteAsync<T>(connection, entity, transaction, commandTimeout, tableName, schemaName);
        }

        /// <summary>
        /// Executes a delete query using the specified predicate.
        /// </summary>
        public static bool Delete<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Delete<T>(connection, predicate, null, null, transaction, commandTimeout);

        public static bool Delete<T>(this IDbConnection connection, object predicate, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Delete<T>(connection, predicate, tableName, null, transaction, commandTimeout);

        public static bool Delete<T>(this IDbConnection connection, object predicate, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Delete<T>(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }
        public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
           => await DeleteAsync<T>(connection, predicate, null, null, transaction, commandTimeout);

        public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, object predicate, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await DeleteAsync<T>(connection, predicate, tableName, null, transaction, commandTimeout);

        public static async Task<bool> DeleteAsync<T>(this IDbConnection connection, object predicate, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.DeleteAsync<T>(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }

        #endregion



        #region Count
        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<T>(this IDbConnection connection, object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Count<T>(connection, null, null, predicate, transaction, commandTimeout);

        public static int Count<T>(this IDbConnection connection, string tableName, object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => Count<T>(connection, tableName, null, predicate, transaction, commandTimeout);

        public static int Count<T>(this IDbConnection connection, string tableName, string schemaName, object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Count<T>(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }

        public static async Task<int> CountAsync<T>(this IDbConnection connection, object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await CountAsync<T>(connection, null, null, predicate, transaction, commandTimeout);

        public static async Task<int> CountAsync<T>(this IDbConnection connection, string tableName, object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await CountAsync<T>(connection, tableName, null, predicate, transaction, commandTimeout);

        public static async Task<int> CountAsync<T>(this IDbConnection connection, string tableName, string schemaName, object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.CountAsync<T>(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }

        #endregion

        #region Get
        /// <summary>
        /// Executes a query for the specified id, returning the data typed as per T
        /// </summary>
        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var result = Instance.Get<T>(connection, id, transaction, commandTimeout, null, null);
            return (T)result;
        }

        public static T Get<T>(this IDbConnection connection, dynamic id, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var result = Instance.Get<T>(connection, id, transaction, commandTimeout, tableName, null);
            return (T)result;
        }

        public static T Get<T>(this IDbConnection connection, dynamic id, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var result = Instance.Get<T>(connection, id, transaction, commandTimeout, tableName, schemaName);
            return (T)result;
        }

        public static async Task<T> GetAsync<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetAsync<T>(connection, id, null, null, transaction, commandTimeout);

        public static async Task<T> GetAsync<T>(this IDbConnection connection, dynamic id, string tableName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
         => await GetAsync<T>(connection, id, null, null, transaction, commandTimeout);

        public static async Task<T> GetAsync<T>(this IDbConnection connection, dynamic id, string tableName, string schemaName, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.GetAsync<T>(connection, id, transaction, commandTimeout, tableName, schemaName);
        }

        #endregion

        #region GetList
        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// </summary>
        public static IEnumerable<T> GetList<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
            => GetList<T>(connection, null, null, predicate, sort, transaction, commandTimeout, buffered);

        public static IEnumerable<T> GetList<T>(this IDbConnection connection, string tableName, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
            => GetList<T>(connection, tableName, null, predicate, sort, transaction, commandTimeout, buffered);

        public static IEnumerable<T> GetList<T>(this IDbConnection connection, string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetList<T>(connection, predicate, sort, transaction, commandTimeout, buffered, tableName, schemaName);
        }

        public static async Task<IEnumerable<T>> GetListAsync<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetListAsync<T>(connection, null, null, predicate, sort, transaction, commandTimeout);

        public static async Task<IEnumerable<T>> GetListAsync<T>(this IDbConnection connection, string tableName, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetListAsync<T>(connection, tableName, null, predicate, sort, transaction, commandTimeout);

        public static async Task<IEnumerable<T>> GetListAsync<T>(this IDbConnection connection, string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.GetListAsync<T>(connection, predicate, sort, transaction, commandTimeout, tableName, schemaName);
        }

        #endregion

        #region GetSet
        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified firstResult and maxResults.
        /// </summary>
        public static IEnumerable<T> GetSet<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
            => GetSet<T>(connection, null, null, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);

        public static IEnumerable<T> GetSet<T>(this IDbConnection connection, string tableName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
            => GetSet<T>(connection, tableName, null, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);

        public static IEnumerable<T> GetSet<T>(this IDbConnection connection, string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetSet<T>(connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered, tableName, schemaName);
        }

        public static async Task<IEnumerable<T>> GetSetAsync<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetSetAsync<T>(connection, null, null, predicate, sort, firstResult, maxResults, transaction, commandTimeout);

        public static async Task<IEnumerable<T>> GetSetAsync<T>(this IDbConnection connection, string tableName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetSetAsync<T>(connection, tableName, null, predicate, sort, firstResult, maxResults, transaction, commandTimeout);

        public static async Task<IEnumerable<T>> GetSetAsync<T>(this IDbConnection connection, string tableName, string schemaName, object predicate = null, IList<ISort> sort = null, int firstResult = 1, int maxResults = 10, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.GetSetAsync<T>(connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, tableName, schemaName);
        }

        #endregion

        #region GetPage
        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified page and resultsPerPage.
        /// </summary>
        public static IEnumerable<T> GetPage<T>(this IDbConnection connection, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
            => GetPage<T>(connection, null, null, page, resultsPerPage, predicate, sort, transaction, commandTimeout, buffered);

        public static IEnumerable<T> GetPage<T>(this IDbConnection connection, string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
            => GetPage<T>(connection, tableName, null, page, resultsPerPage, predicate, sort, transaction, commandTimeout, buffered);

        public static IEnumerable<T> GetPage<T>(this IDbConnection connection, string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetPage<T>(connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered, tableName, schemaName);
        }

        public static async Task<IEnumerable<T>> GetPageAsync<T>(this IDbConnection connection, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetPageAsync<T>(connection, null, null, page, resultsPerPage, predicate, sort,  transaction, commandTimeout);

        public static async Task<IEnumerable<T>> GetPageAsync<T>(this IDbConnection connection, string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null,  IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetPageAsync<T>(connection, tableName, null, page, resultsPerPage, predicate, sort,transaction, commandTimeout);

        public static async Task<IEnumerable<T>> GetPageAsync<T>(this IDbConnection connection, string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.GetPageAsync<T>(connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, tableName, schemaName);
        }
        #endregion

        #region GetPageData

        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified page and resultsPerPage.
        /// </summary>
        public static PageData<T> GetPageData<T>(this IDbConnection connection, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null,  IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => GetPageData<T>(connection, null, null, page, resultsPerPage, predicate, sort, transaction, commandTimeout);

        public static PageData<T> GetPageData<T>(this IDbConnection connection, string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null,  IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => GetPageData<T>(connection, tableName, null, page, resultsPerPage, predicate, sort, transaction, commandTimeout);

        public static PageData<T> GetPageData<T>(this IDbConnection connection, string tableName, string schemaName, int page = 1, int resultsPerPage = 10,  object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.GetPageData<T>(connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, tableName, schemaName);
        }
        public static async Task<PageData<T>> GetPageDataAsync<T>(this IDbConnection connection, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
           => await GetPageDataAsync<T>(connection, null, null, page, resultsPerPage, predicate, sort, transaction, commandTimeout);

        public static async Task<PageData<T>> GetPageDataAsync<T>(this IDbConnection connection, string tableName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
            => await GetPageDataAsync<T>(connection, tableName, null, page, resultsPerPage, predicate, sort, transaction, commandTimeout);

        public static async Task<PageData<T>> GetPageDataAsync<T>(this IDbConnection connection, string tableName, string schemaName, int page = 1, int resultsPerPage = 10, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return await Instance.GetPageDataAsync<T>(connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, tableName, schemaName);
        }
        #endregion

        #region GetMultiple

        /// <summary>
        /// Executes a select query for multiple objects, returning IMultipleResultReader for each predicate.
        /// </summary>
        public static IMultipleResultReader GetMultiple(this IDbConnection connection, MultiplePredicate predicate = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => GetMultiple(connection, null, null, predicate, transaction, commandTimeout);

        public static IMultipleResultReader GetMultiple(this IDbConnection connection, string tableName, MultiplePredicate predicate = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => GetMultiple(connection, tableName, null, predicate, transaction, commandTimeout);

        public static IMultipleResultReader GetMultiple(this IDbConnection connection, string tableName, string schemaName, MultiplePredicate predicate = null,  IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Instance.GetMultiple(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }

        public static async Task<IMultipleResultReader> GetMultipleAsync(this IDbConnection connection, MultiplePredicate predicate = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await GetMultipleAsync(connection, null, null, predicate, transaction, commandTimeout);

        public static async Task<IMultipleResultReader> GetMultipleAsync(this IDbConnection connection, string tableName, MultiplePredicate predicate = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await GetMultipleAsync(connection, tableName, null, predicate, transaction, commandTimeout);

        public static async Task<IMultipleResultReader> GetMultipleAsync(this IDbConnection connection, string tableName, string schemaName, MultiplePredicate predicate = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await Instance.GetMultipleAsync(connection, predicate, transaction, commandTimeout, tableName, schemaName);
        }
        #endregion


        #region Expression
        /// <summary>
        /// 统计数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int CountByExpression<T>(this IDbConnection connection, Expression<Func<T, bool>> whereExp, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.CountByExpression<T>(connection, whereExp, tableName, transaction, commandTimeout);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool DeleteByExpression<T>(this IDbConnection connection, Expression<Func<T, bool>> whereExp, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.DeleteByExpression<T>(connection, whereExp, tableName, transaction, commandTimeout);
        }

        /// <summary>
        /// 更新扩展，部分更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entity"></param>
        /// <param name="updateExp"></param>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool UpdateByExpression<T>(this IDbConnection connection, T entity, Expression<Func<T, object>> updateExp, Expression<Func<T, bool>> whereExp, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.UpdateByExpression<T>(connection, entity, updateExp, whereExp, tableName, transaction, commandTimeout);
        }

        /// <summary>
        ///  获取单项扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="con"></param>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static T GetByExpression<T>(this IDbConnection connection, Expression<Func<T, bool>> whereExp, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.GetByExpression<T>(connection, whereExp, tableName, transaction, commandTimeout);
        }

        /// <summary>
        /// 获取列表扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetListByExpression<T>(this IDbConnection connection, Expression<Func<T, bool>> whereExp, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = true) where T : class
        {
            return Instance.GetListByExpression<T>(connection, whereExp, tableName, transaction, commandTimeout, buffered);
        }

        #endregion


        #region helper
        /// <summary>
        /// Gets the appropriate mapper for the specified type T. 
        /// If the mapper for the type is not yet created, a new mapper is generated from the mapper type specifed by DefaultMapper.
        /// </summary>
        public static IClassMapper GetMap<T>() where T : class
        {
            return Instance.SqlGenerator.Configuration.GetMap<T>();
        }

        /// <summary>
        /// Clears the ClassMappers for each type.
        /// </summary>
        public static void ClearCache()
        {
            Instance.SqlGenerator.Configuration.ClearCache();
        }

        /// <summary>
        /// Generates a COMB Guid which solves the fragmented index issue.
        /// See: http://davybrion.com/blog/2009/05/using-the-guidcomb-identifier-strategy
        /// </summary>
        public static Guid GetNextGuid()
        {
            return Instance.SqlGenerator.Configuration.GetNextGuid();
        }
        #endregion
    }
}
