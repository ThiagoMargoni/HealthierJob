using MySql.Data.MySqlClient;

namespace HealthierJob
{
    public class Sintomas
    {
        private int codigoSintoma;
        private string nomeSintoma;
        private int qtdRelatos;

        static MySqlConnection con = new MySqlConnection("server=localhost;database=healthier;user id=root;password=thiago123");
        public Sintomas(int codigoSintoma, string nomeSintoma, int qtdRelatos)
        {
            this.codigoSintoma = codigoSintoma;
            this.nomeSintoma = nomeSintoma;
            this.qtdRelatos = qtdRelatos;
        }

        public int CodigoSintoma { get => codigoSintoma; set => codigoSintoma = value; }
        public string NomeSintoma { get => nomeSintoma; set => nomeSintoma = value; }
        public int QtdRelatos { get => qtdRelatos; set => qtdRelatos = value; }


        public string Cadastrar()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("INSERT INTO Sintomas(nomeSint, qtdRelatos) VALUES(@nome, @qtd)", con);

                qry.Parameters.AddWithValue("@nome", nomeSintoma);
                qry.Parameters.AddWithValue("@qtd", qtdRelatos);

                qry.ExecuteNonQuery();

                return "Cadastrado com Sucesso";

            } catch (MySqlException e)
            {

                switch (e.Number)
                {
                    case 1406: return "Texto Muito Grande no Campo"; break;

                    case 1062: return "Código Já Cadastrado"; break;

                    case 1048: return "Favor Preencher Todos os Campos Obrigatórios"; break;

                    default: return "Tente Novamente ou Espere até mais Tarde";
                }

            } catch (Exception e)
            {

                return "Erro: " + e;

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

                MySqlCommand qry = new MySqlCommand("UPDATE Sintomas SET nomeSint = @n, qtdRelatos = @q WHERE codigoSint = @c", con);

                qry.Parameters.AddWithValue("@n", nomeSintoma);
                qry.Parameters.AddWithValue("@q", qtdRelatos);
                qry.Parameters.AddWithValue("@c", codigoSintoma);

                qry.ExecuteNonQuery();

                return "Sintoma Editado com Sucesso";

            } catch (MySqlException e)
            {

                switch (e.Number)
                {
                    case 1406: return "Texto Muito Grande no Campo"; break;

                    case 1062: return "Código Já Cadastrado"; break;

                    case 1048: return "Favor Preencher Todos os Campos Obrigatórios"; break;

                    default: return "Tente Novamente ou Espere até mais Tarde";
                }

            } catch (Exception e)
            {

                return "Erro: " + e;

            }
            finally
            {
                con.Close();
            }
        }

        public Sintomas RetornarSint()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM Sintomas WHERE codigoSint = @c", con);
                qry.Parameters.AddWithValue("@c", codigoSintoma);

                MySqlDataReader leitor = qry.ExecuteReader();

                if (leitor.Read())
                {
                    return new Sintomas((int)leitor["codigoSint"], leitor["nomeSint"].ToString(), (int)leitor["qtdRelatos"]);
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

        public static List<Sintomas> Listar()
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

        public static List<string> RetornaNome(List<int> id)
        {

            List<string> lista = new List<string>();

            try
            {
                con.Open();

                for(int i = 0; i < id.Count; i++)
                {
                    MySqlCommand qry = new MySqlCommand("SELECT * FROM Sintomas WHERE codigoSint = @c", con);

                    qry.Parameters.AddWithValue("@c", id[i]);

                    MySqlDataReader leitor = qry.ExecuteReader();

                    if (leitor.Read())
                    {
                        lista.Add(leitor["nomeSint"].ToString());
                    }

                    leitor.Close();
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

        public static List<int> RetornaId(string[] nome)
        {

            List<int> lista = new List<int>();

            try
            {
                con.Open();

                for(int i = 0; i < nome.Length; i++)
                {
                    MySqlCommand qry = new MySqlCommand("SELECT * FROM Sintomas WHERE nomeSint = @n", con);

                    qry.Parameters.AddWithValue("@n", nome[i]);

                    MySqlDataReader leitor = qry.ExecuteReader();

                    if (leitor.Read())
                    {
                        lista.Add(int.Parse(leitor["codigoSint"].ToString()));
                    }
                    else
                    {
                        lista.Add(-1);
                    }

                    leitor.Close();
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

        public static void Relato(List<int> lista, string codigo, DateTime data)
        {
            try
            {
                con.Open();

                for (int i = 0; i < lista.Count; i++)
                {
					MySqlCommand qry = new MySqlCommand("INSERT INTO Relata VALUES(@d, @cf, @cs)", con);

					MySqlCommand addRelato = new MySqlCommand("UPDATE Sintomas SET qtdRelatos = qtdRelatos + 1 WHERE codigoSint = @c", con);

					qry.Parameters.AddWithValue("@d", data);
                    qry.Parameters.AddWithValue("@cf", codigo);
                    qry.Parameters.AddWithValue("@cs", lista[i]);

                    qry.ExecuteNonQuery();

                    addRelato.Parameters.AddWithValue("@c", lista[i]);

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
    }
}
