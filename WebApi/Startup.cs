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

namespace WebApi
{


    //public class DomainProfile : Profile
    //{
    //    public DomainProfile()
    //    {
    //        Mapper.Initialize(cfg =>
    //        {
    //            cfg.AddProfile(new AutoMapperConfig());
    //        });
    //    }
    //}

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

           
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); // Give me the 2.1 behaviors

            var builder = new ContainerBuilder();
            builder.RegisterModule(new DependencyServiceModule());
            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);

            // Register the Swagger generator, defining 1 or more Swagger documents

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddDebug();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseMvc();

            app.Run(async (context) =>
            {
                 context.Response.Redirect("swagger");
            });
        }
    }
}