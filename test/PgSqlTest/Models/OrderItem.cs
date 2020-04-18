using Agile.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PgSqlTest.Models
{
    [AgileTable("OrderDetail")]
    public class OrderItem
    {
        [AgileColumn(IsPrimaryKey =true, IsIdentity =true)]
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public decimal? Price { get; set; }
        [AgileColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }
    }
}
