using BUTV.Models.MovieModels;
using BUTV.Services.Catalog;

using BUTV.Services.Movie;
using Microsoft.AspNetCore.Mvc;
using BUTV.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BUTV.Services.Seo;

using System;
using BUTV.Models.Catalog;
using BUTV.Core.Domain.Catalog;

using BUTV.Services.Caching;
using BUTV.Factories;

namespace BUTV.ViewComponents
{
    public class MoviesOnCategoryViewComponent : ViewComponent
    {

        private readonly IMovieModelFactory _movieModelFactory;
        
        //private readonly ICountryService _countryService;
        private readonly ICategoryService _categoryService;
       
        private readonly IMovieService _movieService;
        private ICacheManager _cacheManager;
        public MoviesOnCategoryViewComponent(ICategoryService categoryService,
             
             IMovieService movieService,
             
             ICacheManager cacheManager,
             
            IMovieModelFactory movieModelFactory)
        {
             _movieModelFactory = movieModelFactory;            
            _movieService = movieService;
            _categoryService = categoryService;
            
            _cacheManager = cacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(
            int pageIndex = 1, int pageSize = 12)
        {
            int categoryId = ViewData["categoryId"] != null ? (int)ViewData["categoryId"] : 0;

            var model = new MoviesOnCategoriesModel();
            //string key = "";
            //key = string.Format(ModelCacheEventConsumer.MOVIES_ON_CATEGORY, categoryId);

           // var cachedEntry = await _cacheManager.Get(key, async () =>
           //{
               model = await LoadMovie(categoryId, pageIndex, pageSize);  //should not use cache here
               //return model;
           //});

            return View(model);
        }
        private async Task<MoviesOnCategoriesModel> LoadMovie(int categoryId = 0,
            int pageIndex = 1, int pageSize = 12)
        {
            var model = new MoviesOnCategoriesModel();
            try
            {
                var category = _categoryService.GetCategoryById(categoryId);
                model.CategoryName = category.Name;
                model.CategoryId = category.Id;
                model.CategorySeName = category.GetSeName();               

                var movies = await _movieService.SearchMoviesWithAsync(pageSize: pageSize, categoryIds: new List<int> { categoryId });
                model.Movies = movies.ToList().Select(x => _movieModelFactory.PrepareMovieModel(x, 400)).ToList();               
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
