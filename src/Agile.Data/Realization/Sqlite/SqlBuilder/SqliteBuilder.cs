using Agile.Data.Abstract;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Agile.Data.Realization
{
    public class SqliteBuilder : SqlBuilderProvider
    {
        public override string SqlTranslationLeft { get { return "`"; } }
        public override string SqlTranslationRight { get { return "`"; } }
        public override string SqlDateNow
        {
            get
            {
                return "DATETIME('now') ";
            }
        }
        public override string FullSqlDateNow
        {
            get
            {
                return "select DATETIME('now') ";
            }
        }
    }
}
