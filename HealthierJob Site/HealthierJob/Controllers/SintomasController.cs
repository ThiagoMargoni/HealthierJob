using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HealthierJob
{
    public class SintomasController : Controller
    {
        MetodosController metodos = new MetodosController();

        public IActionResult Responder()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                if (TempData["media"] != null)
                {

                    TempData["media"] = TempData["media"];

                    return View();

                }
                else
                {
                    TempData["msg"] = "Responda o Questionário Primeiro";

                    return RedirectToAction("Responder", "Questionario");
                }
            }
            else
            {
                TempData["msg"] = "Logue antes de continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }


        [HttpPost]
        public IActionResult Responder(List<string> sintomas, int qtdPassos)
        {

            Usuario u = metodos.RetornaObjeto(HttpContext);

            if (sintomas != null)
            {

                string s = DateTime.Now.ToString();
                DateTime.TryParse(s, out var data);

                List<int> lista = metodos.VoltaCodigo(sintomas);


				Sintomas.Relato(lista, u.Codigo, data);

				Doencas doenca = new Doencas(lista, 0, "", 0, "", 0);
				Doencas d = doenca.PesquisaDoenca();

                int m = int.Parse(TempData["media"].ToString());

				Feedback f = new Feedback(u.Codigo, d.CodDoenca, 0, qtdPassos, DateTime.Now, m, metodos.RetornaRecomendacao(lista, qtdPassos, m));

                TempData["msg"] = f.Adicionar();

                return RedirectToAction("Retornar", "Feedback");
            } else
            {

                TempData["media"] = TempData["media"];

                TempData["msg"] = "Preencher Antes de Enviar";
                return RedirectToAction("Responder");
            }

        }

        public IActionResult Cadastrar()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                if (metodos.VerificaAdm(this.HttpContext) == true)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Inicial", "Home");
                }
            }
            else
            {
                TempData["msg"] = "Logue antes de continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(string nomeSintoma, int qtdRelatos)
        {
            Sintomas s = new Sintomas(0, metodos.RemoveEspaco(nomeSintoma), qtdRelatos);

            TempData["msg"] = s.Cadastrar();

            return RedirectToAction("Cadastrar");
        }

        public IActionResult Editar(int codigo)
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {

                if (metodos.VerificaAdm(this.HttpContext) == true)
                {

                    Sintomas s = new Sintomas(codigo, "", 0);

                    if (s.RetornarSint() != null)
                    {

                        ViewData["s"] = s.RetornarSint();
                        return View();

                    }
                    else
                    {
                        TempData["msg"] = "Não foi Possível Executar essa Ação";
                        return RedirectToAction("Listar");
                    }
                }
                else
                {
                    return RedirectToAction("Inicial", "Home");
                }

            }
            else
            {

                TempData["msg"] = "Logue antes de continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public IActionResult Editar(int codigoSintoma, string nomeSintoma, int qtdRelatos)
        {

            Sintomas s = new Sintomas(codigoSintoma, metodos.RemoveEspaco(nomeSintoma), qtdRelatos);

            TempData["msg"] = s.Editar();

            return RedirectToAction("Listar");
        }

        public IActionResult Listar()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {

                if (metodos.VerificaAdm(this.HttpContext) == true)
                {
                    return View(Sintomas.Listar());
                }
                else
                {
                    return RedirectToAction("Inicial", "Home");
                }

            }
            else
            {

                TempData["msg"] = "Logue antes de continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }
    }
}
