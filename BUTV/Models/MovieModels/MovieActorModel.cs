using BUTV.Models.ActorModels;

namespace BUTV.Models.MovieModels
{
    public class MovieActorModel: BaseModel
    {
        public int MovieId { get; set; }        
        public int ActorId { get; set; }
        public ActorModel Actor { get; set; }        
        public string Character { get; set; }
    }
   
}
