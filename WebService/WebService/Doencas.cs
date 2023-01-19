using MySql.Data.MySqlClient;

namespace WebService
{
	public class Doencas
	{
		public List<int> sintomas { get; set; }
		public int codDoenca { get; set; }
		public string nome { get; set; }
		public int qtdCasos { get; set; }
		public string recomendacao { get; set; }
		public int nvlPerigo { get; set; }

		static MySqlConnection con = new MySqlConnection("server=localhost;database=healthier;user id=root;password=thiago123");

		public Doencas(List<int> sintomas, int codDoenca, string nome, int qtdCasos, string recomendacao, int nvlPerigo)
		{
			this.sintomas = sintomas;
			this.codDoenca = codDoenca;
			this.nome = nome;
			this.qtdCasos = qtdCasos;
			this.recomendacao = recomendacao;
			this.nvlPerigo = nvlPerigo;
		}
	}
}
