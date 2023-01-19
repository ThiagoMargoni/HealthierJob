namespace WebService
{
	public class Usuario
	{
		public string tipo { get; set; }
		public string status { get; set; }
		public string codigo { get; set; }
		public string nomeCompleto { get; set; }
		public int idade { get; set; }
		public string areaAtuacao { get; set; }
		public DateTime dataEntrada { get; set; }
		public DateTime dataSaida { get; set; }
		public string senha { get; set; }
		public byte[] img { get; set; }

		public Usuario(string tipo, string status, string codigo, string nomeCompleto,
			int idade, string areaAtuacao, DateTime dataEntrada, DateTime dataSaida, string senha, byte[] img)
		{
			this.tipo = tipo;
			this.status = status;
			this.codigo = codigo;
			this.nomeCompleto = nomeCompleto;
			this.idade = idade;
			this.areaAtuacao = areaAtuacao;
			this.dataEntrada = dataEntrada;
			this.dataSaida = dataSaida;
			this.senha = senha;
			this.img = img;
		}
	}
}
