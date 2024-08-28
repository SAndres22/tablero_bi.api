using System.Data.SqlClient;
using System.Data;
using tablero_bi.Application;
using tablero_bi.Infraestructure;
using NLog;
using tablero_bi.api.Extensions;
using tablero_bi.api.Handlers;
using Microsoft.Extensions.FileProviders;
using tablero_bi.api.Middleware;
using NLog.Extensions.Logging;
using NLog.Extensions.Hosting;

LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
var logger = LogManager.GetCurrentClassLogger();

try
{
    logger.Info("Iniciando Aplicacion...");
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    builder.Logging.AddNLog();

    // Registrar servicios personalizados
    builder.Services.ConfigureLoggerService();
    builder.Services.AddExceptionHandler<GlobalExecptionHandler>();

    builder.Services.AddScoped<IDbConnection>(c => new SqlConnection(builder.Configuration.GetConnectionString("BdTablero")));

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddAuthorizationPolicies();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGenConfiguration();

    var app = builder.Build();

    // Configurar el pipeline de HTTP request
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseMiddleware<TokenValidationMiddleware>();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
        RequestPath = "/Images"
    });

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{

    logger.Error(ex, "Error al iniciar la aplicación");
    throw;
}
finally
{
    // Asegura liberar los recursos del logger
    LogManager.Shutdown();
}
