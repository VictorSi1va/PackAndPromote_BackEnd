using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FaixaEtariaController : Controller
    {
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _dbPackAndPromote; // Contexto de banco de dados.

        public FaixaEtariaController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context; // Injeta o contexto de banco de dados na controller.
        }

        #endregion

        #region Faixa Etária - Métodos

        #region Listar Faixas Etárias
        [HttpGet("ListarFaixasEtarias")]
        public ActionResult<IEnumerable<FaixaEtaria>> ListarFaixasEtarias()
        {
            // Busca todas as faixas etárias do banco de dados.
            var listaRetorno = _dbPackAndPromote.FaixaEtaria.ToList();

            // Retorna uma resposta HTTP 200 (OK) com a lista de faixas etárias.
            return Ok(listaRetorno);
        }
        #endregion

        #region Pesquisar Faixa Etária por Id
        [HttpGet("PesquisarFaixaEtaria/{id}")]
        public ActionResult<FaixaEtaria> PesquisarFaixaEtaria(int id)
        {
            // Busca a faixa etária pelo ID fornecido.
            var faixaEtaria = _dbPackAndPromote.FaixaEtaria.Find(id);

            // Retorna NotFound (404) se a faixa etária não for encontrada.
            if (faixaEtaria == null)
                return NotFound();

            // Retorna uma resposta HTTP 200 (OK) com a faixa etária encontrada.
            return Ok(faixaEtaria);
        }
        #endregion

        #region Criar Faixa Etária
        [Authorize] // Requer autorização para criar uma faixa etária.
        [HttpPost("CriarFaixaEtaria")]
        public ActionResult<FaixaEtaria> CriarFaixaEtaria(FaixaEtariaDto faixaEtariaDto)
        {
            // Verifica se a descrição da faixa etária é vazia ou nula.
            if (string.IsNullOrWhiteSpace(faixaEtariaDto.DescricaoFaixaEtaria))
                return BadRequest("A descrição da faixa etária não pode ser vazia ou nula."); // Retorna erro 400.

            // Cria uma nova instância de FaixaEtaria com a descrição fornecida.
            FaixaEtaria faixaEtaria = new FaixaEtaria
            {
                DescricaoFaixaEtaria = faixaEtariaDto.DescricaoFaixaEtaria
            };

            // Adiciona a nova faixa etária ao banco de dados e salva as mudanças.
            _dbPackAndPromote.FaixaEtaria.Add(faixaEtaria);
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 201 (Created), incluindo a rota para acessar a nova faixa etária.
            return CreatedAtAction(nameof(PesquisarFaixaEtaria), new { id = faixaEtaria.IdFaixaEtaria }, faixaEtaria);
        }
        #endregion

        #region Alterar Faixa Etária
        [Authorize] // Requer autorização para atualizar uma faixa etária.
        [HttpPut("AlterarFaixaEtaria/{id}")]
        public IActionResult AlterarFaixaEtaria(int id, FaixaEtariaDto faixaEtariaDto)
        {
            // Busca a faixa etária pelo ID fornecido.
            var faixaEtaria = _dbPackAndPromote.FaixaEtaria.Find(id);

            // Retorna NotFound (404) se a faixa etária não for encontrada.
            if (faixaEtaria == null)
                return NotFound();

            // Atualiza a descrição da faixa etária com o valor fornecido.
            faixaEtaria.DescricaoFaixaEtaria = faixaEtariaDto.DescricaoFaixaEtaria;

            // Salva as mudanças no banco de dados.
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 200 (OK) com a faixa etária atualizada.
            return Ok(faixaEtaria);
        }
        #endregion

        #region Excluir Faixa Etária
        [Authorize] // Requer autorização para excluir uma faixa etária.
        [HttpDelete("ExcluirFaixaEtaria/{id}")]
        public IActionResult ExcluirFaixaEtaria(int id)
        {
            // Busca a faixa etária pelo ID fornecido.
            var faixaEtaria = _dbPackAndPromote.FaixaEtaria.Find(id);

            // Retorna NotFound (404) se a faixa etária não for encontrada.
            if (faixaEtaria == null)
                return NotFound();

            // Remove a faixa etária do banco de dados e salva as mudanças.
            _dbPackAndPromote.FaixaEtaria.Remove(faixaEtaria);
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 200 (OK) para indicar a exclusão bem-sucedida.
            return Ok();
        }
        #endregion

        #endregion
    }
}