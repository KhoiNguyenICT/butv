using BUTV.Core.Domain.Directory;
using System.Collections.Generic;

namespace BUTV.Services.Directory
{
    public partial interface ITagService
    {
        Tags GetById(int id);
        Tags GetByName(string name);
        IList<Tags> GetAll();
        IList<Tags> GetPopular();
        void InsertTags(Tags tag);
        void SaveTags(Tags tag);
    }
}
