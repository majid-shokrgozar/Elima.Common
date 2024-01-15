using Autofac;
using PersonalWeb.Shared.DependencyInjection;

namespace PersonalWeb.Shared.Modules;

public abstract class ModuleBase<T> : Module
    where T : class
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterAutofacAutoServices(typeof(T).Assembly);
        builder.RegisterAutofacCommandQueryServices(typeof(T).Assembly);

        LoadService(builder);
    }

    public abstract void LoadService(ContainerBuilder builder);
}
