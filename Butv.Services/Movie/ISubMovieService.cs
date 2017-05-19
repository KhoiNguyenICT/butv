using BUTV.Core.Domain.Movie;
using BUTV.Data.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BUTV.Services.Movie
{
    public partial interface ISubMovieService
    {        
        IList<SubMovieItem> GetAllMovieEpisodes(int movieId);
        SubMovieItem GetMovieEpisodeById(int id);
        void InsertMovieEpisode(SubMovieItem movie);
        void SaveMovieEpisode(SubMovieItem movie);
    }
}
