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
        #region Variáveis e Construtor

        private readonly DbPackAndPromote _context;
        private readonly PreferenciaAlvoController _controller;

        // Construtor para configurar o contexto do banco de dados em memória e a controller para testes
        public PreferenciaAlvoTests()
        {
            // Configura o banco de dados em memória, usado para realizar testes sem persistência real
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options); // Inicializa o contexto com a configuração em memória
            _controller = new PreferenciaAlvoController(_context); // Inicializa a controller com o contexto
        }

        #endregion

        #region Preferência Alvo - Testes

        #region RetornaOkResultAoListarPreferenciasAlvo
        [Fact]
        public void RetornaOkResultAoListarPreferenciasAlvo()
        {
            // Arrange: Adiciona dados de teste ao contexto
            _context.PreferenciaAlvo.Add(new PreferenciaAlvo { DescricaoPreferenciaAlvo = "Preferência 1" });
            _context.PreferenciaAlvo.Add(new PreferenciaAlvo { DescricaoPreferenciaAlvo = "Preferência 2" });
            _context.SaveChanges();

            // Act: Executa o método ListarPreferenciasAlvo na controller
            var result = _controller.ListarPreferenciasAlvo();

            // Assert: Verifica se o resultado é do tipo OkObjectResult e se contém a lista de preferências
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var preferenciasAlvo = Assert.IsAssignableFrom<IEnumerable<PreferenciaAlvo>>(okResult.Value);
            Assert.Equal(2, preferenciasAlvo.Count()); // Verifica se a lista contém 2 itens
        }
        #endregion

        #region RetornaCreatedResultQuandoPreferenciaAlvoForCriada
        [Fact]
        public void RetornaCreatedResultQuandoPreferenciaAlvoForCriada()
        {
            // Arrange: Configura o DTO com os dados da nova preferência
            var preferenciaAlvoDto = new PreferenciaAlvoDto { DescricaoPreferenciaAlvo = "Nova Preferência" };

            // Act: Executa o método CriarPreferenciaAlvo na controller
            var result = _controller.CriarPreferenciaAlvo(preferenciaAlvoDto);

            // Assert: Verifica se o resultado é do tipo CreatedAtActionResult e se os dados da preferência estão corretos
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var preferenciaAlvo = Assert.IsType<PreferenciaAlvo>(createdResult.Value);
            Assert.Equal(preferenciaAlvoDto.DescricaoPreferenciaAlvo, preferenciaAlvo.DescricaoPreferenciaAlvo);
        }
        #endregion

        #region RetornaNotFoundResultQuandoPreferenciaAlvoNaoExistirAoAlterar
        [Fact]
        public void RetornaNotFoundResultQuandoPreferenciaAlvoNaoExistirAoAlterar()
        {
            // Arrange: Configura o DTO com os dados para alteração e um ID inexistente
            var preferenciaAlvoDto = new PreferenciaAlvoDto { DescricaoPreferenciaAlvo = "Alteração" };
            int invalidId = 999; // ID inválido

            // Act: Executa o método AlterarPreferenciaAlvo com o ID inválido
            var result = _controller.AlterarPreferenciaAlvo(invalidId, preferenciaAlvoDto);

            // Assert: Verifica se o resultado é do tipo NotFoundResult
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #endregion
    }
}