using EducationHub.Alunos.Data;
using EducationHub.Conteudo.Data;
using EducationHub.Faturamento.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationHub.API.Configurations
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<ConteudoDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("ConteudoConnectionLite")));

                builder.Services.AddDbContext<FaturamentoDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("FaturamentoConnectionLite")));

                builder.Services.AddDbContext<AlunoDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("AlunoConnectionLite")));
            }
            else
            {
                builder.Services.AddDbContext<ConteudoDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("ConteudoConnection")));

                builder.Services.AddDbContext<FaturamentoDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("FaturamentoConnection")));

                builder.Services.AddDbContext<AlunoDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("AlunoConnection")));
            }
        }

    }
}
