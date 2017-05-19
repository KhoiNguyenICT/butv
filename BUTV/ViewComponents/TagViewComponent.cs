using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BUTV.Services.Caching;
using BUTV.Extensions;
using System.Threading.Tasks;
using BUTV.Infrastructure;
using BUTV.Services.Directory;

namespace BUTV.ViewComponents
{
    public class TagViewComponent : ViewComponent
    {
        private readonly ITagService _tagService;
        private ICacheManager _cacheManager;
        public TagViewComponent(ITagService tagService,
             ICacheManager cacheManager)
        {
            _tagService = tagService;
            _cacheManager = cacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            string key = ModelCacheEventConsumer.MENU_BREADCRUMB_PATTERN_KEY;
            //  var cachedEntry = _cacheManager.Get(key, () =>
            //{
            var model = _tagService.GetPopular().Select(x=>x.ToModel());
            
            //    return model;
            //});
            return View(model);
        }       
    }
}
