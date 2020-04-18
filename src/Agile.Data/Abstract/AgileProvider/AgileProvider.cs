using Agile.Data.Entities;
using Agile.Data.Enums;
using Agile.Data.ExpressionsToSql;
using Agile.Data.Infrastructure;
using Agile.Data.Interface;
using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Agile.Data.Abstract
{
    ///<summary>
    /// ** description：Create datathis.access object
    /// </summary>
    public partial class AgileProvider: IAgileClient
    {

        #region Constructor
        public AgileProvider(ConnectionConfig config)
        {
            this.Context = this;
            this.CurrentConnectionConfig = config;
            this.ContextID = Guid.NewGuid();
            Check.ArgumentNullException(config, "config is null");
            CheckDbDependency(config);
        }
        #endregion

        #region  ADO Methods
        /// <summary>
        ///Datathis.operation
        /// </summary>
        public virtual IAdo Ado
        {
            get
            {
                if (this.ContextAdo == null)
                {
                    var result = InstanceFactory.GetAdo(this.Context.CurrentConnectionConfig);
                    this.ContextAdo = result;
                    result.Context = this;
                    return result;
                }
                return this._Ado;
            }
        }
        #endregion

        #region Aop Log Methods
        public virtual AopProvider Aop { get { return new AopProvider(this); } }
        #endregion

        #region Util Methods
        public virtual IContextMethods Utilities
        {
            get
            {
                if (ContextRewritableMethods == null)
                {
                    ContextRewritableMethods = new ContextMethods();
                    ContextRewritableMethods.Context = this;
                }
                return ContextRewritableMethods;
            }
            set { ContextRewritableMethods = value; }
        }
        #endregion

        #region Queryable
        /// <summary>
        /// Get datebase time
        /// </summary>
        /// <returns></returns>
        public DateTime GetDate()
        {
            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.Context.CurrentConnectionConfig);
            return this.Ado.GetDateTime(sqlBuilder.FullSqlDateNow);
        }
        /// <summary>
        /// Lambda Query operation
        /// </summary>
        public virtual Interface.IAgileQueryable<T> Queryable<T>()
        {

            InitMappingInfo<T>();
            var result = this.CreateQueryable<T>();
            return result;
        }
        /// <summary>
        /// Lambda Query operation
        /// </summary>
        public virtual Interface.IAgileQueryable<T> Queryable<T>(string shortName)
        {
            Check.Exception(shortName.HasValue() && shortName.Length > 20, ErrorMessage.GetThrowMessage("shortName参数长度不能超过20，你可能是想用这个方法 db.SqlQueryable(sql)而不是db.Queryable(shortName)", "Queryable.shortName max length 20"));
            var queryable = Queryable<T>();
            queryable.SqlBuilder.QueryBuilder.TableShortName = shortName;
            return queryable;
        }
        /// <summary>
        /// Lambda Query operation
        /// </summary>
        public virtual Interface.IAgileQueryable<ExpandoObject> Queryable(string tableName, string shortName)
        {
            var queryable = Queryable<ExpandoObject>();
            queryable.SqlBuilder.QueryBuilder.EntityName = tableName;
            queryable.SqlBuilder.QueryBuilder.TableShortName = shortName;
            return queryable;
        }
        public virtual IAgileQueryable<T, T2> Queryable<T, T2>(Expression<Func<T, T2, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2>();
            var types = new Type[] { typeof(T2) };
            var queryable = InstanceFactory.GetQueryable<T, T2>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2> Queryable<T, T2>(Expression<Func<T, T2, JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2>();
            var types = new Type[] { typeof(T2) };
            var queryable = InstanceFactory.GetQueryable<T, T2>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3> Queryable<T, T2, T3>(Expression<Func<T, T2, T3, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3>();
            var types = new Type[] { typeof(T2), typeof(T3) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3> Queryable<T, T2, T3>(Expression<Func<T, T2, T3,JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3>();
            var types = new Type[] { typeof(T2), typeof(T3) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4> Queryable<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4> Queryable<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4,JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5> Queryable<T, T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5> Queryable<T, T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5,JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6> Queryable<T, T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6> Queryable<T, T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7> Queryable<T, T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7> Queryable<T, T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8> Queryable<T, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8> Queryable<T, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        #region  9-12
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, object[]>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, JoinQueryInfos>> joinExpression)
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this.CurrentConnectionConfig);
            this.CreateQueryJoin(joinExpression, types, queryable);
            return queryable;
        }
        #endregion
        public virtual IAgileQueryable<T, T2> Queryable<T, T2>(Expression<Func<T, T2, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2>();
            var types = new Type[] { typeof(T2) };
            var queryable = InstanceFactory.GetQueryable<T, T2>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3> Queryable<T, T2, T3>(Expression<Func<T, T2, T3, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3>();
            var types = new Type[] { typeof(T2), typeof(T3) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4> Queryable<T, T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5> Queryable<T, T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6> Queryable<T, T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7> Queryable<T, T2, T3, T4, T5, T6, T7>(Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8> Queryable<T, T2, T3, T4, T5, T6, T7, T8>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }

        #region 9-12
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual IAgileQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Queryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>> joinExpression) where T : class, new()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
            var types = new Type[] { typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12) };
            var queryable = InstanceFactory.GetQueryable<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this.CurrentConnectionConfig);
            this.CreateEasyQueryJoin(joinExpression, types, queryable);
            queryable.Where(joinExpression);
            return queryable;
        }
        public virtual Interface.IAgileQueryable<T> Queryable<T>(Interface.IAgileQueryable<T> queryable) where T : class, new()
        {
            var sqlobj = queryable.ToSql();
            return this.SqlQueryable<T>(sqlobj.Key).AddParameters(sqlobj.Value);
        }
        public virtual IAgileQueryable<T, T2> Queryable<T, T2>(
     Interface.IAgileQueryable<T> joinQueryable1, Interface.IAgileQueryable<T2> joinQueryable2, Expression<Func<T, T2, bool>> joinExpression) where T : class, new() where T2 : class, new()
        {
            return Queryable(joinQueryable1, joinQueryable2, JoinType.Inner, joinExpression);
        }
        public virtual IAgileQueryable<T, T2> Queryable<T, T2>(
             Interface.IAgileQueryable<T> joinQueryable1, Interface.IAgileQueryable<T2> joinQueryable2, JoinType joinType, Expression<Func<T, T2, bool>> joinExpression) where T : class, new() where T2 : class, new()
        {
            Check.Exception(joinQueryable1.QueryBuilder.Take != null || joinQueryable1.QueryBuilder.Skip != null || joinQueryable1.QueryBuilder.OrderByValue.HasValue(), "joinQueryable1 Cannot have 'Skip' 'ToPageList' 'Take' Or 'OrderBy'");
            Check.Exception(joinQueryable2.QueryBuilder.Take != null || joinQueryable2.QueryBuilder.Skip != null || joinQueryable2.QueryBuilder.OrderByValue.HasValue(), "joinQueryable2 Cannot have 'Skip' 'ToPageList' 'Take' Or 'OrderBy'");

            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.Context.CurrentConnectionConfig);

            sqlBuilder.Context = this;
            InitMappingInfo<T, T2>();
            var types = new Type[] { typeof(T2) };
            var queryable = InstanceFactory.GetQueryable<T, T2>(this.CurrentConnectionConfig);
            queryable.Context = this.Context;
            queryable.SqlBuilder = sqlBuilder;
            queryable.QueryBuilder = InstanceFactory.GetQueryBuilder(this.CurrentConnectionConfig);
            queryable.QueryBuilder.JoinQueryInfos = new List<JoinQueryInfo>();
            queryable.QueryBuilder.Builder = sqlBuilder;
            queryable.QueryBuilder.Context = this;
            queryable.QueryBuilder.EntityType = typeof(T);
            queryable.QueryBuilder.LambdaExpressions = InstanceFactory.GetLambdaExpressions(this.CurrentConnectionConfig);

            //master
            var shortName1 = joinExpression.Parameters[0].Name;
            var sqlObj1 = joinQueryable1.ToSql();
            string sql1 = sqlObj1.Key;
            UtilMethods.RepairReplicationParameters(ref sql1, sqlObj1.Value.ToArray(), 0, "Join");
            queryable.QueryBuilder.EntityName = sqlBuilder.GetPackTable(sql1, shortName1); ;
            queryable.QueryBuilder.Parameters.AddRange(sqlObj1.Value);

            //join table 1
            var shortName2 = joinExpression.Parameters[1].Name;
            var sqlObj2 = joinQueryable2.ToSql();
            string sql2 = sqlObj2.Key;
            UtilMethods.RepairReplicationParameters(ref sql2, sqlObj2.Value.ToArray(), 1, "Join");
            queryable.QueryBuilder.Parameters.AddRange(sqlObj2.Value);
            var exp = queryable.QueryBuilder.GetExpressionValue(joinExpression, ResolveExpressType.WhereMultiple);
            queryable.QueryBuilder.JoinQueryInfos.Add(new JoinQueryInfo() { JoinIndex = 0, JoinType = joinType, JoinWhere = exp.GetResultString(), TableName = sqlBuilder.GetPackTable(sql2, shortName2) });

            return queryable;
        }
        #endregion

        public virtual Interface.IAgileQueryable<T> UnionAll<T>(params Interface.IAgileQueryable<T>[] queryables) where T : class, new()
        {
            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.Context.CurrentConnectionConfig);
            Check.Exception(queryables.IsNullOrEmpty(), "UnionAll.queryables is null ");
            int i = 1;
            List<KeyValuePair<string, List<AgileParameter>>> allItems = new List<KeyValuePair<string, List<AgileParameter>>>();
            foreach (var item in queryables)
            {
                var sqlObj = item.ToSql();
                string sql = sqlObj.Key;
                UtilMethods.RepairReplicationParameters(ref sql, sqlObj.Value.ToArray(), i, "UnionAll");
                if (sqlObj.Value.HasValue())
                    allItems.Add(new KeyValuePair<string, List<AgileParameter>>(sql, sqlObj.Value));
                else
                    allItems.Add(new KeyValuePair<string, List<AgileParameter>>(sql, new List<AgileParameter>()));
                i++;
            }
            var allSql = sqlBuilder.GetUnionAllSql(allItems.Select(it => it.Key).ToList());
            var allParameters = allItems.SelectMany(it => it.Value).ToArray();
            var resulut = this.Context.Queryable<ExpandoObject>().AS(UtilMethods.GetPackTable(allSql, "unionTable")).With(SqlWith.Null);
            resulut.AddParameters(allParameters);
            return resulut.Select<T>(sqlBuilder.SqlSelectAll);
        }
        public virtual Interface.IAgileQueryable<T> UnionAll<T>(List<Interface.IAgileQueryable<T>> queryables) where T : class, new()
        {
            Check.Exception(queryables.IsNullOrEmpty(), "UnionAll.queryables is null ");
            return UnionAll(queryables.ToArray());
        }
        public virtual Interface.IAgileQueryable<T> Union<T>(params Interface.IAgileQueryable<T>[] queryables) where T : class, new()
        {
            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.Context.CurrentConnectionConfig);
            Check.Exception(queryables.IsNullOrEmpty(), "UnionAll.queryables is null ");
            int i = 1;
            List<KeyValuePair<string, List<AgileParameter>>> allItems = new List<KeyValuePair<string, List<AgileParameter>>>();
            foreach (var item in queryables)
            {
                var sqlObj = item.ToSql();
                string sql = sqlObj.Key;
                UtilMethods.RepairReplicationParameters(ref sql, sqlObj.Value.ToArray(), i, "Union");
                if (sqlObj.Value.HasValue())
                    allItems.Add(new KeyValuePair<string, List<AgileParameter>>(sql, sqlObj.Value));
                else
                    allItems.Add(new KeyValuePair<string, List<AgileParameter>>(sql, new List<AgileParameter>()));
                i++;
            }
            var allSql = sqlBuilder.GetUnionSql(allItems.Select(it => it.Key).ToList());
            var allParameters = allItems.SelectMany(it => it.Value).ToArray();
            var resulut = this.Context.Queryable<ExpandoObject>().AS(UtilMethods.GetPackTable(allSql, "unionTable")).With(SqlWith.Null);
            resulut.AddParameters(allParameters);
            return resulut.Select<T>(sqlBuilder.SqlSelectAll);
        }
        public virtual Interface.IAgileQueryable<T> Union<T>(List<Interface.IAgileQueryable<T>> queryables) where T : class, new()
        {
            Check.Exception(queryables.IsNullOrEmpty(), "Union.queryables is null ");
            return Union(queryables.ToArray());
        }
        #endregion

        #region SqlQueryable
        public Interface.IAgileQueryable<T> SqlQueryable<T>(string sql) where T : class, new()
        {
            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.Context.CurrentConnectionConfig);
            return this.Context.Queryable<T>().AS(sqlBuilder.GetPackTable(sql, sqlBuilder.GetDefaultShortName())).With(SqlWith.Null).Select(sqlBuilder.GetDefaultShortName() + ".*");
        }
        #endregion

        #region Insertable
        public virtual IInsertable<T> Insertable<T>(T[] insertObjs) where T : class, new()
        {
            InitMappingInfo<T>();
            InsertableProvider<T> result = this.CreateInsertable(insertObjs);
            return result;
        }
        public virtual IInsertable<T> Insertable<T>(List<T> insertObjs) where T : class, new()
        {
            if (insertObjs == null|| insertObjs.IsNullOrEmpty())
            {
                insertObjs = new List<T>();
                insertObjs.Add(default(T));
            }
            return this.Context.Insertable(insertObjs.ToArray());
        }
        public virtual IInsertable<T> Insertable<T>(T insertObj) where T : class, new()
        {
            return this.Context.Insertable(new T[] { insertObj });
        }
        public virtual IInsertable<T> Insertable<T>(Dictionary<string, object> columnDictionary) where T : class, new()
        {
            InitMappingInfo<T>();
            Check.Exception(columnDictionary == null || columnDictionary.Count == 0, "Insertable.columnDictionary can't be null");
            var insertObject = this.Context.Utilities.DeserializeObject<T>(this.Context.Utilities.SerializeObject(columnDictionary));
            var columns = columnDictionary.Select(it => it.Key).ToList();
            return this.Context.Insertable(insertObject).InsertColumns(columns.ToArray()); ;
        }
        public virtual IInsertable<T> Insertable<T>(dynamic insertDynamicObject) where T : class, new()
        {
            InitMappingInfo<T>();
            if (insertDynamicObject is T)
            {
                return this.Context.Insertable((T)insertDynamicObject);
            }
            else
            {
                var columns = ((object)insertDynamicObject).GetType().GetProperties().Select(it => it.Name).ToList();
                Check.Exception(columns.IsNullOrEmpty(), "Insertable.updateDynamicObject can't be null");
                T insertObject = this.Context.Utilities.DeserializeObject<T>(this.Context.Utilities.SerializeObject(insertDynamicObject));
                return this.Context.Insertable(insertObject).InsertColumns(columns.ToArray());
            }
        }
        #endregion

        #region Deleteable
        public virtual IDeleteable<T> Deleteable<T>() where T : class, new()
        {
            InitMappingInfo<T>();
            DeleteableProvider<T> result = this.CreateDeleteable<T>();
            return result;
        }
        public virtual IDeleteable<T> Deleteable<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            InitMappingInfo<T>();
            return this.Context.Deleteable<T>().Where(expression);
        }
        public virtual IDeleteable<T> Deleteable<T>(dynamic primaryKeyValue) where T : class, new()
        {
            InitMappingInfo<T>();
            return this.Context.Deleteable<T>().In(primaryKeyValue);
        }
        public virtual IDeleteable<T> Deleteable<T>(dynamic[] primaryKeyValues) where T : class, new()
        {
            InitMappingInfo<T>();
            return this.Context.Deleteable<T>().In(primaryKeyValues);
        }
        public virtual IDeleteable<T> Deleteable<T>(List<dynamic> pkValue) where T : class, new()
        {
            InitMappingInfo<T>();
            return this.Context.Deleteable<T>().In(pkValue);
        }
        public virtual IDeleteable<T> Deleteable<T>(T deleteObj) where T : class, new()
        {
            InitMappingInfo<T>();
            return this.Context.Deleteable<T>().Where(deleteObj);
        }
        public virtual IDeleteable<T> Deleteable<T>(List<T> deleteObjs) where T : class, new()
        {
            InitMappingInfo<T>();
            return this.Context.Deleteable<T>().Where(deleteObjs);
        }
        #endregion

        #region Updateable
        public virtual IUpdateable<T> Updateable<T>(T[] UpdateObjs) where T : class, new()
        {
            InitMappingInfo<T>();
            UpdateableProvider<T> result = this.CreateUpdateable(UpdateObjs);
            return result;
        }
        public virtual IUpdateable<T> Updateable<T>(List<T> UpdateObjs) where T : class, new()
        {
            Check.ArgumentNullException(UpdateObjs, "Updateable.UpdateObjs can't be null");
            return Updateable(UpdateObjs.ToArray());
        }
        public virtual IUpdateable<T> Updateable<T>(T UpdateObj) where T : class, new()
        {
            return this.Context.Updateable(new T[] { UpdateObj });
        }
        public virtual IUpdateable<T> Updateable<T>() where T : class, new()
        {
            var result = this.Context.Updateable(new T[] { new T() });
            result.UpdateParameterIsNull = true;
            return result;
        }
        public virtual IUpdateable<T> Updateable<T>(Expression<Func<T, T>> columns) where T : class, new()
        {
            var result = this.Context.Updateable<T>().SetColumns(columns);
            result.UpdateParameterIsNull = true;
            return result;
        }
        public virtual IUpdateable<T> Updateable<T>(Expression<Func<T, bool>> columns) where T : class, new()
        {
            var result = this.Context.Updateable<T>().SetColumns(columns);
            result.UpdateParameterIsNull = true;
            return result;
        }
  

        public virtual IUpdateable<T> Updateable<T>(Dictionary<string, object> columnDictionary) where T : class, new()
        {
            InitMappingInfo<T>();
            Check.Exception(columnDictionary == null || columnDictionary.Count == 0, "Updateable.columnDictionary can't be null");
            var updateObject = this.Context.Utilities.DeserializeObject<T>(this.Context.Utilities.SerializeObject(columnDictionary));
            var columns = columnDictionary.Select(it => it.Key).ToList();
            return this.Context.Updateable(updateObject).UpdateColumns(columns.ToArray()); ;
        }
        public virtual IUpdateable<T> Updateable<T>(dynamic updateDynamicObject) where T : class, new()
        {
            InitMappingInfo<T>();
            if (updateDynamicObject is T)
            {
                return this.Context.Updateable((T)updateDynamicObject);
            }
            else
            {
                var columns = ((object)updateDynamicObject).GetType().GetProperties().Select(it => it.Name).ToList();
                Check.Exception(columns.IsNullOrEmpty(), "Updateable.updateDynamicObject can't be null");
                T updateObject = this.Context.Utilities.DeserializeObject<T>(this.Context.Utilities.SerializeObject(updateDynamicObject));
                return this.Context.Updateable(updateObject).UpdateColumns(columns.ToArray()); ;
            }
        }
        #endregion

        #region Saveable
        public ISaveable<T> Saveable<T>(List<T> saveObjects) where T : class, new()
        {
            return new SaveableProvider<T>(this, saveObjects);
        }
        public ISaveable<T> Saveable<T>(T saveObject) where T : class, new()
        {
            return new SaveableProvider<T>(this, saveObject);
        }
        #endregion

        #region DbFirst
        public virtual IDbFirst DbFirst
        {
            get
            {
                IDbFirst dbFirst = InstanceFactory.GetDbFirst(this.Context.CurrentConnectionConfig);
                dbFirst.Context = this.Context;
                dbFirst.Init();
                return dbFirst;
            }
        }
        #endregion

        #region CodeFirst
        public virtual ICodeFirst CodeFirst
        {
            get
            {
                ICodeFirst codeFirst = InstanceFactory.GetCodeFirst(this.Context.CurrentConnectionConfig);
                codeFirst.Context = this;
                return codeFirst;
            }
        }
        #endregion

        #region Db Maintenance
        public virtual IDbMaintenance DbMaintenance
        {
            get
            {
                if (this._DbMaintenance == null)
                {
                    IDbMaintenance maintenance = InstanceFactory.GetDbMaintenance(this.Context.CurrentConnectionConfig);
                    this._DbMaintenance = maintenance;
                    maintenance.Context = this;
                }
                return this._DbMaintenance;
            }
        }
        #endregion

        #region Entity Maintenance
        public virtual EntityMaintenanceProvider EntityProvider
        {
            get
            {
                if (this._EntityProvider == null)
                {
                    this._EntityProvider = new EntityMaintenanceProvider();
                    this._EntityProvider.Context = this;
                }
                return this._EntityProvider;
            }
            set { this._EntityProvider = value; }
        }
        #endregion

        #region Gobal Filter
        public virtual QueryFilterProvider QueryFilter
        {
            get
            {
                if (this._QueryFilterProvider == null)
                {
                    this._QueryFilterProvider = new QueryFilterProvider();
                    this._QueryFilterProvider.Context = this;
                }
                return this._QueryFilterProvider;
            }
            set { this._QueryFilterProvider = value; }
        }
        #endregion

        #region SimpleClient
        public virtual SimpleClient<T> GetSimpleClient<T>() where T : class, new()
        {
            return new SimpleClient<T>(this);
        }
        public virtual SimpleClient GetSimpleClient()
        {
            if (this._SimpleClient == null)
                this._SimpleClient = new SimpleClient(this);
            return this._SimpleClient;
        }
        #endregion

        #region Dispose OR Close
        public virtual void Close()
        {
            if (this.Context.Ado != null)
                this.Context.Ado.Close();
        }
        public virtual void Open()
        {
            if (this.Context.Ado != null)
                this.Context.Ado.Open();
        }
        public virtual void Dispose()
        {
            if (this.Context.Ado != null)
                this.Context.Ado.Dispose();
        }
        #endregion

        #region   Queue
        public int SaveQueues(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.ExecuteCommand(sql, parameters); });
        }

        public async Task<int> SaveQueuesAsync(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => { return  this.Ado.ExecuteCommandAsync(sql, parameters); });
        }
        public List<T> SaveQueues<T>(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.SqlQuery<T>(sql, parameters); });
        }
        public async Task<List<T>> SaveQueuesAsync<T>(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => {  return  this.Ado.SqlQueryAsync<T>(sql, parameters); });
        }
        public Tuple<List<T>, List<T2>> SaveQueues<T, T2>(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.SqlQuery<T, T2>(sql, parameters); });
        }
        public async Task<Tuple<List<T>, List<T2>>> SaveQueuesAsync<T, T2>(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => { return  this.Ado.SqlQueryAsync<T, T2>(sql, parameters); });
        }
        public Tuple<List<T>, List<T2>, List<T3>> SaveQueues<T, T2, T3>(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.SqlQuery<T, T2, T3>(sql, parameters); });
        }
        public async Task<Tuple<List<T>, List<T2>, List<T3>>> SaveQueuesAsync<T, T2, T3>(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => { return  this.Ado.SqlQueryAsync<T, T2, T3>(sql, parameters); });
        }
        public Tuple<List<T>, List<T2>, List<T3>, List<T4>> SaveQueues<T, T2, T3, T4>(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.SqlQuery<T, T2, T3, T4>(sql, parameters); });
        }
        public async Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>>> SaveQueuesAsync<T, T2, T3, T4>(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => { return  this.Ado.SqlQueryAsync<T, T2, T3, T4>(sql, parameters); });
        }
        public Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>> SaveQueues<T, T2, T3, T4, T5>(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.SqlQuery<T, T2, T3, T4, T5>(sql, parameters); });
        }
        public async Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>>> SaveQueuesAsync<T, T2, T3, T4, T5>(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => { return  this.Ado.SqlQueryAsync<T, T2, T3, T4, T5>(sql, parameters); });
        }
        public Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> SaveQueues<T, T2, T3, T4, T5, T6>(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.SqlQuery<T, T2, T3, T4, T5, T6>(sql, parameters); });
        }
        public async Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>>> SaveQueuesAsync<T, T2, T3, T4, T5, T6>(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => { return  this.Ado.SqlQueryAsync<T, T2, T3, T4, T5, T6>(sql, parameters); });
        }
        public Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> SaveQueues<T, T2, T3, T4, T5, T6, T7>(bool isTran = true)
        {
            return SaveQueuesProvider(isTran, (sql, parameters) => { return this.Ado.SqlQuery<T, T2, T3, T4, T5, T6, T7>(sql, parameters); });
        }
        public async Task<Tuple<List<T>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>>> SaveQueuesAsync<T, T2, T3, T4, T5, T6, T7>(bool isTran = true)
        {
            return await SaveQueuesProviderAsync(isTran, (sql, parameters) => { return  this.Ado.SqlQueryAsync<T, T2, T3, T4, T5, T6, T7>(sql, parameters); });
        }
        public void AddQueue(string sql, object parsmeters=null)
        {
            if (Queues == null)
            {
                Queues = new QueueList();
            }
            this.Queues.Add(sql,this.Context.Ado.GetParameters(parsmeters));
        }
        public void AddQueue(string sql, AgileParameter  parsmeter)
        {
            if (Queues == null)
            {
                Queues = new QueueList();
            }
            this.Queues.Add(sql, new List<AgileParameter>() { parsmeter });
        }
        public void AddQueue(string sql, List<AgileParameter> parsmeters)
        {
            if (Queues == null)
            {
                Queues = new QueueList();
            }
            this.Queues.Add(sql, parsmeters);
        }
        public QueueList Queues { get { if (_Queues == null) { _Queues = new QueueList(); } return _Queues; }  set { _Queues = value; } }



        private async Task<T> SaveQueuesProviderAsync<T>(bool isTran, Func<string, List<AgileParameter>, Task<T>> func)
        {
            try
            {
                if (this.CurrentConnectionConfig.DbType == DatabaseType.Oracle)
                {
                    throw new Exception("Oracle no support SaveQueues");
                }
                if (this.Queues == null || this.Queues.Count == 0) return default(T);
                isTran = isTran && this.Ado.Transaction == null;
                if (isTran) this.Ado.BeginTran();
                StringBuilder sqlBuilder = new StringBuilder();
                var parsmeters = new List<AgileParameter>();
                var index = 1;
                if (this.Queues.HasValue())
                {
                    foreach (var item in Queues)
                    {
                        if (item.Sql == null)
                            item.Sql = string.Empty;
                        if (item.Parameters == null)
                            item.Parameters = new AgileParameter[] { };
                        var itemParsmeters = item.Parameters.OrderByDescending(it => it.ParameterName.Length).ToList();
                        List<AgileParameter> addParameters = new List<AgileParameter>();
                        var itemSql = item.Sql;
                        foreach (var itemParameter in itemParsmeters)
                        {
                            var newName = itemParameter.ParameterName + "_q_" + index;
                            AgileParameter parameter = new AgileParameter(newName, itemParameter.Value);
                            parameter.DbType = itemParameter.DbType;
                            itemSql = UtilMethods.ReplaceSqlParameter(itemSql, itemParameter, newName);
                            addParameters.Add(parameter);
                        }
                        parsmeters.AddRange(addParameters);
                        itemSql = itemSql.TrimEnd(';') + ";";
                        sqlBuilder.AppendLine(itemSql);
                        index++;
                    }
                }
                this.Queues.Clear();
                var result =await func(sqlBuilder.ToString(), parsmeters);
                if (isTran) this.Ado.CommitTran();
                return result;
            }
            catch (Exception ex)
            {
                if (isTran) this.Ado.RollbackTran();
                throw ex;
            }
        }
        private T SaveQueuesProvider<T>(bool isTran, Func<string, List<AgileParameter>, T> func)
        {
            try
            {
                if (this.CurrentConnectionConfig.DbType == DatabaseType.Oracle) {
                    throw new Exception("Oracle no support SaveQueues");
                }
                if (this.Queues == null || this.Queues.Count == 0) return default(T);
                isTran = isTran && this.Ado.Transaction == null;
                if (isTran) this.Ado.BeginTran();
                StringBuilder sqlBuilder = new StringBuilder();
                var parsmeters = new List<AgileParameter>();
                var index = 1;
                if (this.Queues.HasValue())
                {
                    foreach (var item in Queues)
                    {
                        if (item.Sql == null)
                            item.Sql = string.Empty;
                        if (item.Parameters == null)
                            item.Parameters = new AgileParameter[] { };
                        var itemParsmeters = item.Parameters.OrderByDescending(it => it.ParameterName.Length).ToList();
                        List<AgileParameter> addParameters = new List<AgileParameter>();
                        var itemSql = item.Sql;
                        foreach (var itemParameter in itemParsmeters)
                        {
                            var newName = itemParameter.ParameterName + "_q_" + index;
                            AgileParameter parameter = new AgileParameter(newName, itemParameter.Value);
                            parameter.DbType = itemParameter.DbType;
                            itemSql = UtilMethods.ReplaceSqlParameter(itemSql, itemParameter, newName);
                            addParameters.Add(parameter);
                        }
                        parsmeters.AddRange(addParameters);
                        itemSql = itemSql.TrimEnd(';')+";";
                        sqlBuilder.AppendLine(itemSql);
                        index++;
                    }
                }
                this.Queues.Clear();
                var result = func(sqlBuilder.ToString(), parsmeters);
                if (isTran) this.Ado.CommitTran();
                return result;
            }
            catch (Exception ex)
            {
                if (isTran) this.Ado.RollbackTran();
                throw ex;
            }
        }

        #endregion
    }


    public partial class AgileProvider
    {
        #region Properties
        public AgileProvider Context
        {
            get
            {
                _Context = this;
                return _Context;
            }
            set
            {
                _Context = value;
            }
        }
        public AgileClient Root { get; set; }
        public ConnectionConfig CurrentConnectionConfig { get; set; }
        public Dictionary<string, object> TempItems { get { if (_TempItems == null) { _TempItems = new Dictionary<string, object>(); } return _TempItems; } set { _TempItems = value; } }
        public bool IsSystemTablesConfig { get { return this.CurrentConnectionConfig.InitKeyType == InitKeyType.SystemTable; } }
        public Guid ContextID { get; set; }
        public MappingTableList MappingTables { get; set; }
        public MappingColumnList MappingColumns { get; set; }
        public IgnoreColumnList IgnoreColumns { get; set; }
        public IgnoreColumnList IgnoreInsertColumns { get; set; }


        #endregion

        #region Fields       
        public Dictionary<string, object> _TempItems;
        public QueueList _Queues;
        protected ISqlBuilder _SqlBuilder;
        protected AgileProvider _Context { get; set; }
        protected EntityMaintenanceProvider _EntityProvider;
        protected IAdo _Ado;
        protected ILambdaExpressions _LambdaExpressions;
        protected IContextMethods _RewritableMethods;
        protected IDbMaintenance _DbMaintenance;
        protected QueryFilterProvider _QueryFilterProvider;
        protected SimpleClient _SimpleClient;
        protected IAdo ContextAdo
        {
            get
            {
                return this._Ado;
            }
            set
            {
                this._Ado = value;
            }
        }
        protected IContextMethods ContextRewritableMethods
        {
            get
            {
                return this._RewritableMethods;
            }
            set
            {
                this._RewritableMethods = value;
            }
        }
        #endregion

        #region Init mappingInfo
        protected void InitMappingInfo<T, T2>()
        {
            InitMappingInfo<T>();
            InitMappingInfo<T2>();
        }
        protected void InitMappingInfo<T, T2, T3>()
        {
            InitMappingInfo<T, T2>();
            InitMappingInfo<T3>();
        }
        protected void InitMappingInfo<T, T2, T3, T4>()
        {
            InitMappingInfo<T, T2, T3>();
            InitMappingInfo<T4>();
        }
        protected void InitMappingInfo<T, T2, T3, T4, T5>()
        {
            InitMappingInfo<T, T2, T3, T4>();
            InitMappingInfo<T5>();
        }
        protected void InitMappingInfo<T, T2, T3, T4, T5, T6>()
        {
            InitMappingInfo<T, T2, T3, T4, T5>();
            InitMappingInfo<T6>();
        }
        protected void InitMappingInfo<T, T2, T3, T4, T5, T6, T7>()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6>();
            InitMappingInfo<T7>();
        }
        protected void InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8>()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7>();
            InitMappingInfo<T8>();
        }

        #region 9-12
        protected void InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8>();
            InitMappingInfo<T9>();
        }
        protected void InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9>();
            InitMappingInfo<T10>();
        }
        protected void InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
            InitMappingInfo<T11>();
        }
        protected void InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
        {
            InitMappingInfo<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
            InitMappingInfo<T12>();
        }
        #endregion

        public void InitMappingInfo<T>()
        {
            InitMappingInfo(typeof(T));
        }
        public void InitMappingInfo(Type type)
        {
            string cacheKey = "Context.InitAttributeMappingTables" + type.FullName;
            var entityInfo = this.Context.Utilities.GetReflectionInoCacheInstance().GetOrCreate<EntityInfo>(cacheKey,
(Func<EntityInfo>)(() =>
{
    var result = this.Context.EntityProvider.GetEntityInfo(type);
    return (EntityInfo)result;
}));
            var copyObj = CopyEntityInfo(entityInfo);
            InitMappingInfo(copyObj);
        }

        private EntityInfo CopyEntityInfo(EntityInfo entityInfo)
        {
            EntityInfo result = new EntityInfo()
            {
                DbTableName = entityInfo.DbTableName,
                EntityName = entityInfo.EntityName,
                Type = entityInfo.Type
            };
            List<EntityColumnInfo> columns = new List<EntityColumnInfo>();
            if (entityInfo.Columns.HasValue())
            {
                foreach (var item in entityInfo.Columns)
                {
                    EntityColumnInfo column = new EntityColumnInfo()
                    {
                        ColumnDescription = item.ColumnDescription,
                        DataType = item.DataType,
                        DbColumnName = item.DbColumnName,
                        DbTableName = item.DbTableName,
                        DecimalDigits = item.DecimalDigits,
                        DefaultValue = item.DefaultValue,
                        EntityName = item.EntityName,
                        IsIdentity = item.IsIdentity,
                        IsIgnore = item.IsIgnore,
                        IsNullable = item.IsNullable,
                        IsOnlyIgnoreInsert = item.IsOnlyIgnoreInsert,
                        IsPrimarykey = item.IsPrimarykey,
                        Length = item.Length,
                        OldDbColumnName = item.OldDbColumnName,
                        OracleSequenceName = item.OracleSequenceName,
                        PropertyInfo = item.PropertyInfo,
                        PropertyName = item.PropertyName
                    };
                    columns.Add(item);
                }
            }
            result.Columns = columns;
            return result;
        }

        private void InitMappingInfo(EntityInfo entityInfo)
        {
            if (this.MappingTables == null)
                this.MappingTables = new MappingTableList();
            if (this.MappingColumns == null)
                this.MappingColumns = new MappingColumnList();
            if (this.IgnoreColumns == null)
                this.IgnoreColumns = new IgnoreColumnList();
            if (this.IgnoreInsertColumns == null)
                this.IgnoreInsertColumns = new IgnoreColumnList();
            if (!this.MappingTables.Any(it => it.EntityName == entityInfo.EntityName))
            {
                if (entityInfo.DbTableName != entityInfo.EntityName && entityInfo.DbTableName.HasValue())
                {
                    this.MappingTables.Add(entityInfo.EntityName, entityInfo.DbTableName);
                }
            }
            if (entityInfo.Columns.Any(it => it.EntityName == entityInfo.EntityName))
            {
                var mappingColumnInfos = this.MappingColumns.Where(it => it.EntityName == entityInfo.EntityName);
                foreach (var item in entityInfo.Columns.Where(it => it.IsIgnore == false))
                {
                    if (!mappingColumnInfos.Any(it => it.PropertyName == item.PropertyName))
                        if (item.PropertyName != item.DbColumnName && item.DbColumnName.HasValue())
                            this.MappingColumns.Add(item.PropertyName, item.DbColumnName, item.EntityName);
                }
                var ignoreInfos = this.IgnoreColumns.Where(it => it.EntityName == entityInfo.EntityName);
                foreach (var item in entityInfo.Columns.Where(it => it.IsIgnore))
                {
                    if (!ignoreInfos.Any(it => it.PropertyName == item.PropertyName))
                        this.IgnoreColumns.Add(item.PropertyName, item.EntityName);
                }

                var ignoreInsertInfos = this.IgnoreInsertColumns.Where(it => it.EntityName == entityInfo.EntityName);
                foreach (var item in entityInfo.Columns.Where(it => it.IsOnlyIgnoreInsert))
                {
                    if (!ignoreInsertInfos.Any(it => it.PropertyName == item.PropertyName))
                        this.IgnoreInsertColumns.Add(item.PropertyName, item.EntityName);
                }
            }
        }
        #endregion

        #region Create Instance
        protected Interface.IAgileQueryable<T> CreateQueryable<T>()
        {
            Interface.IAgileQueryable<T> result = InstanceFactory.GetQueryable<T>(this.CurrentConnectionConfig);
            return CreateQueryable(result);
        }
        protected Interface.IAgileQueryable<T> CreateQueryable<T>(Interface.IAgileQueryable<T> result)
        {
            Check.Exception(typeof(T).IsClass() == false || typeof(T).GetConstructors().Length == 0, "Queryable<{0}> Error ,{0} is invalid , need is a class,and can new().", typeof(T).Name);
            var sqlBuilder = InstanceFactory.GetSqlbuilder(CurrentConnectionConfig);
            result.Context = this.Context;
            result.SqlBuilder = sqlBuilder;
            result.SqlBuilder.QueryBuilder = InstanceFactory.GetQueryBuilder(CurrentConnectionConfig);
            result.SqlBuilder.QueryBuilder.Builder = sqlBuilder;
            result.SqlBuilder.Context = result.SqlBuilder.QueryBuilder.Context = this;
            result.SqlBuilder.QueryBuilder.EntityType = typeof(T);
            result.SqlBuilder.QueryBuilder.EntityName = typeof(T).Name;
            result.SqlBuilder.QueryBuilder.LambdaExpressions = InstanceFactory.GetLambdaExpressions(CurrentConnectionConfig);
            return result;
        }
        protected InsertableProvider<T> CreateInsertable<T>(T[] insertObjs) where T : class, new()
        {
            var result = InstanceFactory.GetInsertableProvider<T>(this.CurrentConnectionConfig);
            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.CurrentConnectionConfig); ;
            result.Context = this;
            result.EntityInfo = this.Context.EntityProvider.GetEntityInfo<T>();
            result.SqlBuilder = sqlBuilder;
            result.InsertObjs = insertObjs;
            sqlBuilder.InsertBuilder = result.InsertBuilder = InstanceFactory.GetInsertBuilder(this.CurrentConnectionConfig);
            sqlBuilder.InsertBuilder.Builder = sqlBuilder;
            sqlBuilder.InsertBuilder.LambdaExpressions = InstanceFactory.GetLambdaExpressions(this.CurrentConnectionConfig);
            sqlBuilder.Context = result.SqlBuilder.InsertBuilder.Context = this;
            result.Init();
            return result;
        }
        protected DeleteableProvider<T> CreateDeleteable<T>() where T : class, new()
        {
            var result = InstanceFactory.GetDeleteableProvider<T>(this.CurrentConnectionConfig);
            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.CurrentConnectionConfig); ;
            result.Context = this;
            result.SqlBuilder = sqlBuilder;
            sqlBuilder.DeleteBuilder = result.DeleteBuilder = InstanceFactory.GetDeleteBuilder(this.CurrentConnectionConfig);
            sqlBuilder.DeleteBuilder.Builder = sqlBuilder;
            sqlBuilder.DeleteBuilder.LambdaExpressions = InstanceFactory.GetLambdaExpressions(this.CurrentConnectionConfig);
            sqlBuilder.Context = result.SqlBuilder.DeleteBuilder.Context = this;
            return result;
        }
        protected UpdateableProvider<T> CreateUpdateable<T>(T[] UpdateObjs) where T : class, new()
        {
            var result = InstanceFactory.GetUpdateableProvider<T>(this.CurrentConnectionConfig);
            var sqlBuilder = InstanceFactory.GetSqlbuilder(this.CurrentConnectionConfig); ;
            result.Context = this;
            result.EntityInfo = this.Context.EntityProvider.GetEntityInfo<T>();
            result.SqlBuilder = sqlBuilder;
            result.UpdateObjs = UpdateObjs;
            sqlBuilder.UpdateBuilder = result.UpdateBuilder = InstanceFactory.GetUpdateBuilder(this.CurrentConnectionConfig);
            sqlBuilder.UpdateBuilder.Builder = sqlBuilder;
            sqlBuilder.UpdateBuilder.LambdaExpressions = InstanceFactory.GetLambdaExpressions(this.CurrentConnectionConfig);
            sqlBuilder.Context = result.SqlBuilder.UpdateBuilder.Context = this;
            result.Init();
            var ignoreColumns = result.EntityInfo.Columns.Where(it => it.IsOnlyIgnoreUpdate).ToList();
            if (ignoreColumns != null && ignoreColumns.Any())
            {
                result = (UpdateableProvider<T>)result.IgnoreColumns(ignoreColumns.Select(it => it.PropertyName).ToArray());
            }
            return result;
        }

        protected void CreateQueryJoin<T>(Expression joinExpression, Type[] types, Interface.IAgileQueryable<T> queryable)
        {
            this.CreateQueryable<T>(queryable);
            string shortName = string.Empty;
            List<AgileParameter> paramters = new List<AgileParameter>();
            queryable.SqlBuilder.QueryBuilder.JoinQueryInfos = this.GetJoinInfos(queryable.SqlBuilder, joinExpression, ref paramters, ref shortName, types);
            queryable.SqlBuilder.QueryBuilder.TableShortName = shortName;
            queryable.SqlBuilder.QueryBuilder.JoinExpression = joinExpression;
            if (paramters != null)
            {
                queryable.SqlBuilder.QueryBuilder.Parameters.AddRange(paramters);
            }
        }
        protected void CreateEasyQueryJoin<T>(Expression joinExpression, Type[] types, Interface.IAgileQueryable<T> queryable)
        {
            this.CreateQueryable<T>(queryable);
            string shortName = string.Empty;
            queryable.SqlBuilder.QueryBuilder.EasyJoinInfos = this.GetEasyJoinInfo(joinExpression, ref shortName, queryable.SqlBuilder, types);
            queryable.SqlBuilder.QueryBuilder.TableShortName = shortName;
            queryable.SqlBuilder.QueryBuilder.JoinExpression = joinExpression;
        }
        #endregion

        #region Private methods
        private static void CheckDbDependency(ConnectionConfig config)
        {
            switch (config.DbType)
            {
                case DatabaseType.MySql:
                    DependencyManagement.TryMySqlData();
                    break;
                case DatabaseType.SqlServer:
                    DependencyManagement.TrySqlServer();
                    break;
                case DatabaseType.Sqlite:
                    DependencyManagement.TrySqlite();
                    break;
                case DatabaseType.Oracle:
                    DependencyManagement.TryOracle();
                    break;
                case DatabaseType.PostgreSQL:
                    DependencyManagement.TryPostgreSQL();
                    break;
                default:
                    throw new Exception("ConnectionConfig.DbType is null");
            }
        }
        protected List<JoinQueryInfo> GetJoinInfos(ISqlBuilder sqlBuilder, Expression joinExpression, ref List<AgileParameter> parameters, ref string shortName, params Type[] entityTypeArray)
        {
            List<JoinQueryInfo> result = new List<JoinQueryInfo>();
            var lambdaParameters = ((LambdaExpression)joinExpression).Parameters.ToList();
            ILambdaExpressions expressionContext = sqlBuilder.QueryBuilder.LambdaExpressions;
            expressionContext.MappingColumns = this.MappingColumns;
            expressionContext.MappingTables = this.MappingTables;
            if (this.Context.CurrentConnectionConfig.MoreSettings != null)
            {
                expressionContext.PgSqlIsAutoToLower = this.Context.CurrentConnectionConfig.MoreSettings.PgSqlIsAutoToLower;
            }
            else
            {
                expressionContext.PgSqlIsAutoToLower = true;
            }
            if (this.Context.CurrentConnectionConfig.ConfigureExternalServices != null)
                expressionContext.SqlFuncServices = this.Context.CurrentConnectionConfig.ConfigureExternalServices.SqlFuncServices;
            expressionContext.Resolve(joinExpression, ResolveExpressType.Join);
            int i = 0;
            var joinArray = MergeJoinArray(expressionContext.Result.GetResultArray());
            if (joinArray == null) return null;
            parameters = expressionContext.Parameters;
            foreach (var entityType in entityTypeArray)
            {
                var isFirst = i == 0; ++i;
                JoinQueryInfo joinInfo = new JoinQueryInfo();
                var hasMappingTable = expressionContext.MappingTables.HasValue();
                MappingTable mappingInfo = null;
                if (hasMappingTable)
                {
                    mappingInfo = expressionContext.MappingTables.FirstOrDefault(it => it.EntityName.Equals(entityType.Name, StringComparison.CurrentCultureIgnoreCase));
                    joinInfo.TableName = mappingInfo != null ? mappingInfo.DbTableName : entityType.Name;
                }
                else
                {
                    joinInfo.TableName = entityType.Name;
                }
                if (isFirst)
                {
                    var firstItem = lambdaParameters.First();
                    lambdaParameters.Remove(firstItem);
                    shortName = firstItem.Name;
                }
                var joinString = joinArray[i * 2 - 2];
                joinInfo.ShortName = lambdaParameters[i - 1].Name;
                joinInfo.JoinType = (JoinType)Enum.Parse(typeof(JoinType), joinString);
                joinInfo.JoinWhere = joinArray[i * 2 - 1];
                joinInfo.JoinIndex = i;
                result.Add((joinInfo));
            }
            expressionContext.Clear();
            return result;
        }

        private string[] MergeJoinArray(string[] joinArray)
        {
            List<string> result = new List<string>();
            string joinValue = null;
            int i = 0;
            if (joinArray == null) return null;
            foreach (var item in joinArray)
            {
                ++i;
                var isLast = joinArray.Length == i;
                var isJoinType = item.IsIn(JoinType.Inner.ToString(), JoinType.Left.ToString(), JoinType.Right.ToString());
                if (isJoinType)
                {
                    if (joinValue != null)
                        result.Add(joinValue);
                    joinValue = null;
                    result.Add(item);
                }
                else
                {
                    isJoinType = false;
                    joinValue += joinValue == null ? item : ("," + item);
                }
                if (isLast)
                {
                    result.Add(joinValue);
                }
            }
            return result.ToArray(); ;
        }

        protected Dictionary<string, string> GetEasyJoinInfo(Expression joinExpression, ref string shortName, ISqlBuilder builder, params Type[] entityTypeArray)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var lambdaParameters = ((LambdaExpression)joinExpression).Parameters.ToList();
            shortName = lambdaParameters.First().Name;
            var index = 1;
            foreach (var item in entityTypeArray)
            {
                result.Add(UtilConstants.Space + lambdaParameters[index].Name, item.Name);
                ++index;
            }
            return result;
        }
        #endregion
    }
}
