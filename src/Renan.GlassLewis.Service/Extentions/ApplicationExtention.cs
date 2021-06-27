using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Service.CompaniesUseCases;

namespace Renan.GlassLewis.Service.Extentions
{
    public static class ApplicationExtention
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<ICompanyUseCase, CompanyUseCase>();
            return services;
        }
    }
}