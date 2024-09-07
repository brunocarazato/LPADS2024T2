using LPADS2024T2.Data;
using LPADS2024T2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LPADS2024T2.Controllers
{
    public class AlunosController : Controller
    {

        private readonly ConnectionContext _context;

        public AlunosController(ConnectionContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Alunos.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var aluno = await _context.Alunos.FirstOrDefaultAsync(aluno => aluno.Id == id);
            if(aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

    }
}
