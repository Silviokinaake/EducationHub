using EducationHub.Alunos.Data;
using EducationHub.API.Data;
using EducationHub.Conteudo.Data;
using EducationHub.Core.Mediator;
using EducationHub.Faturamento.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EducationHub.Tests.Integration.Fixtures;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Definir ambiente como Test para desabilitar migrations/seed
        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(services =>
        {
            // Remover os DbContexts reais
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(DbContextOptions<AlunoDbContext>));
            services.RemoveAll(typeof(DbContextOptions<ConteudoDbContext>));
            services.RemoveAll(typeof(DbContextOptions<FaturamentoDbContext>));

            // Adicionar DbContexts em memória para testes
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDbIdentity");
            });

            services.AddDbContext<AlunoDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("TestDbAlunos");
                // Obter IMediatorHandler do service provider
                var mediatorHandler = sp.GetRequiredService<IMediatorHandler>();
            });

            services.AddDbContext<ConteudoDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDbConteudo");
            });

            services.AddDbContext<FaturamentoDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("TestDbFaturamento");
                var mediatorHandler = sp.GetRequiredService<IMediatorHandler>();
            });
        });
    }
}
