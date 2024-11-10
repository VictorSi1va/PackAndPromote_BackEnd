using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlanoController : Controller
    {
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _dbPackAndPromote;

        public PlanoController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context; // Injeta o contexto de banco de dados na controller.
        }

        #endregion

        #region Plano - Métodos

        #region Listar Planos
        [HttpGet("ListarPlanos")]
        public ActionResult<IEnumerable<Plano>> ListarPlanos()
        {
            // Recupera todos os planos do banco de dados.
            var planos = _dbPackAndPromote.Plano.ToList();

            // Retorna uma resposta HTTP 200 (OK) com a lista de planos.
            return Ok(planos);
        }
        #endregion

        #region Pesquisar Plano por Id
        [HttpGet("PesquisarPlano/{id}")]
        public ActionResult<Plano> PesquisarPlano(int id)
        {
            // Busca o plano com o ID fornecido.
            var plano = _dbPackAndPromote.Plano.Find(id);

            // Retorna NotFound (404) se o plano não existir.
            if (plano == null)
                return NotFound();

            // Retorna uma resposta HTTP 200 (OK) com o plano encontrado.
            return Ok(plano);
        }
        #endregion

        #region Criar Plano
        [Authorize]
        [HttpPost("CriarPlano")]
        public ActionResult<Plano> CriarPlano(PlanoDto planoDto)
        {
            // Verifica se o nome do plano está vazio ou nulo.
            if (string.IsNullOrWhiteSpace(planoDto.NomePlano))
                return BadRequest("O nome do plano não pode ser vazio ou nulo."); // Retorna erro 400.

            // Verifica se a descrição do plano está vazia ou nula.
            if (string.IsNullOrWhiteSpace(planoDto.DescricaoPlano))
                return BadRequest("A descrição do plano não pode ser vazia ou nula."); // Retorna erro 400.

            // Verifica se o custo do plano é diferente de um valor negativo.
            if (planoDto.Custo < 0)
                return BadRequest("O valor do plano deve ser um valor positivo!"); // Retorna erro 400.

            // Cria uma nova instância da entidade Plano com os dados fornecidos.
            Plano plano = new Plano
            {
                NomePlano = planoDto.NomePlano,
                DescricaoPlano = planoDto.DescricaoPlano,
                Custo = planoDto.Custo
            };

            // Adiciona o novo plano ao banco de dados e salva as mudanças.
            _dbPackAndPromote.Plano.Add(plano);
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 201 (Created), incluindo a rota para acessar o novo plano.
            return CreatedAtAction(nameof(PesquisarPlano), new { id = plano.IdPlano }, plano);
        }
        #endregion

        #region Alterar Plano
        [Authorize]
        [HttpPut("AlterarPlano/{id}")]
        public IActionResult AlterarPlano(int id, PlanoDto planoDto)
        {
            // Busca o plano com o ID fornecido.
            var plano = _dbPackAndPromote.Plano.Find(id);

            // Retorna NotFound (404) se o plano não existir.
            if (plano == null)
                return NotFound();

            // Atualiza o nome, a descrição e o custo do plano com os dados fornecidos.
            plano.NomePlano = planoDto.NomePlano;
            plano.DescricaoPlano = planoDto.DescricaoPlano;
            plano.Custo = planoDto.Custo;

            // Salva as alterações no banco de dados.
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 200 (OK) com o plano atualizado.
            return Ok(plano);
        }
        #endregion

        #region Excluir Plano
        [Authorize]
        [HttpDelete("ExcluirPlano/{id}")]
        public IActionResult ExcluirPlano(int id)
        {
            // Busca o plano com o ID fornecido.
            var plano = _dbPackAndPromote.Plano.Find(id);

            // Retorna NotFound (404) se o plano não existir.
            if (plano == null)
                return NotFound();

            // Remove o plano do banco de dados e salva as mudanças.
            _dbPackAndPromote.Plano.Remove(plano);
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 200 (OK) para indicar que a exclusão foi realizada com sucesso.
            return Ok();
        }
        #endregion

        #endregion
    }
}
