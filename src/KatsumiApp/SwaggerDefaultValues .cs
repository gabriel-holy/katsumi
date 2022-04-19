using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace KatsumiApp
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Katsumi Social Media API",
                Version = description.ApiVersion.ToString(),
                Description = "This API was built in Vertical Slice Architectural Pattern to provide and manage simple Katsumi Social Media informations",
                Contact = new OpenApiContact() { Name = "Gabriel Rio Campo Oliveira", Email = "gabriel.holy@hotmail.com" },
                License = new OpenApiLicense() { Name = "Open Source", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
            {
                var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                    parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }

    public class OperationsOrderingFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            Dictionary<KeyValuePair<string, OpenApiPathItem>, int> paths = new Dictionary<KeyValuePair<string, OpenApiPathItem>, int>();
            foreach (var path in openApiDoc.Paths)
            {
                SwaggerOrderAttribute orderAttribute = 
                    context.ApiDescriptions
                    .FirstOrDefault(x => x.RelativePath.Replace("/", string.Empty)
                    .Equals(path.Key.Replace("/", string.Empty), StringComparison.InvariantCultureIgnoreCase))? 
                    .ActionDescriptor?
                    .EndpointMetadata?
                    .FirstOrDefault(x => x is SwaggerOrderAttribute) as SwaggerOrderAttribute;

                int order = orderAttribute?.Order ?? 99;

                paths.Add(path, order);
            }

            var orderedPaths = paths.OrderBy(x => x.Value).ToList();
            openApiDoc.Paths.Clear();
            orderedPaths.ForEach(x => openApiDoc.Paths.Add(x.Key.Key, x.Key.Value));
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerOrderAttribute : Attribute
    {
        public int Order { get; }

        public SwaggerOrderAttribute(int order)
        {
            this.Order = order;
        }
    }
}
