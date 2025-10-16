using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsicoAgendaApi.Data;
using PsicoAgendaApi.Models; // Adicionamos a referência para o Model 'Pagamento'

namespace PsicoAgendaApi.Controllers
{
    [Authorize] // Este gerente continua protegido pela segurança
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly PsicoAgendaContext _context;

        public RelatoriosController(PsicoAgendaContext context)
        {
            _context = context;
        }

        // GET: api/Relatorios/inadimplentes
        // ATENÇÃO: A assinatura do método foi simplificada para este teste.
        // Agora ele "promete" retornar uma lista de 'Pagamento', não de 'InadimplenteDto'.
        [HttpGet("inadimplentes")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetInadimplentes()
        {
            // TESTE 1: A única tarefa deste método é tentar ler a tabela de pagamentos.
            // Se isso funcionar, sabemos que a conexão e o acesso básico estão OK.
            var pagamentos = await _context.Pagamentos.ToListAsync();

            // Retorna a lista de pagamentos encontrados com um status de sucesso (200 OK).
            return Ok(pagamentos);
        }
    }
}