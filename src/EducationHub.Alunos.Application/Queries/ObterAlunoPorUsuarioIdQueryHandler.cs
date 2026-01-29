using AutoMapper;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Repositorio;
using MediatR;

namespace EducationHub.Alunos.Application.Queries
{
    public class ObterAlunoPorUsuarioIdQueryHandler : IRequestHandler<ObterAlunoPorUsuarioIdQuery, AlunoViewModel>
    {
        private readonly IAlunoRepositorio _alunoRepositorio;
        private readonly IMapper _mapper;

        public ObterAlunoPorUsuarioIdQueryHandler(IAlunoRepositorio alunoRepositorio, IMapper mapper)
        {
            _alunoRepositorio = alunoRepositorio;
            _mapper = mapper;
        }

        public async Task<AlunoViewModel> Handle(ObterAlunoPorUsuarioIdQuery request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepositorio.ObterPorUsuarioIdAsync(request.UsuarioId);
            return _mapper.Map<AlunoViewModel>(aluno);
        }
    }
}
