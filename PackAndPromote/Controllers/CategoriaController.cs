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
        private readonly DbPackAndPromote _dbPackAndPromote;

        public CategoriaController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        [HttpGet("ListarCategorias")]
        public ActionResult<IEnumerable<Categoria>> ListarCategorias()
        {
            var categorias = _dbPackAndPromote.Categoria.ToList();

            return Ok(categorias);
        }

        [HttpGet("PesquisarCategoria/{id}")]
        public ActionResult<Categoria> PesquisarCategoria(int id)
        {
            var categoria = _dbPackAndPromote.Categoria.Find(id);

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost("CriarCategoria")]
        public ActionResult<Categoria> CriarCategoria(CategoriaDto categoriaDto)
        {
            Categoria categoria = new Categoria
            {
                NomeCategoria = categoriaDto.NomeCategoria
            };

            _dbPackAndPromote.Categoria.Add(categoria);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarCategoria), new { id = categoria.IdCategoria }, categoria);
        }

        [HttpPut("AlterarCategoria/{id}")]
        public IActionResult AlterarCategoria(int id, CategoriaDto categoriaDto)
        {
            var categoria = _dbPackAndPromote.Categoria.Find(id);

            if (categoria == null)
                return NotFound();

            categoria.NomeCategoria = categoriaDto.NomeCategoria;

            _dbPackAndPromote.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("ExcluirCategoria/{id}")]
        public IActionResult ExcluirCategoria(int id)
        {
            var categoria = _dbPackAndPromote.Categoria.Find(id);

            if (categoria == null)
                return NotFound();

            _dbPackAndPromote.Categoria.Remove(categoria);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }
    }
}
