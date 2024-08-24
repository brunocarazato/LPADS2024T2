using LPADS2024T2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LPADS2024T2.Controllers
{
    public class AlunosController : Controller
    {
 
        private static List<Aluno> alunos = new List<Aluno>
        {
            new Aluno
            {
                Id = 1,
                Nome = "João Silva",
                Email = "joao@gmail.com"
            },
            new Aluno
            {
                Id = 2,
                Nome = "Maria Oliveira",
                Email = "maria@gmail.com"
            },
        };
        public IActionResult Index()
        {
            return View(alunos);
        }

        public IActionResult Details(int id)
        {
            var aluno = alunos.Find(aluno => aluno.Id == id);
            if(aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

    }
}
