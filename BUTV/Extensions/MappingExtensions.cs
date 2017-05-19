using BUTV.Core.Domain.Catalog;
using BUTV.Core.Domain.Directory;
using BUTV.Core.Domain.Movie;
using BUTV.Models.Catalog;
using BUTV.Models.Directory;
using BUTV.Models.MovieModels;
using BUTV.Services.Seo;
using System;
using BUTV.Models.ActorModels;
using System.Globalization;
using BUTV.Models.Tag;

namespace BUTV.Extensions
{
    public static class MappingExtensions
    {
        public static MovieEntityModel ToModel(this MovieItem entity)
        {
            if (entity == null)
                return null;
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("vi-VN");
            var model = new MovieEntityModel
            {
                Id = entity.Id,
                Name = entity.Name,
                RealName = entity.RealName,
                Year = entity.Year,
                Trailer = entity.Trailer,
                ReleaseDate = entity.ReleasedDate.ToString("d MMM yyyy", cultureinfo),
                ImdbRating = entity.ImdbRate,
                SeName = entity.GetSeName(),
                View = entity.View,
                Length = entity.Length,
                UserRating = entity.Rate,
               
            };
           // not include shortdesc and description here for performance
            return model;
        }
        public static SubMovieItemModel ToModel(this SubMovieItem entity)
        {
            if (entity == null)
                return null;

            var model = new SubMovieItemModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Source = entity.Source,
                SourceUrl = entity.SourceUrl,
                GoogleUrlGeneratedDate = entity.GoogleUrlGeneratedDate,
                CreatedDate = entity.CreatedDate,
                GoogleUrl = entity.GoogleUrl,
                Quality = entity.Quality,
                View = entity.View,

            };
            var tp = DateTime.Now - entity.GoogleUrlGeneratedDate;
            model.GoogleUrl = tp.TotalMinutes < 170 ? entity.GoogleUrl : "";
            return model;
        }
        public static TagModel ToModel(this Tags entity)
        {
            if (entity == null)
                return null;

            var model = new TagModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Hit = entity.Hit,

            };

            return model;
        }
        public static ActorModel ToModel(this Actor entity)
        {
            if (entity == null)
                return null;

            var model = new ActorModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Imdb = entity.Imdb,

            };

            return model;
        }
        public static CategoryModel ToModel(this Category entity)
        {
            if (entity == null)
                return null;

            var model = new CategoryModel
            {
                Id = entity.Id,
                Name = entity.Name,
                IncludeInTopMenu = entity.IncludeInTopMenu,
                MetaDescription = entity.MetaDescription,
                MetaKeywords = entity.MetaKeywords,
                MetaTitle = entity.MetaTitle,
                PageSize = entity.PageSize,
                Published = entity.Published,
                ShowOnHomePage = entity.ShowOnHomePage,
                ParentCategoryId = entity.ParentCategoryId,
                SeName = entity.GetSeName(),
                ShortCountryName = entity.ShortCountryName
            };
            return model;
        }
        public static CountryModel ToModel(this Country entity)
        {
            if (entity == null) return null;
            var model = new CountryModel
            {
                Id = entity.Id,
                Name = entity.Name,
                DisplayOrder = entity.DisplayOrder,
                ShortName = entity.ShortName
            };
            return model;
        }
    }
}
