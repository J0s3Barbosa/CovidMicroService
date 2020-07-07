
using CovidComponent;
using CovidComponent.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Interfaces;
using Services.Application.Logic;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Infra.Repository;

namespace Services.Application.Extensions
{
    public static class MappingExtensions
    {
        public static void SetDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericAsync<>), typeof(RepositoryGenericAsync<>));
            services.AddScoped<ICovid19ServiceLogic, Covid19ServiceLogic>();
            services.AddScoped<ICovid19Logic, Covid19Logic>();
            services.AddCovidComponentConnector();


        }


    }
}
