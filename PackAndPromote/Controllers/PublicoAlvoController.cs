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

        public PublicoAlvoController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        [HttpGet("ListarPublicosAlvo")]
        public ActionResult<IEnumerable<PublicoAlvo>> ListarPublicosAlvo()
        {
            var listaRetorno = _dbPackAndPromote.PublicoAlvo.ToList();

            return Ok(listaRetorno);
        }

        [HttpGet("PesquisarPublicoAlvo/{id}")]
        public ActionResult<PublicoAlvo> PesquisarPublicoAlvo(int id)
        {
            var publicoAlvo = _dbPackAndPromote.PublicoAlvo.Find(id);

            if (publicoAlvo == null)
                return NotFound();

            return Ok(publicoAlvo);
        }

        [HttpPost("CriarPublicoAlvo")]
        public ActionResult<PublicoAlvo> CriarPublicoAlvo(PublicoAlvoDto publicoAlvoDto)
        {
            if (string.IsNullOrWhiteSpace(publicoAlvoDto.DescricaoPublicoAlvo))
                return BadRequest("A descrição do Público Alvo não pode ser vazia ou nula.");

            PublicoAlvo publicoAlvo = new PublicoAlvo
            {
                DescricaoPublicoAlvo = publicoAlvoDto.DescricaoPublicoAlvo
            };

            _dbPackAndPromote.PublicoAlvo.Add(publicoAlvo);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarPublicoAlvo), new { id = publicoAlvo.IdPublicoAlvo }, publicoAlvo);
        }

        [HttpPut("AlterarPublicoAlvo/{id}")]
        public IActionResult AlterarPublicoAlvo(int id, PublicoAlvoDto publicoAlvoDto)
        {
            var publicoAlvo = _dbPackAndPromote.PublicoAlvo.Find(id);

            if (publicoAlvo == null)
                return NotFound();

            publicoAlvo.DescricaoPublicoAlvo = publicoAlvoDto.DescricaoPublicoAlvo;

            _dbPackAndPromote.SaveChanges();

            return Ok(publicoAlvo);
        }

        [HttpDelete("ExcluirPublicoAlvo/{id}")]
        public IActionResult ExcluirPublicoAlvo(int id)
        {
            var publicoAlvo = _dbPackAndPromote.PublicoAlvo.Find(id);

            if (publicoAlvo == null)
                return NotFound();

            _dbPackAndPromote.PublicoAlvo.Remove(publicoAlvo);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }
    }
}
