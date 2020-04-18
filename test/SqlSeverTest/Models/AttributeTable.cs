using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SqlSeverTest.Models
{
    [Table("MyAttributeTable")]
    //[AgileTable("CustomAttributeTable")]
    public class  AttributeTable
    {

        [Key]
        //[AgileColumn(IsPrimaryKey =true)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
