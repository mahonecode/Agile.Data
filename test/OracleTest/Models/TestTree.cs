using Agile.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleTest.Models
{
    public class TestTree
    {
        [AgileColumn(ColumnDataType = "hierarchyid")]
        public string TreeId { get; set; }
        [AgileColumn(ColumnDataType = "Geography")]
        public string GId { get; set; }
        public string Name { get; set; }
    }
}
