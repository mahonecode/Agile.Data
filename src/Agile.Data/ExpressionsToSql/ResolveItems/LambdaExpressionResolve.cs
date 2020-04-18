using Agile.Data.Enums;
using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Agile.Data.ExpressionsToSql
{
    public class LambdaExpressionResolve : BaseResolve
    {
        public LambdaExpressionResolve(ExpressionParameter parameter) : base(parameter)
        {
            LambdaExpression lambda = base.Expression as LambdaExpression;
            var expression = lambda.Body;
            base.Expression = expression;
            if (parameter.Context.ResolveType.IsIn(ResolveExpressType.FieldMultiple, ResolveExpressType.FieldSingle)) {
                parameter.CommonTempData = CommonTempDataType.Append;
            }
            base.Start();
        }
    }
}
