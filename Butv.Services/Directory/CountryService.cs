using System;
using System.Collections.Generic;
using System.Linq;
using BUTV.Core.Domain.Directory;
using BUTV.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BUTV.Services.Directory
{
    public class CountryService : ICountryService
    {
        #region Constants
       
        private const string COUNTRIES_ALL_KEY = "BUTV.country.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string COUNTRIES_PATTERN_KEY = "BUTV.country-{0}";

        #endregion

        #region Fields

        private readonly IRepository<Country> _countryRepository;
        private IMemoryCache _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
       
        public CountryService(IMemoryCache cacheManager,
            IRepository<Country> countryRepository )
        {
            this._cacheManager = cacheManager;
            this._countryRepository = countryRepository;
            
        }

        #endregion
        public IList<Country> GetAllCountries()
        {
            string key = string.Format(COUNTRIES_ALL_KEY);
            return _cacheManager.GetOrCreate(key, entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromHours(12));
                var query = _countryRepository.Table;
                //if (!showHidden)
                //    query = query.Where(c => c.Published);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);
                var countries = query.ToList();
                return countries;
            });
        }

        public Country GetCountryById(int countryId)
        {
            if (countryId == 0)
                return null;

            return _countryRepository.Get(countryId);
        }
    }
}
