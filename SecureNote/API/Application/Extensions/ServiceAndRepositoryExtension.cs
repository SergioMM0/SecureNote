using API.Application.Interfaces.Authentication;
using API.Application.Interfaces.Repositories;
using API.Application.Services;
using API.Core.Interfaces;
using API.Infrastructure;
using API.Infrastructure.Initializers;
using API.Infrastructure.Repositories;

namespace API.Application.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceAndRepositoryExtension {
    /// <summary>
    /// Adds the services and repositories.
    /// </summary>
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services) {
        #region Services

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<INoteService, NoteService>();

        #endregion

        #region Repositories

        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        #endregion

        //services.AddScoped<CurrentContext>();

        services.AddScoped<DbInitializer>();

        // DO NOT CHANGE TO Transient or Singleton
        services.AddScoped<AppDbContext>();

        return services;
    }
}
