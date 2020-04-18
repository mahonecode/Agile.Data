using Agile.Data.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleTest.Models
{
    [AgileTable("OrderDetail")]
    public class OrderItemInfo
    {
        [AgileColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public decimal? Price { get; set; }
        [AgileColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; }
        [AgileColumn(IsIgnore = true)]
        public Order Order { get; set; }
    }
    [AgileTable("Order")]
    public class OrderInfo
    {
        [AgileColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [AgileColumn(IsIgnore = true)]
        public List<OrderItem> Items { get; set; }
    }
    public class ABMapping
    {
        [AgileColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int AId { get; set; }
        public int BId { get; set; }
        [AgileColumn(IsIgnore = true)]
        public A A { get; set; }
        [AgileColumn(IsIgnore = true)]
        public B B { get; set; }

    }
    public class A
    {
        [AgileColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class B
    {
        [AgileColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
