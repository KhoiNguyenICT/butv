using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BUTV.Data;
using BUTV.Services.Message;
using BUTV.Services.Movie;
using BUTV.Services.Media;
using BUTV.Services.Catalog;
using BUTV.Services.Seo;
using BUTV.Factories;
using BUTV.Services.Directory;
using BUTV.Services.Infrastructure;
using BUTV.Services.Caching;
using WebMarkupMin.AspNetCore1;

namespace BUTV
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddMemoryCache();
            
            services.AddMvc();

            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddTransient<ICacheService, CacheService>();  // maintain cache state in sql db


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IMovieCategoryService, MovieCategoryService>();
            services.AddTransient<ISubMovieService, SubMovieService>();
            services.AddTransient<IPictureService, PictureService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUrlRecordService, UrlRecordService>();
            services.AddTransient<ICategoryTemplateService, CategoryTemplateService>();
            services.AddTransient<ICountryService, CountryService>();

            services.AddTransient<IMovieActorService, MovieActorService>();
            services.AddTransient<IActorService, ActorService>();
            services.AddTransient<IDirectorService, DirectorService>();
            services.AddTransient<ITagService, TagService>();
            
            //factories
            services.AddTransient<ICategoryModelFactory, CategoryModelFactory>();
            services.AddTransient<IMovieModelFactory, MovieModelFactory>();

            services.AddWebMarkupMin(
        options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = true;
            options.AllowCompressionInDevelopmentEnvironment = true;
        })
        .AddHtmlMinification(
            options =>
            {
                options.MinificationSettings.RemoveRedundantAttributes = true;
                options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
            })
        .AddHttpCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseWebMarkupMin();
            }
            EngineContext.Instance = app.ApplicationServices;
            
            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "HomePage",
                    template: "",
                     defaults: new { controller = "Home", action = "Index" });

                //routes.MapRoute(
                //  name: "default2",
                //  template: "{id?}",
                //   defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                  name: "moviedetail",
                  template: "phim/{sename}",
                  defaults: new { controller = "Movie", action = "Details" });

                routes.MapRoute(
                name: "category",
                template: "{sename}",
                defaults: new { controller = "Category", action = "Category" });

                routes.MapRoute(
               name: "SearchAutoComplete",
               template: "movie/searchtermautocomplete",
               defaults: new { controller = "Movie", action = "SearchTermAutoComplete" });

               
            });
        }
    }
}