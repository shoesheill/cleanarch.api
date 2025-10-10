using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task BeginTransactionAsync();
    Task CommitAsync(CancellationToken cancellationToken = default);

    void Rollback();
    // TODO: add other repositories here
    // IRepository? Repository {get;} 
}