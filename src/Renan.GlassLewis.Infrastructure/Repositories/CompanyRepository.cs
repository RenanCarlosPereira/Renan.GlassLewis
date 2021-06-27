using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Infrastructure.DbContexts;

namespace Renan.GlassLewis.Infrastructure.Repositories
{
    internal class CompanyRepository : GenericRepository<CompanyEntity>, ICompanyRepository
    {
        public CompanyRepository(ApplicationContext context) : base(context)
        {
        }
    }
}