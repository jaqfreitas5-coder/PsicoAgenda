using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsicoAgendaApi.Models
{
    public class Consulta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; } = null!;

        [Required]
        public DateTime DataHora { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Observacoes { get; set; } = string.Empty;

        [Required]
        public bool Confirmada { get; set; } = false;

        [Required]
        public bool Realizada { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}