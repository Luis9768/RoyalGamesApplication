using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalGamess.Aplications.DTOs.ClassificacaoDto;
using RoyalGamess.Aplications.Services;

namespace RoyalGamess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificacaoIndicativaController : ControllerBase
    {

        private readonly ClassificacaoService _service;
        public ClassificacaoIndicativaController(ClassificacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerClassificacaoDto>> Listar()
        {
            List<LerClassificacaoDto> classificacaoDto = _service.Listar();
            if (classificacaoDto == null)
                return NotFound(classificacaoDto);
            else
                return Ok(classificacaoDto);
        }

        [HttpGet("{id}")]
        public ActionResult<LerClassificacaoDto> ObterPorId(int id)
        {
            try
            {
                LerClassificacaoDto classificacaoDto = _service.ObterPorId(id);
                return Ok(classificacaoDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost]
        public ActionResult<LerClassificacaoDto> Adicionar(CriarClassificacaoDto criarDto)
        {
            LerClassificacaoDto classificacaoDto = _service.Adicionar(criarDto);
            //return Created(classificacaoDto);
            return StatusCode(201, classificacaoDto);

        }

        [HttpPut("{id}")]
        public ActionResult Atualizar(int id, CriarClassificacaoDto criarDto)
        {
            LerClassificacaoDto classificacaoDto = _service.Atualizar(id, criarDto);
            if (classificacaoDto == null)
                return NotFound(classificacaoDto);
            else
                return Ok(classificacaoDto);
        }

        [HttpDelete("{id}")]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return Ok(id);
            }

            catch (Exception ex
            )
            {
                return NotFound(ex.Message);
            }
        }
    }
}