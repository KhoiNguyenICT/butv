using BUTV.Core.Domain.Catalog;
using BUTV.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUTV.Services.Catalog
{
    public static class CategoryExtensions
    {
        public static IList<Category> GetCategoryBreadCrumb(this Category category,
          ICategoryService categoryService,
          bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var result = new List<Category>();

            //used to prevent circular references
            var alreadyProcessedCategoryIds = new List<int>();

            while (category != null && category.ParentCategoryId >0 &&//not null
                //!category.Deleted && //not deleted
                (showHidden || category.Published) && //published
                //(showHidden || aclService.Authorize(category)) && //ACL
                //(showHidden || storeMappingService.Authorize(category)) && //Store mapping
                !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.Id);

                category = categoryService.GetCategoryById(category.ParentCategoryId);
            }
            result.Reverse();
            return result;
        }
    }
}
