using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Infrastructure.DbContexts;

namespace Renan.GlassLewis.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<GlassLewisDbContext>(
                options => options.UseSqlServer(connectionString));
            return services;
        }
    }
}