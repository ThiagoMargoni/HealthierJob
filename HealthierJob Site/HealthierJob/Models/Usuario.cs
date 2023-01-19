using MySql.Data.MySqlClient;

namespace HealthierJob
{
    public class Usuario
    {

        private string tipo;
        private string status;
        private string codigo;
        private string nomeCompleto;
        private int idade;
        private string areaAtuacao;
        private DateTime dataEntrada;
        private DateTime dataSaida;
        private string senha;
        private byte[] img;

        static MySqlConnection con = new MySqlConnection("server=localhost;database=healthier;user id=root;password=thiago123");

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

        public string Tipo { get => tipo; set => tipo = value; }
        public string Status { get => status; set => status = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string NomeCompleto { get => nomeCompleto; set => nomeCompleto = value; }
        public int Idade { get => idade; set => idade = value; }
        public string AreaAtuacao { get => areaAtuacao; set => areaAtuacao = value; }
        public DateTime DataEntrada { get => dataEntrada; set => dataEntrada = value; }
        public DateTime DataSaida { get => dataSaida; set => dataSaida = value; }
        public string Senha { get => senha; set => senha = value; }

        public byte[] Img { get => img; set => img = value; }

        public string Cadastrar()
        {
            try
            {
                con.Open();

                MySqlCommand c = new MySqlCommand("SELECT * FROM Funcionario WHERE codigoFunc = @codigo", con);
                c.Parameters.AddWithValue("@codigo", codigo);

                MySqlDataReader leitor = c.ExecuteReader();

                if (leitor.Read())
                {
                    return "Você Já Está Cadastrado";
                }
                else
                {

                    leitor.Close();

                    MySqlCommand qry = new MySqlCommand("INSERT INTO Funcionario VALUES(@tipo, @status, @codigo, @nome, @idade, @area, @entrada, @saida, @senha, @img)", con);

                    qry.Parameters.AddWithValue("@tipo", tipo);
                    qry.Parameters.AddWithValue("@status", "ativo");
                    qry.Parameters.AddWithValue("@codigo", codigo);
                    qry.Parameters.AddWithValue("@nome", nomeCompleto);
                    qry.Parameters.AddWithValue("@idade", idade);
                    qry.Parameters.AddWithValue("@area", areaAtuacao);
                    qry.Parameters.AddWithValue("@entrada", dataEntrada);
                    qry.Parameters.AddWithValue("@saida", dataSaida);
                    qry.Parameters.AddWithValue("@senha", senha);
                    qry.Parameters.AddWithValue("@img", img);

                    qry.ExecuteNonQuery();

                    return "Usuário Cadastrado com Sucesso";

                }
            } catch(MySqlException e)
            {
                
                switch(e.Number)
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

        public Usuario Verificar()
        {
            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("SELECT *, DATE_FORMAT(entrada,'%d/%m/%Y') AS entradaA, DATE_FORMAT(saida,'%d/%m/%Y') AS saidaA FROM Funcionario WHERE codigoFunc = @codigo AND senha = @senha", con);
                qry.Parameters.AddWithValue("@codigo", codigo);
                qry.Parameters.AddWithValue("@senha", senha);
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

        public Usuario RetornarUsu()
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

        public string Editar()
        {
            try
            {
                con.Open();

                string comando = "UPDATE Funcionario SET tipo = @tipo, status = @status, nomeCompleto = @nome, idade = @idade, " +
                    "areaAtua = @area, entrada = @entrada, saida = @saida ";

                if (senha != null)
                {
                    comando += ", senha = @senha";
                }

                comando += ", img = @img WHERE codigoFunc = @codigo";

                MySqlCommand qry = new MySqlCommand(comando, con);

                qry.Parameters.AddWithValue("@tipo", tipo);
                qry.Parameters.AddWithValue("@status", status);
                qry.Parameters.AddWithValue("@codigo", codigo);
                qry.Parameters.AddWithValue("@nome", nomeCompleto);
                qry.Parameters.AddWithValue("@idade", idade);
                qry.Parameters.AddWithValue("@area", areaAtuacao);
                qry.Parameters.AddWithValue("@entrada", dataEntrada);
                qry.Parameters.AddWithValue("@saida", dataSaida);
                qry.Parameters.AddWithValue("@senha", senha);
                qry.Parameters.AddWithValue("@img", img);

                qry.ExecuteNonQuery();

                return "Usuário Editado com Sucesso";

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

        public static List<Usuario> Listar()
        {

            List<Usuario> lista = new List<Usuario>();

            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT *, DATE_FORMAT(entrada,'%d/%m/%Y') AS entradaA, DATE_FORMAT(saida,'%d/%m/%Y') AS saidaA FROM Funcionario", con);

                MySqlDataReader leitor = qry.ExecuteReader();

                while (leitor.Read())
                {
                    Usuario u = new Usuario(leitor["tipo"].ToString(), leitor["status"].ToString(), leitor["codigoFunc"].ToString(), leitor["nomeCompleto"].ToString(),
                        (int)leitor["idade"], leitor["areaAtua"].ToString(), DateTime.ParseExact(leitor["entradaA"].ToString(), "dd/MM/yyyy", null),
                        DateTime.ParseExact(leitor["saidaA"].ToString(), "dd/MM/yyyy", null), leitor["senha"].ToString(), (byte[])leitor["img"]);

                    lista.Add(u);
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

        public string InativarAtivar()
        {

            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("UPDATE Funcionario SET status = @status WHERE codigoFunc = @codigo", con);
                qry.Parameters.AddWithValue("@status", status);
                qry.Parameters.AddWithValue("@codigo", codigo);

                qry.ExecuteNonQuery();

                return "Usuário " + status + " com Sucesso";
            } catch (MySqlException e)
            {
                return "Tente Novamente ou Espere até mais Tarde";

            } catch (Exception e)
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
