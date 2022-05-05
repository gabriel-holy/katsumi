using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using MediatR;
using FluentValidation.AspNetCore;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using KatsumiApp.V1.Infrastructure;
using Serilog;
using KatsumiApp.V1.Data.EntityFramework.Contexts;

namespace KatsumiApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var currentAssembly = GetType().Assembly;

            services.AddControllers();
            services.AddAutoMapper(currentAssembly);
            services.AddMediatR(currentAssembly);
            services.AddDependencyInjectionModules(currentAssembly);
            services.AddControllers(options => options.Filters.Add<FluentValidationExceptionFilter>())
                    .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(currentAssembly));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ThrowFluentValidationExceptionBehavior<,>));
            services.AddMvc();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 1);
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.CustomSchemaIds(type => $"{type.Name} ({System.Guid.NewGuid()})");
                options.DocumentFilter<OperationsOrderingFilter>();
            });
        }

        public void Configure
        (
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IMapper mapper,
            IApiVersionDescriptionProvider provider
        )
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseCors(builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            //app.Seed<FollowingContext>();
            //app.Seed<UserProfileContext>();

            app.UseSwagger();

            app.UseSwaggerUI(swaggerOptions =>
            {
                swaggerOptions.RoutePrefix = string.Empty;

                swaggerOptions.InjectStylesheet("/swagger-ui/SwaggerDark.css");

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    swaggerOptions.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "$";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}