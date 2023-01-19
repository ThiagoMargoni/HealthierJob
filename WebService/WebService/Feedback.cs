namespace WebService
{
	public class Feedback
	{
		public string codigoFunc { get; set; }
		public int codDoenca { get; set; }
		public int id { get; set; }
		public int qtdPassos { get; set; }
		public DateTime data { get; set; }
		public int saude { get; set; }
		public string recomendacao { get; set; }

		public Feedback(string codigoFunc, int codDoenca, int id, int qtdPassos, DateTime data, int saude, string recomendacao)
		{
			this.codigoFunc = codigoFunc;
			this.id = id;
			this.codDoenca = codDoenca;
			this.qtdPassos = qtdPassos;
			this.data = data;
			this.saude = saude;
			this.recomendacao = recomendacao;
		}
	}
}
