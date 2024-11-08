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
        #region Variáveis e Construtor

        // Contexto do banco de dados em memória usado para testes
        private readonly DbPackAndPromote _context;
        // Controller de Vendas a ser testado
        private readonly VendasController _controller;

        public VendasTests()
        {
            // Configura o uso de um banco de dados em memória para os testes
            var options = new DbContextOptionsBuilder<DbPackAndPromote>()
                    .UseInMemoryDatabase(databaseName: "DatabaseTest")
                    .Options;

            // Inicializa o contexto com as opções definidas
            _context = new DbPackAndPromote(options);
            // Inicializa a controller passando o contexto
            _controller = new VendasController(_context);

            // Limpa lojas existentes no contexto para garantir um ambiente de teste limpo
            var lojasExistentes = _context.Loja.ToList();
            _context.Loja.RemoveRange(lojasExistentes);
            // Adiciona duas lojas para testes
            _context.Loja.AddRange(new List<Loja>
            {
                new Loja { IdLoja = 22, NomeLoja = "Loja A", EnderecoLoja = "End A", DescricaoLoja = "Desc A",
                    TelefoneLoja = "123456789", CNPJLoja = "12.345.678/0001-90", EmailLoja = "lojaA@example.com",
                    DataCriacao = DateTime.Now },
                new Loja { IdLoja = 23, NomeLoja = "Loja B", EnderecoLoja = "End B", DescricaoLoja = "Desc B",
                    TelefoneLoja = "987654321", CNPJLoja = "98.765.432/0001-90", EmailLoja = "lojaB@example.com",
                    DataCriacao = DateTime.Now }
            });
            // Salva as alterações no contexto
            _context.SaveChanges();
        }

        #endregion

        #region Vendas - Testes

        #region RetornaOkResultAoListarLojas
        // Testa se o método ListarLojas retorna um resultado Ok quando há lojas
        [Fact]
        public void RetornaOkResultAoListarLojas()
        {
            // Arrange: Configura o estado inicial, assegurando que as lojas estão no contexto
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

            // Act: Chama o método ListarLojas da controller
            var result = _controller.ListarLojas();

            // Assert: Verifica se o resultado é do tipo OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            // Verifica se o valor retornado é uma lista de LojaDto
            var lojas = Assert.IsType<List<LojaDto>>(okResult.Value);
            // Verifica se a quantidade de lojas retornadas está correta
            Assert.Equal(2, lojas.Count);
        }
        #endregion

        #region RetornaOkResultAoEncontrarLojaExistesteAoPesquisar
        // Testa se o método PesquisarLoja retorna um resultado Ok para uma loja existente
        [Fact]
        public void RetornaOkResultAoEncontrarLojaExistesteAoPesquisar()
        {
            // Act: Chama o método PesquisarLoja com o ID da loja existente
            var result = _controller.PesquisarLoja(22);

            // Assert: Verifica se o resultado é do tipo OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            // Verifica se o valor retornado é um LojaDto
            var loja = Assert.IsType<LojaDto>(okResult.Value);
            // Verifica se os dados da loja estão corretos
            Assert.Equal(22, loja.IdLoja);
            Assert.Equal("Loja A", loja.NomeLoja);
        }
        #endregion

        #region RetornaOkResultAoAlterarLojaExistente
        // Testa se o método AlterarLoja atualiza corretamente uma loja existente
        [Fact]
        public void RetornaOkResultAoAlterarLojaExistente()
        {
            // Arrange: Prepara um DTO de loja com os novos dados
            var lojaAlteradaDto = new LojaAlteradaDto
            {
                NomeLoja = "Loja A Alterada",
                EnderecoLoja = "Novo Endereco A",
                DescricaoLoja = "Nova Desc A",
                TelefoneLoja = "123456789",
                CNPJLoja = "12.345.678/0001-90",
                EmailLoja = "lojaA@alterado.com"
            };

            // Act: Chama o método AlterarLoja com o ID da loja e o DTO alterado
            var result = _controller.AlterarLoja(22, lojaAlteradaDto);

            // Assert: Verifica se o resultado é do tipo OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Verifica se a mensagem de sucesso está correta
            Assert.Equal("Loja alterada com sucesso!", okResult.Value);

            // Verifica se os dados da loja foram atualizados no contexto
            var lojaAtualizada = _context.Loja.Find(22);
            Assert.Equal("Loja A Alterada", lojaAtualizada.NomeLoja);
        }
        #endregion

        #endregion
    }
}