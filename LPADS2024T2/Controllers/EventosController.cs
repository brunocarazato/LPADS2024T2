using Microsoft.AspNetCore.Mvc;
using LPADS2024T2.Data;
using LPADS2024T2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LPADS2024T2.Controllers
{
	public class EventosController : Controller
	{
		private readonly ConnectionContext _context;
		public IActionResult Index()
		{
			var eventos = _context.Eventos.ToList();
			return View(eventos);
		}

		public EventosController(ConnectionContext context)
		{
			_context = context;
		}

		public IActionResult Create()
		{
			return View();
		}

		//POST
		[HttpPost]
		public ActionResult Create(Evento evento)
		{
			if (ModelState.IsValid)
			{
				_context.Eventos.Add(evento);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(evento);

		}

		public IActionResult Details(int id)
		{
			var evento = _context.Eventos
				.Include(e => e.AlunoEventos)
				.ThenInclude(ae => ae.Aluno)
				.FirstOrDefault(evento => evento.Id == id);

			if (evento == null)
			{
				return NotFound();
			}

			ViewBag.Alunos = new SelectList(_context.Alunos, "Id", "Nome");
			return View(evento);
		}

		[HttpPost]
		public IActionResult InscreverAluno(int eventoId)
		{
			var evento = _context.Eventos
				.Include(e => e.AlunoEventos)
				.FirstOrDefault(e => e.Id == eventoId);
			
			if(evento == null)
			{
				return NotFound();
			}

			var alunoId = int.Parse(Request.Form["AlunoId"]);

			if (alunoId == 0)
			{
				ModelState.AddModelError(string.Empty, "Aluno não selecionado");
				return RedirectToAction("Details", new { id = eventoId });
			}

			if(evento.AlunoEventos.Any(ae => ae.AlunoId == alunoId))
			{
				ModelState.AddModelError(string.Empty, "Este aluno já esta inscrito no evento");
                return RedirectToAction("Details", new { id = eventoId });
            }

			if(evento.AlunoEventos.Count() >= evento.QuantidadeMaximaInscricoes)
			{
				ModelState.AddModelError(string.Empty, "O evento atingiu o número máximo de inscrições.");

                return RedirectToAction("Details", new { id = eventoId });
            }

			var alunoEvento = new AlunoEvento
			{
				AlunoId = alunoId,
				EventoId = evento.Id
			};

			_context.AlunoEventos.Add(alunoEvento);
			_context.SaveChanges();

            return RedirectToAction("Details", new { id = evento.Id });
        }

		//GET -> Eventos/Delete/2
		public async Task <IActionResult> Delete (int? id)
		{
			if(id == null)
			{
				return NotFound();
			}

			var evento = await _context.Eventos
				.FirstOrDefaultAsync(e => e.Id == id);

			if(evento == null)
			{
                return NotFound();
            }

			return View(evento);
		}

		//POST
		[HttpPost, ActionName("Delete")]
		public IActionResult DeleteConfirmed(int id)
		{
			var evento = _context.Eventos
				.Include(e => e.AlunoEventos)
				.FirstOrDefault(e=>e.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

			if (evento.AlunoEventos.Any())
			{
                ModelState.AddModelError(string.Empty, "Não é possível apagar um evento com Alunos inscritos");

                return RedirectToAction("Details", new { id = evento.Id });
            }

			_context.Remove(evento);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}


		//GET
		public IActionResult DeleteInscricao(int EventoId, int AlunoId)
		{
			var inscricao = _context.AlunoEventos
				.Include(ae => ae.Evento)
				.FirstOrDefault(ae => ae.EventoId == EventoId && ae.AlunoId == AlunoId);

			if(inscricao == null)
			{
				return NotFound();
			}

			if(DateTime.Now >= inscricao.Evento.DataInicio)
			{
				ModelState.AddModelError(string.Empty, "Não é possível apagar a inscrição, pois o Evento já começou");

				return RedirectToAction("Details", new { id = inscricao.Evento.Id });
			}

			_context.AlunoEventos.Remove(inscricao);
			_context.SaveChanges();
			return RedirectToAction("Details", new { id = EventoId });
		}

	}
}
