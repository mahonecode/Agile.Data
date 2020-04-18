using Agile.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlTest.Models
{
    public class Tree
    {
        [AgileColumn(IsPrimaryKey =true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        [AgileColumn(IsIgnore = true)]
        public Tree Parent { get; set; }
        [AgileColumn(IsIgnore = true)]
        public List<Tree> Child { get; set; }
    }
}
