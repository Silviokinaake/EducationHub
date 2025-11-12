using AutoMapper;
using EducationHub.Alunos.Data;
using EducationHub.API.Configurations;
using EducationHub.Conteudo.Application.Services;
using EducationHub.Conteudo.Data.Repository;
using EducationHub.Conteudo.Domain.Interfaces;
using EducationHub.Faturamento.Application.Services;
using EducationHub.Faturamento.Data.Repository;
using EducationHub.Faturamento.Domain.Interfaces;
using EducationHub.Faturamento.Domain.Services;
using EducationHub.Faturamento.Domain.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseSelector();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<ICursoRepositorio, CursoRepository>();
builder.Services.AddScoped<ICursoAppService, CursoAppService>();
builder.Services.AddScoped<IAulaRepositorio, AulaRepository>();
builder.Services.AddScoped<IAulaAppService, AulaAppService>();

builder.Services.AddScoped<IPagamentoAppService, PagamentoAppService>();
builder.Services.AddScoped<IPagamentoServico, PagamentoServico>();
builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepository>();

builder.Services.AddScoped<EducationHub.Alunos.Domain.Repositorio.IAlunoRepositorio, EducationHub.Alunos.Data.Repository.AlunoRepository>();
builder.Services.AddScoped<EducationHub.Alunos.Application.Services.IAlunoAppService, EducationHub.Alunos.Application.Services.AlunoAppService>();

builder.Services.AddScoped<IPagamentoGateway, PagamentoGatewaySimulado>();
builder.Services.AddScoped<ICardTokenizationServico, CardTokenizationService>();
builder.Services.AddScoped<IMatriculaServico, MatriculaServicoSimulado>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
    }
}
app.UseDbMigrationHelper();

app.Run();
