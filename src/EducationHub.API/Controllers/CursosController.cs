using EducationHub.Conteudo.Application.Services;
using EducationHub.Conteudo.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly ICursoAppService _cursoAppService;

        public CursosController(ICursoAppService cursoAppService)
        {
            _cursoAppService = cursoAppService;
        }

        /// <summary>
        /// Retorna todos os cursos disponíveis.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObterTodosCursosAsync()
        {
            var cursos = await _cursoAppService.ObterTodosAsync();
            return Ok(cursos);
        }

        /// <summary>
        /// Retorna um curso específico pelo ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterCursoPorIdAsync(Guid id)
        {
            var curso = await _cursoAppService.ObterPorIdAsync(id);
            if (curso == null) return NotFound("Curso não encontrado.");
            return Ok(curso);
        }

        /// <summary>
        /// Adiciona um novo curso.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AdicionarCusroAsync([FromBody] CursoViewModel cursoViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _cursoAppService.AdicionarAsync(cursoViewModel);
            return CreatedAtAction(nameof(ObterCursoPorIdAsync), new { id = cursoViewModel.Id }, cursoViewModel);
        }

        /// <summary>
        /// Atualiza um curso existente.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarCursoAsync(Guid id, [FromBody] CursoViewModel cursoViewModel)
        {
            if (id != cursoViewModel.Id)
                return BadRequest("O ID informado não corresponde ao curso enviado.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _cursoAppService.AtualizarAsync(cursoViewModel);
            return NoContent();
        }

    }
}
