using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BUTV.Core.Domain.Directory
{
    public class Country : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        [MaxLength(50)]
        public string ShortName { get; set; }
    }
}
