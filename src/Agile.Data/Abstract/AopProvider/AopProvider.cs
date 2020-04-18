using Agile.Data.Entities;
using Agile.Data.ExpressionsToSql;
using Agile.Data.Infrastructure;
using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Abstract
{
    public class AopProvider
    {
        private AopProvider() { }
        public AopProvider(AgileProvider context)
        {
            this.Context = context;
            this.Context.Ado.IsEnableLogEvent = true;
        }
        private AgileProvider Context { get; set; }
        public Action<DiffLogModel> OnDiffLogEvent { set { this.Context.CurrentConnectionConfig.AopEvents.OnDiffLogEvent = value; } }
        public Action<UtilExceptions> OnError { set { this.Context.CurrentConnectionConfig.AopEvents.OnError = value; } }
        public Action<string, AgileParameter[]> OnLogExecuting { set { this.Context.CurrentConnectionConfig.AopEvents.OnLogExecuting= value; } }
        public Action<string, AgileParameter[]> OnLogExecuted { set { this.Context.CurrentConnectionConfig.AopEvents.OnLogExecuted = value; } }
        public Func<string, AgileParameter[], KeyValuePair<string, AgileParameter[]>> OnExecutingChangeSql { set { this.Context.CurrentConnectionConfig.AopEvents.OnExecutingChangeSql = value; } }
    }
}
