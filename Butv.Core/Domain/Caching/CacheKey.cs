using System;
using System.Collections.Generic;
using System.Text;

namespace BUTV.Core.Domain.Caching
{
    public partial class CacheKey: BaseEntity
    {
        public string Key { get; set; }
        public bool IsLive { get; set; }
    }
}
