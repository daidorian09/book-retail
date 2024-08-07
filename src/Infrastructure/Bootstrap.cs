namespace Infrastructure;

public static class Bootstrap
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerDocument();

        // This is a fake authentication. Remove this on real scenario after implemention own authentication
        /* services.AddAuthentication(FakeAuthHandler.AuthenticationScheme)
               .AddScheme<FakeAuthHandlerOptions, FakeAuthHandler>(FakeAuthHandler.AuthenticationScheme, options => {
               });*/

        return services;
    }
}
