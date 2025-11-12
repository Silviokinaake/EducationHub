using EducationHub.Core.DomainObjects;

namespace EducationHub.Faturamento.Domain.Entidades
{
    public class DadosCartao
    {
        public string NomeTitular { get; private set; }
        public string Numero { get; private set; }
        public string Validade { get; private set; } // formato MM/AA
        public string Cvv { get; private set; }

        protected DadosCartao() { }

        public DadosCartao(string nomeTitular, string numero, string validade, string cvv)
        {
            NomeTitular = nomeTitular;
            Numero = numero;
            Validade = validade;
            Cvv = cvv;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(NomeTitular, "O nome do titular é obrigatório.");
            Validacoes.ValidarTamanho(NomeTitular, 3, 150, "O nome do titular deve ter entre 3 e 150 caracteres.");

            Validacoes.ValidarSeVazio(Numero, "O número do cartão é obrigatório.");
            Validacoes.ValidarTamanho(Numero, 13, 19, "O número do cartão deve ter entre 13 e 19 dígitos.");

            Validacoes.ValidarSeVazio(Validade, "A validade é obrigatória.");
            Validacoes.ValidarSeDiferente(@"^(0[1-9]|1[0-2])\/\d{2}$", Validade, "A validade deve estar no formato MM/AA.");

            Validacoes.ValidarSeVazio(Cvv, "O código de segurança (CVV) é obrigatório.");
            Validacoes.ValidarTamanho(Cvv, 3, 4, "O CVV deve conter 3 ou 4 dígitos.");
        }
    }
}
