using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly DbPackAndPromote _dbPackAndPromote;

        public LoginController (DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }
    }
}
