namespace PsicoAgendaApi.Models.DTOs
{
    public class PagamentoDto
    {
        public int Id { get; set; }
        public int ConsultaId { get; set; }
        public string PacienteNome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Status { get; set; } = "Pendente";
        public string MetodoPagamento { get; set; } = "Pix";
    }

    public class CreatePagamentoDto
    {
        public int ConsultaId { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; } = "Pix";
    }
}