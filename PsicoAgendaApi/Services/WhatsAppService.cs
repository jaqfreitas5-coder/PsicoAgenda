using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PsicoAgendaApi.Services
{
    public interface IWhatsAppService
    {
        Task EnviarLembreteConsulta(string numeroPaciente, string nomePaciente, DateTime dataConsulta);
    }

    public class WhatsAppService : IWhatsAppService
    {
        private readonly IConfiguration _configuration;
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _whatsAppNumber;

        public WhatsAppService(IConfiguration configuration)
        {
            _configuration = configuration;
            _accountSid = _configuration["Twilio:AccountSid"]!;
            _authToken = _configuration["Twilio:AuthToken"]!;
            _whatsAppNumber = _configuration["Twilio:WhatsAppNumber"]!;

            // Inicializa o Twilio apenas se as credenciais existirem
            if (!string.IsNullOrEmpty(_accountSid) && !string.IsNullOrEmpty(_authToken))
            {
                TwilioClient.Init(_accountSid, _authToken);
            }
        }

        public async Task EnviarLembreteConsulta(string numeroPaciente, string nomePaciente, DateTime dataConsulta)
        {
            try
            {
                // Verifica se as credenciais do Twilio estão configuradas
                if (string.IsNullOrEmpty(_accountSid) || string.IsNullOrEmpty(_authToken))
                {
                    Console.WriteLine("Twilio não configurado. Configure AccountSid e AuthToken no appsettings.json");
                    return;
                }

                var mensagem = $"Olá {nomePaciente}! 😊 Lembrete da sua sessão de terapia que acontecerá em 15 minutos ({dataConsulta:dd/MM/yyyy 'às' HH:mm}). Estou te esperando! 💙";

                var message = await MessageResource.CreateAsync(
                    body: mensagem,
                    from: new PhoneNumber($"whatsapp:{_whatsAppNumber}"),
                    to: new PhoneNumber($"whatsapp:{FormatarNumero(numeroPaciente)}")
                );

                Console.WriteLine($"✅ Lembrete enviado para {nomePaciente}. SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar WhatsApp para {nomePaciente}: {ex.Message}");
            }
        }

        private string FormatarNumero(string numero)
        {
            // Remove caracteres não numéricos
            var numeroLimpo = new string(numero.Where(char.IsDigit).ToArray());

            // Formata para o padrão internacional
            if (numeroLimpo.Length == 11 && numeroLimpo.StartsWith("0"))
            {
                numeroLimpo = "55" + numeroLimpo.Substring(1); 
            }
            else if (numeroLimpo.Length == 10)
            {
                numeroLimpo = "55" + numeroLimpo; 
            }
            else if (numeroLimpo.Length == 13 && numeroLimpo.StartsWith("55"))
            {
               
            }

            return "+" + numeroLimpo;
        }
    }
}