using bd.webappth.servicios.Interfaces;
using bd.webappth.servicios.Servicios;
using bd.webappth.web.Models;
using bd.webappth.web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace bd.webappth.web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser,IdentityRole>(options=> 
            {
                
                options.Cookies.ApplicationCookie.LoginPath = new PathString("/Login/Index");
                options.Cookies.ApplicationCookie.AccessDeniedPath = new PathString("/Homes/AccesoDenegado");
                
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromHours(2);

            })
           
            .AddDefaultTokenProviders();
            // Add framework services.
            services.AddMvc(
         
            );
           


            services.AddSingleton<IApiServicio, ApiServicio>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("EstaAutorizado",
                                  policy => policy.Requirements.Add(new RolesRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, RolesHandler>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            await InicializarWebApp.InicializarWeb("SwTalentoHumano", new Uri("http://localhost:4000"));
            await InicializarWebApp.InicializarWebRecursosMateriales("SwRecursosMateriales", new Uri("http://localhost:4000"));
            await InicializarWebApp.InicializarLogEntry("LogWebService", new Uri("http://localhost:4000"));
            //await InicializarWebApp.InicializarLogEntry("LogWebService", new Uri("http://192.168.100.21:8081"));
            //await InicializarWebApp.InicializarWeb("SwTalentoHumano", new Uri("http://192.168.100.21:8081"));
            //await InicializarWebApp.InicializarWebRecursosMateriales("SwRecursosMateriales", new Uri("http://192.168.100.21:8081"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseExceptionHandler("/Home/Error");
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
#pragma warning disable CS0612 // El tipo o el miembro están obsoletos
            app.UseApplicationInsightsRequestTelemetry();
#pragma warning restore CS0612 // El tipo o el miembro están obsoletos

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                //.WriteTo.RollingFile("log-{Date}.txt")
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();


            loggerFactory.AddSerilog(logger);
            Log.Logger = logger;
            loggerFactory.AddSerilog();

           



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();


                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
                {
                    
                    //serviceScope.ServiceProvider.GetService<LogDbContext>()
                    //         .Database.Migrate();

                   // serviceScope.ServiceProvider.GetService<InicializacionServico>().InicializacionAsync();
                }

            }
            else
            {
                
            }

           

            app.UseStaticFiles();
            app.UseCookieAuthentication();
            app.UseIdentity();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Homes}/{action=Index}/{id?}");
            });
        }
    }
}
