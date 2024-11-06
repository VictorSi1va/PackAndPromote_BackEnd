using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegiaoAlvoController : Controller
    {
        private readonly DbPackAndPromote _dbPackAndPromote; // Contexto do banco de dados

        // Construtor que injeta o contexto do banco de dados
        public RegiaoAlvoController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context; // Inicializa o contexto do banco de dados
        }

        // Endpoint para listar todas as regiões alvo
        [HttpGet("ListarRegioesAlvo")]
        public ActionResult<IEnumerable<RegiaoAlvo>> ListarRegioesAlvo()
        {
            // Obtém todas as regiões alvo do banco de dados
            var regioes = _dbPackAndPromote.RegiaoAlvo.ToList();

            return Ok(regioes); // Retorna um Ok com a lista de regiões alvo
        }

        // Endpoint para pesquisar uma região alvo pelo ID
        [HttpGet("PesquisarRegiaoAlvo/{id}")]
        public ActionResult<RegiaoAlvo> PesquisarRegiaoAlvo(int id)
        {
            // Busca a região alvo no banco de dados pelo ID fornecido
            var regiaoAlvo = _dbPackAndPromote.RegiaoAlvo.Find(id);

            // Verifica se a região alvo foi encontrada
            if (regiaoAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrado

            return Ok(regiaoAlvo); // Retorna a região alvo encontrada
        }

        // Endpoint para criar uma nova região alvo
        [Authorize] // Requer autorização
        [HttpPost("CriarRegiaoAlvo")]
        public ActionResult<RegiaoAlvo> CriarRegiaoAlvo(RegiaoAlvoDto regiaoAlvoDto)
        {
            // Valida se o nome da região alvo foi fornecido
            if (string.IsNullOrWhiteSpace(regiaoAlvoDto.NomeRegiaoAlvo))
                return BadRequest("O nome da Região Alvo não pode ser vazio ou nulo."); // Retorna BadRequest (400)

            // Cria uma nova região alvo a partir do DTO recebido
            RegiaoAlvo regiaoAlvo = new RegiaoAlvo
            {
                NomeRegiaoAlvo = regiaoAlvoDto.NomeRegiaoAlvo
            };

            // Adiciona e salva a nova região alvo no banco de dados
            _dbPackAndPromote.RegiaoAlvo.Add(regiaoAlvo);
            _dbPackAndPromote.SaveChanges();

            // Retorna a nova região alvo criada com CreatedAtAction
            return CreatedAtAction(nameof(PesquisarRegiaoAlvo), new { id = regiaoAlvo.IdRegiaoAlvo }, regiaoAlvo);
        }

        // Endpoint para alterar uma região alvo existente
        [Authorize] // Requer autorização
        [HttpPut("AlterarRegiaoAlvo/{id}")]
        public IActionResult AlterarRegiaoAlvo(int id, RegiaoAlvoDto regiaoAlvoDto)
        {
            // Busca a região alvo pelo ID
            var regiaoAlvo = _dbPackAndPromote.RegiaoAlvo.Find(id);

            // Verifica se a região alvo existe
            if (regiaoAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrado

            // Atualiza o nome da região alvo com o valor recebido no DTO
            regiaoAlvo.NomeRegiaoAlvo = regiaoAlvoDto.NomeRegiaoAlvo;

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            return Ok(regiaoAlvo); // Retorna a região alvo atualizada
        }

        // Endpoint para excluir uma região alvo existente pelo ID
        [Authorize] // Requer autorização
        [HttpDelete("ExcluirRegiaoAlvo/{id}")]
        public IActionResult ExcluirRegiaoAlvo(int id)
        {
            // Busca a região alvo pelo ID
            var regiaoAlvo = _dbPackAndPromote.RegiaoAlvo.Find(id);

            // Verifica se a região alvo existe
            if (regiaoAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrado

            // Remove a região alvo e salva as alterações no banco de dados
            _dbPackAndPromote.RegiaoAlvo.Remove(regiaoAlvo);
            _dbPackAndPromote.SaveChanges();

            return Ok(); // Retorna uma confirmação de sucesso
        }
    }
}