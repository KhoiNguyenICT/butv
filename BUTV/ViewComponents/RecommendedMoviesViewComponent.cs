using BUTV.Services.Caching;
using BUTV.Services.Media;
using BUTV.Services.Movie;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using BUTV.Factories;
using BUTV.Infrastructure;

namespace BUTV.ViewComponents
{
    public class RecommendedMoviesViewComponent : ViewComponent
    {
       
        private readonly IMovieService _movieService;
        private readonly ICacheManager _cacheManager;
        private readonly IMovieModelFactory _movieModelFactory;
        public RecommendedMoviesViewComponent(
             IMovieService movieService,
             ICacheManager cacheManager,             
             IMovieModelFactory movieModelFactory)
        {
            _movieModelFactory = movieModelFactory;
            _movieService = movieService;
           
            _cacheManager = cacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string key = string.Format(ModelCacheEventConsumer.RECOMMENDED_MOVIES);

            var cacheEntry = _cacheManager.Get(key, () => {

                var model = _movieService.GetRecommendedMovies()
                   .Select(x => _movieModelFactory.PrepareMovieModel(x)).ToList();
                return model;
            });
           
            return View(cacheEntry);
        }
    }
}
