using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalGamess.Aplications.DTOs.GeneroDto;
using RoyalGamess.Aplications.Services;

namespace RoyalGamess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {

        private readonly GeneroService _service;

        public GeneroController(GeneroService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerGeneroDto>> Listar()
        {
            List<LerGeneroDto> list = _service.Listar();
            if (list != null)
                return Ok(list);
            else
                return NotFound(list);
        }

        [HttpGet("{id}")]
        public ActionResult<LerGeneroDto> ObterPorId(int id)
        {
            try
            {
                LerGeneroDto genero = _service.ObterPorId(id);
                return Ok(genero);
            }

            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpGet("nome/{nomeGenero}")]
        public ActionResult<LerGeneroDto> ObterPorNome(string nomeGenero)
        {
            try
            {
                LerGeneroDto genero = _service.ObterPorNome(nomeGenero);
                return Ok(genero);
            }

            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost]
        public ActionResult<LerGeneroDto> Adicionar(CriarGeneroDto criarDto)
        {
            try
            {
                LerGeneroDto genero = _service.Adicionar(criarDto);
                return StatusCode(201, genero);
            }

            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        
        public ActionResult <LerGeneroDto> Atualizar(int id, CriarGeneroDto criarDto)
        {
        try
        {
            LerGeneroDto genero = _service.Atualizar(id, criarDto);
            return StatusCode(200, genero);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
            //
        }
        }
        

        [HttpDelete("{id}")]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
            return StatusCode(200, id);
            }

            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        }
}