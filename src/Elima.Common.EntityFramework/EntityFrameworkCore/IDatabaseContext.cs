using Elima.Common.EntityFramework.Uow;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Elima.Common.EntityFramework.EntityFrameworkCore;

public interface IDatabaseContext:IDisposable,ITransactionApi
{

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
