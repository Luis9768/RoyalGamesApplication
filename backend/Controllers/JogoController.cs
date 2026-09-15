using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RoyalGamess.Aplications.DTOs.JogoDto;
using RoyalGamess.Aplications.Services;
using RoyalGamess.Exceptions;


namespace RoyalGamess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogoController : ControllerBase
    {

        private readonly JogoService _service;
        public JogoController(JogoService service)
        {
            _service = service;
        }

        private int ObterUsuarioIdLogado()
        {
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idTexto))
                throw new DomainException("Usuário não encontrado!");


            return int.Parse(idTexto);
        }

        [HttpGet]
        public ActionResult<List<LerJogoDto>> Listar()
        {
            List<LerJogoDto> jogos = _service.Listar();
            if (jogos == null)
                return NotFound(jogos);
            else
                return Ok(jogos);

        }

        [HttpGet("{id}")]
        public ActionResult<LerJogoDto> ObterPorId(int id)
        {
            try
            {
                LerJogoDto jogo = _service.ObterPorId(id);
                return Ok(jogo);
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }


        }

        [HttpGet("nome/{nomeJogo}")]
        public ActionResult<LerJogoDto> ObterPorNome(string nomeJogo)
        {
            try
            {
                LerJogoDto jogo = _service.ObterPorNome(nomeJogo);
                return Ok(jogo);
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/imagem")]
        public ActionResult ObterImagem(int id)
        {
            try

            {

                var imagem = _service.ObterImagem(id);

                return File(imagem, "image/jpeg");
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public ActionResult Adicionar( [FromForm] CriarJogoDto jogoDto)
        {
            try
            {
                int usuarioId = ObterUsuarioIdLogado();
                _service.Adicionar(usuarioId, jogoDto);
                return StatusCode(201);
            }

            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize]

        public ActionResult Atualizar([FromForm] AtualizarJogoDto jogoDto, int id)
        {
            try
            {
                _service.Atualizar(jogoDto, id);
                return Ok();
            }

            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize]

        public ActionResult Delete(int id)
        {
            try
            {
                _service.Remover(id);
                return StatusCode(204, id);
            }

            catch (DomainException ex)
            {
                return StatusCode(400, ex);
            }
        }


    }
}
