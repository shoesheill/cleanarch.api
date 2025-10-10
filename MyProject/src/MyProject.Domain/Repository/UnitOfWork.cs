using Microsoft.EntityFrameworkCore.Storage;
using MyProject.Domain.Configurations;
using MyProject.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Domain.Repository;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];

    private IDbContextTransaction? _transaction;

    // TODO: repository implementation
    // init repository on constructor
    // public IRepository Repository =>
    //     repository ?? new Repository(dbContext);

    public IGenericRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (!_repositories.TryGetValue(type, out var repo))
        {
            var repositoryInstance = new GenericRepository<T>(dbContext);
            _repositories[type] = repositoryInstance;
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        dbContext.Dispose();
    }
}