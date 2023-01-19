using MySql.Data.MySqlClient;

namespace HealthierJob
{
    public class Questionario
    {
        private int id_perg;
        private string pergunta;

        static MySqlConnection con = new MySqlConnection("server=localhost;database=healthier;user id=root;password=thiago123");

        public Questionario(int id_perg, string pergunta)
        {
            this.id_perg = id_perg;
            this.pergunta = pergunta;
        }
        public int Id_perg { get => id_perg; set => id_perg = value; }

        public string Pergunta { get => pergunta; set => pergunta = value; }

        public string Cadastrar()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("INSERT INTO Quest(pergunta) VALUES(@pergunta)", con);

                qry.Parameters.AddWithValue("@pergunta", pergunta);

                qry.ExecuteNonQuery();

                return "Pergunta Cadastrada com Sucesso";

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

            } finally
            {
                con.Close();
            }
        }

        public string Editar()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("UPDATE Quest SET pergunta = @p WHERE id_perg = @i", con);
                qry.Parameters.AddWithValue("@i", id_perg);
                qry.Parameters.AddWithValue("@p", pergunta);

                qry.ExecuteNonQuery();

                return "Pergunta Editada com Sucesso";

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

        public Questionario RetornarQuest()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM Quest WHERE id_perg = @i", con);
                qry.Parameters.AddWithValue("@i", id_perg);

                MySqlDataReader leitor = qry.ExecuteReader();

                if (leitor.Read())
                {
                    return new Questionario((int)leitor["id_perg"], leitor["pergunta"].ToString());
                } else
                {
                    return null;
                }

            } catch(Exception e)
            {

                return null;

            } finally
            {
                con.Close();
            }
        }

        public static List<Questionario> Listar()
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

        public string Remover()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("DELETE FROM Quest WHERE id_perg = @i", con);
                qry.Parameters.AddWithValue("@i", id_perg);

                qry.ExecuteNonQuery();

                return "Pergunta Removida com Sucesso";

            }
            catch (MySqlException e)
            {
                return "Tente Novamente ou Espere até mais Tarde";

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
    }
}
