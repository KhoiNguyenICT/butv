using BUTV.Models.ActorModels;
using BUTV.Services.Media;
using BUTV.Services.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BUTV.Extensions;
using BUTV.Models.Media;
using System.Threading.Tasks;
using BUTV.Infrastructure;

namespace BUTV.ViewComponents
{
    public class MovieActorViewComponent : ViewComponent
    {
        private readonly IPictureService _pictureService;
        private readonly IActorService _actorService;
        //private readonly ICategoryService _categoryService;
        private readonly IMovieActorService _movieActorService;
        private IMemoryCache _cacheManager;
        public MovieActorViewComponent(
             IPictureService pictureService, IMovieActorService movieActorService,
             IMemoryCache cacheManager,
             IActorService actorService)
        {
            _actorService = actorService;
            _movieActorService = movieActorService;
            //_categoryService = categoryService;
            _pictureService = pictureService;
            _cacheManager = cacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int movieId)
        {
            var model = new ActorListModel();
            var mv = _movieActorService.GetActorsbyMovieId(movieId);
            int picturesize = 200;
            foreach (var item in mv)
            {
                var actor = _actorService.GetActorById(item.ActorId);
                if (actor != null)
                {
                    var actorModel = actor.ToModel();
                    actorModel.Character = item.Character;
                   
                    var actorPictureCacheKey = string.Format(ModelCacheEventConsumer.ACTOR_PICTURE_PATTERN_KEY, actor.Id, picturesize);
                    actorModel.Picture = await _cacheManager.GetOrCreateAsync(actorPictureCacheKey, entry =>
                    {
                        var picture = _pictureService.GetPictureById(actor.PictureId);

                        return Task.FromResult(new PictureModel
                        {
                            FullSizeImageUrl = _pictureService.GetPictureUrl(picture),
                            ImageUrl = _pictureService.GetPictureUrl(picture, picturesize)
                        });
                    });
                    model.ActorList.Add(actorModel);
                }
            }
            return View(model);

        }
    }
}
