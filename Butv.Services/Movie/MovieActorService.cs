using BUTV.Services.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using BUTV.Core.Domain.Movie;
using BUTV.Data;

namespace BUTV.Services.Movie
{
    public class MovieActorService : IMovieActorService
    {
        private readonly IRepository<MovieActor> _repoMovieActor;
        public MovieActorService(IRepository<MovieActor> repoMovieActor)
        {
            _repoMovieActor = repoMovieActor;
        }
        public IList<MovieActor> GetActorsbyMovieId(int movieId)
        {
            var mv = _repoMovieActor.TableNoTracking.Where(a => a.MovieId == movieId);
            return mv.ToList();
        }

        public MovieActor GetMovieActorById(int id) => _repoMovieActor.Get(id);


        public void InsertMovieActor(MovieActor movie)
        {
            throw new NotImplementedException();
        }

        public void SaveMovieActor(MovieActor movie)
        {
            throw new NotImplementedException();
        }
    }
    public class ActorService : IActorService
    {
        private readonly IRepository<Actor> _repoActor;
        public ActorService(IRepository<Actor> repoActor)
        {
            _repoActor = repoActor;
        }
        public Actor GetActorById(int id) => _repoActor.Get(id);
        
    }
}
