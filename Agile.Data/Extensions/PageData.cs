using System;
using System.Collections.Generic;

namespace Agile.Data.Extensions
{
    public class PageData<T> 
    {
        /// <summary>
        /// ����Ŀ��
        /// </summary>
        public long TotalItems { get; set; }

        /// <summary>
        /// ÿҳ��Ŀ��
        /// </summary>
        public long ItemsPerPage { get; set; }

        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public long CurrentPage { get; set; }

        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        public long TotalPages
        {
            get { return (long)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}