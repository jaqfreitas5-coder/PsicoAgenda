using Microsoft.EntityFrameworkCore;
using PsicoAgendaApi.Data;

namespace PsicoAgendaApi.Services
{
    public class ConsultaBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ConsultaBackgroundService> _logger;

        public ConsultaBackgroundService(IServiceProvider serviceProvider, ILogger<ConsultaBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<PsicoAgendaContext>();
                    var whatsAppService = scope.ServiceProvider.GetRequiredService<IWhatsAppService>();

                    var agora = DateTime.Now;
                    var consultasParaLembrar = await context.Consultas
                        .Include(c => c.Paciente)
                        .Where(c => c.DataHora >= agora &&
                                   c.DataHora <= agora.AddMinutes(15) &&
                                   !c.Realizada &&
                                   c.Confirmada &&
                                   !string.IsNullOrEmpty(c.Paciente.WhatsApp))
                        .ToListAsync(stoppingToken);

                    foreach (var consulta in consultasParaLembrar)
                    {
                        await whatsAppService.EnviarLembreteConsulta(
                            consulta.Paciente.WhatsApp,
                            consulta.Paciente.Nome,
                            consulta.DataHora
                        );

                        // Marcar como notificada para evitar notificações repetidas
                        // Você pode adicionar uma propriedade "Notificada" na model Consulta se quiser
                    }

                    _logger.LogInformation($"📅 Verificadas {consultasParaLembrar.Count} consultas para lembrete");

                    // Aguarda 1 minuto antes da próxima verificação
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erro ao processar lembretes de consulta");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Espera mais em caso de erro
                }
            }
        }
    }
}