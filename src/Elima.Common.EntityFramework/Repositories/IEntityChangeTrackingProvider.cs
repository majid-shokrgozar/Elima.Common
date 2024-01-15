using System;

namespace Elima.Common.EntityFramework.Repositories;

public interface IEntityChangeTrackingProvider
{
    bool? Enabled { get; }

    IDisposable Change(bool? enabled);
}
