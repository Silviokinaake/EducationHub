using EducationHub.Alunos.Application.Queries;
using EducationHub.Alunos.Application.ViewModels;
using EducationHub.Alunos.Domain.Entidades;
using EducationHub.Alunos.Domain.Enums;
using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Conteudo.Application.Queries;
using EducationHub.Core.DomainObjects;
using MediatR;

namespace EducationHub.Alunos.Application.Commands
{
    public class EmitirCertificadoCommandHandler : IRequestHandler<EmitirCertificadoCommand, CertificadoEmitidoViewModel>
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICertificadoRepository _certificadoRepository;
        private readonly IMediator _mediator;

        public EmitirCertificadoCommandHandler(
            IMatriculaRepository matriculaRepository,
            ICertificadoRepository certificadoRepository,
            IMediator mediator)
        {
            _matriculaRepository = matriculaRepository;
            _certificadoRepository = certificadoRepository;
            _mediator = mediator;
        }

        public async Task<CertificadoEmitidoViewModel> Handle(EmitirCertificadoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido()) return null;

            var matricula = await _matriculaRepository.ObterPorId(request.MatriculaId);
            
            if (matricula == null)
                throw new DomainException("Matrícula não encontrada.");

            // Validar se matrícula está concluída
            if (matricula.Status != StatusMatriculaEnum.Concluida)
                throw new DomainException($"Não é possível emitir certificado. A matrícula deve estar concluída. Status atual: {matricula.Status}");

            // Verificar se já existe certificado para esta matrícula
            var certificadoExistente = await _certificadoRepository.ObterPorMatriculaId(request.MatriculaId);
            if (certificadoExistente != null)
                throw new DomainException("Certificado já foi emitido para esta matrícula.");

            // Buscar dados do aluno
            var aluno = await _mediator.Send(new ObterAlunoPorIdQuery(matricula.AlunoId), cancellationToken);
            if (aluno == null)
                throw new DomainException("Aluno não encontrado.");

            // Buscar dados do curso
            var curso = await _mediator.Send(new ObterCursoPorIdQuery(matricula.CursoId), cancellationToken);
            if (curso == null)
                throw new DomainException("Curso não encontrado.");

            // Criar certificado
            var certificado = new Certificado(
                alunoId: matricula.AlunoId,
                cursoId: matricula.CursoId,
                tituloCurso: curso.Titulo,
                dataEmissao: DateTime.UtcNow
            );

            await _certificadoRepository.AdicionarAsync(certificado);
            await _certificadoRepository.UnitOfWork.Commit();

            // Gerar mensagem do certificado
            var mensagem = GerarMensagemCertificado(aluno.Nome, curso.Titulo, curso.CargaHoraria, certificado.DataEmissao, certificado.Codigo);

            return new CertificadoEmitidoViewModel
            {
                CertificadoId = certificado.Id,
                Codigo = certificado.Codigo,
                DataEmissao = certificado.DataEmissao,
                MensagemCertificado = mensagem,
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
                    Valor = curso.Valor
                }
            };
        }

        private string GerarMensagemCertificado(string nomeAluno, string tituloCurso, TimeSpan cargaHoraria, DateTime dataEmissao, string codigo)
        {
            return $@"
╔═══════════════════════════════════════════════════════════════════╗
║                      CERTIFICADO DE CONCLUSÃO                     ║
╚═══════════════════════════════════════════════════════════════════╝

Certificamos que

    {nomeAluno.ToUpper()}

concluiu com êxito o curso

    {tituloCurso}

com carga horária de {cargaHoraria.TotalHours} horas, 
realizado através da plataforma EducationHub.

Data de Conclusão: {dataEmissao:dd/MM/yyyy}
Código de Verificação: {codigo}

___________________________
      EducationHub
 Educação de Qualidade
";
        }
    }
}
