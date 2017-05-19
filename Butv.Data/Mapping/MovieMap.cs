using BUTV.Core.Domain.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BUTV.Data.Mapping
{
    //public class MovieMap
    //{
    //    public MovieMap(EntityTypeBuilder<MovieItem> entityBuilder)
    //    {
    //        entityBuilder.HasKey(t => t.Id);
    //        entityBuilder.Property(t => t.Name).IsRequired();
    //        entityBuilder.Property(t => t.ISBN).IsRequired();
    //        entityBuilder.Property(t => t.Publisher).IsRequired();
    //        entityBuilder.HasOne(e => e.Author).WithMany(e => e.Books).HasForeignKey(e => e.AuthorId);

    //        entityBuilder.to("Movie");
    //    }
    //}
}
