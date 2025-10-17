using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsicoAgendaApi.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string WhatsApp { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar(MAX)")]
        public string Observacoes { get; set; } = string.Empty;

        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
        public ICollection<Relatorio> Relatorios { get; set; } = new List<Relatorio>();
    }
}