using KursovaWork.Infrastructure.Repositories;
using KursovaWork.Infrastructure.Services.Entities;
using KursovaWork.Infrastructure.Services.Helpers.Transient;
using Microsoft.Extensions.DependencyInjection;

namespace KursovaWork.Infrastructure;

/// <summary>
/// Class for dependency injection in KursovaWork.Infrastructure Layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Method for dependency injection.
    /// </summary>
    /// <param name="services">Services collection.</param>
    public static void Inject(IServiceCollection services)
    {
        services.AddTransientHelpers();

        services.AddRepositories();

        services.AddEntityServices();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<UserRepository>()
            .AddClasses(classes => classes.InNamespaceOf<UserRepository>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    private static void AddEntityServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<UserService>()
            .AddClasses(classes => classes.InNamespaceOf<UserService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    private static void AddTransientHelpers(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IdRetriever>()
            .AddClasses(classes => classes.InNamespaceOf<IdRetriever>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}