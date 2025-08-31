using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Observabilidade.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Get()
    {
        //Adicionar logs com contexto 
        //O userId é uma variavel que pode ser consultada posteriormente no log
        using var scope = _logger.BeginScope("{UserId}", 12345);
        _logger.LogInformation("Processando requisição GET");
        
        //Criar um span manual para trace
        var activitySource = new ActivitySource("Observabilidade.Api");
        using var activity = activitySource.StartActivity("ProcessarTeste");
        activity?.SetTag("operacao", "teste"); 
        activity?.AddEvent(new ActivityEvent("Iniciando processamento"));
 
        // Simular trabalho
        Thread.Sleep(100);

        activity?.AddEvent(new ActivityEvent("Processamento concluído"));

        return Ok("Requisição criada com sucesso");
    }
}