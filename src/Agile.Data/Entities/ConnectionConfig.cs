﻿using Agile.Data.Enums;
using Agile.Data.ExpressionsToSql;
using Agile.Data.ExternalServiceInterface;
using Agile.Data.Infrastructure;
using Agile.Data.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Data.Entities
{
    public class ConnectionConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public dynamic ConfigId { get; set; }
        /// <summary>
        ///DbType.SqlServer Or Other
        /// </summary>
        public DatabaseType DbType { get; set; }
        /// <summary>
        ///Database Connection string
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// true does not need to close the connection
        /// </summary>
        public bool IsAutoCloseConnection { get; set; }
        /// <summary>
        /// Default SystemTable,If you do not have system table permissions, use attribute
        /// </summary>
        public InitKeyType InitKeyType = InitKeyType.SystemTable;
        /// <summary>
        ///If true, there is only one connection instance in the same thread within the same connection string
        /// </summary>
        public bool IsShardSameThread { get; set; }
        /// <summary>
        /// Configure External Services replace default services,For example, Redis storage
        /// </summary>
        [JsonIgnore]
        public ConfigureExternalServices ConfigureExternalServices = new ConfigureExternalServices();
        /// <summary>
        /// If SlaveConnectionStrings has value,ConnectionString is write operation, SlaveConnectionStrings is read operation.
        /// All operations within a transaction is ConnectionString
        /// </summary>
        public List<SlaveConnectionConfig> SlaveConnectionConfigs { get; set; }
        /// <summary>
        /// More Gobal Settings
        /// </summary>
        public ConnMoreSettings MoreSettings { get; set; }
        /// <summary>
        /// Used for debugging errors or BUG,Used for debugging, which has an impact on Performance
        /// </summary>
        public AgileDebugger Debugger { get; set; }

        [JsonIgnore]
        public AopEvents AopEvents { get;set; }
    }
    public class AopEvents
    {
        public Action<DiffLogModel> OnDiffLogEvent { get; set; }
        public Action<UtilExceptions> OnError { get; set; }
        public Action<string, AgileParameter[]> OnLogExecuting { get; set; }
        public Action<string, AgileParameter[]> OnLogExecuted { get; set; }
        public Func<string, AgileParameter[], KeyValuePair<string, AgileParameter[]>> OnExecutingChangeSql { get; set; }
    }
    public class ConfigureExternalServices
    {

        private ISerializeService _SerializeService;
        private ICacheService _ReflectionInoCache;
        private ICacheService _DataInfoCache;
        private IRazorService _RazorService;

        public IRazorService RazorService
        {
            get
            {
                if (_RazorService == null)
                    return _RazorService;
                else
                    return _RazorService;
            }
            set { _RazorService = value; }
        }

        public ISerializeService SerializeService
        {
            get
            {
                if (_SerializeService == null)
                    return DefaultServices.Serialize;
                else
                    return _SerializeService;
            }
            set{ _SerializeService = value;}
        }

        public ICacheService ReflectionInoCacheService
        {
            get
            {
                if (_ReflectionInoCache == null)
                    return DefaultServices.ReflectionInoCache;
                else
                    return _ReflectionInoCache;
            }
            set{_ReflectionInoCache = value;}
        }

        public ICacheService DataInfoCacheService
        {
            get
            {
                if (_DataInfoCache == null)
                    return DefaultServices.DataInoCache;
                else
                    return _DataInfoCache;
            }
            set { _DataInfoCache = value; }
        }

        public List<SqlFuncExternal> SqlFuncServices { get; set; }
        public List<KeyValuePair<string, CSharpDataType>> AppendDataReaderTypeMappings { get;  set; }


        public Action<PropertyInfo, EntityColumnInfo> EntityService{ get; set; }
        public Action<Type,EntityInfo> EntityNameService { get; set; }
    }
}