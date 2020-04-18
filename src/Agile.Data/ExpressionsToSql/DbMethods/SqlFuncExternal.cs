using Agile.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.ExpressionsToSql
{
    public class SqlFuncExternal
    {
        public string UniqueMethodName { get; set; }
        public Func<MethodCallExpressionModel, DatabaseType, ExpressionContext, string> MethodValue { get; set; }
    }
}
