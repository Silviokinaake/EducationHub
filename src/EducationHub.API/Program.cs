using AutoMapper;
using EducationHub.Alunos.Data;
using EducationHub.Conteudo.Application.Services;
using EducationHub.Conteudo.Data;
using EducationHub.Conteudo.Data.Repository;
using EducationHub.Conteudo.Domain.Interfaces;
using EducationHub.Faturamento.Application.Services;
using EducationHub.Faturamento.Data;
using EducationHub.Faturamento.Data.Repository;
using EducationHub.Faturamento.Domain.Interfaces;
using EducationHub.Faturamento.Domain.Services;
using EducationHub.Faturamento.Domain.Servicos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContexts (EF Core)
builder.Services.AddDbContext<ConteudoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConteudoConnection")));

builder.Services.AddDbContext<FaturamentoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FaturamentoConnection")));

// registrar DbContext Alunos
builder.Services.AddDbContext<AlunoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AlunoConnection"))); // ou outra connection string específica

// AutoMapper - configuração explícita (compatível com AutoMapper 15.x)
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Injeção de dependências - Conteúdo
builder.Services.AddScoped<ICursoRepositorio, CursoRepository>();
builder.Services.AddScoped<ICursoAppService, CursoAppService>();
builder.Services.AddScoped<IAulaRepositorio, AulaRepository>();
builder.Services.AddScoped<IAulaAppService, AulaAppService>();

// Injeção de dependências - Faturamento (Application, Domain e Repository)
builder.Services.AddScoped<IPagamentoAppService, PagamentoAppService>();
builder.Services.AddScoped<IPagamentoServico, PagamentoServico>();
builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepository>();

// registrar repositório e app service
builder.Services.AddScoped<EducationHub.Alunos.Domain.Repositorio.IAlunoRepositorio, EducationHub.Alunos.Data.Repository.AlunoRepository>();
builder.Services.AddScoped<EducationHub.Alunos.Application.Services.IAlunoAppService, EducationHub.Alunos.Application.Services.AlunoAppService>();

// Registrando implementações simuladas de infra para evitar erro de DI
builder.Services.AddScoped<IPagamentoGateway, PagamentoGatewaySimulado>();
builder.Services.AddScoped<ICardTokenizationServico, CardTokenizationService>();
builder.Services.AddScoped<IMatriculaServico, MatriculaServicoSimulado>();

// Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    if (env.IsDevelopment())
    {
        var context = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
        EducationHub.Alunos.Data.AlunoDbInitializer.Seed(context);
    }
}

app.Run();
