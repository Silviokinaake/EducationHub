using EducationHub.Conteudo.Application.Commands;
using EducationHub.Conteudo.Application.Queries;
using EducationHub.Conteudo.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CursosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna todos os cursos disponíveis.
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
        /// <param name="situacao">Filtro de situação do curso (Ativo=1, Inativo=2)</param>
        [HttpGet]
        public async Task<IActionResult> ObterTodosCursosAsync(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] EducationHub.Conteudo.Domain.Enums.SituacaoCurso? situacao = null)
        {
            var query = new ObterCursosQuery(pageNumber, pageSize, situacao);
            var cursos = await _mediator.Send(query);
            return Ok(cursos);
        }

        /// <summary>
        /// Retorna um curso específico pelo ID.
        /// </summary>
        [HttpGet("{id:guid}", Name = "ObterCursoPorId")]
        public async Task<IActionResult> ObterCursoPorIdAsync(Guid id)
        {
            var query = new ObterCursoPorIdQuery(id);
            var curso = await _mediator.Send(query);
            
            if (curso == null) 
                return NotFound("Curso não encontrado.");
                
            return Ok(curso);
        }

        /// <summary>
        /// Adiciona um novo curso (requer autenticação como Administrador).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AdicionarCursoAsync([FromBody] CriarCursoCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _mediator.Send(command);
            
            if (!resultado)
                return BadRequest("Não foi possível criar o curso.");

            // Busca o curso criado para retornar o ViewModel completo
            var curso = await _mediator.Send(new ObterCursoPorIdQuery(command.Id));

            return CreatedAtRoute("ObterCursoPorId", new { id = command.Id }, curso);
        }

        /// <summary>
        /// Atualiza um curso existente (requer autenticação como Administrador).
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarCursoAsync(Guid id, [FromBody] AtualizarCursoCommand command)
        {
            command.Id = id;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _mediator.Send(command);
            
            if (!resultado)
                return BadRequest("Não foi possível atualizar o curso.");

            return NoContent();
        }

        /// <summary>
        /// Inativa um curso (requer autenticação como Administrador).
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> InativarCursoAsync(Guid id)
        {
            var command = new InativarCursoCommand(id);
            var resultado = await _mediator.Send(command);
            
            if (!resultado)
                return NotFound("Curso não encontrado.");

            return NoContent();
        }
    }
}
