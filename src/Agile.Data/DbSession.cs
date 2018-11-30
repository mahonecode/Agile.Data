﻿using Agile.Data.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        bool Delete<T>(object predicate ) where T : class;
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

        #region sqlcommand
        /// <summary>
        /// 执行sql语句，返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteCommad(string sql, dynamic param = null);

        /// <summary>
        /// 执行sql语句，查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        T ExecuteQuerySingle<T>(string sql, dynamic param = null) where T : class;

        /// <summary>
        /// 执行sql语句，查询实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        IEnumerable<T> ExecuteQuery<T>(string sql, dynamic param = null, bool buffered = false) where T : class;

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
        IEnumerable<T> ExecuteQuery<T>(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null) where T : class;

        /// <summary>
        /// 执行sql语句，查询datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataTable ExecuteQueryDataTable(string sql, dynamic param = null);

        /// <summary>
        /// 执行sql语句，查询分页datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        DataTable ExecuteQueryDataTable(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null);

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
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int ExecuteProc(string procName, dynamic param = null);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> ExecuteProc<T>(string procName, dynamic param) where T : class;

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





        #region 直接执行sql语句
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteCommad(string sql, dynamic param = null)
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(sql, param);
            }
            return Connection.Execute(sql, param as object, Transaction);
        }


        /// <summary>
        /// 执行sql语句 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteQuerySingle<T>(string sql, dynamic param = null) where T : class
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(sql, param);
            }
            return Connection.QuerySingle<T>(sql, param as object, Transaction);
        }



        /// <summary>
        /// 执行sql语句 根据条件筛选数据集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteQuery<T>(string sql, dynamic param = null, bool buffered = false) where T : class
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(sql, param);
            }
            return Connection.Query<T>(sql, param as object, Transaction, buffered);
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
        public IEnumerable<T> ExecuteQuery<T>(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null) where T : class
        {
            while (sql.Contains("\r\n"))
            {
                sql = sql.Replace("\r\n", " ");
            }
            while (sql.Contains("  "))
            {
                sql = sql.Replace("  ", " ");
            }
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            var pageSql = DapperExtensions.SqlDialect.GetPagingSql(sql, pageIndex, pageSize, parameters);

            DynamicParameters dynamicParameters = new DynamicParameters();
            if (param != null)
            {
                dynamicParameters = param as DynamicParameters;
            }
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(pageSql, param);
            }

            string allRowsCountSql = DapperExtensions.Instance.SqlGenerator.PageCount(sql);
            IEnumerable<T> list = Connection.Query<T>(pageSql, dynamicParameters, Transaction, true);
            allRowsCount = (int)Connection.Query(allRowsCountSql, dynamicParameters, Transaction, false).Single().Total;
            return list;
        }


        /// <summary>
        /// 执行sql语句 根据条件筛选数据集合（返回DataTable）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryDataTable(string sql, dynamic param = null)
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(sql, param);
            }

            var dReader = Connection.ExecuteReader(sql, param as object, Transaction);

            //datareader 转 datatable
            return DataReadToDataTable(dReader);
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
        public DataTable ExecuteQueryDataTable(string sql, int pageIndex, int pageSize, out int allRowsCount, dynamic param = null)
        {
            while (sql.Contains("\r\n"))
            {
                sql = sql.Replace("\r\n", " ");
            }
            while (sql.Contains("  "))
            {
                sql = sql.Replace("  ", " ");
            }
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var pageSql = DapperExtensions.SqlDialect.GetPagingSql(sql, pageIndex, pageSize, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (param != null)
            {
                dynamicParameters = param as DynamicParameters;
            }
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(pageSql, param);
            }

            string allRowsCountSql = DapperExtensions.Instance.SqlGenerator.PageCount(sql);
            var dReader = Connection.ExecuteReader(pageSql, dynamicParameters, Transaction);
            allRowsCount = (int)Connection.Query(allRowsCountSql, dynamicParameters, Transaction, false).Single().Total;

            //datareader 转 datatable
            return DataReadToDataTable(dReader);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(string procName, dynamic param = null)
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(procName, param);
            }

            return Connection.Execute(procName, param as object, Transaction, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 执行存储过程 根据条件筛选数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteProc<T>(string procName, dynamic param) where T : class
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(procName, param);
            }

            IEnumerable<T> list = Connection.Query<T>(procName, param as object, Transaction, false, null, CommandType.StoredProcedure);
            return list;
        }


        /// <summary>
        /// 执行SQL语句，返回查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, dynamic param = null)
        {
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(sql, param);
            }
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
            if (DapperExtensions.Configuration.IsEnableLogEvent)
            {
                DapperExtensions.Configuration.LogEventCompleted?.Invoke(sql, param);
            }
            return Connection.ExecuteScalar<T>(sql, param as object, Transaction);
        }



        /// <summary>
        /// datareader 转 datatable
        /// </summary>
        /// <param name="dReader"></param>
        /// <returns></returns>
        private DataTable DataReadToDataTable(IDataReader dReader)
        {
            lock (lockObj)
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
        }
        #endregion
    }
}