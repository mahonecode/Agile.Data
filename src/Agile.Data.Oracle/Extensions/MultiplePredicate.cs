using System;
using System.Collections.Generic;

namespace Agile.Data.Oracle.Extensions
{
    /// <summary>
    /// 多条件组合
    /// </summary>
    public class MultiplePredicate
    {
        private readonly List<MultiplePredicateItem> _items;

        public MultiplePredicate()
        {
            _items = new List<MultiplePredicateItem>();
        }

        public IEnumerable<MultiplePredicateItem> Items
        {
            get { return _items.AsReadOnly(); }
        }

        public void Add<T>(IPredicate predicate, IList<ISort> sort = null) where T : class
        {
            _items.Add(new MultiplePredicateItem
                           {
                               Value = predicate,
                               Type = typeof(T),
                               Sort = sort
                           });
        }

        public void Add<T>(object id) where T : class
        {
            _items.Add(new MultiplePredicateItem
                           {
                               Value = id,
                               Type = typeof (T)
                           });
        }
        
    }


    /// <summary>
    /// 条件项
    /// </summary>
    public class MultiplePredicateItem
    {
        public object Value { get; set; }
        public Type Type { get; set; }
        public IList<ISort> Sort { get; set; }
    }
}