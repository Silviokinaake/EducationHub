using AutoMapper;
using EducationHub.Conteudo.Application.Services;
using EducationHub.Conteudo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AulasController : ControllerBase
    {
        private readonly IAulaAppService _aulaAppService;
        private readonly IMapper _mapper;

        public AulasController(IAulaAppService aulaAppService, IMapper mapper)
        {
            _aulaAppService = aulaAppService;
            _mapper = mapper;
        }
        /// <summary>
        /// Cria uma nova aula.
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> AdicionarAulaAsync([FromBody] CriarAulaViewModel criarAulaViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var aulaViewModel = _mapper.Map<AulaViewModel>(criarAulaViewModel);
            aulaViewModel.Id = Guid.NewGuid();

            await _aulaAppService.AdicionarAsync(aulaViewModel);

            return CreatedAtRoute("GetAulaById", new { id = aulaViewModel.Id }, aulaViewModel);
        }

        /// <summary>
        /// Obtém todas as aulas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObterTodasAulasAsync()
        {
            var aulas = await _aulaAppService.ObterTodosAsync();
            return Ok(aulas);
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
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarAulaAsync(Guid id, [FromBody] AtualizarAulaViewModel atualizarAulaViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var aulaViewModel = _mapper.Map<AulaViewModel>(atualizarAulaViewModel);
            aulaViewModel.Id = id;

            await _aulaAppService.AtualizarAsync(aulaViewModel);
            return NoContent();
        }

        /// <summary>
        /// Exclui uma aula. A aula será desvinculada do curso ao qual está atrelada.
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> ExcluirAulaAsync(Guid id)
        {
            try
            {
                await _aulaAppService.RemoverAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
