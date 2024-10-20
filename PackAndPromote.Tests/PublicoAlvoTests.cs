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
        private readonly DbPackAndPromote _context;
        private readonly PublicoAlvoController _controller;

        public PublicoAlvoTests()
        {
            // Usa o Banco de Dados em Memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options);
            _controller = new PublicoAlvoController(_context);
        }

        [Fact]
        public void RetornaOkResultComListaVaziaQuandoNaoExistirPublicoAlvoAoListar()
        {
            // Act
            var result = _controller.ListarPublicosAlvo();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var publicosAlvo = Assert.IsAssignableFrom<IEnumerable<PublicoAlvo>>(okResult.Value);
            Assert.Empty(publicosAlvo);
        }

        [Fact]
        public void RetornaBadRequestResultQuandoDescricaoForNulaOuVaziaAoCriarPublicoAlvo()
        {
            // Arrange
            var publicoAlvoDto = new PublicoAlvoDto { DescricaoPublicoAlvo = null };

            // Act
            var result = _controller.CriarPublicoAlvo(publicoAlvoDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void RetornaNotFoundResultQuandoPublicoAlvoNaoExistirAoExcluir()
        {
            // Arrange
            int invalidId = 999; // ID que não existe

            // Act
            var result = _controller.ExcluirPublicoAlvo(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
