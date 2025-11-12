using EducationHub.Alunos.Data;
using EducationHub.Conteudo.Data;
using EducationHub.Faturamento.Data;
using Microsoft.EntityFrameworkCore;
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

            if (env.IsDevelopment())
            {
                await alunoContext.Database.MigrateAsync();
                await conteudoContext.Database.MigrateAsync();
                await faturamentoContext.Database.MigrateAsync();

                await EnsureSeedProducts(alunoContext, conteudoContext, faturamentoContext);
            }
        }

        private static async Task EnsureSeedProducts(AlunoDbContext alunoContext, ConteudoDbContext conteudoContext, FaturamentoDbContext faturamentoContext)
        {
            // Alunos / Matrículas / Certificados
            if (!alunoContext.Alunos.Any())
            {
                var alunoId = Guid.NewGuid();
                var usuarioId = Guid.NewGuid();
                var aluno = new Alunos.Domain.Entidades.Aluno(alunoId, usuarioId, "Aluno Teste", "aluno.teste@example.com", DateTime.UtcNow.AddYears(-25));
                await alunoContext.Alunos.AddAsync(aluno);

                // Criar matrícula pendente
                var cursoId = Guid.NewGuid();
                var matricula = new Alunos.Domain.Entidades.Matricula(cursoId, alunoId, 1500m, DateTime.UtcNow);
                await alunoContext.Matriculas.AddAsync(matricula);

                // Exemplo de certificado (normalmente gerado após conclusão)
                var certificado = new Alunos.Domain.Entidades.Certificado(alunoId, cursoId, "Curso Exemplo", DateTime.UtcNow);
                await alunoContext.Certificados.AddAsync(certificado);

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
                    cp);

                await conteudoContext.Cursos.AddAsync(curso);
                await conteudoContext.SaveChangesAsync();

                // Após salvar curso, criar aulas vinculadas
                var aula1 = new Conteudo.Domain.Entidades.Aula("Boas-vindas", "Apresentação do curso", "Slides", TimeSpan.FromMinutes(30), curso.Id);
                var aula2 = new Conteudo.Domain.Entidades.Aula("Sintaxe básica", "Variáveis, tipos e operadores", "PDF", TimeSpan.FromMinutes(60), curso.Id);

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
                        $"INSERT INTO Cursos (Id, Titulo, Descricao, CargaHoraria, Instrutor, Ativo, Nivel, ConteudoProgramatico_Objetivo, ConteudoProgramatico_Conteudo, ConteudoProgramatico_Metodologia, ConteudoProgramatico_Bibliografia) VALUES ('{c.Id}', '{c.Titulo.Replace("'", "''")}', '{c.Descricao.Replace("'", "''")}', '{c.CargaHoraria}', '{c.Instrutor.Replace("'", "''")}', {(c.Ativo ? 1 : 0)}, '{c.Nivel}', '{c.ConteudoProgramatico.Objetivo.Replace("'", "''")}', '{c.ConteudoProgramatico.Conteudo.Replace("'", "''")}', '{c.ConteudoProgramatico.Metodologia.Replace("'", "''")}', '{c.ConteudoProgramatico.Bibliografia.Replace("'", "''")}');");
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
