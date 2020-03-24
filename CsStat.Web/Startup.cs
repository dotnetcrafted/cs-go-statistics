using AutoMapper;
using BusinessFacade.Repositories;
using BusinessFacade.Repositories.Implementations;
using CsStat.LogApi;
using CsStat.LogApi.Interfaces;
using CsStat.StrapiApi;
using CsStat.Web.Helpers;
using DataService;
using DataService.Interfaces;
using ErrorLogger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerQueries.Source;

namespace CsStat.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddResponseCaching();
            services.AddControllersWithViews();
            services.AddRouting(options => {
                options.LowercaseUrls = true;
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton(Configuration);
            services.AddSingleton<HtmlHelperExtensions>();

            services.AddTransient<ICsLogsApi, CsLogsApi>();
            services.AddTransient<IMongoRepositoryFactory, MongoRepositoryFactory>();
            services.AddTransient<IConnectionStringFactory, ConnectionStringFactory>();
            services.AddTransient<ILogsRepository, LogsRepository>();
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
            services.AddTransient<ISteamApi, SteamApi>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IUsefulLinkRepository, UsefulLinkRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IStrapiApi, StrapiApi.StrapiApi>();
            services.AddTransient<IQueryConnection, QueryConnection>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseSession();

            app.UseAuthorization();
            app.UseResponseCaching();

            app.UseMvc(RouteConfig.RegisterRoutes);
        }
    }
}
