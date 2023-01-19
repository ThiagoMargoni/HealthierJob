namespace WebService
{
	public class Questionario
	{
		public int id_perg { get; set; }
		public string pergunta { get; set; }

		public Questionario(int id_perg, string pergunta)
		{
			this.id_perg = id_perg;
			this.pergunta = pergunta;
		}
	}
}
