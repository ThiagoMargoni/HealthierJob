using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthierJob
{
    public class MetodosController : Controller
    {

        public bool VerificaLogado(HttpContext httpContext)
        {
            if (httpContext.Session.GetString("user") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Código Retorna Objeto Uusário*/
        public Usuario RetornaObjeto(HttpContext httpContext)
        {
            return JsonConvert.DeserializeObject<Usuario>(httpContext.Session.GetString("user"));
        }

        /*Código Verifca se é Adm ou Uusário*/
        public bool VerificaAdm(HttpContext httpContext)
        {

            Usuario u = RetornaObjeto(httpContext);

            if (u.Tipo == "adm")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*Código Remove Espaço*/
        public string RemoveEspaco(string texto)
        {
            texto = texto.Trim();

            for (int i = 0; i < texto.Length; i++)
            {
                texto = texto.Replace("  ", " ");
            }

            return texto;
        }

        public List<string> VoltaNome(List<int> sintomas)
        {
            List<string> nomes = new List<string>();

            nomes = Sintomas.RetornaNome(sintomas);

            return nomes;
        }

        public List<int> VoltaCodigo(List<string> lista)
        {
            List<int> sintomas = new List<int>();

            if (!(lista.Contains("Nada")))
            {
                string[] texto = lista[0].Split(",");

                sintomas = Sintomas.RetornaId(texto);
            }
            else
            {
                sintomas.Add(0);
            }

            return sintomas;
        }

        public string RemoverZero(string data)
        {

            data = data.Replace("00:00:00", "");

            return data;
        }

        public string RetornaRecomendacao(List<int> sintomas, int qtdPassos, int media)
        {
            string recomendacao = "";

            if (sintomas[0] != -1)
            {

                Doencas doenca = new Doencas(sintomas, 0, "", 0, "", 0);

                Doencas d = doenca.PesquisaDoenca();

                recomendacao += "Com base nos Sintomas relatados, recomendamos que fique atento na seguinte doença: " + d.Nome + ". E com base nela, a sugestão" +
                    " médica é: " + d.Recomendacao;
            } else
            {
                if(media <= 5 && media > 3)
                {
					recomendacao += "Como você não relatou nenhum sintoma, deduzimos com base nas suas respostas do questionário que, sua saúde está boa.";
				} else if(media <= 3 && media > 2)
                {
					recomendacao += "Como você não relatou nenhum sintoma, recomendamos com base nas suas respostas do questionário que, uma boa noite de sono" +
                        " e praticar esportes são essenciais para melhorar o humor. Se o problema persistir, converse com um médico e com seu chefe.";
				} else
                {
                    recomendacao += "Como você não relatou nenhum sintoma, recomendamos com base nas suas respostas do questionário que, fique alerta quanto a sua saúde." +
						" Busque pedir ajuda, para tentar resolver seus problemas o quanto antes. Recomendamos também uma consulta médica.";
                }
            }

            if(qtdPassos < 9000)
            {
                recomendacao += ". Quando analisada sua média de passos diária, percebe-se que você está abaixo da média. Portanto recomendamos que dê uma caminhada e tome um pouco de ar";
            } 

            return recomendacao;
        }
    }
}
