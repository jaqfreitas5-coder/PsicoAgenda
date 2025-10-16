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
    public class PacientesController : ControllerBase
    {
        private readonly PsicoAgendaContext _context;

        public PacientesController(PsicoAgendaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacientes()
        {
            var pacientes = await _context.Pacientes
                .OrderBy(p => p.Nome)
                .Select(p => new PacienteDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    WhatsApp = p.WhatsApp,
                    DataNascimento = p.DataNascimento,
                    Observacoes = p.Observacoes
                })
                .ToListAsync();

            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDto>> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes
                .Where(p => p.Id == id)
                .Select(p => new PacienteDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    WhatsApp = p.WhatsApp,
                    DataNascimento = p.DataNascimento,
                    Observacoes = p.Observacoes
                })
                .FirstOrDefaultAsync();

            if (paciente == null) return NotFound();

            return paciente;
        }

        [HttpPost]
        public async Task<ActionResult<PacienteDto>> CreatePaciente(PacienteDto pacienteDto)
        {
            var paciente = new Paciente
            {
                Nome = pacienteDto.Nome,
                Email = pacienteDto.Email,
                Telefone = pacienteDto.Telefone,
                WhatsApp = pacienteDto.WhatsApp,
                DataNascimento = pacienteDto.DataNascimento,
                Observacoes = pacienteDto.Observacoes,
                DataCadastro = DateTime.Now
            };

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            pacienteDto.Id = paciente.Id;
            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, pacienteDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaciente(int id, PacienteDto pacienteDto)
        {
            if (id != pacienteDto.Id) return BadRequest();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            paciente.Nome = pacienteDto.Nome;
            paciente.Email = pacienteDto.Email;
            paciente.Telefone = pacienteDto.Telefone;
            paciente.WhatsApp = pacienteDto.WhatsApp;
            paciente.DataNascimento = pacienteDto.DataNascimento;
            paciente.Observacoes = pacienteDto.Observacoes;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}