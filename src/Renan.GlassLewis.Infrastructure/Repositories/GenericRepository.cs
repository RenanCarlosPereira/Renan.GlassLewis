using Microsoft.EntityFrameworkCore;
using Renan.GlassLewis.Domain.Repositories;
using Renan.GlassLewis.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Infrastructure.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationContext Context;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(ApplicationContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public ValueTask<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return DbSet.FindAsync(id, cancellationToken);
        }

        public IAsyncEnumerable<T> GetAllAsync()
        {
            return DbSet.AsAsyncEnumerable();
        }

        public IAsyncEnumerable<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression).AsAsyncEnumerable();
        }

        public async ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return (await DbSet.AddAsync(entity, cancellationToken)).Entity;
        }

        public ValueTask<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var entry = DbSet.Update(entity);
            return ValueTask.FromResult(entry.Entity);
        }

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return DbSet.AddRangeAsync(entities, cancellationToken);
        }

        public ValueTask RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            return ValueTask.CompletedTask;
        }

        public ValueTask RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);
            return ValueTask.CompletedTask;
        }
    }
}