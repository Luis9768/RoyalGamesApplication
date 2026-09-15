using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalGamess.Aplications.DTOs.PlataformaDto;
using RoyalGamess.Aplications.Services;
using RoyalGamess.Exceptions;

namespace RoyalGamess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlataformaController : ControllerBase
    {
        private readonly PlataformaService _service;
        public PlataformaController(PlataformaService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerPlataformaDto>> Listar()
        {
            List<LerPlataformaDto> lista = _service.Listar();
            return Ok(lista);
        }
        [HttpGet("{id}")]
        public ActionResult<LerPlataformaDto> ObterPorId(int id)
        {
            try
            {
                LerPlataformaDto plat = _service.ObterPorId(id);
                return Ok(plat);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<LerPlataformaDto> Adicionar(CriarPlataformaDto criarDto)
        {
            try
            {
                _service.Adicionar(criarDto);
                return StatusCode(201);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<LerPlataformaDto> Atualizar(int id, CriarPlataformaDto criarDto)
        {
            try
            {
                _service.Atualizar(id, criarDto);
                return Ok();
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
