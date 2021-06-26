using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Renan.GlassLewis.Infrastructure.DbContexts
{
    internal class GlassLewisDbContext : DbContext
    {
        public GlassLewisDbContext(DbContextOptions<GlassLewisDbContext> options) : base(options)
        {
        }
    }

    internal class BloggingContextFactory : IDesignTimeDbContextFactory<GlassLewisDbContext>
    {
        public GlassLewisDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GlassLewisDbContext>();
            optionsBuilder.UseSqlServer();

            return new GlassLewisDbContext(optionsBuilder.Options);
        }
    }
}