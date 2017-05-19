
namespace BUTV.Infrastructure
{
    /// <summary>
    /// help to keep all cachekey in the same pattern throughout.
    /// </summary>
    public partial class ModelCacheEventConsumer
    {
        
        public const string CATEGORY_TEMPLATE_MODEL_KEY = "BUTV.pres.categorytemplate-{0}";
        public const string CATEGORY_TEMPLATE_PATTERN_KEY = "BUTV.pres.categorytemplate";
        // so later on, we can clear some patterns like BUTV.pres.movie to wipe out all cacheKeys begining with BUTV.pres.movie
        public const string MOVIES_BY_PERIODS_KEY = "BUTV.pres.movie.byperiods-{0}";
        public const string ALL_MOVIES_KEY = "BUTV.pres.movie.all";
        public const string MOVIE_PICTURE_PATTERN_KEY = "BUTV.pres.movie.picture-{0}-{1}";
        
        public const string MOVIES_ON_CATEGORY_HOMEPAGE = "BUTV.pres.movie.homepage";
        public const string RECOMMENDED_MOVIES = "BUTV.pres.movie.recommended";
        public const string MOVIES_ON_CATEGORY = "BUTV.pres.movie.category-{0}";
        public const string MOVIE_BREADCRUMB_KEY = "BUTV.pres.movie.breadcrumb-{0}";

        public const string ACTOR_PICTURE_PATTERN_KEY = "BUTV.pres.actor.picture-{0}-{1}";

        public const string CATEGORY_BREADCRUMB_KEY = "BUTV.pres.category.breadcrumb-{0}";
        public const string CATEGORY_BREADCRUMB_PATTERN_KEY = "BUTV.pres.category.breadcrumb";

        public const string MENU_BREADCRUMB_PATTERN_KEY = "BUTV.pres.menu";
    }
}
