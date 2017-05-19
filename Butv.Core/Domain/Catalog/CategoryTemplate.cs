using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BUTV.Core.Domain.Catalog
{
    public partial class CategoryTemplate : BaseEntity
    {
        [MaxLength(400)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string ViewPath { get; set; }

        public int DisplayOrder { get; set; }
    }
}
