using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class FaixaEtariaTests
    {
        private readonly DbPackAndPromote _context;
        private readonly FaixaEtariaController _controller;

        public FaixaEtariaTests()
        {
            // Usa o Banco de Dados em Memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options);
            _controller = new FaixaEtariaController(_context);
        }

        [Fact]
        public void RetornaOkResultQuandoFaixaEtariaForAlterada()
        {
            // Arrange
            var faixaEtaria = new FaixaEtaria { DescricaoFaixaEtaria = "0-12 anos" };
            _context.FaixaEtaria.Add(faixaEtaria);
            _context.SaveChanges();

            var faixaEtariaDto = new FaixaEtariaDto { DescricaoFaixaEtaria = "0-10 anos" };

            // Act
            var result = _controller.AlterarFaixaEtaria(faixaEtaria.IdFaixaEtaria, faixaEtariaDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedFaixaEtaria = Assert.IsType<FaixaEtaria>(okResult.Value);
            Assert.Equal(faixaEtariaDto.DescricaoFaixaEtaria, updatedFaixaEtaria.DescricaoFaixaEtaria);
        }

        [Fact]
        public void RetornaOkResultQuandoFaixaEtariaForExcluida()
        {
            // Arrange
            var faixaEtaria = new FaixaEtaria { DescricaoFaixaEtaria = "13-17 anos" };
            _context.FaixaEtaria.Add(faixaEtaria);
            _context.SaveChanges();

            // Act
            var result = _controller.ExcluirFaixaEtaria(faixaEtaria.IdFaixaEtaria);

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.Null(_context.FaixaEtaria.Find(faixaEtaria.IdFaixaEtaria));
        }

        [Fact]
        public void RetornaBadRequestResultQuandoDescricaoForVaziaAoCriarFaixaEtaria()
        {
            // Arrange
            var faixaEtariaDto = new FaixaEtariaDto { DescricaoFaixaEtaria = "" };

            // Act
            var result = _controller.CriarFaixaEtaria(faixaEtariaDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
