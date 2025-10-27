using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoRepositorio _alunoRepositorio;

        public AlunosController(IAlunoRepositorio alunoRepositorio)
        {
            _alunoRepositorio = alunoRepositorio ?? throw new ArgumentNullException(nameof(alunoRepositorio));
        }

        /// <summary>
        /// Obtém todos os alunos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObterTodosAlunos()
        {
            var alunos = await _alunoRepositorio.ObterTodosAsync();
            return Ok(alunos);
        }

        /// <summary>
        /// Obtém um aluno por id.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterAlunoPorId(Guid id)
        {
            var aluno = await _alunoRepositorio.ObterPorIdAsync(id);
            if (aluno == null) return NotFound();
            return Ok(aluno);
        }

        /// <summary>
        /// Cria um novo aluno.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AdicionarAluno([FromBody] CreateAlunoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = Guid.NewGuid();
            var aluno = new Aluno(id, request.UsuarioId, request.Nome, request.Email, request.DataNascimento);

            await _alunoRepositorio.AdicionarAsync(aluno);
            if (!await CommitIfPossible()) return StatusCode(500, "Erro ao persistir novo aluno.");

            return CreatedAtRoute(new { id = aluno.Id }, aluno);
        }

        /// <summary>
        /// Atualiza um aluno existente.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AlualizarAluno(Guid id, [FromBody] UpdateAlunoRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != request.Id) return BadRequest("O id informado não corresponde ao id do payload.");

            var existing = await _alunoRepositorio.ObterPorIdAsync(id);
            if (existing == null) return NotFound();

            existing.SetNome(request.Nome);
            existing.SetEmail(request.Email);

            await _alunoRepositorio.AtualizarAsync(existing);
            if (!await CommitIfPossible()) return StatusCode(500, "Erro ao persistir atualização do aluno.");

            return NoContent();
        }

        public class CreateAlunoRequest
        {
            [Required] public Guid UsuarioId { get; set; }
            [Required] [StringLength(150, MinimumLength = 3)] public string Nome { get; set; }
            [Required] [EmailAddress] public string Email { get; set; }
            [Required] public DateTime DataNascimento { get; set; }
        }

        public class UpdateAlunoRequest
        {
            [Required] public Guid Id { get; set; }
            [Required] [StringLength(150, MinimumLength = 3)] public string Nome { get; set; }
            [Required] [EmailAddress] public string Email { get; set; }
        }

        private async Task<bool> CommitIfPossible()
        {
            try
            {
                var prop = _alunoRepositorio.GetType().GetProperty("UnitOfWork", BindingFlags.Instance | BindingFlags.Public);
                if (prop is null) return true;

                var unitOfWork = prop.GetValue(_alunoRepositorio);
                if (unitOfWork == null) return false;

                var commitMethod = unitOfWork.GetType().GetMethod("Commit", BindingFlags.Instance | BindingFlags.Public);
                if (commitMethod == null) return false;

                var task = commitMethod.Invoke(unitOfWork, Array.Empty<object>()) as Task<bool>;
                if (task == null) return false;

                return await task;
            }
            catch
            {
                return false;
            }
        }
    }
}
