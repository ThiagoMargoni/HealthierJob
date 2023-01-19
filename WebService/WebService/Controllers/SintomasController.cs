using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SintomasController : ControllerBase
	{

		MySqlConnection con = new MySqlConnection("server=localhost;Database=healthier;user=root;password=thiago123");

		[HttpGet]
		public List<Sintomas> Listar()
		{
			List<Sintomas> lista = new List<Sintomas>();

			try
			{
				con.Open();

				MySqlCommand qry = new MySqlCommand("SELECT * FROM Sintomas", con);

				MySqlDataReader leitor = qry.ExecuteReader();

				while (leitor.Read())
				{
					Sintomas s = new Sintomas((int)leitor["codigoSint"], leitor["nomeSint"].ToString(), (int)leitor["qtdRelatos"]);

					lista.Add(s);
				}

				return lista;

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

		[Route("{lista}/{codigo}/{data}")]
		[HttpPost]
		public void Relato(List<string> lista, string codigo, DateTime data)
		{

			List<int> codigoS = VoltaCodigo(lista);

			try
			{
				con.Open();

				for (int i = 0; i < lista.Count; i++)
				{
					MySqlCommand qry = new MySqlCommand("INSERT INTO Relata VALUES(@d, @cf, @cs)", con);

					MySqlCommand addRelato = new MySqlCommand("UPDATE Sintomas SET qtdRelatos = qtdRelatos + 1 WHERE codigoSint = @c", con);

					qry.Parameters.AddWithValue("@d", DateTime.Now);
					qry.Parameters.AddWithValue("@cf", codigo);
					qry.Parameters.AddWithValue("@cs", codigoS[i]);

					qry.ExecuteNonQuery();

					addRelato.Parameters.AddWithValue("@c", codigoS[i]);

					addRelato.ExecuteNonQuery();
				}

			}
			catch (Exception e)
			{

			}
			finally
			{
				con.Close();
			}
		}

        public List<int> VoltaCodigo(List<string> lista)
        {
            List<int> sintomas = new List<int>();

            if (!(lista.Contains("Nada")))
            {
                string[] texto = lista[0].Split(";");

                try
                {
                    con.Open();

                    for (int i = 0; i < texto.Length; i++)
                    {
                        MySqlCommand qry = new MySqlCommand("SELECT * FROM Sintomas WHERE nomeSint = @n", con);

                        qry.Parameters.AddWithValue("@n", texto[i]);

                        MySqlDataReader leitor = qry.ExecuteReader();

                        if (leitor.Read())
                        {
                            sintomas.Add(int.Parse(leitor["codigoSint"].ToString()));
                        }
                        else
                        {
                            sintomas.Add(-1);
                        }

                        leitor.Close();
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
            else
            {
                sintomas.Add(0);
            }

            return sintomas;
        }
    }
}
