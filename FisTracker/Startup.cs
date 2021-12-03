using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FisTracker
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FisTracker", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "fck-cors",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:8081", "http://localhost:8081").AllowAnyHeader().AllowAnyMethod();
                    });
            });

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Data.AppDbContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
            services.AddDistributedMemoryCache();
            services.AddTransient<Data.AppDbContext>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication("Session").AddScheme<SimpleAuthenticationOptions, SimpleAuthentication>("Session", null);
            services.AddSingleton<AuthorizationHandler<SimpleAuthorizationRequirement>, SimpleAuthorization>();

            //todo: fix policy settings
            services.AddAuthorization(options =>
             {
                 options.AddPolicy("Basic", policy =>
                 {
                     policy.AddAuthenticationSchemes("Session")
                         .Requirements.Add(new SimpleAuthorizationRequirement());
                 });
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerProvider log)
        {
            app.ConfigureExceptionHandler(log.CreateLogger("ExceptionHandler"));

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FisTracker v1"));
            }

            app.UseSession();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("fck-cors"); // cors has to be after routing and before auth

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
