using Microsoft.AspNetCore.Mvc;

namespace LPADS2024T2.Controllers
{
    public class CursosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
