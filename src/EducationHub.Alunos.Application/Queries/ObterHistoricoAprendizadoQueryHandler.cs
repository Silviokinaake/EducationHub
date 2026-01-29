using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Enums;
using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Conteudo.Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EducationHub.Alunos.Application.Queries
{
    public class ObterHistoricoAprendizadoQueryHandler : IRequestHandler<ObterHistoricoAprendizadoQuery, IEnumerable<HistoricoAprendizadoViewModel>>
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IMediator _mediator;

        public ObterHistoricoAprendizadoQueryHandler(IMatriculaRepository matriculaRepository, IMediator mediator)
        {
            _matriculaRepository = matriculaRepository ?? throw new ArgumentNullException(nameof(matriculaRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<HistoricoAprendizadoViewModel>> Handle(ObterHistoricoAprendizadoQuery request, CancellationToken cancellationToken)
        {
            // Acessa o DbContext através do UnitOfWork
            var dbContext = _matriculaRepository.UnitOfWork as DbContext;
            if (dbContext == null)
                return Enumerable.Empty<HistoricoAprendizadoViewModel>();

            // Busca todas as matrículas concluídas do aluno
            var matriculasConcluidas = await dbContext.Set<EducationHub.Alunos.Domain.Entidades.Matricula>()
                .Where(m => m.AlunoId == request.AlunoId && m.Status == StatusMatriculaEnum.Concluida)
                .ToListAsync(cancellationToken);

            if (!matriculasConcluidas.Any())
                return Enumerable.Empty<HistoricoAprendizadoViewModel>();

            var historico = new List<HistoricoAprendizadoViewModel>();

            foreach (var matricula in matriculasConcluidas)
            {
                // Busca os dados do curso via MediatR
                var curso = await _mediator.Send(new ObterCursoPorIdQuery(matricula.CursoId), cancellationToken);

                if (curso != null)
                {
                    historico.Add(new HistoricoAprendizadoViewModel
                    {
                        CursoId = matricula.CursoId,
                        NomeCurso = curso.Titulo,
                        DataInicio = matricula.DataAtivacao ?? matricula.DataMatricula,
                        DataConclusao = matricula.DataConclusao ?? DateTime.UtcNow
                    });
                }
            }

            return historico.OrderByDescending(h => h.DataConclusao);
        }
    }
}
