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
        private readonly DbPackAndPromote _dbPackAndPromote;

        public FaixaEtariaController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        [HttpGet("ListarFaixasEtarias")]
        public ActionResult<IEnumerable<FaixaEtaria>> ListarFaixasEtarias()
        {
            var listaRetorno = _dbPackAndPromote.FaixaEtaria.ToList();

            return Ok(listaRetorno);
        }

        [HttpGet("PesquisarFaixaEtaria/{id}")]
        public ActionResult<FaixaEtaria> PesquisarFaixaEtaria(int id)
        {
            var faixaEtaria = _dbPackAndPromote.FaixaEtaria.Find(id);

            if (faixaEtaria == null)
                return NotFound();

            return Ok(faixaEtaria);
        }

        [Authorize]
        [HttpPost("CriarFaixaEtaria")]
        public ActionResult<FaixaEtaria> CriarFaixaEtaria(FaixaEtariaDto faixaEtariaDto)
        {
            if (string.IsNullOrWhiteSpace(faixaEtariaDto.DescricaoFaixaEtaria))
                return BadRequest("A descrição da faixa etária não pode ser vazia ou nula.");

            FaixaEtaria faixaEtaria = new FaixaEtaria
            {
                DescricaoFaixaEtaria = faixaEtariaDto.DescricaoFaixaEtaria
            };

            _dbPackAndPromote.FaixaEtaria.Add(faixaEtaria);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarFaixaEtaria), new { id = faixaEtaria.IdFaixaEtaria }, faixaEtaria);
        }

        [Authorize]
        [HttpPut("AlterarFaixaEtaria/{id}")]
        public IActionResult AlterarFaixaEtaria(int id, FaixaEtariaDto faixaEtariaDto)
        {
            var faixaEtaria = _dbPackAndPromote.FaixaEtaria.Find(id);

            if (faixaEtaria == null)
                return NotFound();

            faixaEtaria.DescricaoFaixaEtaria = faixaEtariaDto.DescricaoFaixaEtaria;

            _dbPackAndPromote.SaveChanges();

            return Ok(faixaEtaria);
        }

        [Authorize]
        [HttpDelete("ExcluirFaixaEtaria/{id}")]
        public IActionResult ExcluirFaixaEtaria(int id)
        {
            var faixaEtaria = _dbPackAndPromote.FaixaEtaria.Find(id);

            if (faixaEtaria == null)
                return NotFound();

            _dbPackAndPromote.FaixaEtaria.Remove(faixaEtaria);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }
    }
}
