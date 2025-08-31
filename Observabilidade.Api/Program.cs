#region

using Microsoft.AspNetCore.HttpLogging;
using Observabilidade.Api.Extensions;

#endregion

var builder = WebApplication.CreateBuilder(args);

//Open Telemetry
builder.Logging.AddCustomOpenTelemetryLogging(builder.Configuration);

//Swagger
builder.Services.AddCustomSwagger();

// Request logging (útil para diagnósticos rápidos e correlação)
builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                      HttpLoggingFields.ResponsePropertiesAndHeaders |
                      HttpLoggingFields.Duration;
});

// Health Checks (ponto de verificação para orquestradores e monitoramento)
builder.Services.AddHealthChecks();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseCustomSwagger();

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();