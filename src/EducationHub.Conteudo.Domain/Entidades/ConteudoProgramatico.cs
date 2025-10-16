namespace EducationHub.Conteudo.Domain.Entidades
{
    public class ConteudoProgramatico
    {
        public string Objetivo { get; private set; }
        public string Conteudo { get; private set; }
        public string Metodologia { get; private set; }
        public string Bibliografia { get; private set; }

        protected ConteudoProgramatico() { }

        public ConteudoProgramatico(string objetivo, string conteudo, string metodologia, string bibliografia)
        {
            Objetivo = objetivo;
            Conteudo = conteudo;
            Metodologia = metodologia;
            Bibliografia = bibliografia;
        }

        public override bool Equals(object obj)
        {
            if (obj is not ConteudoProgramatico other) return false;
            return Objetivo == other.Objetivo &&
                   Conteudo == other.Conteudo &&
                   Metodologia == other.Metodologia &&
                   Bibliografia == other.Bibliografia;
        }

        public override int GetHashCode()
        {
            return (Objetivo + Conteudo + Metodologia + Bibliografia).GetHashCode();
        }
    }
}
