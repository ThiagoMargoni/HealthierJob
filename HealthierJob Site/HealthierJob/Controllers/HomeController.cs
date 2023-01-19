using Microsoft.AspNetCore.Mvc;

namespace HealthierJob
{
    public class HomeController : Controller
    {
        MetodosController metodos = new MetodosController();

        public IActionResult Inicial()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                return View();
            }
            else
            {
                TempData["msg"] = "Logue antes de continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }

        public IActionResult Faq()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                return View();
            }
            else
            {
                TempData["msg"] = "Logue antes de continuar";
                return RedirectToAction("Login", "Usuario");
            }
        }
    }
}
