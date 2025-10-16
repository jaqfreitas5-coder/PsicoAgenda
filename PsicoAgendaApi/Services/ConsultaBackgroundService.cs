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

                    // ✅ CORREÇÃO: Consultas que começam em 15 minutos
                    var consultasParaLembrar = await context.Consultas
                        .Include(c => c.Paciente)
                        .Where(c => c.DataHora >= agora.AddMinutes(15) &&    // Começa em 15 min
                                   c.DataHora <= agora.AddMinutes(16) &&     // Janela de 1 minuto
                                   !c.Realizada &&
                                   c.Confirmada &&
                                   !string.IsNullOrEmpty(c.Paciente.WhatsApp))
                        .ToListAsync(stoppingToken);

                    foreach (var consulta in consultasParaLembrar)
                    {
                        _logger.LogInformation($"🔔 Enviando lembrete para {consulta.Paciente.Nome} - {consulta.DataHora}");

                        await whatsAppService.EnviarLembreteConsulta(
                            consulta.Paciente.WhatsApp,
                            consulta.Paciente.Nome,
                            consulta.DataHora
                        );

                        _logger.LogInformation($"✅ Lembrete enviado para {consulta.Paciente.Nome}");
                    }

                    _logger.LogInformation($"📅 Verificadas consultas. Encontradas: {consultasParaLembrar.Count}");

                    // Aguarda 1 minuto antes da próxima verificação
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erro ao processar lembretes de consulta");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
        }
    
