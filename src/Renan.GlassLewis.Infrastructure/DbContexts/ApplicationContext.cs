using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Infrastructure.EntityTypeConfigurations;

namespace Renan.GlassLewis.Infrastructure.DbContexts
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<CompanyEntity> Companies { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}