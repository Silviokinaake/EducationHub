using EducationHub.Faturamento.Domain.Entidades;

namespace EducationHub.Faturamento.Domain.Interfaces
{
    public interface ICardTokenizationServico
    {
        Task<string> TokenizarAsync(DadosCartao dadosCartao);
        string MascararNumero(string numeroCartao);
    }
}
