﻿using Agile.Data.Oracle.Extensions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Agile.Data.Oracle
{
    public class AgileClient
    {
        public ConnectionConfig CurrentConnectionConfig { get; set; }

        #region Constructor
        public AgileClient(ConnectionConfig config)
        {
            this.CurrentConnectionConfig = config;
            DapperExtensions.SqlDialect = new OracleDialect();
        }
        #endregion


        /// <summary>
        /// 通用数据库访问类实例
        /// </summary>
        public IDbSession DBSession
        {
            get
            {
                return CreateSession();
            }
        }


        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateDbConnection()
        {
            IDbConnection conn = new OracleConnection(CurrentConnectionConfig.ConnectionString);
            if (conn == null)
            {
                throw new Exception("数据库连接创建失败");
            }

            //dapper手动打开的话会保持长连接，dapper底层不关闭
            //否则每次操作之后dapper底层会自动关闭连接
            if (conn.State == ConnectionState.Closed)
            {
                if (!CurrentConnectionConfig.IsAutoCloseConnection)
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
            IDbConnection conn = CreateDbConnection();
            IDbSession session = new DbSession(conn);
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
            IDbSession session = new DbSession(conn, trans);
            return session;
        }


        #region Transaction
        public void RunInTransaction(Action<IDbSession> action)
        {
            IDbSession session = CreateSession();
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
            catch (System.Exception ex)
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

        public T RunInTransaction<T>(Func<IDbSession, T> func)
        {
            IDbSession session = CreateSession();
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
            catch (System.Exception ex)
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


        public bool IsEnableLogEvent
        {
            get
            {
                return DapperExtensions.Configuration.IsEnableLogEvent;
            }
            set
            {
                DapperExtensions.Configuration.IsEnableLogEvent = value;
            }
        }

        public Action<string, object> OnLogExecuted { set { DapperExtensions.Configuration.LogEventCompleted = value; } }
    }
}