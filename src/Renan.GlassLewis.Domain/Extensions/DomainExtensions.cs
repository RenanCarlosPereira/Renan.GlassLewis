using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Domain.Company;

namespace Renan.GlassLewis.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<ICompanyDomainService, CompanyDomainService>();
            return services;
        }
    }
}