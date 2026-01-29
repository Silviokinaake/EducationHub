using EducationHub.Alunos.Application.Queries;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Conteudo.Application.Queries;
using EducationHub.Core.Messages;
using MediatR;

namespace EducationHub.Alunos.Application.Commands
{
    public class MatricularAlunoCommandHandler :
        IRequestHandler<MatricularAlunoCommand, MatriculaRealizadaViewModel>
    {
        private readonly IMediator _mediator;
        private readonly IMatriculaRepository _matriculaRepository;

        public MatricularAlunoCommandHandler(
            IMediator mediator,
            IMatriculaRepository matriculaRepository)
        {
            _mediator = mediator;
            _matriculaRepository = matriculaRepository;
        }

        public async Task<MatriculaRealizadaViewModel> Handle(MatricularAlunoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return null;

            // Buscar dados do aluno
            var aluno = await _mediator.Send(new ObterAlunoPorIdQuery(message.AlunoId), cancellationToken);
            if (aluno == null) return null;

            // Buscar dados do curso
            var curso = await _mediator.Send(new ObterCursoPorIdQuery(message.CursoId), cancellationToken);
            if (curso == null) return null;

            // Criar a matrícula com status Pendente (0)
            var matricula = new Matricula(
                cursoId: message.CursoId,
                alunoId: message.AlunoId,
                valor: (decimal)curso.CargaHoraria.TotalHours * 50m, // Exemplo: R$ 50 por hora
                dataMatricula: DateTime.UtcNow
            );

            // Persistir no banco de dados
            await _matriculaRepository.AdicionarAsync(matricula);
            await _matriculaRepository.CommitAsync();

            return new MatriculaRealizadaViewModel
            {
                MatriculaId = matricula.Id,
                Message = "Matrícula realizada com sucesso. Status: Pendente de pagamento.",
                Aluno = new AlunoMatriculaViewModel
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    Email = aluno.Email,
                    DataNascimento = aluno.DataNascimento
                },
                Curso = new CursoMatriculaViewModel
                {
                    Id = curso.Id,
                    Titulo = curso.Titulo,
                    CargaHoraria = curso.CargaHoraria,
                    Descricao = curso.Descricao,
                    Instrutor = curso.Instrutor,
                    Nivel = curso.Nivel,
                    Valor = (decimal)curso.CargaHoraria.TotalHours * 50m
                }
            };
        }

        private bool ValidarComando(MatricularAlunoCommand message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                //_mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
