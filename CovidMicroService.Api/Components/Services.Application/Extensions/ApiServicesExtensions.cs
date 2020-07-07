using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Services.Application.Extensions
{
    public static class ApiServicesExtensions
    {
        static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        static string origin = Environment.GetEnvironmentVariable("CorsOrigin");

        public static void AddExtentions(this IServiceCollection builder, IConfiguration configuration)
        {
            builder.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
            origin ??= configuration.GetSection("ApiSettings").GetSection("CorsOrigin").Value;
            builder.SetCors(MyAllowSpecificOrigins, origin);
            builder.SetDI();
            builder.SetToken(configuration);
            builder.SetApiVersion();
            builder.SetSwagger();

        }

        public static void AddExtensions(this IApplicationBuilder app, IApiVersionDescriptionProvider versionProvider)
        {
            app.UseCors(MyAllowSpecificOrigins);
            app.SetSwagger(versionProvider);

        }

    }

}
