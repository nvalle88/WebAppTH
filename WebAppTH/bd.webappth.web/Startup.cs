using bd.log.guardar.Inicializar;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Interfaces;
using bd.webappth.servicios.Servicios;
using bd.webappth.web.Models;
using bd.webappth.web.Services;
using EnviarCorreo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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

            


            var appSettings = Configuration.GetSection("AppSettings");
            //services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            
            services.AddDistributedMemoryCache();


            Session.TiempoSessionMinutos = Convert.ToInt32( Configuration.GetSection("TiempoSessionMinutos").Value);

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(Session.TiempoSessionMinutos);
                //options.Cookie.HttpOnly = true;
            });

            services.AddDataProtection()
           .UseCryptographicAlgorithms(
           new AuthenticatedEncryptionSettings()
           {
               EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
               ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
           });

            services.AddDataProtection()
            .SetDefaultKeyLifetime
            (TimeSpan.FromDays
             (Convert.ToInt32
              (Configuration.GetSection("DiasValidosClaveEncriptada").Value)
             )
            );

            services.AddSingleton<IApiServicio, ApiServicio>();
            services.AddSingleton<IMenuServicio, MenuServicio>();
            services.AddSingleton<IConstantesNomina, ConstanteNominaServicio>();
            services.AddSingleton<IFuncionesNomina, FuncionesNominaServicio>();
            services.AddSingleton<IConjuntoNomina, ConjuntoNominaServicio>();

            services.AddSingleton<IAuthorizationHandler, RolesHandler>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUploadFileService, UploadFileService>();



            services.AddMvc().Services.AddAuthorization(options=>
            {
                options.AddPolicy("EstaAutorizado",
                                  policy => policy.Requirements.Add(new RolesRequirement()));
            });


            WebApp.BaseAddressWebAppLogin = Configuration.GetSection("HostWebAppLogin").Value;

            ///Configuración del servidor de roportes
            ///--------------------------------------
            ReportConfig.DefaultNetworkCredentials = Convert.ToBoolean(Configuration.GetSection("DefaultNetworkCredentials").Value);

            if (!ReportConfig.DefaultNetworkCredentials)
            {
                ReportConfig.UserName = Configuration.GetSection("UserNameReport").Value;
                ReportConfig.Password = Configuration.GetSection("PasswordReport").Value;
                ReportConfig.CustomDomain = Configuration.GetSection("CustomDomain").Value;
            }
            ReportConfig.ReportServerUrl = Configuration.GetSection("ReportServerUrl").Value;
            ReportConfig.ReportFolderPath = Configuration.GetSection("ReportFolderPath").Value;
            ReportConfig.CompletePath = string.Format("{0}{1}", ReportConfig.ReportServerUrl, ReportConfig.ReportFolderPath);

            ///--------------------------------------


            WebApp.NombreAplicacion = Configuration.GetSection("NombreAplicacion").Value;

            WebApp.BaseAddress = Configuration.GetSection("HostServiciosTalentoHumano").Value;
            WebApp.BaseAddressSeguridad= Configuration.GetSection("HostServicioSeguridad").Value;
            WebApp.BaseAddressRM= Configuration.GetSection("HostServiciosRecursosMateriales").Value;
            AppGuardarLog.BaseAddress= Configuration.GetSection("HostServicioLog").Value;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {


            var defaultCulture = new CultureInfo("es-ec");
            defaultCulture.NumberFormat.NumberDecimalSeparator = ".";
            defaultCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            //defaultCulture.DateTimeFormat = DateTimeFormatInfo.CurrentInfo;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture },
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = new List<IRequestCultureProvider> { }
            });

            //app.UseExceptionHandler("/Home/Error");
           
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();


            //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            //    .CreateScope())
            //    {
                    
            //        //serviceScope.ServiceProvider.GetService<LogDbContext>()
            //        //         .Database.Migrate();

            //       // serviceScope.ServiceProvider.GetService<InicializacionServico>().InicializacionAsync();
            //    }

            //}
            //else
            //{
                
            //}

            app.UseStaticFiles();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                LoginPath = new PathString("/"),
                AccessDeniedPath = new PathString("/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieName = "ASPTest",
                ExpireTimeSpan = new TimeSpan(1, 0, 0), //1 hour
                DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"c:\shared-auth-ticket-keys\"))
            });
            //app.UseIdentity();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Login}/{id?}");
            });
        }
    }
}
