using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        ValueTask<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        IAsyncEnumerable<T> GetAllAsync();

        IAsyncEnumerable<T> FindAsync(Expression<Func<T, bool>> expression);

        ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        ValueTask RemoveAsync(T entity, CancellationToken cancellationToken = default);

        ValueTask RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}