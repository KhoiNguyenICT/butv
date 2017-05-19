using System.Collections.Generic;
using System.Linq;
using BUTV.Core.Domain.Movie;
using BUTV.Data;


namespace BUTV.Services.Movie
{
    public class MovieCategoryService : IMovieCategoryService
    {
       
        private readonly IRepository<MovieCategory> _repoMovieCategory;
        public MovieCategoryService( IRepository<MovieCategory> repoMovieCategory)
        {
            
            _repoMovieCategory = repoMovieCategory;
        }
      
       
        public IList<MovieCategory> GetMovieCategoriesById(int movieId) => _repoMovieCategory
                                          .TableWithInclude(c=>c.Category).Where(m => m.MovieId == movieId).ToList();

        
       
    }
}
