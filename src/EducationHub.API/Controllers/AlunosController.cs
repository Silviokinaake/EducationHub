using EducationHub.Alunos.Application.Commands;
using EducationHub.Alunos.Application.Queries;
using EducationHub.Core.DomainObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlunosController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Obtém todos os alunos (requer Administrador).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObterTodosAlunos()
        {
            var query = new ObterAlunosQuery();
            var alunos = await _mediator.Send(query);
            return Ok(alunos);
        }

        /// <summary>
        /// Obtém os dados do aluno logado.
        /// </summary>
        [HttpGet("logado")]
        [Authorize]
        public async Task<IActionResult> ObterAlunoLogado()
        {
            // Obter o ID do usuário logado
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");
            
            // Obter o email do usuário logado
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            
            // Tentar buscar o aluno pelo usuarioId primeiro
            var aluno = await _mediator.Send(new ObterAlunoPorUsuarioIdQuery(Guid.Parse(usuarioId)));
            
            // Se não encontrou pelo usuarioId, busca por email (fallback para compatibilidade)
            if (aluno == null && !string.IsNullOrEmpty(email))
            {
                var todosAlunos = await _mediator.Send(new ObterAlunosQuery());
                aluno = todosAlunos.FirstOrDefault(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            
            if (aluno == null) 
                return NotFound("Aluno não encontrado para o usuário logado.");
            
            return Ok(aluno);
        }

        /// <summary>
        /// Obtém um aluno por id.
        /// </summary>
        [HttpGet("{id:guid}", Name = "GetAlunoById")]
        [Authorize]
        public async Task<IActionResult> ObterAlunoPorId(Guid id)
        {
            var query = new ObterAlunoPorIdQuery(id);
            var aluno = await _mediator.Send(query);
            
            if (aluno == null) return NotFound();
            return Ok(aluno);
        }

        /// <summary>
        /// Cria um novo aluno.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AdicionarAluno([FromBody] CriarAlunoCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Obter o ID do usuário logado
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");
            
            // Atribui o usuarioId do usuário logado ao command
            command.UsuarioId = Guid.Parse(usuarioId);

            var resultado = await _mediator.Send(command);
            
            if (!resultado) 
                return BadRequest("Não foi possível criar o aluno. CPF ou Email já cadastrados.");

            // Busca o aluno criado para retornar o ViewModel completo
            var aluno = await _mediator.Send(new ObterAlunoPorIdQuery(command.Id));
            
            return CreatedAtRoute("GetAlunoById", new { id = command.Id }, aluno);
        }

        /// <summary>
        /// Matricula o aluno em um curso.
        /// </summary>
        [HttpPost("{id:guid}/matriculas")]
        [Authorize]
        public async Task<IActionResult> MatricularAluno(Guid id, [FromBody] MatricularAlunoCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            if (id == Guid.Empty)
                return BadRequest("O Id do aluno é obrigatório.");
            
            // Atribui o alunoId da rota ao command
            command.AlunoId = id;

            var resultado = await _mediator.Send(command);
            
            if (resultado == null) 
                return BadRequest("Não foi possível realizar a matrícula. Verifique se o aluno e o curso existem.");

            return Ok(resultado);
        }

        /// <summary>
        /// Obtém todas as matrículas (Administrador) ou as matrículas do aluno logado.
        /// </summary>
        [HttpGet("matriculas")]
        [Authorize]
        public async Task<IActionResult> ObterMatriculas()
        {
            // Verifica se o usuário é administrador
            if (User.IsInRole("Administrador"))
            {
                var todasMatriculas = await _mediator.Send(new ObterTodasMatriculasQuery());
                return Ok(todasMatriculas);
            }

            // Se não for administrador, busca apenas as matrículas do aluno logado
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");

            // Busca o aluno pelo usuarioId
            var aluno = await _mediator.Send(new ObterAlunoPorUsuarioIdQuery(Guid.Parse(usuarioId)));
            if (aluno == null)
                return NotFound("Aluno não encontrado para o usuário logado.");

            // Retorna as matrículas do aluno
            var matriculas = await _mediator.Send(new ObterMatriculasPorAlunoIdQuery(aluno.Id));
            return Ok(matriculas);
        }

        /// <summary>
        /// Obtém as matrículas de um aluno específico com dados dos cursos.
        /// Aluno comum: somente suas próprias matrículas.
        /// Administrador: matrículas de qualquer aluno.
        /// </summary>
        [HttpGet("{alunoId:guid}/matriculas")]
        [Authorize]
        public async Task<IActionResult> ObterMatriculasPorAlunoId(Guid alunoId)
        {
            if (alunoId == Guid.Empty)
                return BadRequest("O Id do aluno é obrigatório.");

            // Se não for administrador, valida se está consultando suas próprias matrículas
            if (!User.IsInRole("Administrador"))
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(usuarioId))
                    return Unauthorized("Usuário não autenticado.");

                // Busca o aluno pelo ID da URL para validar
                var alunoConsultado = await _mediator.Send(new ObterAlunoPorIdQuery(alunoId));
                if (alunoConsultado == null)
                    return NotFound("Aluno não encontrado.");

                // Verifica se o aluno pertence ao usuário logado (por usuarioId ou email)
                var email = User.FindFirst(ClaimTypes.Name)?.Value;
                var pertenceAoUsuario = 
                    (alunoConsultado.UsuarioId.ToString() == usuarioId) ||
                    (alunoConsultado.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (!pertenceAoUsuario)
                    return Forbid(); // 403 - não pode ver matrículas de outro aluno
            }

            var resultado = await _mediator.Send(new ObterAlunoComMatriculasQuery(alunoId));
            
            if (resultado == null)
                return NotFound("Aluno não encontrado.");

            return Ok(resultado);
        }

        /// <summary>
        /// Conclui uma matrícula (altera status para Concluída).
        /// Somente matrículas com status Ativo podem ser concluídas.
        /// </summary>
        [HttpPost("matriculas/{matriculaId:guid}/concluir")]
        [Authorize]
        public async Task<IActionResult> ConcluirMatricula(Guid matriculaId)
        {
            if (matriculaId == Guid.Empty)
                return BadRequest("O Id da matrícula é obrigatório.");

            try
            {
                var command = new ConcluirMatriculaCommand(matriculaId);
                var resultado = await _mediator.Send(command);

                if (!resultado)
                    return BadRequest("Não foi possível concluir a matrícula.");

                return Ok(new { message = "Matrícula concluída com sucesso.", matriculaId });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Emite certificado para uma matrícula concluída.
        /// Somente matrículas com status Concluída podem gerar certificado.
        /// </summary>
        [HttpPost("matriculas/{matriculaId:guid}/certificado")]
        [Authorize]
        public async Task<IActionResult> EmitirCertificado(Guid matriculaId)
        {
            if (matriculaId == Guid.Empty)
                return BadRequest("O Id da matrícula é obrigatório.");

            try
            {
                var command = new EmitirCertificadoCommand(matriculaId);
                var certificado = await _mediator.Send(command);

                return Ok(certificado);
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtém o histórico de aprendizado do aluno logado.
        /// Retorna todos os cursos concluídos com ID, nome, data de início e data de conclusão.
        /// </summary>
        [HttpGet("historicoAprendizado")]
        [Authorize]
        public async Task<IActionResult> ObterHistoricoAprendizado()
        {
            // Obter o ID do usuário logado
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized("Usuário não autenticado.");

            // Busca o aluno pelo usuarioId
            var aluno = await _mediator.Send(new ObterAlunoPorUsuarioIdQuery(Guid.Parse(usuarioId)));
            if (aluno == null)
                return NotFound("Aluno não encontrado para o usuário logado.");

            // Busca o histórico de cursos concluídos
            var historico = await _mediator.Send(new ObterHistoricoAprendizadoQuery(aluno.Id));
            
            return Ok(historico);
        }
    }
}
