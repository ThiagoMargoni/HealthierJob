using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class QuestionarioController : ControllerBase
	{

		MySqlConnection con = new MySqlConnection("server=localhost;Database=healthier;user=root;password=thiago123");

		[HttpGet]
		public List<Questionario> Listar()
		{

			List<Questionario> lista = new List<Questionario>();

			try
			{
				con.Open();

				MySqlCommand qry = new MySqlCommand("SELECT * FROM Quest", con);

				MySqlDataReader leitor = qry.ExecuteReader();

				while (leitor.Read())
				{
					Questionario q = new Questionario((int)leitor["id_perg"], leitor["pergunta"].ToString());

					lista.Add(q);
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
	}
}
