using System.Data.SqlClient;
using System.Data;
using tablero_bi.Application;
using tablero_bi.Infraestructure;
using NLog;
using tablero_bi.api.Extensions;
using tablero_bi.api.Handlers;

var builder = WebApplication.CreateBuilder(args);

//Configuracion Nlog
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureLoggerService();
builder.Services.AddExceptionHandler<GlobalExecptionHandler>();
//Configuracion a BD
builder.Services.AddScoped<IDbConnection>(c => new SqlConnection(builder.Configuration.GetConnectionString("BdTablero")));


//Inyeccion de dependencias de Application e Infraestructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
