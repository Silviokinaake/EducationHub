using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

var builder = WebApplication.CreateBuilder(args);

// DbContexts (EF Core)
builder.Services.AddDbContext<ConteudoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<FaturamentoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FaturamentoConnection")));

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

// Injeção de dependências - Faturamento (Application, Domain e Repository)
builder.Services.AddScoped<IPagamentoAppService, PagamentoAppService>();
builder.Services.AddScoped<IPagamentoServico, PagamentoServico>();
builder.Services.AddScoped<IPagamentoRepositorio, PagamentoRepository>();

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
app.Run();
