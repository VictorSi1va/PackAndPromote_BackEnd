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
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _context; // Contexto do banco de dados
        private readonly RegiaoAlvoController _controller; // Controller que será testado

        // Construtor para configurar o contexto do banco de dados em memória e a controller para testes
        public RegiaoAlvoTests()
        {
            // Configura o banco de dados em memória para simular operações sem afetar o banco de dados real
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options); // Inicializa o contexto com as opções em memória
            _controller = new RegiaoAlvoController(_context); // Inicializa a controller com o contexto
        }

        #endregion

        #region Região Alvo - Testes

        #region RetornaOkResultAoListarRegioes
        [Fact]
        public void RetornaOkResultAoListarRegioes()
        {
            // Arrange: Adiciona duas regiões alvo ao banco de dados em memória
            _context.RegiaoAlvo.Add(new RegiaoAlvo { NomeRegiaoAlvo = "Região 1" });
            _context.RegiaoAlvo.Add(new RegiaoAlvo { NomeRegiaoAlvo = "Região 2" });
            _context.SaveChanges(); // Salva as alterações no contexto

            // Act: Executa o método ListarRegioesAlvo na controller
            var result = _controller.ListarRegioesAlvo();

            // Assert: Verifica se o resultado é do tipo OkObjectResult e se a lista contém duas regiões
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var regioes = Assert.IsType<List<RegiaoAlvo>>(okResult.Value);
            Assert.Equal(2, regioes.Count); // Verifica se a contagem das regiões é igual a 2
        }
        #endregion

        #region RetornaNotFoundResultQuandoNaoExistirRegiaoAlvoAoPesquisar
        [Fact]
        public void RetornaNotFoundResultQuandoNaoExistirRegiaoAlvoAoPesquisar()
        {
            // Act: Tenta pesquisar uma região alvo que não existe (ID 1)
            var result = _controller.PesquisarRegiaoAlvo(1);

            // Assert: Verifica se o resultado é do tipo NotFoundResult
            Assert.IsType<NotFoundResult>(result.Result);
        }
        #endregion

        #region RetornaCreatedResultAoCriarRegiaoAlvo
        [Fact]
        public void RetornaCreatedResultAoCriarRegiaoAlvo()
        {
            // Arrange: Configura um DTO para criar uma nova região alvo
            var regiaoAlvoDto = new RegiaoAlvoDto { NomeRegiaoAlvo = "Nova Região" };

            // Act: Executa o método CriarRegiaoAlvo na controller
            var result = _controller.CriarRegiaoAlvo(regiaoAlvoDto);

            // Assert: Verifica se o resultado é do tipo CreatedAtActionResult e se o nome da região criada é correto
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var regiaoAlvo = Assert.IsType<RegiaoAlvo>(createdResult.Value);
            Assert.Equal("Nova Região", regiaoAlvo.NomeRegiaoAlvo); // Verifica se o nome da região corresponde ao esperado
        }
        #endregion

        #endregion
    }
}