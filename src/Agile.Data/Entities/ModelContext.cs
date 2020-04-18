using Agile.Data.Abstract;
using Agile.Data.Interface;
using Agile.Data.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Entities
{
    public class ModelContext
    {
        [AgileColumn(IsIgnore = true)]
        [JsonIgnore]
        public AgileProvider Context { get; set; }
        public Interface.IAgileQueryable<T> CreateMapping<T>() where T : class, new()
        {
            Check.ArgumentNullException(Context, "Please use Sqlugar.ModelContext");
            using (Context)
            {
                return Context.Queryable<T>();
            }
        }
    }
}
