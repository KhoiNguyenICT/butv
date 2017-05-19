
using BUTV.Infrastructure;
using BUTV.Models.Media;
using BUTV.Models.MovieModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BUTV.Models.Catalog
{
    public class CategoryModel : BaseModel
    {

        public string Name { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }

        public int ParentCategoryId { get; set; }

        public PictureModel PictureModel { get; set; }


        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }
        public string ShortCountryName { get; set; }
        public int PageSize { get; set; }

        public bool ShowOnHomePage { get; set; }

        public bool IncludeInTopMenu { get; set; }

        public bool Published { get; set; }
        public string SeName { get; set; }
        public bool IsActive { get; set; }
        public virtual IList<SubCategoryModel> SubCategories { get; set; }
        public virtual IList<MovieEntityModel> Movies { get; set; }
        public virtual IList<CategoryModel> CategoryBreadcrumb { get; set; }
        #region Nested Classes

        public partial class SubCategoryModel : BaseModel
        {
            public SubCategoryModel()
            {
                PictureModel = new PictureModel();
            }

            public string Name { get; set; }

            public string SeName { get; set; }

            public PictureModel PictureModel { get; set; }
        }

        #endregion
    }
    public class CategorySimpleModel : BaseModel
    {
        public CategorySimpleModel()
        {
            SubCategories = new List<CategorySimpleModel>();
        }
        public string Name { get; set; }
        public string SeName { get; set; }
        public virtual IList<CategorySimpleModel> SubCategories { get; set; }
    }
    public partial class CatalogPagingFilteringModel : BasePageableModel
    {
        public IList<SelectListItem> PageSizeOptions { get; set; }

        /// <summary>
        /// Order by
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// Product sorting
        /// </summary>
        public string ViewMode { get; set; }
    }
}

