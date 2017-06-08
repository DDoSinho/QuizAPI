using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Model.Identity;
using QuizAPI.Model;
using QuizAPI.Services;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using QuizAPI.Mapping;

namespace QuizAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
           .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:30795")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            services.AddSingleton<IMapper>(MapperConfig.Configure());

            services.AddDbContext<QuizDbContext>(options => options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=QuizAPI;Trusted_Connection=True;"));

            services.AddIdentity<QuizUser, IdentityRole>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
            })
            .AddEntityFrameworkStores<QuizDbContext>();

            services.AddScoped<QuizDbContext>();
            services.AddScoped<QuestionManager>();
            services.AddTransient<IQuizService, QuizService>();

            services.AddMvc();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10000);
                options.CookieHttpOnly = true;
            });

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new Info { Title = "Quiz API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowSpecificOrigin");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseIdentity();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API v1");
            });
        }
    }
}