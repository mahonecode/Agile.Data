using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Data.Entities
{
    public class PageModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// output
        /// </summary>
        public int PageCount { get; set; }
    }
}
