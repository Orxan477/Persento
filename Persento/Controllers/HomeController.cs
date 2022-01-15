using Microsoft.AspNetCore.Mvc;
using Persento.DAL;
using System.Linq;

namespace Persento.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.People.Where(p => p.IsDeleted == false).ToList());
        }
    }
}
