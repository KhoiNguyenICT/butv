using System;
using System.Collections.Generic;
using System.Linq;
using BUTV.Core.Domain.Directory;
using BUTV.Data;

namespace BUTV.Services.Directory
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tags> _repoTag;
        public TagService(IRepository<Tags> repoTag)
        {
            _repoTag = repoTag;
        }
        public IList<Tags> GetAll()
        {
            throw new NotImplementedException();
        }

        public Tags GetById(int id) => _repoTag.Get(id);

        public Tags GetByName(string name)
        {
            return _repoTag.TableNoTracking.FirstOrDefault(f => f.Name.Trim().ToLower() == name.Trim().ToLower());            
        }

        public IList<Tags> GetPopular()
        {
          return  _repoTag.TableNoTracking.OrderByDescending(o => o.Hit).Take(20).ToList();
        }

        public void InsertTags(Tags tag)
        {
            if (tag == null) throw new ArgumentNullException("InsertTags");
            var foundTag = GetById(tag.Id);
            if (foundTag == null)
                _repoTag.Insert(tag);
        }

        public void SaveTags(Tags tag)
        {
            if (tag == null) throw new ArgumentNullException("SaveTags");
            _repoTag.Update(tag);
        }
    }
}
