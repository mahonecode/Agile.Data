using Agile.Data.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlSeverTest.Models
{

    public class Order
    {
        [AgileColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        [AgileColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; }
        [AgileColumn(IsNullable =true)]
        public int CustomId { get; set; }
        [AgileColumn(IsIgnore = true)]
        public List<OrderItem> Items { get; set; }
    }
}
