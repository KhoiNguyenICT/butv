using System.Collections.Generic;

namespace BUTV.Models.Catalog
{
    public class MenuModel
    {
        public MenuModel()
        {
            Categories = new List<CategorySimpleModel>();
        }
        public virtual IList<CategorySimpleModel> Categories { get; set; }
    }
}
