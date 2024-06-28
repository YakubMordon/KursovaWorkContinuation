using KursovaWorkBLL.Services.Helpers.Transient;
using KursovaWorkBLL.Services.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace KursovaWorkBLL;

/// <summary>
/// Class for dependency injection in KursovaWorkBLL.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Method for dependency injection.
    /// </summary>
    /// <param name="services">Services collection.</param>
    public static void Inject(IServiceCollection services)
    {
        services.AddEntityServices();

        services.AddTransientHelpers();
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
            .AsSelf()
            .WithScopedLifetime());
    }
}