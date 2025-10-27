using AutoMapper;
using EducationHub.Conteudo.Application.ViewModels;
using EducationHub.Conteudo.Domain.Interfaces;

namespace EducationHub.Conteudo.Application.Services
{
    public class AulaAppService : IAulaAppService
    {
        private readonly IAulaRepositorio _aulaRepositorio;
        private readonly IMapper _mapper;
        public AulaAppService(IAulaRepositorio aulaRepositorio, IMapper mapper)
        {
            _aulaRepositorio = aulaRepositorio ?? throw new ArgumentNullException(nameof(aulaRepositorio));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AulaViewModel>> ObterPorCursoAsync(Guid cursoId)
        {
            var aulas = await _aulaRepositorio.ObterTodosAsync(cursoId);
            return _mapper.Map<IEnumerable<AulaViewModel>>(aulas);
        }

        public async Task<AulaViewModel?> ObterPorIdAsync(Guid id)
        {
            var aula = await _aulaRepositorio.ObterPorIdAsync(id);
            return _mapper.Map<AulaViewModel?>(aula);
        }

        public async Task AdicionarAsync(AulaViewModel aulaViewModel)
        {
            if (aulaViewModel is null) throw new ArgumentNullException(nameof(aulaViewModel));

            var aula = _mapper.Map<Domain.Entidades.Aula>(aulaViewModel);
            await _aulaRepositorio.AdicionarAsync(aula);

            var committed = await (_aulaRepositorio as dynamic).UnitOfWork.Commit();
            if (!committed)
                throw new InvalidOperationException("Não foi possível persistir a nova aula.");
        }

        public async Task AtualizarAsync(AulaViewModel aulaViewModel)
        {
            if (aulaViewModel is null) throw new ArgumentNullException(nameof(aulaViewModel));

            var aula = _mapper.Map<Domain.Entidades.Aula>(aulaViewModel);
            await _aulaRepositorio.AtualizarAsync(aula);

            var committed = await (_aulaRepositorio as dynamic).UnitOfWork.Commit();
            if (!committed)
                throw new InvalidOperationException("Não foi possível atualizar a aula.");
        }

        public void Dispose()
        {
            _aulaRepositorio?.Dispose();
        }
    }
}
