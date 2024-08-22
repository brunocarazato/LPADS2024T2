using LPADS2024T2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LPADS2024T2.Controllers
{
    public class FemaController : Controller
    {
        public IActionResult Index()
        {
            Curso femaCurso1 = new Curso();
            femaCurso1.Descricao = "ADS";
            femaCurso1.Id = 2231;

            Curso femaCurso2 = new Curso();
            femaCurso2.Descricao = "BCC";
            femaCurso2.Id = 5666;

            ViewData["ADS"] = femaCurso1;
            ViewData["BCC"] = femaCurso2;

            return View(femaCurso1);
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
