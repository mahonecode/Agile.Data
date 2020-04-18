using Agile.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//


namespace SqlSeverTest.Models
{
    [AgileTable("MyEntityMapper")]
    public class EntityMapper
    {
        [AgileColumn(ColumnName ="MyName")]
        public string Name { get; set; }
    }
}
