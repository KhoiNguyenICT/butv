using BUTV.Core.Domain.Movie;
using BUTV.Data.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BUTV.Services.Movie
{
    public partial interface IMovieActorService
    {
        
        IList<MovieActor> GetActorsbyMovieId(int movieId);
        MovieActor GetMovieActorById(int id);
        void InsertMovieActor(MovieActor movie);
        void SaveMovieActor(MovieActor movie);
    }
    public partial interface IActorService
    {

        //IList<MovieActor> GetActorsbyMovieId(int movieId);
        Actor GetActorById(int id);
        //void InsertMovieActor(MovieActor movie);
        //void SaveMovieActor(MovieActor movie);
    }
}
