using System.ComponentModel.DataAnnotations;

namespace LPADS2024T2.Models
{
	public class Evento
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Nome { get; set; } = string.Empty;

		[StringLength(500)]
		public string Descricao { get; set; } = string.Empty;

		[Required]
		[Range(1, 100)]
		public int QuantidadeMaximaInscricoes {  get; set; }

		[Required]
		public DateTime DataInicio { get; set; }

		public ICollection<AlunoEvento> AlunoEventos { get; set; } = new List<AlunoEvento>();
	}
}
