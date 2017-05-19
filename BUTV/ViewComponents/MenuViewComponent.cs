using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

using BUTV.Services.Catalog;
using BUTV.Services.Caching;
using BUTV.Models.Catalog;
using BUTV.Services.Seo;
using BUTV.Core.Domain.Catalog;
using System.Threading.Tasks;
using BUTV.Infrastructure;

namespace BUTV.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private ICacheManager _cacheManager;
        public MenuViewComponent(ICategoryService categoryService,
             ICacheManager cacheManager)
        {
            _categoryService = categoryService;
            _cacheManager = cacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            string key = ModelCacheEventConsumer.MENU_BREADCRUMB_PATTERN_KEY;
            var cachedEntry = _cacheManager.Get(key, () =>
          {
              var model = new MenuModel();
              var categories = _categoryService.Categories(true).Where(c => c.ParentCategoryId == 0);
              foreach (var c in categories)
              {
                  var catModel = new CategorySimpleModel()
                  {
                      Name = c.Name,
                      SeName = c.GetSeName(),
                      SubCategories = PrepareSubCategories(c)
                  };
                  model.Categories.Add(catModel);
              }
              return model;
          });
            return View(cachedEntry);
        }
        private IList<CategorySimpleModel> PrepareSubCategories(Category category)
        {
            var categories = _categoryService.Categories().Where(c => c.ParentCategoryId == category.Id);
            var subCategories = new List<CategorySimpleModel>();
            foreach (var c in categories)
            {
                var catModel = new CategorySimpleModel()
                {
                    Name = c.Name,
                    SeName = c.GetSeName()
                };
                subCategories.Add(catModel);
            }
            return subCategories;
        }
    }
}
