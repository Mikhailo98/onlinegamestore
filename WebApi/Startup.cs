using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Autofac.Extensions.DependencyInjection;
using WebApi.Configuration;
using WebApi.Middleware;
using NLog.Extensions.Logging;
using WebApi.Logging;
using WebApi.Filter;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Identity.Context;
using Microsoft.EntityFrameworkCore;
using Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Identity.Initialize;
using WebApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Serialization;
using WebApi.Infrastucture;

namespace WebApi
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GamesStore", Version = "v1" });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",

                });
            });
            //Configure Authentication schemas

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Configuration["Jwt:Issuer"],
                     ValidAudience = Configuration["Jwt:Issuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                 };
             });

            //Add Identity context (you can init new empty project with "Individual user accounts" in order to create database and register users)

            string connstring = Configuration.GetConnectionString("AspIdentityDbConnection");
            services.AddDbContext<AppIdentityDbContext>();

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddScoped<PerformanceLogging>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();

         
            services.AddAutoMapper();

            var builder = new ContainerBuilder();
            builder.RegisterModule(new DependencyServiceModule());
            builder.RegisterModule(new AutoMapperModule());
            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CustomMiddleware>();

            app.UseMiddleware<CustomErrorMiddleware>();



            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());

            //Enable Authentication
            app.UseAuthentication();

            app.UseMvc();


        }
    }
}
