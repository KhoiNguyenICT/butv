
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using BUTV.Core.Domain.Movie;
using BUTV.Core.Domain.Directory;
using BUTV.Core.Domain.Catalog;
using BUTV.Core.Domain.Media;
using BUTV.Core.Domain.Seo;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BUTV.Core.Domain.Caching;

namespace BUTV.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MovieItem>()
                .ToTable("Movie");
            modelBuilder.Entity<SubMovieItem>()
              .ToTable("SubMovieItem")
              .HasOne(b => b.Movie)
              .WithMany(b => b.Subs).HasForeignKey(b => b.MovieId);

            modelBuilder.Entity<Category>()
                .ToTable("Category");
                  //.HasMany(b => b.Movies);

            modelBuilder.Entity<MovieCategory>()
                .ToTable("MovieCategory")
              .HasOne(pt => pt.Movie)
                .WithMany(p => p.MovieCategories)
                .HasForeignKey(pt => pt.MovieId);

            modelBuilder.Entity<MovieCategory>()
               .ToTable("MovieCategory")
             .HasOne(pt => pt.Category)
               .WithMany(p => p.MovieCategories)
               .HasForeignKey(pt => pt.CategoryId);

            modelBuilder.Entity<CategoryTemplate>()
               .ToTable("CategoryTemplate");

            modelBuilder.Entity<Category>()
              .Property(b => b.ShowOnHomePage).HasDefaultValue(false);
            modelBuilder.Entity<Category>()
            .Property(b => b.Published).HasDefaultValue(true);
            modelBuilder.Entity<Category>()
            .Property(b => b.ParentCategoryId).HasDefaultValue(0);
            modelBuilder.Entity<Category>()
            .Property(b => b.PictureId).HasDefaultValue(0);
            modelBuilder.Entity<Category>()
            .Property(b => b.PageSize).HasDefaultValue(10);


            modelBuilder.Entity<Actor>()
                .ToTable("Actor");

            modelBuilder.Entity<MovieActor>()
            .HasKey(t => new { t.MovieId, t.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(pt => pt.Movie)
                .WithMany(p => p.MovieActors)
                .HasForeignKey(pt => pt.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(pt => pt.Actor)
                .WithMany(t => t.MovieActors)
                .HasForeignKey(pt => pt.ActorId);

            modelBuilder.Entity<Director>()
                .ToTable("Director");
              


            modelBuilder.Entity<MovieDirector>()
              .HasOne(pt => pt.Movie)
              .WithMany(p => p.MovieDirectors)
              .HasForeignKey(pt => pt.MovieId);

            modelBuilder.Entity<MovieDirector>()
                .HasOne(pt => pt.Director)
                .WithMany(t => t.MovieDirectors)
                .HasForeignKey(pt => pt.DirectorId);

            modelBuilder.Entity<Country>().
                ToTable("Country");
            modelBuilder.Entity<Genre>().
              ToTable("Genre");
            //.Property<int>("MovieId");  // shadow

            modelBuilder.Entity<Genre>();
            // .HasOne(b => b.Movie)
            // .WithMany(b => b.Genres)
            //.HasForeignKey("MovieId"); // one - many 
            modelBuilder.Entity<Picture>()
               .ToTable("Picture");
            modelBuilder.Entity<AutoSource>()
               .ToTable("AutoSource");
            modelBuilder.Entity<AutoSource>()
       .Property(b => b.HasEpisodes).HasDefaultValue(false);

            modelBuilder.Entity<UrlRecord>()
               .ToTable("UrlRecord");

            modelBuilder.Entity<Tags>()
                .ToTable("Tags");

            modelBuilder.Entity<CacheKey>()
             .ToTable("CacheKey");

           
           
        }


    }
    public class ApplicationDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=server;Database=BUTV;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
