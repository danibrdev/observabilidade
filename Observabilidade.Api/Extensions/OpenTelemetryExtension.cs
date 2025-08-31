#region

using System.Reflection;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

#endregion

namespace Observabilidade.Api.Extensions;

public static class OpenTelemetryServiceExtensions
{
    public static ILoggingBuilder AddCustomOpenTelemetryLogging(this ILoggingBuilder logging, IConfiguration config)
    {
        var serviceName = Assembly.GetExecutingAssembly().GetName().Name!;
        var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var otlpEndpoint = config["OTEL_EXPORTER_OTLP_ENDPOINT"]
                           ?? config["Observability:Otlp:Endpoint"]
                           ?? "http://otel-collector:4317";

        logging.ClearProviders(); // opcional: remove providers padrão para evitar logs duplicados
        logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true; //Mensagem formatada
            options.IncludeScopes = true; //Agrupa por scopos (tudo o que acontece numa req específica)
            options.ParseStateValues = true; //Permite extrair variáveis que posso passar via ILogger
            options.SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName, serviceVersion: serviceVersion, serviceInstanceId: Guid.NewGuid().ToString())
                    .AddTelemetrySdk()
                    .AddEnvironmentVariableDetector()
                    .AddAttributes(new[]
                    {
                        new KeyValuePair<string, object>("deployment.environment", environment),
                        new KeyValuePair<string, object>("team", "BackendObservabilidade"),
                        new KeyValuePair<string, object>("application.layer", "API")
                    })
            );
            options.AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint));
        });

        return logging;
    }
}