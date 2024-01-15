using Elima.Common.Domain.Entities;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.Repositories;

public interface IEfCoreBulkOperationProvider
{
    Task InsertManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity;


    Task UpdateManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity;


    Task DeleteManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity;
}
