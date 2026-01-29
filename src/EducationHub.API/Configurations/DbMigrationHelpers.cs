using EducationHub.Alunos.Data;
using EducationHub.Conteudo.Data;
using EducationHub.Faturamento.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text;

namespace EducationHub.API.Configurations
{
    public static class DbMigrationHelperExtensions
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }
    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            var alunoContext = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
            var conteudoContext = scope.ServiceProvider.GetRequiredService<ConteudoDbContext>();
            var faturamentoContext = scope.ServiceProvider.GetRequiredService<FaturamentoDbContext>();
            
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (env.IsDevelopment())
            {
                // Apenas executa migrations se for banco relacional (não InMemory para testes)
                if (!alunoContext.Database.ProviderName!.Contains("InMemory"))
                    await alunoContext.Database.MigrateAsync();
                    
                if (!conteudoContext.Database.ProviderName!.Contains("InMemory"))
                    await conteudoContext.Database.MigrateAsync();
                    
                if (!faturamentoContext.Database.ProviderName!.Contains("InMemory"))
                    await faturamentoContext.Database.MigrateAsync();

                // Criar usuário administrador padrão
                await EnsureSeedIdentityData(userManager, roleManager);
                
                await EnsureSeedProducts(alunoContext, conteudoContext, faturamentoContext);
            }
        }

        private static async Task EnsureSeedIdentityData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Criar role Administrador se não existir
            if (!await roleManager.RoleExistsAsync("Administrador"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrador"));
            }

            // Criar usuário admin padrão se não existir
            const string adminEmail = "admin@educationhub.com";
            const string adminPassword = "Admin@123";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }
            else
            {
                // Se o usuário já existe, garante que tem a role e senha correta
                var roles = await userManager.GetRolesAsync(adminUser);
                if (!roles.Contains("Administrador"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
                
                // Atualiza a senha removendo a antiga e adicionando nova
                await userManager.RemovePasswordAsync(adminUser);
                await userManager.AddPasswordAsync(adminUser, adminPassword);
            }

            // Criar usuário aluno padrão se não existir
            const string alunoEmail = "aluno1@educationhub.com";
            const string alunoPassword = "Aluno1@123";
            var alunoUser = await userManager.FindByEmailAsync(alunoEmail);
            
            if (alunoUser == null)
            {
                alunoUser = new IdentityUser
                {
                    UserName = alunoEmail,
                    Email = alunoEmail,
                    EmailConfirmed = true
                };
                
                await userManager.CreateAsync(alunoUser, alunoPassword);
            }
            else
            {
                // Se o usuário já existe, atualiza a senha
                await userManager.RemovePasswordAsync(alunoUser);
                await userManager.AddPasswordAsync(alunoUser, alunoPassword);
            }
        }

        private static async Task EnsureSeedProducts(AlunoDbContext alunoContext, ConteudoDbContext conteudoContext, FaturamentoDbContext faturamentoContext)
        {
            // Alunos / Matrículas / Certificados
            if (!alunoContext.Alunos.Any())
            {
                // Criar 4 alunos de exemplo
                var alunos = new[]
                {
                    new Alunos.Domain.Entidades.Aluno(Guid.NewGuid(), Guid.NewGuid(), "Aluno1 Teste", "aluno1@example.com", DateTime.UtcNow.AddYears(-26)),
                    new Alunos.Domain.Entidades.Aluno(Guid.NewGuid(), Guid.NewGuid(), "Aluno2 Teste", "aluno2@example.com", DateTime.UtcNow.AddYears(-25)),
                    new Alunos.Domain.Entidades.Aluno(Guid.NewGuid(), Guid.NewGuid(), "Aluno3 Teste", "aluno3@example.com", DateTime.UtcNow.AddYears(-25)),
                    new Alunos.Domain.Entidades.Aluno(Guid.NewGuid(), Guid.NewGuid(), "Alun4 Teste", "aluno4@example.com", DateTime.UtcNow.AddYears(-25))
                };
                
                await alunoContext.Alunos.AddRangeAsync(alunos);
                await alunoContext.SaveChangesAsync();
            }

            // Conteúdo (Cursos / Aulas)
            if (!conteudoContext.Cursos.Any())
            {
                // ConteudoProgramatico VO — assume ctor (objetivo, conteudo, metodologia, bibliografia)
                var cp = new Conteudo.Domain.Entidades.ConteudoProgramatico(
                    "Aprender os conceitos básicos",
                    "Conceitos, exercícios e práticas",
                    "Aulas expositivas e hands-on",
                    "Documentação oficial e artigos");

                var curso = new Conteudo.Domain.Entidades.Curso(
                    "Introdução ao C#",
                    "Curso básico de C# para iniciantes",
                    TimeSpan.FromHours(10),
                    "Prof. Exemplo",
                    "Iniciante",
                    500.00m,
                    cp);

                await conteudoContext.Cursos.AddAsync(curso);
                await conteudoContext.SaveChangesAsync();

                // Após salvar curso, criar aulas vinculadas
                var aula1 = new Conteudo.Domain.Entidades.Aula("Boas-vindas", "Apresentação do curso", "Slides de apresentação do curso e material introdutório", TimeSpan.FromMinutes(30), curso.Id);
                var aula2 = new Conteudo.Domain.Entidades.Aula("Sintaxe básica", "Variáveis, tipos e operadores", "Apostila em PDF com exemplos de código", TimeSpan.FromMinutes(60), curso.Id);

                await conteudoContext.Aulas.AddRangeAsync(aula1, aula2);
                await conteudoContext.SaveChangesAsync();
            }

            // Faturamento (Pagamentos)
            if (!faturamentoContext.Pagamentos.Any())
            {
                // Usar um aluno/matricula previamente gerado para referenciar
                var exemploAluno = alunoContext.Alunos.FirstOrDefault();
                var exemploMatricula = alunoContext.Matriculas.FirstOrDefault();

                var alunoRef = exemploAluno?.Id ?? Guid.NewGuid();
                var preMatriculaRef = exemploMatricula?.Id ?? Guid.NewGuid();

                var dadosCartao = new Faturamento.Domain.Entidades.DadosCartao("Aluno2 Teste2", "4111111111111112", "12/30", "123");

                var pagamento = new Faturamento.Domain.Entidades.Pagamento(alunoRef, preMatriculaRef, exemploMatricula?.Valor ?? 1500m, dadosCartao);
                // opcional: aplicar token/mascara via serviço — aqui persistimos token nulo (será gerado pelo fluxo real)
                faturamentoContext.Pagamentos.Add(pagamento);
                await faturamentoContext.SaveChangesAsync();
            }
            try
            {
                var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedScripts");
                Directory.CreateDirectory(outputDir);

                var schemaConteudo = conteudoContext.Database.GenerateCreateScript();
                var schemaAlunos = alunoContext.Database.GenerateCreateScript();
                var schemaFaturamento = faturamentoContext.Database.GenerateCreateScript();

                await File.WriteAllTextAsync(Path.Combine(outputDir, "schema_conteudo.sql"), schemaConteudo, Encoding.UTF8);
                await File.WriteAllTextAsync(Path.Combine(outputDir, "schema_alunos.sql"), schemaAlunos, Encoding.UTF8);
                await File.WriteAllTextAsync(Path.Combine(outputDir, "schema_faturamento.sql"), schemaFaturamento, Encoding.UTF8);

                var sbAlunos = new StringBuilder();
                foreach (var a in alunoContext.Alunos.AsNoTracking().ToList())
                {
                    sbAlunos.AppendLine(
                        $"INSERT INTO Alunos (Id, UsuarioId, Nome, Email, DataNascimento) VALUES ('{a.Id}', '{a.UsuarioId}', '{a.Nome.Replace("'", "''")}', '{a.Email}', '{a.DataNascimento:yyyy-MM-dd HH:mm:ss}');");
                }
                await File.WriteAllTextAsync(Path.Combine(outputDir, "seed_alunos.sql"), sbAlunos.ToString(), Encoding.UTF8);

                var sbConteudo = new StringBuilder();
                foreach (var c in conteudoContext.Cursos.AsNoTracking().ToList())
                {
                    sbConteudo.AppendLine(
                        $"INSERT INTO Cursos (Id, Titulo, Descricao, CargaHoraria, Instrutor, Situacao, Nivel, ConteudoProgramatico_Objetivo, ConteudoProgramatico_Conteudo, ConteudoProgramatico_Metodologia, ConteudoProgramatico_Bibliografia) VALUES ('{c.Id}', '{c.Titulo.Replace("'", "''")}', '{c.Descricao.Replace("'", "''")}', '{c.CargaHoraria}', '{c.Instrutor.Replace("'", "''")}', {(int)c.Situacao}, '{c.Nivel}', '{c.ConteudoProgramatico.Objetivo.Replace("'", "''")}', '{c.ConteudoProgramatico.Conteudo.Replace("'", "''")}', '{c.ConteudoProgramatico.Metodologia.Replace("'", "''")}', '{c.ConteudoProgramatico.Bibliografia.Replace("'", "''")}');");
                }
                foreach (var l in conteudoContext.Aulas.AsNoTracking().ToList())
                {
                    sbConteudo.AppendLine(
                        $"INSERT INTO Aulas (Id, CursoId, Titulo, ConteudoAula, MaterialDeApoio, Duracao) VALUES ('{l.Id}', '{l.CursoId}', '{l.Titulo.Replace("'", "''")}', '{l.ConteudoAula.Replace("'", "''")}', '{l.MaterialDeApoio.Replace("'", "''")}', '{l.Duracao}');");
                }
                await File.WriteAllTextAsync(Path.Combine(outputDir, "seed_conteudo.sql"), sbConteudo.ToString(), Encoding.UTF8);

                var sbFat = new StringBuilder();
                foreach (var p in faturamentoContext.Pagamentos.AsNoTracking().ToList())
                {
                    sbFat.AppendLine(
                        $"INSERT INTO Pagamentos (Id, AlunoId, PreMatriculaId, Valor, DataPagamento, Status, TokenCartao, NumeroCartaoMascarado) VALUES ('{p.Id}', '{p.AlunoId}', '{p.PreMatriculaId}', {p.Valor.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{p.DataPagamento:yyyy-MM-dd HH:mm:ss}', {(int)p.Status}, '{(p.TokenCartao ?? "")}', '{(p.NumeroCartaoMascarado ?? "")}');");
                }
                await File.WriteAllTextAsync(Path.Combine(outputDir, "seed_faturamento.sql"), sbFat.ToString(), Encoding.UTF8);
            }
            catch
            {
                // não falhar a inicialização caso escrita de arquivos não funcione
            }
        }
    }
}
