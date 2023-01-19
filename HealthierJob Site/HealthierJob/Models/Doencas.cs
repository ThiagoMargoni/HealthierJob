using MySql.Data.MySqlClient;

namespace HealthierJob
{
    public class Doencas
    {
        private List<int> sintomas = new List<int>();
        private int codDoenca;
        private string nome;
        private int qtdCasos;
        private string recomendacao;
        private int nvlPerigo;

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

        public List<int> Sintomas { get => sintomas; set => sintomas = value; }
        public int CodDoenca { get => codDoenca; set => codDoenca = value; }
        public string Nome { get => nome; set => nome = value; }
        public int QtdCasos { get => qtdCasos; set => qtdCasos = value; }
        public string Recomendacao { get => recomendacao; set => recomendacao = value; }
        public int NvlPerigo { get => nvlPerigo; set => nvlPerigo = value; }

        public string Cadastrar()
        {
            try
            {
                con.Open();

                MySqlCommand getId = new MySqlCommand("SHOW TABLE STATUS LIKE 'doenca'", con);

                MySqlDataReader leitor = getId.ExecuteReader();

                int id = 0;

                if (leitor.Read())
                {
                    id = int.Parse(leitor["Auto_increment"].ToString());

                    id += 2;
                }

                leitor.Close();

                MySqlCommand qry = new MySqlCommand("INSERT INTO Doenca(nome, qtdCasos, recomendacao, nvlPerigo) VALUES(@n, @q, @r, @nvl)", con);

                qry.Parameters.AddWithValue("@n", nome);
                qry.Parameters.AddWithValue("@q", qtdCasos);
                qry.Parameters.AddWithValue("@r", recomendacao);
                qry.Parameters.AddWithValue("@nvl", nvlPerigo);

                qry.ExecuteNonQuery();

                for (int i = 0; i < sintomas.Count; i++)
                {
                    MySqlCommand qry2 = new MySqlCommand("INSERT INTO Relaciona VALUES(@cd, @cs)", con);

                    qry2.Parameters.AddWithValue("@cd", id);
                    qry2.Parameters.AddWithValue("@cs", sintomas[i]);

                    qry2.ExecuteNonQuery();
                }

                return "Doença Cadastrada com Sucesso";

            }
            catch (MySqlException e)
            {

                switch (e.Number)
                {
                    case 1406: return "Texto Muito Grande no Campo"; break;

                    case 1062: return "Código Já Cadastrado"; break;

                    case 1048: return "Favor Preencher Todos os Campos Obrigatórios"; break;

                    default: return "Tente Novamente ou Espere até mais Tarde";
                }

            }
            catch (Exception e)
            {

                return "Erro: " + e;

            }
            finally
            {
                con.Close();
            }
        }

        public static List<Doencas> Listar()
        {

            List<Doencas> doencas = new List<Doencas>();
            List<int> listaCodigos = new List<int>();

            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM Doenca", con);

                MySqlDataReader leitor = qry.ExecuteReader();

                while (leitor.Read())
                {
                    listaCodigos.Add((int)leitor["codDoenca"]);
                }

                leitor.Close();

                for (int i = 0; i < listaCodigos.Count; i++)
                {
                    List<int> listaSintomas = new List<int>();

                    MySqlCommand sintoma = new MySqlCommand("SELECT * FROM Relaciona WHERE fk_codDoenca = @cd", con);

                    sintoma.Parameters.AddWithValue("@cd", listaCodigos[i]);

                    MySqlDataReader leitor1 = sintoma.ExecuteReader();

                    while (leitor1.Read())
                    {
                        listaSintomas.Add((int)leitor1["fk_codSint"]);
                    }

                    leitor1.Close();

                    MySqlCommand doenca = new MySqlCommand("SELECT * FROM Doenca WHERE codDoenca = @cd", con);

                    doenca.Parameters.AddWithValue("@cd", listaCodigos[i]);

                    MySqlDataReader leitor2 = doenca.ExecuteReader();

                    if (leitor2.Read())
                    {
                        doencas.Add(new Doencas(listaSintomas, (int)leitor2["codDoenca"], leitor2["nome"].ToString(), (int)leitor2["qtdCasos"],
                            leitor2["recomendacao"].ToString(), (int)leitor2["nvlPerigo"]));
                    }

                    leitor2.Close();
                }

                return doencas;

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

        public Doencas RetornarDoenca()
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
                } else
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

        public string Editar()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("UPDATE Doenca SET nome = @n, qtdCasos = @q, recomendacao = @r, " +
                    "nvlPerigo = @nvl WHERE codDoenca = @cd", con);

                qry.Parameters.AddWithValue("@cd", codDoenca);
                qry.Parameters.AddWithValue("@n", nome);
                qry.Parameters.AddWithValue("@q", qtdCasos);
                qry.Parameters.AddWithValue("@r", recomendacao);
                qry.Parameters.AddWithValue("@nvl", nvlPerigo);

                qry.ExecuteNonQuery();

                MySqlCommand deletar = new MySqlCommand("DELETE FROM Relaciona WHERE fk_codDoenca = @fcd", con);

                deletar.Parameters.AddWithValue("@fcd", codDoenca);

                deletar.ExecuteNonQuery();

                for (int i = 0; i < sintomas.Count; i++)
                {
                    MySqlCommand adicionar = new MySqlCommand("INSERT INTO Relaciona VALUES(@cd, @cs)", con);

                    adicionar.Parameters.AddWithValue("@cd", codDoenca);
                    adicionar.Parameters.AddWithValue("@cs", sintomas[i]);

                    adicionar.ExecuteNonQuery();
                }

                return "Doença Editada com Sucesso";

            }
            catch (MySqlException e)
            {

                switch (e.Number)
                {
                    case 1406: return "Texto Muito Grande no Campo"; break;

                    case 1062: return "Código Já Cadastrado"; break;

                    case 1048: return "Favor Preencher Todos os Campos Obrigatórios"; break;

                    default: return "Tente Novamente ou Espere até mais Tarde";
                }

            }
            catch (Exception e)
            {

                return "Erro: " + e;

            }
            finally
            {
                con.Close();
            }
        }

        public Doencas PesquisaDoenca()
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
                    } else
                    {
                        comando += " AND " + sintomas[i];
                    }
                }

				MySqlCommand sintoma = new MySqlCommand(comando, con);

				MySqlDataReader leitor1 = sintoma.ExecuteReader();

                if(leitor1.Read())
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
