using Autofac;
using Elima.Common.Application.MediatR.Commands;
using Elima.Common.Application.MediatR.Queries;
using System.Reflection;

namespace PersonalWeb.Shared.DependencyInjection;

public static class DependencyInjectionExtentions
{
    public static IServiceCollection RegisterAutoTransientServices(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<ITransientService>())
                 .AsImplementedInterfaces()
                 .WithTransientLifetime());

        return services;
    }

    public static IServiceCollection RegisterAutoTransientServices(this IServiceCollection services, Assembly assembly, params Assembly[] assemblies)
    {
        return RegisterAutoTransientServices(services, [assembly, .. assemblies]);
    }
    public static IServiceCollection RegisterAutoScopedServices(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
                 .AsImplementedInterfaces()
                 .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection RegisterAutoScopedServices(this IServiceCollection services, Assembly assembly, params Assembly[] assemblies)
    {
        return RegisterAutoScopedServices(services,[assembly, .. assemblies]);
    }

    public static IServiceCollection RegisterAutoSingletonServices(this IServiceCollection services,Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                 .AsImplementedInterfaces()
                 .WithSingletonLifetime());

        return services;
    }

    public static IServiceCollection RegisterAutoSingletonServices(this IServiceCollection services, Assembly assembly, params Assembly[] assemblies)
    {
        return RegisterAutoSingletonServices(services,[assembly, .. assemblies]);
    }

    public static ContainerBuilder RegisterAutofacAutoServices(this ContainerBuilder builder, Assembly[] assembly)
    {
        //builder.RegisterAssemblyTypes(assembly)
        //    .AsClosedTypesOf(typeof(ITransientService))
        //    .AsImplementedInterfaces()
        //    .InstancePerDependency();

        //builder.RegisterAssemblyTypes(assembly)
        //    .AsClosedTypesOf(typeof(IScopedService))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(assembly)
        //    .AsClosedTypesOf(typeof(ISingletonService))
        //    .AsImplementedInterfaces()
        //    .SingleInstance();

        return builder;
    }

    public static ContainerBuilder RegisterAutofacAutoServices(this ContainerBuilder builder, Assembly assembly, params Assembly[] assemblies)
    {
       return RegisterAutofacAutoServices(builder,[assembly,.. assemblies]);
    }

    public static ContainerBuilder RegisterAutofacCommandQueryServices(this ContainerBuilder builder, Assembly[] assemblies)
    {

        builder.RegisterAssemblyTypes(assemblies)
          .AsClosedTypesOf(typeof(IQueryHandler<,>))
          .AsImplementedInterfaces()
          .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assemblies)
          .AsClosedTypesOf(typeof(ICommandHandler<,>))
          .AsImplementedInterfaces()
          .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assemblies)
          .AsClosedTypesOf(typeof(ICommandHandler<>))
          .AsImplementedInterfaces()
          .InstancePerLifetimeScope();

        return builder;
    }

    public static ContainerBuilder RegisterAutofacCommandQueryServices(this ContainerBuilder builder, Assembly assembly, params Assembly[] assemblies)
    {
        return RegisterAutofacCommandQueryServices(builder, [assembly,.. assemblies]);
    }

}
