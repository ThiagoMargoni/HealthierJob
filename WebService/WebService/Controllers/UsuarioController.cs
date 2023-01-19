using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace WebService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		MySqlConnection con = new MySqlConnection("server=localhost;Database=healthier;user=root;password=thiago123");

		[Route("{codigo}/{senha}")]
		[HttpGet]
		public Usuario Login(string codigo, string senha)
		{
			try
			{
				con.Open();

				MySqlCommand qry = new MySqlCommand("SELECT *, DATE_FORMAT(entrada,'%d/%m/%Y') AS entradaA, DATE_FORMAT(saida,'%d/%m/%Y') AS saidaA FROM Funcionario WHERE codigoFunc = @c AND senha = @s", con);
				qry.Parameters.AddWithValue("@c", codigo);
				qry.Parameters.AddWithValue("@s", senha);

				MySqlDataReader leitor = qry.ExecuteReader();

				if (leitor.Read())
				{
					return new Usuario(leitor["tipo"].ToString(), leitor["status"].ToString(), leitor["codigoFunc"].ToString(), leitor["nomeCompleto"].ToString(),
						(int)leitor["idade"], leitor["areaAtua"].ToString(), DateTime.ParseExact(leitor["entradaA"].ToString(), "dd/MM/yyyy", null),
						DateTime.ParseExact(leitor["saidaA"].ToString(), "dd/MM/yyyy", null), leitor["senha"].ToString(), (byte[])leitor["img"]);
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

		[Route("{codigo}")]
		[HttpGet]
		public Usuario RetornarUsu(string codigo)
		{
			try
			{
				con.Open();

				MySqlCommand qry = new MySqlCommand("SELECT *, DATE_FORMAT(entrada,'%d/%m/%Y') AS entradaA, DATE_FORMAT(saida,'%d/%m/%Y') AS saidaA FROM Funcionario WHERE codigoFunc = @codigo", con);
				qry.Parameters.AddWithValue("@codigo", codigo);

				MySqlDataReader leitor = qry.ExecuteReader();

				if (leitor.Read())
				{
					return new Usuario(leitor["tipo"].ToString(), leitor["status"].ToString(), leitor["codigoFunc"].ToString(), leitor["nomeCompleto"].ToString(),
						(int)leitor["idade"], leitor["areaAtua"].ToString(), DateTime.ParseExact(leitor["entradaA"].ToString(), "dd/MM/yyyy", null),
						DateTime.ParseExact(leitor["saidaA"].ToString(), "dd/MM/yyyy", null), leitor["senha"].ToString(), (byte[])leitor["img"]);
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
