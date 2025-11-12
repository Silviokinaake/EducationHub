using EducationHub.Conteudo.Application.Services;
using EducationHub.Conteudo.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AulasController : ControllerBase
    {
        private readonly IAulaAppService _aulaAppService;

        public AulasController(IAulaAppService aulaAppService)
        {
            _aulaAppService = aulaAppService;
        }
        /// <summary>
        /// Cria uma nova aula.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AdicionarAulaAsync([FromBody] AulaViewModel aulaViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _aulaAppService.AdicionarAsync(aulaViewModel);

            return CreatedAtRoute("GetAulaById", new { id = aulaViewModel.Id }, aulaViewModel);
        }

        /// <summary>
        /// Obtém todas as aulas de um curso específico.
        /// </summary>
        [HttpGet("curso/{cursoId:guid}")]
        public async Task<IActionResult> ObterAulasDeUmCursoAsync(Guid cursoId)
        {
            var aulas = await _aulaAppService.ObterPorCursoAsync(cursoId);
            return Ok(aulas);
        }

        /// <summary>
        /// Obtém uma aula pelo identificador.
        /// </summary>
        [HttpGet("{id:guid}", Name = "GetAulaById")]
        public async Task<IActionResult> ObterAulaPorIdAsync(Guid id)
        {
            var aula = await _aulaAppService.ObterPorIdAsync(id);
            if (aula is null) return NotFound();
            return Ok(aula);
        }

        /// <summary>
        /// Atualiza uma aula existente.
        /// </summary>

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarAulaAsync(Guid id, [FromBody] AulaViewModel aulaViewModel)
        {
            if (id != aulaViewModel.Id)
                return BadRequest("O id informado não corresponde ao id do payload.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _aulaAppService.AtualizarAsync(aulaViewModel);
            return NoContent();
        }
    }
}
