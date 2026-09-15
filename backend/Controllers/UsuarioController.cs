using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalGamess.Aplications.DTOs.UsuarioDto;
using RoyalGamess.Aplications.Services;

namespace RoyalGamess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;
        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<List<LerUsuarioDto>> Listar()
        {
            List<LerUsuarioDto> list = _service.Listar();
            if (list == null)
            {
                return NotFound(list);
            }
            else
            {
                return Ok(list);
            }

        }
        [HttpGet("{id}")]
        public ActionResult<LerUsuarioDto> ObterPorId(int id)
        {
            try
            {
                LerUsuarioDto usuario = _service.ObterPorId(id);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("email/{email}")]
        public ActionResult<LerUsuarioDto> ObterPorEmail(string email)
        {
            try
            {
                LerUsuarioDto usuario = _service.ObterPorEmail(email);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<LerUsuarioDto> Adicionar(CriarUsuarioDto criarDto)
        {
            try
            {
                LerUsuarioDto lerDto = _service.Adicionar(criarDto);
                return StatusCode(201, lerDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public ActionResult<LerUsuarioDto> Atualizar(int id, CriarUsuarioDto criarDto)
        {
            try
            {
                LerUsuarioDto lerDto = _service.Atualizar(id,criarDto);
                return Ok(lerDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
