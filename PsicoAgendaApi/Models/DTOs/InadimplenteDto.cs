namespace PsicoAgendaApi.Models.DTOs
{
    public class InadimplenteDto
    {
        public int PacienteId { get; set; }
        public string NomePaciente { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public DateTime ConsultaData { get; set; }
        public decimal ValorDevido { get; set; }
        public int DiasAtraso { get; set; }
    }
}