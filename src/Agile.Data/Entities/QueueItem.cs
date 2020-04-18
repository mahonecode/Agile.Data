using Agile.Data.ExpressionsToSql;
using Agile.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Entities
{
    public class QueueItem
    {
        public string Sql { get; set; }
        public AgileParameter[] Parameters { get; set; }
    }
}
