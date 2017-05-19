using BUTV.Core.Domain.Movie;
using BUTV.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUTV.Services.Movie
{
    public partial interface IMovieService
    {

        Task<PagedList<MovieItem>> SearchMoviesWithAsync(string name="",int pageIndex = 1, int pageSize = int.MaxValue,
             IList<int> categoryIds = null, bool showHidden = false, int sortBy = -1);
        PagedList<MovieItem> SearchMovies(string name = "", int pageIndex = 1, int pageSize = int.MaxValue,
              IList<int> categoryIds = null, bool showHidden = false, int sortBy = -1);
        MovieItem GetMovieById(int id);
        void InsertMovie(MovieItem movie);
        void SaveMovie(MovieItem movie);
        
        IList<MovieItem> GetRecommendedMovies();

    }
}
