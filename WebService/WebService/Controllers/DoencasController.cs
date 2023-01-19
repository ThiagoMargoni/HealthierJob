using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoencasController : ControllerBase
	{
		MySqlConnection con = new MySqlConnection("server=localhost;Database=healthier;user=root;password=thiago123");

		[Route("[action]/{codDoenca}")]
		[HttpGet]
		public Doencas RetornarDoenca(int codDoenca)
		{
			try
			{
				con.Open();

				List<int> listaSintomas = new List<int>();

				MySqlCommand sintoma = new MySqlCommand("SELECT * FROM Relaciona WHERE fk_codDoenca = @cd", con);

				sintoma.Parameters.AddWithValue("@cd", codDoenca);

				MySqlDataReader leitor1 = sintoma.ExecuteReader();

				while (leitor1.Read())
				{
					listaSintomas.Add((int)leitor1["fk_codSint"]);
				}

				leitor1.Close();

				MySqlCommand doenca = new MySqlCommand("SELECT * FROM Doenca WHERE codDoenca = @cd", con);

				doenca.Parameters.AddWithValue("@cd", codDoenca);

				MySqlDataReader leitor2 = doenca.ExecuteReader();

				if (leitor2.Read())
				{
					return new Doencas(listaSintomas, (int)leitor2["codDoenca"], leitor2["nome"].ToString(), (int)leitor2["qtdCasos"],
						leitor2["recomendacao"].ToString(), (int)leitor2["nvlPerigo"]);
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

		[Route("{sintomas}")]
		[HttpGet]
		public Doencas PesquisaDoenca(List<int> sintomas)
		{
			try
			{

				int cod = 0;

				con.Open();

				string comando = "SELECT fk_codDoenca AS d FROM Relaciona WHERE fk_codSint = ";

				for (int i = 0; i < sintomas.Count; i++)
				{
					if (i == 0)
					{
						comando += sintomas[i];
					}
					else
					{
						comando += " AND " + sintomas[i];
					}
				}

				MySqlCommand sintoma = new MySqlCommand(comando, con);

				MySqlDataReader leitor1 = sintoma.ExecuteReader();

				if (leitor1.Read())
				{
					cod = (int)leitor1["d"];
				}

				leitor1.Close();

				MySqlCommand doenca = new MySqlCommand("SELECT * FROM Doenca WHERE codDoenca = @cd", con);

				doenca.Parameters.AddWithValue("@cd", cod);

				MySqlDataReader leitor2 = doenca.ExecuteReader();

				if (leitor2.Read())
				{
					return new Doencas(null, (int)leitor2["codDoenca"], leitor2["nome"].ToString(), (int)leitor2["qtdCasos"],
						leitor2["recomendacao"].ToString(), (int)leitor2["nvlPerigo"]);
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

		public Doencas pesquisaDoenca(List<int> sintomas)
		{
            try
            {

                int cod = 0;

                con.Open();

                string comando = "SELECT fk_codDoenca AS d FROM Relaciona WHERE fk_codSint = ";

                for (int i = 0; i < sintomas.Count; i++)
                {
                    if (i == 0)
                    {
                        comando += sintomas[i];
                    }
                    else
                    {
                        comando += " AND " + sintomas[i];
                    }
                }

                MySqlCommand sintoma = new MySqlCommand(comando, con);

                MySqlDataReader leitor1 = sintoma.ExecuteReader();

                if (leitor1.Read())
                {
                    cod = (int)leitor1["d"];
                }

                leitor1.Close();

                MySqlCommand doenca = new MySqlCommand("SELECT * FROM Doenca WHERE codDoenca = @cd", con);

                doenca.Parameters.AddWithValue("@cd", cod);

                MySqlDataReader leitor2 = doenca.ExecuteReader();

                if (leitor2.Read())
                {
                    return new Doencas(null, (int)leitor2["codDoenca"], leitor2["nome"].ToString(), (int)leitor2["qtdCasos"],
                        leitor2["recomendacao"].ToString(), (int)leitor2["nvlPerigo"]);
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
	}
}
