using Agile.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;

namespace Agile.Data.Utilities
{
    internal class CallContext
    {
        public static ThreadLocal<List<AgileProvider>> ContextList = new ThreadLocal<List<AgileProvider>>();
    }
}
