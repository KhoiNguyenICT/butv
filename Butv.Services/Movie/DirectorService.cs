using System;
using System.Collections.Generic;
using System.Text;
using BUTV.Core.Domain.Movie;
using BUTV.Data;
using System.Linq;
namespace BUTV.Services.Movie
{
    public class DirectorService : IDirectorService
    {
        private readonly IRepository<Director> _repoDirector;
        private readonly IRepository<MovieDirector> _repoMovieDirector;
        public DirectorService(IRepository<Director> repoDirector,
            IRepository<MovieDirector> repoMovieDirector)
        {
            this._repoDirector = repoDirector;
            _repoMovieDirector = repoMovieDirector;
        }
        public Director GetDirectorById(int id) => _repoDirector.Get(id);

        public IList<MovieDirector> GetDirectorsByMovieId(int id)
        {
            return _repoMovieDirector.TableWithInclude(t=>t.Director).Where(t => t.MovieId== id).ToList();
        }
    }
}
