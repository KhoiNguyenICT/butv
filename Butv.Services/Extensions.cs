

namespace BUTV.Services
{
    public static class Extensions
    {
        //// current Entity Framework core does not have Lazy Loading feature, so we do eager loading
        //public static MovieItem Movie(this MovieCategory entity)
        //{            
        //    var repository = EngineContext.Instance.GetService(typeof(IRepository<MovieItem>)) as IRepository<MovieItem>;
        //    return repository.Get(entity.MovieId);
        //}
        //public static Category Category(this MovieCategory entity)
        //{
        //    var repository = EngineContext.Instance.GetService(typeof(IRepository<Category>)) as IRepository<Category>;
        //    return repository.Get(entity.MovieId);
        //}
    }
}
