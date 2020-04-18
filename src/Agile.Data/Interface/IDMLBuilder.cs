using Agile.Data.Abstract;
using Agile.Data.ExpressionsToSql;
using Agile.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Interface
{
    public partial interface IDMLBuilder
    {
        string SqlTemplate { get; }
        List<AgileParameter> Parameters { get; set; }
        AgileProvider  Context { get; set; }
        StringBuilder sql { get; set; }
        string ToSqlString();
        void Clear();
    }
}
