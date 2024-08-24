using LPADS2024T2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LPADS2024T2.Controllers
{
    public class FemaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ADS()
        {
            return View();
        }

        public IActionResult BCC()
        {
            return View();
        }
    }
}
