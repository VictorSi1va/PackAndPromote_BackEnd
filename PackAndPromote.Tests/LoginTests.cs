using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class LoginTests
    {
        private readonly DbPackAndPromote _context;
        private readonly LoginController _controller;

        public LoginTests()
        {
            // Usa o Banco de Dados em Memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options);
            _controller = new LoginController(_context);
        }

        [Fact]
        public void RetornaOkResultAoListarPerfis()
        {
            // Arrange
            _context.Perfil.Add(new Perfil { NomePerfil = "Admin", DescricaoPerfil = "Administrador do sistema" });
            _context.Perfil.Add(new Perfil { NomePerfil = "Cliente", DescricaoPerfil = "Cliente comum" });
            _context.SaveChanges();

            // Act
            var result = _controller.ListarPerfis();

            // Assert
            var okResult = Assert.IsType<List<Perfil>>(result.Value);
            Assert.Equal(2, okResult.Count);
        }

        [Fact]
        public void RetornaNotFoundResultQuandoPerfilNaoExistirAoPesquisar()
        {
            // Act
            var result = _controller.PesquisarPerfil(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void RetornaCreatedResultQuandoUsuarioForCriado()
        {
            // Arrange
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
                IdPreferenciaAlvo = 1
            };

            // Act
            var result = _controller.CriarUsuario(novoUsuarioDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var usuario = Assert.IsType<Usuario>(createdResult.Value);
            Assert.Equal("usuario1", usuario.Login);
        }
    }
}