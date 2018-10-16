﻿using System;
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

namespace WebApi
{



    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GamesStore", Version = "v1" });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });
            //Configure Authentication schemas
            services.AddAuthentication()
                //Add Bearer Authentication support
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


            // services.AddDbContext<AppIdentityDbContext>();

            //Configure Identity
            //services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

            services.AddScoped<PerformanceLogging>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
           
            if (env.IsProduction())
            {
                app.UseMiddleware<CustomErrorMiddleware>();

            }




            app.UseStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
            app.UseAuthentication();

            app.Run(async (context) =>
            {
                context.Response.Redirect("swagger");
            });
        }
    }
}
