using System;
using System.Collections.Generic;
using System.Linq;
using BUTV.Core.Domain.Caching;
using BUTV.Data;

namespace BUTV.Services.Caching
{
    /// <summary>
    /// goal: to maintain the memory cached keys and help to loop through all keys 
    /// Current Imemorycache does not have the loop method yet.
    /// </summary>
    public partial class CacheService : ICacheService
    {
        private readonly IRepository<CacheKey> _repoCacheKey;
        public CacheService(IRepository<CacheKey> repoCacheKey)
        {
            this._repoCacheKey = repoCacheKey;
        }
        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CacheKey> GetAllKeys(string pattern, bool isLive = false)
        {
            var query = _repoCacheKey.Table.Where(c => c.IsLive == isLive);
            if (!String.IsNullOrWhiteSpace(pattern))
            {
                query = query.Where(q => q.Key.Contains(pattern));

            }
            return query;
        }
        //public virtual void RemoveByPattern(string pattern)
        //{
        //    this.RemoveByPattern(pattern, Cache.Select(p => p.Key));
        //}
        public CacheKey GetKey(string key) => _repoCacheKey.TableNoTracking.FirstOrDefault(f => f.Key == key);

        public void Insert(CacheKey key)
        {
            _repoCacheKey.Insert(key);
        }

        public void Save(CacheKey key)
        {
            _repoCacheKey.Update(key);
        }
    }
}
