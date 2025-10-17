namespace PsicoAgendaApi.Models.DTOs
{
    public class PacienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Observacoes { get; set; } = string.Empty;
    }

    public class CreatePacienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Observacoes { get; set; } = string.Empty;
    }
}