using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUTV.Models.Directory
{
    public class CountryModel: BaseModel
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string ShortName { get; set; }
    }
    public class ListCountriesModel
    {
        public IList<CountryModel> Countries { get; set; }
    }
}
