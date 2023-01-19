namespace WebService
{
	public class Sintomas
	{
		public int codigoSintoma { get; set; }
		public string nomeSintoma { get; set; }
		public int qtdRelatos { get; set; }

		public Sintomas(int codigoSintoma, string nomeSintoma, int qtdRelatos)
		{
			this.codigoSintoma = codigoSintoma;
			this.nomeSintoma = nomeSintoma;
			this.qtdRelatos = qtdRelatos;
		}
	}
}
