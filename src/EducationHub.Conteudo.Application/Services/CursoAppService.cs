using AutoMapper;
using EducationHub.Conteudo.Application.ViewModels;
using EducationHub.Conteudo.Domain.Entidades;
using EducationHub.Conteudo.Domain.Repositorios;

namespace EducationHub.Conteudo.Application.Services
{
    public class CursoAppService : ICursoAppService
    {
        private readonly ICursoRepositorio _cursoRepositorio;
        private readonly IMapper _mapper;

        public CursoAppService(ICursoRepositorio cursoRepositorio, IMapper mapper)
        {
            _cursoRepositorio = cursoRepositorio ?? throw new ArgumentNullException(nameof(cursoRepositorio));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CursoViewModel> ObterPorId(Guid id)
        {
            var curso = await _cursoRepositorio.ObterPorIdAsync(id);
            return _mapper.Map<CursoViewModel>(curso);
        }

        public async Task<IEnumerable<CursoViewModel>> ObterTodos()
        {
            var cursos = await _cursoRepositorio.ObterTodosAsync();
            return _mapper.Map<IEnumerable<CursoViewModel>>(cursos);
        }

        public async Task Adicionar(CursoViewModel cursoViewModel)
        {
            if (cursoViewModel is null) throw new ArgumentNullException(nameof(cursoViewModel));

            var curso = _mapper.Map<Curso>(cursoViewModel);
            await _cursoRepositorio.AdicionarAsync(curso);

            var committed = await _cursoRepositorio.UnitOfWork.Commit();
            if (!committed)
                throw new InvalidOperationException("Não foi possível persistir o novo curso.");
        }

        public async Task Atualizar(CursoViewModel cursoViewModel)
        {
            if (cursoViewModel is null) throw new ArgumentNullException(nameof(cursoViewModel));

            var curso = _mapper.Map<Curso>(cursoViewModel);
            await _cursoRepositorio.AtualizarAsync(curso);

            var committed = await _cursoRepositorio.UnitOfWork.Commit();
            if (!committed)
                throw new InvalidOperationException("Não foi possível atualizar o curso.");
        }

        public void Dispose()
        {
            _cursoRepositorio?.Dispose();
        }
    }
}
