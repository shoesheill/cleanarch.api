using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Configurations;
using MyProject.Domain.Interfaces;

namespace MyProject.Domain.Repository;

public class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
    }

    public async Task<TResult?> GetByPropertyAsync<TResult>(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().TagWith(typeof(T).Name);

        if (predicate != null)
            query = query.Where(predicate);

        return await (selector != null
            ? query.Select(selector).FirstOrDefaultAsync(cancellationToken)
            : query.Select(x => (TResult)(object)x).FirstOrDefaultAsync(cancellationToken));
    }

    public async Task<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        string? filterPropertyName = null,
        string? filterKeyword = null,
        string? orderByPropertyName = null,
        bool ascending = true,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsNoTracking().TagWith(typeof(T).Name);

        if (predicate != null)
            query = query.Where(predicate);

        if (!string.IsNullOrEmpty(filterKeyword) && !string.IsNullOrEmpty(filterPropertyName))
            query = query.Where(BuildStringFilterExpression(filterPropertyName, filterKeyword));

        if (!string.IsNullOrEmpty(orderByPropertyName))
            query = ApplySorting(query, orderByPropertyName, ascending);

        var resultQuery = selector != null
            ? query.Select(selector)
            : query.Select(x => (TResult)(object)x);

        if (pageNumber.HasValue && pageSize.HasValue)
            resultQuery = resultQuery
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);

        return await resultQuery.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (predicate != null)
            query = query.Where(predicate);

        return await query.CountAsync(cancellationToken);
    }

    public IQueryable<T> ApplyIncludes(
        IQueryable<T> query,
        params Expression<Func<T, object>>[] includes)
    {
        return includes.Aggregate(query, (current, include) => current.Include(include));
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    private static Expression<Func<T, bool>> BuildStringFilterExpression(
        string propertyName,
        string keyword)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);

        if (property.Type != typeof(string))
            throw new InvalidOperationException("Filter can only be applied to string properties.");

        var method = typeof(string).GetMethod("Contains", [typeof(string)])!;
        var value = Expression.Constant(keyword);
        var body = Expression.Call(property, method, value);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static IQueryable<T> ApplySorting(
        IQueryable<T> query,
        string propertyName,
        bool ascending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = ascending ? "OrderBy" : "OrderByDescending";
        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IQueryable<T>)method.Invoke(null, [query, lambda])!;
    }
}