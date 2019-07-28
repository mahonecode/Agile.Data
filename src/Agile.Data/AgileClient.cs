using Agile.Data.Extensions;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Agile.Data
{
    public class AgileClient
    {
        private ConnectionConfig CurrentConnectionConfig { get; set; }

        public AgileClient(ConnectionConfig config)
        {
            this.CurrentConnectionConfig = config;
        }

        /// <summary>
        /// 通用数据库访问类实例
        /// </summary>
        public IDbSession DBSession
        {
            get
            {
                return this.CreateSession();
            }
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateDbConnection()
        {
            IDbConnection conn;
            switch (this.CurrentConnectionConfig.DbType)
            {
                case DatabaseType.MySql:
                    DapperExtensions.SqlDialect = new MySqlDialect();
                    DapperExtensions.Configuration.ConnConfig = CurrentConnectionConfig;
                    conn = new MySqlConnection(this.CurrentConnectionConfig.ConnectionString);
                    break;
                case DatabaseType.SqlServer:
                    DapperExtensions.SqlDialect = new SqlServerDialect();
                    DapperExtensions.Configuration.ConnConfig = CurrentConnectionConfig;
                    conn = new SqlConnection(this.CurrentConnectionConfig.ConnectionString);
                    break;
                case DatabaseType.Oracle:
                    DapperExtensions.SqlDialect = new OracleDialect();
                    DapperExtensions.Configuration.ConnConfig = CurrentConnectionConfig;
                    conn = new OracleConnection(this.CurrentConnectionConfig.ConnectionString);
                    break;
                case DatabaseType.Sqlite:
                    throw new Exception("Sqlite 暂不支持");
                case DatabaseType.PostgreSQL:
                    throw new Exception("PostgreSQL 暂不支持");
                default:
                    throw new Exception("ConnectionConfig.DbType is null");
            }

            if (conn == null)
            {
                throw new Exception("数据库连接创建失败");
            }

            //dapper手动打开的话会保持长连接，dapper底层不关闭，需要手动关闭
            //否则每次操作之后dapper底层会自动关闭连接
            if (conn.State == ConnectionState.Closed)
            {
                if (!this.CurrentConnectionConfig.IsAutoCloseConnection)
                {
                    conn.Open();
                }
            }

            return conn;
        }


        /// <summary>
        /// 创建数据库连接会话
        /// </summary>
        /// <returns></returns>
        public IDbSession CreateSession()
        {
            IDbSession session = new DbSession(this.CreateDbConnection(), CurrentConnectionConfig);
            return session;
        }

        /// <summary>
        /// 创建数据库事务会话
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public IDbSession CreateSession(IDbConnection conn, IDbTransaction trans)
        {
            IDbSession session = new DbSession(conn, trans, CurrentConnectionConfig);
            return session;
        }


        #region Transaction 数据库事务操作封装
        /// <summary>
        /// 事务操作无返回值
        /// </summary>
        /// <param name="action"></param>
        public void RunInTransaction(Action<IDbSession> action)
        {
            IDbSession session = this.CreateSession();
            try
            {
                session.BeginTrans();

                //执行业务操作
                action(session);

                if (session.Transaction != null)
                {
                    session.Commit();
                }
            }
            catch (Exception ex)
            {
                if (session.Transaction != null)
                {
                    session.Rollback();
                }

                throw ex;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 事务操作有返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T RunInTransaction<T>(Func<IDbSession, T> func)
        {
            IDbSession session = this.CreateSession();
            try
            {
                session.BeginTrans();

                //执行业务操作
                T result = func(session);

                if (session.Transaction != null)
                {
                    session.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                if (session.Transaction != null)
                {
                    session.Rollback();
                }
                throw ex;
            }
            finally
            {
                session.Dispose();
            }
        }
        #endregion
    }
}
