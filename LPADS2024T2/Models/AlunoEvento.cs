using Microsoft.EntityFrameworkCore;

namespace LPADS2024T2.Models
{
	[PrimaryKey(nameof(AlunoId), nameof(EventoId))]
	public class AlunoEvento
	{
		public int AlunoId { get; set; }
		public Aluno? Aluno { get; set; }

		public int EventoId { get; set; }
		public Evento? Evento { get; set; }
	}

}
