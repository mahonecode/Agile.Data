using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgSqlTest.Models
{
    public class ViewOrder:Order
    {
        public string CustomName { get; set; }
    }
}
