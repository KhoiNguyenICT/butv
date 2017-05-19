
using BUTV.Core.Domain.Movie;
using System.Collections.Generic;

namespace BUTV.Services.Movie
{
    public partial interface IDirectorService
    {
        Director GetDirectorById(int id);
        IList<MovieDirector> GetDirectorsByMovieId(int id);
    }
}
