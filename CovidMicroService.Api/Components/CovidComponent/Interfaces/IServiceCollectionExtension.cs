using ComponentsLib;
using ComponentsLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Entities;

namespace CovidComponent.Interfaces
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCovidComponentConnector(this IServiceCollection services)
        {
            services.AddTransient<ICovidActions, CovidActions>();
            services.AddTransient<IMongoGeneric<Covid19>, MongoGeneric<Covid19>>();
            services.AddTransient<IValidationComponent, ValidationComponent>();

            return services;
        }
    }
}