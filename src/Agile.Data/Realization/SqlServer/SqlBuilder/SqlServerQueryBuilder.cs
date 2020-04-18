using Agile.Data.Abstract;
using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Data.Realization
{
    public class SqlServerQueryBuilder: QueryBuilder
    {
        public override string SqlTemplate
        {
            get
            {
                return "SELECT {0}{"+UtilConstants.ReplaceKey+"} FROM {1}{2}{3}{4}";
            }
        }
    }
}
