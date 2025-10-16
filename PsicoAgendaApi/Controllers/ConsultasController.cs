using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsicoAgendaApi.Data;
using PsicoAgendaApi.Models;
using PsicoAgendaApi.Models.DTOs;

namespace PsicoAgendaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsultasController : ControllerBase
    {
        private readonly PsicoAgendaContext _context;

        public ConsultasController(PsicoAgendaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultaDto>>> GetConsultas()
        {
            var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .OrderBy(c => c.DataHora)
                .Select(c => new ConsultaDto
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    PacienteNome = c.Paciente.Nome,
                    PacienteWhatsApp = c.Paciente.WhatsApp,
                    DataHora = c.DataHora,
                    Observacoes = c.Observacoes,
                    Confirmada = c.Confirmada
                })
                .ToListAsync();

            return Ok(consultas);
        }

        [HttpPost]
        public async Task<ActionResult<ConsultaDto>> CreateConsulta(ConsultaDto consultaDto)
        {
            var consulta = new Consulta
            {
                PacienteId = consultaDto.PacienteId,
                DataHora = consultaDto.DataHora,
                Observacoes = consultaDto.Observacoes,
                Confirmada = consultaDto.Confirmada,
                DataCriacao = DateTime.Now
            };

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            // Criar pagamento automaticamente
            var pagamento = new Pagamento
            {
                ConsultaId = consulta.Id,
                Valor = 150.00m, // Valor padrão - ajuste conforme necessário
                DataPagamento = DateTime.Now,
                Status = "Pendente",
                MetodoPagamento = "Pix"
            };

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            consultaDto.Id = consulta.Id;
            return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, consultaDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultaDto>> GetConsulta(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Where(c => c.Id == id)
                .Select(c => new ConsultaDto
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    PacienteNome = c.Paciente.Nome,
                    PacienteWhatsApp = c.Paciente.WhatsApp,
                    DataHora = c.DataHora,
                    Observacoes = c.Observacoes,
                    Confirmada = c.Confirmada
                })
                .FirstOrDefaultAsync();

            if (consulta == null) return NotFound();

            return consulta;
        }

        [HttpPut("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null) return NotFound();

            consulta.Confirmada = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}