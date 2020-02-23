/*
    Copyright (C) 2019  Mads Dürr-Wium

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
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
