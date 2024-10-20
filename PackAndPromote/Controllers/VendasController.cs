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
        public ActionResult<IEnumerable<LojaDto>> ListarLojas()
        {
            var lojas = _dbPackAndPromote.Loja
                                         .Select(xs => new LojaDto
                                         {
                                             IdLoja = xs.IdLoja,
                                             NomeLoja = xs.NomeLoja,
                                             EnderecoLoja = xs.EnderecoLoja,
                                             DescricaoLoja = xs.DescricaoLoja,
                                             TelefoneLoja = xs.TelefoneLoja,
                                             CNPJLoja = xs.CNPJLoja,
                                             EmailLoja = xs.EmailLoja,
                                             DataCriacao = xs.DataCriacao,
                                         })
                                         .ToList();

            return Ok(lojas);
        }

        [HttpGet("PesquisarLoja/{id}")]
        public ActionResult<LojaDto> PesquisarLoja(int id)
        {
            var loja = _dbPackAndPromote.Loja.Find(id);

            if (loja == null)
                return NotFound();

            LojaDto lojaPesquisada = new LojaDto
            {
                IdLoja = loja.IdLoja,
                NomeLoja = loja.NomeLoja,
                EnderecoLoja = loja.EnderecoLoja,
                DescricaoLoja = loja.DescricaoLoja,
                TelefoneLoja = loja.TelefoneLoja,
                CNPJLoja = loja.CNPJLoja,
                EmailLoja = loja.EmailLoja,
                DataCriacao = loja.DataCriacao,
            };

            return Ok(lojaPesquisada);
        }

        [HttpPut("AlterarLoja/{id}")]
        public IActionResult AlterarLoja(int id, LojaAlteradaDto lojaAlteradaDto)
        {
            var loja = _dbPackAndPromote.Loja.Find(id);

            if (loja == null)
                return NotFound();

            loja.NomeLoja = lojaAlteradaDto.NomeLoja;
            loja.EnderecoLoja = lojaAlteradaDto.EnderecoLoja;
            loja.DescricaoLoja = lojaAlteradaDto.DescricaoLoja;
            loja.TelefoneLoja = lojaAlteradaDto.TelefoneLoja;
            loja.CNPJLoja = lojaAlteradaDto.CNPJLoja;
            loja.EmailLoja = lojaAlteradaDto.EmailLoja;

            _dbPackAndPromote.SaveChanges();

            return Ok("Loja alterada com sucesso!");
        }

        [HttpGet("ListarPedidosEmbalagem")]
        public ActionResult<IEnumerable<PedidoEmbalagemDto>> ListarPedidosEmbalagem()
        {
            var pedidos = _dbPackAndPromote.PedidoEmbalagem
                                           .Select(xs => new PedidoEmbalagemDto
                                           {
                                               IdPedidoEmbalagem = xs.IdPedidoEmbalagem,
                                               Quantidade = xs.Quantidade,
                                               DescricaoPersonalizada = xs.DescricaoPersonalizada,
                                               StatusPedido = xs.StatusPedido,
                                               DataPedido = xs.DataPedido,

                                               IdLoja = xs.IdLoja,
                                               IdLojaDelivery = xs.IdLojaDelivery,
                                               IdLojaEmbalagem = xs.IdLojaEmbalagem
                                           })
                                           .ToList();

            foreach (var itemPedido in pedidos)
            {
                var loja = _dbPackAndPromote.Loja.Find(itemPedido.IdLoja);
                var lojaDelivery = _dbPackAndPromote.Loja.Find(itemPedido.IdLojaDelivery);
                var lojaEmbalagem = _dbPackAndPromote.Loja.Find(itemPedido.IdLojaEmbalagem);

                itemPedido.NomeLoja = loja.NomeLoja;
                itemPedido.NomeLojaDelivery = lojaDelivery.NomeLoja;
                itemPedido.NomeLojaEmbalagem = lojaEmbalagem.NomeLoja;
            }

            return Ok(pedidos);
        }

        [HttpGet("PesquisarPedidoEmbalagem/{id}")]
        public ActionResult<PedidoEmbalagemDto> PesquisarPedidoEmbalagem(int id)
        {
            var pedido = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            if (pedido == null)
                return NotFound();

            var loja = _dbPackAndPromote.Loja.Find(pedido.IdLoja);
            var lojaDelivery = _dbPackAndPromote.Loja.Find(pedido.IdLojaDelivery);
            var lojaEmbalagem = _dbPackAndPromote.Loja.Find(pedido.IdLojaEmbalagem);

            PedidoEmbalagemDto pedidoEmbalagem = new PedidoEmbalagemDto
            {
                IdPedidoEmbalagem = pedido.IdPedidoEmbalagem,
                Quantidade = pedido.Quantidade,
                DescricaoPersonalizada = pedido.DescricaoPersonalizada,
                StatusPedido = pedido.StatusPedido,
                DataPedido = pedido.DataPedido,

                IdLoja = loja.IdLoja,
                NomeLoja = loja.NomeLoja,

                IdLojaDelivery = lojaDelivery.IdLoja,
                NomeLojaDelivery = lojaDelivery.NomeLoja,

                IdLojaEmbalagem = lojaEmbalagem.IdLoja,
                NomeLojaEmbalagem = lojaEmbalagem.NomeLoja,
            };

            return Ok(pedidoEmbalagem);
        }

        [HttpPost("CriarPedidoEmbalagem")]
        public ActionResult<PedidoEmbalagemSimplesDto> CriarPedidoEmbalagem(PedidoEmbalagemSimplesDto pedido)
        {
            if (pedido.Quantidade <= 0)
                return BadRequest("A quantidade deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(pedido.DescricaoPersonalizada))
                return BadRequest("A descrição personalizada não pode ser vazia ou nula.");

            if (string.IsNullOrWhiteSpace(pedido.StatusPedido))
                return BadRequest("O status do pedido não pode ser vazio ou nulo.");

            if (pedido.DataPedido == default || pedido.DataPedido < DateTime.Now)
                return BadRequest("A data do pedido não pode ser nula ou inferior à data atual.");

            if (pedido.IdLoja <= 0 || 
                pedido.IdLojaDelivery <= 0 || 
                pedido.IdLojaEmbalagem <= 0)
                return BadRequest("Os IDs das lojas devem ser maiores que zero.");

            PedidoEmbalagem pedidoEmbalagem = new PedidoEmbalagem
            {
                Quantidade = pedido.Quantidade,
                DescricaoPersonalizada = pedido.DescricaoPersonalizada,
                StatusPedido = pedido.StatusPedido,
                DataPedido = pedido.DataPedido,

                IdLoja = pedido.IdLoja,
                IdLojaDelivery = pedido.IdLojaDelivery,
                IdLojaEmbalagem = pedido.IdLojaEmbalagem
            };

            _dbPackAndPromote.PedidoEmbalagem.Add(pedidoEmbalagem);
            _dbPackAndPromote.SaveChanges();

            return CreatedAtAction(nameof(PesquisarPedidoEmbalagem), new { id = pedidoEmbalagem.IdPedidoEmbalagem }, pedidoEmbalagem);
        }

        [HttpPut("AlterarPedidoEmbalagem/{id}")]
        public IActionResult AlterarPedidoEmbalagem(int id, PedidoEmbalagemAlteradoDto pedidoAlteradoDto)
        {
            var pedido = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            if (pedido == null)
                return NotFound();

            pedido.Quantidade = pedidoAlteradoDto.Quantidade;
            pedido.DescricaoPersonalizada = pedidoAlteradoDto.DescricaoPersonalizada;
            pedido.StatusPedido = pedidoAlteradoDto.StatusPedido;

            _dbPackAndPromote.SaveChanges();

            return Ok("Pedido alterado com sucesso!");
        }

        [HttpDelete("ExcluirPedidoEmbalagem/{id}")]
        public IActionResult ExcluirPedidoEmbalagem(int id)
        {
            var pedidoEmbalagem = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            if (pedidoEmbalagem == null)
                return NotFound();

            _dbPackAndPromote.PedidoEmbalagem.Remove(pedidoEmbalagem);
            _dbPackAndPromote.SaveChanges();

            return Ok("Pedido excluído com sucesso!");
        }
    }
}
