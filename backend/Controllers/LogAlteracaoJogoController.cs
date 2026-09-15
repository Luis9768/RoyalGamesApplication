using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalGamess.Aplications.DTOs.LogAlteracaoJogoDto;
using RoyalGamess.Aplications.Services;

namespace RoyalGamess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAlteracaoJogoController : ControllerBase
    {
        private readonly LogAlteracaoJogoService _service;
       public LogAlteracaoJogoController(LogAlteracaoJogoService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<List<LerLogAlteracaoJogoDto>> Listar()
        {
            List<LerLogAlteracaoJogoDto> list = _service.Listar();
            return list;
        }
        [HttpGet("{id}")]
        public ActionResult<List<LerLogAlteracaoJogoDto>> ListarPorProduto(int id)
        {
            List<LerLogAlteracaoJogoDto> list = _service.ListarPorProduto(id);
            return list;
        }
    }
}
