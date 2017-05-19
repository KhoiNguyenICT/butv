using System.Collections.Generic;
using System.Linq;
using BUTV.Core.Domain.Movie;
using BUTV.Data;
using BUTV.Data.Infrastructure;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using System.Data;
using BUTV.Services.Caching;
using BUTV.Core;

namespace BUTV.Services.Movie
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<MovieItem> _repoMovie;
        private readonly ICacheManager _cacheManager;
        public MovieService(IRepository<MovieItem> repoMovie,
            ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _repoMovie = repoMovie;
        }
        
        public MovieItem GetMovieById(int id) => _repoMovie.Get(id);

        public void InsertMovie(MovieItem movie)
        {
            if (movie == null) throw new ArgumentNullException("InsertMovie");
            _repoMovie.Insert(movie);
            _cacheManager.RemoveByPattern(ModelCacheEventConsumer.MOVIES_PATTERN_KEY);
        }
        /// <summary>
        /// take the most viewed movies which have been recently updated.
        /// </summary>
        /// <returns></returns>
        public IList<MovieItem> GetRecommendedMovies()
        {
            return _repoMovie.TableNoTracking.OrderByDescending(o => o.Year)
                .ThenByDescending(t => t.UpdatedDate)
                .ThenByDescending(t => t.View)
                .ThenByDescending(t => t.ReleasedDate)
                .Where(m => m.Publish).Take(18).ToList();
        }

        public void SaveMovie(MovieItem movie)
        {
            if (movie == null) throw new ArgumentNullException("SaveMovie");

            _repoMovie.Update(movie);
            //_cacheManager.RemoveByPattern(ModelCacheEventConsumer.MOVIES_PATTERN_KEY);  // clear all cache for any movie patterns
        }
                
        public async Task<PagedList<MovieItem>> SearchMoviesWithAsync(string name = "", int pageIndex = 1, 
            int pageSize = int.MaxValue, IList<int> categoryIds = null, bool showHidden = false, int sortBy=-1)
        {
            string commaSeparatedCategoryIds = categoryIds == null ? "" : string.Join(",", categoryIds);

            var pCategoryIds = new SqlParameter()
            {
                ParameterName = "@CategoryIds",
                Value = commaSeparatedCategoryIds,
                DbType = DbType.String
            };
            var pKeywords = new SqlParameter()
            {
                ParameterName = "@Keywords",
                DbType = DbType.String,
                Value = name
            };
            var pPageIndex = new SqlParameter()
            {
                ParameterName = "@PageIndex",
                DbType = DbType.Int32,
                Value = pageIndex
            };
            var pPageSize = new SqlParameter()
            {
                ParameterName = "@PageSize",
                DbType = DbType.Int32,
                Value = pageSize
            };

            var pSortBy = new SqlParameter()
            {
                ParameterName = "@SortBy",
                DbType = DbType.Int32,
                Value = sortBy
            };

            var pTotalRecords = new SqlParameter();
            pTotalRecords.ParameterName = "@TotalRecords";
            pTotalRecords.Direction = ParameterDirection.Output;
            pTotalRecords.DbType = DbType.Int32;

            var query = _repoMovie.ExecuteStoredProcedureList("dbo.[SearchMovies] " +
                "@CategoryIds, @Keywords, @PageIndex,@PageSize,@SortBy,@TotalRecords OUTPUT",
                pCategoryIds,
                pKeywords,
                pPageIndex,
                pPageSize,
                pSortBy,
                pTotalRecords);
            int totalRecords =  (int)pTotalRecords.Value;
            
            return await PagedList<MovieItem>.CreateAsync(query, pageIndex, pageSize, totalRecords);
        }
        public PagedList<MovieItem> SearchMovies(string name = "", int pageIndex = 1,
           int pageSize = int.MaxValue, IList<int> categoryIds = null, bool showHidden = false, int sortBy = -1)
        {
            string commaSeparatedCategoryIds = categoryIds == null ? "" : string.Join(",", categoryIds);

            var pCategoryIds = new SqlParameter()
            {
                ParameterName = "@CategoryIds",
                Value = commaSeparatedCategoryIds,
                DbType = DbType.String
            };
            var pKeywords = new SqlParameter()
            {
                ParameterName = "@Keywords",
                DbType = DbType.String,
                Value = name
            };
            var pPageIndex = new SqlParameter()
            {
                ParameterName = "@PageIndex",
                DbType = DbType.Int32,
                Value = pageIndex
            };
            var pPageSize = new SqlParameter()
            {
                ParameterName = "@PageSize",
                DbType = DbType.Int32,
                Value = pageSize
            };

            var pSortBy = new SqlParameter()
            {
                ParameterName = "@SortBy",
                DbType = DbType.Int32,
                Value = sortBy
            };

            var pTotalRecords = new SqlParameter();
            pTotalRecords.ParameterName = "@TotalRecords";
            pTotalRecords.Direction = ParameterDirection.Output;
            pTotalRecords.DbType = DbType.Int32;

            var query = _repoMovie.ExecuteStoredProcedureList("dbo.[SearchMovies] " +
                "@CategoryIds, @Keywords, @PageIndex,@PageSize,@SortBy,@TotalRecords OUTPUT",
                pCategoryIds,
                pKeywords,
                pPageIndex,
                pPageSize,
                pSortBy,
                pTotalRecords);
            int totalRecords = (int)pTotalRecords.Value;

            return new PagedList<MovieItem>(query.ToList(), pageIndex, pageSize, totalRecords);
        }
    }
}
