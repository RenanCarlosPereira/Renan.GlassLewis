using System;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();

        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}