using BUTV.Core.Domain.Movie;
using BUTV.Models.MovieModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

using BUTV.Extensions;
using BUTV.Infrastructure;
using BUTV.Services.Caching;
using BUTV.Services.Media;
using BUTV.Models.Media;
using BUTV.Services.Movie;

namespace BUTV.Factories
{
    public class MovieModelFactory : IMovieModelFactory
    {
        private readonly ICacheManager _cacheManager;
        private readonly IPictureService _pictureService;
        private readonly IMovieCategoryService _movieCategoryService;
        private readonly ISubMovieService _subMovieService;
        public MovieModelFactory(ICacheManager cacheManager,
            IPictureService pictureService,
            IMovieCategoryService movieCategoryService,
             ISubMovieService subMovieService)
        {
            _subMovieService = subMovieService;
            _cacheManager = cacheManager;
            _pictureService = pictureService;
            _movieCategoryService = movieCategoryService;
        }
        public MovieEntityModel PrepareMovieModel(MovieItem x, int picturesize = 400,
            bool includeShortDesc = true, bool includeDesc = false)
        {
            var model = x.ToModel();

            try
            {
                var mc = _movieCategoryService.GetMovieCategoriesById(x.Id).FirstOrDefault();  //
                var category = mc.Category;
                model.HasEpisode = category.HasEpisodes;
                if (model.HasEpisode)
                {
                    var episodes = _subMovieService.GetAllMovieEpisodes(x.Id);
                    if (episodes != null && episodes.Any())                    
                        model.LastEpisode = episodes.LastOrDefault().Name.Split(' ')[1];
                    
                }
                model.Description = includeDesc ? x.Description : "";
                if (includeShortDesc)
                {
                    var words = x.Description.Split(' ');
                    model.ShortDesc = string.Join(" ", words.Take(40));
                }
                model.Trailer = !String.IsNullOrWhiteSpace(model.Trailer) ? model.Trailer.Replace("watch?v=", "embed/") : "";
                var moviePictureCacheKey = string.Format(ModelCacheEventConsumer.MOVIE_PICTURE_PATTERN_KEY, x.Id, picturesize);
                model.Picture = _cacheManager.Get(moviePictureCacheKey, () =>
                {
                    var picture = _pictureService.GetPictureById(x.PictureId);
                    return new PictureModel
                    {
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                        ImageUrl = _pictureService.GetPictureUrl(picture, picturesize)
                    };
                });
            }
            catch (Exception e)
            {
            }

            return model;
        }

    }
    public interface IMovieModelFactory
    {
        MovieEntityModel PrepareMovieModel(MovieItem x, int picturesize = 400,
            bool includeShortDesc = true, bool includeDesc = false);
    }
}
