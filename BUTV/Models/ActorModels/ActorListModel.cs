using BUTV.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUTV.Models.ActorModels
{
    public class ActorListModel
    {
        public ActorListModel()
        {
            ActorList = new List<ActorModel>();
        }
        public IList<ActorModel> ActorList { get; set; }
    }
    public class ActorModel : BaseModel
    {
        public string Name { get; set; }
        public string SeName { get; set; }
        public string Imdb { get; set; }
        public string Character { get; set; }
        public virtual PictureModel Picture { get; set; }
    }
}
