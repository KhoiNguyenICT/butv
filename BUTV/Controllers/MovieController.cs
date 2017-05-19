using BUTV.Services.Seo;
using BUTV.Extensions;
using System.Linq;
using BUTV.Services.Catalog;
using BUTV.Services.Media;
using BUTV.Services.Movie;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using BUTV.Models.MovieModels;

using BUTV.Infrastructure;
using BUTV.Services.Caching;

using BUTV.Models.Catalog;
using BUTV.Core.Domain.Catalog;
using BUTV.Core.Domain.Movie;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using BUTV.Models.Media;
using BUTV.Services.Message;
using BUTV.Factories;
using BUTV.Services.Directory;
using BUTV.Core.Domain.Directory;

namespace BUTV.Controllers
{

    public class MovieController : Controller
    {
        #region Fields
        private readonly ICacheManager _cacheManager;
        private readonly IMovieService _movieService;
        private readonly IMovieCategoryService _movieCategoryService;
        private readonly ISubMovieService _subMovieService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IPictureService _pictureService;
        private readonly ICategoryService _categoryService;
        private readonly IDirectorService _diretorService;
        private readonly IHostingEnvironment _env;

        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailSender _emailSender;

        private readonly ITagService _tagService;
        private readonly IMovieModelFactory _movieModelFactory;
        #endregion
        #region Constructor
        public MovieController(IMovieService movieService,
            IUrlRecordService urlRecordService,
             ICategoryService categoryService,
             IPictureService pictureService,
            ICacheManager cacheManager,
            ISubMovieService subMovieService,
            IDirectorService directorService,
            IHostingEnvironment env,
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IEmailSender emailSender,
           IMovieCategoryService movieCategoryService,
           IMovieModelFactory movieModelFactory,
           ITagService tagService)
        {
            _tagService = tagService;
            _movieModelFactory = movieModelFactory;
            _movieCategoryService = movieCategoryService;
            _diretorService = directorService;
            _subMovieService = subMovieService;
            _cacheManager = cacheManager;
            _movieService = movieService;
            _urlRecordService = urlRecordService;
            _categoryService = categoryService;
            _pictureService = pictureService;
            _env = env;

            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _emailSender = emailSender;
        }
        #endregion
        public IActionResult Details(string sename)
        {
            var urlRecord = _urlRecordService.GetBySlug(sename);
            if (urlRecord == null || urlRecord.EntityName != "MovieItem") return RedirectToAction("Error", "Home");

            var movie = _movieService.GetMovieById(urlRecord.EntityId);
            if (movie == null)
                return RedirectToAction("Error", "Home");

            var model = _movieModelFactory.PrepareMovieModel(movie, 400, true, true);
            model.Description = movie.Description;
            int categoryId = ViewData["categoryId"] != null ? (int)ViewData["categoryId"] : 0;

            if (categoryId == 0)
            {
                var mc = _movieCategoryService.GetMovieCategoriesById(movie.Id);
                if (mc != null)
                    model.CategoryId = categoryId = mc.FirstOrDefault().CategoryId;
                else
                    return RedirectToAction("Error", "Home");

            }
            var category = _categoryService.GetCategoryById(categoryId);

            var tagParts = movie.Tags.Split(',');
            foreach (var item in tagParts)
            {
                model.Tags.Add(item.Trim());
                var tagFound = _tagService.GetByName(item.Trim());
                if (tagFound == null)
                {
                    var newTag = new Tags()
                    {
                        Hit = 0,
                        Name = item.Trim()
                    };
                    _tagService.InsertTags(newTag);
                }
                else
                {
                    tagFound.Hit += 1;
                    _tagService.SaveTags(tagFound);
                }
            }
            var genreParts = movie.Genres.Split(',');
            foreach (var item in genreParts)
            {
                model.Genres.Add(item.Trim());
            }
            var directors = _diretorService.GetDirectorsByMovieId(movie.Id);
            var dirs = new Dictionary<string, string>();
            foreach (var d in directors)
            {
                if (d != null)
                    dirs.Add(d.Director.Name, d.Director.GetSeName());
            }
            model.Directors = dirs;
            var countries = new Dictionary<string, string>();
            var countryParts = movie.Country.Split(',');
            foreach (var c in countryParts)
            {
                countries.Add(c, ""); // need to have sename 
            }
            model.Countries = countries;

            var sources = LoadMovieEpisodes(model.Id);
            var gSources = sources.GroupBy(g => g.Source);
            if (model.HasEpisode)
            {               
                model.MovieSources = new List<MovieEpisodeModel>();
                int count = 1;
                foreach (var item in gSources)
                {
                    var movieEpisode = new MovieEpisodeModel()
                    {
                        Source = item.Key,
                        Episodes = item.ToList()
                    };
                    model.MovieSources.Add(movieEpisode);
                }
                foreach (var source in model.MovieSources.OrderBy(o => o.DisplayOrder))
                    source.DisplaySource = "Server " + count++;
                var mvEpisode = model.MovieSources.FirstOrDefault();
                var lastEp = mvEpisode.Episodes.LastOrDefault();
                lastEp.Default = true;
                model.PlayingSource = lastEp;
            }
            else
            {
                foreach (var item in gSources)
                {
                    if (item.Count() == 1)
                    {
                        var found = item.FirstOrDefault();
                        found.Default = true;
                    }
                    else
                    {
                        var found = item.FirstOrDefault(f => f.Quality.Contains("HD") && f.Name.Contains("VietSub"));
                        if (found != null) found.Default = true; // Vietsub is the top priority.
                        else
                            foreach (var s in item)
                            {
                                if (s.Quality.Contains("HD"))
                                {
                                    s.Default = true;
                                    break;
                                }
                            }
                    }
                    var movieEpisode = new MovieEpisodeModel()
                    {
                        Source = item.Key,
                        Episodes = item.ToList()
                    };

                    model.MovieSources.Add(movieEpisode);
                }
                int count = 1;
                foreach (var source in model.MovieSources)
                {
                    if (source.Source.Equals(nameof(SourcePriority.bilutv)))
                        source.DisplayOrder = (int)SourcePriority.bilutv;

                    if (source.Source.Equals(nameof(SourcePriority.phimmoi)))
                        source.DisplayOrder = (int)SourcePriority.phimmoi;

                    if (source.Source.Equals(nameof(SourcePriority.phimbathu)))
                        source.DisplayOrder = (int)SourcePriority.phimbathu;
                }
                foreach (var source in model.MovieSources.OrderBy(o => o.DisplayOrder))
                    source.DisplaySource = "Server " + count++;


                model.MovieSources = model.MovieSources.OrderBy(o => o.DisplayOrder).ToList();
                if (model.MovieSources.Any())
                {
                    var episode = model.MovieSources.FirstOrDefault();
                    if (episode != null && episode.Episodes.Any())
                        model.PlayingSource = model.MovieSources.FirstOrDefault().Episodes.FirstOrDefault(f => f.Default);
                    else
                        model.PlayingSource = new SubMovieItemModel();
                }
            }

            string breadcrumbCacheKey = string.Format(ModelCacheEventConsumer.MOVIE_BREADCRUMB_KEY, movie.Id);
            model.Breadcrumb = _cacheManager.Get(breadcrumbCacheKey, () =>
            {
                var breadcrumbModel = new MovieBreadcrumbModel
                {
                    MovieId = movie.Id,
                    MovieName = movie.Name,
                    MovieSeName = movie.GetSeName()
                };
                breadcrumbModel.CategoryBreadcrumb = category.GetCategoryBreadCrumb(_categoryService)
                .Select(catBr => new CategorySimpleModel
                {
                    Id = catBr.Id,
                    Name = catBr.Name,
                    SeName = catBr.GetSeName()
                })
                .ToList();
                return breadcrumbModel;
            });

            movie.View += 1;
            _movieService.SaveMovie(movie);
            return View(model);
        }
        private async Task<string> ParseGoogleUrl(SubMovieItem episode)
        {
            return "http://clips.vorwaerts-gmbh.de/VfE_html5.mp4";   // sample  video :D
        }
        private IList<SubMovieItemModel> LoadMovieEpisodes(int movieId)
        {
            return _subMovieService.GetAllMovieEpisodes(movieId).Select(x => x.ToModel()).ToList();
        }

