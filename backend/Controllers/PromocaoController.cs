using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalGamess.Aplications.DTOs.PromocaoDto;
using RoyalGamess.Aplications.Services;
using RoyalGamess.Exceptions;

namespace RoyalGamess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocaoController : ControllerBase
    {
        private readonly PromocaoService _service;

        public PromocaoController(PromocaoService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<List<LerPromocaoDto>> Listar()
        {
            List<LerPromocaoDto> promocoes = _service.Listar();
            return Ok(promocoes);
        }
        [HttpGet("{id}")]
        public ActionResult<LerPromocaoDto> ObterPorId(int id)
        {
            try
            {
                LerPromocaoDto promocao = _service.ObterPorId(id);
                return Ok(promocao);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        //[Authorize]
        public ActionResult<LerPromocaoDto> Adicionar(CriarPromocaoDto promocao)
        {
            try
            {
                _service.Adicionar(promocao);
                return StatusCode(201);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        //[Authorize]
        public ActionResult<LerPromocaoDto> Atualizar (int id, CriarPromocaoDto criarDTO)
        {
            try
            {
                _service.Atualizar(id, criarDTO);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        //[Authorize]
        public ActionResult Remover(int id) {
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
