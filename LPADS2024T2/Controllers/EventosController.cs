using LPADS2024T2.Data;
using LPADS2024T2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace LPADS2024T2.Controllers
{
    public class EventosController : Controller
    {
        private readonly ConnectionContext _context;

        public EventosController(ConnectionContext context)
        {
            _context = context;
        }

        // Método para listar todos os eventos
        public IActionResult Index()
        {
            var eventos = _context.Eventos.ToList();
            return View(eventos);
        }

		// Método GET para exibir o formulário de criação
		public IActionResult Create()
		{
			return View();
		}

		// Método POST para criar um novo evento
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Evento evento)
		{
			if (ModelState.IsValid)
			{
				_context.Eventos.Add(evento);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(evento);
		}

		// Método para exibir detalhes de um evento e listar os alunos inscritos
		public async Task<IActionResult> Details(int id)
		{
			var evento = await _context.Eventos
				.Include(e => e.AlunoEventos)
				.ThenInclude(ae => ae.Aluno)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (evento == null)
			{
				return NotFound();
			}

            // Preencher ViewBag com a lista de alunos disponíveis
            ViewBag.Alunos = new SelectList(_context.Alunos, "Id", "Nome");

            return View(evento);
		}

        [HttpPost]
        public IActionResult InscreverAluno(int eventoId)
        {

            var evento = _context.Eventos
                .Include(e => e.AlunoEventos)
                .FirstOrDefault(e => e.Id == eventoId);

            if (evento == null)
            {
                return NotFound();
            }

            // Verifique se a data atual é anterior à data de início do evento
            if (DateTime.Now >= evento.DataInicio)
            {
                // Adicione um erro ao ModelState se a data atual for igual ou posterior à data de início
                ModelState.AddModelError(string.Empty, "Não é possível remover a inscrição após a data de início do evento.");
                return RedirectToAction("Details", new { id = eventoId }); // Redireciona de volta para a página de detalhes
            }

            var alunoId = int.Parse(Request.Form["AlunoId"]);

            if (alunoId == 0)
            {
                ModelState.AddModelError(string.Empty, "Aluno não selecionado.");
                return RedirectToAction("Details", new { id = eventoId });
            }

            // Verificar se o aluno já está inscrito no evento
            if (evento.AlunoEventos.Any(ae => ae.AlunoId == alunoId))
            {
                ModelState.AddModelError(string.Empty, "Este aluno já está inscrito no evento.");
                return RedirectToAction("Details", new { id = eventoId });
            }

            // Verifica a quantidade máxima de inscrições
            if (evento.AlunoEventos.Count() >= evento.QuantidadeMaximaInscricoes)
            {
                ModelState.AddModelError(string.Empty, "O evento atingiu o número máximo de inscrições.");
                return RedirectToAction("Details", new { id = eventoId });
            }

            // Inscrever o aluno no evento
            var alunoEvento = new AlunoEvento
            {
                AlunoId = alunoId,
                EventoId = evento.Id
            };

            _context.AlunoEventos.Add(alunoEvento);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = evento.Id });
        }

        // GET: Eventos/Delete/2
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }


        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Obtenha o evento com base no ID fornecido
            var evento = _context.Eventos
                                 .Include(e => e.AlunoEventos)
                                 .FirstOrDefault(e => e.Id == id);

            // Verifique se o evento existe
            if (evento == null)
            {
                return NotFound();
            }

            // Verifique se o evento possui inscrições de alunos
            if (evento.AlunoEventos.Any())
            {
                // Adicione um erro ao ModelState se houver alunos inscritos
                ModelState.AddModelError(string.Empty, "Não é possível excluir um evento com alunos inscritos.");
                return RedirectToAction("Details", new { id = id });// Redireciona para a action Detalhes do evento
            }

            // Se não houver alunos inscritos, exclua o evento
            _context.Eventos.Remove(evento);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index)); // Redireciona para a lista de eventos após a exclusão
        }

        // GET Eventos/DeleteInscricao?EventoId=7&AlunoId=12
        public IActionResult DeleteInscricao(int EventoId, int AlunoId)
        {
            // Obtenha o evento com base no ID fornecido
            var inscricao = _context.AlunoEventos
                                 .Include(ae => ae.Evento)
                                 .FirstOrDefault(ae => ae.EventoId == EventoId && ae.AlunoId == AlunoId);

            // Verifique se o evento existe
            if (inscricao == null)
            {
                return NotFound();
            }

            // Verifique se a data atual é anterior à data de início do evento
            if (DateTime.Now >= inscricao.Evento.DataInicio)
            {
                // Adicione um erro ao ModelState se a data atual for igual ou posterior à data de início
                ModelState.AddModelError(string.Empty, "Não é possível remover a inscrição após a data de início do evento.");
                return RedirectToAction("Details", new { id = EventoId }); // Redireciona de volta para a página de detalhes
            }


            _context.AlunoEventos.Remove(inscricao);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = EventoId });// Redireciona para a action Detalhes do evento
        }

    }


}
