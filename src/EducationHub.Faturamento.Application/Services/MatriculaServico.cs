using EducationHub.Alunos.Domain.Enums;
using EducationHub.Alunos.Domain.Interfaces;
using EducationHub.Core.DomainObjects;
using EducationHub.Faturamento.Domain.Interfaces;

namespace EducationHub.Faturamento.Application.Services
{
    public class MatriculaServico : IMatriculaServico
    {
        private readonly IMatriculaRepository _matriculaRepository;

        public MatriculaServico(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository ?? throw new ArgumentNullException(nameof(matriculaRepository));
        }

        public async Task<bool> AtivarMatriculaAsync(Guid preMatriculaId)
        {
            var matricula = await _matriculaRepository.ObterPorId(preMatriculaId);
            
            if (matricula == null)
                throw new DomainException("Matrícula não encontrada.");

            // Verificar se a matrícula está com status Pendente
            if (matricula.Status != StatusMatriculaEnum.Pendente)
                throw new DomainException($"Não é possível realizar o pagamento. Status atual da matrícula: {matricula.Status}. Apenas matrículas com status Pendente podem ser pagas.");

            matricula.Ativar();
            _matriculaRepository.Atualizar(matricula);
            
            await _matriculaRepository.UnitOfWork.Commit();
            
            return true;
        }

        /// <summary>
        /// Valida e ativa a matrícula após confirmação do pagamento.
        /// Este método verifica se a matrícula está Pendente e se o valor pago corresponde ao valor do curso.
        /// </summary>
        public async Task<bool> AtivarMatriculaAsync(Guid preMatriculaId, decimal valorPago)
        {
            var matricula = await _matriculaRepository.ObterPorId(preMatriculaId);
            
            if (matricula == null)
                throw new DomainException("Matrícula não encontrada.");

            // Verificar se a matrícula está com status Pendente
            if (matricula.Status != StatusMatriculaEnum.Pendente)
                throw new DomainException($"Não é possível realizar o pagamento. Status atual da matrícula: {matricula.Status}. Apenas matrículas com status Pendente podem ser pagas.");

            // Verificar se o valor pago é igual ao valor da matrícula (que corresponde ao valor do curso)
            if (valorPago != matricula.Valor)
                throw new DomainException($"O valor do pagamento (R$ {valorPago:F2}) deve ser igual ao valor do curso (R$ {matricula.Valor:F2}).");

            // Ativa a matrícula após todas as validações
            matricula.Ativar();
            _matriculaRepository.Atualizar(matricula);
            
            await _matriculaRepository.UnitOfWork.Commit();
            
            return true;
        }

        /// <summary>
        /// Valida se a matrícula pode receber pagamento SEM ativar.
        /// Usado para validação antecipada antes de processar o pagamento.
        /// </summary>
        public async Task ValidarPagamentoMatriculaAsync(Guid preMatriculaId, decimal valorPago)
        {
            var matricula = await _matriculaRepository.ObterPorId(preMatriculaId);
            
            if (matricula == null)
                throw new DomainException("Matrícula não encontrada.");

            // Verificar se a matrícula está com status Pendente
            if (matricula.Status != StatusMatriculaEnum.Pendente)
                throw new DomainException($"Não é possível realizar o pagamento. Status atual da matrícula: {matricula.Status}. Apenas matrículas com status Pendente podem ser pagas.");

            // Verificar se o valor pago é igual ao valor da matrícula (que corresponde ao valor do curso)
            if (valorPago != matricula.Valor)
                throw new DomainException($"O valor do pagamento (R$ {valorPago:F2}) deve ser igual ao valor do curso (R$ {matricula.Valor:F2}).");
        }
    }
}
