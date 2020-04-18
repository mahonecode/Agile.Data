using Agile.Data.Abstract;
using Agile.Data.Entities;
using Agile.Data.ExpressionsToSql;
using Agile.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Data.Interface
{
    public partial interface ILambdaExpressions
    {
        MappingColumnList MappingColumns { get; set; }
        MappingTableList MappingTables { get; set; }
        IgnoreColumnList IgnoreComumnList { get; set; }
        List<SqlFuncExternal> SqlFuncServices { get; set; }

        List<JoinQueryInfo> JoinQueryInfos { get; set; }
        bool IsSingle { get; set; }
        AgileProvider Context { get; set; }
        IDbMethods DbMehtods { get; set; }
        Expression Expression { get; set; }
        int Index { get; set; }
        int ParameterIndex { get; set; }
        List<AgileParameter> Parameters { get; set; }
        ExpressionResult Result { get; set; }
        string SqlParameterKeyWord { get; }
        string SingleTableNameSubqueryShortName { get; set; }
         Action<Type> InitMappingInfo { get; set; }
         Action RefreshMapping { get; set; }
        bool PgSqlIsAutoToLower { get; set; }

        string GetAsString(string fieldName, string fieldValue);
        void Resolve(Expression expression, ResolveExpressType resolveType);
        void Clear();
    }
}
