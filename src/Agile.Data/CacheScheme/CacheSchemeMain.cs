using Agile.Data.Abstract;
using Agile.Data.Entities;
using Agile.Data.ExternalServiceInterface;
using Agile.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Data.CacheScheme
{
    internal class CacheSchemeMain
    {
        public static T GetOrCreate<T>(ICacheService cacheService, QueryBuilder queryBuilder, Func<T> getData, int cacheDurationInSeconds, AgileProvider context)
        {
            CacheKey key = CacheKeyBuider.GetKey(context, queryBuilder);
            string keyString = key.ToString();
            var result = cacheService.GetOrCreate(keyString, getData, cacheDurationInSeconds);
            return result;
        }

        public static void RemoveCache(ICacheService cacheService, string tableName)
        {
            var keys = cacheService.GetAllKey<string>();
            if (keys.HasValue())
            {
                foreach (var item in keys)
                {
                    if (item.ToLower().Contains(UtilConstants.Dot + tableName.ToLower() + UtilConstants.Dot))
                    {
                        cacheService.Remove<string>(item);
                    }
                }
            }
        }
    }
}
