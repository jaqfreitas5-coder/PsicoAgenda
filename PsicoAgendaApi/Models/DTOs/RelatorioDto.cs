namespace PsicoAgendaApi.Models.DTOs
{
    public class RelatorioDto
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string PacienteNome { get; set; } = string.Empty;
        public DateTime DataRelatorio { get; set; }
        public string Conteudo { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Sessão";
    }

    public class CreateRelatorioDto
    {
        public int PacienteId { get; set; }
        public string Conteudo { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Sessão";
    }
}