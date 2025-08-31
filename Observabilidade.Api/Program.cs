using System.Reflection;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

#region Open Telemetry
builder.Services
    .AddOpenTelemetry();

//Configurar logs para o Open Telemetry
builder.Services.AddLogging(logging =>
{
    logging.AddOpenTelemetry(options =>
    {
        options.IncludeFormattedMessage = true; //Mensagem formatada
        options.IncludeScopes = true; //Agrupa por scopos (tudo o que acontece dentro de uma req específica)
        options.ParseStateValues = true; //Permite extrair variáveis que posso passar via ILogger

        options.SetResourceBuilder(
            ResourceBuilder
                .CreateDefault()
                .AddService(
                    serviceName: "ObservabilidadeApi",
                    serviceVersion: "1.0.0",
                    serviceInstanceId: Environment.MachineName)
                .AddAttributes(
                [
                    new KeyValuePair<string, object>("deployment.environment", "dev")
                ])
        );
        
        options.AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("http://otel-collector:4317"); // Para onde os logs são enviados 
        });
    });
});
#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Observabilidade API", 
        Version = "v1", 
        Description = "API para estudos de observabilidade, com coleta inicialmente de logs."
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    
    // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    // {
    //     Name = "Authorization",
    //     Type = SecuritySchemeType.Http,
    //     Scheme = "bearer",
    //     BearerFormat = "JWT",
    //     In = ParameterLocation.Header,
    //     Description = "Insira o token JWT no campo abaixo"
    // });
    //
    // options.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "Bearer"
    //             }
    //         },
    //         Array.Empty<string>()
    //     }
    // });
});

// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(8080); // Apenas HTTP
// });

var app = builder.Build();

// Configura pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Gera o /swagger/v1/swagger.json
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Observabilidade API v1");
        options.DocumentTitle = "Observabilidade API Docs";
        options.RoutePrefix = "swagger"; // A interface estará em /swagger
        options.DisplayRequestDuration(); // Mostra tempo de resposta
        options.EnableFilter(); // Permite filtrar endpoints
    });
}
else
    app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();