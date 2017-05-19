
using System.Collections.Generic;
using System.Linq;
using BUTV.Core.Domain.Movie;
using BUTV.Data;
using BUTV.Data.Infrastructure;
using System.Threading.Tasks;
using System;

namespace BUTV.Services.Movie
{
    public class SubMovieService : ISubMovieService
    {
        private readonly IRepository<SubMovieItem> _repoMovie;
        public SubMovieService(IRepository<SubMovieItem> repoMovie)
        {
            _repoMovie = repoMovie;
        }
        public IList<SubMovieItem> GetAllMovieEpisodes(int movieId)
        {
            var query = _repoMovie.Table.Where(r => r.Movie.Id == movieId && r.Publish);
            query = query.OrderByDescending(o => o.CreatedDate);
            return query.ToList();
        }

        public SubMovieItem GetMovieEpisodeById(int id) => _repoMovie.Get(id);
        public void InsertMovieEpisode(SubMovieItem movie)
        {
            if (movie == null) throw new ArgumentNullException("InsertMovieEpisode");
            _repoMovie.Insert(movie);
        }
        public void SaveMovieEpisode(SubMovieItem movie)
        {
            if (movie == null) throw new ArgumentNullException("SubMovieItem");
            _repoMovie.Update(movie);

        }
    }
}
