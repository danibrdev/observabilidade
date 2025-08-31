#region

using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Observabilidade.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
            => _logger = logger;

        [HttpGet("generate-advanced")]
        public IActionResult GenerateAdvancedLogs(int count = 500)
        {
            var random = new Random();
            var activitySource = new ActivitySource("Observabilidade.Api");

            for (var i = 0; i < count; i++)
            {
                var level = random.Next(0, 5);
                var traceId = ActivityTraceId.CreateRandom().ToString();

                var payload = new
                {
                    UserId = random.Next(1, 1000),
                    OrderId = Guid.NewGuid(),
                    Quantidade = random.NextDouble() * 1000,
                    Produtos = new[]
                    {
                        new { Name = "Produto", Qty = random.Next(1,5) },
                        new { Name = "Produto", Qty = random.Next(1,3) }
                    },
                    TraceId = traceId
                };

                using var activity = activitySource.StartActivity("ProcessarLog");
                activity?.SetTag("operacao", "teste");
                activity?.AddEvent(new ActivityEvent("Iniciando processamento"));

                var mensagem = $"Log {i} - Payload: {JsonSerializer.Serialize(payload)}";

                switch (level)
                {
                    case 0: _logger.LogDebug(mensagem); break;
                    case 1: _logger.LogInformation(mensagem); break;
                    case 2: _logger.LogWarning(mensagem); break;
                    case 3: _logger.LogError(mensagem); break;
                    case 4: _logger.LogCritical(mensagem); break;
                }

                activity?.AddEvent(new ActivityEvent("Processamento concluído"));
            }

            return Ok($"Logs gerados com sucesso. Total: {count}");
        }
    }
}
