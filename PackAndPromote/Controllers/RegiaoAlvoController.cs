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
        private readonly DbPackAndPromote _dbPackAndPromote;

        public RegiaoAlvoController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        [HttpGet("ListarRegioesAlvo")]
        public ActionResult<IEnumerable<RegiaoAlvo>> ListarRegioesAlvo()
        {
            var regioes = _dbPackAndPromote.RegiaoAlvo.ToList();

            return Ok(regioes);
        }

        [HttpGet("PesquisarRegiaoAlvo/{id}")]
        public ActionResult<RegiaoAlvo> PesquisarRegiaoAlvo(int id)
        {
            var regiaoAlvo = _dbPackAndPromote.RegiaoAlvo.Find(id);

            if (regiaoAlvo == null)
                return NotFound();

            return Ok(regiaoAlvo);
        }

        [HttpPost("CriarRegiaoAlvo")]
        public ActionResult<RegiaoAlvo> CriarRegiaoAlvo(RegiaoAlvoDto regiaoAlvoDto)
        {
            RegiaoAlvo regiaoAlvo = new RegiaoAlvo
            {
                NomeRegiaoAlvo = regiaoAlvoDto.NomeRegiaoAlvo
            };

            _dbPackAndPromote.RegiaoAlvo.Add(regiaoAlvo);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarRegiaoAlvo), new { id = regiaoAlvo.IdRegiaoAlvo }, regiaoAlvo);
        }

        [HttpPut("AlterarRegiaoAlvo/{id}")]
        public IActionResult AlterarRegiaoAlvo(int id, RegiaoAlvoDto regiaoAlvoDto)
        {
            var regiaoAlvo = _dbPackAndPromote.RegiaoAlvo.Find(id);

            if (regiaoAlvo == null)
                return NotFound();

            regiaoAlvo.NomeRegiaoAlvo = regiaoAlvoDto.NomeRegiaoAlvo;

            _dbPackAndPromote.SaveChanges();

            return Ok(regiaoAlvo);
        }

        [HttpDelete("ExcluirRegiaoAlvo/{id}")]
        public IActionResult ExcluirRegiaoAlvo(int id)
        {
            var regiaoAlvo = _dbPackAndPromote.RegiaoAlvo.Find(id);

            if (regiaoAlvo == null)
                return NotFound();

            _dbPackAndPromote.RegiaoAlvo.Remove(regiaoAlvo);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }
    }
}
