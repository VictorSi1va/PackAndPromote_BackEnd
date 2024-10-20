using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class PreferenciaAlvoTests
    {
        private readonly DbPackAndPromote _context;
        private readonly PreferenciaAlvoController _controller;

        public PreferenciaAlvoTests()
        {
            // Usa o Banco de Dados em Memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options);
            _controller = new PreferenciaAlvoController(_context);
        }

        [Fact]
        public void RetornaOkResultAoListarPreferenciasAlvo()
        {
            // Arrange
            _context.PreferenciaAlvo.Add(new PreferenciaAlvo { DescricaoPreferenciaAlvo = "Preferência 1" });
            _context.PreferenciaAlvo.Add(new PreferenciaAlvo { DescricaoPreferenciaAlvo = "Preferência 2" });
            _context.SaveChanges();

            // Act
            var result = _controller.ListarPreferenciasAlvo();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var preferenciasAlvo = Assert.IsAssignableFrom<IEnumerable<PreferenciaAlvo>>(okResult.Value);
            Assert.Equal(2, preferenciasAlvo.Count());
        }

        [Fact]
        public void RetornaCreatedResultQuandoPreferenciaAlvoForCriada()
        {
            // Arrange
            var preferenciaAlvoDto = new PreferenciaAlvoDto { DescricaoPreferenciaAlvo = "Nova Preferência" };

            // Act
            var result = _controller.CriarPreferenciaAlvo(preferenciaAlvoDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var preferenciaAlvo = Assert.IsType<PreferenciaAlvo>(createdResult.Value);
            Assert.Equal(preferenciaAlvoDto.DescricaoPreferenciaAlvo, preferenciaAlvo.DescricaoPreferenciaAlvo);
        }

        [Fact]
        public void RetornaNotFoundResultQuandoPreferenciaAlvoNaoExistirAoAlterar()
        {
            // Arrange
            var preferenciaAlvoDto = new PreferenciaAlvoDto { DescricaoPreferenciaAlvo = "Alteração" };
            int invalidId = 999; // ID que não existe

            // Act
            var result = _controller.AlterarPreferenciaAlvo(invalidId, preferenciaAlvoDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
