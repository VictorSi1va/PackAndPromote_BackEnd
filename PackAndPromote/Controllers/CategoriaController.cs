using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : Controller
    {
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _dbPackAndPromote;

        public CategoriaController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context; // Injeta o contexto de banco de dados na controller.
        }

        #endregion

        #region Categoria - Métodos

        #region Listar Categorias
        [HttpGet("ListarCategorias")]
        public ActionResult<IEnumerable<Categoria>> ListarCategorias()
        {
            // Recupera todas as categorias do banco de dados.
            var categorias = _dbPackAndPromote.Categoria.ToList();

            // Retorna uma resposta HTTP 200 (OK) com a lista de categorias.
            return Ok(categorias);
        }
        #endregion

        #region Pesquisar Categoria por Id
        [HttpGet("PesquisarCategoria/{id}")]
        public ActionResult<Categoria> PesquisarCategoria(int id)
        {
            // Busca a categoria com o ID fornecido.
            var categoria = _dbPackAndPromote.Categoria.Find(id);

            // Retorna NotFound (404) se a categoria não existir.
            if (categoria == null)
                return NotFound();

            // Retorna uma resposta HTTP 200 (OK) com a categoria encontrada.
            return Ok(categoria);
        }
        #endregion

        #region Criar Categoria
        [Authorize]
        [HttpPost("CriarCategoria")]
        public ActionResult<Categoria> CriarCategoria(CategoriaDto categoriaDto)
        {
            // Verifica se o nome da categoria está vazio ou nulo.
            if (string.IsNullOrWhiteSpace(categoriaDto.NomeCategoria))
                return BadRequest("O Nome da Categoria não pode ser vazio ou nulo."); // Retorna erro 400.

            // Cria uma nova instância da entidade Categoria com o nome fornecido.
            Categoria categoria = new Categoria
            {
                NomeCategoria = categoriaDto.NomeCategoria
            };

            // Adiciona a nova categoria ao banco de dados e salva as mudanças.
            _dbPackAndPromote.Categoria.Add(categoria);
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 201 (Created), incluindo a rota para acessar a nova categoria.
            return CreatedAtAction(nameof(PesquisarCategoria), new { id = categoria.IdCategoria }, categoria);
        }
        #endregion

        #region Alterar Categoria
        [Authorize]
        [HttpPut("AlterarCategoria/{id}")]
        public IActionResult AlterarCategoria(int id, CategoriaDto categoriaDto)
        {
            // Busca a categoria com o ID fornecido.
            var categoria = _dbPackAndPromote.Categoria.Find(id);

            // Retorna NotFound (404) se a categoria não existir.
            if (categoria == null)
                return NotFound();

            // Atualiza o nome da categoria com o valor fornecido.
            categoria.NomeCategoria = categoriaDto.NomeCategoria;

            // Salva as alterações no banco de dados.
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 200 (OK) com a categoria atualizada.
            return Ok(categoria);
        }
        #endregion

        #region Excluir Categoria
        [Authorize]
        [HttpDelete("ExcluirCategoria/{id}")]
        public IActionResult ExcluirCategoria(int id)
        {
            // Busca a categoria com o ID fornecido.
            var categoria = _dbPackAndPromote.Categoria.Find(id);

            // Retorna NotFound (404) se a categoria não existir.
            if (categoria == null)
                return NotFound();

            // Remove a categoria do banco de dados e salva as mudanças.
            _dbPackAndPromote.Categoria.Remove(categoria);
            _dbPackAndPromote.SaveChanges();

            // Retorna uma resposta HTTP 200 (OK) para indicar que a exclusão foi realizada com sucesso.
            return Ok();
        }
        #endregion

        #endregion
    }
}