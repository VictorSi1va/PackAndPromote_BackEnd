using Microsoft.EntityFrameworkCore;
using PackAndPromote.Controllers;
using PackAndPromote.Database;

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
    }
}
