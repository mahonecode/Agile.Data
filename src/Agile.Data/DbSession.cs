using Agile.Data.Extensions;
using Agile.Data.SqlMap;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Agile.Data
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


        #region count
        /// <summary>
        /// 总数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count<T>(object predicate = null) where T : class;

        /// <summary>
        /// 总数量扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        int CountByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null) where T : class;
        #endregion


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
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="update"></param>
        /// <param name="where"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool UpdateByExpression<T>(T entity, Expression<Func<T, object>> update, Expression<Func<T, bool>> where, string tableName = null) where T : class;

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

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool DeleteByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null) where T : class;
        #endregion



            #region get
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        T Get<T>(dynamic primaryId) where T : class;

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        T GetByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null) where T : class;
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
        IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class;

        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        IEnumerable<T> GetListByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null, bool buffered = true) where T : class;
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
        IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out int allRowsCount, object predictate = null, IList<ISort> sort = null, bool buffered = true) where T : class;
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
        //PageData<T> GetPageData<T>(int pageIndex = 1, int pageSize = 10, object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class;
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
        /// <param name="buffered">缓冲</param>
        /// <returns></returns>
        IEnumerable<T> QueryList<T>(string sql, dynamic param = null, bool buffered = true);

        /// <summary>
        /// 执行sqlmap sql语句，查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        IEnumerable<T> QueryList<T>(SQLMapConfig config);

        /// <summary>
        /// 执行sql语句，查询分页实体集合,pageindex计数从0开始
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
        /// 执行sqlmap sql语句，查询分页实体集合,pageindex计数从0开始
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
        /// 执行sql语句，查询分页datatable,pageindex计数从0开始
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
        #endregion
    }

    /// <summary>
    /// 数据库连接事务的Session对象
    /// </summary>
    public class DbSession : IDbSession
    {
        private ConnectionConfig _connectionConfig;
        private SqlLoger _sqlLoger;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// 数据库事务对象
        /// </summary>
        public IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="conn">连接</param>
        internal DbSession(IDbConnection conn, ConnectionConfig connConfig)
        {
            Connection = conn;

            _connectionConfig = connConfig;
            _sqlLoger = new SqlLoger(connConfig);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="trans">事务</param>
        internal DbSession(IDbConnection conn, IDbTransaction trans, ConnectionConfig connConfig)
        {
            Connection = conn;
            Transaction = trans;

            _connectionConfig = connConfig;
            _sqlLoger = new SqlLoger(connConfig);
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
            Transaction = Connection.BeginTransaction(isolationLevel);
            return Transaction;
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
            Transaction = null;
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
            Transaction = null;
        }


        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                if (Transaction != null)
                {
                    Transaction.Dispose();
                    Transaction = null;
                }
                Connection.Close();
                Connection = null;
            }
            GC.SuppressFinalize(this);
        }





        private static object lockObj = new object();



        #region Micro-ORM

        #region count
        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count<T>(object predicate = null) where T : class
        {
            return Connection.Count<T>(predicate, Transaction);
        }

        public int CountByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null) where T : class
        {
            return Connection.CountByExpression<T>(whereExp, tableName, Transaction);
        }
        #endregion

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
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="update"></param>
        /// <param name="where"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool UpdateByExpression<T>(T entity, Expression<Func<T, object>> update, Expression<Func<T, bool>> where, string tableName = null) where T : class
        {
            bool isOk = Connection.UpdateByExpression<T>(entity, update, where, tableName, Transaction);
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool DeleteByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null) where T : class
        {
            return Connection.DeleteByExpression<T>(whereExp, tableName, Transaction);
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

        public T GetByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null) where T : class
        {
            return Connection.GetByExpression<T>(whereExp, tableName, Transaction);
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
        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class
        {
            return Connection.GetList<T>(predicate, sort, Transaction, null, buffered);
        }


        /// <summary>
        ///   列表查询
        /// </summary>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        public IEnumerable<T> GetListByExpression<T>(Expression<Func<T, bool>> whereExp, string tableName = null, bool buffered = true) where T : class
        {
            return Connection.GetListByExpression<T>( whereExp,  tableName , Transaction);
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
        //public PageData<T> GetPageData<T>(int pageIndex = 1, int pageSize = 10, object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class
        //{
        //    return null;
        //}

        //public PageData<T> GetPageData<T>(IDBSession session, int pageIndex = 1, int pageSize = 10, object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class
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

        #endregion



        #region SQL   SQLMap
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, dynamic param = null)
        {
            _sqlLoger.DebugSql(sql, param);
            return Connection.Execute(sql, param as object, Transaction);
        }

        /// <summary>
        /// 执行sqlmap sql语句，返回影响行数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public int ExecuteSql(SQLMapConfig config)
        {
            var cmd = ParseSqlMapFile(config);
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
            _sqlLoger.DebugSql(sql, param);
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
            _sqlLoger.DebugSql(sql, param);
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
            _sqlLoger.DebugSql(sql, param);
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
            _sqlLoger.DebugSql(sql, param);
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
            _sqlLoger.DebugSql(sql, param);
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
            _sqlLoger.DebugSql(sql, param);
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
            var cmd = ParseSqlMapFile(config);
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
            var cmd = ParseSqlMapFile(config);
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
            var cmd = ParseSqlMapFile(config);
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
            var cmd = ParseSqlMapFile(config);
            return QuerySingleOrDefault<T>(cmd.TransferedSQL, config.Parameters);
        }



        /// <summary>
        /// 执行sql语句 执行查询并映射结果 根据条件筛选数据集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryList<T>(string sql, dynamic param = null, bool buffered = true)
        {
            _sqlLoger.DebugSql(sql, param);
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
            var cmd = ParseSqlMapFile(config);
            return QueryList<T>(cmd.TransferedSQL, config.Parameters);
        }

        /// <summary>
        /// 执行sql语句 根据条件筛选数据集合（分页）,pageindex计数从0开始
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
            _sqlLoger.DebugSql(allRowsCountSql, dynamicParameters);
            allRowsCount = Connection.ExecuteScalar<int>(allRowsCountSql, dynamicParameters, Transaction);

            _sqlLoger.DebugSql(pageSql, dynamicParameters);
            IEnumerable<T> list = Connection.Query<T>(pageSql, dynamicParameters, Transaction, true);
            return list;
        }

        /// <summary>
        /// 执行sqlmap sql语句，查询分页实体集合,pageindex计数从0开始
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryPageList<T>(SQLMapConfig config)
        {
            var cmd = ParseSqlMapFile(config);
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
            _sqlLoger.DebugSql(sql, param);
            var dReader = Connection.ExecuteReader(sql, param as object, Transaction);
            //datareader 转 datatable
            var dt = DataReadToDataTable(dReader);
            return dt;
        }

        /// <summary>
        /// 执行sqlmap sql语句，查询datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public DataTable QueryDataTable(SQLMapConfig config)
        {
            var cmd = ParseSqlMapFile(config);
            return QueryDataTable(cmd.TransferedSQL, config.Parameters);
        }

        /// <summary>
        /// 执行sql语句 根据条件筛选数据集合（返回分页DataTable）,pageindex计数从0开始
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
            _sqlLoger.DebugSql(allRowsCountSql, dynamicParameters);
            allRowsCount = Connection.ExecuteScalar<int>(allRowsCountSql, dynamicParameters, Transaction);

            _sqlLoger.DebugSql(pageSql, dynamicParameters);
            var dReader = Connection.ExecuteReader(pageSql, dynamicParameters, Transaction);
            //datareader 转 datatable
            var dt = DataReadToDataTable(dReader);
            return dt;
        }


        /// <summary>
        /// 执行sqlmap sql语句，查询分页datatable,pageindex计数从0开始
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public DataTable QueryPageDataTable(SQLMapConfig config)
        {
            var cmd = ParseSqlMapFile(config);
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
            //注意 前台需要传入 DynamicParameters 参数
            _sqlLoger.DebugSql(procName, param);
            return Connection.Execute(procName, param as object, Transaction, null, CommandType.StoredProcedure);

            //针对复杂类型的 存储过程，还是在业务里面扩展一个方法自己控制比较方便，如下案例
            //使用ado.net基础方法操作存储过程
            //using (var conn = Repository.DBSession.Connection)
            //{
            //    conn.Open();
            //    OracleCommand cmd = new OracleCommand("pro_sto_bal", (OracleConnection)conn);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add(new OracleParameter("v_n", OracleDbType.Varchar2,32, "2019", ParameterDirection.Input));
            //    cmd.Parameters.Add(new OracleParameter("v_y", OracleDbType.Varchar2,32, "06", ParameterDirection.Input));
            //    cmd.Parameters.Add(new OracleParameter("poret", OracleDbType.Int32,32,"", ParameterDirection.Output));
            //    cmd.Parameters.Add(new OracleParameter("pomsg", OracleDbType.Varchar2,32,"", ParameterDirection.Output));
            //    cmd.ExecuteNonQuery();
            //    //取出输出参数值
            //    var v1 = cmd.Parameters["poret"].Value;
            //    var v2 = cmd.Parameters["pomsg"].Value;
            //}
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

        private SQLMapCommandInfo ParseSqlMapFile(SQLMapConfig config)
        {
            var fileFolder = string.IsNullOrEmpty(config.SQLMapFileFolder) ? "SqlMap" : config.SQLMapFileFolder;
            var fileName = config.SQLMapFileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileFolder, fileName);
            var cmd = SQLMapHelper.GetByCode(filePath, config.Code, config.Parameters);
            return cmd;
        }
        #endregion
    }
}