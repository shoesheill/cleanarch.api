using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity, CancellationToken cancellationToken = default);
    void Remove(T entity, CancellationToken cancellationToken = default);

    Task<TResult?> GetByPropertyAsync<TResult>(
        Expression<Func<T, bool>> predicate = null,
        Expression<Func<T, TResult>> selector = null,
        CancellationToken cancellationToken = default);

    Task<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<T, bool>> predicate = null,
        Expression<Func<T, TResult>> selector = null,
        string filterPropertyName = null,
        string filterKeyword = null,
        string orderByPropertyName = null,
        bool ascending = true,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<T, bool>> predicate = null,
        CancellationToken cancellationToken = default);

    IQueryable<T> ApplyIncludes(
        IQueryable<T> query,
        params Expression<Func<T, object>>[] includes);
}