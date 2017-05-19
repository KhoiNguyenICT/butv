using BUTV.Core.Domain.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace BUTV.Services.Caching
{
    public partial interface ICacheService
    {
        IEnumerable<CacheKey> GetAllKeys(string pattern, bool isLive = false);
        CacheKey GetKey(string key);
        void Insert(CacheKey key);
        void Save(CacheKey key);
        void Delete(string key);
    }
}
