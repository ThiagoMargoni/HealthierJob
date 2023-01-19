using Microsoft.AspNetCore.Mvc;

namespace HealthierJob.Controllers
{
    public class FeedbackController : Controller
    {
        
        MetodosController metodos = new MetodosController();

        public IActionResult Retornar()
        {

			if (metodos.VerificaLogado(HttpContext) == true)
			{

                Usuario u = metodos.RetornaObjeto(HttpContext);

                string s = DateTime.Now.ToShortDateString();
                DateTime.TryParse(s, out var data);

                Feedback f = new Feedback(u.Codigo, 0, 0, 0, data, 0, "");

                if (f.RetornaFeedback() != null)
                {
                    ViewData["f"] = f.RetornaFeedback();

                    return View();
                } else
                {
                    TempData["msg"] = "Responda o Questionário para Obter o Feedback";
                    return RedirectToAction("Responder", "Questionario");
                }
			}
			else
			{
                TempData["msg"] = "Logue antes de continuar";
				return RedirectToAction("Login", "Usuario");
			}
			
        }

        public IActionResult Calendario()
        {

            if(metodos.VerificaLogado(HttpContext) == true)
            {
				if (TempData["data"] != null)
				{
					Usuario u = metodos.RetornaObjeto(HttpContext);

                    string s = TempData["data"].ToString();
                    DateTime.TryParse(s, out var data);

                    Feedback f = new Feedback(u.Codigo, 0, 0, 0, data, 0, "");

					if(f.RetornaFeedback() != null)
                    {
                        ViewData["f"] = f.RetornaFeedback();
                    } else
                    {
                        ViewData["f"] = "vazio";
                    }
				}

				return View();
			} else
            {
				TempData["msg"] = "Logue antes de continuar";
				return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public IActionResult Calendario(DateTime data)
        {

            TempData["data"] = data;

            return RedirectToAction("Calendario");
        }
    }
}
