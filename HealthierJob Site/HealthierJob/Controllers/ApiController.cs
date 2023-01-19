using Microsoft.AspNetCore.Mvc;

namespace HealthierJob
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {

        MetodosController m = new MetodosController();

        [HttpGet]
        public List<Sintomas> Retornar()
        {
            List<Sintomas> list = Sintomas.Listar();

            return list;
        }

        [Route("{id:int}")]
        [HttpGet]
        public List<string> SintDoenca(int id)
        {
            Doencas d = new Doencas(null, id, "", 0, "", 0);

            Doencas doencas = d.RetornarDoenca();

            List<int> lista = doencas.Sintomas;

            List<string> sint = m.VoltaNome(lista);

            return sint;
        }
    }
}
