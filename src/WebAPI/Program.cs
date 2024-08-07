using Serilog;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using Infrastructure.Middlewares;
using WebAPI.Infrastructure;

public partial class Program {
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, loggerConfig)
            => loggerConfig.ReadFrom.Configuration(context.Configuration));
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();


        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
    //    builder.Services.AddPersistenceServices(builder.Configuration);
       builder.AddInfrastructure();

        builder.Services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSerilogRequestLogging();

        //await app.MigrateDatabase();
        //await app.InitializeDatabase(app.Environment.IsDevelopment() || app.Environment.IsEnvironment("It"));

        app.UseOpenApi();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

public partial class Program { }
