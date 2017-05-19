using BUTV.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Text;

namespace BUTV.Services.Directory
{
    public partial interface ICountryService
    {
        IList<Country> GetAllCountries();
        Country GetCountryById(int countryId);
    }
}
