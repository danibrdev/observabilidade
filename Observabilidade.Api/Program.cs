#region

using System.Reflection;
using L2.Commom.Configurators;
using L2.Commom.Configurators.Otel;
using L2.Commom.Middlewares;
using Microsoft.AspNetCore.HttpLogging;

#endregion

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
builder.Logging.AddConsole();

var serviceName = Assembly.GetExecutingAssembly().GetName().Name!;
var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var serviceInstanceId = Environment.MachineName;

services
    .AddOtelConfigurator(builder.Configuration, serviceName, serviceVersion, environment, serviceInstanceId,
        clearLoggingProviders: true)
    .UseOtelBuilder()
    .WithLogging()
    .WithCompleteTelemetry();

//Swagger
builder.Services.AddCustomSwagger();

// Request logging (útil para diagnósticos rápidos e correlação)
builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | //Método HTTP, URL, Cabecalho
                      HttpLoggingFields.ResponsePropertiesAndHeaders | // Status da resposta, cabecalhos da resposta
                      HttpLoggingFields.Duration; // Tempo total da requisição
});

// Health Checks (ponto de verificação para orquestradores e monitoramento)
builder.Services.AddHealthChecks();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseCustomSwagger();

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();