
using BUTV.Core.Domain.Catalog;
using BUTV.Core.Domain.Seo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BUTV.Core.Domain.Movie
{
    public class MovieItem : BaseEntity, ISlugSupported
    {
        [MaxLength(200)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [MaxLength(200)]
        public string RealName { get; set; }
        public string Genres { get; set; }
        [MaxLength(200)]
        public string Imdb { get; set; }
        [MaxLength(50)]
        public string ImdbRate { get; set; }
        public int View { get; set; }
        [MaxLength(50)]
        public string Length { get; set; }
        [MaxLength(50)]
        public string CurrentStatus { get; set; } // tập 3/10
        public int Year { get; set; }

        public double Rate { get; set; }  // BUTV users' rating
        public int PictureId { get; set; }
        public bool Status { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }

        public string Tags { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }  // phimbo has new episode
        public DateTime ReleasedDate { get; set; }
        public bool Publish { get; set; }

        public string Trailer { get; set; }
        //public int DirectorId { get; set; }
        public virtual ICollection<SubMovieItem> Subs { get; set; }
        public virtual ICollection<MovieActor> MovieActors { get; set; }
        public virtual ICollection<MovieDirector> MovieDirectors { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<MovieCategory> MovieCategories { get; set; }
    }
    // contains all episodes or sources for 
    public class SubMovieItem : BaseEntity
    {
        [MaxLength(200)]
        public string Name { get; set; }
        public int View { get; set; }
        [MaxLength(200)]
        public string Source { get; set; }
        public string SourceUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public int MovieId { get; set; }
        public string GoogleUrl { get; set; }
        public DateTime GoogleUrlGeneratedDate { get; set; }
        [MaxLength(100)]
        public string Quality { get; set; }
        public bool Publish { get; set; }
        public virtual MovieItem Movie { get; set; }
    }
    public class Director : BaseEntity, ISlugSupported
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public int PictureId { get; set; }
        [MaxLength(400)]
        public string Imdb { get; set; }
        public virtual ICollection<MovieDirector> MovieDirectors { get; set; }
    }
    public class Actor : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public int PictureId { get; set; }
        public string Imdb { get; set; }
        public virtual ICollection<MovieActor> MovieActors { get; set; }

    }
    public class MovieActor : BaseEntity
    {
        public int MovieId { get; set; }
        public virtual MovieItem Movie { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        [MaxLength(100)]
        public string Character { get; set; }
    }
    public class MovieCategory : BaseEntity
    {
        public int MovieId { get; set; }
        public virtual MovieItem Movie { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    public class MovieDirector : BaseEntity
    {
        public int MovieId { get; set; }
        public virtual MovieItem Movie { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }

    }
    public class Genre : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

    }

    public enum SortBy
    {
        Day=0,
        Week=7,
        Month=30
    }
}
