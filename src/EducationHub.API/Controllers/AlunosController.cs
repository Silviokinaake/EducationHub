using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EducationHub.Alunos.Application.Services;
using EducationHub.Alunos.Application.ViewModels;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoAppService _alunoAppService;

        public AlunosController(IAlunoAppService alunoAppService)
        {
            _alunoAppService = alunoAppService ?? throw new ArgumentNullException(nameof(alunoAppService));
        }

        /// <summary>
        /// Obtém todos os alunos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObterTodosAlunos()
        {
            var alunos = await _alunoAppService.ObterTodosAsync();
            return Ok(alunos);
        }

        /// <summary>
        /// Obtém um aluno por id.
        /// </summary>
        [HttpGet("{id:guid}", Name = "GetAlunoById")]
        public async Task<IActionResult> ObterAlunoPorId(Guid id)
        {
            var aluno = await _alunoAppService.ObterPorIdAsync(id);
            if (aluno == null) return NotFound();
            return Ok(aluno);
        }

        /// <summary>
        /// Cria um novo aluno.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AdicionarAluno([FromBody] AlunoViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _alunoAppService.CriarAsync(request);
            if (created is null) return StatusCode(500, "Erro ao persistir novo aluno.");

            return CreatedAtRoute("GetAlunoById", new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza um aluno existente.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarAluno(Guid id, [FromBody] AlunoViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != request.Id) return BadRequest("O id informado não corresponde ao id do payload.");

            var existing = await _alunoAppService.ObterPorIdAsync(id);
            if (existing == null) return NotFound();

            var ok = await _alunoAppService.AtualizarAsync(request);
            if (!ok) return StatusCode(500, "Erro ao persistir atualização do aluno.");

            return NoContent();
        }
    }
}