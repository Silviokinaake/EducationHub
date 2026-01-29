using EducationHub.Alunos.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace EducationHub.Alunos.Application.Queries
{
    public class ObterHistoricoAprendizadoQuery : IRequest<IEnumerable<HistoricoAprendizadoViewModel>>
    {
        public Guid AlunoId { get; private set; }

        public ObterHistoricoAprendizadoQuery(Guid alunoId)
        {
            AlunoId = alunoId;
        }
    }
}
