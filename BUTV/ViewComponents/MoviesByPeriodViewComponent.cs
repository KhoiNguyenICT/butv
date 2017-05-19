using BUTV.Services.Movie;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using BUTV.Core.Domain.Movie;
using System;
using BUTV.Infrastructure;
using BUTV.Services.Catalog;
using BUTV.Services.Caching;
using BUTV.Factories;
using BUTV.Models.MovieModels;
using BUTV.Extensions;
namespace BUTV.ViewComponents
{
    public class MoviesByPeriodViewComponent : ViewComponent
    {
      
        private readonly IMovieModelFactory _movieModelFactory;
        private readonly IMovieService _movieService;
        private ICacheManager _cacheManager;
        public MoviesByPeriodViewComponent(
             IMovieService movieService,             
             ICacheManager cacheManager,
             IMovieModelFactory movieModelFactory)
        {
            
            _movieModelFactory = movieModelFactory;
            _movieService = movieService;
            _cacheManager = cacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(SortBy sortBy)
        {
            var now = DateTime.Now;

            string key = "";
            switch (sortBy)
            {
                //case SortBy.Day:

                //    key = string.Format(ModelCacheEventConsumer.MOVIES_BY_PERIODS_KEY, nameof(SortBy.Day));
                //    var cachedDayEntry = await _cacheManager.Get(key, async () =>
                //     {
                //         var movies = await _movieService.GetAllMovies();
                //         if (movies == null) return null;
                //         return movies.Where(w => w.Year == now.Year &&
                //         w.UpdatedDate.Month == now.Month &&
                //         w.UpdatedDate.Day == now.Day)
                //        .OrderByDescending(m => m.View).Take(10).Select(x => PrepareMovieModel(x));
                //     });
                //    return View(cachedDayEntry);
                case SortBy.Week:
                    // use cache here crash the EF
                    key = string.Format(ModelCacheEventConsumer.MOVIES_BY_PERIODS_KEY, nameof(SortBy.Week));
                    var cachedWeekEntry =await _cacheManager.Get(key, async() =>
                    {
                        var movies = await _movieService.SearchMoviesWithAsync("", pageSize: 10, sortBy: (int)SortBy.Week);
                        if (movies == null) return null;
                        return movies.Select(x =>_movieModelFactory.PrepareMovieModel(x, 100, false)).ToList();
                        //.Where(w => IsDayInWeek(w.UpdatedDate))
                        //    .OrderByDescending(m => m.View).Take(10).Select(x => PrepareMovieModel(x));
                    });

                    return View(cachedWeekEntry);
                    
                case SortBy.Month:
                    key = string.Format(ModelCacheEventConsumer.MOVIES_BY_PERIODS_KEY, nameof(SortBy.Month));
                    var cachedMonthEntry =await _cacheManager.Get(key, async() =>
                 {
                     var movies2 = await _movieService.SearchMoviesWithAsync("", pageSize: 10, sortBy: (int)SortBy.Month);
                     if (movies2 == null) return null;
                     return (movies2.Select(x => _movieModelFactory.PrepareMovieModel(x, 100, false)).ToList());

                 });

                    return View(cachedMonthEntry);
            }
            return View(null);
        }
      
        private bool IsDayInWeek(DateTime dt)
        {
            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            // If we started on Sunday, we should actually have gone *back*
            // 6 days instead of forward 1...
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            if (dt >= dates.FirstOrDefault() && dt <= dates.LastOrDefault())
                return true;
            else return false;
        }

    }
}
