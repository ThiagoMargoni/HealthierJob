using MySql.Data.MySqlClient;

namespace HealthierJob
{
    public class Feedback
    {
        private string codigoFunc;
        private int codDoenca;
        private int id;
        private int qtdPassos;
        private DateTime data;
        private int saude;
        private string recomendacao;

        static MySqlConnection con = new MySqlConnection("server=localhost;database=healthier;user id=root;password=thiago123");

        public Feedback(string codigoFunc, int codDoenca, int id, int qtdPassos, DateTime data, int saude, string recomendacao)
        {
            this.codigoFunc = codigoFunc;
            this.id = id;
            this.codDoenca = codDoenca;
            this.qtdPassos = qtdPassos;
            this.data = data;
            this.saude = saude;
            this.recomendacao = recomendacao;  
        }

        public string CodigoFunc { get => codigoFunc; set => codigoFunc = value; }
        public int Id { get => id; set => id = value; }
        public int CodDoenca { get => codDoenca; set => codDoenca = value; }
        public int QtdPassos { get => qtdPassos; set => qtdPassos = value; }
        public DateTime Data { get => data; set => data = value; }
        public int Saude { get => saude; set => saude = value; }
        public string Recomendacao { get => recomendacao; set => recomendacao = value; }

        public string Adicionar()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("INSERT INTO Feedback(fk_codigoFunc, fk_codDoenca, qtdPassos, data, saude, recomendacao) VALUES(@cf, @cd, @q, @d, @s, @r)", con);

                qry.Parameters.AddWithValue("@cf", codigoFunc);
                qry.Parameters.AddWithValue("@cd", codDoenca);
                qry.Parameters.AddWithValue("@q", qtdPassos);
                qry.Parameters.AddWithValue("@d", data);
                qry.Parameters.AddWithValue("@s", saude);
                qry.Parameters.AddWithValue("@r", recomendacao);

                qry.ExecuteNonQuery(); 

                return "Feedback Gerado com Sucesso";

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

        public Feedback RetornaFeedback()
        {
            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT *, DATE_FORMAT(data,'%d/%m/%Y') as dataF FROM Feedback WHERE DATE_FORMAT(data,'%d/%m/%Y') = @d AND fk_codigoFunc = @cf", con);

                string date = data.ToString();
                date = date.Replace("00:00:00", "");
                date = date.Replace(" ", "");

                qry.Parameters.AddWithValue("@d", date);
                qry.Parameters.AddWithValue("@cf", codigoFunc);

                MySqlDataReader leitor = qry.ExecuteReader();

                if(leitor.Read())
                {

                    return new Feedback(leitor["fk_codigoFunc"].ToString(), (int)leitor["fk_codDoenca"], (int) leitor["id_feedback"], (int)leitor["qtdPassos"],
                        DateTime.ParseExact(leitor["dataF"].ToString(), "dd/MM/yyyy", null), (int)leitor["saude"], leitor["recomendacao"].ToString());

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
    }
}
