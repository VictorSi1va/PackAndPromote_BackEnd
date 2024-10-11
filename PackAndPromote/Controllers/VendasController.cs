using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VendasController : Controller
    {
        private readonly DbPackAndPromote _dbPackAndPromote;

        public VendasController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        [HttpGet("ListarLojas")]
        public ActionResult<IEnumerable<Loja>> ListarLojas()
        {
            var lojas = _dbPackAndPromote.Loja.ToList();

            return Ok(lojas);
        }

        [HttpGet("PesquisarLoja/{id}")]
        public ActionResult<Loja> PesquisarLoja(int id)
        {
            var loja = _dbPackAndPromote.Loja.Find(id);

            if (loja == null)
                return NotFound();

            return Ok(loja);
        }

        [HttpPost("CriarLoja")]
        public ActionResult<Loja> CriarLoja(Loja loja)
        {
            _dbPackAndPromote.Loja.Add(loja);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarLoja), new { id = loja.IdLoja }, loja);
        }

        [HttpPut("AlterarLoja/{id}")]
        public IActionResult AlterarLoja(int id, LojaDto lojaDto)
        {
            var loja = _dbPackAndPromote.Loja.Find(id);

            if (loja == null)
                return NotFound();

            loja.NomeLoja = lojaDto.NomeLoja;
            loja.EnderecoLoja = lojaDto.EnderecoLoja;
            loja.DescricaoLoja = lojaDto.DescricaoLoja;
            loja.TelefoneLoja = lojaDto.TelefoneLoja;
            loja.CNPJLoja = lojaDto.CNPJLoja;
            loja.EmailLoja = lojaDto.EmailLoja;
            loja.DataCriacao = lojaDto.DataCriacao;

            _dbPackAndPromote.SaveChanges();

            return Ok(loja);
        }

        [HttpDelete("ExcluirLoja/{id}")]
        public IActionResult ExcluirLoja(int id)
        {
            var loja = _dbPackAndPromote.Loja.Find(id);

            if (loja == null)
                return NotFound();

            _dbPackAndPromote.Loja.Remove(loja);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }


        [HttpGet("ListarPedidosEmbalagem")]
        public ActionResult<IEnumerable<PedidoEmbalagem>> ListarPedidosEmbalagem()
        {
            var pedidos = _dbPackAndPromote.PedidoEmbalagem.ToList();

            return Ok(pedidos);
        }

        [HttpGet("PesquisarPedidoEmbalagem/{id}")]
        public ActionResult<PedidoEmbalagem> PesquisarPedidoEmbalagem(int id)
        {
            var pedido = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpPost("CriarPedidoEmbalagem")]
        public ActionResult<PedidoEmbalagem> CriarPedidoEmbalagem(PedidoEmbalagem pedido)
        {
            _dbPackAndPromote.PedidoEmbalagem.Add(pedido);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarPedidoEmbalagem), new { id = pedido.IdPedidoEmbalagem }, pedido);
        }

        [HttpPut("AlterarPedidoEmbalagem/{id}")]
        public IActionResult AlterarPedidoEmbalagem(int id, PedidoEmbalagemDto pedidoDto)
        {
            var pedido = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            if (pedido == null)
                return NotFound();

            pedido.Quantidade = pedidoDto.Quantidade;
            pedido.DescricaoPersonalizada = pedidoDto.DescricaoPersonalizada;
            pedido.StatusPedido = pedidoDto.StatusPedido;
            pedido.DataPedido = pedidoDto.DataPedido;

            _dbPackAndPromote.SaveChanges();

            return Ok(pedido);
        }

        [HttpDelete("ExcluirPedidoEmbalagem/{id}")]
        public IActionResult ExcluirPedidoEmbalagem(int id)
        {
            var pedido = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            if (pedido == null)
                return NotFound();

            _dbPackAndPromote.PedidoEmbalagem.Remove(pedido);
            _dbPackAndPromote.SaveChanges();

            return Ok();
        }
    }
}
