using Application.Persistence;
using Couchbase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;

namespace Persistence;

public static class Bootstrap
{
    private const string Key = "Couchbase";
    private const string Host = "Host";
    private const string Username = "Username";
    private const string Password = "Password";

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection(Key);

        var host = configurationSection.GetSection(Host).Get<string>();
        var username = configurationSection.GetSection(Username).Get<string>();
        var password = configurationSection.GetSection(Password).Get<string>();

        // Configure Couchbase cluster
        services.AddSingleton<ICluster>(sp =>
        {
            return Cluster.ConnectAsync(host, username, password).Result;
        });


        // Register the repository
        services.Scan(scan => scan
        .FromAssembliesOf(typeof(IRepository<>))
        .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        services.Scan(scan => scan
        .FromAssembliesOf(typeof(CouchbaseRepository<>))
        .AddClasses(classes => classes.AssignableTo(typeof(CouchbaseRepository<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        return services;
    }
}
