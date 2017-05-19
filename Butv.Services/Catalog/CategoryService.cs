using System;
using System.Collections.Generic;
using System.Linq;

using BUTV.Core.Domain.Catalog;
using BUTV.Data;
using Microsoft.Extensions.Caching.Memory;

namespace BUTV.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        private const string CATEGORIES_BY_ID_KEY = "BUTV.category.id-{0}";
        private IRepository<Category> _repoCategory;
        private IMemoryCache _cacheManager;
        public CategoryService(IRepository<Category> repoCategory, IMemoryCache cacheManager)
        {
            _repoCategory = repoCategory;
            _cacheManager = cacheManager;
        }
        public IList<Category> Categories(bool showOnHomePage = false)=>
          _repoCategory.Table.Where(c => c.ShowOnHomePage == showOnHomePage).ToList();
        
        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("AddCategory");
            _repoCategory.Insert(category);

        }

        public Category GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return null;

            string key = string.Format(CATEGORIES_BY_ID_KEY, categoryId);
            return _cacheManager.GetOrCreate(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(24);
                return _repoCategory.Get(categoryId);
            });
        }

        public void SaveCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException("SaveCategory");
            Category dbEntry = _repoCategory.Table
                .FirstOrDefault(p => p.Id == category.Id);
            if (dbEntry != null)
            {
                dbEntry.Name = category.Name;
                dbEntry.Description = category.Description;

            }
            _repoCategory.Update(dbEntry);

        }

        Category ICategoryService.DeleteCategory(int id)
        {
            Category category = _repoCategory.Get(id);
            if (category != null)
            {
                _repoCategory.Delete(category);
            }
            return category;
        }
    }
}
