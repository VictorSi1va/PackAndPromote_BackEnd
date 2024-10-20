using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class CategoriaTests
    {
        private readonly DbPackAndPromote _context;
        private readonly CategoriaController _controller;

        public CategoriaTests()
        {
            // Usa o Banco de Dados em Memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options);
            _controller = new CategoriaController(_context);
        }

        [Fact]
        public void RetornaOkResultAoListarCategorias()
        {
            // Arrange
            _context.Categoria.Add(new Categoria { NomeCategoria = "Categoria 1" });
            _context.Categoria.Add(new Categoria { NomeCategoria = "Categoria 2" });
            _context.SaveChanges();

            // Act
            var result = _controller.ListarCategorias();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var categorias = Assert.IsAssignableFrom<IEnumerable<Categoria>>(okResult.Value);
            Assert.Equal(2, categorias.Count());
        }

        [Fact]
        public void RetornaOkResultQuandoCategoriaExistirAoPesquisar()
        {
            // Arrange
            var categoria = new Categoria { NomeCategoria = "Categoria 1" };
            _context.Categoria.Add(categoria);
            _context.SaveChanges();

            // Act
            var result = _controller.PesquisarCategoria(categoria.IdCategoria);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategoria = Assert.IsType<Categoria>(okResult.Value);
            Assert.Equal(categoria.NomeCategoria, returnedCategoria.NomeCategoria);
        }

        [Fact]
        public void RetornaCreatedResultQuandoCategoriaForCriada()
        {
            // Arrange
            var categoriaDto = new CategoriaDto { NomeCategoria = "Nova Categoria" };

            // Act
            var result = _controller.CriarCategoria(categoriaDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var categoria = Assert.IsType<Categoria>(createdResult.Value);
            Assert.Equal(categoriaDto.NomeCategoria, categoria.NomeCategoria);
        }
    }
}
