using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Domain.Repositories;
using Renan.GlassLewis.Infrastructure.DbContexts;
using Renan.GlassLewis.Infrastructure.Repositories;

namespace Renan.GlassLewis.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(connectionString));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            return services;
        }
    }
}