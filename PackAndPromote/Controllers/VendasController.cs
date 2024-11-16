using Microsoft.AspNetCore.Authorization;
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
        #region Variáveis e Construtor

        // Contexto do banco de dados
        private readonly DbPackAndPromote _dbPackAndPromote;

        // Construtor da controller que recebe o contexto
        public VendasController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }

        #endregion

        #region Vendas - Métodos

        #region Listar Lojas
        // Endpoint para listar todas as lojas
        [HttpGet("ListarLojas")]
        public ActionResult<IEnumerable<LojaDto>> ListarLojas()
        {
            // Obtém a lista de lojas do banco de dados e mapeia para LojaDto
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

            // Retorna as lojas encapsuladas em um resultado Ok
            return Ok(lojas);
        }
        #endregion

        #region Listar Novas Parcerias
        [Authorize]
        [HttpGet("ListarNovasParcerias/{id}")]
        public ActionResult<IEnumerable<ParceriaCardDto>> ListarNovasParcerias(int id)
        {
            var lojas = _dbPackAndPromote.Loja
                                         .Where(xs => xs.IdLoja != id)
                                         .Take(7)
                                         .Select(xs => new ParceriaCardDto
                                         {
                                             IdLoja = xs.IdLoja,
                                             NomeLoja = xs.NomeLoja,
                                             IdImagemLoja = 1, // TODO Pegar da Imagem salva no BD
                                         })
                                         .ToList();

            return Ok(lojas);
        }
        #endregion

        #region Listar Parcerias Atuais
        [Authorize]
        [HttpGet("ListarParceriasAtuais/{id}")]
        public ActionResult<IEnumerable<ParceriaCardDto>> ListarParceriasAtuais(int id)
        {
            var lojas = _dbPackAndPromote.Loja
                                         .Where(xs => xs.IdLoja != id)
                                         .Take(7)
                                         .Select(xs => new ParceriaCardDto
                                         {
                                             IdLoja = xs.IdLoja,
                                             NomeLoja = xs.NomeLoja,
                                             IdImagemLoja = 1, // TODO Pegar da Imagem salva no BD
                                         })
                                         .ToList();

            return Ok(lojas);
        }
        #endregion

        #region Listar Parcerias Solicitadas
        [Authorize]
        [HttpGet("ListarParceriasSolicitadas/{id}")]
        public ActionResult<IEnumerable<ParceriaCardDto>> ListarParceriasSolicitadas(int id)
        {
            var lojas = _dbPackAndPromote.Loja
                                         .Where(xs => xs.IdLoja != id)
                                         .Take(7)
                                         .Select(xs => new ParceriaCardDto
                                         {
                                             IdLoja = xs.IdLoja,
                                             NomeLoja = xs.NomeLoja,
                                             IdImagemLoja = 1, // TODO Pegar da Imagem salva no BD
                                         })
                                         .ToList();

            return Ok(lojas);
        }
        #endregion

        #region Pesquisar Loja por Id
        // Endpoint para pesquisar uma loja pelo ID
        [Authorize]
        [HttpGet("PesquisarLoja/{id}")]
        public ActionResult<DetalhesLojaDto> PesquisarLoja(int id)
        {
            // Busca a loja pelo ID no banco de dados
            var loja = _dbPackAndPromote.Loja.Find(id);

            // Se a loja não for encontrada, retorna um NotFound
            if (loja == null)
                return NotFound();

            // Cria um objeto LojaDto a partir da loja encontrada
            DetalhesLojaDto lojaPesquisada = new DetalhesLojaDto
            {
                IdLoja = loja.IdLoja,
                NomeLoja = loja.NomeLoja,
                EnderecoLoja = loja.EnderecoLoja,
                TelefoneLoja = loja.TelefoneLoja,
                EmailLoja = loja.EmailLoja,
                DescricaoLoja = loja.DescricaoLoja,
                IdImagemLoja = 1 // TODO Pegar da Imagem salva no BD
            };

            var lojaPublicoAlvo = _dbPackAndPromote.LojaPublicoAlvo
                                                .Where(xs => xs.IdLoja == loja.IdLoja)
                                                .Include(xs => xs.PublicoAlvo)
                                                .ToList();

            // Adiciona os Públicos-Alvo à lojaPesquisada sem vírgula extra
            for (int i = 0; i < lojaPublicoAlvo.Count; i++)
            {
                lojaPesquisada.PublicoAlvoLoja += lojaPublicoAlvo[i].PublicoAlvo.DescricaoPublicoAlvo;
                if (i < lojaPublicoAlvo.Count - 1) // Se não for o último item
                {
                    lojaPesquisada.PublicoAlvoLoja += ", ";
                }
            }

            var lojaFaixaEtaria = _dbPackAndPromote.LojaFaixaEtaria
                                    .Where(xs => xs.IdLoja == loja.IdLoja)
                                    .Include(xs => xs.FaixaEtaria)
                                    .ToList();

            // Adiciona as Faixas Etárias à lojaPesquisada sem vírgula extra
            for (int i = 0; i < lojaFaixaEtaria.Count; i++)
            {
                lojaPesquisada.FaixaEtariaLoja += lojaFaixaEtaria[i].FaixaEtaria.DescricaoFaixaEtaria;
                if (i < lojaFaixaEtaria.Count - 1) // Se não for o último item
                {
                    lojaPesquisada.FaixaEtariaLoja += ", ";
                }
            }

            var lojaRegiaoAlvo = _dbPackAndPromote.LojaRegiaoAlvo
                                                    .Where(xs => xs.IdLoja == loja.IdLoja)
                                                    .Include(xs => xs.RegiaoAlvo)
                                                    .ToList();

            // Adiciona as Regiões-Alvo à lojaPesquisada sem vírgula extra
            for (int i = 0; i < lojaRegiaoAlvo.Count; i++)
            {
                lojaPesquisada.RegiaoLoja += lojaRegiaoAlvo[i].RegiaoAlvo.NomeRegiaoAlvo;
                if (i < lojaRegiaoAlvo.Count - 1) // Se não for o último item
                {
                    lojaPesquisada.RegiaoLoja += ", ";
                }
            }

            var lojaPreferenciaAlvo = _dbPackAndPromote.LojaPreferenciaAlvo
                                                .Where(xs => xs.IdLoja == loja.IdLoja)
                                                .Include(xs => xs.PreferenciaAlvo)
                                                .ToList();

            // Adiciona as Preferências-Alvo à lojaPesquisada sem vírgula extra
            for (int i = 0; i < lojaPreferenciaAlvo.Count; i++)
            {
                lojaPesquisada.PreferenciaParceriasLoja += lojaPreferenciaAlvo[i].PreferenciaAlvo.DescricaoPreferenciaAlvo;
                if (i < lojaPreferenciaAlvo.Count - 1) // Se não for o último item
                {
                    lojaPesquisada.PreferenciaParceriasLoja += ", ";
                }
            }

            // Retorna a loja encontrada encapsulada em um resultado Ok
            return Ok(lojaPesquisada);
        }

        #endregion

        #region Alterar Loja
        // Endpoint para alterar os dados de uma loja existente
        [Authorize]
        [HttpPut("AlterarLoja/{id}")]
        public IActionResult AlterarLoja(int id, LojaAlteradaDto lojaAlteradaDto)
        {
            // Busca a loja pelo ID
            var loja = _dbPackAndPromote.Loja.Find(id);

            // Se a loja não for encontrada, retorna um NotFound
            if (loja == null)
                return NotFound();

            // Atualiza os dados da loja com os dados do DTO
            loja.NomeLoja = lojaAlteradaDto.NomeLoja;
            loja.EnderecoLoja = lojaAlteradaDto.EnderecoLoja;
            loja.DescricaoLoja = lojaAlteradaDto.DescricaoLoja;
            loja.TelefoneLoja = lojaAlteradaDto.TelefoneLoja;
            loja.CNPJLoja = lojaAlteradaDto.CNPJLoja;
            loja.EmailLoja = lojaAlteradaDto.EmailLoja;

            // Salva as alterações no contexto
            _dbPackAndPromote.SaveChanges();

            // Retorna uma mensagem de sucesso
            return Ok("Loja alterada com sucesso!");
        }
        #endregion

        #region Listar Pedidos Embalagem
        // Endpoint para listar todos os pedidos de embalagem
        [Authorize]
        [HttpGet("ListarPedidosEmbalagem")]
        public ActionResult<IEnumerable<PedidoEmbalagemDto>> ListarPedidosEmbalagem()
        {
            // Obtém a lista de pedidos de embalagem do banco de dados e mapeia para PedidoEmbalagemDto
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

            // Retorna os pedidos encapsulados em um resultado Ok
            return Ok(pedidos);
        }
        #endregion

        #region Pesquisar Pedido Embalagem
        [Authorize]
        [HttpGet("PesquisarPedidoEmbalagem/{id}")]
        public ActionResult<PedidoEmbalagemDto> PesquisarPedidoEmbalagem(int id)
        {
            // Busca o pedido de embalagem pelo ID fornecido
            var pedido = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            // Se o pedido não for encontrado, retorna um status 404 NotFound
            if (pedido == null)
                return NotFound();

            // Busca as lojas associadas ao pedido
            var loja = _dbPackAndPromote.Loja.Find(pedido.IdLoja);
            var lojaDelivery = _dbPackAndPromote.Loja.Find(pedido.IdLojaDelivery);
            var lojaEmbalagem = _dbPackAndPromote.Loja.Find(pedido.IdLojaEmbalagem);

            // Cria um objeto DTO com os detalhes do pedido e das lojas
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

            // Retorna um status 200 OK com os dados do pedido de embalagem
            return Ok(pedidoEmbalagem);
        }
        #endregion

        #region Criar Pedido Embalagem
        [Authorize]
        [HttpPost("CriarPedidoEmbalagem")]
        public ActionResult<PedidoEmbalagemSimplesDto> CriarPedidoEmbalagem(PedidoEmbalagemSimplesDto pedido)
        {
            // Valida a quantidade do pedido
            if (pedido.Quantidade <= 0)
                return BadRequest("A quantidade deve ser maior que zero.");

            // Valida a descrição personalizada do pedido
            if (string.IsNullOrWhiteSpace(pedido.DescricaoPersonalizada))
                return BadRequest("A descrição personalizada não pode ser vazia ou nula.");

            // Valida o status do pedido
            if (string.IsNullOrWhiteSpace(pedido.StatusPedido))
                return BadRequest("O status do pedido não pode ser vazio ou nulo.");

            // Valida a data do pedido
            if (pedido.DataPedido == default || pedido.DataPedido < DateTime.Now)
                return BadRequest("A data do pedido não pode ser nula ou inferior à data atual.");

            // Valida os IDs das lojas
            if (pedido.IdLoja <= 0 ||
                pedido.IdLojaDelivery <= 0 ||
                pedido.IdLojaEmbalagem <= 0)
                return BadRequest("Os IDs das lojas devem ser maiores que zero.");

            // Cria um novo objeto de pedido de embalagem
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

            // Adiciona o novo pedido de embalagem ao banco de dados
            _dbPackAndPromote.PedidoEmbalagem.Add(pedidoEmbalagem);
            _dbPackAndPromote.SaveChanges();

            // Retorna um status 201 Created com a localização do novo pedido
            return CreatedAtAction(nameof(PesquisarPedidoEmbalagem), new { id = pedidoEmbalagem.IdPedidoEmbalagem }, pedidoEmbalagem);
        }
        #endregion

        #region Alterar Pedido Embalagem
        [Authorize]
        [HttpPut("AlterarPedidoEmbalagem/{id}")]
        public IActionResult AlterarPedidoEmbalagem(int id, PedidoEmbalagemAlteradoDto pedidoAlteradoDto)
        {
            // Busca o pedido de embalagem pelo ID fornecido
            var pedido = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            // Se o pedido não for encontrado, retorna um status 404 NotFound
            if (pedido == null)
                return NotFound();

            // Atualiza os dados do pedido com as informações do DTO
            pedido.Quantidade = pedidoAlteradoDto.Quantidade;
            pedido.DescricaoPersonalizada = pedidoAlteradoDto.DescricaoPersonalizada;
            pedido.StatusPedido = pedidoAlteradoDto.StatusPedido;

            // Salva as alterações no banco de dados
            _dbPackAndPromote.SaveChanges();

            // Retorna um status 200 OK com uma mensagem de sucesso
            return Ok("Pedido alterado com sucesso!");
        }
        #endregion

        #region Excluir Pedido Embalagem
        [Authorize]
        [HttpDelete("ExcluirPedidoEmbalagem/{id}")]
        public IActionResult ExcluirPedidoEmbalagem(int id)
        {
            // Busca o pedido de embalagem pelo ID fornecido
            var pedidoEmbalagem = _dbPackAndPromote.PedidoEmbalagem.Find(id);

            // Se o pedido não for encontrado, retorna um status 404 NotFound
            if (pedidoEmbalagem == null)
                return NotFound();

            // Remove o pedido de embalagem do banco de dados
            _dbPackAndPromote.PedidoEmbalagem.Remove(pedidoEmbalagem);
            _dbPackAndPromote.SaveChanges();

            // Retorna um status 200 OK com uma mensagem de sucesso
            return Ok("Pedido excluído com sucesso!");
        }
        #endregion

        #endregion
    }
}