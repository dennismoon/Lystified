using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Lystified
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration accesstor.
        /// </summary>
        public IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //#if DEBUG
            //            services.AddCors(options =>
            //            {
            //                options.AddPolicy("AllowSpecificOrigin",
            //                    builder => builder.WithOrigins("http://localhost:5003", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
            //                );
            //            });
            //#endif        

            // Adds services required for using options.
            services.AddOptions();

            //// Register the IConfiguration instances which options bind against.
            //services.Configure<ClientOptions>(Configuration.GetSection("ClientOptions"));

            //services.Configure<SsoOptions>(Configuration.GetSection("SsoOptions"));

            // Add logging services
            services.AddLogging();

            // Needed for NLog.Web
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //var config = new AutoMapper.MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new AutoMapperProfileConfiguration());
            //    cfg.AddGlobalIgnore("CreateUserId");
            //    cfg.AddGlobalIgnore("CreateUtcTimestamp");
            //    cfg.AddGlobalIgnore("UpdateUserId");
            //    cfg.AddGlobalIgnore("UpdateUtcTimestamp");
            //});

            //var mapper = config.CreateMapper();

            //config.AssertConfigurationIsValid();

            //services.AddSingleton(mapper);

            // Add MVC framework services.
            services.AddMvc();
            //services.AddMvc(options =>
            //    options.Filters.Add(typeof(ValidateUrlPathAndQueryFilterAttribute))
            //);

            //services.AddScoped<CustomHeaders, CustomHeaders>();

            //var connectionString = Configuration["ConnectionStrings:DefaultConnectionString"];

            //services.AddDbContext<RolloverContext>(o => o.UseSqlServer(connectionString,
            //    sqlServerOptionsAction: options =>
            //    {
            //        options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            //        options.UseRowNumberForPaging();
            //    }
            //));

            //// Register repositories once per request
            //services.AddScoped<IRolloverRepository, RolloverRepository>();

            //services.AddScoped<IWebApiClient, WebApiClient>();

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add various loggers
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            // Add NLog to .NET Core
            loggerFactory.AddNLog();

            // Enable ASP.NET Core features (NLog.web)
            app.AddNLogWeb();

            //// Configure NLog
            //var logLevel = Configuration["Logging:LogLevel:Default"];
            //var logFileName = Configuration["NLogConfig:FileName"];
            //var nlogConfiguration = new NLogConfig(logLevel, logFileName);
            //nlogConfiguration.Configure();

            // Configure developer exception handling
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseStatusCodePages();
            }

            //#if DEBUG
            //            app.UseCors("AllowSpecificOrigin");
            //#endif

            // Support loading static files
            app.UseStaticFiles();

            // Use default MVC routing rules
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" });
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUi();
        }
    }

}