        [Route("api/movie/getgoogleurl")]
        public async Task<IActionResult> GetGoogleUrl(int id, bool hasEpisode = true, bool reloaded = false)
        {
            var episode = _subMovieService.GetMovieEpisodeById(id);
            if (episode == null)
                return NotFound();
            var tp = DateTime.Now - episode.GoogleUrlGeneratedDate;
            if (tp.TotalMinutes >= 170)
            {
                int count = 0;
                ReloadUrl:
                string url = await ParseGoogleUrl(episode);

                episode.GoogleUrl = url;
                episode.GoogleUrlGeneratedDate = DateTime.Now;
                _subMovieService.SaveMovieEpisode(episode);


            }
            episode.View += 1;

            _subMovieService.SaveMovieEpisode(episode);
            if (!reloaded)
            {
                var movie = _movieService.GetMovieById(episode.MovieId);
                if (movie != null)
                {
                    movie.View += 1;   // not initial loading movie
                    _movieService.SaveMovie(movie);
                }
            }
            return Ok(new { url = episode.GoogleUrl });
        }

        #region Utilities       

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ReportBug(int id)
        {
            if (id <= 0)
                return Json(new { msg = "không tồn tại phim" });

            var movie = _movieService.GetMovieById(id);
            if (movie == null) return Json(new { msg = "không tồn tại phim" });
            _emailSender.SendEmailAsync("congthanhgiong@outlook.com", "Phim bị lỗi " + id + "- BUTV", "good");
            return Json(new { msg = "Cám ơn bạn đã hổ trợ chúng tôi. Chúng tôi sẽ kiểm tra sửa lỗi trong thời gian nhanh nhất có thể" });
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> LoadMore(MoviePagingFilteringModel command, int categoryId = 0)
        {
            if (command.PageSize <= 0) command.PageSize = 12;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            var model = new MoviesOnCategoriesModel();

            try
            {
                var categories = _categoryService.Categories().ToList();
                foreach (var item in categories)
                {
                    var subs = _categoryService.Categories().Where(c => c.ParentCategoryId == item.Id);
                    var moviesOnCategory = new MoviesOnCategoriesModel()
                    {
                        CategoryName = item.Name,
                        CategoryId = item.Id,
                        CategorySeName = item.GetSeName(),

                    };

                    var movies = await _movieService.SearchMoviesWithAsync("", command.PageNumber, command.PageSize,
                        categoryIds: new List<int> { categoryId });
                    model.Movies = movies.Select(x => _movieModelFactory.PrepareMovieModel(x)).ToList();

                    string modelString = await RenderToStringAsync("Movie/_LoadMore", model);
                    return Json(new { ModelString = modelString, pageNumber = command.PageNumber });

                }
            }
            catch (Exception e)
            {

                throw;
            }
            return Json(model);
        }
        #endregion
        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }    

        public async Task<ActionResult> SearchTermAutoComplete(string term)
        {
            if (String.IsNullOrWhiteSpace(term) || term.Length < 3)
                return Content("");

            var termParts = term.Split(' ');

            term = string.Join("*", termParts);
            var source = (await _movieService.SearchMoviesWithAsync(term, pageSize: 10))
                        .Select(x => _movieModelFactory.PrepareMovieModel(x));
            var result = (from p in source
                          select new
                          {
                              label = p.Name,
                              url = Url.RouteUrl("moviedetail", new { sename = p.SeName }),
                              pictureurl = ""
                          })
                         .ToList();

            return Json(result);
        }
    }

}