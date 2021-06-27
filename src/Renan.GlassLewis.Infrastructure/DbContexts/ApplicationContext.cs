using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renan.GlassLewis.Domain.Company;

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

    internal class ApplicationContextContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer();

            return new ApplicationContext(optionsBuilder.Options);
        }
    }

    public class CompanyTypeConfiguration : IEntityTypeConfiguration<CompanyEntity>
    {
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Isin);
        }
    }
}