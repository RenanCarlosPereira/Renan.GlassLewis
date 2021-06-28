using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Renan.GlassLewis.Infrastructure.DbContexts
{
    internal class ApplicationContextContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer();

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}