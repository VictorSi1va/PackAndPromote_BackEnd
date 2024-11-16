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
        [HttpPost("SalvarImagem")]
        public async Task<IActionResult> SalvarImagem([FromBody] ImagemDto imagemDto)
        {
            if (imagemDto == null)
                return BadRequest("Dados da imagem são obrigatórios.");

            if (imagemDto.DadosImagem == null || imagemDto.DadosImagem.Length == 0)
                return BadRequest("Conteúdo da imagem é obrigatório.");

            if (string.IsNullOrWhiteSpace(imagemDto.TipoExtensao))
                return BadRequest("Extensão da imagem é obrigatório.");

            if (string.IsNullOrWhiteSpace(imagemDto.NomeImagem))
                return BadRequest("Nome da imagem é obrigatório.");

            var novaImagem = new Imagem
            {
                DadosImagem = imagemDto.DadosImagem,
                TipoExtensao = imagemDto.TipoExtensao,
                NomeImagem = imagemDto.NomeImagem,
                DataCriacao = DateTime.Now,
            };

            _dbPackAndPromote.Imagem.Add(novaImagem);
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