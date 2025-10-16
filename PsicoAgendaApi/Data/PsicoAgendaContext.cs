using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PsicoAgendaApi.Models;

namespace PsicoAgendaApi.Data
{
    public class PsicoAgendaContext : IdentityDbContext<IdentityUser>
    {
        public PsicoAgendaContext(DbContextOptions<PsicoAgendaContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Consulta>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.PacienteId);

            builder.Entity<Pagamento>()
                .HasOne(p => p.Consulta)
                .WithMany()
                .HasForeignKey(p => p.ConsultaId);

            builder.Entity<Relatorio>()
                .HasOne(r => r.Paciente)
                .WithMany(p => p.Relatorios)
                .HasForeignKey(r => r.PacienteId);
        }
    }
}