using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FeedbackController : ControllerBase
	{
		MySqlConnection con = new MySqlConnection("server=localhost;Database=healthier;user=root;password=thiago123");

		[HttpPost]
		public IActionResult Adicionar([FromBody] Feedback f)
		{
			try
			{
				con.Open();

				MySqlCommand qry = new MySqlCommand("INSERT INTO Feedback(fk_codigoFunc, fk_codDoenca, qtdPassos, data, saude, recomendacao) VALUES(@cf, @cd, @q, @d, @s, @r)", con);

				qry.Parameters.AddWithValue("@cf", f.codigoFunc);
				qry.Parameters.AddWithValue("@cd", voltaCodDoenca(f.recomendacao));
				qry.Parameters.AddWithValue("@q", f.qtdPassos);
				qry.Parameters.AddWithValue("@d", DateTime.Now);
				qry.Parameters.AddWithValue("@s", f.saude);
				qry.Parameters.AddWithValue("@r", gerarRecomendacao(f));

				if (qry.ExecuteNonQuery() != 0)
				{
					return Ok(new { result = "cadastrado", status = 200 });
				}
				else
				{
					return NoContent();
				}
			}
			catch (Exception e)
			{

				return NoContent();

			}
			finally
			{
				con.Close();
			}
		}

		[Route("{data}/{codigo}")]
		[HttpGet]
		public Feedback Retornar(string data, string codigo)
		{
			try
			{
				con.Open();

				MySqlCommand qry = new MySqlCommand("SELECT *, DATE_FORMAT(data,'%d/%m/%Y') as dataF FROM Feedback WHERE DATE_FORMAT(data,'%d/%m/%Y') = @d AND fk_codigoFunc = @cf", con);

				data = data.Replace("00:00:00", "");
				data = data.Replace(" ", "");

				qry.Parameters.AddWithValue("@d", data);
				qry.Parameters.AddWithValue("@cf", codigo);

				MySqlDataReader leitor = qry.ExecuteReader();

				if (leitor.Read())
				{

					return new Feedback(leitor["fk_codFunc"].ToString(), (int)leitor["fk_codDoenca"], (int)leitor["id_feedback"], (int)leitor["qtdPassos"],
						DateTime.ParseExact(leitor["dataF"].ToString(), "dd/MM/yyyy", null), (int)leitor["saude"], leitor["recomendacao"].ToString());

				}
				else
				{
					return null;
				}

			}
			catch (Exception e)
			{
				return null;

			}
			finally
			{
				con.Close();
			}
		}

		SintomasController s = new SintomasController();
		DoencasController d = new DoencasController();


        public int voltaCodDoenca(string strin)
		{
			int codigo = 0;

			List<string> sintomas = new List<string>();

			sintomas[0] = strin;

			List<int> codSint = s.VoltaCodigo(sintomas);

			Doencas doenca = d.pesquisaDoenca(codSint);

			codigo = doenca.codDoenca;

			return codigo;
		}

		public string gerarRecomendacao(Feedback f)
		{
			string recomendacao = "";

            if (f.recomendacao != null)
            {

				List<string> lista = new List<string>();

				lista[0] = f.recomendacao;

                 Doencas doenca = d.pesquisaDoenca(s.VoltaCodigo(lista));

                recomendacao += "Com base nos Sintomas relatados, recomendamos que fique atento na seguinte doença: " + doenca.nome + ". E com base nela, a sugestão" +
                    " médica é: " + doenca.recomendacao;
            }
            else
            {
                if (f.saude <= 5 && f.saude > 3)
                {
                    recomendacao += "Como você não relatou nenhum sintoma, deduzimos com base nas suas respostas do questionário que, sua saúde está boa.";
                }
                else if (f.saude <= 3 && f.saude > 2)
                {
                    recomendacao += "Como você não relatou nenhum sintoma, recomendamos com base nas suas respostas do questionário que, uma boa noite de sono" +
                        " e praticar esportes são essenciais para melhorar o humor. Se o problema persistir, converse com um médico e com seu chefe.";
                }
                else
                {
                    recomendacao += "Como você não relatou nenhum sintoma, recomendamos com base nas suas respostas do questionário que, fique alerta quanto a sua saúde." +
                        " Busque pedir ajuda, para tentar resolver seus problemas o quanto antes. Recomendamos também uma consulta médica.";
                }
            }

            if (f.qtdPassos < 9000)
            {
                recomendacao += ". Quando analisada sua média de passos diária, percebe-se que você está abaixo da média. Portanto recomendamos que dê uma caminhada e tome um pouco de ar";
            }

            return recomendacao;
		}
	}
}
