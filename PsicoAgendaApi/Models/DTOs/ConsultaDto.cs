namespace PsicoAgendaApi.Models.DTOs
{
    public class ConsultaDto
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string PacienteNome { get; set; } = string.Empty;
        public string PacienteWhatsApp { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public bool Confirmada { get; set; }
        public bool Realizada { get; set; }
    }

    public class CreateConsultaDto
    {
        public int PacienteId { get; set; }
        public DateTime DataHora { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public bool Confirmada { get; set; }
    }
}