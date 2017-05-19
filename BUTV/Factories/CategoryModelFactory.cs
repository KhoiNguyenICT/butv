using BUTV.Services.Catalog;
using BUTV.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;


namespace BUTV.Factories
{
    public class CategoryModelFactory: ICategoryModelFactory
    {
        private readonly IMemoryCache _cacheManager;
        private readonly ICategoryTemplateService _categoryTemplateService;
        public CategoryModelFactory(IMemoryCache cacheManager, ICategoryTemplateService categoryTemplateService)
        {
            _cacheManager = cacheManager;
            _categoryTemplateService = categoryTemplateService;
        }
        public virtual string PrepareCategoryTemplateViewPath(int templateId)
        {
            var templateCacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_TEMPLATE_MODEL_KEY, templateId);
            var templateViewPath = _cacheManager.GetOrCreate(templateCacheKey, entry =>
            {
                var template = _categoryTemplateService.GetCategoryTemplateById(templateId);
                if (template == null)
                    template = _categoryTemplateService.GetAllCategoryTemplates().FirstOrDefault();
                if (template == null)
                    throw new Exception("No default template could be loaded");
                return template.ViewPath;
            });

            return templateViewPath;
        }
    }
    public partial interface ICategoryModelFactory
    {
        string PrepareCategoryTemplateViewPath(int templateId);
    }
}
