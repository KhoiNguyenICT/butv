using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BUTV.Data;

namespace Butv.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BUTV.Core.Domain.Caching.CacheKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsLive");

                    b.Property<string>("Key");

                    b.HasKey("Id");

                    b.ToTable("CacheKey");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Catalog.AutoSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<int>("DisplayOrder");

                    b.Property<bool>("HasEpisodes")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Link");

                    b.Property<string>("Source");

                    b.HasKey("Id");

                    b.ToTable("AutoSource");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Catalog.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryTemplateId");

                    b.Property<string>("Description");

                    b.Property<bool>("HasEpisodes");

                    b.Property<bool>("IncludeInTopMenu");

                    b.Property<string>("MetaDescription");

                    b.Property<string>("MetaKeywords");

                    b.Property<string>("MetaTitle");

                    b.Property<string>("Name");

                    b.Property<int>("PageSize")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(10);

                    b.Property<int>("ParentCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("PictureId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<bool>("Published")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("ShortCountryName")
                        .HasMaxLength(50);

                    b.Property<bool>("ShowOnHomePage")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Catalog.CategoryTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DisplayOrder");

                    b.Property<string>("Name")
                        .HasMaxLength(400);

                    b.Property<string>("ViewPath")
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.ToTable("CategoryTemplate");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Directory.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DisplayOrder");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("ShortName")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Directory.Tags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Hit");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Media.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AltAttribute");

                    b.Property<bool>("IsNew");

                    b.Property<string>("MimeType");

                    b.Property<byte[]>("PictureBinary");

                    b.Property<string>("SeoFilename");

                    b.Property<string>("TitleAttribute");

                    b.HasKey("Id");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Imdb");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<int>("PictureId");

                    b.HasKey("Id");

                    b.ToTable("Actor");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.Director", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Imdb")
                        .HasMaxLength(400);

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<int>("PictureId");

                    b.HasKey("Id");

                    b.ToTable("Director");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieActor", b =>
                {
                    b.Property<int>("MovieId");

                    b.Property<int>("ActorId");

                    b.Property<string>("Character")
                        .HasMaxLength(100);

                    b.Property<int>("Id");

                    b.HasKey("MovieId", "ActorId");

                    b.HasIndex("ActorId");

                    b.ToTable("MovieActor");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<int>("MovieId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieCategory");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieDirector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DirectorId");

                    b.Property<int>("MovieId");

                    b.HasKey("Id");

                    b.HasIndex("DirectorId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieDirector");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("Country")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CurrentStatus")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("Genres");

                    b.Property<string>("Imdb")
                        .HasMaxLength(200);

                    b.Property<string>("ImdbRate")
                        .HasMaxLength(50);

                    b.Property<string>("Length")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<int>("PictureId");

                    b.Property<bool>("Publish");

                    b.Property<double>("Rate");

                    b.Property<string>("RealName")
                        .HasMaxLength(200);

                    b.Property<DateTime>("ReleasedDate");

                    b.Property<bool>("Status");

                    b.Property<string>("Tags");

                    b.Property<string>("Trailer");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("View");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.SubMovieItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("GoogleUrl");

                    b.Property<DateTime>("GoogleUrlGeneratedDate");

                    b.Property<int>("MovieId");

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<bool>("Publish");

                    b.Property<string>("Quality")
                        .HasMaxLength(100);

                    b.Property<string>("Source")
                        .HasMaxLength(200);

                    b.Property<string>("SourceUrl");

                    b.Property<int>("View");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("SubMovieItem");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Seo.UrlRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EntityId");

                    b.Property<string>("EntityName")
                        .HasMaxLength(400);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Slug")
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.ToTable("UrlRecord");
                });

            modelBuilder.Entity("BUTV.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieActor", b =>
                {
                    b.HasOne("BUTV.Core.Domain.Movie.Actor", "Actor")
                        .WithMany("MovieActors")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BUTV.Core.Domain.Movie.MovieItem", "Movie")
                        .WithMany("MovieActors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieCategory", b =>
                {
                    b.HasOne("BUTV.Core.Domain.Catalog.Category", "Category")
                        .WithMany("MovieCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BUTV.Core.Domain.Movie.MovieItem", "Movie")
                        .WithMany("MovieCategories")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieDirector", b =>
                {
                    b.HasOne("BUTV.Core.Domain.Movie.Director", "Director")
                        .WithMany("MovieDirectors")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BUTV.Core.Domain.Movie.MovieItem", "Movie")
                        .WithMany("MovieDirectors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.MovieItem", b =>
                {
                    b.HasOne("BUTV.Core.Domain.Catalog.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BUTV.Core.Domain.Movie.SubMovieItem", b =>
                {
                    b.HasOne("BUTV.Core.Domain.Movie.MovieItem", "Movie")
                        .WithMany("Subs")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BUTV.Data.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BUTV.Data.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BUTV.Data.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
