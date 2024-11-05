using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PublicoAlvoController : Controller
    {
        private readonly DbPackAndPromote _dbPackAndPromote;

        // Construtor que injeta o contexto do banco de dados
        public PublicoAlvoController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context; // Inicializa o contexto do banco de dados
        }

        // Endpoint para listar todos os Públicos Alvo
        [HttpGet("ListarPublicosAlvo")]
        public ActionResult<IEnumerable<PublicoAlvo>> ListarPublicosAlvo()
        {
            // Obtém todos os Públicos Alvo do banco de dados e os retorna
            var listaRetorno = _dbPackAndPromote.PublicoAlvo.ToList();

            return Ok(listaRetorno); // Retorna um Ok com a lista de públicos alvo
        }

        // Endpoint para pesquisar um Público Alvo pelo ID
        [HttpGet("PesquisarPublicoAlvo/{id}")]
        public ActionResult<PublicoAlvo> PesquisarPublicoAlvo(int id)
        {
            // Busca o Público Alvo no banco de dados pelo ID fornecido
            var publicoAlvo = _dbPackAndPromote.PublicoAlvo.Find(id);

            // Verifica se o Público Alvo foi encontrado
            if (publicoAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrado

            return Ok(publicoAlvo); // Retorna o Público Alvo encontrado
        }

        // Endpoint para criar um novo Público Alvo
        [Authorize] // Requer autorização
        [HttpPost("CriarPublicoAlvo")]
        public ActionResult<PublicoAlvo> CriarPublicoAlvo(PublicoAlvoDto publicoAlvoDto)
        {
            // Valida se a descrição do Público Alvo foi fornecida
            if (string.IsNullOrWhiteSpace(publicoAlvoDto.DescricaoPublicoAlvo))
                return BadRequest("A descrição do Público Alvo não pode ser vazia ou nula."); // Retorna BadRequest (400)

            // Cria um novo Público Alvo a partir do DTO recebido
            PublicoAlvo publicoAlvo = new PublicoAlvo
            {
                DescricaoPublicoAlvo = publicoAlvoDto.DescricaoPublicoAlvo
            };

            // Adiciona e salva o novo Público Alvo no banco de dados
            _dbPackAndPromote.PublicoAlvo.Add(publicoAlvo);
            _dbPackAndPromote.SaveChanges();

            // Retorna o Público Alvo criado com CreatedAtAction
            return CreatedAtAction(nameof(PesquisarPublicoAlvo), new { id = publicoAlvo.IdPublicoAlvo }, publicoAlvo);
        }

        // Endpoint para alterar um Público Alvo existente
        [Authorize] // Requer autorização
        [HttpPut("AlterarPublicoAlvo/{id}")]
        public IActionResult AlterarPublicoAlvo(int id, PublicoAlvoDto publicoAlvoDto)
        {
            // Busca o Público Alvo pelo ID
            var publicoAlvo = _dbPackAndPromote.PublicoAlvo.Find(id);

            // Verifica se o Público Alvo existe
            if (publicoAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrado

            // Atualiza a descrição do Público Alvo com o valor recebido no DTO
            publicoAlvo.DescricaoPublicoAlvo = publicoAlvoDto.DescricaoPublicoAlvo;

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            return Ok(publicoAlvo); // Retorna o Público Alvo atualizado
        }

        // Endpoint para excluir um Público Alvo existente pelo ID
        [Authorize] // Requer autorização
        [HttpDelete("ExcluirPublicoAlvo/{id}")]
        public IActionResult ExcluirPublicoAlvo(int id)
        {
            // Busca o Público Alvo pelo ID
            var publicoAlvo = _dbPackAndPromote.PublicoAlvo.Find(id);

            // Verifica se o Público Alvo existe
            if (publicoAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrado

            // Remove o Público Alvo e salva as alterações no banco de dados
            _dbPackAndPromote.PublicoAlvo.Remove(publicoAlvo);
            _dbPackAndPromote.SaveChanges();

            return Ok(); // Retorna uma confirmação de sucesso
        }
    }
}