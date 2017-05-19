using BUTV.Core.Domain.Movie;
using BUTV.Data.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUTV.Services.Movie
{
    public partial interface IMovieCategoryService
    {

        IList<MovieCategory> GetMovieCategoriesById(int movieId);
    }
}
