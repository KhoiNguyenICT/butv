using BUTV.Core.Domain.Caching;
using BUTV.Data;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUTV.Services.Caching
{
    public partial class MemoryCacheManager : ICacheManager
    {
        private readonly ICacheService _cacheService; // maintain the keys on SqlServer
        private readonly IMemoryCache _cache;
        public MemoryCacheManager(ICacheService cacheService,
            IMemoryCache memCache)
        {
            this._cacheService = cacheService;
            this._cache = memCache;
        }
        public virtual T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
             .SetSlidingExpiration(TimeSpan.FromMinutes(cacheTime));
            _cache.Set(key, data, cacheEntryOptions);

            var cacheKey = _cacheService.GetKey(key);
            if (cacheKey != null)
            {
                if (cacheKey.IsLive)
                {
                    cacheKey.IsLive = true;
                    _cacheService.Save(cacheKey);
                }
            }
            else
            {
                var newKey = new CacheKey()
                {
                    Key = key,
                    IsLive = true
                };
                _cacheService.Insert(newKey);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            var cacheEntry = _cache.Get(key);
            return cacheEntry == null ? false : true;
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public virtual void Remove(string key)
        {
            _cache.Remove(key);
            var cacheKey=_cacheService.GetKey(key);
            if (cacheKey != null)
            {
                cacheKey.IsLive = false;
                _cacheService.Save(cacheKey);
            }
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var allKeys = _cacheService.GetAllKeys("", true);
            this.RemoveByPattern(pattern, allKeys.Select(p => p.Key));
            foreach (var item in allKeys)
            {
                item.IsLive = false;
                _cacheService.Save(item);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
            var allKeys = _cacheService.GetAllKeys("", true);
            foreach (var item in allKeys)
            {
                Remove(item.Key);  // remove from cache
                item.IsLive = false;
                _cacheService.Save(item); // update state
            }

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
