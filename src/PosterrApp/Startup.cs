using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using MediatR;
using FluentValidation.AspNetCore;
using PosterrApp.Infrastructure;
using System;
using Microsoft.OpenApi.Models;

namespace PosterrApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var currentAssembly = GetType().Assembly;
            services.AddAutoMapper(currentAssembly);
            services.AddMediatR(currentAssembly);
            services.AddDependencyInjectionModules(currentAssembly);
            services.AddControllers(options => options.Filters.Add<FluentValidationExceptionFilter>())
                    .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(currentAssembly));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ThrowFluentValidationExceptionBehavior<,>));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Posterr MVP API",
                    Version = "v1",
                    Description = "This API has the role to provide and manage Homepage and Userpage informations for Posterr Web Front-end",
                    Contact = new OpenApiContact
                    {
                        Name = "Gabriel Rio Campo Oliveira",
                        Email = "gabriel.holy@hotmail.com",
                    },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            app.Seed<Data.ProductContext>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Posterr MVP API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
