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
            // Configura o contexto para usar um banco de dados em memória, simulando o banco de dados real para testes.
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;

            _context = new DbPackAndPromote(options); // Instancia o contexto de banco de dados em memória.
            _controller = new CategoriaController(_context); // Instancia a controller de categorias, injetando o contexto.
        }

        [Fact]
        public void RetornaOkResultAoListarCategorias()
        {
            // Arrange: Adiciona duas categorias no banco de dados em memória para simular a existência de dados.
            _context.Categoria.Add(new Categoria { NomeCategoria = "Categoria 1" });
            _context.Categoria.Add(new Categoria { NomeCategoria = "Categoria 2" });
            _context.SaveChanges(); // Salva as alterações no banco de dados em memória.

            // Act: Executa o método de listar categorias da controller.
            var result = _controller.ListarCategorias();

            // Assert: Verifica o tipo do resultado e valida o conteúdo.
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica se o resultado é do tipo OkObjectResult.
            var categorias = Assert.IsAssignableFrom<IEnumerable<Categoria>>(okResult.Value); // Garante que o valor é uma lista de categorias.
            Assert.Equal(2, categorias.Count()); // Verifica se o número de categorias retornadas é 2.
        }

        [Fact]
        public void RetornaOkResultQuandoCategoriaExistirAoPesquisar()
        {
            // Arrange: Cria e salva uma categoria no banco de dados em memória para simular uma pesquisa existente.
            var categoria = new Categoria { NomeCategoria = "Categoria 1" };
            _context.Categoria.Add(categoria);
            _context.SaveChanges(); // Salva a categoria no banco de dados em memória.

            // Act: Executa o método de pesquisa de categoria na controller, passando o ID da categoria criada.
            var result = _controller.PesquisarCategoria(categoria.IdCategoria);

            // Assert: Verifica se o resultado da pesquisa é do tipo OkObjectResult e compara o nome da categoria.
            var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifica o tipo OkObjectResult.
            var returnedCategoria = Assert.IsType<Categoria>(okResult.Value); // Confirma que o valor é do tipo Categoria.
            Assert.Equal(categoria.NomeCategoria, returnedCategoria.NomeCategoria); // Compara o nome da categoria original com a retornada.
        }

        [Fact]
        public void RetornaCreatedResultQuandoCategoriaForCriada()
        {
            // Arrange: Cria um DTO de categoria para simular os dados de uma nova categoria.
            var categoriaDto = new CategoriaDto { NomeCategoria = "Nova Categoria" };

            // Act: Executa o método de criação de categoria na controller.
            var result = _controller.CriarCategoria(categoriaDto);

            // Assert: Verifica se o resultado é do tipo CreatedAtActionResult e valida o conteúdo criado.
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result); // Verifica o tipo CreatedAtActionResult.
            var categoria = Assert.IsType<Categoria>(createdResult.Value); // Confirma que o valor é do tipo Categoria.
            Assert.Equal(categoriaDto.NomeCategoria, categoria.NomeCategoria); // Compara o nome do DTO com o nome da categoria criada.
        }
    }
}