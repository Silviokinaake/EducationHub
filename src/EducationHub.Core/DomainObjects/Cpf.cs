namespace EducationHub.Core.DomainObjects
{
    public class Cpf
    {
        public const int CpfMaxLength = 11;
        public string Numero { get; private set; }

        protected Cpf() { }

        public Cpf(string numero)
        {
            if (!EhValido(numero))
                throw new DomainException("CPF inválido.");
            Numero = numero.Replace(".", "").Replace("-", "").Trim();
        }

        public static bool EhValido(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != CpfMaxLength) return false;

            var numerosInvalidos = new[]
            {
                "00000000000","11111111111","22222222222","33333333333",
                "44444444444","55555555555","66666666666","77777777777",
                "88888888888","99999999999"
            };

            if (Array.Exists(numerosInvalidos, e => e == cpf)) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf, digito;
            int soma, resto;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito = resto.ToString();

            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public override string ToString() => Numero;
    }
}
