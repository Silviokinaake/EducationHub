using EducationHub.Alunos.Domain.Entidades;

namespace EducationHub.Alunos.Data
{
    public static class AlunoDbInitializer
    {
        public static void Seed(AlunoDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Alunos.Any())
            {
                var aluno = new Aluno(Guid.NewGuid(), Guid.NewGuid(), "Aluno Teste", "aluno.teste@example.com", DateTime.UtcNow.AddYears(-25));
                context.Alunos.Add(aluno);
                context.SaveChanges();
            }
        }
    }
}
