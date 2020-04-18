using Agile.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Agile.Data.ExpressionsToSql
{
    public class ExpResolveAccessory
    {
        protected List<AgileParameter> _Parameters;
        protected ExpressionResult _Result;
    }
}
