using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthierJob
{
    public class UsuarioController : Controller
    {
        MetodosController metodos = new MetodosController();

        public IActionResult Cadastrar()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                if (metodos.VerificaAdm(this.HttpContext) == true)
                {

                    if (TempData["msg"] != null)
                    {
                        TempData.Clear(); 
                    } 

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
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(string tipo, string status, string codigo, string nomeCompleto, int idade, string areaAtuacao,
            DateTime dataEntrada, DateTime dataSaida, string senha)
        {

            byte[] img = null;

            try
            {
                foreach (IFormFile arquivo in Request.Form.Files)
                {
                    string tipoArquivo = arquivo.ContentType;

                    if (tipoArquivo.Contains("png") || tipoArquivo.Contains("jpg") || tipoArquivo.Contains("jpeg"))
                    {
                        MemoryStream s = new MemoryStream();
                        arquivo.CopyToAsync(s);
                        byte[] bytesArquivo = s.ToArray();

                        img = bytesArquivo;
                    }
                    else
                    {
                        TempData["msg"] = "Favor Inserir um Tipo de Imagem Válida";
                        return View();
                    }
                }
            } catch (Exception e)
            {
                TempData["msg"] = "Tamanho da Imagem Muito Grande";
                return View();
            }

            Usuario u = new Usuario(metodos.RemoveEspaco(tipo), metodos.RemoveEspaco(status), metodos.RemoveEspaco(codigo), metodos.RemoveEspaco(nomeCompleto),
                idade, metodos.RemoveEspaco(areaAtuacao), dataEntrada, dataSaida, metodos.RemoveEspaco(senha), img);

            TempData["msg"] = u.Cadastrar();

            if (TempData["msg"].ToString().Contains("Sucesso"))
            {
                return RedirectToAction("Cadastrar");

            } else
            {
                return View();
            }
        }

        public IActionResult Login()
        {
            if (metodos.VerificaLogado(this.HttpContext) == false)
            {
                return View();
            }
            else
            {
                TempData["msg"] = "Você já está logado";
                return RedirectToAction("Inicial", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(string codigo, string senha)
        {

            DateTime data = DateTime.ParseExact("11/11/1111", "dd/MM/yyyy", null);

            Usuario u = new Usuario("", "", codigo, "", 0, "", data, data, senha, null);

            Usuario user = u.Verificar();

            if (user != null)
            {

                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));

                return RedirectToAction("Inicial", "Home");

            }
            else
            {
                TempData["msg"] = "Usuário ou Senha Incorretos";
                return RedirectToAction("Login");
            }

        }

        public IActionResult Editar(string id)
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                if (metodos.VerificaAdm(this.HttpContext) == true)
                {

                    Usuario sessao = metodos.RetornaObjeto(HttpContext);

                    if (id == sessao.Codigo)
                    {

                        ViewData["u"] = sessao;
                        return View();

                    }
                    else
                    {

                        DateTime data = DateTime.ParseExact("11/11/1111", "dd/MM/yyyy", null);

                        Usuario u = new Usuario("", "", id, "", 0, "", data, data, "", null);

                        if (u.RetornarUsu() != null)
                        {
                            ViewData["u"] = u.RetornarUsu();
                            return View();

                        }
                        else
                        {
                            TempData["msg"] = "Não foi Possível Executar essa Ação";
                            return RedirectToAction("Listar");
                        }

                    }
                }
                else
                {
                    return RedirectToAction("Inicial", "Home");
                }
            }
            else
            {
                TempData["msg"] = "Logue Antes de Continuar";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Editar(string tipo, string status, string codigo, string nomeCompleto, int idade, string areaAtuacao,
            DateTime dataEntrada, DateTime dataSaida, string senha)
        {
            byte[] img = null;

            try
            {
                foreach (IFormFile arquivo in Request.Form.Files)
                {
                    string tipoArquivo = arquivo.ContentType;

                    if (tipoArquivo.Contains("png") || tipoArquivo.Contains("jpg") || tipoArquivo.Contains("jpeg"))
                    {
                        MemoryStream s = new MemoryStream();
                        arquivo.CopyToAsync(s);
                        byte[] bytesArquivo = s.ToArray();

                        img = bytesArquivo;
                    }
                    else
                    {
                        TempData["msg"] = "Favor Inserir um Tipo de Imagem Válida";
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                TempData["msg"] = "Tamanho da Imagem Muito Grande";
                return View();
            }

            if (senha != null)
            {
                senha = metodos.RemoveEspaco(senha);
            } 

            Usuario u = new Usuario(metodos.RemoveEspaco(tipo), metodos.RemoveEspaco(status), codigo, metodos.RemoveEspaco(nomeCompleto),
                idade, metodos.RemoveEspaco(areaAtuacao), dataEntrada, dataSaida, senha, img);

            Usuario user = metodos.RetornaObjeto(HttpContext);

            TempData["msg"] = u.Editar();

            if (user.Codigo == codigo)
            {
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(u));
            }

            return RedirectToAction("Listar");
        }

        public IActionResult Listar()
        {
            if (metodos.VerificaLogado(this.HttpContext) == true)
            {
                if (metodos.VerificaAdm(this.HttpContext) == true)
                {
                    return View(Usuario.Listar());
                }
                else
                {
                    return RedirectToAction("Inicial", "Home");
                }
            }
            else
            {
                TempData["msg"] = "Logue antes de Continuar";
                return RedirectToAction("Login");
            }
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Remove("user");

            TempData.Clear();

            return RedirectToAction("Login");
        }

        public IActionResult InativarAtivar(string acao, string id)
        {
            DateTime data = DateTime.ParseExact("11/11/1111", "dd/MM/yyyy", null);

            Usuario u = new Usuario("", acao, id, "", 0, "", data, data, "", null);

            TempData["msg"] = u.InativarAtivar();

            return RedirectToAction("Listar");
        }
    }
}
