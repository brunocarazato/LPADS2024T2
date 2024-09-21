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

        // GET Aluno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(c => c.Curso)
                .FirstOrDefaultAsync(aluno => aluno.Id == id);

            if (aluno == null)
            {
                return NotFound();
            }

            ViewData["CursoId"] = new SelectList(
                _context.Cursos, "Id", "Nome", aluno.Curso?.Id
            );

            return View(aluno);
        }

        // POST: Alunos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,DataNascimento,Curso")] Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (aluno.Curso != null)
                    {
                        aluno.Curso = await
                            _context.Cursos.FindAsync(aluno.Curso.Id);
                    }
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        // GET: Alunos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.Id == id);
        }

    }
}
