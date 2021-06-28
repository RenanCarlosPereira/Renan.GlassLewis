using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renan.GlassLewis.Domain.Company;

namespace Renan.GlassLewis.Infrastructure.EntityTypeConfigurations
{
    public class CompanyTypeConfiguration : IEntityTypeConfiguration<CompanyEntity>
    {
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Isin);
        }
    }
}