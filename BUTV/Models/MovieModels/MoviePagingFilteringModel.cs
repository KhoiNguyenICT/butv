using BUTV.Infrastructure;
namespace BUTV.Models.MovieModels
{
    public class MoviePagingFilteringModel: BasePageableModel
    {
       
        /// <summary>
        /// Order by
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// Product sorting
        /// </summary>
        public string ViewMode { get; set; }
    }
}
