using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Repositorio;
using MediatR;

namespace EducationHub.Alunos.Application.Commands
{
    public class CriarAlunoCommandHandler : IRequestHandler<CriarAlunoCommand, bool>
    {
        private readonly IAlunoRepositorio _alunoRepositorio;

        public CriarAlunoCommandHandler(IAlunoRepositorio alunoRepositorio)
        {
            _alunoRepositorio = alunoRepositorio;
        }

        public async Task<bool> Handle(CriarAlunoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return false;

            // Verificar se já existe aluno com mesmo CPF ou Email
            var alunoExistente = await _alunoRepositorio.ObterPorCpf(request.Cpf);
            if (alunoExistente != null)
                return false;

            var aluno = new Aluno(
                request.Id,
                request.UsuarioId, // Usar o UsuarioId do usuário autenticado
                request.Nome,
                request.Email,
                request.DataNascimento
            );

            _alunoRepositorio.Adicionar(aluno);
            return await _alunoRepositorio.UnitOfWork.Commit();
        }
    }
}
