namespace PsicoAgendaApi.Models.DTOs
{
    public class PagamentoUpdateDto
    {
        public string Status { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}