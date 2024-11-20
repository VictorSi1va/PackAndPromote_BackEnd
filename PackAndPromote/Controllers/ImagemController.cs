using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagemController : Controller
    {
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _dbPackAndPromote;

        public ImagemController(DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context; // Injeta o contexto de banco de dados na controller.
        }

        #endregion

        #region Imagem

        #region Pesquisar Imagem
        [HttpGet("PesquisarImagem/{id}")]
        public async Task<IActionResult> PesquisarImagem(int id)
        {
            if (id <= 0)
                return BadRequest("Imagem não encontrada.");

            var imagem = await _dbPackAndPromote.Imagem.FindAsync(id);

            if (imagem == null)
                return NotFound("Imagem não encontrada.");

            return File(imagem.DadosImagem, imagem.TipoExtensao);
        }
        #endregion

        #region Salvar Imagem
        [Authorize]
        [HttpPost("SalvarImagem/{idUsuarioLogado}")]
        public async Task<IActionResult> SalvarImagem(int idUsuarioLogado, [FromBody] ImagemDto imagemDto)
        {
            // Validação inicial
            if (imagemDto == null || imagemDto.DadosImagem == null || imagemDto.DadosImagem.Length == 0)
                return BadRequest("Conteúdo da imagem é obrigatório.");

            if (string.IsNullOrWhiteSpace(imagemDto.TipoExtensao))
                return BadRequest("Extensão da imagem é obrigatória.");

            if (string.IsNullOrWhiteSpace(imagemDto.NomeImagem))
                return BadRequest("Nome da imagem é obrigatório.");

            // Obtendo o IdLoja do usuário logado
            var idLoja = _dbPackAndPromote.Usuario
                                .Where(u => u.IdUsuario == idUsuarioLogado)
                                .Select(u => u.IdLoja)
                                .FirstOrDefault();

            // Verifica se a loja foi encontrada
            if (idLoja == 0)
                return NotFound("Loja não encontrada para o usuário.");

            // Remover os relacionamentos LojaImagem, exceto para o IdImagem == 3
            var lojaImagensAtuais = _dbPackAndPromote.LojaImagem
                                              .Where(li => li.IdLoja == idLoja && li.IdImagem != 3)
                                              .ToList();

            // Remover os relacionamentos LojaImagem (exceto o que tem IdImagem == 3)
            if (lojaImagensAtuais.Any())
            {
                _dbPackAndPromote.LojaImagem.RemoveRange(lojaImagensAtuais);
            }

            // Remover as imagens associadas à loja, exceto a imagem com IdImagem == 3
            var imagensRemover = _dbPackAndPromote.Imagem
                                                  .Where(i => lojaImagensAtuais
                                                  .Select(li => li.IdImagem).Contains(i.IdImagem))
                                                  .ToList();

            if (imagensRemover.Any())
            {
                _dbPackAndPromote.Imagem.RemoveRange(imagensRemover);
            }

            // Criar e adicionar a nova imagem
            var novaImagem = new Imagem
            {
                DadosImagem = imagemDto.DadosImagem,
                TipoExtensao = imagemDto.TipoExtensao,
                NomeImagem = imagemDto.NomeImagem,
                DataCriacao = DateTime.Now
            };

            _dbPackAndPromote.Imagem.Add(novaImagem);

            // Adicionar relacionamento LojaImagem
            var novaLojaImagem = new LojaImagem
            {
                IdImagem = novaImagem.IdImagem,
                IdLoja = idLoja
            };

            _dbPackAndPromote.LojaImagem.Add(novaLojaImagem);

            // Remover o relacionamento LojaImagem da imagem com IdImagem == 3
            var lojaImagemComId3 = _dbPackAndPromote.LojaImagem
                                                    .FirstOrDefault(li => li.IdLoja == idLoja &&
                                                                          li.IdImagem == 3);

            if (lojaImagemComId3 != null)
            {
                _dbPackAndPromote.LojaImagem.Remove(lojaImagemComId3);
            }

            await _dbPackAndPromote.SaveChangesAsync();
            return Ok(novaImagem.IdImagem);
        }
        #endregion

        #region Excluir Imagem
        [Authorize]
        [HttpDelete("ExcluirImagem/{id}")]
        public async Task<IActionResult> ExcluirImagem(int id)
        {
            if (id <= 0)
                return BadRequest("Imagem não encontrada.");

            var imagem = await _dbPackAndPromote.Imagem.FindAsync(id);

            if (imagem == null)
                return NotFound("Imagem não encontrada.");

            _dbPackAndPromote.Imagem.Remove(imagem);
            await _dbPackAndPromote.SaveChangesAsync();

            return Ok("Imagem excluída com sucesso.");
        }
        #endregion

        #endregion
    }
}