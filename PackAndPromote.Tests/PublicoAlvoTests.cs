using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class PublicoAlvoTests
    {
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _context;
        private readonly PublicoAlvoController _controller;

        // Construtor para configurar o contexto do banco de dados em memória e a controller para testes
        public PublicoAlvoTests()
        {
            // Configura o banco de dados em memória, usado para realizar testes sem persistência real
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options); // Inicializa o contexto com a configuração em memória
            _controller = new PublicoAlvoController(_context); // Inicializa a controller com o contexto
        }

        #endregion

        #region Público Alvo - Testes

        #region RetornaOkResultComListaVaziaQuandoNaoExistirPublicoAlvoAoListar
        [Fact]
        public void RetornaOkResultComListaVaziaQuandoNaoExistirPublicoAlvoAoListar()
        {
            // Act: Executa o método ListarPublicosAlvo na controller
            var result = _controller.ListarPublicosAlvo();

            // Assert: Verifica se o resultado é do tipo OkObjectResult e se a lista está vazia
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var publicosAlvo = Assert.IsAssignableFrom<IEnumerable<PublicoAlvo>>(okResult.Value);
            Assert.Empty(publicosAlvo); // Verifica se a lista retornada está vazia
        }
        #endregion

        #region RetornaBadRequestResultQuandoDescricaoForNulaOuVaziaAoCriarPublicoAlvo
        [Fact]
        public void RetornaBadRequestResultQuandoDescricaoForNulaOuVaziaAoCriarPublicoAlvo()
        {
            // Arrange: Configura o DTO com a descrição nula
            var publicoAlvoDto = new PublicoAlvoDto { DescricaoPublicoAlvo = null };

            // Act: Executa o método CriarPublicoAlvo na controller
            var result = _controller.CriarPublicoAlvo(publicoAlvoDto);

            // Assert: Verifica se o resultado é do tipo BadRequestObjectResult
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        #endregion

        #region RetornaNotFoundResultQuandoPublicoAlvoNaoExistirAoExcluir
        [Fact]
        public void RetornaNotFoundResultQuandoPublicoAlvoNaoExistirAoExcluir()
        {
            // Arrange: Define um ID inválido que não existe no banco de dados
            int invalidId = 999; // ID que não existe

            // Act: Executa o método ExcluirPublicoAlvo com o ID inválido
            var result = _controller.ExcluirPublicoAlvo(invalidId);

            // Assert: Verifica se o resultado é do tipo NotFoundResult
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #endregion
    }
}