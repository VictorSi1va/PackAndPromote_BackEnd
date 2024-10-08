using Microsoft.AspNetCore.Mvc;
using PackAndPromote.Database;

namespace PackAndPromote.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VendasController : Controller
    {
        private readonly DbPackAndPromote _dbPackAndPromote;

        public VendasController (DbPackAndPromote _context)
        {
            _dbPackAndPromote = _context;
        }
    }
}
