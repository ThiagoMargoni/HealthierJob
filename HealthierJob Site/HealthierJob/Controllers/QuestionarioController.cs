using Microsoft.AspNetCore.Mvc;

namespace HealthierJob
{
    public class QuestionarioController : Controller
    {
        MetodosController metodos = new MetodosController();

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
                TempData["msg"] = "Logue antes de Continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(string pergunta)
        {

            Questionario q = new Questionario(0, metodos.RemoveEspaco(pergunta));

            TempData["msg"] = q.Cadastrar();

            return RedirectToAction("Cadastrar");
        }

        public IActionResult Editar(int id)
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                if (metodos.VerificaAdm(this.HttpContext) == true)
                {
                    Questionario q = new Questionario(id, "");

                    ViewData["q"] = q.RetornarQuest();

                    return View();
                }
                else
                {
                    return RedirectToAction("Inicial", "Home");
                }
            }
            else
            {
                TempData["msg"] = "Logue antes de Continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public IActionResult Editar(int id, string pergunta) 
        {

            Questionario q = new Questionario(id, metodos.RemoveEspaco(pergunta));

            TempData["msg"] = q.Editar();

            return RedirectToAction("Listar");
        }

        public IActionResult Listar()
        {

            if (metodos.VerificaLogado(this.HttpContext) == true)
            {

                if (metodos.VerificaAdm(this.HttpContext) == true)
                {
                    return View(Questionario.Listar());
                }
                else
                {
                    return RedirectToAction("Inicial", "Home");
                }

            }
            else
            {
                TempData["msg"] = "Logue antes de Continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }

        public IActionResult Responder()
        {

            List<Questionario> perguntas = Questionario.Listar();

            if (metodos.VerificaLogado(this.HttpContext) == true)
            {

                Usuario u = metodos.RetornaObjeto(this.HttpContext);

                string s = DateTime.Now.ToShortDateString();
                DateTime.TryParse(s, out var data);

                Feedback f = new Feedback(u.Codigo, 0, 0, 0, data, 0, "");

                if(f.RetornaFeedback() == null)
                {
					int param = 0;

					if (TempData["perg"] != null)
					{
						param = (int)TempData["perg"];
					}

					if (perguntas == null)
					{
						if (metodos.VerificaAdm(this.HttpContext) == true)
						{
							TempData["msg"] = "Cadastre uma Pergunta Antes de Continuar";

							return RedirectToAction("Cadastrar");
						}
						else
						{
							TempData["msg"] = "Peça para o Administrador Cadastrar Perguntas";

							return RedirectToAction("Inicial", "Home");
						}
					}
					else
					{

						Questionario q = perguntas[param];

						ViewData["q"] = q;

						return View();
					}
				} else
                {
                    TempData["msg"] = "Você já respondeu seu questionário hoje";
                    return RedirectToAction("Retornar", "Feedback");
                }
            }
            else
            {
                TempData["msg"] = "Logue antes de Continuar";
                return RedirectToAction("Login", "Usuario");
            }

        }

        [HttpPost]
        public IActionResult Responder(int numPerg, int valor, int total)
        {

            List<Questionario> perguntas = Questionario.Listar();

            if (numPerg == perguntas.Count)
            {
                total += valor;
                TempData["media"] = total/numPerg;
                return RedirectToAction("Responder", "Sintomas");

            }
            else
            {
                TempData["perg"] = numPerg;
                TempData["tot"] = total += valor;

                return RedirectToAction("Responder");
            }
        }

        public IActionResult Remover(int id)
        {

            Questionario q = new Questionario(id, "");

            TempData["msg"] = q.Remover();

            return RedirectToAction("Listar");
        }
    }
}
