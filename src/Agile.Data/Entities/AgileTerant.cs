using Agile.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.Entities
{
    public class AgileTerant
    {
        public AgileProvider Context { get; set; }
        public ConnectionConfig ConnectionConfig { get; set; }
    }
}
