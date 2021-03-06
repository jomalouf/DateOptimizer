using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using dateOptimizer.domain.services;
using Microsoft.EntityFrameworkCore;
using dateOptimizer.data;
using Swashbuckle.AspNetCore.Swagger;
using dateOptimizer.domain.contracts;

namespace dateOptimizer.web
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
            services.AddDbContext<DateOptimizerContext>(options => options.UseNpgsql("User Id=postgres;Password=jubjub67;Host=localhost;Port=5432;Database=dateOptimizer"));
            services.AddTransient<IDateService, DateService>();
            services.AddTransient<IRepository, DateOptimizerRepository>();


            // Stay at bottom
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Date Optimizer");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            bool seedOrNot = false;
            if (seedOrNot)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    // Adding test Client
                    DateOptimizerContext context = (DateOptimizerContext)scope.ServiceProvider.GetService<DateOptimizerContext>();
                    DateOptimizerRepository _repo = (DateOptimizerRepository)scope.ServiceProvider.GetService<IRepository>(); ;
                    _repo.SeedDatabaseAppraisalPercentages();

                }
            }

            seedOrNot = true;
            if (seedOrNot)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    // Adding test Client
                    DateOptimizerContext context = (DateOptimizerContext)scope.ServiceProvider.GetService<DateOptimizerContext>();
                    DateOptimizerRepository _repo = (DateOptimizerRepository)scope.ServiceProvider.GetService<IRepository>(); ;
                    _repo.SeedDatabaseCountyInfo();

                }
            }
        }
    }
}