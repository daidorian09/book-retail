using Infrastructure.Authorization;

namespace Infrastructure;

public static class Bootstrap
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerDocument();
       
         services.AddAuthentication(FakeAuthHandler.AuthenticationScheme)
               .AddScheme<FakeAuthHandlerOptions, FakeAuthHandler>(FakeAuthHandler.AuthenticationScheme, options => {
               });

        return services;
    }
}
