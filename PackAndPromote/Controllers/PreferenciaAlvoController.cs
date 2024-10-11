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
        private readonly DbPackAndPromote _dbPackAndPromote;

        public PreferenciaAlvoController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        [HttpGet("ListarPreferenciasAlvo")]
        public ActionResult<IEnumerable<PreferenciaAlvo>> ListarPreferenciasAlvo()
        {
            var listaRetorno = _dbPackAndPromote.PreferenciaAlvo.ToList();

            return Ok(listaRetorno);
        }

        [HttpGet("PesquisarPreferenciaAlvo/{id}")]
        public ActionResult<PreferenciaAlvo> PesquisarPreferenciaAlvo(int id)
        {
            var preferenciaAlvo = _dbPackAndPromote.PreferenciaAlvo.Find(id);

            if (preferenciaAlvo == null)
                return NotFound();

            return Ok(preferenciaAlvo);
        }

        [HttpPost("CriarPreferenciaAlvo")]
        public ActionResult<PreferenciaAlvo> CriarPreferenciaAlvo(PreferenciaAlvoDto preferenciaAlvoDto)
        {
            PreferenciaAlvo preferenciaAlvo = new PreferenciaAlvo
            {
                DescricaoPreferenciaAlvo = preferenciaAlvoDto.DescricaoPreferenciaAlvo
            };

            _dbPackAndPromote.PreferenciaAlvo.Add(preferenciaAlvo);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarPreferenciaAlvo), new { id = preferenciaAlvo.IdPreferenciaAlvo }, preferenciaAlvo);
        }

        [HttpPut("AlterarPreferenciaAlvo/{id}")]
        public IActionResult AlterarPreferenciaAlvo(int id, PreferenciaAlvoDto preferenciaAlvoDto)
        {
            var preferenciaAlvo = _dbPackAndPromote.PreferenciaAlvo.Find(id);

            if (preferenciaAlvo == null)
                return NotFound();

            preferenciaAlvo.DescricaoPreferenciaAlvo = preferenciaAlvoDto.DescricaoPreferenciaAlvo;

            _dbPackAndPromote.SaveChanges();

            return Ok(preferenciaAlvo);
        }

        [HttpDelete("ExcluirPreferenciaAlvo/{id}")]
        public IActionResult ExcluirPreferenciaAlvo(int id)
        {
            var preferenciaAlvo = _dbPackAndPromote.PreferenciaAlvo.Find(id);

            if (preferenciaAlvo == null)
                return NotFound();

            _dbPackAndPromote.PreferenciaAlvo.Remove(preferenciaAlvo);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }
    }
}
