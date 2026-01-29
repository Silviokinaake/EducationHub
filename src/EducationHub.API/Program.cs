using AutoMapper;
using EducationHub.Alunos.Application.Commands;
using EducationHub.Alunos.Data;
using EducationHub.API.Configurations;
using EducationHub.API.Settings;
using EducationHub.Conteudo.Application.Services;
using EducationHub.Conteudo.Data.Repository;
using EducationHub.Conteudo.Domain.Interfaces;
using EducationHub.Core.Mediator;
using EducationHub.Faturamento.Application.Services;
using EducationHub.Faturamento.Data.Repository;
using EducationHub.Faturamento.Domain.Interfaces;
using EducationHub.Faturamento.Domain.Services;
using EducationHub.Faturamento.Domain.Servicos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseSelector();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityConfiguration(builder.Configuration);

// MediatR e Mediator Handler
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(MatricularAlunoCommand).Assembly); // Alunos.Application
    cfg.RegisterServicesFromAssembly(typeof(EducationHub.Conteudo.Application.Commands.CriarCursoCommand).Assembly); // Conteudo.Application
    cfg.RegisterServicesFromAssembly(typeof(EducationHub.Faturamento.Application.Commands.RealizarPagamentoCommand).Assembly); // Faturamento.Application
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

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
builder.Services.AddScoped<EducationHub.Alunos.Domain.Interfaces.IMatriculaRepository, EducationHub.Alunos.Data.Repository.MatriculaRepository>();
builder.Services.AddScoped<EducationHub.Alunos.Domain.Interfaces.ICertificadoRepository, EducationHub.Alunos.Data.Repository.CertificadoRepository>();
builder.Services.AddScoped<EducationHub.Alunos.Application.Services.IAlunoAppService, EducationHub.Alunos.Application.Services.AlunoAppService>();

builder.Services.AddScoped<IPagamentoGateway, PagamentoGatewaySimulado>();
builder.Services.AddScoped<ICardTokenizationServico, CardTokenizationService>();
builder.Services.AddScoped<IMatriculaServico, EducationHub.Faturamento.Application.Services.MatriculaServico>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new EducationHub.API.Configurations.TimeSpanJsonConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerConfig();

//Pegando o Token de autentica��o
var jwtSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = jwtSettings.ValidoEm,
        ValidIssuer = jwtSettings.Emissor,
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
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

// Não executar migrations/seed durante testes
if (!app.Environment.IsEnvironment("Test"))
{
    app.UseDbMigrationHelper();
}

app.Run();

// Tornar Program acessível para testes de integração
public partial class Program { }
