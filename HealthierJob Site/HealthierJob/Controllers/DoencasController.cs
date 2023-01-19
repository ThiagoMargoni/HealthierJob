using Microsoft.AspNetCore.Mvc;

namespace HealthierJob
{
    public class DoencasController : Controller
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
                TempData["msg"] = "Logue antes de continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(List<string> sintomas, string nome, int qtdCasos, string recomendacao, int nvlPerigo)
        {

            if(sintomas != null)
            {
                Doencas d = new Doencas(metodos.VoltaCodigo(sintomas), 0, metodos.RemoveEspaco(nome), qtdCasos, metodos.RemoveEspaco(recomendacao), nvlPerigo);

                TempData["msg"] = d.Cadastrar();

                return RedirectToAction("Cadastrar");
            }
            else
            {
                TempData["msg"] = "Favor Preencher Todos os Campos";

                return View();
            }

        }

        public IActionResult Listar()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {

                if (metodos.VerificaAdm(this.HttpContext) == true)
                {
                    return View(Doencas.Listar());
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

        public IActionResult Editar(int id)
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                if (metodos.VerificaAdm(this.HttpContext) == true)
                {
                    Doencas d = new Doencas(null, id, "", 0, "", 0);

                    ViewData["d"] = d.RetornarDoenca();

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
        public IActionResult Editar(List<string> sintomas, int codDoenca,string nome, int qtdCasos, string recomendacao, int nvlPerigo)
        {

            if (sintomas != null)
            {
                Doencas d = new Doencas(metodos.VoltaCodigo(sintomas), codDoenca, metodos.RemoveEspaco(nome), qtdCasos, metodos.RemoveEspaco(recomendacao), nvlPerigo);

                TempData["msg"] = d.Editar();

                return RedirectToAction("Listar");
            }
            else
            {
                TempData["msg"] = "Favor Preencher Todos os Campos";
                TempData["cod"] = codDoenca;

                return View();
            }
        }
    }
}
