using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsicoAgendaApi.Models
{
    public class Relatorio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; } = null!;

        [Required]
        public DateTime DataRelatorio { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Conteudo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = "Sessão"; // Sessão, Evolução, Avaliação
    }
}