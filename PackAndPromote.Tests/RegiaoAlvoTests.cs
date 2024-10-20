using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class RegiaoAlvoTests
    {
        private readonly DbPackAndPromote _context;
        private readonly RegiaoAlvoController _controller;

        public RegiaoAlvoTests()
        {
            // Usa o Banco de Dados em Memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options);
            _controller = new RegiaoAlvoController(_context);
        }

        [Fact]
        public void RetornaOkResultAoListarRegioes()
        {
            // Arrange
            _context.RegiaoAlvo.Add(new RegiaoAlvo { NomeRegiaoAlvo = "Região 1" });
            _context.RegiaoAlvo.Add(new RegiaoAlvo { NomeRegiaoAlvo = "Região 2" });
            _context.SaveChanges();

            // Act
            var result = _controller.ListarRegioesAlvo();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var regioes = Assert.IsType<List<RegiaoAlvo>>(okResult.Value);
            Assert.Equal(2, regioes.Count);
        }

        [Fact]
        public void RetornaNotFoundResultQuandoNaoExistirRegiaoAlvoAoPesquisar()
        {
            // Act
            var result = _controller.PesquisarRegiaoAlvo(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void RetornaCreatedResultAoCriarRegiaoAlvo()
        {
            // Arrange
            var regiaoAlvoDto = new RegiaoAlvoDto { NomeRegiaoAlvo = "Nova Região" };

            // Act
            var result = _controller.CriarRegiaoAlvo(regiaoAlvoDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var regiaoAlvo = Assert.IsType<RegiaoAlvo>(createdResult.Value);
            Assert.Equal("Nova Região", regiaoAlvo.NomeRegiaoAlvo);
        }
    }
}
