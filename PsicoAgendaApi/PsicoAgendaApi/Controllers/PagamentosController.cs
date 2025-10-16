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
    public class PagamentosController : ControllerBase
    {
        private readonly PsicoAgendaContext _context;

        public PagamentosController(PsicoAgendaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagamentoDto>>> GetPagamentos()
        {
            var pagamentos = await _context.Pagamentos
                .Include(p => p.Consulta)
                .ThenInclude(c => c.Paciente)
                .OrderByDescending(p => p.DataPagamento)
                .Select(p => new PagamentoDto
                {
                    Id = p.Id,
                    ConsultaId = p.ConsultaId,
                    PacienteNome = p.Consulta.Paciente.Nome,
                    Valor = p.Valor,
                    DataPagamento = p.DataPagamento,
                    Status = p.Status,
                    MetodoPagamento = p.MetodoPagamento
                })
                .ToListAsync();

            return Ok(pagamentos);
        }

        [HttpGet("inadimplentes")]
        public async Task<ActionResult<IEnumerable<InadimplenteDto>>> GetInadimplentes()
        {
            var inadimplentes = await _context.Pagamentos
                .Include(p => p.Consulta)
                .ThenInclude(c => c.Paciente)
                .Where(p => p.Status == "Pendente" && p.DataPagamento < DateTime.Now.AddDays(-1))
                .Select(p => new InadimplenteDto
                {
                    PacienteId = p.Consulta.Paciente.Id,
                    NomePaciente = p.Consulta.Paciente.Nome,
                    WhatsApp = p.Consulta.Paciente.WhatsApp,
                    ConsultaData = p.Consulta.DataHora,
                    ValorDevido = p.Valor,
                    DiasAtraso = (DateTime.Now - p.DataPagamento).Days
                })
                .ToListAsync();

            return Ok(inadimplentes);
        }

        [HttpPut("{id}/pagar")]
        public async Task<IActionResult> MarcarComoPago(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null) return NotFound();

            pagamento.Status = "Pago";
            pagamento.DataPagamento = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagamento(int id, [FromBody] PagamentoUpdateDto dto)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null) return NotFound();

            // Atualiza apenas os campos permitidos
            pagamento.Valor = dto.Valor;
            pagamento.Status = dto.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}