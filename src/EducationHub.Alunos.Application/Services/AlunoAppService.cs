using AutoMapper;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Repositorio;

namespace EducationHub.Alunos.Application.Services
{
    public class AlunoAppService : IAlunoAppService
    {
        private readonly IAlunoRepositorio _alunoRepositorio;
        private readonly IMapper _mapper;

        public AlunoAppService(IAlunoRepositorio alunoRepositorio, IMapper mapper)
        {
            _alunoRepositorio = alunoRepositorio ?? throw new ArgumentNullException(nameof(alunoRepositorio));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AlunoViewModel>> ObterTodosAsync()
        {
            var alunos = await _alunoRepositorio.ObterTodosAsync();
            return _mapper.Map<IEnumerable<AlunoViewModel>>(alunos);
        }

        public async Task<AlunoViewModel?> ObterPorIdAsync(Guid id)
        {
            var aluno = await _alunoRepositorio.ObterPorIdAsync(id);
            return _mapper.Map<AlunoViewModel?>(aluno);
        }

        public async Task<AlunoViewModel?> CriarAsync(AlunoViewModel alunoVm)
        {
            if (alunoVm is null) throw new ArgumentNullException(nameof(alunoVm));

            var aluno = _mapper.Map<Aluno>(alunoVm);

            await _alunoRepositorio.AdicionarAsync(aluno);
            var committed = await _alunoRepositorio.UnitOfWork.Commit();
            if (!committed) return null;

            var resultVm = _mapper.Map<AlunoViewModel>(aluno);
            return resultVm;
        }

        public async Task<bool> AtualizarAsync(AlunoViewModel alunoVm)
        {
            if (alunoVm is null) throw new ArgumentNullException(nameof(alunoVm));

            var existing = await _alunoRepositorio.ObterPorIdAsync(alunoVm.Id);
            if (existing is null) return false;

            existing.SetNome(alunoVm.Nome);
            existing.SetEmail(alunoVm.Email);

            await _alunoRepositorio.AtualizarAsync(existing);
            return await _alunoRepositorio.UnitOfWork.Commit();
        }

        public async Task<MatriculaViewModel?> ObterMatriculaPorIdAsync(Guid matriculaId)
        {
            var matricula = await _alunoRepositorio.ObterMatriculaPorIdAsync(matriculaId);
            return _mapper.Map<MatriculaViewModel?>(matricula);
        }

        public async Task<IEnumerable<MatriculaViewModel>> ObterMatriculasPorAlunoAsync(Guid alunoId)
        {
            var matriculas = await _alunoRepositorio.ObterMatriculasPorAlunoAsync(alunoId);
            return _mapper.Map<IEnumerable<MatriculaViewModel>>(matriculas);
        }

        public async Task<IEnumerable<CertificadoViewModel>> ObterCertificadosPorAlunoAsync(Guid alunoId)
        {
            var certificados = await _alunoRepositorio.ObterCertificadosPorAlunoAsync(alunoId);
            return _mapper.Map<IEnumerable<CertificadoViewModel>>(certificados);
        }

        public void Dispose()
        {
            _alunoRepositorio?.Dispose();
        }
    }
}