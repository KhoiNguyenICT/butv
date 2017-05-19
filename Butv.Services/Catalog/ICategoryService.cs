using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BUTV.Core.Domain.Catalog;

namespace BUTV.Services.Catalog
{
    public interface ICategoryService
    {
        Category GetCategoryById(int categoryId);
        IList<Category> Categories(bool showOnHomePage = false);  // careful with IEnumrable in EF
        void SaveCategory(Category category);
        void AddCategory(Category category);
        Category DeleteCategory(int id);
    }
}
