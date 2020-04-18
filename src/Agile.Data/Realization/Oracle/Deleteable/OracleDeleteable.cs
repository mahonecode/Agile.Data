using Agile.Data.Abstract;
using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Realization
{
    public class OracleDeleteable<T>:DeleteableProvider<T> where T:class,new()
    {
        protected override List<string> GetIdentityKeys()
        {
            return this.EntityInfo.Columns.Where(it => it.OracleSequenceName.HasValue()).Select(it => it.DbColumnName).ToList();
        }
    }
}
