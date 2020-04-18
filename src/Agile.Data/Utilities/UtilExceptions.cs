using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Agile.Data.Abstract;
using Newtonsoft.Json;

namespace Agile.Data.Utilities
{
    public class UtilExceptions : Exception
    {
        public  string Sql { get; set; }
        public  object Parametres { get; set; }
        public new Exception InnerException;
        public new string  StackTrace;
        public new MethodBase TargetSite;
        public new string Source;

        public UtilExceptions(string message)
            : base(message){}

        public UtilExceptions(AgileProvider context,string message, string sql)
            : base(message) {
            this.Sql = sql;
        }

        public UtilExceptions(AgileProvider context, string message, string sql, object pars)
            : base(message) {
            this.Sql = sql;
            this.Parametres = pars;
        }

        public UtilExceptions(AgileProvider context, Exception ex, string sql, object pars)
            : base(ex.Message)
        {
            this.Sql = sql;
            this.Parametres = pars;
            this.InnerException = ex.InnerException;
            this.StackTrace = ex.StackTrace;
            this.TargetSite = ex.TargetSite;
            this.Source = ex.Source;
        }

        public UtilExceptions(AgileProvider context, string message, object pars)
            : base(message) {
            this.Parametres = pars;
        }
    }
    public class VersionExceptions : UtilExceptions
    {
        public VersionExceptions(string message)
            : base(message){ }
    }
}
