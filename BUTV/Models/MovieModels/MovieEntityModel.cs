
using BUTV.Models.Catalog;

using BUTV.Models.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BUTV.Models.MovieModels
{
    public class MovieEntityModel : BaseModel
    {
        public MovieEntityModel()
        {
            Picture = new PictureModel();
            Genres = new List<string>();
            Tags = new List<string>();
            Breadcrumb = new MovieBreadcrumbModel();
            
            MovieSources = new List<MovieEpisodeModel>();
        }
        [Required]
        public string Name { get; set; }
        public string RealName { get; set; }
        public double UserRating { get; set; }
        public string ImdbRating { get; set; }
        public Dictionary<string,string> Directors { get; set; }
        public Dictionary<string, string> Countries { get; set; }
        [Required]
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public IList<string> Genres { get; set; }
        public string ReleaseDate { get; set; }
        public int Year { get; set; }
        public string Trailer { get; set; }
        public int View { get; set; }
        public string Length { get; set; }

        [Required]
        public virtual PictureModel Picture { get; set; }
        public string SeName { get; set; }
        /// <summary>
        /// 1 movie could be in many categories, but we should know in the current context, it belongs to which cat
        /// </summary>
        public int CategoryId { get; set; } 

        public string Source { get; set; }
        public string SourceUrl { get; set; }
        public string GoogleUrl { get; set; }
        public bool HasEpisode { get; set; }
        public string LastEpisode { get; set; }  // phim bo => show dang tap nao
        public DateTime GoogleUrlGeneratedDate { get; set; }
        public IList<string> Tags { get; set; }
        
        public MovieBreadcrumbModel Breadcrumb { get; set; }
        public SubMovieItemModel PlayingSource { get; set; }
        public DateTime PublishDateTime { get; set; }
        
        public virtual IList<MovieEpisodeModel> MovieSources { get; set; } // Full HD, hd, vietsub, thuyetminh... for phimle
    }
    public class MovieEpisodeModel
    {
        public IList<SubMovieItemModel> Episodes { get; set; }
        public string Source { get; set; }
        public string DisplaySource { get; set; }
        public int DisplayOrder { get; set; }
    }
    public partial class MovieBreadcrumbModel 
    {
        public MovieBreadcrumbModel()
        {
            CategoryBreadcrumb = new List<CategorySimpleModel>();
        }        
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieSeName { get; set; }
        public IList<CategorySimpleModel> CategoryBreadcrumb { get; set; }
    }
    public class SubMovieItemModel : BaseModel
    {
        public string Name { get; set; }
        public int View { get; set; }
        public string Source { get; set; }
        
        public string SourceUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Quality { get; set; }
        public string GoogleUrl { get; set; }
        public DateTime GoogleUrlGeneratedDate { get; set; }
        public bool Default { get; set; } // best to play
        
    }
    public class MoviesOnCategoriesModel
    {
        public MoviesOnCategoriesModel()
        {
            this.Movies = new List<MovieEntityModel>();
            this.Categories = new List<CategoryModel>();
            //this.Movies = new List<MovieEntityModel>();
        }
        public IList<MovieEntityModel> Movies { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string CategorySeName { get; set; }
        //public IList<string> SubCategories { get; set; }
        public IList<CategoryModel> Categories { get; set; }
         
    }
}
