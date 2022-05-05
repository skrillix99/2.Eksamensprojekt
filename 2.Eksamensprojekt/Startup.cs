using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2.Eksamensprojekt.Services.Interfaces;
using _2.Eksamensprojekt.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _2.Eksamensprojekt
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
            services.AddRazorPages();
            services.AddSingleton<ILogIndService, LogIndService>();
            services.AddSingleton<ILedigeLokalerService, LedigeLokalerService>();


            // Marcus
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential
                //cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(cookieOptions =>
                {
                    cookieOptions.LoginPath = "/LogInd/LogInd";
                });
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                // mapper der er låst uden logind. Bliver redirected til logind page hvis man prøver.
                options.Conventions.AuthorizeFolder("/StuderendePages");
                options.Conventions.AuthorizeFolder("/UnderviserPages");
                options.Conventions.AuthorizeFolder("/AdministrationPages");
                //options.Conventions.AuthorizeFolder("/Shared");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // sender hen til en AccessDenied side hvis man prøver at komme ind på en side man ikke må.
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            services.AddSingleton<IAdministrationService, AdministrationService>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
