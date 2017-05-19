using Microsoft.AspNetCore.Mvc;
using BUTV.Extensions;
using BUTV.Services.Catalog;
using BUTV.Factories;
using BUTV.Services.Seo;

namespace BUTV.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ICategoryModelFactory _catalogModelFactory;
        public CategoryController(ICategoryService categoryService, 
            ICategoryModelFactory categoryModelFactory,
            IUrlRecordService urlRecordService)
        {
            this._categoryService = categoryService;
            _catalogModelFactory = categoryModelFactory;
            _urlRecordService = urlRecordService;
        }
        public ActionResult Category(string sename)
        {
            var urlRecord=_urlRecordService.GetBySlugCached(sename);
            var category = _categoryService.GetCategoryById(urlRecord.EntityId);
            var model = category.ToModel();
            var templateViewPath = _catalogModelFactory.PrepareCategoryTemplateViewPath(category.CategoryTemplateId);
            ViewData["categoryId"] = category.Id;
            return View(templateViewPath, model);
        }
        
    }
}