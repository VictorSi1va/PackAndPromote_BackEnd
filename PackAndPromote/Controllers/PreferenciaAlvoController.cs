using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PreferenciaAlvoController : Controller
    {
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _dbPackAndPromote;

        // Construtor que injeta o contexto do banco de dados
        public PreferenciaAlvoController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        #endregion

        #region Preferência Alvo - Métodos

        #region Listar Preferências Alvo
        // Endpoint para listar todas as PreferenciasAlvo
        [HttpGet("ListarPreferenciasAlvo")]
        public ActionResult<IEnumerable<PreferenciaAlvo>> ListarPreferenciasAlvo()
        {
            // Obtém todas as PreferenciasAlvo do banco de dados e as retorna
            var listaRetorno = _dbPackAndPromote.PreferenciaAlvo.ToList();

            return Ok(listaRetorno); // Retorna um Ok com a lista de preferências
        }
        #endregion

        #region Pesquisar Preferência Alvo por Id
        // Endpoint para pesquisar uma PreferenciaAlvo pelo ID
        [HttpGet("PesquisarPreferenciaAlvo/{id}")]
        public ActionResult<PreferenciaAlvo> PesquisarPreferenciaAlvo(int id)
        {
            // Busca a PreferenciaAlvo no banco de dados pelo ID fornecido
            var preferenciaAlvo = _dbPackAndPromote.PreferenciaAlvo.Find(id);

            // Verifica se a PreferenciaAlvo foi encontrada
            if (preferenciaAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrada

            return Ok(preferenciaAlvo); // Retorna a PreferenciaAlvo encontrada
        }
        #endregion

        #region Criar Preferência Alvo
        // Endpoint para criar uma nova PreferenciaAlvo
        [Authorize] // Requer autorização
        [HttpPost("CriarPreferenciaAlvo")]
        public ActionResult<PreferenciaAlvo> CriarPreferenciaAlvo(PreferenciaAlvoDto preferenciaAlvoDto)
        {
            // Valida se a descrição da PreferenciaAlvo foi fornecida
            if (string.IsNullOrWhiteSpace(preferenciaAlvoDto.DescricaoPreferenciaAlvo))
                return BadRequest("A descrição da Preferencia Alvo não pode ser vazia ou nula."); // Retorna BadRequest (400)

            // Cria uma nova PreferenciaAlvo a partir do DTO recebido
            PreferenciaAlvo preferenciaAlvo = new PreferenciaAlvo
            {
                DescricaoPreferenciaAlvo = preferenciaAlvoDto.DescricaoPreferenciaAlvo
            };

            // Adiciona e salva a nova PreferenciaAlvo no banco de dados
            _dbPackAndPromote.PreferenciaAlvo.Add(preferenciaAlvo);
            _dbPackAndPromote.SaveChanges();

            // Retorna a PreferenciaAlvo criada com CreatedAtAction
            return CreatedAtAction(nameof(PesquisarPreferenciaAlvo), new { id = preferenciaAlvo.IdPreferenciaAlvo }, preferenciaAlvo);
        }
        #endregion

        #region Alterar Preferência Alvo
        // Endpoint para alterar uma PreferenciaAlvo existente
        [Authorize] // Requer autorização
        [HttpPut("AlterarPreferenciaAlvo/{id}")]
        public IActionResult AlterarPreferenciaAlvo(int id, PreferenciaAlvoDto preferenciaAlvoDto)
        {
            // Busca a PreferenciaAlvo pelo ID
            var preferenciaAlvo = _dbPackAndPromote.PreferenciaAlvo.Find(id);

            // Verifica se a PreferenciaAlvo existe
            if (preferenciaAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrada

            // Atualiza a descrição da PreferenciaAlvo com o valor recebido no DTO
            preferenciaAlvo.DescricaoPreferenciaAlvo = preferenciaAlvoDto.DescricaoPreferenciaAlvo;

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            return Ok(preferenciaAlvo); // Retorna a PreferenciaAlvo atualizada
        }
        #endregion

        #region Excluir Preferência Alvo
        // Endpoint para excluir uma PreferenciaAlvo existente pelo ID
        [Authorize] // Requer autorização
        [HttpDelete("ExcluirPreferenciaAlvo/{id}")]
        public IActionResult ExcluirPreferenciaAlvo(int id)
        {
            // Busca a PreferenciaAlvo pelo ID
            var preferenciaAlvo = _dbPackAndPromote.PreferenciaAlvo.Find(id);

            // Verifica se a PreferenciaAlvo existe
            if (preferenciaAlvo == null)
                return NotFound(); // Retorna NotFound (404) se não encontrada

            // Remove a PreferenciaAlvo e salva as alterações no banco de dados
            _dbPackAndPromote.PreferenciaAlvo.Remove(preferenciaAlvo);
            _dbPackAndPromote.SaveChanges();

            return Ok(); // Retorna uma confirmação de sucesso
        }
        #endregion

        #endregion
    }
}