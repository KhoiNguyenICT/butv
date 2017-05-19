namespace BUTV.Models.MovieModels
{
    public partial class SearchBoxModel : BaseModel
    {
        public bool AutoCompleteEnabled { get; set; }
        public bool ShowImagesInSearchAutoComplete { get; set; }
        public int SearchTermMinimumLength { get; set; }
    }
}
