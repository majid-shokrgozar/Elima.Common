using System;
using System.Threading.Tasks;

namespace Elima.Common.EntityFramework.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : IEfCoreDbContext
{
    [Obsolete("Use GetDbContextAsync method.")]
    TDbContext GetDbContext();

    Task<TDbContext> GetDbContextAsync();
}
