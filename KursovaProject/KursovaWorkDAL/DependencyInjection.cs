using KursovaWorkDAL.Repositories.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace KursovaWorkDAL;

/// <summary>
/// Class for dependency injection in KursovaWorkDAL.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Method for dependency injection.
    /// </summary>
    /// <param name="services">Services collection.</param>
    public static void Inject(IServiceCollection services)
    {
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<UserRepository>()
            .AddClasses(classes => classes.InNamespaceOf<UserRepository>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}