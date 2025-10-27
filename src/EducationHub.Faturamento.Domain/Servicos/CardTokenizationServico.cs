using EducationHub.Faturamento.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EducationHub.Faturamento.Domain.Servicos
{
    public class CardTokenizationService : ICardTokenizationServico
    {
        public Task<string> TokenizarAsync(DadosCartao dadosCartao)
        {
            if (dadosCartao is null) throw new ArgumentNullException(nameof(dadosCartao));
            var nonce = Guid.NewGuid().ToString("N");
            var input = $"{dadosCartao.Numero}|{dadosCartao.Validade}|{nonce}";
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            var token = "tok_" + Convert.ToHexString(hash).ToLowerInvariant().Substring(0, 32);
            return Task.FromResult(token);
        }

        public string MascararNumero(string numeroCartao)
        {
            if (string.IsNullOrWhiteSpace(numeroCartao)) return numeroCartao;
            var digits = numeroCartao.Replace(" ", "");
            if (digits.Length <= 4) return new string('*', digits.Length);
            var last4 = digits[^4..];
            return $"**** **** **** {last4}";
        }
    }
}
