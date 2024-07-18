using System.Data.SqlClient;
using System.Data;
using tablero_bi.Application;
using tablero_bi.Infraestructure;
using NLog;
using tablero_bi.api.Extensions;
using tablero_bi.api.Handlers;
using Microsoft.Extensions.FileProviders;
using tablero_bi.api.Middleware;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();
builder.Services.AddExceptionHandler<GlobalExecptionHandler>();

builder.Services.AddScoped<IDbConnection>(c => new SqlConnection(builder.Configuration.GetConnectionString("BdTablero")));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddJwtAuthentication(builder.Configuration);

//autorización basada en roles
builder.Services.AddAuthorizationPolicies();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
