using Microsoft.AspNetCore.Builder;
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
        private const string DefaultConnectionString =
            "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";

        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(connectionString ?? DefaultConnectionString));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            return services;
        }

        public static void EnsureMigrationOfContext(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
            context.Database.Migrate();
        }
    }
}