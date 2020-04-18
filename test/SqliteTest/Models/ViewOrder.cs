using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteTest.Models
{
    public class ViewOrder:Order
    {
        public string CustomName { get; set; }
    }
}
