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
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _context; // Contexto do banco de dados em memória para testes.
        private readonly FaixaEtariaController _controller; // Controller de FaixaEtaria para ser testada.

        public FaixaEtariaTests()
        {
            // Configura o uso de um banco de dados em memória para simular a base de dados real.
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;

            _context = new DbPackAndPromote(options); // Instancia o contexto de banco de dados em memória.
            _controller = new FaixaEtariaController(_context); // Instancia a controller de FaixaEtaria com o contexto.
        }

        #endregion

        #region Faixa Etária - Testes

        #region RetornaOkResultQuandoFaixaEtariaForAlterada
        [Fact]
        public void RetornaOkResultQuandoFaixaEtariaForAlterada()
        {
            // Arrange: Configura uma FaixaEtaria inicial para o teste de atualização.
            var faixaEtaria = new FaixaEtaria { DescricaoFaixaEtaria = "0-12 anos" };
            _context.FaixaEtaria.Add(faixaEtaria); // Adiciona a faixa etária ao contexto de banco em memória.
            _context.SaveChanges(); // Salva as mudanças para persistir o registro no banco de dados em memória.

            // Cria um DTO com a nova descrição para alterar a FaixaEtaria.
            var faixaEtariaDto = new FaixaEtariaDto { DescricaoFaixaEtaria = "0-10 anos" };

            // Act: Executa o método de AlterarFaixaEtaria no controlador.
            var result = _controller.AlterarFaixaEtaria(faixaEtaria.IdFaixaEtaria, faixaEtariaDto);

            // Assert: Verifica se o resultado é do tipo OkObjectResult e se a descrição foi atualizada.
            var okResult = Assert.IsType<OkObjectResult>(result); // Verifica que o resultado é um OkObjectResult.
            var updatedFaixaEtaria = Assert.IsType<FaixaEtaria>(okResult.Value); // Verifica que o valor retornado é uma FaixaEtaria.
            Assert.Equal(faixaEtariaDto.DescricaoFaixaEtaria, updatedFaixaEtaria.DescricaoFaixaEtaria); // Compara a nova descrição.
        }
        #endregion

        #region RetornaOkResultQuandoFaixaEtariaForExcluida
        [Fact]
        public void RetornaOkResultQuandoFaixaEtariaForExcluida()
        {
            // Arrange: Configura uma FaixaEtaria inicial para o teste de exclusão.
            var faixaEtaria = new FaixaEtaria { DescricaoFaixaEtaria = "13-17 anos" };
            _context.FaixaEtaria.Add(faixaEtaria); // Adiciona a faixa etária ao banco de dados em memória.
            _context.SaveChanges(); // Salva a nova faixa etária.

            // Act: Executa o método ExcluirFaixaEtaria na controller.
            var result = _controller.ExcluirFaixaEtaria(faixaEtaria.IdFaixaEtaria);

            // Assert: Verifica se o resultado é OkResult e se a faixa etária foi removida.
            Assert.IsType<OkResult>(result); // Verifica se o retorno é do tipo OkResult.
            Assert.Null(_context.FaixaEtaria.Find(faixaEtaria.IdFaixaEtaria)); // Confirma que a faixa etária não existe mais no banco.
        }
        #endregion

        #region RetornaBadRequestResultQuandoDescricaoForVaziaAoCriarFaixaEtaria
        [Fact]
        public void RetornaBadRequestResultQuandoDescricaoForVaziaAoCriarFaixaEtaria()
        {
            // Arrange: Cria um DTO com uma descrição vazia para testar a criação de uma FaixaEtaria inválida.
            var faixaEtariaDto = new FaixaEtariaDto { DescricaoFaixaEtaria = "" };

            // Act: Executa o método CriarFaixaEtaria na controller.
            var result = _controller.CriarFaixaEtaria(faixaEtariaDto);

            // Assert: Verifica se o resultado é um BadRequestObjectResult, indicando uma entrada inválida.
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); // Verifica que o retorno é um BadRequest.
        }
        #endregion

        #endregion
    }
}