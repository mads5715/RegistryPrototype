using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.PackageCache
{
    public class PackageCache<T>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public T GetOrCache(object key, Func<T> createItem) {
            T cacheEntry;
            if (!_cache.TryGetValue(key,out cacheEntry))
            {
                cacheEntry = createItem();
                _cache.Set(key, cacheEntry);
            }
            return cacheEntry;
        }
    }
}
