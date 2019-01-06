using Agile.Data.MySql.Extensions;
using Agile.Data.MySql.SqlMap;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Agile.Data.MySql
{
    /// <summary>
    /// 数据连接事务的 DbSession 接口
    /// </summary>
    public interface IDbSession : IDisposable
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// 数据库事务
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolation"></param>
        /// <returns></returns>
        IDbTransaction BeginTrans(IsolationLevel isolation = IsolationLevel.ReadCommitted);

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();


        #region insert update delete
        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        dynamic Insert<T>(T entity) where T : class;

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <returns></returns>
        void InsertBatch<T>(IEnumerable<T> entityList) where T : class;

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update<T>(T entity) where T : class;

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Update<T>(T entity, object predicate) where T : class;

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete<T>(T entity) where T : class;

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Delete<T>(object predicate) where T : class;
        #endregion

        #region count
        /// <summary>
        /// 总数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count<T>(object predicate) where T : class;
        #endregion

        #region get
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        T Get<T>(dynamic primaryId) where T : class;
        #endregion

        #region getlist
        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, bool buffered = false) where T : class;
        #endregion

        #region getpage
        /// <summary>
        /// 查询分页实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="predictate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out int allRowsCount, object predictate = null, IList<ISort> sort = null, bool buffered = false) where T : class;
        #endregion

        #region getpagedata
        /// <summary>
        /// 查询分页实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        //PageData<T> GetPageData<T>(int pageIndex = 1, int pageSize = 10, object predicate = null, IList<ISort> sort = null, bool buffered = false) where T : class;
        #endregion

        #region GetMultiple
        /// <summary>
        /// 查询IMultipleResultReader
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IMultipleResultReader GetMultiple(MultiplePredicate predicate);
        #endregion

        #region SQLCommand   SQLMap
        /// <summary>
        /// 执行sql语句，返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteSql(string sql, dynamic param = null);

        /// <summary>
        /// 执行sqlmap sql语句，返回影响行数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        int ExecuteSql(SQLMapConfig config);


        /// <summary>
        /// 执行sql语句，返回第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, dynamic param = null);


        /// <summary>
        /// 执行sql语句，返回第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, dynamic param = null);


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果
        /// First           没有项:抛异常 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirst<T>(string sql, dynamic param = null);

        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// FirstOrDefault  没有项:默认值 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(string sql, dynamic param = null);

        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列中没有元素则会引发异常
        /// Single          没有项:抛异常 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QuerySingle<T>(string sql, dynamic param = null);

        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// SingleOrDefault 没有项:默认值 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QuerySingleOrDefault<T>(string sql, dynamic param = null);

        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果
        /// First           没有项:抛异常 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirst<T>(SQLMapConfig config);


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// FirstOrDefault  没有项:默认值 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(SQLMapConfig config);

        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列中没有元素则会引发异常
        /// Single          没有项:抛异常 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QuerySingle<T>(SQLMapConfig config);

        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// SingleOrDefault 没有项:默认值 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T QuerySingleOrDefault<T>(SQLMapConfig config);


        /// <summary>
        /// 执行sql语句，查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        IEnumerable<T> QueryList<T>(string sql, dynamic param = null, bool buffered = false);

        /// <summary>
        /// 执行sqlmap sql语句，查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        IEnumerable<T> QueryList<T>(SQLMapConfig config);

        /// <summary>
        /// 执行sql语句，查询分页实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> QueryPageList<T>(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null);

        /// <summary>
        /// 执行sqlmap sql语句，查询分页实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        IEnumerable<T> QueryPageList<T>(SQLMapConfig config);


        /// <summary>
        /// 执行sql语句，查询datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataTable QueryDataTable(string sql, dynamic param = null);

        /// <summary>
        /// 执行sqlmap sql语句，查询datatable
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        DataTable QueryDataTable(SQLMapConfig config);

        /// <summary>
        /// 执行sql语句，查询分页datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataTable QueryPageDataTable(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null);

        /// <summary>
        /// 执行sqlmap sql语句，查询分页datatable
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        DataTable QueryPageDataTable(SQLMapConfig config);
        #endregion


        #region 存储过程
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteProcedure(string procName, dynamic param = null);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> ExecuteProcedure<T>(string procName, dynamic param);
        #endregion
    }

    /// <summary>
    /// 数据库连接事务的Session对象
    /// </summary>
    public class DbSession : IDbSession
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public IDbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// 数据库事务对象
        /// </summary>
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="conn">连接</param>
        internal DbSession(IDbConnection conn)
        {
            _connection = conn;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        internal DbSession(IDbConnection conn, IDbTransaction trans)
        {
            _connection = conn;
            _transaction = trans;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolation"></param>
        /// <returns></returns>
        public IDbTransaction BeginTrans(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (Connection.State != ConnectionState.Open)
            {
                //dapper手动打开的话会保持长连接，否则每次查询之后会关闭连接
                Connection.Open();
            }
            _transaction = Connection.BeginTransaction(isolationLevel);
            return _transaction;
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }


        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                if (_transaction != null)
                {
                    //_transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;

                }
                _connection.Close();
                _connection = null;
            }
            GC.SuppressFinalize(this);
        }


        private static object lockObj = new object();




        #region insert update delete

        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public dynamic Insert<T>(T entity) where T : class
        {
            dynamic result = Connection.Insert<T>(entity, Transaction);
            return result;
        }



        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        public void InsertBatch<T>(IEnumerable<T> entityList) where T : class
        {
            Connection.Insert<T>(entityList, Transaction);
        }


        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update<T>(T entity) where T : class
        {
            bool isOk = Connection.Update<T>(entity, Transaction);
            return isOk;
        }



        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Update<T>(T entity, object predicate) where T : class
        {
            bool isOk = Connection.Update<T>(entity, predicate, Transaction);
            return isOk;
        }


        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<T>(T entity) where T : class
        {
            bool isOk = Connection.Delete<T>(entity, Transaction);
            return isOk;
        }


        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns> 
        public bool Delete<T>(object predicate) where T : class
        {
            return Connection.Delete<T>(predicate, Transaction);
        }
        #endregion

        #region count
        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count<T>(object predicate) where T : class
        {
            return Connection.Count<T>(predicate, Transaction);
        }

        #endregion


        #region get
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T Get<T>(dynamic primaryId) where T : class
        {
            return Connection.Get<T>(primaryId as object, Transaction);
        }

        #endregion

        #region getlist
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, bool buffered = false) where T : class
        {
            return Connection.GetList<T>(predicate, sort, Transaction, null, buffered);
        }
        #endregion

        #region getpage
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out int allRowsCount, object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class
        {
            IEnumerable<T> entityList = Connection.GetPage<T>(pageIndex, pageSize, predicate, sort, Transaction, null, buffered);
            allRowsCount = Connection.Count<T>(predicate, Transaction);
            return entityList;
        }
        #endregion


        #region getpagedata
        //public PageData<T> GetPageData<T>(int pageIndex = 1, int pageSize = 10, object predicate = null, IList<ISort> sort = null, bool buffered = false) where T : class
        //{
        //    return null;
        //}

        //public PageData<T> GetPageData<T>(IDBSession session, int pageIndex = 1, int pageSize = 10, object predicate = null, IList<ISort> sort = null, bool buffered = false) where T : class
        //{
        //    return null;
        //}
        #endregion


        #region GetMultiple
        /// <summary>
        /// 通过 MultiplePredicate 查询返回 IMultipleResultReader
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IMultipleResultReader GetMultiple(MultiplePredicate predicate)
        {
            return Connection.GetMultiple(predicate, Transaction);
        }
        #endregion





        #region SQLCommand   SQLMap
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            return Connection.Execute(sql, param as object, Transaction);
        }

        /// <summary>
        /// 执行sqlmap sql语句，返回影响行数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public int ExecuteSql(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return ExecuteSql(cmd.TransferedSQL, config.Parameters);
        }


        /// <summary>
        /// 执行sql语句，返回第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            return Connection.ExecuteScalar(sql, param as object, Transaction);
        }


        /// <summary>
        /// 执行SQL语句，返回查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            return Connection.ExecuteScalar<T>(sql, param as object, Transaction);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果
        /// First           没有项:抛异常 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            return Connection.QueryFirst<T>(sql, param as object, Transaction);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// FirstOrDefault  没有项:默认值 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            return Connection.QueryFirstOrDefault<T>(sql, param as object, Transaction);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列中没有元素则会引发异常
        /// Single          没有项:抛异常 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            return Connection.QuerySingle<T>(sql, param as object, Transaction);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// SingleOrDefault 没有项:默认值 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingleOrDefault<T>(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            return Connection.QuerySingleOrDefault<T>(sql, param as object, Transaction);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果
        /// First           没有项:抛异常 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirst<T>(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return QueryFirst<T>(cmd.TransferedSQL, config.Parameters);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列不包含任何元素则为默认值
        /// FirstOrDefault  没有项:默认值 有一项:当前项 有多项:第一项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return QueryFirstOrDefault<T>(cmd.TransferedSQL, config.Parameters);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列中没有元素则会引发异常
        /// Single          没有项:抛异常 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle<T>(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return QuerySingle<T>(cmd.TransferedSQL, config.Parameters);
        }


        /// <summary>
        /// 执行sql语句 执行查询并映射第一个结果，如果序列为空则为默认值。如果序列中有多个元素，则此方法将引发异常
        /// SingleOrDefault 没有项:默认值 有一项:当前项 有多项:抛异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingleOrDefault<T>(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return QuerySingleOrDefault<T>(cmd.TransferedSQL, config.Parameters);
        }



        /// <summary>
        /// 执行sql语句 执行查询并映射结果 根据条件筛选数据集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryList<T>(string sql, dynamic param = null, bool buffered = false)
        {
            DebugSql(sql, param);
            return Connection.Query<T>(sql, param as object, Transaction, buffered);
        }

        /// <summary>
        /// 执行sqlmap sql语句，执行查询并映射结果 根据条件筛选数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryList<T>(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return QueryList<T>(cmd.TransferedSQL, config.Parameters);
        }

        /// <summary>
        /// 执行sql语句 根据条件筛选数据集合（分页）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageList<T>(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            var pageSql = DapperExtensions.SqlDialect.GetPagingSql(sql, pageIndex, pageSize, parameters);

            DynamicParameters dynamicParameters = new DynamicParameters();
            if (param != null)
            {
                dynamicParameters = new DynamicParameters(param);
            }
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            string allRowsCountSql = DapperExtensions.Instance.SqlGenerator.PageCount(sql);
            DebugSql(pageSql, dynamicParameters);
            IEnumerable<T> list = Connection.Query<T>(pageSql, dynamicParameters, Transaction, true);

            DebugSql(allRowsCountSql, dynamicParameters);
            allRowsCount = (int)Connection.Query(allRowsCountSql, dynamicParameters, Transaction, false).Single().Total;
            return list;
        }

        /// <summary>
        /// 执行sqlmap sql语句，查询分页实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageList<T>(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            int allRowsCount = 0;
            var list = QueryPageList<T>(cmd.TransferedSQL, config.PageIndex, config.PageSize, out allRowsCount, config.Parameters);
            config.AllRowsCount = allRowsCount;
            return list;
        }

        /// <summary>
        /// 执行sql语句 根据条件筛选数据集合（返回DataTable）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql, dynamic param = null)
        {
            DebugSql(sql, param);
            var dReader = Connection.ExecuteReader(sql, param as object, Transaction);

            //datareader 转 datatable
            return DataReadToDataTable(dReader);
        }

        /// <summary>
        /// 执行sqlmap sql语句，查询datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public DataTable QueryDataTable(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return QueryDataTable(cmd.TransferedSQL, config.Parameters);
        }

        /// <summary>
        /// 执行sql语句 根据条件筛选数据集合（返回分页DataTable）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable QueryPageDataTable(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var pageSql = DapperExtensions.SqlDialect.GetPagingSql(sql, pageIndex, pageSize, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (param != null)
            {
                dynamicParameters = new DynamicParameters(param);
            }
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            string allRowsCountSql = DapperExtensions.Instance.SqlGenerator.PageCount(sql);
            DebugSql(pageSql, dynamicParameters);
            var dReader = Connection.ExecuteReader(pageSql, dynamicParameters, Transaction);

            DebugSql(allRowsCountSql, dynamicParameters);
            allRowsCount = (int)Connection.Query(allRowsCountSql, dynamicParameters, Transaction, false).Single().Total;

            //datareader 转 datatable
            return DataReadToDataTable(dReader);
        }


        /// <summary>
        /// 执行sqlmap sql语句，查询分页datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public DataTable QueryPageDataTable(SQLMapConfig config)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlMap", config.SQLMapFile);
            if (!File.Exists(filePath))
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SqlMap", config.SQLMapFile);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            int allRowsCount = 0;
            var datatable = QueryPageDataTable(cmd.TransferedSQL, config.PageIndex, config.PageSize, out allRowsCount, config.Parameters);
            config.AllRowsCount = allRowsCount;
            return datatable;
        }
        #endregion


        #region 存储过程
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProcedure(string procName, dynamic param = null)
        {
            DebugSql(procName, param);
            return Connection.Execute(procName, param as object, Transaction, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 执行存储过程 根据条件筛选数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteProcedure<T>(string procName, dynamic param)
        {
            DebugSql(procName, param);
            IEnumerable<T> list = Connection.Query<T>(procName, param as object, Transaction, false, null, CommandType.StoredProcedure);
            return list;
        }
        #endregion

        #region helper

        /// <summary>
        /// datareader 转 datatable
        /// </summary>
        /// <param name="dReader"></param>
        /// <returns></returns>
        private DataTable DataReadToDataTable(IDataReader dReader)
        {
            lock (lockObj)
            {
                try
                {
                    DataTable dt = new DataTable();
                    bool init = false;
                    dt.BeginLoadData();
                    object[] vals = new object[0];
                    while (dReader.Read())
                    {
                        if (!init)
                        {
                            init = true;
                            int fieldCount = dReader.FieldCount;
                            for (int i = 0; i < fieldCount; i++)
                            {
                                dt.Columns.Add(dReader.GetName(i), dReader.GetFieldType(i));
                            }
                            vals = new object[fieldCount];
                        }
                        dReader.GetValues(vals);
                        dt.LoadDataRow(vals, true);
                    }
                    dReader.Close();
                    dt.EndLoadData();
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception($"DataReadToDataTable异常：{ex.Message}");
                }
                finally
                {
                    if (!dReader.IsClosed)
                        dReader.Close();
                }
            }
        }


        /// <summary>
        /// 调试sql脚本
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        private void DebugSql(string sql, object param)
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(sql, param);
            }
        }
        #endregion
    }
}