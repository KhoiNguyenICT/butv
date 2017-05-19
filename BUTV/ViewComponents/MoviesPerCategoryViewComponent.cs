using BUTV.Models.MovieModels;
using BUTV.Services.Catalog;
using BUTV.Services.Media;
using BUTV.Services.Movie;
using Microsoft.AspNetCore.Mvc;
using BUTV.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BUTV.Core.Domain.Movie;
using BUTV.Services.Seo;
using BUTV.Models.Media;
using System;
using BUTV.Models.Catalog;
using BUTV.Core.Domain.Catalog;
using BUTV.Services.Directory;

using Microsoft.Extensions.Caching.Memory;
using BUTV.Infrastructure;
using BUTV.Services.Caching;
using BUTV.Factories;

namespace BUTV.ViewComponents
{
    public class MoviesPerCategoryViewComponent : ViewComponent
    {

        private readonly IMovieModelFactory _movieModelFactory;
        
        private readonly ICountryService _countryService;
        private readonly ICategoryService _categoryService;
        private readonly ISubMovieService _subMovieService;
        private readonly IMovieService _movieService;
        private readonly ICacheManager _cacheManager;
        public MoviesPerCategoryViewComponent(ICategoryService categoryService,
             IPictureService pictureService, IMovieService movieService,
             ICountryService countryService,
             ICacheManager cacheManager,
             ISubMovieService subMovieService,
             IMovieModelFactory movieModelFactory)
        {
            _movieModelFactory = movieModelFactory;
            _subMovieService = subMovieService;
            _countryService = countryService;
            _movieService = movieService;
            _categoryService = categoryService;
            
            _cacheManager = cacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(
            bool showOnHomePage = false, int pageIndex = 1, int pageSize = 12)
        {
            int categoryId = ViewData["categoryId"] != null ? (int)ViewData["categoryId"] : 0;

            var model = new List<MoviesOnCategoriesModel>();
            string key = "";
            key = (showOnHomePage) ? string.Format(ModelCacheEventConsumer.MOVIES_ON_CATEGORY_HOMEPAGE) :
                                     string.Format(ModelCacheEventConsumer.MOVIES_ON_CATEGORY, categoryId);

            var cachedEntry = await _cacheManager.Get(key,  async() =>
            {
                model = await LoadMovie(categoryId, showOnHomePage, pageIndex, pageSize);
                return model;
            });
           
            return View(cachedEntry);
        }
        private async Task<List<MoviesOnCategoriesModel>> LoadMovie(int categoryId = 0,
            bool showOnHomePage = false, int pageIndex = 1, int pageSize = 12)
        {
            var model = new List<MoviesOnCategoriesModel>();

            try
            {
                var categories = _categoryService.Categories(showOnHomePage);               
                foreach (var item in categories)
                {
                    var subs = _categoryService.Categories().Where(c => c.ParentCategoryId == item.Id);
                    var moviesOnCategory = new MoviesOnCategoriesModel()
                    {
                        CategoryName = item.Name,
                        CategoryId = item.Id,
                        CategorySeName = item.GetSeName(),
                        Categories = PrepareCategoryModel(subs.ToList())
                    };
                    foreach (var c in moviesOnCategory.Categories)
                    {
                        if (c.Id == categoryId)
                        {
                            c.IsActive = true;
                            break;
                        }
                    }
                    if (categoryId > 0)
                        subs = subs.Where(c => c.Id == categoryId).ToList();
                    var categoryIds = subs.Select(x => x.Id).ToList();
                    var movies = await _movieService.SearchMoviesWithAsync(pageSize: pageSize, categoryIds: categoryIds);
                   
                    moviesOnCategory.Movies = movies.Select(x => _movieModelFactory.PrepareMovieModel(x)).ToList();

                    model.Add(moviesOnCategory);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return model;
        }
   
        private IList<CategoryModel> PrepareCategoryModel(IList<Category> categories)
        {
            return categories.Select(x => x.ToModel()).ToList();
        }
        
    }
}
