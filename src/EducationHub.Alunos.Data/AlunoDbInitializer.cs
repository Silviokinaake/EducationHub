//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//        using EducationHub.Alunos.Domain.Entidades;

//        namespace EducationHub.Alunos.Data
//        {
//            public static class AlunoDbInitializer
//            {
//                // Mantém apenas método assíncrono idempotente
//                public static async Task SeedAsync(AlunoDbContext context)
//                {
//                    // garante que o banco pode ser acessado e aplica migrations se necessário
//                    if (!await context.Database.CanConnectAsync())
//                        await context.Database.MigrateAsync();

//                    // seed seguro: insere apenas se não houver dados
//                    if (!context.Alunos.AsNoTracking().Any())
//                    {
//                        var alunoId = Guid.NewGuid();
//                        var usuarioId = Guid.NewGuid();
//                        var aluno = new Aluno(alunoId, usuarioId, "Aluno Teste", "aluno.teste@example.com", DateTime.UtcNow.AddYears(-25));
//                        await context.Alunos.AddAsync(aluno);

//                        var cursoId = Guid.NewGuid();
//                        var matricula = new Matricula(cursoId, alunoId, 1500m, DateTime.UtcNow);
//                        await context.Matriculas.AddAsync(matricula);

//                        var certificado = new Certificado(alunoId, cursoId, "Curso Exemplo", DateTime.UtcNow);
//                        await context.Certificados.AddAsync(certificado);

//                        await context.SaveChangesAsync();
//                    }
//                }
//            }
//        }
