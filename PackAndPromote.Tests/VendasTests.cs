using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;
using PackAndPromote.Dtos;
using PackAndPromote.Entities;

namespace PackAndPromote.Tests
{
    public class VendasTests
    {
        private readonly DbPackAndPromote _context;
        private readonly VendasController _controller;

        public VendasTests()
        {
            // Usa o Banco de Dados em Memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            _context = new DbPackAndPromote(options);
            _controller = new VendasController(_context);

            var lojasExistentes = _context.Loja.ToList();
            _context.Loja.RemoveRange(lojasExistentes);
            _context.Loja.AddRange(new List<Loja>
            {
                new Loja { IdLoja = 22, NomeLoja = "Loja A", EnderecoLoja = "End A", DescricaoLoja = "Desc A", 
                    TelefoneLoja = "123456789", CNPJLoja = "12.345.678/0001-90", EmailLoja = "lojaA@example.com", 
                    DataCriacao = DateTime.Now },
                new Loja { IdLoja = 23, NomeLoja = "Loja B", EnderecoLoja = "End B", DescricaoLoja = "Desc B", 
                    TelefoneLoja = "987654321", CNPJLoja = "98.765.432/0001-90", EmailLoja = "lojaB@example.com", 
                    DataCriacao = DateTime.Now }
            });
            _context.SaveChanges();
        }

        [Fact]
        public void RetornaOkResultAoListarLojas()
        {
            // Act
            var result = _controller.ListarLojas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var lojas = Assert.IsType<List<LojaDto>>(okResult.Value);
            Assert.Equal(2, lojas.Count);
        }

        [Fact]
        public void RetornaOkResultAoEncontrarLojaExistesteAoPesquisar()
        {
            // Act
            var result = _controller.PesquisarLoja(22);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var loja = Assert.IsType<LojaDto>(okResult.Value);
            Assert.Equal(22, loja.IdLoja);
            Assert.Equal("Loja A", loja.NomeLoja);
        }

        [Fact]
        public void RetornaOkResultAoAlterarLojaExistente()
        {
            // Arrange
            var lojaAlteradaDto = new LojaAlteradaDto
            {
                NomeLoja = "Loja A Alterada",
                EnderecoLoja = "Novo Endereco A",
                DescricaoLoja = "Nova Desc A",
                TelefoneLoja = "123456789",
                CNPJLoja = "12.345.678/0001-90",
                EmailLoja = "lojaA@alterado.com"
            };

            // Act
            var result = _controller.AlterarLoja(22, lojaAlteradaDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Loja alterada com sucesso!", okResult.Value);

            var lojaAtualizada = _context.Loja.Find(22);
            Assert.Equal("Loja A Alterada", lojaAtualizada.NomeLoja);
        }
    }
}
