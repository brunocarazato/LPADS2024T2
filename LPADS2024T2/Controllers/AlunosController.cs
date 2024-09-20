using LPADS2024T2.Data;
using LPADS2024T2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var aluno = await _context.Alunos
                .Include(c => c.Curso)
                .FirstOrDefaultAsync(aluno => aluno.Id == id);

            if(aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        //GET Alunos/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,DataNascimento,Curso")] Aluno aluno)
        {
            if (aluno.Curso != null)
            {
                aluno.Curso = await
                    _context.Cursos.FindAsync(aluno.Curso.Id);
            }
            if (ModelState.IsValid)
            {
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

    }
}
