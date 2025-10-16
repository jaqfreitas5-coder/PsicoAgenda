using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsicoAgendaApi.Models
{
    public class Pagamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ConsultaId { get; set; }

        [ForeignKey("ConsultaId")]
        public Consulta Consulta { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required]
        public DateTime DataPagamento { get; set; }

        [Required]
        [StringLength(50)]
        public string MetodoPagamento { get; set; } = "Pix";

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pendente"; // Pendente, Pago, Atrasado

        [Column(TypeName = "nvarchar(MAX)")]
        public string Observacoes { get; set; } = string.Empty;
    }
}