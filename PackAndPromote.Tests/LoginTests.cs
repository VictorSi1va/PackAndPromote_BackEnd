using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class LoginTests
    {
        #region Variáveis e Construtor

        // Campos privados para o contexto de banco de dados em memória, o controller de login e as configurações da aplicação
        private readonly DbPackAndPromote _context;
        private readonly LoginController _controller;
        private readonly IConfiguration _configuration;

        public LoginTests()
        {
            // Configuração de teste para simular o ambiente de configuração
            var configurationBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables(); // Carrega variáveis de ambiente
            _configuration = configurationBuilder.Build(); // Constrói o objeto de configuração

            // Obtém a chave secreta JWT das configurações
            var secretKeyJWT = _configuration["Jwt_SecretKey"];
            if (string.IsNullOrEmpty(secretKeyJWT))
            {
                throw new InvalidOperationException("A chave secreta JWT não está definida.");
            }

            // Configuração do banco de dados em memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;

            _context = new DbPackAndPromote(options); // Inicializa o contexto com as opções configuradas
            _controller = new LoginController(_configuration, _context); // Inicializa a controller de login com as configurações e contexto
        }

        #endregion

        #region Login - Testes

        #region RetornaOkResultAoListarPerfis
        [Fact]
        public void RetornaOkResultAoListarPerfis()
        {
            // Arrange - Adiciona perfis ao banco em memória para teste
            _context.Perfil.Add(new Perfil { NomePerfil = "Admin", DescricaoPerfil = "Administrador do sistema" });
            _context.Perfil.Add(new Perfil { NomePerfil = "Cliente", DescricaoPerfil = "Cliente comum" });
            _context.SaveChanges();

            // Act - Chama o método ListarPerfis da controller
            var result = _controller.ListarPerfis();

            // Assert - Verifica se o resultado é uma lista de perfis com 2 itens
            var okResult = Assert.IsType<List<Perfil>>(result.Value);
            Assert.Equal(2, okResult.Count);
        }
        #endregion

        #region RetornaNotFoundResultQuandoPerfilNaoExistirAoPesquisar
        [Fact]
        public void RetornaNotFoundResultQuandoPerfilNaoExistirAoPesquisar()
        {
            // Act - Tenta buscar um perfil inexistente (ID 99)
            var result = _controller.PesquisarPerfil(99);

            // Assert - Verifica se o resultado é NotFound
            Assert.IsType<NotFoundResult>(result.Result);
        }
        #endregion

        #region RetornaCreatedResultQuandoUsuarioForCriado
        [Fact]
        public void RetornaCreatedResultQuandoUsuarioForCriado()
        {
            // Arrange - Define um novo usuário a ser criado
            var novoUsuarioDto = new UsuarioDto
            {
                Login = "usuario1",
                Senha = "senha123",
                NomeLoja = "Loja 1",
                EnderecoLoja = "Rua A, 123",
                DescricaoLoja = "Descrição da loja",
                TelefoneLoja = "123456789",
                CNPJLoja = "00.000.000/0001-00",
                EmailLoja = "email@loja.com",
                IdCategoria = 1,
                IdPublicoAlvo = 1,
                IdFaixaEtaria = 1,
                IdRegiaoAlvo = 1,
                IdPreferenciaAlvo = 1,
                IdPlano = 1,
            };

            // Act - Chama o método CriarUsuario da controller
            var result = _controller.CriarUsuario(novoUsuarioDto);

            // Assert - Verifica se o resultado é CreatedAtAction e se o usuário foi criado com o login esperado
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var usuario = Assert.IsType<Usuario>(createdResult.Value);
            Assert.Equal("usuario1", usuario.Login);
        }
        #endregion

        #endregion
    }
}