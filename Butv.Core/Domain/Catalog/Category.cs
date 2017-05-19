using BUTV.Core.Domain.Movie;
using BUTV.Core.Domain.Seo;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BUTV.Core.Domain.Catalog
{
    public class Category : BaseEntity, ISlugSupported
    {
        public string Name { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        [DefaultValue(0)]
        public int ParentCategoryId { get; set; }
        [DefaultValue(0)]
        public int PictureId { get; set; }
        [DefaultValue(10)]
        public int PageSize { get; set; }
        [DefaultValue(1)]
        public bool ShowOnHomePage { get; set; }
        [DefaultValue(true)]
        public bool IncludeInTopMenu { get; set; }
        [DefaultValue(true)]
        public bool Published { get; set; }
        public int CategoryTemplateId { get; set; }
       
        [MaxLength(50)]
        public string ShortCountryName { get; set; }
        public bool HasEpisodes { get; set; }
        public virtual ICollection<MovieCategory> MovieCategories { get; set; }
    }
    public class AutoSource : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Link { get; set; }
        public string Source { get; set; }
        public bool HasEpisodes { get; set; }
        public int DisplayOrder { get; set; }

    }
    public enum SourcePriority
    {
        phimmoi=0,
        phimbathu=1,
        bilutv=2,
        
    }
}

