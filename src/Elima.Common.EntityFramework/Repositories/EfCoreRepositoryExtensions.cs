﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Elima.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elima.Common.EntityFramework.Repositories;

public static class EfCoreRepositoryExtensions
{
    [Obsolete("Use GetDbContextAsync method.")]
    public static DbContext GetDbContext<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToEfCoreRepository().DbContext;
    }

    public static Task<DbContext> GetDbContextAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToEfCoreRepository().GetDbContextAsync();
    }

    [Obsolete("Use GetDbSetAsync method.")]
    public static DbSet<TEntity> GetDbSet<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToEfCoreRepository().DbSet;
    }

    public static Task<DbSet<TEntity>> GetDbSetAsync<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        return repository.ToEfCoreRepository().GetDbSetAsync();
    }

    public static IEfCoreRepository<TEntity> ToEfCoreRepository<TEntity>(this IReadOnlyBasicRepository<TEntity> repository)
        where TEntity : class, IEntity
    {
        if (repository is IEfCoreRepository<TEntity> efCoreRepository)
        {
            return efCoreRepository;
        }

        throw new ArgumentException("Given repository does not implement " + typeof(IEfCoreRepository<TEntity>).AssemblyQualifiedName, nameof(repository));
    }

    public static IQueryable<TEntity> AsNoTrackingIf<TEntity>(this IQueryable<TEntity> queryable, bool condition)
        where TEntity : class, IEntity
    {
        return condition ? queryable.AsNoTracking() : queryable;
    }
}
